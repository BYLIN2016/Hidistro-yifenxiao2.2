var isadd=false;

$(document).ready(function() {
     $.each($(".SKUValueClass"), function() {
        $(this).bind("click", function() { SelectSkus(this); });
    });
    
    $("#buyAmount").bind("keyup", function() { ChangeBuyAmount(); });
    $("#buyButton").bind("click", function() { AddCurrentProductToCart(); });//立即购买
    $("#addcartButton").bind("click", function() { AddProductToCart(); });  //加入购物车
    $("#imgCloseLogin").bind("click", function() { $("#loginForBuy").hide(); });
    $("#btnLoginAndBuy").bind("click", function() { LoginAndBuy(); });
    $("#btnAmoBuy").bind("click", function() { AnonymousBuy(); });
    $("#textfieldusername").keydown(function(e) {
        if (e.keyCode == 13) {
            LoginAndBuy();
        }
    });

    $("#textfieldpassword").keydown(function(e) {
        if (e.keyCode == 13) {
            LoginAndBuy();
        }
    });

});

function SelectSkus(clt) {
    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    $("#skuContent_" + AttributeId).attr("ValueStr", $(clt).attr("value"));
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var selectedOptions = "";
    var skuShows = "";
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function() {
            selectedOptions += $(this).attr("value") + ",";
            skuShows += $(this).attr("ValueStr") + ",";
        });
        selectedOptions = selectedOptions.substring(0, selectedOptions.length - 1);
        skuShows = skuShows.substring(0, skuShows.length - 1);
        $.ajax({
            url: "ShoppingHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetSkuByOptions", productId: $("#hiddenProductId").val(), options: selectedOptions },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.AlertStock, resultData.SalePrice, "已选择：" + skuShows);
                }
                else {
                    ResetCurrentSku("", "", "", "", "", "0", "请选择："); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
                }
            }
        });
    }
}

// 是否所有规格都已选
function IsallSelected() {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname']"), function() {
        if ($(this).attr("value").length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

// 重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId) {
    var pvid = skuSelectId.split("_");
    $.ajax({
        url: "ShoppingHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "UnUpsellingSku", productId: $("#hiddenProductId").val(), AttributeId: pvid[1], ValueId: pvid[2] },
        success: function(resultData) {
            if (resultData.Status == "OK") {
                $.each($("#productSkuSelector dd input,#productSkuSelector dd img"), function(index, item) {
                    var currentPid = $(this).attr("AttributeId");
                    var currentVid = $(this).attr("ValueId");
                    // 不同属性选择绑定事件
                    var isBind = false;
                    $.each($(resultData.SkuItems), function() {
                        if (currentPid == this.AttributeId && currentVid == this.ValueId) {
                            isBind = true;
                        }
                    });
                    if (isBind) {
                        if ($(item).attr("class") == "SKUUNSelectValueClass") {
                            $(item).attr("class", "SKUValueClass");
                        }
                        $(item).attr("disabled",false);
                    }
                    else {
                        $(item).attr("class", "SKUUNSelectValueClass");
                        $(item).attr("disabled",true);
                    }
                });
            }
        }
    });

    $.each($("#" + skuRowId + " dd input,#" + skuRowId + " dd img"), function() {
        $(this).attr({ "class": "SKUValueClass" });
    });

    $("#" + skuSelectId).attr({ "class": "SKUSelectValueClass" });
}

// 重置SKU
function ResetCurrentSku(skuId, sku, weight, stock, alertstock, salePrice, options) {
    $("#showSelectSKU").html(options);
    $("#productDetails_sku").html(sku);
    $("#productDetails_sku_v").val(skuId);
    $("#productDetails_Stock").html(stock);
    $("#productDetails_AlertStock").val(alertstock);
    if(weight != "")
        weight = weight + " g";
    $("#ProductDetails_litWeight").html(weight); 
    $("#SubmitOrder_Weight").html(weight);
            
    $("#ProductDetails_lblBuyPrice").html(salePrice);

    if (ValidateBuyAmount() && document.URL.indexOf("groupbuyproduct_detail") == -1 && document.URL.indexOf("countdownproduct_detail") == -1) {
        var quantity = parseInt($("#buyAmount").val());
        var totalPrice = eval(salePrice) * quantity;
        $("#productDetails_Total").html(totalPrice.toFixed(2));
        $("#productDetails_Total_v").val(totalPrice);
   }
}

// 购买数量变化以后的处理
function ChangeBuyAmount() {
    if (ValidateBuyAmount()) {
        var quantity = parseInt($("#buyAmount").val());
        var oldQuantiy = parseInt($("#oldBuyNumHidden").val());
        var productTotal = eval($("#productDetails_Total").html()); 
        var totalPrice = productTotal / oldQuantiy * quantity;
        
        $("#productDetails_Total").html(totalPrice.toFixed(2));
        $("#oldBuyNumHidden").attr("value", quantity);
    }
}

// 购买按钮单击事件
function AddCurrentProductToCart() {
    isadd=false;
    if (!ValidateBuyAmount()) {
        return false;
    }

    if (!IsallSelected()) {
        alert("请选择规格");
        return false;
    }
    var quantity = parseInt($("#buyAmount").val());
    var maxcount = parseInt($("#maxcount").html());
    var stock = parseInt(document.getElementById("productDetails_Stock").innerHTML);
    var alertstock=parseInt($("#productDetails_AlertStock").val());
    if (quantity > stock) {
        alert("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }
    if (maxcount != ""&&maxcount!=null) {
        if (quantity > maxcount) {
            alert("此为限购商品，每人限购" + maxcount + "件");
            return false;
        }
    }
    if(subsiteuserId != "0" && alertstock >= stock){
            alert("商品库存低于警戒库存，无法购买！");
            return false;
    }
    

    if ($("#hiddenIsLogin").val() == "nologin") {
        $("#loginForBuy").show();
        return false;
    }
    
    BuyProduct();
}

// 登录后再购买
function LoginAndBuy() {
    var username = $("#textfieldusername").val();
    var password = $("#textfieldpassword").val();
    var thisURL = document.URL; 

    if (username.length == 0 || password.length == 0) {
        alert("请输入您的用户名和密码!");
        return;
    }

    $.ajax({
        url: "Login.aspx",
        type: "post",
        dataType: "json",
        timeout: 10000,
        data: { username: username, password: password, action: "Common_UserLogin" },
        async: false,
        success: function(data) {
            if (data.Status == "Succes") {
                if(isadd){
                    $("#loginForBuy").hide('hide');
                    $("#hiddenIsLogin").val('logined');
                    BuyProductToCart();//调用添加到购物车
                }else{
                    BuyProduct();
                    window.location.reload();
                }
                
            }
            else {
                alert(data.Msg);
            }
        }
    });    
}

// 购买商品
function BuyProduct() {
    var thisURL = document.URL; 
    if($("#productDetails_sku_v").val().replace(/\s/g,"")==""){
            alert("此商品已经不存在(可能库存不足或被删除或被下架)，暂时不能购买");
            return false;
    }
    if(thisURL.indexOf("groupbuyproduct_detail") != -1)
    {
        location.href = applicationPath + "/SubmmitOrder.aspx?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val()+"&from=groupBuy";
    }
    else if(thisURL.indexOf("countdownproduct_detail") != -1)
    {
        location.href = applicationPath + "/SubmmitOrder.aspx?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=countDown";
    }
    else
    {       
       if($("#buyAmount").val().replace(/\s/g,"")==""||parseInt($("#buyAmount").val().replace(/\s/g,""))<=0){
           alert("商品库存不足 " + parseInt($("#buyAmount").val()) + " 件，请修改购买数量!");
           return false;
       }
       
       location.href = applicationPath + "/SubmmitOrder.aspx?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val()+"&from=signBuy";
    }
}

// 验证数量输入
function ValidateBuyAmount() {
    var buyAmount = $("#buyAmount");
    if ($(buyAmount).val().length == 0) {
        alert("请先填写购买数量!");
        return false;
    }
    if ($(buyAmount).val() == "0" || $(buyAmount).val().length > 5) {
        alert("填写的购买数量必须大于0小于99999!");
        var str = $(buyAmount).val();
        $(buyAmount).val(str.substring(0, 5));
        return false;
    }
    var amountReg = /^[1-9]d*|0$/;
    if (!amountReg.test($(buyAmount).val())) {
        alert("请填写正确的购买数量!");
        return false;
    }

    return true;
}
//*************匿名购买**********************************//
function AnonymousBuy() {
    if (isadd) {
        BuyProductToCart();
    }
    else {
        BuyProduct();
    }
    $("#loginForBuy").hide();
}

//*************2011-07-25  添加到购物车按钮单击事件****************//
function AddProductToCart() {
    if (!ValidateBuyAmount()) {
        return false;
    }

    if (!IsallSelected()) {
        alert("请选择规格");
        return false;
    }

    var quantity = parseInt($("#buyAmount").val());
    var stock = parseInt(document.getElementById("productDetails_Stock").innerHTML);
    var alertstock=parseInt($("#productDetails_AlertStock").val());
    if (quantity > stock) {
        alert("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }
    if(subsiteuserId != "0" && alertstock >= stock){
            alert("商品库存低于警戒库存，无法购买！");
            return false;
    }
    var spcounttype = parseInt($("#spcounttype").text());
    if (quantity + spcounttype > stock) {
        alert("商品库存不足!");
        return false;
    }
    BuyProductToCart();//添加到购物车
}

function BuyProductToCart(){
   var xtarget=$("#addcartButton").offset().left;
   var ytarget=$("#addcartButton").offset().top;
   $("#divshow,#divbefore").css("top",parseInt(ytarget+40)+"px");
   
   $("#divshow,#divbefore").css("left",parseInt(xtarget)+"px");
   if($(document).scrollTop()<=145){
        $("#divshow,#divbefore").css("top",parseInt(ytarget-125)+"px");
   }
   $(".dialog_title_r,.btn-continue").bind("click",function (){$("#divshow").css('display','none')});
   $(".btn-viewcart").attr("href",applicationPath+"/ShoppingCart.aspx");
   $.ajax({
        url: "ShoppingHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyAmount").val()), productSkuId: $("#productDetails_sku_v").val() },
        async: false,
        beforeSend:function(){
            $("#divbefore").css('display','block');
        },
        complete:function(){
            // setTimeout("if($('#divshow').css('display')=='block'){$('#divshow').css('display','none')}",6000);
             //$("#divshow").blur(function(){alert('aaaa')});
        },
        success: function(resultData) {
            if(resultData.Status=="OK"){
                $("#divbefore").css('display','none');
                $("#divshow").css('display','block');//显示添加购物成功
                $("#spcounttype").text(resultData.Quantity);
                $("#sptotal").text(resultData.TotalMoney);
                
                $("#spcartNum").text(resultData.Quantity);
                $("#ProductDetails_ctl03___cartMoney").text(resultData.TotalMoney);
            }else if (resultData.Status == "0") {
                // 商品已经下架
                 $("#divbefore").css('display','none');
                alert("此商品已经不存在(可能被删除或被下架)，暂时不能购买");
            }
            else if (resultData.Status == "1") {
                // 商品库存不足
                $("#divbefore").css('display','none');
                alert("商品库存不足 " + parseInt($("#buyAmount").val()) + " 件，请修改购买数量!");
            }
            else {
                // 抛出异常消息
                $("#divbefore").css('display','none');
                alert(resultData.Status+'66');
            }
        }
   });
}

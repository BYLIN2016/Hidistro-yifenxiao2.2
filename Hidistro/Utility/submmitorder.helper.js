$(document).ready(function () {
    // 如果默认选中了一个收货地址
    if ($("#addresslist .list").length > 0) {
        $("#addresslist .list").click(function () {
            $(this).addClass("select").siblings().removeClass("select");
            if ($(this).find("input[type='hidden']") != null && $(this).find("input[type='hidden']") != "" && $(this).find("input[type='hidden']") != "undefined") {
                ResetAddress($(this).find("input[type='hidden']").eq(0).val());
            }
        });
        $("#user_shippingaddress").css("display", "none");
        var firstshippId = $("#addresslist .list:first").addClass("select").find("input[type='hidden']").eq(0).val();
        if (firstshippId != "" && firstshippId != "undefined" && parseInt(firstshippId) > 0) {
            ResetAddress(firstshippId);
        }
    } else {
        $("#btnaddr").hide();
    }

    // 收获地址列表选择触发事件
    $("input[type='radio'][name='SubmmitOrder$Common_ShippingAddressesRadioButtonList']").bind('click', function () {
        var shippingId = $(this).attr("value");
        ResetAddress(shippingId);
    })

    //3级收货地址选择触发事件
    $("#ddlRegions1").bind("change", function () {
        CalculateFreight($("#ddlRegions1").val());
    })

    // 配送方式选择触发事件
    $("input[name='shippButton'][type='radio']").bind('click', function () {
        var regionId = $("#regionSelectorValue").val();
        var shippmodeId = $(this).attr("value");
        $("#SubmmitOrder_inputShippingModeId").val(shippmodeId);
        CalculateFreight(regionId);
    })

    // 支付方式选择触发事件
    $("input[name='paymentMode'][type='radio']").bind('click', function () {
        $('#SubmmitOrder_inputPaymentModeId').val($(this).val());

        CalculateTotalPrice();
    })

    $("#SubmmitOrder_chkTax").click(function () {
        if ($("#SubmmitOrder_chkTax").prop("checked") == true) {
            var tax = eval($("#SubmmitOrder_lblTotalPrice").html()) * eval($("#SubmmitOrder_litTaxRate").html()) / 100;
            $("#SubmmitOrder_lblTax").html(tax.toFixed(2));
        }
        else {
            $("#SubmmitOrder_lblTax").html("0.00");
        }

        CalculateTotalPrice();
    });
    // 使用优惠券
    $("#btnCoupon").click(function () {
        if (location.href.indexOf("groupBuy") > 0 || location.href.indexOf("countDown") > 0) {
            alert("团购或限时抢购不能使用优惠券")
            return false;
        }
        var couponCode = $("#SubmmitOrder_CmbCoupCode").combobox("getValue");
        var cartTotal = $("#SubmmitOrder_lblTotalPrice").html();
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: "ProcessorUseCoupon", CartTotal: cartTotal, CouponCode: couponCode },
            cache: false,
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    $("#SubmitOrder_CouponName").html(resultData.CouponName);
                    $("#SubmmitOrder_litCouponAmout").html("-" + resultData.DiscountValue);
                    $("#SubmmitOrder_htmlCouponCode").val(couponCode);
                    CalculateTotalPrice();
                }
                else {
                    alert("您的优惠券编号无效(可能不在有效期范围内), 或者您的商品金额不够");
                }
            }
        });
    })

    // 创建订单
    $("#SubmmitOrder_btnCreateOrder").click(function () {

        var str = $("#SubmmitOrder_txtShipTo").val();
        var reg = new RegExp("[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*");
        if (str == "" || !reg.test(str)) {
            alert("请输入正确的收货人姓名");
            return false;
        }

        if ($("#SubmmitOrder_txtAddress").val() == "") {
            alert("请输入收货人详细地址");
            return false;
        }
        if ($("#SubmmitOrder_txtTelPhone").val() == "" && $("#SubmmitOrder_txtCellPhone").val() == "") {
            alert("请输入电话号码或手机号码");
            return false;
        }
        // 验证配送地区选择了没有
        var selectedRegionId = GetSelectedRegionId();
        if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
            alert("请选择您的收货人地址");
            return false;
        }

        if (!PageIsValid()) {
            alert("部分信息没有通过检验，请查看页面提示");
            return false;
        }

        if ($("#SubmmitOrder_inputShippingModeId").val() == "") {
            alert("请选择配送方式");
            return false;
        }
        if ($("#SubmmitOrder_inputPaymentModeId").val() == "") {
            alert("请选择支付方式");
            return false;
        }

    });

    $("#btnaddaddress").click(function () {

        $("#tab_address").show();
        $("#user_shippingaddress").attr("class", "cart_Order_address2").show();

    });
    $("#imgCloseLogin").click(function () {
        $("#user_shippingaddress").hide();
        $("#tab_address").hide();
        $("#user_shippingaddress").attr("class", "cart_Order_address");
    });



    $("#a_salemode").toggle(function () {
        $("#tab_pasteaddress").css("display", "block");
        $("#user_shippingaddress").show();

        $(this).text("切换到普通模式");


        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#addresslist").hide();
            $("#addresslist .list").removeClass("select");
        }
        $("#btnaddr").hide();

        ClearAddress();

    }, function () {
        $("#user_shippingaddress").attr("class", "cart_Order_address");
        $("#tab_pasteaddress").css("display", "none");
        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#user_shippingaddress").hide();
        }


        $(this).text("切换到代销模式");

        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#addresslist").show();
            $("#addresslist .list").removeClass("select");

        }
    });

});


function ClearAddress() {
    $("#SubmmitOrder_txtShipTo").val('');
    $("#SubmmitOrder_txtAddress").val('');
    $("#SubmmitOrder_txtZipcode").val('');
    $("#SubmmitOrder_txtCellPhone").val('');
    $("#SubmmitOrder_txtTelPhone").val('');
    $("#ddlRegions1").val("");
    $("#ddlRegions2").val(null);
    $("#ddlRegions3").val(null);

}


function AddShippAddress() {
    if (!PageIsValid()) {
        alert("部分信息没有通过检验，请查看页面提示");
        return false;
    }
    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: "AddShippingAddress", ShippingTo: $("#SubmmitOrder_txtShipTo").val().replace(/\s/g, ""),
            RegionId: $("#regionSelectorValue").val(),
            AddressDetails: $("#SubmmitOrder_txtAddress").val().replace(/\s/g, ""),
            ZipCode: $("#SubmmitOrder_txtZipcode").val().replace(/\s/g, ""),
            TelPhone: $("#SubmmitOrder_txtTelPhone").val().replace(/\s/g, ""),
            CellHphone: $("#SubmmitOrder_txtCellPhone").val().replace(/\s/g, "")
        },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                var divlist = "<div class=\"list select\">";
                divlist += "<div class=\"inner\">";
                divlist += "<div class=\"addr-hd\">";
                divlist += resultData.Result.RegionId + "（<span class=\"name\">" + resultData.Result.ShipTo + "</span><span>收）</span>";
                divlist += "</div>";
                divlist += "<div class=\"addr-bd\" title=\"" + resultData.Result.ShippingAddress + "\">";
                divlist += "<span class=\"street\">" + resultData.Result.ShippingAddress + "</span><span class=\"phone\">" + resultData.Result.CellPhone + "</span>";
                divlist += "<span class=\"last\">&nbsp;</span>";
                divlist += "</div>";
                divlist += "</div>";
                divlist += "<em class=\"curmarker\"></em><input type=\"hidden\" value=\"" + resultData.Result.ShippingId + "\"/>";
                divlist += "</div>";
                $(".list").removeClass("select");
                $(".list:last").after(divlist);
                $("#user_shippingaddress").hide();
                ReSetAddress(resultData.Result.ShippingId);

            }
            else {
                alert(resultData.Result);
            }
        }
    });
}

// 重置收货地址
function ResetAddress(shippingId) {
    var ConsigneeName = $("#SubmmitOrder_txtShipTo");
    var ConsigneeAddress = $("#SubmmitOrder_txtAddress");
    var ConsigneePostCode = $("#SubmmitOrder_txtZipcode");
    var ConsigneeTel = $("#SubmmitOrder_txtTelPhone");
    var ConsigneeHandSet = $("#SubmmitOrder_txtCellPhone");

    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: "GetUserShippingAddress", ShippingId: shippingId },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                $(ConsigneeName).val(resultData.ShipTo);
                ConsigneeName.focus();

                $(ConsigneeAddress).val(resultData.Address);
                ConsigneeAddress.focus();

                $(ConsigneePostCode).val(resultData.Zipcode);
                ConsigneePostCode.focus();

                $(ConsigneeTel).val(resultData.TelPhone);
                ConsigneeTel.focus();

                $(ConsigneeHandSet).val(resultData.CellPhone);
                ConsigneeHandSet.focus();

                ResetSelectedRegion(resultData.RegionId);
                CalculateFreight(resultData.RegionId);
            }
            else {
                alert("收货地址选择出错，请重试!");
            }
        }
    });
}
// 总金额
function CalculateTotalPrice() {    
    var cartTotalPrice = $("#SubmmitOrder_lblTotalPrice").html();
    var shippmodePrice = $("#SubmmitOrder_lblShippModePrice").html();
    var couponPrice = $("#SubmmitOrder_litCouponAmout").html();
    var tax = $("#SubmmitOrder_lblTax").html();
    
    var total = eval(cartTotalPrice) + eval(couponPrice) + eval(tax);
    
    if ($("#SubmmitOrder_hlkFeeFreight").html() == "")
        total = total + eval(shippmodePrice);
    // 计算支付手续费
    var paymentModeId = $('#SubmmitOrder_inputPaymentModeId').val();
    if (paymentModeId == "")
        $("#SubmmitOrder_lblOrderTotal").html(total.toFixed(2));
    else {
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: 'ProcessorPaymentMode', ModeId: paymentModeId, TotalPrice: total },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    $("#SubmmitOrder_lblPaymentPrice").html(resultData.Charge);
                    var paymentPrice = eval(resultData.Charge)
                    if (paymentPrice > 0) {
                        $("#divPaymentPrice").show();
                        total = total + paymentPrice;
                    }
                    else {
                        $("#divPaymentPrice").hide();
                    }
                    $("#SubmmitOrder_lblOrderTotal").html(total.toFixed(2));
                }
            }
        });
    }
}

// 重新计算运费
function CalculateFreight(regionId) {
    var weight = $("#SubmmitOrder_litAllWeight").html();
    var shippingModeId = $("#SubmmitOrder_inputShippingModeId").val();
    //alert(shippingModeId+"____"+weight+"======="+regionId);
    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: 'CalculateFreight', ModeId: shippingModeId, Weight: weight, RegionId: regionId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                if ($("#SubmmitOrder_hlkFeeFreight").html() == "") {
                    $("#SubmmitOrder_lblShippModePrice").html(resultData.Price);
                }
                else {
                    $("#SubmmitOrder_lblShippModePrice").html("0.00");
                }
                CalculateTotalPrice();
            }
        }
    });
}
 
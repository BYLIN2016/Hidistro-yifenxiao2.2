function checkAll() {
    var isCheck = $("#chkCheckAll").attr("checked");

    $("input[type='checkbox'][name='chkSerachResult']").attr("checked", isCheck);
}
function ShowAddDiv() {
    DialogFrameClose("promotion/SearchBindProduct.aspx", "添加捆绑商品", null, null);
}

function nextPage() {
    var index = $("#currentPage").val()
    index = Number(index) + 1;
    if (index <= 0)
        return;
    $("#currentPage").val(index);
    GetSearchData(index);
}

function prevPage() {
    var index = $("#currentPage").val()
    index -= 1;
    if (index <= 0) {
        return;
    }
    $("#currentPage").val(index);
    GetSearchData(index);
}

function search() {
    $("#currentPage").val(1);
    GetSearchData(1);
}
function Remove(jq) {
    $(jq).parent().parent().remove();
    GetTotalPrice();
}

function CollectInfos() {
    var inputstr = '';
    var flag = true;
    var trs = $("tr[name='appendlist']");
    if (trs.length < 2) {
        alert("捆绑商品至少需选择二件商品");
        flag = false;
    }
    else {
        trs.each(function(i, item) {
            var num = $(item).children().eq(3).find("input").val();
            if (num == "" || num == 'undefined') {
                alert("绑定商品数量只能为正整数!");
                flag = false;
                $(item).children().eq(3).find("input").focus();
                return false;
            }
            if (isNaN(num)||parseInt(num)<=0) {
                alert("绑定商品数量只能为正整数!");
                flag = false;
                $(item).children().eq(3).find("input").focus();
                return false;
            }

            inputstr += $(item).children().eq(4).html() + "|" + num + ",";
        });
        inputstr = inputstr.substr(0, inputstr.length - 1);
        $("#selectProductsinfo").val(inputstr);
    }
    return flag;
}


//function selectProduct() {
//    var chks = $("input[name='chkSerachResult']:checked");
//    var tr = '';
//    var total = 0;
//    var arr = new Array();
//    $(chks).each(function(i, item) {
//        arr = $(item).val().split('|');
//        if ($("#addlist span[id='" + arr[1] + "']").length == 0)
//            tr += String.format("<tr name='appendlist'><td>{0}</td><td >{2}</td><td>{3}</td><td><input type='text' value='1' name='txtNum'/></td><td style='display:none'>{4}</td><td ><span  id='{1}' style='cursor:pointer;color:blue' onclick='Remove(this)'>删除</span></td></tr>", arr[0], arr[1], arr[2], arr[3], arr[4] +"|" + arr[1]);
//    });
//    $("#addlist").append(tr);
//    GetTotalPrice();
// 
//}

function GetTotalPrice() {

    var total = 0;
    var inputNum = $("#addlist input");
    if (inputNum.length > 0) {
        inputNum.each(function(i, item) {
            var tdprice = parseFloat($(item).parent().prev().html()) * parseInt($(item).val());
            total += tdprice;
        }
         );
        $("#addlist input").bind("blur", function() { GetTotalPrice() });
    }
    $("#totalprice").html(total);
}

function GetSearchData(pageindex) {
    $.ajax({
        url: "AddBundlingProduct.aspx?isCallback=true&serachName=" + $("#serachName").val() + "&categoryId=" + $("#ctl00_contentHolder_dropCategories").val() +
                 "&brandId=" + $("#ctl00_contentHolder_dropBrandList").val() + "&page=" + pageindex + "&date=" + new Date(),
        type: 'GET', dataType: 'json',
        success: function(json) {
            var currentPage = $("#currentPage").val();
            var pageCount = Math.ceil(json.recCount / 5);
            if (currentPage >= pageCount)
                $("#nextLink").css("display", "none");
            else
                $("#nextLink").css("display", "");
            if (currentPage <= 1)
                $("#prevLink").css("display", "none");
            else
                $("#prevLink").css("display", "");
            $("#sp_pagetotal").text(pageCount);
            $("#sp_pageindex").text(pageindex);
            $("#serachResult").empty();
            $("#serachResult").append("<tr class='table_title'><th  scope='col'><input id='chkCheckAll' onclick='checkAll()' type='checkbox' />商品名称</th><th scope='col'>操作</th></tr>")
            $.each(json.data, function(i, item) {
                if (item != undefined && item.sku != "") {
                    var str = String.format("<tr><td  style='width:40%;text-align:left; border-bottom:solid 1px #ccc'>{0}</td>", item.Name);
                    str += String.format("<td style='width:60%;border-bottom:solid 1px #ccc'><table style='width:100%; text-align:left'>");
                    $.each(item.skus, function(j, sku) {
                        str += String.format("<tr><td align='left'> <input type='checkbox'  name='chkSerachResult' value='{0}|{1}|{2}|{3}|{5}' /></td><td style='width:50%'>{2}</td><td style='width:25%'>售价:{3}</td><td style='width:25%'>库存:{4}</td></tr>", item.Name, sku.skuid, sku.skucontent, sku.saleprice, sku.stock, item.ProductId);
                    });
                    str += String.format("</table></td></tr>");
                    $("#serachResult").append(str);
                }
            });
        }
    });
}



function CloseFrameWindow() {
    GetTotalPrice();
}
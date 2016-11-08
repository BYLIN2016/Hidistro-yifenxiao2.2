function CloseDiv(id) {
    var div = document.getElementById(id);
    div.style.display = "none";

    mybg.style.display = "none";
    document.body.style.overflow = "";
}
//确认退款
function CheckRefund(orderId) {
    $("#ctl00_contentHolder_lblOrderId").html(orderId);
    $("#ctl00_contentHolder_hidOrderId").val(orderId);
    $.ajax({
        url: "ManageMyOrder.aspx?type=refund&orderId=" + orderId,
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            isCallback: "true"
        },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "1") {
                $("#ctl00_contentHolder_lblOrderTotal").html(resultData.OrderTotal);
                $("#ctl00_contentHolder_lblRefundType").html(resultData.RefundTypeStr);
                $("#ctl00_contentHolder_hidRefundType").val(resultData.RefundType);
                $("#ctl00_contentHolder_lblContacts").html(resultData.Contacts);
                $("#ctl00_contentHolder_lblEmail").html(resultData.Email);
                $("#ctl00_contentHolder_lblTelephone").html(resultData.Telephone);
                $("#ctl00_contentHolder_lblAddress").html(resultData.Address);
                $("#ctl00_contentHolder_lblRefundRemark").html(resultData.Remark);
                DivWindowOpen(600, 480, "CheckRefund");
            }
        }

    });
}
//确认退货
function CheckReturn(orderId) {
    $("#ctl00_contentHolder_return_lblOrderId").html(orderId);
    $("#ctl00_contentHolder_hidOrderId").val(orderId);
    $.ajax({
        url: "ManageMyOrder.aspx?type=return&orderId=" + orderId,
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            isCallback: "true"
        },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "1") {
                $("#ctl00_contentHolder_hidOrderTotal").val(resultData.OrderTotal);
                $("#ctl00_contentHolder_return_lblOrderTotal").html(resultData.OrderTotal);
                $("#ctl00_contentHolder_return_lblRefundType").html(resultData.RefundTypeStr);
                $("#ctl00_contentHolder_hidRefundType").val(resultData.RefundType);
                $("#ctl00_contentHolder_return_lblContacts").html(resultData.Contacts);
                $("#ctl00_contentHolder_return_lblEmail").html(resultData.Email);
                $("#ctl00_contentHolder_return_lblTelephone").html(resultData.Telephone);
                $("#ctl00_contentHolder_return_lblAddress").html(resultData.Address);
                $("#ctl00_contentHolder_return_lblReturnRemark").html(resultData.Remark);
                DivWindowOpen(600, 480, "CheckReturn");
            }
        }

    });
}
//确认换货
function CheckReplace(orderId) {
    $("#ctl00_contentHolder_replace_lblOrderId").html(orderId);
    $("#ctl00_contentHolder_hidOrderId").val(orderId);
    $.ajax({
        url: "ManageMyOrder.aspx?type=replace&orderId=" + orderId,
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            isCallback: "true"
        },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "1") {
                $("#ctl00_contentHolder_hidOrderTotal").val(resultData.OrderTotal);
                $("#ctl00_contentHolder_replace_lblOrderTotal").html(resultData.OrderTotal);
                $("#ctl00_contentHolder_replace_lblComments").html(resultData.Comments);
                $("#ctl00_contentHolder_replace_lblContacts").html(resultData.Contacts);
                $("#ctl00_contentHolder_replace_lblEmail").html(resultData.Email);
                $("#ctl00_contentHolder_replace_lblTelephone").html(resultData.Telephone);
                $("#ctl00_contentHolder_replace_lblAddress").html(resultData.Address);
                $("#ctl00_contentHolder_replace_lblPostCode").html(resultData.PostCode);
                DivWindowOpen(600, 500, "CheckReplace");
            }
        }

    });
}

function SetBackGround() {
    mybg = document.createElement("div");
    mybg.setAttribute("id", "mybg");
    mybg.style.background = "#000";
    mybg.style.width = "100%";
    mybg.style.height = "100%";
    mybg.style.position = "absolute";
    mybg.style.top = "0";
    mybg.style.left = "0";
    mybg.style.zIndex = "500";
    mybg.style.opacity = "0.3";
    mybg.style.filter = "Alpha(opacity=30)";
    document.body.appendChild(mybg);

    document.body.style.height = window.screen.height + "px";
    document.body.style.position = "relative";
    document.body.style.overflow = "hidden";
}
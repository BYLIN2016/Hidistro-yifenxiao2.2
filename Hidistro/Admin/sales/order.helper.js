//确认退款
function CheckRefund(orderId) {
    $("#ctl00_contentHolder_lblOrderId").html(orderId);
    $("#ctl00_contentHolder_hidOrderId").val(orderId);
    setArryText('ctl00_contentHolder_txtAdminRemark', '');
    $.ajax({
        url: "ManageOrder.aspx?type=refund&orderId=" + orderId,
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
                $("#ctl00_contentHolder_lblRefundRemark").html((resultData.Remark));
                $("#ctl00_contentHolder_lblEmail").html(resultData.Email);
                $("#ctl00_contentHolder_lblTelephone").html(resultData.Telephone);
                $("#ctl00_contentHolder_lblAddress").html(resultData.Address);
                ShowMessageDialog("确认退款", "checkrefund", "CheckRefund");
            }
        }

    });
}
//确认退货
function CheckReturn(orderId) {
    arrytext = null;
    $("#ctl00_contentHolder_return_lblOrderId").html(orderId);
    $("#ctl00_contentHolder_hidOrderId").val(orderId);
    setArryText('ctl00_contentHolder_return_txtRefundMoney', '');
    setArryText('ctl00_contentHolder_return_txtAdminRemark', '');
    $.ajax({
        url: "ManageOrder.aspx?type=return&orderId=" + orderId,
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            isCallback: "true"
        },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "1") {
                $("#ctl00_contentHolder_return_lblOrderTotal").html(resultData.OrderTotal);
                $("#ctl00_contentHolder_return_lblRefundType").html(resultData.RefundTypeStr);
                $("#ctl00_contentHolder_hidRefundType").val(resultData.RefundType);
                $("#ctl00_contentHolder_return_lblContacts").html(resultData.Contacts);
                $("#ctl00_contentHolder_return_lblReturnRemark").html(resultData.Remark);
                $("#ctl00_contentHolder_return_lblEmail").html(resultData.Email);
                $("#ctl00_contentHolder_return_lblTelephone").html(resultData.Telephone);
                $("#ctl00_contentHolder_return_lblAddress").html(resultData.Address);
                ShowMessageDialog("确认退货", "checkreturn", "CheckReturn");
            }
        }

    });
}
//确认换货
function CheckReplace(orderId) {
    arrytext = null;
    $("#ctl00_contentHolder_replace_lblOrderId").html(orderId);
    $("#ctl00_contentHolder_hidOrderId").val(orderId);
    setArryText('ctl00_contentHolder_replace_txtAdminRemark', '');
    $.ajax({
        url: "ManageOrder.aspx?type=replace&orderId=" + orderId,
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            isCallback: "true"
        },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "1") {
                $("#ctl00_contentHolder_replace_lblOrderTotal").html(resultData.OrderTotal);
                $("#ctl00_contentHolder_replace_lblComments").html(resultData.Comments);
                $("#ctl00_contentHolder_replace_lblContacts").html(resultData.Contacts);
                $("#ctl00_contentHolder_replace_lblEmail").html(resultData.Email);
                $("#ctl00_contentHolder_replace_lblTelephone").html(resultData.Telephone);
                $("#ctl00_contentHolder_replace_lblAddress").html(resultData.Address);
                $("#ctl00_contentHolder_replace_lblPostCode").html(resultData.PostCode);
                ShowMessageDialog("确认换货", "checkreplace", "CheckReplace");
            }
        }

    });
}
function acceptRefund() {
    var adminRemark = $("#ctl00_contentHolder_txtAdminRemark").val();
    $("#ctl00_contentHolder_hidAdminRemark").val(adminRemark);
    $("#ctl00_contentHolder_btnAcceptRefund").trigger("click");
}
function refuseRefund() {
    var adminRemark = $("#ctl00_contentHolder_txtAdminRemark").val();
    $("#ctl00_contentHolder_hidAdminRemark").val(adminRemark);
    $("#ctl00_contentHolder_btnRefuseRefund").trigger("click");
}
function acceptReplace() {
    var adminRemark = $("#ctl00_contentHolder_replace_txtAdminRemark").val();
    $("#ctl00_contentHolder_hidAdminRemark").val(adminRemark);
    $("#ctl00_contentHolder_btnAcceptReplace").trigger("click");
}
function refuseReplace() {
    var adminRemark = $("#ctl00_contentHolder_replace_txtAdminRemark").val();
    $("#ctl00_contentHolder_hidAdminRemark").val(adminRemark);
    $("#ctl00_contentHolder_btnRefuseReplace").trigger("click");
}
function acceptReturn() {
    var refundMoney = $("#ctl00_contentHolder_return_txtRefundMoney").val();
    $("#ctl00_contentHolder_hidRefundMoney").val(refundMoney);
    var adminRemark = $("#ctl00_contentHolder_return_txtAdminRemark").val();
    $("#ctl00_contentHolder_hidAdminRemark").val(adminRemark);
    var orderTotal = $("#ctl00_contentHolder_return_lblOrderTotal").html();
    $("#ctl00_contentHolder_hidOrderTotal").val(orderTotal);
    $("#ctl00_contentHolder_btnAcceptReturn").trigger("click");
}
function refuseReturn() {
    var adminRemark = $("#ctl00_contentHolder_return_txtAdminRemark").val();
    $("#ctl00_contentHolder_hidAdminRemark").val(adminRemark);
    $("#ctl00_contentHolder_btnRefuseReturn").trigger("click");
}
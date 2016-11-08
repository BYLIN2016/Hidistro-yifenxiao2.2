function ResetGroupBuyProducts() {
    var categoryId = $("#ctl00_contentHolder_dropCategories").val();
    var sku = $("#ctl00_contentHolder_txtSKU").val();
    var productName = $("#ctl00_contentHolder_txtSearchText").val();
    var postUrl = "addmygroupbuy.aspx?isCallback=true&action=getGroupBuyProducts&timestamp=";
    postUrl += new Date().getTime() + "&categoryId=" + categoryId + "&sku=" + encodeURI(sku) + "&productName=" + encodeURI(productName);

    document.getElementById("ctl00_contentHolder_dropGroupBuyProduct").options.length = 0;
    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'json', timeout: 10000,
        async: false,
        success: function(resultData) {
            if (resultData.Status == "0") {
            }
            else if (resultData.Status == "OK") {
                FillProducts(resultData.Product);
            }
        }
    });
}

function FillProducts(product) {
    var productSelector = $("#ctl00_contentHolder_dropGroupBuyProduct");
    productSelector.append("<option selected=\"selected\" value=\"0\">--\u8BF7\u9009\u62E9--<\/option>");

    $.each(product, function(i, product) {
        productSelector.append(String.format("<option value=\"{0}\">{1}<\/option>", product.ProductId, product.ProductName));
    });
}
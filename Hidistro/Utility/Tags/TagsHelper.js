//  选项卡片商品切换效果
function changeTab(thisObj, tabId, itmeNum) {
    $.each($("#" + tabId + " li"), function (index, item) {
        $(item).attr("class", "");
    });

    $.each($("#" + tabId + " div"), function (index, item) {
        if ($(item).attr("class") == "tab_item")
            $(item).css('display', 'none');
    });

    $(thisObj).attr("class", "select");
    $("#" + tabId + itmeNum).css('display', 'block');
}
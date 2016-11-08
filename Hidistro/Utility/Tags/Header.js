// 头页查询事件
function searchs() {
    var item = $("#drop_Search_Class").val();
    var key = $("#txt_Search_Keywords").val();

    if (item == undefined)
        item = "";

    if (key == undefined)
        key = "";

    var url = applicationPath + "/SubCategory.aspx?keywords=" + key + "&categoryId=" + item;
    window.location.href = encodeURI(url);
}

$(document).ready(function () {
    // 回车自动搜索
    $('#txt_Search_Keywords').keydown(function (e) {
        if (e.keyCode == 13) {
            searchs();
            return false;
        }
    })
    // 当前页头菜单样式区别
    var pathurl = location.href.substr(location.href.lastIndexOf('/'), location.href.length - location.href.lastIndexOf('/'));
    $("#ty_menu_title li a[href$='" + pathurl + "']").addClass("ty_menu_select");
});
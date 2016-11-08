var groupdiv = "#grounddiv"; //阴影层
var assitsdiv = "#assistdiv"; //阴影边框
var groudedite = "#groundeidtdiv"; //编辑层
var a_edite = "#a_design_Edit"; //编辑链接
var a_down = "#a_design_down"; //向下移动
var a_up = "#a_design_up"; //向上移动
var a_delete = "#a_design_delete"; //删除按钮
var ctrnamediv = "#ctrnamediv"; //控件类型层
var prefix = "_"; //元素id前缀
var tempdiv="#tempdiv";
var taboverlaycontent = "#taboverlaycontent"; //加载临时控件的html
var desigurl = "Handler/DesigHandler.ashx"; //处理类

var Hidistro_designer = {
    ControlDic: new jQuery.Hashtable(), //所有控件集合
    CurrentContentID: "", //当前选中的层ID
    CurrentcontrolType: "", //当前选中层的控件类型
    CurrentPageName: "", //当前页
    MoveElementId: "", //移动对象ID
    ElementValues: "", //存储编辑元素之前的值
    IsParent: false, //当前元素是否为父类
    IsShow: true, //是否显示
    Lock: false, //是否锁屏
    MouseDown: false, //当前操作是否在移动
    CurrentContentTop: 0, //当前元素的Top值
    CurrentContentWidth: 0, //当前元素的宽度
    CurrentContentHeight: 0, //当前元素的高度
    ScrollTop: 0, //当前浏览器滚动条的top值
    WindowHight: 0, // 当前浏览器的高度
    PageHight: 0, //当前页高度
    Top: 0, //存储元素top值
    Left: 0, //存储元素left值
    MainId: "", //当前元素的父类ID
    EditAttribute: "", //  当前编辑元素的属性值
    Design_Page_Init: function () {
        $.ajax({
            url: desigurl,
            async: false,
            type: "post",
            data: {
                ModelId: "Load",
                PageName: Hidistro_designer.CurrentPageName
            },
            dataType: "json",
            beforeSend: function (msg) {

                Hidistro_designer.ControlDic.add("top", "销售排行榜");
                Hidistro_designer.ControlDic.add("tab", "商品选项卡编辑");
                Hidistro_designer.ControlDic.add("custom", "自定义编辑区");
                Hidistro_designer.ControlDic.add("floor", "商品楼层");
                Hidistro_designer.ControlDic.add("group", "商品组合");
                Hidistro_designer.ControlDic.add("simple", "商品列表");
                Hidistro_designer.ControlDic.add("slide", "轮播广告");
                Hidistro_designer.ControlDic.add("image", "单张图片广告");
                Hidistro_designer.ControlDic.add("article", "文章列表");
                Hidistro_designer.ControlDic.add("category", "分类列表");
                Hidistro_designer.ControlDic.add("brand", "品牌列表");
                Hidistro_designer.ControlDic.add("keyword", "关键字列表");
                Hidistro_designer.ControlDic.add("attribute", "属性列表");
                Hidistro_designer.ControlDic.add("title", "文字导航标签");
                Hidistro_designer.ControlDic.add("morelink", "更多链接标签");
                Hidistro_designer.ControlDic.add("logo", "Logo更换");

            },
            success: function (msg) {
                if (msg.success) {
                    $.each(msg.Result, function (index, item) {
                        $("#" + item.TempleteID).html(item.TempleteContent);
                    });
                } else {
                    Hidistro_Dialog.dialogContent = msg.Result;
                    Hidistro_Dialog.dialogShow();
                }
            },
            complete: function () {
                windowMoveto(); //重新设置窗口和页面的高度
                Hidistro_designer.BindEvent(); //绑定对象事件
                $(".cssEdite").each(function (index, item) {
                    var editID = $(item).attr("id");
                    Hidistro_designer.BindSingleEvent(editID, true);
                });
            }
        });
    }, //页面加载
    MoveUp: function () {

    }, // 向上移动
    MoveDown: function () {

    },   //向下移动
    BindSingleEvent: function (singleobj, isevent) {
        var currentsignle = singleobj; //获取当前传过来的id值
        $("#" + currentsignle).hover(function () {
            var currentdesign = this;
            $(currentdesign).focus();
            if (Hidistro_designer.IsShow && !Hidistro_designer.MouseDown
            && $("#" + currentsignle).attr("id") != Hidistro_designer.MoveElementId) {//当窗口为显示状态并且当前层不为移动层时
                $(groupdiv).css({ top: $(currentdesign).offset().top, left: $(currentdesign).offset().left }); //定位阴影层
                $(groupdiv).width($(currentdesign).outerWidth() - 3);
                $(groupdiv).height($(currentdesign).outerHeight() - 3);
                $(assitsdiv).width($(currentdesign).outerWidth() - 3);
                $(assitsdiv).height($(currentdesign).outerHeight() - 3);
                $(assitsdiv).css({ top: $(currentdesign).offset().top, left: $(currentdesign).offset().left });

                var m = $(currentdesign).width() - 176; //定位编辑层样式

                $(groudedite).css({
                    top: $(currentdesign).offset().top,
                    left: $(currentdesign).offset().left + m
                });
                $(groudedite).width(176);

                try {
                    Hidistro_designer.CurrentcontrolType = $(currentdesign).attr("type"); //获取层控件类型
                    Hidistro_designer.MainId = $(currentdesign).attr("id").split(prefix)[1];
                    if ($(currentdesign).attr("mainid") != undefined && $(currentdesign).attr("mainid") != "") {
                        Hidistro_designer.MainId = $(currentdesign).attr("mainid");
                    }
                    Hidistro_designer.EditAttribute = $(currentdesign).attr("attribute");
                } catch (n) {
                    try {
                        Hidistro_designer.MainId = $(currentdesign).attr("id").split(prefix)[1];
                    } catch (n) { }
                }
                Hidistro_designer.CurrentContentID = $(currentdesign).attr("id"); //设置当前层Id


                Hidistro_designer.CurrentContentTop = $(currentdesign).offset().top; //获取当前层的top值
                Hidistro_designer.CurrentContentWidth = $(currentdesign).outerWidth() - 3; //获取当前层宽度
                Hidistro_designer.CurrentContentHeight = $(currentdesign).outerHeight() - 3; //获取当前层高度

                //定位层类型对象
                $(ctrnamediv).css({
                    top: $(currentdesign).offset().top + ($(currentdesign).outerHeight() / 2 - 12),
                    left: $(currentdesign).offset().left + $(currentdesign).outerWidth() / 2 - 80
                });

                $(ctrnamediv).html(Hidistro_designer.ControlDic.get($(currentdesign).attr("type"))); //获取属于哪种类型的标签
                $(ctrnamediv).hide();

                if ($(ctrnamediv).html() != "") {
                    Hidistro_designer.IsParent = false;
                }

                if ($(currentdesign).offset().top != 0 && $(currentdesign).offset().left != 0) {//当前层的Top值和Left不为0时，显示阴影浮动层
                    $(assitsdiv).show();
                    $(groupdiv).show();
                    $(groudedite).show()
                }

                Hidistro_designer.MoveElementId = "";
                $(a_edite).show();
                $(a_down).show();
                $(a_up).show();
                $(a_delete).show();

                if ($(currentdesign).attr("IsEdit") == "true" || $(currentdesign).attr("del") == "no" || 1 == 1) { //当层属性只为编辑或者不可删除时
                    $(a_down).hide();
                    $(a_up).hide();
                    $(a_delete).hide();
                }

                if ($(currentdesign).attr("del") == "1") {//当删除值为1时，隐藏编辑
                    $(a_edite).hide();
                }

                if ($(currentdesign).attr("move") == "no") {//指定层属性是否可移动
                    $(a_down).hide();
                    $(a_up).hide()
                }
            }
        });
    }, //绑定单个对象事件
    BindEvent: function () {
        var top = 500
        $(".cssEdite").hover(function () {
            Hidistro_designer.IsShow && !Hidistro_designer.MouseDown && !Hidistro_designer.Lock;
        });

        $(groudedite).hover(function () {
            $(assitsdiv).show();
            $(groupdiv).show();
            $(groudedite).show();
            $(ctrnamediv).hide();
        });

        $(assitsdiv).mouseout(function () {
            $(this).hide();
            $(groupdiv).hide();
            $(groudedite).hide();
            $(ctrnamediv).hide();
        });

        if ($.browser.msie && $.browser.version == "6.0") {
            $(window).scroll(function (e) {
                if (Hidistro_designer.MouseDown) {
                    if (!Hidistro_designer.Lock) {
                        var currentassitsdiv = Hidistro_designer.CurrentContentTop - top;
                        $(assistdiv).css({
                            top: currentassitsdiv - (Hidistro_designer.ScrollTop - $(document).scrollTop())
                        });
                        $(groupdiv).css({
                            top: currentassitsdiv + top - (Hidistro_designer.ScrollTop - $(document).scrollTop())
                        });
                    }
                }
            });
        } else if ($.browser.msie) {
            $("body").scroll(function (e) {
                if (Hidistro_designer.MouseDown) {
                    if (!Hidistro_designer.Lock) {
                        var d = Hidistro_designer.CurrentContentTop - top;
                        $(assistdiv).css({
                            top: currentassitsdiv - (Hidistro_designer.ScrollTop - $(document).scrollTop())
                        });
                        $(groupdiv).css({
                            top: currentassitsdiv + top - (Hidistro_designer.ScrollTop - $(document).scrollTop())
                        });
                    }
                }
            });
        } else {
            $(document).scroll(function (e) {
                if (Hidistro_designer.MouseDown) {
                    if (!Hidistro_designer.Lock) {
                        var d = Hidistro_designer.CurrentContentTop - top;
                        $(assistdiv).css({
                            top: currentassitsdiv - (Hidistro_designer.ScrollTop - $(document).scrollTop())
                        });
                        $(groupdiv).css({
                            top: currentassitsdiv + top - (Hidistro_designer.ScrollTop - $(document).scrollTop())
                        });
                    }
                }
            });
        }

        $(groudedite).mouseup(function () {
            Hidistro_designer.MouseDown = false;
            $("#" + Hidistro_designer.CurrentContentID).show();
        });
        $(groudedite).mousemove(function (a) {
            if (Hidistro_designer.MouseDown) {
                $(assistdiv).css({
                    top: a.pageY - GJG_designer.Top + top,
                    left: a.pageX - GJG_designer.Left + top
                });
                $("#" + Hidistro_designer.CurrentContentID).hide();
            }
        });
        $(document).mousemove(function (h) {
            g = true;
            if (Hidistro_designer.MouseDown) {
                Hidistro_designer.Lock = false;
                if (h.clientY <= 180 && h.clientY > 100) {
                    Hidistro_designer.Lock = g;
                    $(document).scrollTop($(document).scrollTop() - 15)
                }
                if (h.clientY <= 100 && h.clientY > 50) {
                    Hidistro_designer.Lock = g;
                    $(document).scrollTop($(document).scrollTop() - 25)
                }
                if (h.clientY <= 50) {
                    Hidistro_designer.Lock = g;
                    $(document).scrollTop($(document).scrollTop() - 45)
                }
                var i = Hidistro_designer.WindowHight - h.clientY;
                if (i <= 180 && i > 80) {
                    Hidistro_designer.Lock = g;
                    $(document).scrollTop($(document).scrollTop() + 15)
                }
                if (i <= 80 && i > 40) {
                    Hidistro_designer.Lock = g;
                    $(document).scrollTop($(document).scrollTop() + 25)
                }
                if (i <= 40) {
                    Hidistro_designer.Lock = g;
                    $(document).scrollTop($(document).scrollTop() + 45)
                }
                $(assistdiv).css({
                    top: h.pageY - Hidistro_designer.Top + top,
                    left: h.pageX - Hidistro_designer.Left + top
                });
                $(groupdiv).css({
                    top: h.pageY - Hidistro_designer.Top + 1e3,
                    left: h.pageX - Hidistro_designer.Left + 1e3
                });
                $(groudedite).hide();
            }
        });

    }, //绑定加载事件
    EditeDesigDialog: function () { //编辑显示弹出窗
        $.ajax({
            url: desigurl,
            type: "post",
            cache: false,
            data: {
                ModelId: "editedialog",
                Type: Hidistro_designer.CurrentcontrolType,
                Elementid: Hidistro_designer.CurrentContentID,
                rand:Math.random()
            },
            dataType: "json",
            success: function (msg) {
                if (msg.success) {
                    Hidistro_designer.ElementValues = msg.Result;
                    $(taboverlaycontent).load(msg.Result.DialogName+"?rand="+Math.random(),
                    function () {
                        try {
                            loadEditDialogData();
                        } catch (b) {
                           // alert(b);
                        }
                    });
                }
            }
        })
    }, //显示
    ShowDialog: function () {

    },
    setHight: function () {
        Hidistro_designer.WindowHight = $(window).height();
        Hidistro_designer.PageHight = $(document).height()
    } //调整当前窗口的宽度和高度
};


function windowMoveto() {
    try {
        window.moveTo(0, 0);
        window.resizeTo(window.screen.width, window.screen.height);
        Hidistro_designer.setHight()
    } catch (a) { }
}

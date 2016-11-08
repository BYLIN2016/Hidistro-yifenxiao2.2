<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle runat="server" />
    <Hi:MetaTags runat="server" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <Hi:Style ID="Stylee1" runat="server" Href="/Templates/master/default/style/style.css"></Hi:Style>
    <Hi:Style ID="Style1" runat="server" Href="/Templates/master/default/style/menu.css"></Hi:Style>
</head>
<body>
    <div id="header">
        <div class="top_nav_bg">
            <div class="top_nav">
                <div class="top_link">
                    <div class="login_re">
                        <Hi:Common_CurrentUser runat="server" ID="lblCurrentUser" />
                        <Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" />
                        <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /></div>
                    <div class="link_list">
                        <span>
                            <Hi:SiteUrl ID="SiteUrl1" UrlName="productunslaes" runat="server">下架区</Hi:SiteUrl></span>
                        <span>|</span> <span>
                            <Hi:SiteUrl ID="SiteUrl2" UrlName="distributorLogin" runat="server">分销商登录</Hi:SiteUrl></span>
                        <span>|</span> <span>
                            <Hi:SiteUrl ID="SiteUrl3" UrlName="LeaveComments" runat="server">店铺交流区</Hi:SiteUrl></span>
                        <span>|</span> <span>
                            <Hi:SiteUrl ID="SiteUrl4" UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="header_p1">
            <div class="logo">
                <Hi:Common_Logo ID="Common_Logo1" runat="server" />
            </div>
            <div class="search_tab">
                <div class="search">
                    <Hi:Common_Search ID="Common_Search" runat="server" />
                </div>
                <div class="hot_key">
                    <em>热门关键字：</em>
                    <Hi:Common_SubjectKeyword ID="Common_SubjectKeyword1" runat="server" CommentId="4" />
                </div>
            </div>
            <div class="top_buycart">
                <Hi:Common_ShoppingCart_Info ID="Common_ShoppingCart_Info1" runat="server" />
            </div>
        </div>
        <div class="nav_bg">
            <div class="nav">
                <div class="side_nav">
                    <h3 class="title">
                        <Hi:SiteUrl ID="SiteUrl5" UrlName="Category" runat="server">全部商品分类</Hi:SiteUrl>
                    </h3>
                    <div class="my_left_category">
                        <Hi:Common_CategoriesWithWindow ID="Common_CategoriesWithWindow1" MaxCNum="12" runat="server" />
                    </div>
                </div>
                <div class="main_nav" id="ty_menu_title">
                    <ul>
                        <li><a href="/"><span>首页</span></a></li>
                        <Hi:Common_PrimaryClass ID="Common_PrimaryClass1" runat="server" />
                        <Hi:Common_HeaderMune ID="Common_HeaderMune1" runat="server" />
                    </ul>
                </div>
            </div>
        </div>
            <script>
                var currenturl = location.href.substr(location.href.lastIndexOf('/') + 1, location.href.length - 20);
                if (currenturl != "" && currenturl != null && currenturl != "Default.aspx" && currenturl != "Desig_Templete.aspx?skintemp=default") {
                    $(".side_nav").hover(function () {
                        $(".my_left_category").css({ 'display': 'block' });
                    }, function () {
                        $(".my_left_category").css({ 'display': 'none' });
                    });
                } else {
                    $(".my_left_category").css({ 'display': 'block' });
                }
    </script>
    </div>
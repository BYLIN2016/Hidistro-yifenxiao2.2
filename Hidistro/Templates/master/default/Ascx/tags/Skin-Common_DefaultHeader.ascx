<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>		
<Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />

<div class="top_nav_bg">
    <div class="top_nav">
    <div class="top_link">
    <div class="login_re"><Hi:Common_CurrentUser runat="server" ID="lblCurrentUser"/> <Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" /> <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /></div>
    <div class="link_list">
    <span><Hi:SiteUrl ID="SiteUrl1" UrlName="productunslaes" runat="server">下架区</Hi:SiteUrl></span>
    <span>|</span>
    <span><Hi:SiteUrl ID="SiteUrl2" UrlName="distributorLogin" runat="server">分销商登录</Hi:SiteUrl></span>
    <span>|</span>
    <span><Hi:SiteUrl ID="SiteUrl3" UrlName="LeaveComments" runat="server">店铺交流区</Hi:SiteUrl></span>
    <span>|</span>
    <span><Hi:SiteUrl ID="SiteUrl4" UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl></span>
    </div>
    </div>

    </div>
</div>


<div class="header_p1">
    <div class="logo"><Hi:Common_Logo runat="server" /></div>
    <div class="search_tab">
        <div class="search"><Hi:Common_Search ID="Common_Search" runat="server" /></div>
        <div class="hot_key">
            <em>热门关键字：</em>
            <Hi:Common_SubjectKeyword runat="server" CommentId="4" />
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
            <Hi:SiteUrl ID="SiteUrl6" UrlName="Category" runat="server">全部商品分类</Hi:SiteUrl>
            </h3>
            <div class="my_left_category">
               <Hi:Common_CategoriesWithWindow  MaxCNum="13"  runat="server" />
            </div>
        </div>
        <div class="main_nav" id="ty_menu_title">
            <ul>
                <li><a  href="/"><span>首页</span></a></li>
                <Hi:Common_PrimaryClass ID="Common_PrimaryClass1" runat="server"  />
                <Hi:Common_HeaderMune ID="Common_HeaderMune2" runat="server" />
            </ul>
        </div>
    </div>
</div>



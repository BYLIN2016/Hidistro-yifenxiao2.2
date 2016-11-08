<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>		

<!--头部开始-->
<div class="member_top_info">
<div class="member_top_info_c">
<div class="member_top_info_c_l">		<Hi:Common_CurrentUser runat="server" ID="lblCurrentUser"/>
			[<Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" />
			<Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" />]</div>
    <ul  class="member_top_info_c_r">
        <li><span><Hi:SiteUrl UrlName="shoppingCart" runat="server">我的购物车</Hi:SiteUrl></span> <span>|</span>
             <span><Hi:SiteUrl UrlName="LeaveComments" runat="server">店铺交流区</Hi:SiteUrl></span> <span>|</span> 
             <span><Hi:SiteUrl UrlName="Promotes" runat="server">促销活动</Hi:SiteUrl></span> <span>|</span>
             <span><Hi:SiteUrl UrlName="Category" runat="server">全部分类</Hi:SiteUrl></span> 
             <span>|</span> <span><Hi:SiteUrl UrlName="AllArticles" runat="server">商城资讯</Hi:SiteUrl></span></li>
          </ul>
</div>
</div>

<div class="core_blank16"></div> 
<Hi:Common_Logo runat="server" />
 
<div class="core_blank16"></div>

<div class="member_nav">
<ul class="member_nav_l">
<li><Hi:SiteUrl UrlName="user_MyAccountSummary" runat="server">我的帐户</Hi:SiteUrl></li>
<li><Hi:SiteUrl UrlName="home" runat="server">商城首页</Hi:SiteUrl></li>
<li><Hi:SiteUrl UrlName="user_UserOrders" runat="server">订单查询</Hi:SiteUrl></li>
<li><Hi:SiteUrl UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl></li>
</ul>
<div class="member_nav_r"><div class="member_nav_rc"><Hi:Common_ShoppingCart_Info runat="server" /></div>  </div>
</div>
<div class="core_blank8"></div>


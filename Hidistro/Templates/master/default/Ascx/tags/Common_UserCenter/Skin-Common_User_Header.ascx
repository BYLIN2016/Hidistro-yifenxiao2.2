<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>		

<!--ͷ����ʼ-->
<div class="member_top_info">
<div class="member_top_info_c">
<div class="member_top_info_c_l">		<Hi:Common_CurrentUser runat="server" ID="lblCurrentUser"/>
			[<Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" />
			<Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" />]</div>
    <ul  class="member_top_info_c_r">
        <li><span><Hi:SiteUrl UrlName="shoppingCart" runat="server">�ҵĹ��ﳵ</Hi:SiteUrl></span> <span>|</span>
             <span><Hi:SiteUrl UrlName="LeaveComments" runat="server">���̽�����</Hi:SiteUrl></span> <span>|</span> 
             <span><Hi:SiteUrl UrlName="Promotes" runat="server">�����</Hi:SiteUrl></span> <span>|</span>
             <span><Hi:SiteUrl UrlName="Category" runat="server">ȫ������</Hi:SiteUrl></span> 
             <span>|</span> <span><Hi:SiteUrl UrlName="AllArticles" runat="server">�̳���Ѷ</Hi:SiteUrl></span></li>
          </ul>
</div>
</div>

<div class="core_blank16"></div> 
<Hi:Common_Logo runat="server" />
 
<div class="core_blank16"></div>

<div class="member_nav">
<ul class="member_nav_l">
<li><Hi:SiteUrl UrlName="user_MyAccountSummary" runat="server">�ҵ��ʻ�</Hi:SiteUrl></li>
<li><Hi:SiteUrl UrlName="home" runat="server">�̳���ҳ</Hi:SiteUrl></li>
<li><Hi:SiteUrl UrlName="user_UserOrders" runat="server">������ѯ</Hi:SiteUrl></li>
<li><Hi:SiteUrl UrlName="AllHelps" runat="server">��������</Hi:SiteUrl></li>
</ul>
<div class="member_nav_r"><div class="member_nav_rc"><Hi:Common_ShoppingCart_Info runat="server" /></div>  </div>
</div>
<div class="core_blank8"></div>


<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

���������<b><asp:Literal ID="cartNum" runat="server" Text="0" /></b> ����Ʒ �ϼ�<b>
<Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" /></b>Ԫ 
<Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server"><img src="/Templates/master/default/images/common/jiesuan_icon.jpg" width="43" height="17" /></Hi:SiteUrl>
 
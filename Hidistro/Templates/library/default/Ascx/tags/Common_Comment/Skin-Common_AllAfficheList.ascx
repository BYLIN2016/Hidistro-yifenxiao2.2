<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="article_list">
        <tr>
            <td width="24"> </td>
            <td width="593" align="left">
                <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("AffichesDetails",Eval("AfficheId"))%>' target="_blank">
                    <asp:Literal ID="litTitle" runat="server" Text='<%#Eval("Title") %>' />
                </a>
            </td>
            <td width="121"><em><Hi:FormatedTimeLabel id="lblAddedDate" Time='<%#Eval("AddedDate") %>' runat="server" /></em></td>
       </tr>
</table>
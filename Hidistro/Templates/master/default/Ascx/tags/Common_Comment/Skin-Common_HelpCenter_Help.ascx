<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
 <li class="article_list">
        <em style="float:right; margin-right:10px;"><Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("AddedDate") %>' runat="server" /></em>
        <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails",Eval("HelpId"))%>'><Hi:SubStringLabel ID="SubStringLabel1" Field="Title" runat="server" /></a>        
</li>



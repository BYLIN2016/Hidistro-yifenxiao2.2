<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
 <li class="article_list">
     <em style="float:right; margin-right:10px;"><Hi:FormatedTimeLabel runat="server" Time='<% #Eval("AddedDate") %>'  ID="time"/></em>
    <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails",Eval("ArticleId"))%>' target="_blank"  Title='<%#Eval("Title") %>'>
        <Hi:SubStringLabel ID="SubStringLabel" Field="Title" StrLength="14" runat="server"  />
    </a>   
</li>

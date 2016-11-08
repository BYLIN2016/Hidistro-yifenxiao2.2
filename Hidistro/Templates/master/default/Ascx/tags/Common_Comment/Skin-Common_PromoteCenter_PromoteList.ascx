<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>    

 <li class="article_list">
     <em style="float:right; margin-right:10px;"><%#Eval("PromoteTypeName")%></em>
     <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails",Eval("ActivityId"))%>' ><%#Eval("Name") %></a> 
</li>
 
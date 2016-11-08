<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<h4><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("Helps",Eval("CategoryId"))%>'><%#Eval("Name")%></a></h4>
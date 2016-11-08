<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<h4><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("Articles",Eval("CategoryId"))%>'><Hi:SubStringLabel ID="lblCategoryName" StrLength="10" Field="Name" runat="server" /></a></h4>
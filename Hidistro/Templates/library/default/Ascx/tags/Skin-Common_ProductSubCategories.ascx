<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<li><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%> </a></li>

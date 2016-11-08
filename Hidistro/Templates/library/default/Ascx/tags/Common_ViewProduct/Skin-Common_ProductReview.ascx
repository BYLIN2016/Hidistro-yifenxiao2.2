<%@ Control Language="C#"%>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<li class="product_reviews_list_ask product_reviews_list">
	<div><span class="product_reviews_time" style="float:right;"><%# Eval("ReviewDate")%></span><b class="color_36c"><%# Eval("UserName").ToString().Substring(0,1) %>***<%# Eval("UserName").ToString().Substring(Eval("UserName").ToString().Length-1)%>&nbsp;˵:</b> </div>
	<div><%# Eval("ReviewText") %> </div>  
</li>

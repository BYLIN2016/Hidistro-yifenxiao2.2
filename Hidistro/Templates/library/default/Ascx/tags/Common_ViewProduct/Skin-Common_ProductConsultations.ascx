<%@ Control Language="C#" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

  
  <div class="product_reviews_list">
   <div class="product_reviews_list_ask">
			<div><span class="product_reviews_time"><%# Eval("ConsultationDate")%></span><span class="color_re_red"> <b><%# Eval("UserName").ToString().Substring(0,1) %>***<%# Eval("UserName").ToString().Substring(Eval("UserName").ToString().Length - 1) %>&nbsp;˵:</b> </span></div>
			<div><asp:Label ID="Label1" runat="server" Text='<%# Eval("ConsultationText") %>'></asp:Label></div> 
			</div>
  <div class="product_reviews_list_re">
			<div><span class="product_reviews_time"><%# Eval("ReplyDate") != null ? Eval("ReplyDate") : "����"%></span><span class="color_re_green"> <b>����Ա�ظ���</b> </span></div>
			<div><%# Eval("ReplyText") != null ? Eval("ReplyText") : "����"%></div>  	
			</div>
</div>

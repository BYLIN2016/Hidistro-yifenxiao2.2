<%@ Control Language="C#"%>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<table class="tab_sales">
<tr class="tr_head">
<td>买家</td><td>数量</td><td>付款时间</td><td>款式和型号</td>
</tr>
<asp:Repeater ID="rp_productsales" runat="server">
<ItemTemplate>
<tr>
<td><%#Eval("Username") %></td>
<td><%#Eval("Quantity") %></td>
<td><%#Eval("PayDate")%></td>
<td><%# Eval("SKUContent") %>&nbsp;</td>
</tr>
</ItemTemplate>
</asp:Repeater>
</table>
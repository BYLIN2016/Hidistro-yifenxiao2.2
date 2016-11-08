<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
 <li>
    <div class="pic"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                    <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl60" /></Hi:ProductDetailsLink>
                </div>
    <div class="info">
    <div class="name"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="false"/></div>
    <div class="reviews"><asp:Label ID="Label2" runat="server" Text='<%# Eval("ReviewText") %>'></asp:Label></div>
    <span class="colorA" style="display:none;"><strong class="colorD">评论网友：</strong><%# Eval("UserName") %> </span> </td><td><span class="colorC">评论时间：<%# Eval("ReviewDate")%></span>
    </div>
</li>
<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
  <asp:Repeater ID="repcountdown" runat="server">
<ItemTemplate>

 <li>

            <div class="time"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span>
 <Hi:LeaveListTime runat="server" ID="LeaveListTime" /></div>
            <div class="pic"> <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
        <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl160" /></Hi:ProductDetailsLink></div>
            <div class="name"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  IsCountDownProduct="true"  ProductName='<%# Eval("ProductName") %>'  
        ProductId='<%# Eval("ProductId") %>' ImageLink="false"/></div>
            <div class="price"><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>'/></div>

            </li>
                        </ItemTemplate>
</asp:Repeater>
            <p style="display:none;"> <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true" CssClass="btnbuy">立即抢购</Hi:ProductDetailsLink></p>
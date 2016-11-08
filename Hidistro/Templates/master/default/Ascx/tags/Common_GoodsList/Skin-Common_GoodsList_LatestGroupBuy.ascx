<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Repeater ID="regroupbuy" runat="server">
<ItemTemplate>
<li>

              <p class="pro_time" style="display:none;"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span>剩余：<Hi:LeaveListTime runat="server" ID="LeaveListTime" /> </p>  
            <div class="pic"><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl100" /></div>
            <div class="info">
            <div class="name"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsGroupBuyProduct="true" ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink></div>
            <div class="price">￥<Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /><span style=" display:none;"><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("OldPrice") %>'/></span></div>
            <div class="btn"><Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" IsGroupBuyProduct="true" ProductId='<%# Eval("ProductId") %>' ImageLink="true"><img src="/templates/master/default/images/common/group_btn.jpg" width="64" height="24" /></Hi:ProductDetailsLink></div>
            </div>

            </li>
            </ItemTemplate>
</asp:Repeater>
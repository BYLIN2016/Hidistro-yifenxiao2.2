<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlShopProductCart" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <th width="120" align="center">
                商品图片
            </th>
            <th width="350" align="center">
                商品名称
            </th>
            <th width="120" align="center">
                商品单价
            </th>
            <th width="160" align="center">
                购买数量
            </th>
            <th width="160" align="center">
                发货数量
            </th>
            <th width="120" align="center">
                小计
            </th>
        </tr>
        <asp:Repeater ID="dataListShoppingCrat" runat="server">
            <ItemTemplate>
                <tr>
                    <td height="40" align="center">
                        <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>'
                            ProductName='<%# Eval("Name")%>' runat="server" ImageLink="true">
                   <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl60" runat="server" />
                        </Hi:ProductDetailsLink>
                    </td>
                    <td align="left" style="padding: 10px">
                        <div class="name">
                            <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductId='<%# Eval("ProductId")%>'
                                ProductName='<%# Eval("Name")%>' runat="server"  /></div>
                        <div style="color: #999;">
                            <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></div>
                    </td>
                    <td align="center" class="cart_Order_price">
                        <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" runat="server" Money='<%# Eval("MemberPrice")%>' /></span>
                    </td>
                    <td align="center">
                        <asp:Literal runat="server" ID="txtStock" Text='<%# Eval("Quantity")%>' />
                        <div>
                            <asp:Literal ID="litGiveQuantity" Text='<%# (int)Eval("Quantity")==(int)Eval("ShippQuantity")?"":"赠送："+((int)Eval("ShippQuantity")-(int)Eval("Quantity")) %>'
                                runat="server" /></div>
                    </td>
                    <td align="center">
                        <%# Eval("ShippQuantity")%>
                    </td>
                    <td align="center" class="cart_Order_price">
                        <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" runat="server" Money='<%# Eval("SubTotal")%>' /></span>
                        <div class="color_36c">
                            <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                                Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>

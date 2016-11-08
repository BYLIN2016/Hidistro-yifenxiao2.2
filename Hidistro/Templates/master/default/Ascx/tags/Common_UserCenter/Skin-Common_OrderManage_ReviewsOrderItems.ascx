<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<table cellspacing="0" border="0" >
    <tr class="GridViewHeaderStyle" style="color:#858585; text-align:left;">
        <th class="content_table_title" width="60px">商品图片</th>
         <th class="content_table_title" width="130px">货号</th>
        <th class="content_table_title" width="350px">商品名称</th>
        <th class="content_table_title" width="290px">评论</th>
         <asp:Repeater ID="rp_orderItem" runat="server">
         <ItemTemplate>
         <tr>
                <td >
                <input type="hidden" runat="server" id="hdproductId" value=<%# Eval("ProductId")+"&"+Eval("SKU")%>/>
                    <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                        <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl"/>
                    </Hi:ProductDetailsLink>                            
                </td>
                <td>
                    <asp:Literal ID="litSKU" runat="server" Text='<%# Eval("SKU")+"&nbsp;" %>'></asp:Literal></td>
                <td>
                <Hi:ProductDetailsLink ID="productNavigationDetails"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' runat="server"/>
                <br />
                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                <br />
                </td>
                <td>
                    <textarea id="txtcontent" rows="3" cols="30" runat="server"></textarea>
                </td>
            </tr>
         </ItemTemplate>
         </asp:Repeater>
    </tr>
</table>
<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Panel ID="pnlFreeShopGiftCart" runat="server">
 <h3>促销赠送礼品</h3>
<asp:DataList ID="dataShopFreeGift" runat="server" Width="100%">
    <HeaderTemplate>
    <div class="cart_Order_info2">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <th width="120" align="center">
                    礼品图片
                </th>
                <th width="350" align="center">
                    礼品名称
                </th>
                <th width="120" align="center">
                    兑换所需积分
                </th>
                <th width="160" align="center">
                    数量
                </th>
                <th width="160" align="center">
                    积分小计
                </th>
                 <th width="120" align="center">
                &nbsp;
                 </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
          <tr>
            <td width="120" align="center">
                <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'>
                        <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl60" runat="server" />
                </a>
            </td>
            <td align="left" style="padding: 10px" >
               <a style="color: #015697;" href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><%# Eval("Name") %></a>
            </td>
            <td align="center" class="cart_Order_price">
            <%# Eval("NeedPoint")%>
            </td>
            <td   align="center">
                <%# Eval("Quantity")%>            </td>
            <td  align="center" >
                <asp:Literal ID="Literal1" runat="server"  Text='<%# Eval("SubPointTotal") %>' /><br />
                 <%# Eval("PromoType").ToString().Equals("5") ? "<img src=\"" + Globals.ApplicationPath + "/Utility/pics/mjcx.png\" />" : ""%>
            </td>
            <td>&nbsp;</td>
        </tr>
       
    </ItemTemplate>
    <FooterTemplate>
        </table>
       </div>
    </FooterTemplate>
</asp:DataList>
</asp:Panel>


<asp:Panel ID="pnlShopGiftCart" runat="server">
 <h3>积分兑换礼品</h3>
<asp:DataList ID="dataListShoppingCrat" runat="server"  Width="100%">
         <HeaderTemplate>
     <div class="cart_Order_info2">    
         <table border="0" cellpadding="0" cellspacing="0" style="text-align:center;">
        <tr>
            <th  width="120" align="center" >礼品图片</th>
            <th width="350" align="center">礼品名称</th>
            <th width="120" align="center">兑换所需积分</th>
            <th width="160" align="center">兑换数量</th>
            <th width="160" align="center">小计</th>
            <td width="120" align="center"> &nbsp;</td>
        </tr>
          </HeaderTemplate>
         <ItemTemplate>
        <tr>
            <td height="40">
               <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" >
                        <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl60" runat="server" />
                </a>
            </td>
            <td style="padding:10px ">
               <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank"><%# Eval("Name") %></a>
            </td>
            <td><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("NeedPoint") %>' /></td>
            <td>
                    <asp:Literal runat="server" ID="txtStock" Text='<%# Eval("Quantity")%>' />                   
            </td>
            <td>
            <asp:Literal ID="Literal2" runat="server"  Text='<%# Eval("SubPointTotal") %>' /><br />
             <%# Eval("PromoType").ToString().Equals("5") ? "<img src=\"/Templates/master/default/images/mjcx.png\" />" : ""%>
            
            </td>
            <td>&nbsp;</td>
        </tr>
</ItemTemplate>
<FooterTemplate>
</table>
</div>
</FooterTemplate>
</asp:DataList>
</asp:Panel>
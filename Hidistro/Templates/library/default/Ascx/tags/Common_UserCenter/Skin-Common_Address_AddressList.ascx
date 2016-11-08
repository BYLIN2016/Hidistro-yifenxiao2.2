<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:DataList ID="dtlstRegionsSelect" runat="server" Width="100%" DataKeyField="ShippingId">
     <ItemTemplate>
     <table class="address">
     <tr>
     <td width="10%">收&nbsp;货&nbsp;人：</td><td width="20%"> <asp:Label ID="lblShipTo" runat="server" Text='<%#Bind("ShipTo") %>'></asp:Label></td>
     <td width="10%">邮政编码：</td><td width="20%"><asp:Label ID="lblZipcode" runat="server" Text='<%#Bind("ZipCode") %>'></asp:Label></td>
     </tr>
     <tr>
     <td> 电话号码：</td><td><asp:Label ID="lblTellPhone" runat="server" Text='<%#Bind("TelPhone")%>'></asp:Label></td>
     <td>手机号码：</td><td><asp:Label ID="lblPhone" runat="server" Text='<%#Bind("CellPhone") %>'></asp:Label></td>
     <td><asp:Button ID="btnEdit" class="btn_style_bar"  runat="server" CommandName="Edit" Text="编辑" /></td><td><asp:Button ID="btnDelete" class="btn_style_bar" runat="server" CommandName="Delete" Text="删除" /></td>
     </tr>
    <tr><td>街道地址：</td><td colspan="5"><Hi:RegionAllName ID="RegionAllName1" RegionId='<%# Eval("RegionId") %>' runat="server"></Hi:RegionAllName><asp:Label ID="lblAddress" runat="server" Text='<%#Bind("Address") %>'></asp:Label></td></tr>
     <tr><td colspan="3"></td><td colspan="3"></td></tr>
     </table>
    </ItemTemplate>
</asp:DataList>

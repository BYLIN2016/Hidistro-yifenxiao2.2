<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManualPurchaseOrder_Items.ascx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManualPurchaseOrder_Items" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

 <asp:DataList ID="dlstOrderItems" runat="server" Width="100%" >
         <HeaderTemplate>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr class="table_title">
        <td colspan="2" class="td_right td_left">商品名称</td>
        <td width="12%" class="td_right td_left">采购价(元) </td>
        <td width="10%" class="td_right td_left">数量 </td>
        <td width="12%" class="td_left td_right_fff">总采购价(元) </td>
      </tr>
        </HeaderTemplate>
        <ItemTemplate>
        
        <tr>
        <td width="7%"> <a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" /></a> </td>
        <td width="32%"><span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank"><%# Eval("ItemDescription") %></a></span> 
            <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' /> <%#Eval("SKUContent")%></span></td>
        <td><Hi:FormatedMoneyLabel ID="lblItemPurchasePrice" runat="server" Money='<%# Eval("ItemPurchasePrice") %>' /></td>
        <td>×<asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
        <td><strong class="colorG"><Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2"  runat="server" Money='<%# (decimal)Eval("ItemPurchasePrice")*(int)Eval("Quantity") %>'/></strong></td>
      </tr>       
        
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>
    
    <div class="Price">
  <table width="200" border="0" cellspacing="0">
	       <tr class="bg">
	      <td class="Pg_top td_none" width="88%" align="right" >商品采购金额：</td>
	      <td class="Pg_top td_none" width="12%" ><strong class="fonts colorG"><Hi:FormatedMoneyLabel ID="lblGoodsAmount" runat="server" /></strong></td>
        </tr>
	    <tr class="bg">
	      <td class="Pg_bot" align="right">商品总重量（克）：</td>
	      <td class="Pg_bot" ><strong class="fonts "><asp:Literal ID="lblWeight" runat="server" /></strong></td>
        </tr>
        </table>
	  </div>
      
      <h1>礼品列表</h1>
      
      <asp:DataList ID="grdOrderGift" runat="server" DataKeyField="GiftId" Width="100%" >
         <HeaderTemplate>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
         <tr class="table_title">
           <td colspan="2" class="td_right td_left">礼品名称</td>
           <td width="16%" class="td_right td_left">礼品采购价(元)</td>
           <td width="14%" class="td_right td_left">数量 </td>
           <td width="16%" class="td_left td_right_fff">总采购价(元)</td>
         </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
           <td width="6%"><Hi:HiImage ID="HiImage1" AutoResize="true" Width="54" Height="50" runat="server" DataField="ThumbnailsUrl" /></td>
           <td width="48%"><span class="Name"><asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span></td>
           <td><Hi:FormatedMoneyLabel ID="giftPrice" runat="server" Text='<%# Eval("PurchasePrice") %>'></Hi:FormatedMoneyLabel></td>
           <td>×<asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'  ></asp:Literal> </td>
           <td><strong class="colorG"> <Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" Money='<%# (decimal)Eval("PurchasePrice")*(int)Eval("Quantity") %>' /></strong></td>
         </tr>
       
        
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ItemsList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_ItemsList" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

    <asp:DataList ID="dlstOrderItems" runat="server"  Width="100%" >
         <HeaderTemplate>
      <table width="200" border="0" cellspacing="0">
	    <tr class="table_title">
	      <td colspan="2" class="td_right td_left">商品名称</td>
	      <td width="12%" class="td_right td_left">商品单价(元) </td>
	      <td width="10%" class="td_right td_left">购买数量 </td>
	      <td width="12%" class="td_right td_left">发货数量 </td>
	      <td width="12%" class="td_left td_right_fff">小计(元) </td>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>        
          <tr>
	      <td width="7%"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" /></a> </td>
	      <td width="32%"><span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
	        <%# Eval("ItemDescription")%></a></span> <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' />
	        <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
	     </td>
	      <td><Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' /></td>
	      <td>×<asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
	      <td>×<asp:Literal runat="server" ID="litShipmentQuantity" Text='<%#Eval("ShipmentQuantity") %>' /></td>
	      <td>
	        <div class="color_36c">
            <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
            </div>
	        <strong class="colorG"><Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2"  runat="server"  Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>'/></strong>
	      </td>
        </tr>        
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>
      
	  <div class="Price" style="width:820px; margin:0 auto">
	    <span class="pricecol">商品重量：<b style="font-size:16px; color:red"><asp:Literal runat="server" ID="litWeight" /></b>g</span>
	    <span style="margin-left:400px;"><b style="font-size:16px; color:red"><asp:Literal runat="server" ID="lblAmoutPrice" /></b></span>
        <span><b style="font-size:16px; color:red"><asp:HyperLink ID="hlkReducedPromotion" runat="server" Target="_blank" /></b></span>
        <strong>购物车小计：<b style="font-size:16px; color:red"><Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" /></b>元</strong>
        <samp style="margin-left:15px;"><asp:Literal runat="server" ID="lblBundlingPrice" /></samp>
	  </div>
	  
	  <h1><asp:Label ID="lblOrderGifts" runat="server" Text="礼品列表"></asp:Label> </h1>
	  <asp:DataList ID="grdOrderGift" runat="server" DataKeyField="GiftId" Width="100%" >
         <HeaderTemplate>
      <table width="200" border="0" cellspacing="0">
        <tr class="table_title">
            <td width="170" class="td_right td_left">礼品名称</td>
            <td width="30" class="td_right td_left">数量 </td>
          </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
            <td ><Hi:HiImage ID="HiImage1" AutoResize="true" Width="60" Height="60" runat="server" DataField="ThumbnailsUrl" /> <span><asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span> </td>
            
            <td>×<asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'  ></asp:Literal></td>
            
        </tr>
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>






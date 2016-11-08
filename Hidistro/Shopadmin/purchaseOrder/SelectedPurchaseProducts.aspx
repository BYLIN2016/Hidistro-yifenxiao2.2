<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SelectedPurchaseProducts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SelectedPurchaseProducts" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="optiongroup mainwidth">
		<ul>
		<li class="optionstar"><a href="PurchaseProduct.aspx" class="optionnext"><span>1.选择商品</span></a></li>
		<li class="menucurrent"><a href="javascript:void(0);"><span>2.确认已选商品</span></a></li>
		<li  class="optionend"><a href="SubmitPurchaseOrder.aspx"><span >3.填写收货信息</span></a></li>	
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
	<input type="hidden" id="hidPageValue"/>	

		<!--搜索-->
		<!--结束-->
		
		<!--数据列表区域-->
	  <div class="datalist">
      <UI:Grid ID="grdSelectedProducts" DataKeyNames="SkuId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns> 
                    <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                        <table cellpadding="0" cellspacing="0" style="border:none;">
                        <tr><td rowspan="2" style="border:none;"><Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailsUrl"/></td>
                        <td style="border:none;"> <span class="Name"><asp:Label ID="Label2" Text='<%#Eval("ItemDescription") %>' runat="server" /></span></td></tr>
                        <tr><td style="border:none;">  <span class="colorC">货号：<%#Eval("SKU") %>&nbsp;&nbsp;<%#Eval("SKUContent")%></span></td></tr>
                        </table>                          
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="零售价" HeaderStyle-Width="135px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%#Eval("ItemListPrice", "{0:F2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="采购价(元)" HeaderStyle-Width="110px" DataField="ItemPurchasePrice" DataFormatString="{0:F2}" HeaderStyle-CssClass="td_right td_left"/>                    
                   
                    <asp:BoundField HeaderText="采购数量" ItemStyle-Width="10%" DataField="Quantity" HeaderStyle-CssClass="td_right td_left" />
                        
                    <asp:TemplateField HeaderText="总价格(元)" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <%# ((decimal)Eval("ItemPurchasePrice") * (int)Eval("Quantity")).ToString("F2")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemTemplate>                            
                            <span class="Name"><Hi:ImageLinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="删除" IsShow="false"  /></span>
                         </ItemTemplate>
                     </asp:TemplateField>                    
                    </Columns>
                </UI:Grid>
	  </div>
      <div class=" VIPbg m_none colorG"><asp:Literal runat="server" ID="litPurchaseCount" /></div>
	  </div>
</asp:Content>

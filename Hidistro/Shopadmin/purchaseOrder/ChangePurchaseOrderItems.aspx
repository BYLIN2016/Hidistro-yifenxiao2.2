<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ChangePurchaseOrderItems.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.purchaseOrder.ChangePurchaseOrderItems" %>
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
<div class="databody">
    <div class="title"> <em><img src="../images/02.gif" width="32" height="32" /></em>
        <h1>修改采购单商品</h1>
        <span>您可以在这里修改采购单商品购买数量，添加、删除采购单中商品</span></div>
</div>
<div class="dataarea mainwidth">
  <div class="datalist">
    <UI:Grid runat="server" ID="grdOrderItems" Width="100%" AllowSorting="false" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc" SortOrderBy="ProductId" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
        <Columns>
            <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank"><%# Eval("ItemDescription") %></a></span>
                    <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' /> <%#Eval("SKUContent")%></span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="零售价(元)" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <Hi:FormatedMoneyLabel  runat="server" Money='<%# Eval("ItemListPrice") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="采购价(元)" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <Hi:FormatedMoneyLabel  runat="server" Money='<%# Eval("ItemPurchasePrice") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="数量" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <asp:TextBox ID="txtItemNumber" Width="50px" runat="server" Text='<%# Eval("Quantity") %>' />
                    <asp:Button runat="server" ID="btnEditItem" CommandArgument='<%# Eval("SkuId") %>' CommandName="UPDATE_QUANTITY" Text="修改" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="零售价小计(元)" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <Hi:FormatedMoneyLabel runat="server" Money='<%# (decimal)Eval("ItemListPrice") * (int)Eval("Quantity")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="采购价小计(元)" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <Hi:FormatedMoneyLabel runat="server" Money='<%# (decimal)Eval("ItemPurchasePrice") * (int)Eval("Quantity")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" Text="删除" CommandArgument='<%# Eval("SkuId") %>' CommandName="UPDATE_ITEMS"  DeleteMsg="确认要删除所选商品吗？" ForeColor="Red" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </UI:Grid>
  </div>
    <div class="blank12 clearfix"></div>
<asp:Panel runat="server" ID="pnlEmpty" Visible="false">
    <div>当前订单状态不是等待付款状态，不能进行操作  <a href="ManageMyManualPurchaseOrder.aspx">返回</a></div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlHasStatus">
    <div>
       <span style="font-weight:700;font-size:14px;font-family:宋体,Arial Narrow;">添加商品</span>
    </div>
    	<!--搜索-->
	<div class="searcharea clearfix">
		<ul>
			<li><span>选择产品线：</span><span><Hi:AuthorizeProductLineDropDownList runat="server" ID="ddlProductLine" CssClass="forminput" AllowNull="true" NullToDisplay="--请选择--" /></span></li>
            <li><span>商品名称：</span><span><asp:TextBox runat="server" ID="txtProductName" CssClass="forminput" /></span></li>
            <li><span>商家编码：</span><span><asp:TextBox runat="server" ID="txtProductCode" CssClass="forminput" /></span></li>
			<li><asp:Button runat="server" ID="btnSearch" Text="查询"  class="searchbutton"/></li>
			<li></li>
		</ul>
	</div>
      <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>
			</div>
			<!--结束-->
    </div>
		
		<!--数据列表区域-->
	  <div class="datalist">
      <UI:Grid ID="grdAuthorizeProducts" DataKeyNames="ProductId" runat="server"  ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
        <Columns> 
        <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
            <ItemTemplate>
            <table cellpadding="0" cellspacing="0" style="border:none;">
            <tr><td rowspan="2" style="border:none;"><Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl100"/></td>
            <td style="border:none;"> 
                <span class="Name"><%#Eval("ProductName") %></span>
                <span>商家编码：<%# Eval("ProductCode") %></span>
                市场价：<%#Eval("MarketPrice", "{0:F2}")%>
            </td></tr>
            </table>                          
            </ItemTemplate>
        </asp:TemplateField>                  
        
         <asp:TemplateField HeaderText="操作" ItemStyle-Width="70%"  >
            <ItemTemplate>
               <UI:Grid  ID="grdSkus" DataKeyNames="SkuId" runat="server"  AutoGenerateColumns="false"  GridLines="None" Width="100%" ShowHeader="false" 
                     OnRowCommand="grdSkus_RowCommand">
                   <Columns>               
                       <asp:TemplateField ItemStyle-Width="30%" >                       
                       <ItemTemplate >
                         <span class="colorC">货号：<%#Eval("SKU") %>&nbsp;<Hi:SkuContentLabel runat="server" ID="litSkuContent" Text='<%#Eval("SkuId") %>' /></span>          
                       </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField ItemStyle-Width="15%" >                       
                       <ItemTemplate >
                         <span class="colorC">库存：<%#Eval("Stock")%></span>          
                       </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField ItemStyle-Width="20%" >                       
                       <ItemTemplate >
                         <span class="colorC">一口价：<%#Eval("SalePrice", "{0:F2}")%></span>          
                       </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField ItemStyle-Width="20%" >                       
                       <ItemTemplate >
                         <span class="colorC">采购价：<%#Eval("PurchasePrice", "{0:F2}")%></span>          
                       </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField ItemStyle-Width="8%">
                        <ItemTemplate>
                         <asp:TextBox ID="txtNum" runat="server" Text="1" Width="30px" CssClass="forminput"></asp:TextBox>       
                       </ItemTemplate>
                      </asp:TemplateField>                       
                      <asp:TemplateField ItemStyle-Width="7%">
                       <ItemTemplate>
                         <span class="Name">
                         <asp:LinkButton ID="lbtnAdd" Text="添加" runat="server" CommandName="add" />
                         </span>
                                
                       </ItemTemplate>
                       </asp:TemplateField>
                    </Columns>
               </UI:Grid>                   
            </div>                
               </ItemTemplate>
            </asp:TemplateField>                           
            </Columns>
        </UI:Grid>
	    <div class="blank12 clearfix"></div>
      </div>
	  <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>	  
      </div>
</asp:Panel>
</div>
</asp:Content>


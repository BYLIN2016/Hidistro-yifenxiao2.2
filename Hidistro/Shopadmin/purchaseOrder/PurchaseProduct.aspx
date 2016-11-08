<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="PurchaseProduct.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.PurchaseProduct" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
		<li class="menucurrent"><a href="javascript:void(0);"><span>1.选择商品</span></a></li>
		<li ><a href="SelectedPurchaseProducts.aspx" ><span>2.确认已选商品</span></a></li>
		<li class="optionend"><a href="SubmitPurchaseOrder.aspx"><span >3.填写收货信息</span></a></li>	
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
	
	<div id="ChoseProduct">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>选择产品线：</span><span><Hi:AuthorizeProductLineDropDownList runat="server" ID="ddlProductLine" CssClass="forminput" AllowNull="true" NullToDisplay="--请选择--" /></span></li>
                 <li><span>商品名称：</span><span><asp:TextBox runat="server" ID="txtProductName" CssClass="forminput" /></span></li>
                <li><span>商家编码：</span><span><asp:TextBox runat="server" ID="txtProductCode" CssClass="forminput" /></span></li>
				<li><asp:Button runat="server" ID="btnSearch" Text="查询"  class="searchbutton"/></li>
			</ul>
		</div>
		<!--结束-->

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
			 <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
				<ul>
					 <li class="batchHandleButton">
                      <span class="signicon"></span> 
                      <span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0);">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0);">反选</a></span>
                    <span class="submit_btnxiajia m_none"><asp:LinkButton runat="server" ID="lkbtnAdddCheck">添加</asp:LinkButton></span>
                     <span class="submit_btnxiajia m_none"><asp:LinkButton runat="server" ID="lkbtncancleCheck">取消</asp:LinkButton></span>
                    </li>
				</ul>
			</div>
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
        
         <asp:TemplateField HeaderText="操作" ItemStyle-Width="65%"  >
            <ItemTemplate>
               <UI:Grid  ID="grdSkus" DataKeyNames="SkuId" runat="server"  AutoGenerateColumns="false"  GridLines="None" Width="100%" ShowHeader="false" 
                    OnRowDataBound="grdSkus_RowDataBound"  OnRowCommand="grdSkus_RowCommand">
                   <Columns> 
                    <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>              
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
	</div>
	<input type="hidden" id="hidPageValue"/>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

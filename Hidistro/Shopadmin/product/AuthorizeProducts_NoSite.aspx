<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="AuthorizeProducts_NoSite.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.AuthorizeProducts_NoSite" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1 class="title_height"><strong><asp:Literal runat="server" ID="litPageTitle" Text="授权产品目录" /></strong></h1>
</div>
		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li><span>商品名称：</span><span>
				  <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
			  </span></li>
			  
			  <li><span>商家编码：</span><span>
				  <asp:TextBox ID="txtSKU" runat="server"></asp:TextBox>
			  </span></li>
				
			  <li><asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="查询" /></li>
				
		  </ul>
	</div>
		<div class="advanceSearchArea clearfix">
			<!--预留显示高级搜索项区域-->
	    </div>
		<!--结束-->
		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		  <div class="pageNumber"> <div class="pagination">
             <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
      </div>
        </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  
      </div>
		<!--数据列表区域-->
		<div class="datalist">
		
		<UI:Grid ID="grdAuthorizeProducts" DataKeyNames="ProductId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns> 
                    <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="60%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                        <table cellpadding="0" cellspacing="0" style="border:none;">
                        <tr><td rowspan="2" style="border:none;"><Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/></td>
                        <td style="border:none;"> <span class="Name"><a href='<%# "../../ProductDetails.aspx?productId="+Eval("ProductId")%>'><asp:Label ID="lblCategoryName" Text='<%#Eval("ProductName") %>' runat="server" /></a></span></td></tr>
                        <tr><td style="border:none;">  <span class="colorC">商家编码：<%#Eval("ProductCode")%></span></td></tr>
                        </table>                          
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="采购价(元)" HeaderStyle-Width="110px" DataField="PurchasePrice" DataFormatString="{0:F2}" HeaderStyle-CssClass="td_right td_left"/>
                    <asp:TemplateField HeaderText="库存" ItemStyle-Width="10%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                            <%# (int)Eval("Stock") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                                     
                    </Columns>
                </UI:Grid>
		 
		  <div class="blank12 clearfix"></div>
      </div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
			
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
            </div>

			</div>
		</div>


	</div>
  <div class="databottom"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

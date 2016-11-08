<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyProducts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyProducts" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
    <div class="toptitle"> <em><img src="../images/03.gif" width="32" height="32" /></em>
      <h1 class="title_height">查看商品分类下的商品</h1>
    </div>
    <!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li><span>商品名称：</span><span>
				 <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  />
			  </span></li>
                <li><span>商品分类：</span>
				  <abbr class="formselect">
						<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" />
					</abbr>
				</li>
                
                <li><span>商家编码：</span><span>
				  <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput"></asp:TextBox>
			  </span></li>
                
			
				<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
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
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton"> <span class="signicon"></span>
		      <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
		       <span class="delete"> <Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" Height="25px" /></span></li>
	        </ul>
	      </div>
	</div>
		<!--数据列表区域-->
		<div class="datalist">
		<UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="false" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc" SortOrderBy="DisplaySequence" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>    
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="45px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="280px" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <Hi:DistributorProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                <Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                </Hi:DistributorProductDetailsLink>
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name">  <Hi:DistributorProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField HeaderText="采购价"
                            ItemStyle-Width="80" DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />
                        <asp:BoundField HeaderText="一口价"
                            ItemStyle-Width="80"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />                        
                        <asp:TemplateField HeaderText="差价" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblPrice" runat="server" Text='<%# Convert.ToDecimal(Eval("SalePrice")) -  Convert.ToDecimal(Eval("PurchasePrice"))%>' ></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="库存数量" ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="20%" HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                            <itemtemplate>
                                 <span class="submit_bianji"><a href='<%# "EditMyProduct.aspx?ProductId="+Eval("ProductId")%>' >编辑</a> </span>
                                 <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDelete1" CommandName="Delete" runat="server" Text="删除" IsShow="true"  /></span>
                         </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
		 
		  <div class="blank12 clearfix"></div>
    </div>
		<!--数据列表底部功能区域-->
 
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                  <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
               </div>

			</div>
		</div>


</div>
  <div class="databottom"></div>
<%--<div class="areacolumn clearfix">
<div class="columnleft2">
		  <div class="columnleftmenu2 clearfix">
		    <ul>		      
              <li class="itempitchon"><span>商品分类管理</span></li>
              <li><a href="AddMyCategory.aspx"><span>添加商品分类</span></a></li>		      
	        </ul>
	      </div>
		  <div class="columnleftbottom2 clearfix"></div>
    </div>
	<div class="columnright">	 
        <h1>查看商品分类下的商品</h1>
        <div class="search_title">查询条件</div>
          <div class="search_border">
          <ul>
               
              <li>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                      <td width="9%">商品名称：</td>
                      <td width="20%"><asp:TextBox ID="txtSearchText" runat="server" CssClass="inputnormal input160"  /></td>
                      <td width="9%">商品分类：</td>
                      <td>
                         <Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" /></td>
                      <td width="3%">商家编码</td>
                    <td width="20%">
                        <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="inputnormal input160"></asp:TextBox>
                    </td>
                      <td width="9%">页面大小：</td>
                      <td width="12%"><Hi:PageSizeDropDownList ID="dropPageSize" runat="server"  CssClass="formselect" /></td>
                      <td><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="submit54"/></td>
                  </tr>
              </table>
              </li>
            </ul>        
          </div>
          <div class="table">
            <div class="options border_top">
              <ul>
                <li>操作选项：</li>
                <li>
                  <input onclick="CheckClickAll()" type="button" class="submit66"  value="全部选择" />
                </li>
                <li>
                  <input onclick="CheckReverse()" type="button" class="submit66" value="反向选择" />
                </li>
                <li>
                  <Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除选定" IsShow="true" CssClass="submit66" Height="25px" />
                </li>
              </ul>
            </div>
            <div>
                <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc" SortOrderBy="DisplaySequence" AutoGenerateColumns="false" HeaderStyle-CssClass="border_background">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <UI:CheckBoxField HeadWidth="35" HeaderStyle-CssClass="border_right border_top border_bottom" />
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="45px" HeaderStyle-CssClass="border_right border_top border_bottom" />
                        <asp:TemplateField ItemStyle-Width="280px" HeaderText="商品" HeaderStyle-CssClass="border_right border_top border_bottom">
                            <itemtemplate>
                                <a href='<%#"../../ProductDetails.aspx?ProductId="+"Eval(ProductId)"%>' target="_blank">
                                        <Hi:ListImage  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 <div>
                                 <div style="color:Blue"> <a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></div>
                                  商家编码：<%# Eval("SKU") %>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField HeaderText="采购价"
                            ItemStyle-Width="80" SortExpression="PurchasePrice" DataField="PurchasePrice" HeaderStyle-CssClass="border_right border_top border_bottom"  />
                        <asp:BoundField HeaderText="一口价"
                            ItemStyle-Width="80" SortExpression="SalePrice" DataField="SalePrice" HeaderStyle-CssClass="border_right border_top border_bottom"  />                        
                        <asp:TemplateField HeaderText="差价" ItemStyle-Width="80" HeaderStyle-CssClass="border_right border_top border_bottom">
                            <itemtemplate>
                             <asp:Label ID="lblPrice" runat="server" Text='<%# Convert.ToDecimal(Eval("SalePrice")) -  Convert.ToDecimal(Eval("PurchasePrice"))%>' ></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="库存数量" SortExpression="Stock" ItemStyle-Width="100" HeaderStyle-CssClass="border_right border_top border_bottom">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                            <itemtemplate>
                                 <a href='<%# "EditMyProduct.aspx?ProductId="+Eval("ProductId")%>' >编辑</a> 
                         </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
            </div>
            <div class="page"><UI:Pager runat="server" ID="pager"  ListToPaging="grdProducts" RunningMode="Get" /></div>
          </div>          
        </div>
        </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

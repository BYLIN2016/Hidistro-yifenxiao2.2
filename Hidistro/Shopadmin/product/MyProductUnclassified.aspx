<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Shopadmin.MyProductUnclassified" MasterPageFile="~/Shopadmin/Shopadmin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth td_top_ccc">
    <div class="toptitle">
          <em><img src="../images/03.gif" width="32" height="32" /></em>
          <h1 class="title_height"><strong>商品扩展分类</strong></h1>
          <span class="title_height">查看店铺中所有商品所属的分类及扩展分类，您可以在此对商品设置扩展分类，也可以批量转移商品的主分类及扩展分类</span> 
        </div>
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>商品名称：</span><span>
				    <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" IsUnclassified="true"  NullToDisplay="--请选择商品分类--" runat="server" />
					</abbr>
				</li>
                <li><span>商家编码：</span><span><asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></span></li>
				<li>
				    <asp:Button ID="btnSearch" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
		</div>
			<div class="advanceSearchArea ">
		<!--预留显示高级搜索项区域-->
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
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
            </div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					    <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true"  /></span>
				</ul>
			</div>
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	    <UI:Grid ID="grdProducts" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ProductId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>   
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>                    
                       <asp:TemplateField ItemStyle-Width="45%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <Hi:DistributorProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                <Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                </Hi:DistributorProductDetailsLink>
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name">  <Hi:DistributorProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode")%></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="所属分类" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <div>
                                    <nobr><span style="font-size:13px;" >主分类：<abbr style=" color:Blue"><asp:Literal runat="server" ID="litMainCategory" /></abbr></span></nobr>
                                </div>
                                <div>
                                    <nobr><span style="font-size:13px;">扩展分类：<abbr style=" color:Blue"><asp:Literal runat="server" ID="litExtendCategory" /></abbr></span></nobr>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="设置扩展分类" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <Hi:DistributorProductCategoriesDropDownList ID="dropAddToCategories" 
                                    runat="server" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
		  <div class="blank12 clearfix"></div>
      </div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
				</div>
			</div>
			</div>
		<div class="blank12 clearfix"></div>
	  <div  class=" br_search" style=" border-bottom:1px #ddd solid;">
       	<div class="searcharea clearfix">
        	<ul>
				<li><span><b>移动商品到分类：</b></span>
					<span class="formselect">
						<Hi:DistributorProductCategoriesDropDownList ID="dropMoveToCategories" runat="server" />
					</span>
				</li>
                <li>
                    <asp:Button runat="server" ID="btnMove" Text="转移主类" CssClass="submit_queding"/>
                </li>
			</ul>
          </div>
          <div class="colorD ">批量转移商品的主类或者将商品转移到未分类，在转移以前请先选择要转移的商品。</div>
		</div>
        
   </div> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>


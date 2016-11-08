<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyProductReviews.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyProductReviews" %>
<%@ Import Namespace="Hidistro.Core"%>
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
  <em><img src="../images/07.gif" width="32" height="32" /></em>
  <h1>商品评论管理</h1>
  <span>管理店铺的所有商品评论，您可以查询或删除商品评论 </span>
</div>
		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li><span>商品名称：</span><span>
				  <asp:TextBox ID="txtSearchText" runat="server"  CssClass="forminput" />
			  </span></li>
				<li>
					<abbr class="formselect">
					  商品分类：<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" />
					</abbr>
				</li>
				<li>
				<span>商家编码：</span><span><asp:TextBox ID="txtSKU"  CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="搜索" /></li>
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
		<div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
				</div>
			</div>	
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  
		  
    </div>
		<!--数据列表区域-->
		<div class="datalist">
		   <asp:DataList ID="dlstPtReviews" runat="server" DataKeyField="ReviewId" Style="width: 100%;" >
                <HeaderTemplate>
                    <table width="200" cellspacing="0px" border="0px"  >
                        <tr class="table_title">
                            <td width="20%" class="td_right td_left"> 评论商品 </td>
                            <td width="9%" class="td_right td_left"> 评论人</td>
                            <td width="40%" class="td_right td_left"> 评论内容</td>
                            <td width="20%" class="td_right td_left"> 评论时间 </td>
                            <td width="11%" class="td_left td_right_fff"> 操作 </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                      <td ><span class="Name"><Hi:DistributorProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink></span></td>
                      <td><span  class="Name"> <a href='<%# Globals.ApplicationPath+(string.Format("/Shopadmin/Underling/UnderlingDetails.aspx?userId={0}",Eval("UserId"))) %>' ><asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>' /></a></span></td>
                      <td ><asp:Label ID="lblText" runat="server" Text='<%# Eval("ReviewText")%>' CssClass="line"></asp:Label> </td>
                      <td><Hi:FormatedTimeLabel ID="ConsultationDateTime" Time='<%# Eval("ReviewDate") %>' runat="server" /></td>
                      <td><span class="submit_shanchu"><Hi:ImageLinkButton ID="btnReviewDelete" runat="server" CommandName="Delete" IsShow="true" CommandArgument='<%# Eval("ReviewId")%>' Text="删除" /></span></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
		  <div class="blank12 clearfix"></div>
		</div>
        
		<!--数据列表底部功能区域-->  
		<!--翻页-->
	 <div class="page">
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
			</div>
			</div>
		</div>
		</div>
	</div>	
     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

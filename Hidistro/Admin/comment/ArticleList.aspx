<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ArticleList" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
			<li class="menucurrent"><a href="ArticleList.aspx"><span>文章管理</span></a></li>
			<li class="optionend"><a href="ArticleCategories.aspx"><span>文章分类管理</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
      <div class="Pa_15">
         <a href="AddArticle.aspx" class="submit_jia">添加新文章</a>
	    </div>
		<!--结束-->
	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
		    <li><span>关键字：</span><span><asp:TextBox ID="txtKeywords" runat="server" CssClass="forminput" /></span></li>
				<li>
					<abbr>
						<Hi:ArticleCategoryDropDownList ID="dropArticleCategory" NullToDisplay="全部" runat="server" Width="150px" style=" padding:4px;" />
					</abbr>
				</li>
                <li><span>选择时间段：</span><span><UI:WebCalendar runat="server" CalendarType="StartDate" CssClass="forminput" ID="calendarStartDataTime" /></span><span class="Pg_1010">至</span><span><UI:WebCalendar runat="server"  CalendarType="EndDate"  CssClass="forminput" ID="calendarEndDataTime" IsStartTime="false"/></span></li>
				<li style="margin-top:3px;"><asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="查询" /></li>
			</ul>
	  </div>

<div class="functionHandleArea clearfix m_none">
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
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck"　IsShow="true" runat="server" Text="删除" /></span></li>
				</ul>
			</div>
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	  <UI:Grid ID="grdArticleList" runat="server" ShowHeader="true" AutoGenerateColumns="false"  DataKeyNames="ArticleId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                <asp:TemplateField HeaderText="图片" ShowHeader="true" ItemStyle-Width="20px" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate><span></span>
                        <Hi:HiImage ID="HiImage1" runat="server" DataField="IconUrl"  CssClass="Img100_30" ImageUrl='<%# Eval("IconUrl") %>' style="display:block;"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="文章分类" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                         <asp:Label ID="lblCategoryName" Text='<%#Eval("Name") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="文章标题" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <div class="infoTitle">
                            <a href='<%# "../../ArticleDetails.aspx?ArticleId=" + Eval("ArticleId")%> ' target="_blank">
                               <span style="word-break:break-all;"> <asp:Literal ID="lblArticleTitle" Text='<%#Eval("Title") %>' runat="server"></asp:Literal></span></a></div>
                    </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="立即发布" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <div class="infoTitle">
                        <asp:LinkButton ID="btnrelease" runat="server" CommandName="Release" CommandArgument='<%# Eval("IsRelease") %>'><%#Eval("IsRelease").Equals(false)?"<img alt='点击发布' src='../images/ta.gif' />" : "<img alt='点击取消' src='../images/iconaf.gif' />"%></asp:LinkButton>
                            </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="添加时间" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <Hi:FormatedTimeLabel ID="lblAddedDate" Time='<%#Eval("AddedDate") %>' runat="server"></Hi:FormatedTimeLabel>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass=" td_left td_right_fff">
                     <ItemTemplate>
                         <span class="submit_bianji"><a href='<%# "../../admin/comment/EditArticle.aspx?ArticleId=" + Eval("ArticleId")%> ' class="SmallCommonTextButton">编辑</a></span>
                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbtnDeleteSelect" CommandName="Delete" runat="server" IsShow="true" CssClass="SmallCommonTextButton" Text="删除" /></span>
                        <span class="submit_bianji"> <a href="javascript:DialogFrame('<%# "comment/RelatedArticleProduct.aspx?ArticleId=" + Eval("ArticleId") %>','文章相关商品',null,null);">相关商品</a></span>
                     </ItemTemplate>
                 </asp:TemplateField>
            </Columns>
        </UI:Grid>        
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
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
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
      
</asp:Content>



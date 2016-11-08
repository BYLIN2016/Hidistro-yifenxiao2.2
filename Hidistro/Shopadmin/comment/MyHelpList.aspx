<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyHelpList.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyHelpList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
			<li class="menucurrent"><a href="MyHelpList.aspx"><span>帮助管理</span></a></li>
			<li class="optionend"><a href="MyHelpCategories.aspx"><span>帮助分类管理</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		
		<!--结束-->
	    <div class="Pa_15">
          <a href="AddMyHelp.aspx" class="submit_jia">添加新帮助</a>
	    </div>
        
	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
                
				<li><span>关键字：</span><span><asp:TextBox ID="txtkeyWords" runat="server" CssClass="forminput" /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:DistributorHelpCategoryDropDownList ID="dropHelpCategory" NullToDisplay="全部" runat="server"/>
					</abbr>
				</li>
                <li><span>选择时间段：</span><span><UI:WebCalendar runat="server" CalendarType="StartDate" CssClass="forminput" ID="calendarStartDataTime" /></span>
                <span class="Pg_1010">至</span><span><UI:WebCalendar runat="server"  CalendarType="EndDate"  CssClass="forminput" ID="calendarEndDataTime" IsStartTime="false"/></span></li>
				<li><asp:Button ID="btnSearch" runat="server" CssClass="searchbutton"　Text="查询" /></li>
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
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
				</div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" IsShow="true" runat="server" >删除</Hi:ImageLinkButton></span>
				</ul>
			</div>
	  </div>
		
		<!--数据列表区域-->
	  <div class="datalist">
		    <UI:Grid ID="grdHelpList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="HelpId" SortOrderBy="AddedDate" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                 <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left" />
                 <asp:TemplateField HeaderText="分类名称" HeaderStyle-CssClass="td_right td_left">
                       <ItemTemplate>
                           <Hi:SubStringLabel id="lblHelpCategory" Field="Name" StrLength="60" StrReplace="..." runat="server"></Hi:SubStringLabel>
                        </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="主题" HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                       <Hi:DistributorDetailsHyperLink ID="lkh" DetailsId='<%# Eval("HelpId") %>' Title='<%#  Eval("Title")%>' DetailsPageUrl="/HelpDetails.aspx?HelpId=" runat="server" />                          
                        </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="开始时间"  ItemStyle-Width="20%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel id="lblAddedDate" Time='<%#Eval("AddedDate") %>' runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff" >
                     <ItemTemplate>
                         <span class="submit_bianji"><a href='<%# "EditMyhelp.aspx?HelpId=" + Eval("HelpId")%> '>编辑</a></span>
                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" IsShow="true" Text="删除"  CommandName="Delete" runat="server" /></span>
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
			<div class=pagination>
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
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

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>


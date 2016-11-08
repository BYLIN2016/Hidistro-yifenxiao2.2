<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageLogs.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageLogs" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth databody">
    <div class="title">
		  <em><img src="../images/02.gif" width="32" height="32" /></em>
          <h1><strong>操作日志</strong></h1>
          <span>查看各个管理员的历史操作记录 </span>
		</div>

    <!--数据列表区域-->
    <div class="datalist">
	
    <!--搜索-->
    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
		    <li><span>操作人：</span><span><Hi:LogsUserNameDropDownList ID="dropOperationUserNames" runat="server"/></span></li>
				
                <li><span>选择时间段：</span><span><UI:WebCalendar runat="server" CalendarType="StartDate" CssClass="forminput" ID="calenderFromDate" /></span><span class="Pg_1010">至</span><span><UI:WebCalendar runat="server"  CalendarType="EndDate"  CssClass="forminput" ID="calenderToDate" IsStartTime="false"/></span></li>
				<li><asp:Button ID="btnQueryLogs" runat="server" class="searchbutton" Text="查询" /></li>
			</ul>
	  </div>
	  	 <div class="searcharea clearfix br_search">
			 <ul>
				<li>
					<span class="submit_dalata"><Hi:ImageLinkButton ID="lkbDeleteAll" runat="server" Text="清空日志" IsShow="true" DeleteMsg="确定要删除所有操作日志吗？删除后将不可恢复。"/></span>
			   </li>
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
      <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbDeleteCheck1" runat="server" Text="删除" IsShow="true"/></span>
					</li>
        </ul>
      </div>
    </div>
     <asp:DataList ID="dlstLog" runat="server" DataKeyField="LogId" Style="width: 100%;">
     <HeaderTemplate>
      <table width="0" border="0" cellspacing="0" >
        <tr class="table_title">
          <td colspan="2" class="td_right td_left">选择</td>
          <td width="18%" class="td_right td_left">详情</td>
          <td width="23%" class="td_left td_right_fff">操作</td>
        </tr>
     </HeaderTemplate>
     <ItemTemplate>
      <tr  class="td_bg">
          <td width="3%"><label>
           <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("LogId") %>'>
          </label></td>
          <td width="56%">用户名： <asp:Literal runat="server" ID="litUserName" Text=' <%#Eval("UserName")%>' /></td>
          <td >用户IP地址：<asp:Literal runat="server" ID="litIp" Text=' <%#Eval("IPAddress")%>' /></td>
          <td>添加时间：<Hi:FormatedTimeLabel runat="server" ID="time" Time='<%# Eval("AddedTime")%>' /></td>
        </tr>
         <tr>
          <td colspan="3"><span>页面地址：<asp:Literal runat="server" ID="litPageUrl" Text=' <%#Eval("PageUrl")%>' /></span>
          日志的详细描述：<abbr class="colorG"><asp:Literal runat="server" ID="litDescription" Text=' <%#Eval("Description")%>' /></abbr></td>
          <td><span class="submit_shanchu"><Hi:ImageLinkButton  ID="lkbDelete" IsShow="true" runat="server"  CommandName="Delete"  Text="删除" /></span></td>
        </tr>
     </ItemTemplate>
     <FooterTemplate>
             </table>
     </FooterTemplate>
     </asp:DataList>
      

    </div>
    <!--数据列表底部功能区域-->
<div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
						<span class="bottomSignicon"></span>
						<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
						<span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span>
					<span class="delete"><Hi:ImageLinkButton ID="lkbDeleteCheck" runat="server" Text="删除" IsShow="true"/></span></li>
				</ul>
			</div>
		</div>
    <div class="bottomPageNumber clearfix">
      <div class="pageNumber"> 
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
        </div>
        </div>
    </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>


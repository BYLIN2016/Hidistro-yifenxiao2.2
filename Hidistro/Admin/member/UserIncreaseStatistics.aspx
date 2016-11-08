<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.UserIncreaseStatistics" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
<div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>会员增长统计</h1>
  <span>会员增长报表查询</span>
</div>
	      <!--数据列表区域-->
    
	      <div class="datalist">
             
	        <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li><span class="colorG">最近7天客户增长值</span></li>
              </ul>
            </div>
	        <div class="Pg_8 Pg_20"><img id="imgChartOfSevenDay" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;" /></div>
	        


	        <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li><span class="colorG">按月查看客户增长( <strong><asp:Literal ID="litlOfMonth" runat="server" ></asp:Literal></strong> )</span> </li>
	            <li> <abbr class="formselect">
	              <Hi:YearDropDownList ID="drpYearOfMonth" runat="server" />
	              </abbr> </li>
	            <li> <abbr class="formselect">
	             <Hi:MonthDropDownList ID="drpMonthOfMonth" runat="server" />
	              </abbr> </li>
	            <li>
	               <asp:Button ID="btnOfMonth" runat="server" class="searchbutton" Text="查询"/>
                </li>
              </ul>
            </div>
	        <div class="Pg_8 Pg_20"><img id="imgChartOfMonth" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;" /></div>
	        
            
            <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li><span class="colorG">按年查看客户增长( <strong><asp:Literal ID="litlOfYear" runat="server" ></asp:Literal></strong> )</span></li>
	            <li> <abbr class="formselect">
	              <Hi:YearDropDownList ID="drpYearOfYear" runat="server" />
	              </abbr></li>
	            <li>
	              <asp:Button ID="btnOfYear" runat="server" Text="查询" class="searchbutton" />
                </li>
              </ul>
            </div>
	        <div class="Pg_8 Pg_20"><img id="imgChartOfYear" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;"/></div>
	      
          </div>
	    <!--数据列表底部功能区域-->
	  <div class="page"></div>

</div>                                   
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="UnderlingIncreaseStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.UnderlingIncreaseStatistics" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>            
             <li class="optionstar"><a href="UnderlingRanking.aspx" class="optionnext"><span>会员消费排行</span></a></li>
         <li class="menucurrent"><a href="#"><span class="optioncenter">会员增长查询</span></a></li>
        <li class="optionend"><a href="UnderlingArealDistributionStatistics.aspx"><span>会员地区分布</span></a></li>
		</ul>
</div>
<div class="dataarea mainwidth">
		<!--搜索-->
		<!--结束-->
	  <!--数据列表区域-->
	    <div>
	      <h3 class="a_none">客户增长</h3>
	      <!--数据列表区域-->
	      <div class="datalist">
	        <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li class="a_none"><span class="colorG">最近7天客户增长值</span></li>
              </ul>
            </div>
	        <div class="Pg_8 Pg_20"><img id="imgChartOfSevenDay" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;" /></div>
	        
	        <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li class="a_none"><span class="colorG">按月查看客户增长( <strong><asp:Literal ID="litlOfMonth" runat="server" ></asp:Literal></strong> )</span> </li>
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
	            <li class="a_none"><span class="colorG">按年查看客户增长( <strong><asp:Literal ID="litlOfYear" runat="server" ></asp:Literal></strong> )</span></li>
	            <li> <abbr class="formselect">
	              <Hi:YearDropDownList ID="drpYearOfYear" runat="server" />
	              </abbr></li>
	            <li>
	              <asp:Button ID="btnOfYear" runat="server" Text="查询" class="searchbutton" />
                </li>
              </ul>
            </div>
	        <div class="Pg_8"><img id="imgChartOfYear" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;"/></div>
	      </div>
      </div>
	    <!--数据列表底部功能区域-->
	  <div class="page"></div>

</div>                          
<%--<div class="areacolumn clearfix">
<div class="columnleft2">
			<div class="columnleftmenu2 clearfix">
				<ul>
				    <li><a href="UnderlingRanking.aspx"><span>会员排行</span></a></li> 
				    <li class="itempitchon"><span>会员分析统计</span></li>           			                   
				</ul>
			</div>
			<div class="columnleftbottom2 clearfix"></div>
		</div>
		<div class="columnright">
        <h1>客户分析统计<span class="spanF">客户所在地的分布统计以及客户增长报表查询</span></h1>
        
        <div class="subnav"  style="margin-top:20px;">
          	<ul>
          	    <li class="subnav_a subnav_a_span"><span class="subnav_padding">客户增长</span></li>
            	<li class="subnav_link"><a href="UnderlingArealDistributionStatistics.aspx"><span class="subnav_padding">客户分布</span></a></li>       	    	
            </ul>
          </div>     
        <h2 style="margin-top:20px;">客户增长</h2>  
        <div>
            <div style="margin-top:10px; margin-left:10px;">最近七天客户增长</div>
            <div style="margin-top:10px;margin-left:10px;">
                <img id="imgChartOfSevenDay" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;" />
            </div>
            <div style="margin-top:10px;">
                <span style="float:left;">按月查看客户增长( <strong><asp:Literal ID="litlOfMonth" runat="server" ></asp:Literal></strong> )</span>
                <span >
                    <Hi:YearDropDownList ID="drpYearOfMonth" runat="server" />年<Hi:MonthDropDownList ID="drpMonthOfMonth" runat="server" />
                    月 <asp:Button ID="btnOfMonth" runat="server" class="submit54" Text="查询"/></span>
            </div>
            <div style="margin-top:10px; margin-left:10px;">
                <img id="imgChartOfMonth" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;" />
            </div>
                <div style="margin-top:10px;">
                    <span style="float:left;">按年查看客户增长( <strong><asp:Literal ID="litlOfYear" runat="server" ></asp:Literal></strong> )</span>
                    <span ><Hi:YearDropDownList ID="drpYearOfYear" runat="server" />年
                    <asp:Button ID="btnOfYear" runat="server" Text="查询" class="submit54" /></span>
                </div>
            <div style="margin-top:10px; margin-left:10px;">
                <img id="imgChartOfYear" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;"/>
            </div>
        </div>   
        
       
   </div>  
   </div>                        --%>                 
</asp:Content>

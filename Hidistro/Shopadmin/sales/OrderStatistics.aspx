<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="OrderStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.OrderStatistics"  %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
	<div class="optiongroup mainwidth">
	  <ul>
        <li class="optionstar"><a href="SaleReport.aspx" class="optionnext"><span>生意报告</span></a></li>
        <li class="menucurrent"><a href="#"><span class="optioncenter">订单统计</span></a></li>
	    <li><a href="SaleDetails.aspx"><span>销售明细</span></a></li>
	    <li><a href="SaleTargets.aspx"><span>销售指标分析</span></a></li>
	    <li><a href="../product/ProductSaleRanking.aspx"><span>销售排行</span></a></li>
	    <li class="optionend"><a href="../product/ProductSaleStatistics.aspx"><span>商品购买与访问次数</span></a></li>
      </ul>
</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		<!--结束-->
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
		    <li><span>会员名：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"></asp:TextBox></span></li>
            <li><span>收货人：</span><span><asp:TextBox ID="txtShipTo" runat="server" CssClass="forminput"></asp:TextBox></span></li>
            <li><span>订单号：</span><span><asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput"></asp:TextBox></span></li>
				</ul>
				
				<ul>
     <li><span>选择时间段：</span><span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput"/></span><span class="Pg_1010">至</span><span><UI:WebCalendar ID="calendarEndDate" CssClass="forminput" runat="server" CalendarType="EndDate"/></span></li>
				<li> <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/></li>
				<li><p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p></li>
			</ul>
	  </div>
      <div class="blank12 clearfix"></div>
	    <!--数据列表区域-->
	  <div class="datalist">
	   <UI:Grid ID="grdUserOrderStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                            <asp:BoundField HeaderText="订单号" DataField="OrderId" HeaderStyle-CssClass="td_right td_left"/>    
                            <asp:TemplateField HeaderText="下单时间" HeaderStyle-CssClass="td_right td_left">
                                <itemtemplate>
                                    <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' runat="server" ></Hi:FormatedTimeLabel>
                                </itemtemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField HeaderText="总订单金额" HeaderStyle-CssClass="td_right td_left">
                                <itemtemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("Total") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </itemtemplate>
                            </asp:TemplateField>                                         
                            <asp:BoundField HeaderText="用户名" DataField="UserName" HeaderStyle-CssClass="td_right td_left"/>   
                            <asp:BoundField HeaderText="收货人" DataField="ShipTo" HeaderStyle-CssClass="td_right td_left"/>                                  
                            <asp:TemplateField HeaderText="利润" HeaderStyle-CssClass="td_left td_right_fff">
                                <itemtemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%#Eval("Profits") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </itemtemplate>
                            </asp:TemplateField>   
                            
                    </Columns>
                </UI:Grid>
      
      <div class="blank12 clearfix"></div>
      <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="Pg_10 clearfix">
					 当前页总计：<strong class="colorG fonts"><asp:Label ID="lblPageCount"  runat="server"></asp:Label></strong></li>
					 </ul>
					 <ul>
                    <li  class="Pg_10 clearfix">
					 当前查询结果合计：<span class="colorB fonts"><asp:Label ID="lblSearchCount"  runat="server"></asp:Label></li></span>
				</ul>
			</div>
		</div>
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
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

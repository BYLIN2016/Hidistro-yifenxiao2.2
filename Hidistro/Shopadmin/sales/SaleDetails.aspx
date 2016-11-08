<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SaleDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SaleDetails"  %>
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
	    <li class="optionstar"><a href="SaleReport.aspx"><span>生意报告</span></a></li>
        <li><a href="OrderStatistics.aspx" class="optionnext"><span>订单统计</span></a></li>
        <li class="menucurrent"><a href="#"><span class="optioncenter">销售明细</span></a></li>
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
		 
          <li><span>成交时间段：</span><span><UI:WebCalendar ID="calendarStart" runat="server" class="forminput"/></span><span class="Pg_1010">至</span><span><UI:WebCalendar ID="calendarEnd" runat="server" class="forminput" IsStartTime="false" /></span></li>
				<li><asp:Button ID="btnQuery" runat="server" Text="查询" class="searchbutton" /></li>
			</ul>
	  </div>
      <div class="blank12 clearfix"></div>
	    <!--数据列表区域-->
	  <div class="datalist">
	    <asp:GridView ID="grdOrderLineItem" runat="server"  AutoGenerateColumns="false" ShowHeader="true" AllowSorting="true"  GridLines="None" HeaderStyle-CssClass="table_title" >                                                        
                                    <Columns>  
                                     <asp:BoundField HeaderText="订单号" DataField="OrderId" ItemStyle-Width="25%" HeaderStyle-CssClass="td_right td_left" />                                                                                                                                                                                                                                                                                                                                                                
                                        <asp:BoundField HeaderText="商品名称" DataField="ProductName" HeaderStyle-CssClass="td_right td_left"/>    
                                       
                                        <asp:BoundField HeaderText="数量" DataField="Quantity" ItemStyle-Width="25%" HeaderStyle-CssClass="td_right td_left" />
                                        <Hi:MoneyColumnForAdmin HeaderText="价格" DataField="ItemAdjustedPrice"  HeaderStyle-CssClass="td_right td_left"/>
                                        <asp:TemplateField HeaderText="成交时间" HeaderStyle-CssClass="td_left td_right_fff" >
                                            <ItemTemplate>
                                                <Hi:FormatedTimeLabel ID="litDateTime" Time='<%# Eval("orderDate") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>    
                                    </Columns>
                            </asp:GridView>
     
      <div class="blank12 clearfix"></div>
     
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

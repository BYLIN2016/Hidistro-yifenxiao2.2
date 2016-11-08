<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DistributionProductSaleRanking.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributionProductSaleRanking" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

	<!--选项卡-->

  <div class="dataarea mainwidth databody">
  	<div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>商品销售排行</h1>
  <span>统计出一段时间内分销采购单中的商品销售量和销售额排行．默认排序为商品的销售量从高到低</span>
</div>
		<!--结束-->
      <div>
        <!--数据列表区域-->
	  <div class="datalist">
      <div class="searcharea clearfix br_search">
          <ul class="a_none_left">
           <li><span>起始日期：</span><span><UI:WebCalendar CalendarType="StartDate" CssClass="forminput" ID="calendarStartDate" runat="server"  /></span></li>
           <li><span>终止日期：</span><span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" /></span></li>
            <li><asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/></li>
             <li><p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告"/></p></li>
          </ul>
      </div>
	  <UI:Grid ID="grdProductSaleStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                                                                           
                             <asp:TemplateField HeaderText="排行" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("IDOfSaleTotals"))==1?"../images/0001.gif":Convert.ToInt32(Eval("IDOfSaleTotals"))==2?"../images/0002.gif":"../images/0003.gif" %>'
                                     Visible='<%#Convert.ToInt32(Eval("IDOfSaleTotals"))<4 %>' /> <strong><%#Eval("IDOfSaleTotals")%></strong>
                                </ItemTemplate>
                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                                                                           
                            <asp:BoundField HeaderText="商品名称" DataField="ProductName" HeaderStyle-CssClass="td_right td_left"/>   
                             <asp:TemplateField HeaderText="商家编码" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuyCount"  runat="Server" Text='<%# Eval("SKU") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>   
                              <asp:TemplateField HeaderText="销售量" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuyCount"  runat="Server" Text='<%# Eval("ProductSaleCounts") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>  
                               <asp:TemplateField HeaderText="销售额" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("ProductSaleTotals") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </ItemTemplate>
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="利润" HeaderStyle-CssClass="td_right td_right_fff">
                                <ItemTemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%# Eval("ProductProfitsTotals") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </ItemTemplate>
                            </asp:TemplateField>   
                    </Columns>
                </UI:Grid>
      
	  </div>
     </div>
     <div class="blank12 clearfix"></div>
     <div class="page">
	     <div class="bottomPageNumber clearfix">
			<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
            </div>
	    </div>
	</div>
	<div class="blank12 clearfix"></div>
    </div>
   <div class="blank12 clearfix"></div>
    
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.MemberRanking" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
<div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>会员消费排行</h1>
  <span>查询有成交记录的会员的订单数和购物金额,并按购物金额从高到低排行</span>
</div>
	
	    <!--数据列表区域-->
	  <div class="datalist">
      	<!--搜索-->
		<!--结束-->
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
             <li><span>起始日期：</span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" class="forminput" /></li>
            <li><span>终止日期：</span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" class="forminput" /></li>
             <li><span>排行方式：</span><span class="formselect"><asp:DropDownList runat="server" ID="ddlSort" style="width:150px;"/></span></li>
		    <li>
				<asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
            </li>
            <li>
                <p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p>
			</li>
			
			</ul>
	  </div>
      <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea "style="float:left;">				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>					
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
			</div>
			<!--结束-->
	  </div>
	    <UI:Grid ID="grdProductSaleStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                                                                             
                             <asp:TemplateField HeaderText="排行" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("IndexId"))==1?"../images/0001.gif":Convert.ToInt32(Eval("IndexId"))==2?"../images/0002.gif":"../images/0003.gif" %>'
                                     Visible='<%#Convert.ToInt32(Eval("IndexId"))<4 && Convert.ToDecimal(Eval("SaleTotals"))>0 %>' />
                                   <strong><asp:Literal ID="Label1"  runat="Server" Text='<%#Eval("IndexId")%>' Visible='<%# Convert.ToDecimal(Eval("SaleTotals"))>0 %>' /></strong>
                                </ItemTemplate>
                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                                                                           
                             <asp:TemplateField HeaderText="会员" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="Label2"  runat="Server" Text='<%# Eval("UserName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="订单数" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="Label3"  runat="Server" Text='<%# Eval("OrderCount") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>    
                               <asp:TemplateField HeaderText="消费金额" HeaderStyle-CssClass="table_title">
                                <ItemTemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("SaleTotals") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </ItemTemplate>
                            </asp:TemplateField>
                                        
                    </Columns>
                </UI:Grid>
                
	    
	    <div class="blank12 clearfix"></div>
     
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
      </div>

</div>    
</asp:Content>


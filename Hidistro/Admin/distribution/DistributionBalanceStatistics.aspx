<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.DistributionBalanceStatistics" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
      <em><img src="../images/04.gif" width="32" height="32" /></em>
      <h1>分销商预付款报表</h1>
      <span>统计分销商预付款信息</span>
  </div>

	    <!--数据列表区域-->
	  <div class="datalist">
      		<!--搜索-->
		<!--结束-->
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
			<li>
			    <span>用户名：</span><asp:TextBox ID="txtUserName" runat="server" class="forminput"></asp:TextBox>
			</li>
             <li><span>交易时间从：</span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" class="forminput" /></li>
            <li><span>至：</span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate" class="forminput" /></li>
		    <li>
				<asp:Button ID="btnQueryBalanceDetails" runat="server" Text="查询" class="searchbutton"/>
            </li>
            <li>
                <p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p>
			</li>
			</ul>
	  </div>
      <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea" style="float:left;">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
		
			</div>
			
			<div class="pageNumber" style="float:right;">
					<div class="pagination">
              <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
			</div>
			<!--结束-->
	  </div>
	     <UI:Grid ID="grdBalanceDetails" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="用户名" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("UserName")%>
                            </ItemTemplate>
                        </asp:TemplateField>		                		    			      
	                    <asp:TemplateField HeaderText="交易时间" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <Hi:FormatedTimeLabel ID="litDateTime" Time='<%# Eval("TradeDate") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
	                    <asp:TemplateField HeaderText = "业务摘要" HeaderStyle-CssClass="td_right td_left">
	                        <ItemTemplate>			               
	                            <Hi:TradeTypeNameLabel ID="lblTradeType" runat="server"  TradeType="TradeType" />			         
	                        </ItemTemplate>
	                    </asp:TemplateField>
	                    <Hi:MoneyColumnForAdmin HeaderText="转入金额" DataField="Income" HeaderStyle-CssClass="td_right td_left"/>
	                    <Hi:MoneyColumnForAdmin HeaderText="转出金额" DataField="Expenses" HeaderStyle-CssClass="td_right td_left"/>
	                    <Hi:MoneyColumnForAdmin HeaderText="当前余额" DataField="Balance" HeaderStyle-CssClass="td_right td_left"/>
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


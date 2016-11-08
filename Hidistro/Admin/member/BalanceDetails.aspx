<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BalanceDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1 class="title_height"><strong class="colorE">“<asp:Literal ID="litUser" runat="server"></asp:Literal>”</strong> 会员账户明细</h1>
  </div>		
  <div class="VIPbg fonts">
  <ul>
     <li>预付款总额：<strong class="colorG"><asp:Literal ID="litBalance" runat="server" /></strong></li>
     <li>可用余额：<strong class="colorB"><asp:Literal ID="litUserBalance" runat="server" /></strong></li>
     <li>冻结金额：<strong class="colorQ"><asp:Literal ID="litDrawBalance" runat="server" /></strong></li>
     <li><asp:LinkButton runat="server" ID="lbtnDrawRequest" Text="查看提现记录" /></li>
  </ul>
  </div>
  <div class="searcharea clearfix">
	  <ul>
	    <li>
                <span>选择时间段：</span>
                <span><UI:WebCalendar CalendarType="StartDate" CssClass="forminput" ID="calendarStart" runat="server" /></span>
                <span class="Pg_1010">至</span>
                <span><UI:WebCalendar CalendarType="EndDate" CssClass="forminput" ID="calendarEnd" runat="server" /></span>
        </li>
        <li>    
            <span>类型：</span>
            <Hi:TradeTypeDropDownList ID="dropTradeType" runat="server" />
        </li>
		<li>
		    <span><asp:Button ID="btnQueryBalanceDetails" runat="server" class="searchbutton" Text="查询" /></span>
		</li>
			</ul>
	</div>		
<!--结束-->		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
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
</div>
		<!--数据列表区域-->
		<div class="datalist">
		    <UI:Grid ID="grdBalanceDetails" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                            <Columns>
                                <asp:BoundField HeaderText="流水号" DataField="JournalNumber" HeaderStyle-CssClass="td_right td_left" />	
                           		<asp:TemplateField HeaderText="用户名" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                       <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/EditMember.aspx?userId={0}", Eval("UserId")))%>'><asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label></a>
                                    </ItemTemplate>
                               </asp:TemplateField>	  		                		    			      
			                    <asp:TemplateField HeaderText="时间" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                        <Hi:FormatedTimeLabel ID="lblTradeDate" Time='<%#Eval("TradeDate")%>' runat="server" ></Hi:FormatedTimeLabel>
                                    </ItemTemplate>
                                </asp:TemplateField>
			                    <asp:TemplateField HeaderText = "类型" HeaderStyle-CssClass="td_right td_left" >
			                        <ItemTemplate>			               
			                            <Hi:TradeTypeNameLabel ID="lblTradeType" runat="server"  TradeType="TradeType" />
			                        </ItemTemplate>
			                    </asp:TemplateField>
			                    <Hi:MoneyColumnForAdmin HeaderText="收入" DataField="Income" HeaderStyle-CssClass="td_right td_left"/>
			                    <Hi:MoneyColumnForAdmin HeaderText="支出" DataField="Expenses" HeaderStyle-CssClass="td_right td_left" />
			                    <Hi:MoneyColumnForAdmin HeaderText="账户余额" DataField="Balance" HeaderStyle-CssClass="td_right td_left" />	
			                    <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_left td_right_fff" >
                                    <itemtemplate>
                                        <%# Globals.HtmlEncode(Eval("Remark").ToString())%>
                                    </itemtemplate>
                                </asp:TemplateField> 		      	        
	                        </Columns> 
	                      </UI:Grid>		
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
		</div>
	</div>
     
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyBalanceDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyBalanceDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="toptitle"><em><img src="../images/09.gif" width="32" height="32" /></em>
    <h1 >账户明细</h1>
	    <span >查看自己的预付款帐户明细.</span>
</div>
<div class="searcharea clearfix br_search">
  <ul>
    <li> <span>起始日期：</span>
      <UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" CssClass="forminput"/>
      </li>
    <li> <span>终止日期：</span>
     <UI:WebCalendar CalendarType="EndDate" ID="calendarEnd" runat="server" CssClass="forminput"/>
    </li>
    <li>    
            <span>类型：</span>
            <Hi:TradeTypeDropDownList ID="dropTradeType" runat="server" IsDistributor="true" />
        </li>
    <li>
      <asp:Button ID="btnQueryBalanceDetails" runat="server" class="searchbutton" Text="查询" />
      </li>
    </ul>
</div>
<div class="functionHandleArea m_none">
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
</div>
<div class="datalist">
 <UI:Grid ID="grdBalanceDetails" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" CssClass="table_content"  HeaderStyle-CssClass="table_title" SortOrder="DESC" >
                            <Columns>
                                <asp:BoundField HeaderText="业务流水号" DataField="JournalNumber" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%" />                            		                		    			      
			                    <asp:TemplateField HeaderText="日期" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <Hi:FormatedTimeLabel ID="lblTradeDate" Time='<%#Eval("TradeDate")%>' runat="server" ></Hi:FormatedTimeLabel>
                                    </ItemTemplate>
                                </asp:TemplateField>
			                    <asp:TemplateField HeaderText = "类型" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
			                        <ItemTemplate>			               
			                            <Hi:TradeTypeNameLabel ID="lblTradeType" IsDistributor="true" runat="server"  TradeType="TradeType" />
			                        </ItemTemplate>
			                    </asp:TemplateField>
			                    <Hi:MoneyColumnForAdmin HeaderText="收入" DataField="Income" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%"/>
			                    <Hi:MoneyColumnForAdmin HeaderText="支出" DataField="Expenses" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%" />
			                    <Hi:MoneyColumnForAdmin HeaderText="账户总额" DataField="Balance" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="10%" />
                                <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="35%" >
                                    <itemtemplate><%# Globals.HtmlEncode(Eval("Remark").ToString())%></itemtemplate>
                                    </asp:TemplateField>
	                        </Columns> 
	          </UI:Grid>
  
  <div class="blank12 clearfix"></div>
</div>
<div class="bottomPageNumber clearfix">
	    <div class="pageNumber"> <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
       </div> </div>
  </div>
</div>
<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SaleReport" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->

	<!--选项卡-->

	<div class="dataarea mainwidth databody">
	  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>零售生意报告</h1>
  <span>查看网店生意情况，您可以按月或按日分别查看店铺订单交易量、交易额和销售利润（需要设置商品成本价）</span>
</div>
		<!--搜索-->
      <div class="btn"><h3>按月统计</h3></div>
		<!--结束-->
      <div>

		
		<!--数据列表区域-->
	  <div class="datalist">
      	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
		    <li><span>请选择：</span>
					<abbr class="formselect">
						<Hi:YearDropDownList ID="dropMonthForYaer" runat="server" />
					</abbr>
                    <samp class="colorR">&nbsp;年</samp>
				</li>		
                		
             <li  style="width:300px;"><span id="SaleStatis2"><Hi:SaleStatisticsTypeRadioButtonList ID= "radioMonthForSaleType" runat="server" /></span></li>
             
		     <li> <asp:Button ID="btnQueryMonthSaleTotal" runat="server" class="searchbutton" Text="查询" /></li>
             <li><p><asp:LinkButton ID="btnCreateReportOfMonth" runat="server" Text="生成报告"/></p></li>
			</ul>
	  </div>

<div class="functionHandleArea clearfix m_none">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum Pg_10"><asp:Label ID="lblMonthAllTotal" runat="server"></asp:Label><strong class="colorA"><asp:Literal ID="litMonthAllTotal" runat="server"></asp:Literal></strong></li>
                    <li class="paginalNum"><asp:Label ID="lblMonthMaxTotal" runat="server"></asp:Label><strong class="colorA"><asp:Literal ID="litMonthMaxTotal" runat="server"></asp:Literal></strong></li>
				</ul>
			</div>

</div>
	  <UI:Grid ID="grdMonthSaleTotalStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        
                        <asp:BoundField HeaderText="月份" DataField="Date" HeaderStyle-CssClass="td_right td_left"/>   
                        <asp:BoundField HeaderText="月销售额"  DataField="SaleTotal"  HeaderStyle-CssClass="td_right td_left"/> 
                        <Hi:MoneyColumnForAdmin HeaderText="月销售额" DataField="SaleTotal" HeaderStyle-CssClass="td_right td_left"/>
                        <asp:TemplateField HeaderText="比例示意图"  HeaderStyle-Width="60%"  HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <img width='<%# string.Format("{0}px", Eval("Lenth")) %>' height="15" class="votelenth"/><asp:Literal ID="lblPercentage" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.Percentage", "{0:N2}") %>' ></asp:Literal>%
                            </ItemTemplate>
                        </asp:TemplateField>   
                        
                    </Columns>
                </UI:Grid>
      
	  </div>
     </div>
        <div>
        <div class="btn"><h3>按日统计</h3></div>        
          <!--数据列表区域-->
          <div class="datalist">
            <div class="searcharea clearfix br_search">
            <ul class="a_none_left">
              <li><span>请选择：</span></li>
              <li> <abbr class="formselect">
                <Hi:YearDropDownList ID="dropDayForYear" runat="server" />
              </abbr><samp class="colorR">年</samp> </li>
              <li>  <abbr class="formselect">
                <Hi:MonthDropDownList ID="dropMoth" runat="server" />
              </abbr> <samp class="colorR">月</samp></li>
              <li style="width:300px"><span id="SaleStatis2">
                <Hi:SaleStatisticsTypeRadioButtonList ID= "radioDayForSaleType" runat="server" /></span></li>
              
              <li>
                <asp:Button ID="btnQueryDaySaleTotal" runat="server" Text="查询" class="searchbutton" />
              </li>
              <li>
                <p> <asp:LinkButton ID="btnCreateReportOfDay" runat="server" Text="生成报告" /></p>
              </li>
            </ul>
          </div>
          <div class="functionHandleArea clearfix m_none">
            <!--分页功能-->
            <div class="pageHandleArea">
              <ul>
                <li class="paginalNum Pg_10"><asp:Label ID="lblDayAllTotal" runat="server"></asp:Label><strong class="colorA"><asp:Literal ID="litDayAllTotal" runat="server"></asp:Literal></strong></li>
                <li class="paginalNum"><asp:Label ID="lblDayMaxTotal" runat="server"></asp:Label><strong class="colorA"><asp:Literal ID="litDayMaxTotal" runat="server"></asp:Literal></strong></li>
              </ul>
            </div>
          </div>
          <asp:GridView ID="grdDaySaleTotalStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        
                        <asp:BoundField HeaderText="日期" DataField="Date" HeaderStyle-CssClass="td_right td_left" />   
                        <asp:BoundField HeaderText="月销售额" DataField="SaleTotal"  HeaderStyle-CssClass="td_right td_left"/> 
                        <Hi:MoneyColumnForAdmin HeaderText="月销售额" DataField="SaleTotal" HeaderStyle-CssClass="td_right td_left"/>
                        <asp:TemplateField HeaderText="比例示意图"  HeaderStyle-Width="60%" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <img width='<%# string.Format("{0}px", Eval("Lenth")) %>' height="15" class="votelenth"/><asp:Literal ID="lblPercentage" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.Percentage", "{0:N2}") %>' ></asp:Literal>%
                            </ItemTemplate>
                        </asp:TemplateField>    
                        
                    </Columns>
                </asp:GridView>
            
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


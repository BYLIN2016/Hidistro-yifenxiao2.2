<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SaleTargets" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
				  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>销售指标分析</h1>
  <span>查询网店的销售指标(注：其中订单数指付过款的订单数；订单总金额指付过款的订单金额减去成功退款的订单金额)。</span>
</div>
		<!--搜索-->
		<!--结束-->
      <div>
		<!--数据列表区域-->
	  <div class="datalist">
              
<div class="functionHandleArea a_none clearfix">
			<!--分页功能-->
		  <div class="pageHandleArea">
				<ul>
					<li><strong class="fonts colorQ paihang">平均每位客户订单金额</strong></li>
				</ul>
	  </div>
		  <!--结束-->
  </div>
	   <UI:Grid ID="grdOrderAvPrice" runat="server" AutoGenerateColumns="false" ShowHeader="true" AllowSorting="true" GridLines="None"  HeaderStyle-CssClass="table_title" >                                                        
                                        <Columns>     
                                            <asp:TemplateField HeaderText="订单总金额" ItemStyle-Width="30%"  HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("OrderPrice") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                                </itemtemplate>
                                            </asp:TemplateField>   
                                            <asp:TemplateField HeaderText="总会员数"  ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%#Eval("UserNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="平均每位客户订单金额" ItemStyle-Width="30%"  HeaderStyle-CssClass="td_right td_right_fff">
                                                <itemtemplate>
                                                    <%# (Convert.ToDecimal(Eval("UserNumb"))>0?(decimal.Parse(Convert.ToString(Eval("OrderPrice")))/Convert.ToDecimal(Eval("UserNumb"))):0).ToString("C") %>
                                                </itemtemplate>
                                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                   
                                        </Columns>
                                </UI:Grid>
	   
	  </div>
     </div>
        <div>
          <!--数据列表区域-->
          <div class="datalist">
          
          <div class="functionHandleArea a_none clearfix">
            <!--分页功能-->
            <div class="pageHandleArea">
              <ul>
                <li><strong class="fonts colorQ paihang">平均每次访问订单金额</strong></li>
              </ul>
            </div>
            <!--结束-->
          </div>
          <UI:Grid ID="grdVisitOrderAvPrice" runat="server" AutoGenerateColumns="false" ShowHeader="true" AllowSorting="true" CssClass="GridViewStyle" GridLines="None" HeaderStyle-CssClass="table_title" >                                                        
                                        <Columns>     
                                            <asp:TemplateField HeaderText="订单总金额" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%#Eval("OrderPrice") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                                </itemtemplate>
                                            </asp:TemplateField>   
                                            <asp:TemplateField HeaderText="总访问次数" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%#Eval("ProductVisitNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="平均每次访问订单金额" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_right_fff">
                                                <itemtemplate>
                                                    <%# (Convert.ToDecimal(Eval("ProductVisitNumb")) > 0 ? decimal.Parse(Convert.ToString(Eval("OrderPrice"))) / Convert.ToDecimal(Eval("ProductVisitNumb")) : 0).ToString("C")%>
                                                </itemtemplate>
                                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                   
                                        </Columns>
                                </UI:Grid>
           
          </div>
      </div>
        <div>
          <!--数据列表区域-->
          <div class="datalist">
          
          <div class="functionHandleArea a_none clearfix">
            <!--分页功能-->
            <div class="pageHandleArea">
              <ul>
                <li><strong class="fonts colorQ paihang">订单转化率</strong></li>
              </ul>
            </div>
            <!--结束-->
          </div>
          <UI:Grid ID="grdOrderTranslatePercentage" runat="server" AutoGenerateColumns="false" ShowHeader="true" AllowSorting="true" GridLines="None"  HeaderStyle-CssClass="table_title" >                                                        
                                        <Columns>     
                                            <asp:TemplateField HeaderText="总订单数" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%#Eval("OrderNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>   
                                            <asp:TemplateField HeaderText="总访问次数" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%#Eval("ProductVisitNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="订单转化率" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_right_fff" >
                                                <itemtemplate>
                                                    <%# (Convert.ToDecimal(Eval("ProductVisitNumb")) > 0 ? decimal.Parse(Convert.ToString(Eval("OrderNumb"))) / Convert.ToDecimal(Eval("ProductVisitNumb")) * 100 : 0).ToString("F2")%>%
                                                </itemtemplate>
                                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                   
                                        </Columns>
                                </UI:Grid>
            
          </div>
      </div>
        <div>
          <!--数据列表区域-->
          <div class="datalist">
          
          <div class="functionHandleArea a_none clearfix">
            <!--分页功能-->
            <div class="pageHandleArea">
              <ul>
                <li><strong class="fonts colorQ paihang">注册会员购买率</strong></li>
              </ul>
            </div>
            <!--结束-->
          </div>
           <UI:Grid ID="grdUserOrderPercentage" runat="server" AutoGenerateColumns="false" ShowHeader="true" AllowSorting="true" CssClass="GridViewStyle"  HeaderStyle-CssClass="table_title" >                                                        
                                        <Columns>     
                                            <asp:TemplateField HeaderText="有过订单的会员"  ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%#Eval("UserOrderedNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>   
                                            <asp:TemplateField HeaderText="总会员数" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%#Eval("UserNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="注册会员购买率" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_right_fff">
                                                <itemtemplate>
                                                    <%# (Convert.ToDecimal(Eval("UserNumb")) > 0 ? decimal.Parse(Convert.ToString(Eval("UserOrderedNumb"))) / Convert.ToDecimal(Eval("UserNumb")) * 100 : 0).ToString("F2")%>%
                                                </itemtemplate>
                                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                   
                                        </Columns>
                                </UI:Grid>
            
          </div>
      </div>
      <div>
  <!--数据列表区域-->
  <div class="datalist">
  
        <div class="functionHandleArea a_none clearfix">
    <!--分页功能-->
    <div class="pageHandleArea">
      <ul>
        <li><strong class="fonts colorQ paihang">平均会员订单量</strong></li>
      </ul>
    </div>
    <!--结束-->
  </div>
   <UI:Grid ID="grdUserOrderAvNumb" runat="server" AutoGenerateColumns="false" ShowHeader="true" AllowSorting="true" CssClass="GridViewStyle"  HeaderStyle-CssClass="table_title" >                                                        
                                        <Columns>     
                                            <asp:TemplateField HeaderText="总订单数" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%# Eval("OrderNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>   
                                            <asp:TemplateField HeaderText="总会员数" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                                                <itemtemplate>
                                                    <%#Eval("UserNumb") %>
                                                </itemtemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="平均会员订单量" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_right_fff">
                                                <itemtemplate>
                                                    <%# (Convert.ToDecimal(Eval("UserNumb")) > 0 ? decimal.Parse(Convert.ToString(Eval("OrderNumb"))) / Convert.ToDecimal(Eval("UserNumb")) : 0).ToString("F2")%>
                                                </itemtemplate>
                                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                   
                                        </Columns>
                                </UI:Grid>
   
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


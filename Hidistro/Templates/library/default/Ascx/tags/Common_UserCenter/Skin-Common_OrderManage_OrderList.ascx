<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<UI:Grid ID="listOrders" runat="server" SortOrderBy="OrderDate" SortOrder="Desc"
    AutoGenerateColumns="False" DataKeyNames="OrderId"   AllowSorting="true" GridLines="None" Width="775px" CssClass="User_manForm" HeaderStyle-CssClass="diplayth1">
    <Columns>
        <asp:TemplateField HeaderText="订单编号">
        <itemtemplate>
            <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("OrderId") %>' ></asp:Label>
        </itemtemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="收货人/物流单号">
         <itemtemplate>                       
            <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("ShipTo") %>'></asp:Label><br />
            <%# Eval("ShipOrderNumber")%>
         </itemtemplate> 
         </asp:TemplateField>
         <asp:TemplateField HeaderText="支付方式">
         <itemtemplate>                       
            <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType") %>'></asp:Label>
         </itemtemplate> 
         </asp:TemplateField>
         <asp:TemplateField HeaderText="金额">
         <itemtemplate>                       
            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("OrderTotal") %>' runat="server" />
         </itemtemplate> 
         </asp:TemplateField>
        <asp:TemplateField HeaderText="订单状态">
            <itemtemplate>
                <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" />
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="下单时间">
            <itemtemplate>
                <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true" runat="server" />
            </itemtemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="操作" ItemStyle-Width="160px">
            <itemtemplate>
            <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderReviews",Eval("orderId")) %>'>评论</asp:HyperLink>
            <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>' Text="查看" />
            <a href="javascript:void(0)" onclick="return paySelect(this)"  runat="server" oId='<%# Eval("OrderId") %>' pId='<%# Eval("PaymentTypeId") %>' ID="hlinkPay">付款</a>
            <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="确认订单" CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE"  DeleteMsg="确认已经收到货并完成该订单吗？" Visible="false" ForeColor="Red" />   
            <Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="关闭" CommandArgument='<%# Eval("OrderId") %>' CommandName="CLOSE_TRADE"  DeleteMsg="确认关闭该订单吗？" Visible="false"/>
            <a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server" ID="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>申请退款</a><br />
            <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server" ID="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>申请退货</a>
            <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server" ID="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>申请换货</a>       
            </itemtemplate>
        </asp:TemplateField>
        
    </Columns>
</UI:Grid>
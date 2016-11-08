<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<UI:Grid ID="listOrders" runat="server" SortOrderBy="OrderDate" SortOrder="Desc"
    AutoGenerateColumns="False" DataKeyNames="OrderId"   AllowSorting="true" GridLines="None" Width="775px" CssClass="User_manForm" HeaderStyle-CssClass="diplayth1">
    <Columns>
        <asp:TemplateField HeaderText="�������">
        <itemtemplate>
            <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("OrderId") %>' ></asp:Label>
        </itemtemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="�ջ���/��������">
         <itemtemplate>                       
            <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("ShipTo") %>'></asp:Label><br />
            <%# Eval("ShipOrderNumber")%>
         </itemtemplate> 
         </asp:TemplateField>
         <asp:TemplateField HeaderText="֧����ʽ">
         <itemtemplate>                       
            <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType") %>'></asp:Label>
         </itemtemplate> 
         </asp:TemplateField>
         <asp:TemplateField HeaderText="���">
         <itemtemplate>                       
            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("OrderTotal") %>' runat="server" />
         </itemtemplate> 
         </asp:TemplateField>
        <asp:TemplateField HeaderText="����״̬">
            <itemtemplate>
                <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" />
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="�µ�ʱ��">
            <itemtemplate>
                <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true" runat="server" />
            </itemtemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="����" ItemStyle-Width="160px">
            <itemtemplate>
            <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderReviews",Eval("orderId")) %>'>����</asp:HyperLink>
            <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>' Text="�鿴" />
            <a href="javascript:void(0)" onclick="return paySelect(this)"  runat="server" oId='<%# Eval("OrderId") %>' pId='<%# Eval("PaymentTypeId") %>' ID="hlinkPay">����</a>
            <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="ȷ�϶���" CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE"  DeleteMsg="ȷ���Ѿ��յ�������ɸö�����" Visible="false" ForeColor="Red" />   
            <Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="�ر�" CommandArgument='<%# Eval("OrderId") %>' CommandName="CLOSE_TRADE"  DeleteMsg="ȷ�Ϲرոö�����" Visible="false"/>
            <a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server" ID="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>�����˿�</a><br />
            <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server" ID="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>�����˻�</a>
            <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server" ID="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>���뻻��</a>       
            </itemtemplate>
        </asp:TemplateField>
        
    </Columns>
</UI:Grid>
<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<UI:Grid ID="listRefunds" runat="server" SortOrderBy="ApplyForTime" SortOrder="Desc"
    AutoGenerateColumns="False" DataKeyNames="RefundId" AllowSorting="true" GridLines="None"
    Width="744px" CssClass="User_manForm" HeaderStyle-CssClass="diplayth1">
    <Columns>
        <asp:TemplateField HeaderText="订单编号">
            <ItemTemplate>
                <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("OrderId") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="申请时间">
            <ItemTemplate>
                <asp:Label ID="lblApplyForTime" runat="server" Text='<%# Eval("ApplyForTime") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="退款方式">
            <ItemTemplate>
                <asp:Label ID="lblRefundType" runat="server" Text='<%# Eval("RefundType") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="退款留言">
            <ItemTemplate>
                <asp:Label ID="lblRefundReamrk" runat="server" Text='<%# Eval("RefundRemark") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态">
            <ItemTemplate>
                <asp:Label ID="lblHandleStatus" Text='<%# Eval("HandleStatus") %>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="处理时间">
            <ItemTemplate>
                <asp:Label ID="lblStartTimes" Text='<%#Eval("HandleTime") %>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="管理员备注">
            <ItemTemplate>
                <asp:Label ID="lblAdminRemark" Text='<%#Eval("AdminRemark") %>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="操作">
            <ItemTemplate>
                <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>'
                    Text="查看" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</UI:Grid>

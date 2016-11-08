<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:GridView ID="grdShippingMode" runat="server" CssClass="cart_Order_deliver2" AutoGenerateColumns="false" DataKeyNames="ModeId"  Width="100%" BorderWidth="0">
    <Columns>
        <asp:TemplateField HeaderText="选择" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <Hi:ListRadioButton ID="radioButton" GroupName="shippButton" runat="server" value='<%# Eval("ModeId") %>' />
            </ItemTemplate>
            <ItemStyle />
        </asp:TemplateField>
         <asp:BoundField HeaderText="配送方式" ItemStyle-Width="15%" DataField="Name" ItemStyle-HorizontalAlign="Center" />
        <asp:TemplateField HeaderText="支持物流" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
             <asp:Literal runat="server" ID="litExpressCompanyName" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="详细介绍" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <span style="word-break:break-all;"><%# Eval("Description") %></span>
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
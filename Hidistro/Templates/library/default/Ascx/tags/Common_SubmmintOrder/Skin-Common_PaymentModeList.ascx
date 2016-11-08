<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:GridView ID="grdPayment" runat="server" AutoGenerateColumns="false" DataKeyNames="Gateway"  Width="100%"  BorderWidth="0" CssClass="cart_Order_deliver2">
    <Columns>
        <asp:TemplateField HeaderText="ѡ��" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <Hi:ListRadioButton ID="radioButton" GroupName="paymentMode" runat="server" value='<%# Eval("ModeId") %>' />
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
        <asp:BoundField HeaderText="֧����ʽ" ItemStyle-Width="20%" DataField="Name" ItemStyle-HorizontalAlign="Center"  />
        <asp:TemplateField HeaderText="��ϸ����"   ItemStyle-Width="65%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <span style="word-break:break-all;"><%# Eval("Description") %></span>
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
    </Columns>
</asp:GridView>



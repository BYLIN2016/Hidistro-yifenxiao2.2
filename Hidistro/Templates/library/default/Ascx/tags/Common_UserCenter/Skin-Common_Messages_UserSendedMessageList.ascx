<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<UI:Grid ID="messagesList" runat="server" AutoGenerateColumns="False"  DataKeyNames="messageid"
    CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="false" CssClass="datalist" HeaderStyle-CssClass="diplayth1"
    Width="100%" RunningMode="Server" >
    <Columns>
        <UI:CheckBoxColumn HeaderStyle-CssClass="firstcell" ItemStyle-Width="5%"/>                 
        <asp:TemplateField HeaderText="标题" >
            <ItemTemplate>
                <%#Eval("Title")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="收件人" ItemStyle-Width="10%">
            <ItemTemplate>管理员</ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="时间" SortExpression="PublishDate" ItemStyle-Width="18%" >
            <ItemTemplate>
                <Hi:FormatedTimeLabel ID="litDateTime" Time='<%# Eval("Date")%>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="内容" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="50%">
            <ItemTemplate>
               <asp:Label ID="litPublishContent" runat="server" Text='<%#Eval("Content")%>' CssClass="line" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <RowStyle CssClass="grdrow" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
    <AlternatingRowStyle BackColor="White" />
</UI:Grid>
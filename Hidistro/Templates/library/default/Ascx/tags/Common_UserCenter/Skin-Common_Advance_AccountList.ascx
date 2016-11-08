<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:DataList ID="dataListAccountDetails" runat="server"  Width="100%">
         <HeaderTemplate>
         <table  border="0" cellpadding="0" cellspacing="0" class="datalist">
            <tr height="23" class="diplayth">
                <td class="firstcell">
                    <asp:Literal ID="litJournalNumberHead" runat="server" Text="流水号"></asp:Literal></b></td>
                <td >
                    <asp:Literal ID="litTradeDateHead" runat="server" Text="时间"></asp:Literal></b></td>
                <td >
                    <asp:Literal ID="litTradeTypeHead" runat="server" Text="类型"></asp:Literal></b></td>
                <td >
                    <asp:Literal ID="litIncomeHead" runat="server" Text="收入"></asp:Literal></b></td>
                <td >
                    <asp:Literal ID="litExpensesHead" runat="server" Text="支出"></asp:Literal></b></td>
                <td >
                    <asp:Literal ID="litBalanceHead" runat="server" Text="账户余额"></asp:Literal></b></td>
                <td >
                    <asp:Literal ID="litRemarkHead" runat="server" Text="备注"></asp:Literal></b></td>
            </tr>
         </HeaderTemplate>
         <ItemTemplate>
            <tr>
                <td >
                    <asp:Literal ID="litJournalNumber" runat="server" Text='<%#Eval("JournalNumber") %>'></asp:Literal></td>
                <td >
                    <Hi:FormatedTimeLabel ID="lblTradeDate" runat="server" Time='<%# Eval("TradeDate") %>'></Hi:FormatedTimeLabel></td>
                <td >
                    <Hi:TradeTypeNameLabel ID="lblTradeType" runat="server" TradeType="TradeType"></Hi:TradeTypeNameLabel></td>
                <td >
                    <Hi:FormatedMoneyLabel ID="lblIncome" runat="server" Money='<%# Eval("Income") %>'></Hi:FormatedMoneyLabel></td>
                <td >
                    <Hi:FormatedMoneyLabel ID="lblExpenses" runat="server" Money='<%# Eval("Expenses") %>'></Hi:FormatedMoneyLabel></td>
                <td >
                    <Hi:FormatedMoneyLabel ID="lblBalance" runat="server" Money='<%# Eval("Balance") %>'></Hi:FormatedMoneyLabel></td>
                <td style="word-break:break-all;width:260px;"><asp:Literal ID="litRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Literal></td>
            </tr>
         </ItemTemplate>
         <FooterTemplate>
         </table>
         </FooterTemplate>
         </asp:DataList>

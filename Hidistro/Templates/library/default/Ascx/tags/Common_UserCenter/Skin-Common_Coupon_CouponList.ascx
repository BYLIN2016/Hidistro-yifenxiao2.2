<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:DataList ID="dataListCoupon" runat="server"  Width="100%">
         <HeaderTemplate>
        <table  border="0" cellpadding="0" cellspacing="0" class="datalist">
            <tr height="23" class="diplayth">
                <td class="firstcell">
            
                    <asp:Literal ID="litJournalNumberHead" runat="server" Text="�Ż�ȯ���Ƽ�����"></asp:Literal></td>
                <td >
                    <asp:Literal ID="litTradeDateHead" runat="server" Text="������"></asp:Literal></td>
                <td >
                    <asp:Literal ID="litTradeTypeHead" runat="server" Text="��ֵ"></asp:Literal></td>
                         <td >
                    <asp:Literal ID="lblStartTime" runat="server" Text="��ʼ����"></asp:Literal></td>
                <td >
                    <asp:Literal ID="litIncomeHead" runat="server" Text="��Ч��(ֹ)"></asp:Literal></td>
                        <td >
                    <asp:Literal ID="Literal1" runat="server" Text="ʹ��״̬"></asp:Literal></td>
                
            </tr>
               </HeaderTemplate>
         <ItemTemplate>
            <tr>
                <td >
                    <Hi:SubStringLabel ID="lblName" Field="Name" StrLength="12" StrReplace="..  " runat="server" />
                   <div><%# Eval("ClaimCode")%></div></td>
                <td >
                    <Hi:FormatedMoneyLabel ID="lblAmount" Money='<%#Eval("Amount") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                <td >
                    <Hi:FormatedMoneyLabel ID="lblValue" Money='<%#Eval("DiscountValue") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                        <td >
                    <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%#Eval("StartTime") %>' runat="server"></Hi:FormatedTimeLabel></td>
                    
                <td >
                    <Hi:FormatedTimeLabel ID="lblClosingTime" Time='<%#Eval("ClosingTime") %>' runat="server"></Hi:FormatedTimeLabel></td>
                    <td><%# Eval("CouponStatus").ToString() == "1" ? "��ʹ��" : "δʹ��"%></td>
       
            </tr>
                      </ItemTemplate>
         <FooterTemplate>
         </table>
         </FooterTemplate>
         </asp:DataList>
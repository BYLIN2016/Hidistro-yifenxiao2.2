<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.AccountCenter.CodeBehind" Assembly="Hidistro.UI.AccountCenter.CodeBehind" %>

<asp:DataList ID="dataListPointDetails" runat="server"  Width="100%">
         <HeaderTemplate>
            <table width="100%" border="0" class="datalist" cellspacing="0" cellpadding="0" >
                  <tr class="diplayth1" >
                    <!-- ��ˮ�� -->
                    <th class="firstcell"><asp:Literal ID="Literal1" runat="server" Text="��ˮ��"></asp:Literal></th>
                    <!--�������-->
                    <th style="width:20%;"><asp:Literal ID="Literal2" runat="server" Text="�������"/></th>
                    <!-- ���� -->
                    <th style="width:20%;"><asp:Literal ID="Literal4" runat="server" Text="ʱ��"/></th>
                    <!-- ���� -->
                    <th style="width:15%;"><asp:Literal ID="Literal5" runat="server" Text="����"/></th>
                    <!-- ���� -->
                    <th style="width:10%;"><asp:Literal ID="Literal6" runat="server" Text="����"/></th>
                    <!-- ���� -->
                    <th style="width:10%;"><asp:Literal ID="Literal7" runat="server" Text="����"/></th>
                    <!-- ��ǰ���� -->
                    <th class="rightborder"  ><asp:Literal ID="Literal8" runat="server" Text="��ǰ����"/></th>
                  </tr>
                   </HeaderTemplate>
         <ItemTemplate>
                  <tr>
                    <td align=center>
                        <asp:Label ID="lblJournalNumber" runat="server" Text='<%# Eval("JournalNumber") %>'></asp:Label>
                    </td>
                    <td align=center >
                        <asp:Label ID="lblOrderId" runat="server" Text='<%# string.IsNullOrEmpty(Eval("OrderId").ToString())?"*":Eval("OrderId").ToString() %>'></asp:Label>
                    </td>
                    <td align=center ><Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%#Eval("TradeDate") %>' runat="server" /></td>
                    <td align=center >
                        <asp:Label ID="lblPointType" runat="server" Text='<%#Eval("TradeType") %>'></asp:Label>
                    </td>
                    <td align=center >
                        <asp:Label ID="increasedNumber" runat="server" Text='<%#Eval("Increased") %>'></asp:Label>
                    </td>
                    <td align=center >
                        <asp:Label ID="reducedNumber" runat="server" Text='<%#Eval("Reduced") %>'></asp:Label>
                    </td>
                    <td align=center class="rightborder">
                         <asp:Label ID="pointNumber" runat="server" Text='<%#Eval("Points") %>'></asp:Label>
                    </td>
                  </tr>
               </ItemTemplate>
         <FooterTemplate>
         </table>
         </FooterTemplate>
         </asp:DataList>
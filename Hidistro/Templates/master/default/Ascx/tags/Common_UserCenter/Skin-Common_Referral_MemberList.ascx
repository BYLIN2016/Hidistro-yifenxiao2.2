<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.AccountCenter.CodeBehind" Assembly="Hidistro.UI.AccountCenter.CodeBehind" %>

<asp:DataList ID="dataListPointDetails" runat="server"  Width="100%">
         <HeaderTemplate>
            <table width="100%" border="0" class="datalist" cellspacing="0" cellpadding="0" >
                  <tr class="diplayth1" >
                    <th class="firstcell">用户名</th>
                     <th style="width:20%;">等级</th>
                    <th style="width:20%;">邮件</th>
                    <th style="width:10%;">积分</th>
                    <th style="width:10%;">交易额</th>
                    <th style="width:20%;">创建日期</th>
                  </tr>
                   </HeaderTemplate>
         <ItemTemplate>
                  <tr>
                    <td align=center>
                        <%# Eval("Username") %>
                    </td>
                    <td align=center >
                        <%#Eval("GradeName") %>
                    </td>
                    <td align=center >
                        <%#Eval("Email") %>
                    </td>
                    <td align=center >
                        <%#Eval("Points") %>
                    </td>
                    <td align=center >
                        <%#Eval("Expenditure", "{0:F2}")%>
                    </td>
                    <td align=center >
                        <Hi:FormatedTimeLabel ID="litDate" runat="server" DataField="CreateDate" />
                    </td>
                  </tr>
               </ItemTemplate>
         <FooterTemplate>
         </table>
         </FooterTemplate>
         </asp:DataList>
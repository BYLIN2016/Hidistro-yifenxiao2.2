<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
        
        <UI:Grid ID="gridMessageList" runat="server" AutoGenerateColumns="False"  DataKeyNames="MessageId"
                    CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="false" 
                    Width="100%" RunningMode="Callback" CssClass="datalist" HeaderStyle-CssClass="diplayth1">
                    <Columns>
                        <UI:CheckBoxColumn HeaderStyle-CssClass="firstcell" ItemStyle-Width="5%" />
                        <asp:TemplateField HeaderText="编号" ItemStyle-Width="7%" >
                            <ItemTemplate>
                                <asp:Label runat="server" Id="lblMessage" Text='<%# Eval("MessageId") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="标题" ItemStyle-Width="45%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Eval("Title")%>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:TemplateField HeaderText="发件人" ItemStyle-Width="10%">
                            <ItemTemplate>
                                管理员
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="时间" ItemStyle-Width="18%">
                            <ItemTemplate>
                                <Hi:FormatedTimeLabel ID="litDateTime" Time='<%# Eval("Date")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="已读" SortExpression="IsRead" ItemStyle-Width="5%">
                            <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("IsRead").ToString()=="False"? "否":"是" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="15%" SortExpression="PublishDate">
                            <itemtemplate>
                                <a href='<%# Globals.ApplicationPath+(string.Format("/User/ReplyReceivedMessage.aspx?MessageId={0}", Eval("MessageId")))%> ' class="SmallCommonTextButton">回复</a>
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="grdrow" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <AlternatingRowStyle BackColor="White" />
                </UI:Grid>
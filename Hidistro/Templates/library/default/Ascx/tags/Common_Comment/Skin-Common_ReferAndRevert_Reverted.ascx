<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.AccountCenter.CodeBehind" Assembly="Hidistro.UI.AccountCenter.CodeBehind" %>

        <table cellspacing="0" border="0" width="100%" style="border-bottom: solid 1px #C0C0C0;">
            <!-- ��һ��[����] -->
            <tr>
                <td colspan="2" style="text-align: left;">
                    <span class="clewB">
                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("IndexId")%>'></asp:Label>.
                    </span>&nbsp;&nbsp;��
                    ��<span class="productMainClass">
                        <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                            ProductId='<%# Eval("ProductId") %>' runat="server" />
                    </span><strong>&nbsp;��</strong>
                    ����ѯ
                </td>
            </tr>
            <!-- �ڶ���[��ƷͼƬ����ѯ���ݡ���ѯʱ��] -->
            <tr>
                <!-- ��ƷͼƬ -->
                <td style="text-align: left; padding-left: 20px; padding-right: 20px; vertical-align: top;
                    width: 10%;">
                    <Hi:ListImage runat="server" Width="50" Height="50" DataField="ThumbnailUrl40"/>
                </td>
                <td style="text-align: left; width: 80%;">
                    <table cellspacing="0" border="0" width="100%">
                        <!-- ��ѯ���� -->
                        <tr>
                            <td style="text-align: left; color: #525757;">
                                �������ˣ�<asp:Label ID="lbltext" runat="server" Text='<%# Eval("ConsultationText") %>'></asp:Label>
                            </td>
                        </tr>
                        <!-- ��ѯʱ�� -->
                        <tr>
                            <td style="text-align: right;  color: #898989; padding-right: 10px;" class="fontgrey">
                                �������ڣ�
                                <Hi:FormatedTimeLabel ID="FormatedTimeLabel2" Time='<%# Eval("ConsultationDate") %>'
                                    runat="server"></Hi:FormatedTimeLabel>
                            </td>
                        </tr>
                        <!-- �ظ���Ϣ - Ƕ�� -->
                        <tr>
                            <td style="padding: 10px 5px 10px 20px;">
                                <asp:DataList ID="dlstPtConsultationReply" runat="server" Width="100%" DataKeyField="ConsultationId"
                                    DataSource='<%# Eval( "ConsultationReplys") %>'>
                                    <ItemTemplate>
                                        <table cellspacing="0" border="0" width="100%" style="background: #f7f7f7; line-height: 2em;">
                                            <!-- �ظ����� -->
                                            <tr>
                                                <td style="text-align: left; width: 95%; padding-left: 10px; color:#d75509">
                                                    ����Ա�ظ���<asp:Label ID="lblReplyContent" runat="server" Text='<%# Eval("ReplyText") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <!-- �ظ��ˡ��ظ�ʱ�� -->
                                            <tr>
                                                <td colspan="2" style="padding-left: 30px; padding-top: 6px; color: #898989;">
                                                    �ظ���
                                                    <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%# Eval("ReplyDate") %>'
                                                        runat="server"></Hi:FormatedTimeLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
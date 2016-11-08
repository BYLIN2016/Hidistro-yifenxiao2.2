<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.AccountCenter.CodeBehind" Assembly="Hidistro.UI.AccountCenter.CodeBehind" %>

        <table cellspacing="0" border="0" width="100%" style="border-bottom: solid 1px #C0C0C0;">
            <!-- ��һ��[����] -->
            <tr>
                <td colspan="2" style="text-align: left;">
                    <span class="clewB">
                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("IndexId")%>'></asp:Label>.
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
                    width: 20%;">
                    <Hi:ListImage runat="server" DataField="ThumbnailUrl40"/>
                </td>
                <td style="text-align: left; vertical-align: top; width: 80%;">
                    <table cellspacing="0" border="0" width="100%">
                        <!-- ��ѯ���� -->
                        <tr>
                            <td style="text-align: left; color: #525757;">
                                <asp:Label ID="lbltext" runat="server" Text='<%# Eval("ConsultationText") %>'></asp:Label>
                            </td>
                        </tr>
                        <!-- ��ѯʱ�� -->
                        <tr>
                            <td style="text-align: right; padding-right: 10px; padding-top: 6px;" class="fontgrey">
                                ������
                                <Hi:FormatedTimeLabel ID="FormatedTimeLabel2" Time='<%# Eval("ConsultationDate") %>'
                                    runat="server"></Hi:FormatedTimeLabel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
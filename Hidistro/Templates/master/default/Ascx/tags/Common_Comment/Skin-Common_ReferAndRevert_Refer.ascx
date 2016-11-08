<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.AccountCenter.CodeBehind" Assembly="Hidistro.UI.AccountCenter.CodeBehind" %>

        <table cellspacing="0" border="0" width="100%" style="border-bottom: solid 1px #C0C0C0;">
            <!-- 第一行[标题] -->
            <tr>
                <td colspan="2" style="text-align: left;">
                    <span class="clewB">
                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("IndexId")%>'></asp:Label>.
                    </span>&nbsp;&nbsp;对
                    “<span class="productMainClass">
                        <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                            ProductId='<%# Eval("ProductId") %>' runat="server" />
                    </span><strong>&nbsp;”</strong>
                    的咨询
                </td>
            </tr>
            <!-- 第二行[商品图片、咨询内容、咨询时间] -->
            <tr>
                <!-- 商品图片 -->
                <td style="text-align: left; padding-left: 20px; padding-right: 20px; vertical-align: top;
                    width: 20%;">
                    <Hi:ListImage runat="server" DataField="ThumbnailUrl40"/>
                </td>
                <td style="text-align: left; vertical-align: top; width: 80%;">
                    <table cellspacing="0" border="0" width="100%">
                        <!-- 咨询内容 -->
                        <tr>
                            <td style="text-align: left; color: #525757;">
                                <asp:Label ID="lbltext" runat="server" Text='<%# Eval("ConsultationText") %>'></asp:Label>
                            </td>
                        </tr>
                        <!-- 咨询时间 -->
                        <tr>
                            <td style="text-align: right; padding-right: 10px; padding-top: 6px;" class="fontgrey">
                                发表于
                                <Hi:FormatedTimeLabel ID="FormatedTimeLabel2" Time='<%# Eval("ConsultationDate") %>'
                                    runat="server"></Hi:FormatedTimeLabel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
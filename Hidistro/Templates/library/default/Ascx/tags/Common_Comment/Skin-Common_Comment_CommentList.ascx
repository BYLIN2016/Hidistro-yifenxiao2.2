<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Membership.Context"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.AccountCenter.CodeBehind" Assembly="Hidistro.UI.AccountCenter.CodeBehind" %>

        <table cellspacing="0" border="0" cellpadding="0" width="750" style=" background-color:#F9F9F9; margin-bottom:20px;" >
            <!-- 第一层第一行[商品名称] -->
            <tr>
                <td style="text-align: left; width: 96%;" colspan="2">
                    对
                    “<span style="color:#0066FF; font-weight: bold;">
                        <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                            ProductId='<%# Eval("ProductId") %>' runat="server" ForeColor="#0066FF" />
                    </span>”
                    的评论
                </td>
            </tr>
            <tr><td rowspan="2" Width="15%"><Hi:ListImage runat="server" DataField="ProductImage"  /></td><td></td></tr>
            <tr>
                <td style="padding-left:0px;" valign="top">
                    <!-- 第二层[商品评论] -->
                    <asp:DataList ID="dlstPtReviews" runat="server" Width="100%" DataKeyField="ReviewId"
                        DataSource='<%# Eval( "PtReviews") %>'>
                        <ItemTemplate>
                            <table id="Table1" cellspacing="0" border="0"  style="text-align: left; width: 100%;
                                border-top: solid 1px #C0C0C0;">
                                <tr>
                                    <td style="text-align: left;">
                                        <table cellspacing="0" border="0" style="text-align: left; width: 100%;">
                                            <tr>
                                                <!-- 评论类型、标题 -->
                                                <td style="text-align: left; width: 70%">
                                                    <span style="color: #FF8C00; font-weight: bold;">
                                                    </span><span style="font-weight: bold;">
                                                       
                                                    </span>
                                                </td>
                                                <!-- 评级 -->
                                                <td style="text-align: right;">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <!-- 评论内容 -->
                                    <td style="text-align: left; ">
                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("ReviewText") %>'></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                <td style="text-align: right; padding-left: 10px;">
                                <span style="color:Silver;">我于&nbsp;<Hi:FormatedTimeLabel ID="FormatedTimeLabel" runat="server" Time='<%# Eval("ReviewDate") %>' />&nbsp;评论&nbsp;&nbsp;&nbsp;</span>
                                </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>

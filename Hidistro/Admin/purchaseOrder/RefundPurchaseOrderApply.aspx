<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RefundPurchaseOrderApply.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RefundPurchaseOrderApply" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="purchaseorder.helper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<input type="hidden" runat="server" id="hidPurchaseOrderId" />
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <!--搜索-->
        <div class="title">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1>
                退款申请单</h1>
            <span>对退款申请单进行管理</span>
        </div>
        <div class="datalist">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>采购单编号：</span><span>
                        <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                            runat="server" Style="display: none;"></asp:Label>
                    </span></li>
                    <li><span>处理状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList runat="server" ID="ddlHandleStatus">
                                <asp:ListItem Value="-1">所有状态</asp:ListItem>
                                <asp:ListItem Value="0">待处理</asp:ListItem>
                                <asp:ListItem Value="1">已处理</asp:ListItem>
                                <asp:ListItem Value="2">已拒绝</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span></li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
                    </li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix m_none">
                <!--分页功能-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                    </div>
                </div>
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                            <a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span> 
                            <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" /></span>
                        </li>
                    </ul>
                </div>
            </div>
            <!--数据列表区域-->
            <div class="datalist">
                <asp:DataList ID="dlstRefund" runat="server" DataKeyField="RefundId" Width="100%">
                    <HeaderTemplate>
                        <table width="0" border="0" cellspacing="0" style="TABLE-LAYOUT: fixed">
                            <tr class="table_title">
                                <td width="5%" class="td_right td_left">
                                    选择
                                </td>
                                <td width="10%" class="td_right td_left">
                                    采购单编号
                                </td>
                                <td width="7%" class="td_right td_left">
                                    分销商
                                </td>
                                <td width="10%" class="td_right td_left">
                                    退款金额(元)
                                </td>
                                <td width="8%" class="td_right td_left">
                                    申请时间
                                </td>
                                <td class="td_right td_left" width="20%">
                                    退款备注
                                </td>
                                <td width="6%" class="td_right td_left">
                                    处理状态
                                </td>
                                <td width="8%" class="td_right td_left">
                                    处理时间
                                </td>
                                <td class="td_right td_left" width="15%">
                                    管理员备注
                                </td>
                                <td width="14%" class="td_left td_right">
                                    操作
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("RefundId") %>' />
                            </td>
                            <td>
                                <%# Eval("PurchaseOrderId") %>
                            </td>
                            <td>
                                <%# Eval("Distributorname")%>
                            </td>
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" />
                            </td>
                            <td>
                                <%# Eval("ApplyForTime", "{0:d}")%>
                            </td>
                            <td style="word-WRAP:break-word">
                               &nbsp; <%# Eval("RefundRemark")%>
                            </td>
                            <td>
                                &nbsp;<asp:Label ID="lblHandleStatus" runat="server" Text='<%# Eval("HandleStatus")%>'></asp:Label>
                            </td>
                            <td>
                                &nbsp;<%# Eval("HandleTime", "{0:d}")%>
                            </td>
                            <td>
                               &nbsp; <%# Eval("AdminRemark")%>
                            </td>
                            <td>
                             <a href='<%# "PurchaseOrderDetails.aspx?PurchaseOrderId="+Eval("PurchaseOrderId") %>'>详情</a>
                             <a href="javascript:void(0)" onclick="return CheckRefund(this.title)" runat="server" id="lkbtnCheckRefund" visible="false" title='<%# Eval("PurchaseOrderId") %>'>确认退款</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:DataList>
                <div class="blank5 clearfix">
                </div>
            </div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--确认退款--->
     
    <div id="CheckRefund" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：1.分销商已付款完成，并确认无误；2.确认分销商的申请退款方式。</em></p>
            <p>
                <span class="frame-span frame-input100">采购单号:</span> <span>
                    <asp:Label ID="refund_lblPurchaseOrderId" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">采购单金额:</span> <span>
                    <asp:Label ID="lblPurchaseOrderTotal" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">分销商退款方式:</span> <span>
                    <asp:Label ID="lblRefundType" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">退款原因:</span> <span>
                    <asp:Label ID="lblRefundRemark" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系人:</span> <span>
                    <asp:Label ID="lblContacts" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">电子邮件:</span> <span>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系电话:</span> <span>
                    <asp:Label ID="lblTelephone" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系地址:</span> <span>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">管理员备注:</span> <span>
                    <asp:TextBox ID="txtAdminRemark" runat="server" CssClass="forminput" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button2" onclick="javascript:acceptRefund();" class="submit_DAqueding"
                    value="确认退款" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript:refuseRefund();" class="submit_DAqueding"
                    value="拒绝退款" />
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="确认退款" />
        <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="拒绝退款" />
    </div>
</asp:Content>

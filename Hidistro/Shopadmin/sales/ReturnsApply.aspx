<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true"
    CodeBehind="ReturnsApply.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.sales.ReturnsApply" %>

<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="myorder.helper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <style>
        .a_remark span
        {
            display: none;
        }
        .a_remark:hover
        {
            display: block;
            z-index: 100px;
            border: 1px dash yellow;
        }
    </style>
    <div class="optiongroup mainwidth">
        <ul>
            <li class="optionstar"><a href="RefundApply.aspx" class="optionnext"><span>退款申请单</span></a></li>
            <li class="menucurrent"><a href="javascript:void(0);"><span class="optioncenter">退货申请单</span></a></li>
            <li><a href="ReplaceApply.aspx"><span>换货申请单</span></a></li>
        </ul>
    </div>
    <div class="dataarea mainwidth">
        <!--搜索-->
        <div class="searcharea clearfix br_search">
            <ul>
                <li><span>订单编号：</span><span>
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
                            <a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span> <span class="delete">
                                <Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" /></span>
                    </li>
                </ul>
            </div>
        </div>
        <input type="hidden" id="hidOrderId" runat="server" />
        <!--数据列表区域-->
        <div class="datalist">
            <div>
                <table width="0" border="0" cellspacing="0" style="TABLE-LAYOUT: fixed">
                    <tr class="table_title">
                        <td width="5%" class="td_right td_left">
                            选择
                        </td>
                        <td width="12%" class="td_right td_left">
                            订单编号
                        </td>
                        <td width="10%" class="td_right td_left">
                            会员名
                        </td>
                        <td width="10%" class="td_right td_left">
                            退款金额(元)
                        </td>
                        <td width="8%" class="td_right td_left">
                            申请时间
                        </td>
                        <td class="td_right td_left" width="17%">
                            退货备注
                        </td>
                        <td width="7%" class="td_right td_left">
                            处理状态
                        </td>
                        <td width="8%" class="td_right td_left">
                            处理时间
                        </td>
                        <td class="td_right td_left" width="10%">
                            管理员备注
                        </td>
                        <td width="12%" class="td_left td_right">
                            操作
                        </td>
                    </tr>
                    <asp:Repeater ID="dlstReturns" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td >
                                    <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ReturnsId") %>' />
                                </td>
                                <td >
                                    <%# Eval("OrderId") %>
                                </td>
                                <td >
                                    <%# Eval("Username") %>
                                </td>
                                <td>
                                   <Hi:FormatedMoneyLabel ID="lblRefundMoney" Money='<%#Eval("RefundMoney") %>' runat="server" />
                                </td>
                                <td>
                                    <%# Eval("ApplyForTime", "{0:d}")%>
                                </td>
                                <td  style="word-WRAP:break-word">
                                    <%# Eval("Comments")%>
                                </td>
                                <td>
                                    &nbsp;<asp:Label ID="lblHandleStatus" runat="server" Text='<%# Eval("HandleStatus")%>'></asp:Label>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("HandleTime","{0:d}")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("AdminRemark")%>
                                </td>
                                <td>
                                    &nbsp; <a href='<%# "MyOrderDetails.aspx?OrderId="+Eval("OrderId") %>' target="_blank">
                                        详情</a> <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server"
                                            id="lkbtnCheckReturns" visible="false" title='<%# Eval("OrderId") %>'>确认退货</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <div class="instantstat clearfix" id="divSendOrders">
                </div>
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
    <!--确认退货--->
    <div class="Pop_up" id="CheckReturn" style="display: none;">
        <h1>
            确认退货
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform fonts colorA borbac">
            执行本操作前确保：1.买家已付款完成，并确认无误；<br />
            2.确认买家的申请退款方式。</div>
         <div class="mianform validator2">
         <table width="100%">
                <tr>
                    <td width="15%" align="right">订单号:</td><td width="35%"><asp:Label ID="return_lblOrderId" runat="server"></asp:Label></asp:Label></td>
                    <td width="15%" align="right">订单金额:</td><td width="35%"><asp:Label ID="return_lblOrderTotal" runat="server" /></td>
                </tr>
                  <tr>
                    <td align="right">联系人:</td><td><asp:Label ID="return_lblContacts" runat="server"></asp:Label></td>
                    <td align="right">电子邮件:</td><td><asp:Label ID="return_lblEmail" runat="server"></asp:Label></td>
                </tr>
               <tr>
                    <td align="right">联系电话:</td><td><asp:Label ID="return_lblTelephone" runat="server"></asp:Label></td>
                    <td align="right">联系地址:</td><td> <asp:Label ID="return_lblAddress" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">退款方式:</td><td><asp:Label ID="return_lblRefundType" runat="server"></asp:Label></td>
                    <td align="right">退款原因:</td><td><asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label></td>
                </tr>
                <tr><td align="right">退款金额:</td><td colspan="3"><asp:TextBox ID="return_txtRefundMoney" runat="server" CssClass="forminput" /></td></tr>
                <tr>
                    <td align="right">管理员备注:</td><td colspan="3">
                    <textarea id="return_txtAdminRemark" runat="server" rows="5" cols="52"></textarea>
                    </td>
                </tr>
                <tr><td></td><td colspan="3"> <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="确认退货" />　　
                <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="拒绝退货" /></td></tr>
          </table>
        </div>
    </div>
    <!--备注--->
    <div class="Pop_up" id="div_remark" style="display: none;">
        <h1>
            管理员备注
        </h1>
        <div class="mianform fonts colorA borbac">
            <div class="mianform">
                <ul>
                    <li><span class="formitemtitle Pw_140">订单号:</span> <span>
                        <asp:Label ID="Label1" runat="server"></asp:Label></span></li>
                    <li><span class="formitemtitle Pw_140">订单金额:</span> <span>
                        <asp:Label ID="Label2" runat="server" /></span></li>
                    <li><span class="formitemtitle Pw_140">买家退款方式:</span> <span>
                        <asp:Label ID="Label3" runat="server"></asp:Label></span></li>
                    <li><span class="formitemtitle Pw_140">退货原因:</span> <span>
                        <asp:Label ID="Label4" runat="server"></asp:Label></span></li>
                    <li><span class="formitemtitle Pw_140">联系人:</span> <span>
                        <asp:Label ID="Label5" runat="server"></asp:Label></span></li>
                    <li><span class="formitemtitle Pw_140">电子邮件:</span> <span>
                        <asp:Label ID="Label6" runat="server"></asp:Label></span></li>
                    <li><span class="formitemtitle Pw_140">联系电话:</span> <span>
                        <asp:Label ID="Label7" runat="server"></asp:Label></span></li>
                    <li><span class="formitemtitle Pw_140">联系地址:</span> <span>
                        <asp:Label ID="Label8" runat="server"></asp:Label></span></li>
                    <li><span class="formitemtitle Pw_140">退款金额:</span> <span>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="forminput" /></span></li>
                    <li><span class="formitemtitle Pw_140">管理员备注:</span> <span>
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="forminput" /></span></li>
                </ul>
            </div>
        </div>
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidOrderTotal" runat="server" />
</asp:Content>

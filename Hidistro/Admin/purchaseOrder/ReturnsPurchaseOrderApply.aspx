<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReturnsPurchaseOrderApply.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReturnsPurchaseOrderApply" %>
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
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <!--搜索-->
        <div class="title">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1>
                退货申请单</h1>
            <span>对采购单退货申请单进行管理</span>
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
                    </span>
                    </li>
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
                                    <Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" />
                                </span></li>
                    </ul>
                </div>
            </div>
            <input type="hidden" runat="server" id="hidPurchaseOrderId" />
            <!--数据列表区域-->
            <div class="datalist">
                <asp:DataList ID="dlstReturns" runat="server" DataKeyField="ReturnsId" Width="100%">
                    <HeaderTemplate>
                        <table width="0" border="0" cellspacing="0" style="TABLE-LAYOUT: fixed">
                            <tr class="table_title">
                                <td width="5%" class="td_right td_left">
                                    选择
                                </td>
                                <td width="12%" class="td_right td_left">
                                    采购单编号
                                </td>
                                <td width="7%" class="td_right td_left">
                                    分销商
                                </td>
                                <td width="10%" class="td_right td_left">
                                    退款金额(元)
                                </td>
                                <td width="8%" class="td_right td_left">
                                    申请退货时间
                                </td>
                                <td class="td_right td_left" width="18%">
                                    退货备注
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
                                <td width="10%" class="td_left td_right">
                                    操作
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ReturnsId") %>' />
                            </td>
                            <td>
                                <%# Eval("PurchaseOrderId")%>
                            </td>
                            <td>
                                <%# Eval("Distributorname")%>
                            </td>
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblRefundMoney" Money='<%#Eval("RefundMoney") %>' runat="server" />
                            </td>
                            <td>
                                <%# Eval("ApplyForTime", "{0:d}")%>
                            </td>
                            <td style="word-WRAP:break-word">
                                &nbsp;<%# Eval("Comments")%>
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
                              <a href='<%# "PurchaseOrderDetails.aspx?PurchaseOrderId="+Eval("PurchaseOrderId") %>'>详情</a>
                               <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server" id="lkbtnCheckReturns" visible="false" title='<%# Eval("PurchaseOrderId") %>'>确认退货</a>
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
    <!--确认退货--->
    <div id="CheckReturn" style="display: none;">
        <div class="frame-content" style="margin-top:-20px;">
            <p>
                <em>执行本操作前确保：1.已收到买家寄换回来的货品，并确认无误；
                    2.确认买家的申请退款方式。</em></p>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
              <tr>
                <td align="right" width="30%">采购单号:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblPurchaseOrderId" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">采购单金额:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblPurchaseOrderTotal" runat="server" /></td>
              </tr>
              <tr>
                <td align="right">买家退款方式:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblRefundType" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">退货原因:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系人:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblContacts" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">电子邮件:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblEmail" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系电话:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblTelephone" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系地址:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblAddress" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">退款金额:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" CssClass="forminput" /></td>
              </tr>
            </table>
          
            <p>
                <span class="frame-span frame-input100"  style="margin-left:10px;">管理员备注:</span> <span >
                    <asp:TextBox ID="return_txtAdminRemark" runat="server" CssClass="forminput" style="margin-left:10px;" Width="243"/></span></p>
            
            <div style="text-align: center; padding-top:10px;">
                <input type="button" id="Button4" onclick="javascript:acceptReturn();" class="submit_DAqueding"
                    value="确认退货" />
                &nbsp;
                <input type="button" id="Button5" onclick="javascript:refuseReturn();" class="submit_DAqueding"
                    value="拒绝退货" />
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="确认退货" />
        <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="拒绝退货" />
    </div>
</asp:Content>

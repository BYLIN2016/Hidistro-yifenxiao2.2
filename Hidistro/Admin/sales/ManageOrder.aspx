<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ManageOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageOrder" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="optiongroup mainwidth">
        <ul>
            <li id="anchors0">
                <asp:HyperLink ID="hlinkAllOrder" runat="server"><span>所有订单</span></asp:HyperLink></li>
            <li id="anchors1">
                <asp:HyperLink ID="hlinkNotPay" runat="server" Text=""><span>等待买家付款</span></asp:HyperLink></li>
            <li id="anchors2">
                <asp:HyperLink ID="hlinkYetPay" runat="server" Text=""><span>等待发货</span></asp:HyperLink></li>
            <li id="anchors3">
                <asp:HyperLink ID="hlinkSendGoods" runat="server" Text=""><span>已发货</span></asp:HyperLink></li>
            <li id="anchors5">
                <asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>成功订单</span></asp:HyperLink></li>
            <li id="anchors4">
                <asp:HyperLink ID="hlinkClose" runat="server" Text=""><span>已关闭</span></asp:HyperLink></li>
            <li id="anchors99">
                <asp:HyperLink ID="hlinkHistory" runat="server" Text=""><span>历史订单</span></asp:HyperLink></li>
        </ul>
    </div>
    <!--选项卡-->
    <div class="dataarea mainwidth">
        <!--搜索-->
        <style>
        .searcharea ul li{ padding:5px 0px;}
        </style>
        <div class="searcharea clearfix br_search">
            <ul>
                <li><span>选择时间段：</span><span>
                    <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                </span><span class="Pg_1010">至</span> <span>
                    <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                </span></li>
                <li style="margin-left:13px;"><span>会员名：</span><span>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />
                </span></li>
                <li><span>订单编号：</span><span>
                    <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                        runat="server" Style="display: none;"></asp:Label>
                </span></li>
                <li><span>商品名称：</span><span>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" />
                </span></li>
                <li style="margin-left:23px;_margin-left:12px;"><span>收货人：</span><span>
                    <asp:TextBox ID="txtShopTo" runat="server" CssClass="forminput"></asp:TextBox>
                </span></li>
                <li><span>打印状态：</span><span>
                    <abbr class="formselect">
                        <asp:DropDownList runat="server" ID="ddlIsPrinted" Width="107" />
                    </abbr>
                </span></li>
                
                <li><span>配送方式：</span><span>
                    <abbr  class="formselect">
                        <Hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList"  Width="100" />
                    </abbr>
                </span></li>
                <li style="width: 410px;">
                    <abbr class="formselect">
                        <Hi:RegionSelector runat="server" ID="dropRegion" />
                    </abbr>
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
                                <Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" /></span>
                        <span class="printorder"><a href="javascript:printPosts()">批量打印快递单</a></span> <span
                            class="printorder"><a href="javascript:printGoods()">批量打印发货单</a></span> <span class="downproduct">
                                <a href="javascript:downOrder()">下载配货单</a></span> <span class="sendproducts"><a href="javascript:batchSend()"
                                    onclick="">批量发货</a></span> </li>
                </ul>
            </div>
        </div>
        <input type="hidden" id="hidOrderId" runat="server" />
        <!--数据列表区域-->
        <div class="datalist">
            <asp:DataList ID="dlstOrders" runat="server" DataKeyField="OrderId" Width="100%">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td width="24%" class="td_right td_left">
                                会员名
                            </td>
                            <td width="20%" class="td_right td_left">
                                收货人
                            </td>
                            <td width="18%" class="td_right td_left">
                                支付方式
                            </td>
                            <td width="14%" class="td_right td_left">
                                订单实收款(元)
                            </td>
                            <td width="14%" class="td_right td_left">
                                订单状态
                            </td>
                            <td width="10%" class="td_left td_right_fff">
                                操作
                            </td>
                             
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="td_bg">
                        <td>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />订单编号：<%#Eval("OrderId") %><asp:Literal
                                ID="group" runat="server" Text='<%# Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' />
                        </td>
                        <td>
                            提交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true"
                                runat="server"></Hi:FormatedTimeLabel>
                        </td>
                        <td>
                            <%# (bool)Eval("IsPrinted")?"已打印":"未打印" %>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;
                            <%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "物流单号：" + Eval("ShipOrderNumber")%>
                        </td>
                        <td align="right">&nbsp;
                            <a href="javascript:RemarkOrder('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Eval("ManagerMark") %>','<%#Eval("ManagerRemark") %>');">
                                <Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a>
                        </td>
                    </tr>

                    <tr>
                        <td>&nbsp;
                            <a href="<%# Eval("UserId").ToString()!="1100"?"javascript:DialogFrame('"+Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}",Eval("UserId")))+"','查看会员',null,null)":"javascript:void(0)" %>">
                                <%#Eval("UserName")%></a>
                            <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%#Eval("Wangwang") %>' />
                        </td>
                        <td>
                            <%#Eval("ShipTo") %>&nbsp;
                        </td>
                        <td style="color:#0B5BA5">
                            <%#Eval("PaymentType") %>&nbsp;
                        </td>
                        <td style=" font-weight:bold; font-family:Arial; ">
                            <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
                            <span class="Name">&nbsp;
                                <asp:HyperLink ID="lkbtnEditPrice" runat="server" NavigateUrl='<%# "EditOrder.aspx?OrderId="+ Eval("OrderId") %>'
                                    Text="修改价格" Visible="false"></asp:HyperLink></span>
                        </td>
                        <td style="color:#0B5BA5">&nbsp;
                            <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" />
                            <span class="Name"><a href='<%# "OrderDetails.aspx?OrderId="+Eval("OrderId") %>'>详情</a></span>
                        </td>
                        <td>&nbsp;
                            <span class="Name">
                                <Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="我已线下收款" CommandArgument='<%# Eval("OrderId") %>'
                                    CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false"
                                    ForeColor="Red"></Hi:ImageLinkButton><br />
                                <a href="javascript:CloseOrder('<%#Eval("OrderId") %>');">
                                    <asp:Literal runat="server" ID="litCloseOrder" Visible="false" Text="关闭订单" /></a>
                                <asp:Label CssClass="submit_faihuo" ID="lkbtnSendGoods" Visible="false" runat="server"> 
              <a style="color:Red" href="javascript:DialogFrame('<%# "sales/SendOrderGoods.aspx?OrderId="+ Eval("OrderId") %>','订单发货',null,null)">发货</a> </asp:Label>
                                <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="完成订单"
                                    CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认要完成该订单吗？"
                                    Visible="false" ForeColor="Red" />
                                <a style="color: Red"></a><a href="javascript:void(0)" onclick="return CheckRefund(this.title)"
                                    runat="server" id="lkbtnCheckRefund" visible="false" title='<%# Eval("OrderId") %>'>
                                    确认退款</a> <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server"
                                        id="lkbtnCheckReturn" visible="false" title='<%# Eval("OrderId") %>'>确认退货</a>
                                <a href="javascript:void(0)" onclick="return CheckReplace(this.title)" runat="server"
                                    id="lkbtnCheckReplace" visible="false" title='<%# Eval("OrderId") %>'>确认换货</a>&nbsp;
                            </span>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
            <div class="instantstat clearfix" id="divSendOrders">
                注：订单状态列中“(团)”字的代表团购订单
            </div>
            <div class="blank5 clearfix">
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
    <!--关闭订单--->
    <div id="closeOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em></p>
            <p>
                <span class="frame-span frame-input110">关闭该订单的理由:</span>
                <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" />
            </p>
        </div>
    </div>
    <!--编辑备注--->
    <style>
        .frame-content{ margin-top:-20px;}
    </style>
    <div id="RemarkOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input100">订单号：</span><span id="spanOrderId"></span></p>
            <p>
                <span class="frame-span frame-input100">提交时间：</span><span id="lblOrderDateForRemark"></span></p>
            <p>
                <span class="frame-span frame-input100">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel
                    ID="lblOrderTotalForRemark" runat="server" /></strong></p>
            <span class="frame-span frame-input100">标志：<em>*</em></span><Hi:OrderRemarkImageRadioButtonList
                runat="server" ID="orderRemarkImageForRemark" />
            <p>
                <span>备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server"
                    Width="300" Height="50" /></p>
        </div>
    </div>
    <div id="DownOrder" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnorderph" onclick="javascript:Setordergoods();" class="submit_DAqueding"
                value="订单配货表" />
            &nbsp;
            <input type="button" id="Button1" onclick="javascript:Setproductgoods();" class="submit_DAqueding"
                value="商品配货表" />
            <p>
                导出内容只包括等待发货状态的订单</p>
            <p>
                订单配货表不会合并相同的商品,商品配货表则会合并相同的商品。</p>
        </div>
    </div>
    <!--确认退款--->
    <div id="CheckRefund" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：<br />1.买家已付款完成，并确认无误；
                    2.确认买家的申请退款方式。</em></p>
                    
                    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                      <tr>
                        <td align="right" width="30%">订单号:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">订单金额:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblOrderTotal" runat="server" /></td>
                      </tr>
                      <tr>
                        <td align="right">买家退款方式:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblRefundType" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">退款原因:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblRefundRemark" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">联系人:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblContacts" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">电子邮件:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblEmail" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">联系电话:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblTelephone" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">联系地址:</td>
                        <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                      </tr>
                    </table>

            <p>
                <span class="frame-span frame-input100" style="margin-right:10px;">管理员备注:</span> <span>
                    <asp:TextBox ID="txtAdminRemark" runat="server" CssClass="forminput" Width="243" /></span></p>
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
    <!--确认退货--->
    <div id="CheckReturn" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：<br />1.已收到买家寄换回来的货品，并确认无误；
                    2.确认买家的申请退款方式。</em></p>
                    
                    
                    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                      <tr>
                        <td align="right" width="30%">订单号:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblOrderId" runat="server" ></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">订单金额:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblOrderTotal" runat="server"/></td>
                      </tr>
                      <tr>
                        <td align="right">买家退款方式:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblRefundType" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">退货原因:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">联系人:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblContacts" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">电子邮件:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblEmail" runat="server" ></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">联系电话:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblTelephone" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">联系地址:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblAddress" runat="server"></asp:Label></td>
                      </tr>
                      <tr>
                        <td align="right">退款金额:</td>
                        <td align="left" class="bd_td">&nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" /></td>
                      </tr>
                    </table>

             
            <p>
                <span class="frame-span frame-input100" style="margin-right:10px;">管理员备注:</span> <span>
                    <asp:TextBox ID="return_txtAdminRemark" runat="server" CssClass="forminput" Width="243"/></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button4" onclick="javascript:acceptReturn();" class="submit_DAqueding"
                    value="确认退货" />
                &nbsp;
                <input type="button" id="Button5" onclick="javascript:refuseReturn();" class="submit_DAqueding"
                    value="拒绝退货" />
            </div>
        </div>
    </div>
    <!--确认换货--->
    <div id="CheckReplace" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：<br />1.已收到买家寄还回来的货品，并确认无误；
                    </em></p>
                    
             <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
              <tr>
                <td align="right" width="30%">订单号:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderId" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">订单金额:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderTotal" runat="server" /></td>
              </tr>
              <tr>
                <td align="right">换货备注:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblComments" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系人:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblContacts" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">电子邮件:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblEmail" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系电话:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblTelephone" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系地址:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblAddress" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">邮政编码:</td>
                <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblPostCode" runat="server"></asp:Label></td>
              </tr>
            </table>
        
            
            <p>
                <span class="frame-span frame-input100" style="margin-right:10px;">管理员备注:</span> <span>
                    <asp:TextBox ID="replace_txtAdminRemark" runat="server" CssClass="forminput" Width="243"/></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button6" onclick="javascript:acceptReplace();" class="submit_DAqueding"
                    value="确认换货" />
                &nbsp;
                <input type="button" id="Button7" onclick="javascript:refuseReplace();" class="submit_DAqueding"
                    value="拒绝换货" />
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <asp:Button ID="btnCloseOrder" runat="server" CssClass="submit_DAqueding" Text="关闭订单" />
        <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="确认退款" />
        <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="拒绝退款" />
        <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="确认退货" />
        <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="拒绝退货" />
        <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="确认换货" />
        <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="拒绝换货" />
        <asp:Button runat="server" ID="btnRemark" Text="编辑备注信息" CssClass="submit_DAqueding" />
        <asp:Button ID="btnOrderGoods" runat="server" CssClass="submit_DAqueding" Text="订单配货表" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="submit_DAqueding" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js" type="text/javascript"></script>
    <script type="text/javascript">
        var formtype = "";

        function ConfirmPayOrder() {
            return confirm("如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态\n\n此操作成功完成以后，订单的当前状态将变为已付款状态，确认客户已付款？");
        }

        function ShowOrderState() {
            var status;
            if (navigator.appName.indexOf("Explorer") > -1) {

                status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

            } else {

                status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

            }
            if (status != "0") {
                document.getElementById("anchors0").className = 'optionstar';
            }
            if (status != "99") {
                document.getElementById("anchors99").className = 'optionend';
            }
            document.getElementById("anchors" + status).className = 'menucurrent';
        }

        $(document).ready(function () { ShowOrderState(); });


        //备注信息
        function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            arrytext = null;
            formtype = "remark";
            $("#ctl00_contentHolder_lblOrderTotalForRemark").html(OrderTotal);
            $("#ctl00_contentHolder_hidOrderId").val(OrderId);
            $("#spanOrderId").html(OrderId);
            $("#lblOrderDateForRemark").html(OrderDate);




            for (var i = 0; i <= 5; i++) {
                if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark) {
                    setArryText("ctl00_contentHolder_orderRemarkImageForRemark_" + i, "true");
                    $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", true);
                }
                else {
                    $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", false);
                }

            }
            setArryText("ctl00_contentHolder_txtRemark", managerRemark);
            DialogShow("修改备注", 'updateremark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
        }

        function CloseOrder(orderId) {
            arrytext = null;
            formtype = "close";
            $("#ctl00_contentHolder_hidOrderId").val(orderId);
            DialogShow("关闭订单", 'closeframe', 'closeOrder', 'ctl00_contentHolder_btnCloseOrder');
        }
        
        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                alert("请选择关闭的理由");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }

        // 批量打印发货单
        function printGoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要打印的订单");
            }
            else {
                var url = "/Admin/sales/BatchPrintSendOrderGoods.aspx?OrderIds=" + orderIds;
                window.open(url, "批量打印发货单", "width=700, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=n o, status=no");
            }
        }

        //批量发货
        function batchSend() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要发货的订单");
            }
            else {
                DialogFrame("sales/BatchSendOrderGoods.aspx?OrderIds=" + orderIds, "批量发货", null, null);
            }
        }
        function Setordergoods() {
            $("#ctl00_contentHolder_btnOrderGoods").trigger("click");
        }
        function Setproductgoods() {
            $("#ctl00_contentHolder_btnProductGoods").trigger("click");
        }
        //批量打印快递单
        function printPosts() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要打印的订单");
            }
            else {
                var url = "sales/BatchPrintData.aspx?OrderIds=" + orderIds;
                DialogFrame(url, "批量打印快递单", null, null);
            }
        }

        //验证
        function validatorForm() {
            switch (formtype) {
                case "remark":
                    arrytext = null;
                    $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                    if ($radioId == null || $radioId == "undefined") {
                        alert('请先标记备注');
                        return false;
                    }
                    setArryText($radioId.id, "true");
                    setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                    break;
                case "shipptype":
                    return ValidationShippingMode();
                    break;
                case "close":
                    return ValidationCloseReason();
                    break;
            };
            return true;
        }
        // 下载配货单
        function downOrder() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要下载配货单的订单");
            }
            else {
                ShowMessageDialog("下载配货批次表", "downorder", "DownOrder");
            }
        }
        $(function () {
            $(".datalist img[src$='tui.gif']").each(function (item, i) {
                $parent_link = $(this).parent();
                $parent_link.attr("href", "javascript:DialogFrame('sales/" + $parent_link.attr("href") + "','退款详细信息',null,null);");
            });
        });
   

    </script>
</asp:Content>

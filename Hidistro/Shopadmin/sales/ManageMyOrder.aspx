<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true"
    CodeBehind="ManageMyOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyOrder" %>

<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
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
        <div class="searcharea clearfix br_search">
            <ul>
                <li><span>选择时间段：</span><span>
                    <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                </span><span class="Pg_1010">至</span> <span>
                    <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                </span></li>
                <li><span>会员名：</span><span>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />
                </span></li>
                <li><span>订单编号：</span><span>
                    <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                        runat="server" Style="display: none;"></asp:Label>
                </span></li>
                <li><span>商品名称：</span><span>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" />
                </span></li>
                <li><span>收货人：</span><span>
                    <asp:TextBox ID="txtShopTo" runat="server" CssClass="forminput"></asp:TextBox>
                </span></li>
                <li>
                    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
                </li>
            </ul>
        </div>
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
                                <td width="12%" class="td_right td_left">
                                    订单实收款(元)
                                </td>
                                <td width="12%" class="td_right td_left">
                                    订单状态
                                </td>
                                <td width="12%" class="td_left td_right_fff">
                                    操作
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="td_bg">
                            <td>&nbsp;
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>'>订单编号：<%#Eval("OrderId") %><asp:Literal
                                    ID="group" runat="server" Text='<%# Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' />
                            </td>
                            <td>
                                提交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true"
                                    runat="server"></Hi:FormatedTimeLabel>
                            </td>
                            <td>&nbsp;
                                <%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "物流单编号：" + Eval("ShipOrderNumber")%>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td align="right">&nbsp;
                                <a href="javascript:RemarkOrder('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Eval("ManagerMark") %>','<%#Eval("ManagerRemark") %>')">
                                    <Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                                <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# string.Format("../Underling/UnderlingDetails.aspx?userId={0}", Eval("UserId"))%>'
                                    Enabled='<%# Eval("UserId").ToString()!="1100" %>'><%#Eval("UserName")%></asp:HyperLink>
                                <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%#Eval("Wangwang") %>' />
                            </td>
                            <td>
                                <%#Eval("ShipTo") %>&nbsp;
                            </td>
                            <td>&nbsp;
                                <%#Eval("PaymentType") %>
                            </td>
                            <td>&nbsp;
                                <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
                                <span class="Name">
                                    <asp:HyperLink ID="lkbtnEditPrice" runat="server" NavigateUrl='<%# "EditMyOrder.aspx?OrderId="+ Eval("OrderId") %>'
                                        Target="_blank" Text="修改价格" Visible="false" ForeColor="Blue"></asp:HyperLink></span>
                                <a href="javascript:CloseOrder('<%#Eval("OrderId") %>');">
                                    <asp:Literal runat="server" ID="litCloseOrder" Visible="false" Text="关闭订单" /></a>
                            </td>
                            <td>&nbsp;
                                <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>'
                                    runat="server" />
                                <span class="Name">
                                    <a href='<%# "MyOrderDetails.aspx?OrderId="+Eval("OrderId") %>' target="_blank">详情</a>
                                </span>
                                <span class="Name">
                                    <Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="我已线下收款" CommandArgument='<%# Eval("OrderId") %>'
                                        CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false"
                                        ForeColor="Red"></Hi:ImageLinkButton></span>
                            </td>
                            <td>&nbsp;
                                <Hi:ImageLinkButton ID="lkbtnCreatePurchaseOrder" IsShow="true" runat="server" Visible="false"
                                    Text="生成采购单" CommandArgument='<%# Eval("OrderId") %>' CommandName="CREATE_PURCHASEORDER"
                                    DeleteMsg="只有在团购成功以后，才能为对应的团购订单成生采购单，确定生成采购单吗？" ForeColor="Red" />
                                <span class="submit_faihuo">&nbsp;<asp:HyperLink ID="lkbtnSendGoods" runat="server"
                                    NavigateUrl='<%# "SendMyGoods.aspx?OrderId="+ Eval("OrderId") %>' Target="_blank"
                                    Text="发货" Visible="false" ForeColor="Red"></asp:HyperLink></span>
                                <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="完成订单"
                                    CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认要完成该订单吗？"
                                    Visible="false" ForeColor="Red" />
                                <a href="javascript:void(0)" onclick="return CheckRefund(this.title)" runat="server"
                                    id="lkbtnCheckRefund" visible="false" title='<%# Eval("OrderId") %>'>确认退款</a>
                                <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server"
                                    id="lkbtnCheckReturn" visible="false" title='<%# Eval("OrderId") %>'>确认退货</a>
                                <a href="javascript:void(0)" onclick="return CheckReplace(this.title)" runat="server"
                                    id="lkbtnCheckReplace" visible="false" title='<%# Eval("OrderId") %>'>确认换货</a>
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
    <div class="Pop_up" id="RemarkOrder" style="display: none;">
        <h1>
            编辑备注信息
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li><span class="formitemtitle Pw_128">订单编号：</span><span id="spanOrderId" runat="server" /></li>
                <li><span class="formitemtitle Pw_128">提交时间：</span><span runat="server" id="lblOrderDateForRemark" /></li>
                <li><span class="formitemtitle Pw_128">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel
                    ID="lblOrderTotalForRemark" runat="server" /></strong></li>
                <li><span class="formitemtitle Pw_128">标志：</span> <span>
                    <Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" />
                </span></li>
                <li><span class="formitemtitle Pw_128">备忘录：</span>
                    <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" />
                </li>
            </ul>
            <ul class="up Pa_100">
                <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding" />
            </ul>
        </div>
    </div>

    <div class="Pop_up" id="closeOrder" style="display: none;">
        <h1>
            关闭订单
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform fonts colorA borbac">
            关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</div>
        <div class="mianform ">
            <ul>
                <li><span class="formitemtitle Pw_160">关闭该订单的理由：</span>
                    <abbr class="formselect">
                        <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" />
                    </abbr>
                </li>
            </ul>
            <ul class="up Pa_160">
                <asp:Button ID="btnCloseOrder" runat="server" CssClass="submit_DAqueding" OnClientClick="return ValidationCloseReason()"
                    Text="确 定" />
            </ul>
        </div>
    </div>


    <!--确认退款--->
    <div class="Pop_up" id="CheckRefund" style="display: none;">
        <h1>
            确认退款
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform fonts colorA borbac">
            执行本操作前确保：1.买家已付款完成，并确认无误；<br />
            2.确认买家的申请退款方式。</div>
         <div class="mianform validator2">
                <table width="100%">
                <tr>
                    <td width="15%" align="right">订单号:</td><td width="35%"><asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
                    <td width="15%" align="right">订单金额:</td><td width="35%"><asp:Label ID="lblOrderTotal" runat="server" /></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系人:</td><td><asp:Label ID="lblContacts" runat="server"></asp:Label></td>
                    <td align="right">电子邮件:</td><td><asp:Label ID="lblEmail" runat="server"></asp:Label></td>
                </tr>
               <tr>
                    <td align="right">联系电话:</td><td><asp:Label ID="lblTelephone" runat="server"></asp:Label></td>
                    <td align="right">联系地址:</td><td><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">退款方式:</td><td><asp:Label ID="lblRefundType" runat="server"></asp:Label></td>
                    <td align="right">退款原因:</td><td><asp:Label ID="lblRefundRemark" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">管理员备注:</td><td colspan="3">
                    <textarea id="txtAdminRemark" runat="server" rows="5" cols="52"></textarea>
                    </td>
                </tr>
                <tr><td></td><td colspan="3">
                <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="确认退款" />　　
                <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="拒绝退款" /></td></tr>
            </table>
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

    <!--确认换货--->
    <div class="Pop_up" id="CheckReplace" style="display: none;">
        <h1>
            确认换货
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform fonts colorA borbac">
            执行本操作前确保：1.已收到买家寄还回来的货品，并确认无误；</div>
         <div class="mianform validator2">
             <table width="100%">
                <tr>
                    <td width="15%" align="right">订单号:</td><td width="35%"><asp:Label ID="replace_lblOrderId" runat="server"></asp:Label></td>
                    <td width="15%" align="right">订单金额:</td><td width="35%"><asp:Label ID="replace_lblOrderTotal" runat="server" /></td>
                </tr>
                <tr>
                    <td align="right">联系人:</td><td><asp:Label ID="replace_lblContacts" runat="server"></asp:Label></td>
                    <td align="right">电子邮件:</td><td><asp:Label ID="replace_lblEmail" runat="server"></asp:Label></td>
                </tr>
               <tr>
                    <td align="right">联系电话:</td><td><asp:Label ID="replace_lblTelephone" runat="server"></asp:Label></td>
                    <td align="right">联系地址:</td><td><asp:Label ID="replace_lblAddress" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">邮政编码:</td><td><asp:Label ID="replace_lblPostCode" runat="server"></asp:Label></td>
                    <td align="right">换货备注:</td><td><asp:Label ID="replace_lblComments" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">管理员备注:</td><td colspan="3">
                    <textarea id="replace_txtAdminRemark" runat="server" rows="5" cols="52"></textarea>
                    </td>
                </tr>
                <tr><td></td><td colspan="3">
                <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="确认换货" />　　
                <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="拒绝换货" /></td></tr>
            </table>
        </div>
    </div>


    <input type="hidden" id="hidRefundType" runat="server" />
    <input type="hidden" id="hidOrderTotal" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="myorder.helper.js" type="text/javascript"></script>
    <script type="text/javascript">
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

        
        function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            $("#ctl00_contentHolder_spanOrderId").html(OrderId);
            $("#ctl00_contentHolder_hidOrderId").val(OrderId);
            $("#ctl00_contentHolder_lblOrderDateForRemark").html(OrderDate);
            $("#ctl00_contentHolder_lblOrderTotalForRemark").html(OrderTotal);
            $("#ctl00_contentHolder_txtRemark").val(managerRemark);

            for (var i = 0; i <= 5; i++) {
                if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark)
                    document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = true;
                else
                    document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = false;
            }

            DivWindowOpen(550, 350, 'RemarkOrder')

        }

        

        function CloseOrder(orderId) {
            $("#ctl00_contentHolder_hidOrderId").val(orderId);
            DivWindowOpen(560, 250, 'closeOrder');
        }

        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                alert("请选择关闭的理由");
                return false;
            }

            return true;
        }
    </script>
</asp:Content>

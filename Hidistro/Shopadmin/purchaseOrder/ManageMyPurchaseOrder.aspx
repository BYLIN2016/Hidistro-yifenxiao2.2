<%@ Page Language="C#" MasterPageFile="~/ShopAdmin/ShopAdmin.Master" AutoEventWireup="true"
    CodeBehind="ManageMyPurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyPurchaseOrder" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="optiongroup mainwidth">
        <ul>
            <li id="anchors0">
                <asp:HyperLink ID="hlinkAllOrder" runat="server"><span>所有采购单</span></asp:HyperLink></li>
            <li id="anchors1">
                <asp:HyperLink ID="hlinkNotPay" runat="server"><span>等待付款</span></asp:HyperLink></li>
            <li id="anchors2">
                <asp:HyperLink ID="hlinkYetPay" runat="server"><span>等待发货</span></asp:HyperLink></li>
            <li id="anchors3">
                <asp:HyperLink ID="hlinkSendGoods" runat="server"><span>已发货</span></asp:HyperLink></li>
            <li id="anchors5">
                <asp:HyperLink ID="hlinkFinish" runat="server"><span>成功采购单</span></asp:HyperLink></li>
            <li id="anchors4">
                <asp:HyperLink ID="hlinkClose" runat="server"><span>已关闭</span></asp:HyperLink></li>
            <li id="anchors99">
                <asp:HyperLink ID="hlinkHistory" runat="server"><span>历史采购单</span></asp:HyperLink></li>
        </ul>
    </div>
    <!--选项卡-->
    <div class="dataarea mainwidth">
        <!--搜索-->
        <div class="searcharea clearfix">
            <ul>
                <li><span>选择时间段：</span><span>
                    <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                </span><span class="Pg_1010">至</span> <span>
                    <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                </span></li>
                <li><span>商品名称：</span><span>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" />
                </span></li>
            </ul>
            <div class="blank12 clearfix">
            </div>
            <ul>
                <li><span>订单编号：</span><span>
                    <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" />
                </span></li>
                <li><span>采购单编号：</span><span>
                    <asp:TextBox ID="txtPurchaseOrderId" runat="server" CssClass="forminput"></asp:TextBox>
                    <asp:Label ID="lblStatus" runat="server" Style="display: none;"></asp:Label>
                </span></li>
                <li>
                    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
                </li>
            </ul>
        </div>
        <!--结束-->
        <div class="functionHandleArea clearfix">
            <!--分页功能-->
            <div class="pageHandleArea">
                <ul>
                    <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                    </li>
                </ul>
            </div>
            <div class="pageNumber">
                <div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>
            </div>
            <!--结束-->
            <div class="blank8 clearfix">
            </div>
            <div class="batchHandleArea">
                <ul>
                    <li class="batchHandleButton">-- <span class="signicon"></span><span class="allSelect">
                        <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                            <a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span><Hi:ImageLinkButton
                                ID="btnBatchPayMoney" runat="server" Text="批量付款" DeleteMsg="将当前选中结果中筛选出未付款的采购单进行批量付款，是否继续？"
                                IsShow="true" />--</li>
                </ul>
            </div>
        </div>
        <!--数据列表区域-->
        <div class="datalist">
            <div>
                <input type="hidden" id="hidPurchaseOrderId" runat="server" />
                <asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId"
                    Width="100%">
                    <HeaderTemplate>
                        <table width=" 0" border="0" cellspacing="0">
                            <tr class="table_title">
                                <td width="24%" class="td_right td_left">
                                    收货人
                                </td>
                                <td width="25%" class="td_right td_left">
                                    订单实收款(元)
                                </td>
                                <td width="22%" class="td_right td_left">
                                    采购单实收款(元)
                                </td>
                                <td width="18%" class="td_right td_left">
                                    采购状态
                                </td>
                                <td width="12%" class="td_left td_right_fff">
                                    操作
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="td_bg">
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%# Eval("PurchaseOrderId")%> '>采购单编号：<%# Eval("PurchaseOrderId")%>
                            </td>
                            <td>
                                订单编号：<%# Eval("OrderId")%>
                            </td>
                            <td>
                                提交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%# Eval("PurchaseDate")%> ' ShopTime="true"
                                    runat="server"></Hi:FormatedTimeLabel>
                            </td>
                            <td>&nbsp;
                                <%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "物流单编号：" + Eval("ShipOrderNumber")%>
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                                <%# Eval("ShipTo")%>
                            </td>
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%# Eval("OrderTotal")%> ' runat="server" />
                            </td>
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%# Eval("PurchaseTotal")%> ' runat="server" />
                            </td>
                            <td>
                                <table border="0" style="border: none;" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="border: none;">&nbsp;
                                            <Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus")%> '
                                                Font-Bold="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: none;">&nbsp;
                                            <span class="Name float">                                                
                                                <a target="_blank" href='<%# "MyPurchaseOrderDetails.aspx?purchaseOrderId="+Eval("PurchaseOrderId") %>'>详情</a>
                                            </span>
                                            <span>
                                                <Hi:DistributorChangePurchaseOrderItemsHyperLink ID="lkbtnUpdatePurchaseOrder" 
                                                PurchaseStatusCode='<%# Eval("PurchaseStatus")%> '
                                                    PurchaseOrderId='<%# Eval("PurchaseOrderId")%> ' Target="_blank" Text="修改" runat="server" /></span>
                                            <span>
                                                <asp:Literal ID="litPayment" runat="server"></asp:Literal></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <span class="submit_faihuo">
                                    <asp:HyperLink ID="lkbtnPay" runat="server" 
                                    NavigateUrl='<%# "ChoosePayment.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId")+"&PayMode="+Eval("PaymentTypeId")%> '
                                        Target="_blank" Text="付款" Visible="false"></asp:HyperLink></span> <span class="submit_tongyi">
                                            <asp:HyperLink ID="lkbtnSendGoods" runat="server" 
                                            NavigateUrl='<%#Globals.ApplicationPath +  "/Shopadmin/sales/SendMyGoods.aspx?OrderId="+ Eval("OrderId")%> '
                                                Target="_blank" Text="辅助发货" Visible="false"></asp:HyperLink>
                                        </span>
                                <div runat="server" visible="false" id="lkBtnCancelPurchaseOrder">
                                    <span class="submit_tongyi"><a href="javascript:ShowCloseDiv('<%# Eval("PurchaseOrderId")%>');">
                                        取消采购</a></span></div>
                                &nbsp;
                                <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" Text="确认采购单"
                                    CommandArgument='<%# Eval("PurchaseOrderId")%> ' CommandName="FINISH_TRADE" DeleteMsg="确认要完成该采购单吗？"
                                    Visible="false" ForeColor="Red" />
                                <a href="javascript:void(0)" onclick="return ApplyForPurchaseRefund(this.title)" runat="server"
                                    id="lkbtnApplyForPurchaseRefund" visible="false" title='<%# Eval("PurchaseOrderId")%> '>申请退款</a><br />
                                <a href="javascript:void(0)" onclick="return ApplyForPurchaseReturn(this.title)" runat="server"
                                    id="lkbtnApplyForPurchaseReturn" visible="false" title='<%# Eval("PurchaseOrderId")%> '>申请退货</a>
                                <a href="javascript:void(0)" onclick="return ApplyForPurchaseReplace(this.title)" runat="server"
                                    id="lkbtnApplyForPurchaseReplace" visible="false" title='<%# Eval("PurchaseOrderId")%> '>申请换货</a>
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
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="databottom">
    </div>
    <!--关闭采购单-->
    <div class="Pop_up" id="ClosePurchaseOrder" style="display: none;">
        <h1>
            关闭采购单
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform fonts colorA borbac">
            <strong>取消采购单?请选择取消采购单的理由：</strong></div>
        <div class="mianform ">
            <ul>
                <li><span class="formitemtitle Pw_160">取消该采购单的理由：</span>
                    <abbr class="formselect">
                        <Hi:DistributorClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
                    </abbr>
                </li>
            </ul>
            <ul class="up Pa_160">
                <asp:Button ID="btnClosePurchaseOrder" runat="server" CssClass="submit_DAqueding"
                    OnClientClick="return ValidationCloseReason()" Text="确 定" />
            </ul>
        </div>
    </div>
    <div class="Pop_up" id="refund_div" style="display: none">
        <h1>
            申请退款
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li>1.您需要选择我们如何将退款返回给您，如果选择预存款，金额将退到预存款，如果选择银行转账，请填写银行账号的相关信息。</li>
                <li><span class="formitemtitle Pw_198">选择退款途径：<em>*</em></span> <span>
                    <abbr class="formselect">
                        <asp:DropDownList ID="dropRefundType" runat="server">
                            <asp:ListItem Value="1">退到预存款</asp:ListItem>
                            <asp:ListItem Value="2">银行转帐</asp:ListItem>
                        </asp:DropDownList>
                    </abbr>
                </span></li>
                <li><span class="formitemtitle Pw_198">申请原因以及退款帐号：<em>*</em><br />
                    （银行转账，请填写您的银行账号相关信息）</span> <span>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" CssClass="forminput"
                            Width="300px" Height="70px"></asp:TextBox></span> </li>
                <li style="text-align: center;">
                    <asp:Button ID="btnOk" runat="server" Text="确 定" OnClientClick="return validatorForm()" CssClass="submit_DAqueding" /></li>
            </ul>
        </div>
    </div>
    <div class="Pop_up" id="return_div" style="display: none">
        <h1>
            申请退货
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li>1.请在退货前与客服确认退货，才将商品寄出。务必把物流单号和物流公司填写于备注里面。<br />
                    2、您需要选择我们如何将退款返回给您，如果选择预存款，金额将退到预存款，如果选择银行转账，请填写银行账号的相关信息</li>
                <li><span class="formitemtitle Pw_198">选择退款途径：<em>*</em></span> <span>
                    <abbr class="formselect">
                        <asp:DropDownList ID="dropReturnRefundType" runat="server">
                            <asp:ListItem Value="1">退到预存款</asp:ListItem>
                            <asp:ListItem Value="2">银行转帐</asp:ListItem>
                        </asp:DropDownList>
                    </abbr>
                </span></li>
                <li><span class="formitemtitle Pw_198">申请原因，物流公司，物流单号，退款账号等：<em>*</em><br />
                    （银行转账，请填写您的银行账号相关信息）</span> <span>
                        <asp:TextBox ID="txtReturnRemark" runat="server" TextMode="MultiLine" CssClass="forminput"
                            Width="300px" Height="70px"></asp:TextBox></span> </li>
                <li style="text-align: center;">
                    <asp:Button ID="btnReturn" runat="server" Text="确 定" OnClientClick="return validatorForm()" CssClass="submit_DAqueding" /></li>
            </ul>
        </div>
    </div>
    <div class="Pop_up" id="replace_div" style="display: none">
        <h1>
            申请换货
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li>1.请在换货前仔细阅读换货说明并与客服确认换货，才将商品寄出。务必把物流单号和物流公司填写于备注里面</li>
                <li><span class="formitemtitle Pw_198">换货备注：<em>*</em></span> <span>
                    <asp:TextBox ID="txtReplaceRemark" runat="server" TextMode="MultiLine" CssClass="forminput"
                        Width="300px" Height="70px"></asp:TextBox></span> </li>
                <li style="text-align: center;">
                    <asp:Button ID="btnReplace" runat="server" Text="确 定" OnClientClick="return validatorForm()" CssClass="submit_DAqueding" /></li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function showOrderState() {
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

        $(document).ready(function () { showOrderState(); });

        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择取消的理由") {
                alert("请选择取消的理由");
                return false;
            }

            return true;
        }
        var type = "";
        function validatorForm() {
            if (type == "refund") {
                if ($("#ctl00_contentHolder_txtRemark").val().replace(/\s/g, "") == "") {
                    alert("请输入申请原因以及退款帐号");
                    return false;
                }
            }
            else if (type == "return") {
                if ($("#ctl00_contentHolder_txtReturnRemark").val().replace(/\s/g, "") == "") {
                    alert("请输入申请原因，物流公司，物流单号，退款账号等");
                    return false;
                }
            }
            else if (type == "replace") {
                if ($("#ctl00_contentHolder_txtReplaceRemark").val().replace(/\s/g, "") == "") {
                    alert("请输入换货备注");
                    return false;
                }
            }
            else if (type == "pay") {
                if ($("#ctl00_contentHolder_dropPayType").val() == "") {
                    alert("请选择支付方式");
                    return false;
                }
            }
            return true;
        }
        function ApplyForPurchaseRefund(purchaseOrderId) {
            type = "refund";
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DivWindowOpen(600, 400, "refund_div");
            return false;
        }
        function ApplyForPurchaseReturn(purchaseOrderId) {
            type = "return";
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DivWindowOpen(600, 400, "return_div");
            return false;
        }
        function ApplyForPurchaseReplace(purchaseOrderId) {
            type = "replace";
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DivWindowOpen(600, 400, "replace_div");
            return false;
        }
        function ShowCloseDiv(purchaseOrderId) {

            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);

            DivWindowOpen(550, 200, 'ClosePurchaseOrder');
        }

    </script>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="PurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.PurchaseOrderDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Items" Src="~/Admin/Ascx/PurchaseOrder_Items.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Admin/Ascx/PurchaseOrder_Charges.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_ShippingAddress" Src="~/Admin/Ascx/PurchaseOrder_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title title_height m_none td_bottom">
            <em>
                <img src="../images/02.gif" width="32" height="32" /></em>
            <h1 class="title_line">
                采购单详情</h1>
        </div>

        <div class="Purchase">
            <div>
                <table>
                <tr><td>               
                <strong class="fonts colorE">当前采购单(<asp:Literal ID="litPurchaseOrderId" runat="server" />)状态：<Hi:PuchaseStatusLabel ID="lblPurchaseOrderStatus" runat="server" /></strong>
                </td>
                <td><asp:Label runat="server" ID="lbCloseReason" Text="关闭原因："><asp:Label runat="server" ID="lbReason"></asp:Label></asp:Label></td>
                </tr>
                <tr>
                <td><ul>
                    <li></li>
                    <li>
                        分销商用户名：<asp:Literal runat="server" ID="litUserName" />
                        真实姓名：<asp:Literal runat="server" ID="litRealName" />
                        联系电话：<asp:Literal runat="server" ID="litUserTel" />
                        电子邮件：<asp:Literal runat="server" ID="litUserEmail" />
                        <asp:Literal runat="server" ID="litOrderId" />
                    </li>
                    <li>
                        <asp:Literal ID="litPayTime"  runat="server" />
                        <asp:Literal ID="litSendGoodTime"  runat="server" />
                        <asp:Literal ID="litFinishTime"  runat="server" />
                    </li>
                    <li class="Pg_8">
                        <span class="submit_btnxiugai"><a id="lkbtnEditPrice" runat="server" href="javascript:UpdatePrice();">修改价格</a></span> 
                        <span class="submit_btnbianji"><a href="javascript:ShowRemark();">备注</a></span>
                        <span class="submit_btnguanbi"><a id="lbtnClocsOrder" runat="server" href="javascript:ClosePurchaseOrder();">关闭采购单</a></span>
                        <span class="submit_faihuo"><asp:HyperLink runat="server" ID="lkbtnSendGoods" Text="发货" NavigateUrl="javascript:ShowSend()"></asp:HyperLink></span>
                    </li>
                </ul></td>
                </tr>
                </table>
            </div>
        </div>
        <div class="blank12 clearfix">
        </div>
        <div class="list">
            <cc1:PurchaseOrder_Items runat="server" ID="itemsList" />
            <cc1:PurchaseOrder_Charges ID="chargesList" runat="server" />
            <cc1:PurchaseOrder_ShippingAddress runat="server" ID="shippingAddress" />
        </div>
        
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <!--编辑备注信息-->
    <div id="RemarkPurchaseOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input110">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></p>
            <p>
                <span class="frame-span frame-input110">采购单编号：</span><asp:Literal ID="spanpurcharseOrderId" runat="server" /></p>
            <p>
                <span class="frame-span frame-input110">成交时间：</span><Hi:FormatedTimeLabel runat="server"
                    ID="lblpurchaseDateForRemark" /></p>
            <p>
                <span class="frame-span frame-input110">采购单实收款(元)：</span><em><Hi:FormatedMoneyLabel
                    ID="lblpurchaseTotalForRemark" runat="server" /></em></p>
            
                <span class="frame-span frame-input110">标志：<em>*</em></span><Hi:OrderRemarkImageRadioButtonList
                    runat="server" ID="orderRemarkImageForRemark" />
            
            <p>
                <span class="frame-span frame-input110">备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine"
                    runat="server" Width="300" Height="50" /></p>
        </div>
    </div>
    <!--关闭采购单-->
    <div id="closePurchaseOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>关闭交易?请您确认已经通知分销商,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em></p>
            <p>
                <span>关闭该采购单的理由：<em>*</em></span><Hi:ClosePurchaseOrderReasonDropDownList runat="server"
                    ID="ddlCloseReason" />
            </p>
        </div>
    </div>
    <!--修改配送方式-->
    <div id="setShippingMode" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input130">请选择新的配送方式:<em>*</em></span><Hi:ShippingModeDropDownList
                    runat="server" ID="ddlshippingMode" />
            </p>
        </div>
    </div>
    <!--修改价格-->
    <div id="EditPurchaseOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input110">采购单原实收款：<em><asp:Label ID="lblPurchaseOrderAmount"
                    Text="22" runat="server" /></em>元</span></p>
            <p>
                <span class="frame-span frame-input110">涨价或折扣：<em>*</em></span><asp:TextBox ID="txtPurchaseOrderDiscount"
                    runat="server" CssClass="forminput" onblur="ChargeAmount()" /></p>
            <b>负数代表折扣，正数代表涨价</b>
            <p>                                
                <span class="frame-span frame-input110">分销商实付：</span>
                <asp:Label ID="lblPurchaseOrderAmount1" Text="22" runat="server" /><span>+</span>
                <asp:Label ID="lblPurchaseOrderAmount2" Text="22" runat="server" /><span>=</span>
                <em>
                    <asp:Label ID="lblPurchaseOrderAmount3" Text="22" runat="server" />元 </em>
            </p>
            <b>分销商实付： 采购单原实收款+涨价或折扣</b>
        </div>
    </div>
    <div style="display: none">
        <asp:Button ID="btnClosePurchaseOrder" runat="server" CssClass="submit_DAqueding"
            Text="关闭采购单" />
        <asp:Button ID="btnEditOrder" runat="server" Text="修改采购单价格" CssClass="submit_DAqueding" />
        <asp:Button runat="server" ID="btnRemark" Text="编辑备注" CssClass="submit_DAqueding" />
        <asp:Button ID="btnMondifyShip" runat="server" CssClass="submit_DAqueding" Text="修改配送方式" />
        <input type="hidden" id="hdpurchaseorder" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var formtype = "";
        //修改价格
        function UpdatePrice() {
            formtype = "updateprice";
            arrytext = null;
            DialogShow("修改采购单价格", 'updateprice', 'EditPurchaseOrder', 'ctl00_contentHolder_btnEditOrder');
        }

        // 编辑备注
        function ShowRemark() {
            formtype = "remark";
            arrytext = null;
            DialogShow("修改备注", 'updateremark', 'RemarkPurchaseOrder', 'ctl00_contentHolder_btnRemark');
        }

         //发货
         function ShowSend() {
             var purchaseOrderId ='<%=Page.Request.QueryString["purchaseOrderId"] %>';
             DialogFrame("purchaseOrder/SendPurchaseOrderGoods.aspx?PurchaseOrderId=" + purchaseOrderId, '发货', null, null)
             }

        //关闭采购单
        function ClosePurchaseOrder() {
            formtype = "close";
            arrytext = null;
            DialogShow("关闭采购单", 'closepurchar', 'closePurchaseOrder', 'ctl00_contentHolder_btnClosePurchaseOrder');
        }

        //修改配送方式
        function UpdateShippMode() {
            formtype = "updatemode";
            arrytext = null;
            setArryText("ctl00_contentHolder_ddlshippingMode", $("#ctl00_contentHolder_ddlshippingMode").val())

            DialogShow("修改配送方式", 'updatemode', 'setShippingMode', 'ctl00_contentHolder_btnMondifyShip');
        }
        //关闭采购单验证
        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择退回的理由") {
                alert("请选择退回的理由");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }

        //修改配送方式验证
        function ValidationShippingMode() {
            var mode = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
            if (mode == "") {
                alert("请选择配送方式");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlshippingMode", mode);
            return true;
        }

      

        function chushi() {
//            if (document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value == "") {
//                document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount2").innerHTML = "0.00";
//            }
            initValid(new InputValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '折扣只能是数值，且不能超过2位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', -10000000, 10000000, '折扣只能是数值，不能超过10000000，且不能超过2位小数'));
        }

        $(document).ready(function () {
            chushi();
            var pathurl = $("#ctl00_contentHolder_itemsList_purchaseOrderItemUpdateHyperLink").attr("href"); //修改采购单
            $("#ctl00_contentHolder_itemsList_purchaseOrderItemUpdateHyperLink").attr("href", "javascript: DialogFrame('" + pathurl + "', '修改采购单商品', null, null);");

            pathurl = "purchaseOrder/MondifyAddressFrame.aspx?action=update&PurchaseOrderId=" + $("#ctl00_contentHolder_hdpurchaseorder").val().replace(/\s/g,""); //修改收货地址
            $("#ctl00_contentHolder_shippingAddress_lkBtnEditShippingAddress").attr("href","javascript:DialogFrame('"+pathurl+"','修改收货地址',620,320)");
        });


     

        function ChargeAmount() {
            var reg = /^\-?([1-9]\d*|0)(\.\d+)?$/;
            if (($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val() != "") && reg.test($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val())) {
                $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val());
                var amount1 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount").html());
                var amount2 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount2").html());

                var amount3 = amount1 + amount2;
                $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(amount3);
            }
        }


//        //验证
//        function validatorForm() {
//            switch (formtype) {
//                case "remark":
//                    arrytext = null;
//                    $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
//                    if ($radioId == null || $radioId == "undefined") {
//                        alert('请先标记备注');
//                        return false;
//                    }
//                    setArryText($radioId.id, "true");
//                    setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
//                    break;
//                case "close":
//                    return ValidationCloseReason();
//                    break;
//                case "updateprice":
//                    setArryText("ctl00_contentHolder_txtPurchaseOrderDiscount", $("#ctl00_contentHolder_txtPurchaseOrderDiscount").val());
//                    break;
//                case "updatemode":
//                    return ValidationShippingMode();
//                    break;
//            };
//            return true;
//        }
    </script>
</asp:Content>

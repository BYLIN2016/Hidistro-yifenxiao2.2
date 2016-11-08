<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ManualPurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManualPurchaseOrderDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="ManualPurchaseOrder_Items" Src="~/Shopadmin/Ascx/ManualPurchaseOrder_Items.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Shopadmin/Ascx/PurchaseOrder_Charges.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_ShippingAddress" Src="~/Shopadmin/Ascx/PurchaseOrder_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"><em><img src="../images/003.gif" width="32" height="32" /></em>
      <h1 class="title_line">采购单详情</h1>
</div>
    <div class="Purchase">
      <div class="State">
        <ul>
        	<li><strong class="fonts colorE">当前采购单(<asp:Literal ID="litPurchaseOrderId" runat="server" />)状态：<Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus"  /></strong>
                <asp:Label runat="server" ID="lbCloseReason" Text="关闭原因："><asp:Label runat="server" ID="lbReason"></asp:Label></asp:Label>
                <asp:Label runat="server" ID="lbPurchaseOrderReturn" Text="退款金额:"><Hi:FormatedMoneyLabel ID="lblPurchaseOrderRefundMoney" runat="server"></Hi:FormatedMoneyLabel></asp:Label>
            </li>
            <li class="Pg_8">
                <span class="submit_faihuo"><asp:HyperLink runat="server" ID="lkbtnPay" Text="付款" /></span>
                <span class="submit_btnguanbi"><a runat="server" id="lkbtnClosePurchaseOrder" href="javascript:DivWindowOpen(400, 220, 'ClosePurchaseOrder')">取消采购</a></span>
           </li>                     
        </ul>
      </div>
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
       <cc1:ManualPurchaseOrder_Items runat="server" ID="itemsList" />
       <div><asp:HyperLink runat="server" ID="hlkOrderGifts" Text="添加礼品" /></div>
       <cc1:PurchaseOrder_Charges  ID="chargesList" runat="server" />
       <cc1:PurchaseOrder_ShippingAddress runat="server" ID="shippingAddress" />  
  </div>
  </div>
  
  
   <!--关闭采购单-->
<div class="Pop_up" id="ClosePurchaseOrder" style="display:none;">
  <h1>关闭采购单 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac"><strong>取消采购单?请选择取消采购单的理由：</strong></div>
  <div class="mianform">
    <ul>
      <li><span class="formitemtitle Pw_160">取消该采购单的理由：</span> <abbr class="formselect">
        <Hi:DistributorClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
      </abbr> </li>
    </ul>
    <ul class="up Pa_160">
      <asp:Button ID="btnClosePurchaseOrder"  runat="server" CssClass="submit_DAqueding" OnClientClick="return ValidationCloseReason()" Text="确 定"   />
    </ul>
  </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
 function ValidationCloseReason() {
                     var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
                     if (reason == "请选择取消的理由") {
                         alert("请选择取消的理由");
                         return false;
                     }

                     return true;
                 }                 

</script>
</asp:Content>

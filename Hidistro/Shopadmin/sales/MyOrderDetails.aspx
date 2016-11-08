<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyOrderDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Shopadmin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ChargesList" Src="~/Shopadmin/Ascx/Order_ChargesList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Shopadmin/Ascx/Order_ShippingAddress.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <em><img src="../images/05.gif" width="32" height="32" /></em>
      <h1 class="title_line">订单详情</h1>
</div>
    <div class="Purchase">
      <div class="State">
        <ul>
        	<li><strong class="fonts colorE">
                当前订单（<asp:Literal runat="server" ID="litOrderId" />）状态：<Hi:OrderStatusLabel ID="lblOrderStatus" runat="server" /></strong>
                &nbsp<asp:Label runat="server" ID="lbCloseReason" Text="关闭原因："><asp:Label runat="server" ID="lbReason"></asp:Label></asp:Label>
            </li>
            <li>
                会员名：<asp:Literal runat="server" ID="litUserName" />
                真实姓名：<asp:Literal runat="server" ID="litRealName" />
                联系电话：<asp:Literal runat="server" ID="litUserTel" />
                电子邮件：<asp:Literal runat="server" ID="litUserEmail" />
            </li>
            <li>
                <asp:Literal ID="litPayTime"  runat="server" />
                <asp:Literal ID="litSendGoodTime"  runat="server" />
                <asp:Literal ID="litFinishTime"  runat="server" />
            </li>
            <li class="Pg_8"><span class="submit_btnxiugai"><asp:HyperLink runat="server" ID="lkbtnEditPrice"   Height="25px" Text="修改价格" /></span>
                <span class="submit_btnbianji"><a href="javascript:DivWindowOpen(550, 350, 'RemarkOrder')">备注</a></span>
                 <span class="submit_btnguanbi"><a id="lbtnClocsOrder" runat="server" href="javascript:DivWindowOpen(550, 250, 'closeOrder')" >关闭订单</a></span>
                 <span class="submit_faihuo"><asp:HyperLink runat="server" ID="lkbtnSendGoods" Text="发货" NavigateUrl="javascript:ShowSend()"></asp:HyperLink></span>
           </li>                     
        </ul>
      </div>
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
     <cc1:Order_ItemsList  runat="server" ID="itemsList" />
	  <asp:HyperLink runat="server" ID="hlkOrderGifts" Text="添加订单礼品" ForeColor="blue" />
	 <cc1:Order_ChargesList  ID="chargesList" runat="server" />
<cc1:Order_ShippingAddress runat="server" ID="shippingAddress" />
  </div>
  </div>





  <div class="Pop_up" id="RemarkOrder"  style="display:none;">
  <h1>编辑备注信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></li>
             
       <li><span class="formitemtitle Pw_128">成交时间：</span><Hi:FormatedTimeLabel runat="server" ID="lblorderDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblorderTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">标志：</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">备忘录：</span>
          <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" />
              </li>
        </ul>
         <ul class="up Pa_100">
         <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding"/>
       </ul>
  </div> 
</div>

<div class="Pop_up" id="closeOrder" style="display:none;">
  <h1>关闭订单 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac">关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</strong></div>
  <div class="mianform ">
    <ul>
      <li><span class="formitemtitle Pw_160">关闭该订单的理由：</span> <abbr class="formselect">
        <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" />
      </abbr> </li>
    </ul>
    <ul class="up Pa_160">
      <asp:Button ID="btnCloseOrder"  runat="server" CssClass="submit_DAqueding" OnClientClick="return ValidationCloseReason()" Text="确 定"   />
    </ul>
  </div>
</div>


  <!--修改配送方式-->
<div class="Pop_up" id="setShippingMode" style="display:none;">
  <h1>修改配送方式 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform ">
    <ul>
              <li><span class="formitemtitle Pw_160">请选择新的配送方式：</span>
                <abbr class="formselect">
                   <Hi:ShippingModeDropDownList runat="server" ID="ddlshippingMode" />
                   </abbr>
              </li>
        </ul>
        <ul class="up Pa_160">
      <asp:Button ID="btnMondifyShip"  runat="server" CssClass="submit_DAqueding" Text="确 定" OnClientClick="return ValidationModeName()"  />
  </ul>
  </div>
</div>
  

<div class="Pop_up" id="setPaymentMode" style="display:none; ">
  <h1>修改支付方式 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform ">
    <ul>
              <li><span class="formitemtitle Pw_160">请选择新的支付方式：</span>
                <abbr class="formselect">
                   <Hi:DistributorPaymentDropDownList runat="server" ID="ddlpayment" />
                   </abbr>
              </li>
        </ul>
        <ul class="up Pa_160">
      <asp:Button ID="btnMondifyPay"  runat="server" CssClass="submit_DAqueding" Text="确 定" OnClientClick="return ValidationPayment()"  />
  </ul>
  </div>
</div>


    <div class="Pop_up" id="dlgShipTo" style="display:none;">
  <h1>修改收货信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
            <li> <span class="formitemtitle Pw_100">收货人姓名：</span>
               <asp:TextBox ID="txtShipTo" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">收货人地址：</span>
               <span><Hi:RegionSelector runat="server" id="dropRegions" /></span>      
              
            </li>
            </ul>
            <ul>
            <li class="clearfix"> <span class="formitemtitle Pw_100">详细地址：</span>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput" TextMode="multiLine"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">邮政编码：</span>
                <asp:TextBox ID="txtZipcode" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">电话号码：</span>
               <asp:TextBox ID="txtTelPhone" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">手机号码：</span>
                <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
    </ul>
        <ul class="up Pa_100">
      <asp:Button ID="btnMondifyAddress"  runat="server" Text="修改" OnClientClick="return ValidationAddress()" CssClass="submit_DAqueding" />
  </ul>
  </div>
</div>
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
         function ValidationAddress() {
             var shipTo = document.getElementById("ctl00_contentHolder_txtShipTo").value;
             if (shipTo.length < 2 || shipTo.length > 20) {
                 alert("收货人名字不能为空，长度在2-20个字符之间");
                 return false;
             }
             var address = document.getElementById("ctl00_contentHolder_txtAddress").value;
             if (address.length < 3 || address.length > 200) {
                 alert("详细地址不能为空，长度在3-200个字符之间");
                 return false;
             }          
             var telPhone = document.getElementById("ctl00_contentHolder_txtTelPhone").value;               
            var cellPhone = document.getElementById("ctl00_contentHolder_txtCellPhone").value;              
            if(telPhone.length==0&&cellPhone.length==0){
                alert("电话号码和手机号码必须填写一项！");
                return false;
            }
            
            var selectedRegionId = GetSelectedRegionId();
            if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
                alert("请选择您的收货人地址");
                return false;
            }
            
            return true;
         }

         function ValidationCloseReason() {
             var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
             if (reason == "请选择关闭的理由") {
                 alert("请选择关闭的理由");
                 return false;
             }

             return true;
         }

         function ValidationModeName() {
             var reason = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
             if (reason == "") {
                 alert("请选择配送方式");
                 return false;
             }

             return true;
         }

         function ValidationPayment() {
             var reason = document.getElementById("ctl00_contentHolder_ddlpayment").value;
             if (reason == "") {
                 alert("请选择支付方式");
                 return false;
             }

             return true;
         }
          function ShowSend() {
             var orderId = <% =Page.Request.QueryString["OrderId"] %>;
             DialogFrame("SendMyGoods.aspx?OrderId=" + orderId, '发货', null, null)
             }
         </script>
</asp:Content>

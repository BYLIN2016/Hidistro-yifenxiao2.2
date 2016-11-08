<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SendGoods"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title title_height m_none td_bottom"> <em><img src="../images/05.gif" width="32" height="32" /></em>
    <h1 class="title_line">订单发货</h1>
  </div>
  <div class="Purchase">
    <div class="State" style="width:auto;padding: 11px 12px 10px;">
     <h1>订单详情</h1>
      <table width="100%" border="0" cellspacing="0">
        <tr style="background:#f0f0f0">
          <td width="8%" >订单编号：</td>
          <td width="20%"><asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
          <td width="8%" >创建时间：</td>
          <td width="28%"><Hi:FormatedTimeLabel runat="server" ID="lblOrderTime" ></Hi:FormatedTimeLabel></td>
          <td width="10%" >&nbsp;</td>
          <td width="20%">&nbsp;</td>
        </tr>
      </table>
	  </div>
    </div>
    
   <div class="list">
          <h1>发货</h1>
      <div class="Settlement">
        <style>
            .databody .list .Settlement table{ border:0px}
        </style>
         <table width="100%" border="0" cellspacing="0"  class="br_none" >
          <tr>
            <td width="10%" >配送方式：</td>
            <td class="a_none"><Hi:ShippingModeRadioButtonList AutoPostBack="true" ID="radioShippingMode" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" class="br_none" /></td>
           </tr>
          <tr>
            <td width="10%">物流公司：</td>
            <td class="a_none"><Hi:ExpressRadioButtonList runat="server" RepeatColumns="6" RepeatDirection="Horizontal" ID="expressRadioButtonList" /></td>
          </tr>
          <tr>
            <td >运单号码：</td>
            <td class="a_none"><asp:TextBox runat="server" ID="txtShipOrderNumber" class="forminput"/> <span id="txtShipOrderNumberTip" runat="server" style="line-height:30px; color:red">&nbsp;运单号码不能为空，在1至20个字符之间</span></td>
           </tr>
            
       </table>
         </div>
      <div class="bnt Pa_100 Pg_15 Pg_18" style=" padding-left:82px;">
        <asp:Button ID="btnSendGoods" runat="server" Text="确认发货" class="submit_DAqueding" />
        </div>  
   </div>
    
    <div class="blank12 clearfix"></div>
    <div class="list">
    <cc1:Order_ItemsList  runat="server" ID="itemsList" />
    </div>
    <div class="blank12 clearfix"></div>
	<div class="list">
    
     <h1>物流信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">买家选择：</td>
            <td colspan="2" class="a_none"><asp:Literal runat="server" ID="litShippingModeName" /></td>
          </tr>
          <tr>
            <td align="right">收货地址：</td>
            <td width="65%" class="a_none"><asp:Literal runat="server" ID="litReceivingInfo" /></td>
            <td width="10%" class="a_none"><span class="Name"><a href="javascript:UpdateShippAddress('<%=Page.Request.QueryString["OrderId"] %>')">修改收货地址</a></span></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">送货上门时间：</td>
            <td colspan="2" class="a_none"><asp:Label ID="litShipToDate"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2" class="a_none">&nbsp; <asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
        </table>
        </div>
  </div>
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtShipOrderNumber', 1, 20, false, null, '运单号码不能为空，在1至20个字符之间'));
    }
    $(document).ready(function () { InitValidators(); });


    function UpdateShippAddress(ordernumber){
        var pathurl = "sales/ShippAddress.aspx?action=update&orderId=" + ordernumber;
        DialogFrame(pathurl,"修改收货地址",640,300);   
    }
</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SendMyGoods.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SendMyGoods" %>
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
  <div class="title title_height m_none td_bottom"> <em><img src="../images/05.gif" width="32" height="32" /></em>
    <h1 class="title_line">订单发货</h1>
  </div>
  <div class="Purchase">
    <div class="State">
     <h1>订单详情</h1>
      <table width="200" border="0" cellspacing="0">
        <tr>
          <td width="10%" align="right">订单编号：</td>
          <td width="20%"><asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
          <td width="10%" align="right">创建时间：</td>
          <td width="28%"><Hi:FormatedTimeLabel runat="server" ID="lblOrderTime" ></Hi:FormatedTimeLabel></td>
          <td width="10%" align="right">&nbsp;</td>
          <td width="20%">&nbsp;</td>
        </tr>
      </table>
	  </div>
    </div>
    <div class="list">
           <h1>发货</h1>
      <div class="Settlement">
         <table width="200" border="0" cellspacing="0"  class="br_none">
          <tr>
            <td width="15%" align="right">配送方式：</td>
            <td class="a_none"><Hi:ShippingModeRadioButtonList AutoPostBack="true" ID="radioShippingMode" runat="server" RepeatDirection="Horizontal" RepeatColumns="16" class="br_none" /></td>
           </tr>
          <tr>
            <td width="15%" align="right"  nowrap="nowrap">物流公司：</td>
            <td class="a_none"><Hi:ExpressRadioButtonList runat="server" RepeatColumns="6" RepeatDirection="Horizontal" ID="expressRadioButtonList" /></td>
          </tr>
          <tr>
            <td align="right">运单号码：</td>
            <td class="a_none"><asp:TextBox runat="server" ID="txtShipOrderNumber" /></td>
           </tr>
           <tr>
           <td>&nbsp;
           </td>
           <td>
           <p id="txtShipOrderNumberTip" runat="server">运单号码不能为空，在1至20个字符之间</p>
           </td>
           </tr>
       </table>
         </div>
      <div class="bnt Pa_140 Pg_15 Pg_18">
        <asp:Button ID="btnSendGoods" runat="server" Text="确认发货" class="submit_DAqueding" />
        </div>   
    </div>
    
  <div class="blank12 clearfix"></div>
	<div class="list">
    <cc1:Order_ItemsList  runat="server" ID="itemsList" />
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
            <td width="10%" class="a_none"></td>
          </tr>
           <tr>
            <td align="right" nowrap="nowrap">送货上门时间：</td>
            <td colspan="2" class="a_none"><asp:Label ID="litShipToDate"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
          <tr>
            <td align="right">买家留言：</td>
            <td colspan="2" class="a_none">&nbsp; <asp:Literal runat="server"  ID="litRemark" /></td>
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
             initValid(new InputValidator('ctl00_contentHolder_txtShipOrderNumber', 1, 20, false, null, '运单号码不能为空，在1至20个字符之间'))
         }
         $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>

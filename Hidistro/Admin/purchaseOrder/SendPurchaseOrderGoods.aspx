<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SendPurchaseOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SendPurchaseOrderGoods"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Items" Src="~/Admin/Ascx/PurchaseOrder_Items.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Admin/Ascx/PurchaseOrder_Charges.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <h1 class="title_line">采购单发货</h1>
  </div>
    <div class="Purchase">
      <div class="StepsC">
        <ul>
        	<li><strong class="fonts">第<span class="colorG">1</span>步</strong> 分销商已下单</li>
            <li><strong class="fonts">第<span class="colorG">2</span>步</strong> 分销商付款</li>
            <li><strong class="fonts colorP">第3步</strong> <span class="colorO">发货</span></li>
            <li><strong class="fonts">第<span class="colorG">4</span>步</strong> 交易完成</li>                                          
        </ul>
      </div>    
    </div>
    
    <div class="list">
     <h1>发货</h1>
      <div class="Settlement">
        <table width="200" border="0" cellspacing="0"  class="br_none">
          <tr>
            <td width="15%" align="right">配送方式：</td>
            <td >
              <Hi:ShippingModeRadioButtonList ID="radioShippingMode" AutoPostBack="true" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" class="br_none" /></td>
          </tr>
          <tr>
            <td width="15%" align="right"  nowrap="nowrap">物流公司：</td>
            <td class="a_none"><Hi:ExpressRadioButtonList runat="server" RepeatColumns="6" RepeatDirection="Horizontal" ID="expressRadioButtonList" /></td>
          </tr>
          <tr>
            <td align="right">运单号码：</td>
            <td><asp:TextBox runat="server" ID="txtShipOrderNumber" /></td>
          </tr>
        </table>
     </div>  
      <div class="bnt Pa_140 Pg_15 Pg_18">
       <asp:Button ID="btnSendGoods" runat="server" Text="确认发货" CssClass="submit_DAqueding" />
      </div>  
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
<cc1:PurchaseOrder_Items runat="server" ID="itemsList" />
  <h1>采购单实收款结算</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">运费(元)：</td>
            <td width="12%"><asp:Literal ID="litFreight" runat="server" /> (<asp:Literal ID="lblModeName" runat="server" />)</td>
            <td width="73%">&nbsp;</td>
          </tr>
          <tr>
            <td align="right">涨价或折扣(元)： </td>
            <td colspan="2" class="colorB"><asp:Literal ID="litDiscount" runat="server" /></td>
          </tr>
          <tr class="bg">
            <td align="right" class="colorG">采购单实收款(元)：</td>
            <td colspan="2"> <strong class="colorG fonts"><asp:Literal ID="litTotalPrice" runat="server" /></strong></td>
          </tr>
        </table>
    </div>
        <h1>收货信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">收货地址：</td>
            <td width="29%" class="a_none"><asp:Literal runat="server" ID="litReceivingInfo" /></td>
            <td width="56%" class="a_none"><span class="Name"><a href="javascript:DialogFrame('purchaseOrder/MondifyAddressFrame.aspx?action=update&PurchaseOrderId=<%=Page.Request.QueryString["PurchaseOrderId"] %>','修改收货地址',645,315);" >修改收货地址</a></span></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">送货上门时间：</td>
            <td colspan="2" class="a_none"><asp:Label ID="litShipToDate"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
          <tr>
            <td align="right">买家选择：</td>
            <td colspan="2" class="a_none"><asp:Literal runat="server" ID="litShippingModeName" /></td>
          </tr>
          <tr>
            <td align="right">买家备注：</td>
            <td colspan="2" class="a_none">  <asp:Literal runat="server"  ID="litRemark" />&nbsp;</td>
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

</asp:Content>


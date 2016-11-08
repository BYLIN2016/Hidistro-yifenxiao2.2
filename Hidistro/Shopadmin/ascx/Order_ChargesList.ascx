<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ChargesList.ascx.cs" Inherits="Hidistro.UI.Web.Shopadmin.Order_ChargesList" %>

  
          <div class="Settlement">
          <table width="200" border="0" cellspacing="0">
            <tr>
              <td align="right">运费(元)： </td>
              <td><asp:Literal ID="litFreight" runat="server" />(<asp:Literal ID="litShippingMode" runat="server"></asp:Literal>)&nbsp;<asp:HyperLink ID="hlkFreightFreePromotion" runat="server" Target="_blank" /></td>
              <td>&nbsp;<span class="Name"><asp:LinkButton runat="server" visible="false" ID="lkBtnEditshipingMode" Text="修改配送方式" OnClientClick="javascript:DivWindowOpen(550, 180, 'setShippingMode'); return false;" /></span></td>
            </tr>
            <tr>
              <td align="right">支付手续费(元)：</td>
              <td><asp:Literal ID="litPayCharge" runat="server" />&nbsp;(<asp:Literal ID="litPayMode" runat="server"></asp:Literal>)</td>
              <td>&nbsp;<span class="Name"><asp:LinkButton runat="server" ID="lkBtnEditPayMode"  Text="修改支付方式" OnClientClick="javascript:DivWindowOpen(550, 180, 'setPaymentMode'); return false;" Visible="false" /></span></td>
            </tr>
            <tr>
              <td align="right">优惠券折扣(元)：</td>
              <td colspan="2" ><span class="colorB"><asp:Literal ID="litCouponValue" runat="server" Visible="false" /><asp:Literal ID="litCoupon" runat="server" /></span></td>
            </tr>
            <tr>
              <td align="right">涨价或减价(元)：</td>
              <td><span class="colorB"><asp:Literal ID="litDiscount" runat="server" /></span></td>
              <td>为负代表折扣，为正代表涨价 </td>
            </tr>
            <asp:Literal ID="litTax" runat="server" />
            <asp:Literal ID="litInvoiceTitle" runat="server" />
            <tr>
              <td align="right">订单可得积分：</td>
              <td colspan="2" class="colorA"><asp:Literal ID="litPoints" runat="server" />&nbsp;<asp:HyperLink ID="hlkSentTimesPointPromotion" runat="server" Target="_blank" /></td>
            </tr>
            <tr class="bg">
              <td align="right" class="colorG">订单实收款(元)：</td>
              <td colspan="2"><strong class="colorG fonts"><asp:Literal ID="litTotalPrice" runat="server" /></strong></td>
            </tr>
          </table>
  </div>
  
  
  
  
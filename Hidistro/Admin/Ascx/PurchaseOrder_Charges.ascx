<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrder_Charges.ascx.cs" Inherits="Hidistro.UI.Web.Admin.PurchaseOrder_Charges" %>

<h1>采购单实收款结算</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">运费(元)：</td>
            <td width="12%"><asp:Literal ID="litFreight" runat="server" />&nbsp; (<asp:Literal ID="lblModeName" runat="server" />)</td>
            <td width="73%"><span class="Name"><a runat="server" visible="false" id="lkBtnEditshipingMode" href="javascript:UpdateShippMode();">修改配送方式</a></span></td>
          </tr>
          <tr>
            <td align="right">涨价或折扣(元)： </td>
            <td colspan="2" class="colorB"><asp:Literal ID="litDiscount" runat="server" /></td>
          </tr>
          <asp:Literal ID="litTax" runat="server" />
            <asp:Literal ID="litInvoiceTitle" runat="server" />
          <tr class="bg">
            <td align="right" class="colorG">采购单实收款(元)：</td>
            <td colspan="2"> <strong class="colorG fonts"><asp:Literal ID="litTotalPrice" runat="server" /></strong></td>
          </tr>
        </table>
    </div>
    

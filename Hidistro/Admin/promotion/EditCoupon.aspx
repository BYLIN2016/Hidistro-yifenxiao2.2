<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditCoupon.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditCoupon" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1>编辑优惠券</h1>
            <span>优惠券编辑</span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li>  <asp:Label ID="lblEditCouponId" runat="server" style="display:none"></asp:Label><span class="formitemtitle Pw_100">优惠券名称：<em >*</em></span>
          <asp:TextBox ID="txtCouponName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtCouponNameTip">名称不能为空，在1至60个字符之间</p>
          </li>
          <li> <span class="formitemtitle Pw_100">满足金额：</span>
             <asp:TextBox ID="txtAmount" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtAmountTip"></p>
          </li>
          <li> <span class="formitemtitle Pw_100">可抵扣金额：<em >*</em></span>
            <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtDiscountValueTip"></p>
          </li>
          
           <li>
            <span class="formitemtitle Pw_100">开始日期：<em >*</em></span>
            <UI:WebCalendar ID="calendarStartDate"  runat="server" cssclass="forminput" />       
          </li>     
          
          <li>
            <span class="formitemtitle Pw_100">结束日期：<em >*</em></span>
            <UI:WebCalendar ID="calendarEndDate" runat="server" cssclass="forminput" />       
          </li>     
          <li>
            <span class="formitemtitle Pw_100">兑换需积分：<em >*</em></span>
            <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
            <p id="P1">兑换所需积分只能是数字，必须大于等于O,0表示不能兑换</p>
	      </li>     
      </ul>
      <ul class="btn Pa_100 clear">
         <asp:Button ID="btnEditCoupons" runat="server" Text="保 存"  CssClass="submit_DAqueding"  />
        </ul>
      </div>

      </div>
  </div>






</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_txtCouponName', 1, 60, false, null,  '优惠券的名称，在1至60个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtAmount', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '满足金额只能是数值，0.01-10000000，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAmount', 0.01, 10000000.00, ' 满足金额只能是数值，0.01-10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtDiscountValue', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', ' 可抵扣金额只能是数值，0.01-10000000，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtDiscountValue', 0.01, 10000000.00, '可抵扣金额只能是数值，0.01-10000000，且不能超过2位小数'));
}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>

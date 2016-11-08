<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ReCharge.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ReCharge" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="toptitle td_bottom_ccc"><em><img src="../images/09.gif" width="32" height="32" /></em>
    <h1 >预付款充值</h1>
  <span >通过在线支付方式为自己的预付款帐户充值预付款.</span></div>
<div class="blank12 clearfix"></div>
  <div class="areaform validator1">
	    <ul>
	      
	      <li class="Pa_15"><span class="formitemtitle Pw_198">真实姓名：</span>
          <strong><asp:Literal runat="server" ID="litRealName"></asp:Literal></strong></li>
	      <li class="Pa_15"><span class="formitemtitle Pw_198">预付款可用余额：</span>
          <strong class="colorA fonts"><Hi:FormatedMoneyLabel ID="lblUseableBalance" runat="server" /> </strong></li>
          <li><span class="formitemtitle Pw_198">选择充值方式：<em>*</em></span>
	        <span style="float:left"><Hi:PaymentRadioButtonList runat="server" ID="radioBtnPayment" /></span>
            </li>
            <li><span class="formitemtitle Pw_198">充值金额：<em>*</em></span>
               <asp:TextBox runat="server" ID="txtReChargeBalance" CssClass="forminput"></asp:TextBox>
             <p id="txtReChargeBalanceTip" runat="server">充值金额只能是数值，且不能超过2位小数</p>
            </li>
        </ul>
  </div>
  <div class="btn Pa_198">
    <asp:Button runat="server" ID="btnReChargeNext" OnClientClick="return PageIsValid();" CssClass="submit_DAqueding" Text="下一步" />
  </div>
<div class="btn Pa_198">
  <!--数据列表底部功能区域-->
</div>
</div>
<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
function InitValidators()
{
    initValid(new InputValidator('ctl00_contentHolder_txtReChargeBalance', 1, 10, false, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '充值金额只能是数值，且不能超过2位小数'))
    appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtReChargeBalance', 0.01, 10000000.00, ' 充值金额只能是数值，限制在1000万以内'));   
}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>

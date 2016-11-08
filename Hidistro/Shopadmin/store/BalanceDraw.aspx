<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" EnableSessionState="True" AutoEventWireup="true" CodeBehind="BalanceDraw.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.BalanceDraw" %>
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
    <h1 >申请提现</h1>
  <span >向管理员申请从自己的预付款帐户中提现.</span></div>
  <div class="blank12 clearfix"></div>
  <div class="areaform validator1">
	    <ul>
	      <li class="Pa_198 colorE fonts"><strong>预付款帐户信息</strong></li>
	      <li><span class="formitemtitle Pw_198">真实姓名：</span><strong><asp:Literal runat="server" ID="litRealName"></asp:Literal></strong></li>
	      <li><span class="formitemtitle Pw_198">预付款可用余额：</span>
          <strong class="colorA fonts"><Hi:FormatedMoneyLabel ID="lblUseableBalance" runat="server" /> </strong></li>
          <li><span class="formitemtitle Pw_198">提现金额：<em>*</em></span>
            <asp:TextBox runat="server" ID="txtDrawBalance" CssClass="forminput"></asp:TextBox>
                <p id="txtDrawBalanceTip" runat="server">提现金额只能是数值，且不能超过2位小数</p>
          </li>
            <li><span class="formitemtitle Pw_198">支付密码：<em>*</em></span>
              <asp:TextBox runat="server" ID="txtTradePassword" CssClass="forminput" TextMode="Password"></asp:TextBox>
                <p id="txtTradePasswordTip" runat="server">支付密码不能为空</p>
            </li>
           <li class="Pa_198 colorE fonts"><strong>提现帐号信息</strong></li>
	      <li><span class="formitemtitle Pw_198">开户银行名称：<em>*</em></span>
	        <abbr class="formselect">
                  <asp:TextBox runat="server" ID="txtBankName" CssClass="forminput"></asp:TextBox>
            </abbr>
	      </li>
	      <li><span class="formitemtitle Pw_198">开户人真实姓名：<em>*</em></span>
	        <asp:TextBox runat="server" ID="txtAccountName" CssClass="forminput"></asp:TextBox>
                <p id="txtAccountNameTip" runat="server">开户人真实姓名不能为空,长度限制在30字符以内</p>
	      </li>
          <li><span class="formitemtitle Pw_198">个人银行帐号：<em>*</em></span>
            <asp:TextBox runat="server" ID="txtMerchantCode" CssClass="forminput"></asp:TextBox>
                <p id="txtMerchantCodeTip" runat="server">个人银行帐号不能为空,只能是数字</p>
          </li>
            <li><span class="formitemtitle Pw_198">请再输入一遍：<em>*</em></span>
              <asp:TextBox runat="server" ID="txtMerchantCodeCompare" CssClass="forminput"></asp:TextBox>
                <p id="txtMerchantCodeCompareTip" runat="server">请确认帐号</p>
            </li>
        </ul>
  </div>
  <div class="btn Pa_198">
    <asp:Button runat="server" ID="btnDrawNext" CssClass="submit_DAqueding"  OnClientClick="return valBankName()" Text="下一步" />
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

    initValid(new InputValidator('ctl00_contentHolder_txtDrawBalance', 1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '提现金额只能是数值，且不能超过2位小数'))
    appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtDrawBalance', 0.01, 10000000.00, ' 提现金额只能是数值，限制在1000万以内'));
    initValid(new InputValidator('ctl00_contentHolder_txtTradePassword', 1, 30, false, null, ' 支付密码不能为空'))
    initValid(new InputValidator('ctl00_contentHolder_txtAccountName', 1, 30, false, null, ' 开户人真实姓名不能为空,长度限制在30字符以内'))
    initValid(new InputValidator('ctl00_contentHolder_txtMerchantCode', 1, 100, false, '-?[0-9]\\d*', '个人银行帐号不能为空,只能是数字'))
    initValid(new CompareValidator('ctl00_contentHolder_txtMerchantCodeCompare', 'ctl00_contentHolder_txtMerchantCode', '两次输入的帐号不一致,请重新输入'));
}
$(document).ready(function() { InitValidators(); });


function valBankName() {

    var reason = document.getElementById("ctl00_contentHolder_ddlBankName").value;
    if (reason == "请选择") {
        alert("请选择开户银行");
        return false;
    }

    return PageIsValid();
}
</script>
</asp:Content>

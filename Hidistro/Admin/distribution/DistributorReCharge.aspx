<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.DistributorReCharge" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnright">
          <div class="title"> 
              <em><img src="../images/05.gif" width="32" height="32" /></em>
              <h1>预付款账户加款</h1>
              <span>管理员手工给分销商的预付款账户加款</span>
           </div>
               <div class="datafrom">
    <div class="formitem validator3">
                <ul>
                  <li> <span class="formitemtitle Pw_128">分销商名称： </span><strong class="fonts"><asp:Literal ID="litUserNames" runat="server"></asp:Literal></strong></li>
                  <li> <span class="formitemtitle Pw_128">可用余额：</span><strong class="colorA fonts"><Hi:FormatedMoneyLabel runat="server" ID="lblUseableBalance" /> </strong></li>
                  <li> <span class="formitemtitle Pw_128">加款金额：<em >*</em></span>
                    <asp:TextBox ID="txtReCharge" runat="server" CssClass="forminput"></asp:TextBox>
                    <p id="txtReChargeTip" runat="server">本次充值要给当前客户加款的金额，金额大小正负1000万之间</p>                    
                  </li>
                  <li class="clear"> <span class="formitemtitle Pw_128">备注：</span> 
                    <span><asp:TextBox Width="400px" Height="120px" ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox></span>
                     <p id="txtRemarksTip" runat="server">备注用于记录本次加款相关的细节问题，长度限制在300个字符以内</p>
                  </li>
                </ul>
                <ul class="btntf Pa_128 clear">
                <asp:Button ID="btnReChargeOK" OnClientClick="return PageIsValid();" runat="server" Text="确定"  CssClass="submit_DAqueding inbnt"></asp:Button>
          </ul>
      </div>
    </div>
        </div>
</div>

  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtReCharge', 1, 10, false, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '本次充值要给当前客户加款的金额，金额大小正负1000万之间'));
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtReCharge', -10000000, 10000000, '本次充值要给当前客户加款的金额，金额大小正负1000万之间'));
        initValid(new InputValidator('ctl00_contentHolder_txtRemarks', 0, 300, true, null, '备注用于记录本次加款相关的细节问题，长度限制在300个字符以内'));
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>

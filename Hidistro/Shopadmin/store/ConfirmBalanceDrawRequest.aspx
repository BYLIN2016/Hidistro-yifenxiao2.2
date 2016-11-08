<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" EnableSessionState="True" AutoEventWireup="true" CodeBehind="ConfirmBalanceDrawRequest.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ConfirmBalanceDrawRequest"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
	  <div class="toptitle td_bottom_ccc"><em><img src="../images/09.gif" width="32" height="32" /></em>
	    <h1 >申请提现</h1>
	    <span >向管理员申请从自己的预付款帐户中提现.</span></div>
	  <div class="blank12 clearfix"></div>
  <div class="areaform">
	    <ul>
	      <li class="Pa_198 colorA fonts"><strong>尊敬的分销商,请确认下面的提现信息无误</strong></li>
	      <li><span class="formitemtitle Pw_198">真实姓名：</span><strong><asp:Literal runat="server" ID="litRealName"></asp:Literal></strong></li>
	      <li class="clear Pa_15"><span class="formitemtitle Pw_198">提现金额：</span>
          <strong class="colorA fonts"><Hi:FormatedMoneyLabel ID="lblAmount" runat="server" /> </strong></li>
          <li class="Pa_198 colorE fonts"><strong>提现帐号信息</strong></li>
	      <li><span class="formitemtitle Pw_198">开银行名称：</span><asp:Literal runat="server" ID="litBankName"></asp:Literal></li>
	      <li><span class="formitemtitle Pw_198">开户人真实姓名：</span><asp:Literal runat="server" ID="litName"></asp:Literal></li>
          <li><span class="formitemtitle Pw_198">个人银行帐号：</span><strong class="colorB"><asp:Literal runat="server" ID="litBankCode"></asp:Literal></strong></li>

    </ul>
  </div>
  <div class="btn Pa_198">
     <asp:Button runat="server" ID="btnOK" CssClass="submit_DAqueding" Text="确认提现" />
  </div>
<div class="btn Pa_198">
  <!--数据列表底部功能区域-->
</div>
</div>
<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>


<div class="Pop_up" runat="server" id="message" visible="false" style="z-index:1000; position:absolute;left:30%; top:10%; margin-left:-250px; margin-top:-250px; background-color:White;border:#767576 solid 6px; width:610px;margin:10% auto;padding:0px 15px 30px 15px;">
  <h1>申请提交完成</h1>
    <div class="img_datala"><a href="#" onclick="link()"><img src="../images/icon_dalata.gif" width="38" height="20" /></a></div>
  <div class="mianform">
  <div class="Failure">
    <table width="100%" border="0" cellspacing="0">
      <tr>
        <td align="center" valign="middle"><div class="colorB" id="u2_rtf"><strong>您的提现申请已提交，请等待管理员处理.</strong></div></td>
        </tr>
    </table>
    </div>
  <div class="Pg_20 colorA" style="text-align:center">提现款项暂时被冻结，请等待管理员处理！</div>
    <div class="Failure">
      <input type="button" name="button" id="button" onclick="link()" value="确 定" class="submit_DAqueding"/>
    </div>
  </div>
  <div class="up Pa_100">
      
  </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function link() {
        window.location.href = 'myaccountsummary.aspx';
    }
</script>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchPay.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.purchaseOrder.BatchPay"  MasterPageFile="~/Shopadmin/ShopAdmin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
     <div class="title m_none td_bottom title_height"><em><img src="../images/003.gif" width="32" height="32" /></em>
       <h1 class="title_line">给采购单付款 </h1>
    </div>
    <div class="datafrom">
<div class="formitem">
      <ul>
          <li><span class="formitemtitle Pw_160"></span><abbr class="colorE fonts">您使用的是预付款余额对采购单进行支付.请确保您的预付款可用余额足够支付采购单金额.</abbr></li>
            <li> <span class="formitemtitle Pw_160">预付款可用余额(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel runat="server" ID="lblUseableBalance" /></strong> <a href="../store/ReCharge.aspx">充值</a></li>
          </ul>
</div>
    </div>
    <div class="blank12 clearfix"></div>
    <div class="datafrom">
      <div class="formitem">
        <ul>
          <li> <span class="formitemtitle Pw_160">采购单实付款共计(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel runat="server" ID="lblTotalPrice" /></strong></li>
          <li> <span class="formitemtitle Pw_160">请输入您的交易密码：<em>*</em></span>
           <asp:TextBox runat="server" ID="txtTradePassword" TextMode="Password" CssClass="forminput" />           
          </li>
        </ul>
        <ul class="btntf Pa_160">
          <asp:Button ID="btnConfirmPay" CssClass="submit_DAqueding float" runat="server" Text="确认付款" OnClientClick="return CheckPassword();"/>
        </ul>
      </div>
    </div>
</div>

<div class="Pop_up" runat="server" id="PaySucceess" visible="false" style="z-index:1000; position:absolute;left:30%; top:10%; margin-left:-250px; margin-top:-250px; background-color:White;border:#767576 solid 6px; width:610px;margin:10% auto;padding:0px 15px 30px 15px;">
  <h1>采购单付款成功</h1>
    <div class="img_datala"><asp:ImageButton runat="server" ID="imgBtnBack" ImageUrl="../images/icon_dalata.gif" /></div>
  <div class="mianform">
  <div class="Failure">
    <table width="100%" border="0" cellspacing="0">
      <tr>
        <td align="center" valign="middle"><div class="colorB" id="u2_rtf"><strong>供应商已经收到了采购单的货款</strong></div></td>
        </tr>
    </table>
    </div>
  </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function CloseDiv(id) {
        var div = document.getElementById(id);
        div.style.display = "none";
    }
    
     $(document).ready(function() {

     $("#ctl00_contentHolder_txtTradePassword").keydown(function(e) {
            if (e.keyCode == 13) {
                $('#ctl00_contentHolder_btnConfirmPay').focus();
            }
        });

    });
    function CheckPassword(){
        if($("#ctl00_contentHolder_txtTradePassword").val().replace(/\s/g,"")==""){
            alert("请输入交易密码!");
            return false;
        }
    }
</script>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true" CodeBehind="MyDeductSettings.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyDeductSettings" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtDeduct', 1, 10, false, '-?[0-9]\\d*', '设置订单完成以后推荐人的提成比例'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtDeduct', 0, 100, '设置订单完成以后推荐人的提成比例'));
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1 class="title_line">会员推荐提成设置 </h1>
	    <span class="font">在此可设置站内会员推荐访客注册下单后的订单金额分成比例</span>
     </div>
     <div class="datafrom">
        <div class="formitem validator1">
          <ul>
            <li><span class="formitemtitle Pw_198">推荐人提成比例：<em >*</em></span>
              <asp:TextBox ID="txtDeduct" CssClass="forminput" runat="server"  />%
              <p id="txtDeductTip" runat="server">设置订单付款后推荐人的提成比例</p>
            </li>
          </ul>
           <div style="clear:both"></div>
           <ul class="btntf Pa_198">
            <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
	       </ul>
        </div>
      </div>
</div>
</asp:Content>

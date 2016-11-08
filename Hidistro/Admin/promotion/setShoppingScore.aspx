<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="setShoppingScore.aspx.cs" Inherits="Hidistro.UI.Web.Admin.setShoppingScore" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server"> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>购物积分设置</h1>
        <span>会员积分兑换比例设置</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
              <ul>
                  <li><span class="formitemtitle Pw_198"><em >*</em>几元一积分：</span>
                      <asp:TextBox Id="txtProductPointSet" runat="server" CssClass="forminput"></asp:TextBox>
                      <p id="txtProductPointSetTip" runat="server">几元一积分不能为空,必须在0.1-10000000之间</p>
                  </li>
              </ul>
              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
	          </ul>
          </div>
      </div>
</div>  
<script>
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtProductPointSet', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '几元一积分不能为空,必须在0.1-10000000之间'))
        appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtProductPointSet', 0.1, 10000000, '几元一积分必须在0.1-10000000之间'));
    }

    $(document).ready(function () { InitValidators(); });
</script>
</asp:Content>

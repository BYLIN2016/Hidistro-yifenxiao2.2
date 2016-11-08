<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetPurchaseOrderOption.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetPurchaseOrderOption" %>
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
        <h1>采购设置</h1>
        <span>对采购单管理配置</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
              <ul>
             <li><span class="formitemtitle Pw_198"><em >*</em>过期几天自动关闭采购单：</span>
              <asp:TextBox ID="txtClosePurchaseOrderDays" runat="server" CssClass="forminput" />
              <p id="txtClosePurchaseOrderDaysTip" runat="server">采购后过期几天系统自动关闭未付款的采购单</p>
            </li>
             
            <li><span class="formitemtitle Pw_198"><em >*</em>发货几天自动完成采购单：</span>
              <asp:TextBox ID="txtFinishPurchaseOrderDays" runat="server" CssClass="forminput" />
              <p id="txtFinishPurchaseOrderDaysTip" runat="server">发货几天后，系统自动把采购单改成已完成状态</p>
            </li>
              </ul>
              <div class="clear"></div>
              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();"  />
	          </ul>
          </div>
      </div>
</div>  
    <script>
        function InitValidators() {

            initValid(new InputValidator('ctl00_contentHolder_txtClosePurchaseOrderDays', 1, 10, false, '-?[0-9]\\d*', '采购后过期几天系统自动关闭未付款的采购单'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtClosePurchaseOrderDays', 0, 90, '采购后过期几天系统自动关闭未付款的采购单'));

            initValid(new InputValidator('ctl00_contentHolder_txtFinishPurchaseOrderDays', 1, 10, false, '-?[0-9]\\d*', '发货几天后，系统自动把采购单改成已完成状态'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFinishPurchaseOrderDays', 0, 90, '发货几天后，系统自动把采购单改成已完成状态'));
        }

        $(document).ready(function () { InitValidators(); });
</script>
</asp:Content>


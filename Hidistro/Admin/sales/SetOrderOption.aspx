<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetOrderOption.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetOrderOption" %>
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
        <h1>订单设置</h1>
        <span>对订单管理配置</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
              <ul>
                   <li><span class="formitemtitle Pw_198"><em >*</em>显示几天内订单数：</span>
              <asp:TextBox ID="txtShowDays" runat="server" CssClass="forminput" />
              <p id="txtShowDaysTip" runat="server">前台发货查询中显示最近几天内的订单项</p>
            </li>
            <li><span class="formitemtitle Pw_198"><em >*</em>过期几天自动关闭订单：</span>
              <asp:TextBox ID="txtCloseOrderDays" runat="server" CssClass="forminput" />
              <p id="txtCloseOrderDaysTip" runat="server">下单后过期几天系统自动关闭未付款订单</p>
            </li>
             <li><span class="formitemtitle Pw_198"><em >*</em>发货几天自动完成订单：</span>
              <asp:TextBox ID="txtFinishOrderDays" runat="server" CssClass="forminput" />
              <p id="txtFinishOrderDaysTip" runat="server">发货几天后，系统自动把订单改成已完成状态</p>
            </li>            
            <li><span class="formitemtitle Pw_198"><em >*</em>订单发票税率：</span>
              <asp:TextBox ID="txtTaxRate" runat="server" CssClass="forminput" />%
              <p id="txtTaxRateTip" runat="server">发票收税比率，0表示顾客将不承担订单发票税金</p>
            </li>
            <li> <span class="formitemtitle Pw_198"><em >*</em>快递100所需Key：</span>
		        <asp:TextBox ID="txtKey" runat="server" class="forminput"></asp:TextBox>
                &nbsp;<a rel="nofollow" href="http://www.kuaidi100.com/openapi/applyapi.shtml" 
                target="_blank">点击此处</a>申请快递100所需Key
		        <p id="txtKeyTip" runat="server">快递100所需Key在物流跟踪时会用到，长度限制在60字符以内</p>               
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
            initValid(new InputValidator('ctl00_contentHolder_txtShowDays', 1, 10, false, '-?[0-9]\\d*', '设置前台发货查询中显示最近几天内的已发货订单'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtShowDays', 0, 90, '设置前台发货查询中显示最近几天内的已发货订单'));

            initValid(new InputValidator('ctl00_contentHolder_txtCloseOrderDays', 1, 10, false, '-?[0-9]\\d*', '下单后过期几天系统自动关闭未付款订单'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCloseOrderDays', 0, 90, '下单后过期几天系统自动关闭未付款订单'));
                        
            initValid(new InputValidator('ctl00_contentHolder_txtFinishOrderDays', 1, 10, false, '-?[0-9]\\d*', '发货几天后，系统自动把订单改成已完成状态'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFinishOrderDays', 0, 90, '发货几天后，系统自动把订单改成已完成状态'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtTaxRate', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '税率不能为空,必须在0-100之间'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtTaxRate', 0, 100, '税率必须在0-100之间'));

            initValid(new InputValidator('ctl00_contentHolder_txtKey', 0, 60, true, null, '快递100所需Key在物流跟踪时会用到，长度限制在60字符以内'))
        }

        $(document).ready(function () { InitValidators(); });
</script>
</asp:Content>


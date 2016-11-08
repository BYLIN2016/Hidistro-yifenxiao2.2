<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditDistributorTradePassword.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditDistributorTradePassword" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑分销商</h1>
          </div>
          <div class="formtab Pg_45">
            <ul>
               <li><a href='<%="EditDistributorSettings.aspx?userId="+ Page.Request.QueryString["userId"] %>'>基本设置</a></li>                                      
                      <li><a href='<%="EditDistributorLoginPassword.aspx?userId="+ Page.Request.QueryString["userId"] %>'>登录密码</a></li>
                      <li class="visited">交易密码</li>
                  <li></li>
            </ul>
          </div>
      <div class="formitem validator4">
        <ul>
         <li> <span class="formitemtitle Pw_110">分销商名称：</span>
               <strong class="colorE"><asp:Literal ID="litUserName" runat="server" /> <Hi:WangWangConversations runat="server" ID="WangWangConversations" /></strong>
          </li>
          <li> <span class="formitemtitle Pw_110">新交易密码：<em >*</em></span>
           <asp:TextBox ID="txtNewTradePassword" runat="server" TextMode="Password" CssClass="forminput"></asp:TextBox>
	        <p id="txtNewTradePasswordTip" runat="server">密码不能为空，长度在6-20个字符之间</p>
          </li>
          <li> <span class="formitemtitle Pw_110">重复输入一次：<em >*</em></span>
            <asp:TextBox ID="txtTradePasswordCompare" runat="server" TextMode="Password" CssClass="forminput"></asp:TextBox>
	        <p id="txtTradePasswordCompareTip" runat="server">重复确认交易密码</p>
          </li>
      </ul>
      <ul class="btn Pa_110 clear">
        <asp:Button ID="btnEditDistributorTradePassword" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtNewTradePassword', 6, 20, false, null, '密码不能为空，长度在6-20个字符之间'));
            initValid(new CompareValidator('ctl00_contentHolder_txtTradePasswordCompare', 'ctl00_contentHolder_txtNewTradePassword', '两次输入的密码不一致请重新输入'));
        }
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>

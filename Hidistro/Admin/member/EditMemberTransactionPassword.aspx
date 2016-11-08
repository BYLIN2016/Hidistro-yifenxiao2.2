<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMemberTransactionPassword.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditMemberTransactionPassword" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑会员信息</h1>
          </div>
          <div class="formtab Pg_45">
                   <ul>
                      <li><a href='<%="EditMember.aspx?userId="+Page.Request.QueryString["userId"] %>'>基本信息</a></li>                                      
                      <li><a href='<%="EditMemberLoginPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'>登录密码</a></li>
                      <li class="visited">交易密码</li>
            </ul>
          </div>
      <div class="formitem validator4">
        <ul>
        <li> 
            <span class="formitemtitle Pw_110">用户名：</span><asp:Literal runat="server" Id="litlUserName" />
        </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>新交易密码：</span>
          <asp:TextBox ID="txtTransactionPassWord" runat="server" TextMode="Password" CssClass="forminput" />
          <p runat="server" id="txtTransactionPassWordTip">交易密码不能为空,长度限制在6-20个字符之间</p>
          </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>重复输入一次：</span>
          <asp:TextBox ID="txtTransactionPassWordCompare" runat="server" TextMode="Password" CssClass="forminput" />
           <p runat="server" id="txtTransactionPassWordCompareTip">重复确认交易密码</p>
          </li>
      </ul>
      <ul class="btn Pa_110">
        <asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTransactionPassWord', 6, 20, false, null, '交易密码不能为空,长度限制在6-20个字符之间'));
            initValid(new CompareValidator('ctl00_contentHolder_txtTransactionPassWordCompare', 'ctl00_contentHolder_txtTransactionPassWord', '两次输入的密码不一致请重新输入'));
        }
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>
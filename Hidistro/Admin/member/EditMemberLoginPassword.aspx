<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMemberLoginPassword.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditMemberLoginPassword" %>
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
                      <li class="visited">登录密码</li>
                      <li><a href='<%="EditMemberTransactionPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'>交易密码</a></li>
            </ul>
          </div>
      <div class="formitem clearfix validator4">
        <ul>
            <li> 
            <span class="formitemtitle Pw_110">用户名：</span>
            <asp:Literal runat="server" ID="litlUserName" />
        </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>新登录密码：</span>
            <asp:TextBox runat="server" ID="txtNewPassWord" TextMode="Password" CssClass="forminput" />
            <p runat="server" id="txtNewPassWordTip">密码不能为空，长度在6-20个字符之间</p>
          </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>重复输入一次：</span>
            <asp:TextBox runat="server" ID="txtPassWordCompare" TextMode="Password" CssClass="forminput" />
            <p runat="server" id="txtPassWordCompareTip">重复确认密码</p>
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
            initValid(new InputValidator('ctl00_contentHolder_txtNewPassWord', 6, 20, false, null, '密码不能为空，长度在6-20个字符之间'));
            initValid(new CompareValidator('ctl00_contentHolder_txtPassWordCompare', 'ctl00_contentHolder_txtNewPassWord', '两次输入的密码不一致请重新输入'));
        }
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>
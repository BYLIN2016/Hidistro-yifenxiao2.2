<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditManagerPassword.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditManagerPassword" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="Managers.aspx"><span>管理员管理</span></a></li>
                  </ul>
</div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑管理员信息</h1>
          </div>
          <div class="formtab Pg_45">
                   <ul>
                      <li><a href='<%="EditManager.aspx?userId="+Page.Request.QueryString["userId"] %>'>基本信息</a></li>                                      
                      <li class="visited">修改密码</li>
            </ul>
          </div>
      <div class="formitem validator2">
        <ul>
         <li>
			    <span class="formitemtitle Pw_100">用户名：</span>
			    <strong class="colorG"><asp:Literal ID="lblLoginNameValue" runat="server" /></strong>
		    </li>
		    <li id="panelOld" runat="server">
		        
			        <span class="formitemtitle Pw_100"><em >*</em>旧密码：</span>
			        <asp:TextBox ID="txtOldPassWord" runat="server" TextMode="Password" CssClass="forminput"></asp:TextBox>
			        
		    </li>
          <li> <span class="formitemtitle Pw_100"><em >*</em>新密码：</span>
           <asp:TextBox runat="server" ID="txtNewPassWord" TextMode="Password" CssClass="forminput" />
           <p id="ctl00_contentHolder_txtNewPassWordTip">密码不能为空，长度在6-20个字符之间</p> 
          </li>
          <li> <span class="formitemtitle Pw_100"><em >*</em>确认密码：</span>
           <asp:TextBox runat="server" ID="txtPassWordCompare" TextMode="Password" CssClass="forminput" />
            <p id="ctl00_contentHolder_txtPassWordCompareTip">请再次输入密码</p>
          </li>
      </ul>
      <ul class="btn Pa_100">
        <asp:Button ID="btnEditPassWordOK" runat="server" OnClientClick="return PageIsValid();" CssClass="submit_DAqueding" Text="保 存" /> 
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

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtNewPassWord', 6, 20, false, null, '密码不能为空，长度在6-20个字符之间'));
            initValid(new CompareValidator('ctl00_contentHolder_txtPassWordCompare', 'ctl00_contentHolder_txtNewPassWord', '两次输入的密码不一致请重新输入'));
        }

        function link()
        {
            window.location.href='Managers.aspx';
        }
			        
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>


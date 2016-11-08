<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="UnderlingDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.UnderlingDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="ManageUnderlings.aspx"><span>会员管理</span></a></li>
                  </ul>
</div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1 class="title_line">查看“<asp:Literal runat="server" ID="litUserName" />”会员信息</h1>
          </div>
        <div class="formitem clearfix">
          <ul>
          <li><span class="formitemtitle Pw_110">推广链接：</span><asp:Literal runat="server" ID="lblUserLink" /></li>
          <li><span class="formitemtitle Pw_110">账户状态：</span><asp:Literal runat="server" ID="litIsApproved" /></li>
          <li><span class="formitemtitle Pw_110">会员等级：</span> <asp:Literal runat="server" ID="litGrade" /></li>
          <li><span class="formitemtitle Pw_110">姓名：</span><asp:Literal runat="server" ID="litRealName" /></li>
          <li><span class="formitemtitle Pw_110">生日：</span><asp:Literal runat="server" ID="litBirthDate" /></li>
          <li><span class="formitemtitle Pw_110">性别：</span><asp:Literal runat="server" ID="litGender" /></li>
          <li><span class="formitemtitle Pw_110">电子邮件地址：</span><asp:Literal runat="server" ID="litEmail" /></li>
          <li><span class="formitemtitle Pw_110">详细地址：</span> <asp:Literal runat="server" ID="litAddress" /></li>
          <li><span class="formitemtitle Pw_110">旺旺：</span><asp:Literal runat="server" ID="litWangwang" /></li>
          <li><span class="formitemtitle Pw_110">QQ：</span><asp:Literal runat="server" ID="litQQ" /></li>
          <li><span class="formitemtitle Pw_110">MSN：</span><asp:Literal runat="server" ID="litMSN" /></li>
          <li><span class="formitemtitle Pw_110">电话号码：</span><asp:Literal runat="server" ID="litTelPhone" /></li>
          <li><span class="formitemtitle Pw_110">手机号码：</span><asp:Literal runat="server" ID="litCellPhone" /></li>
          <li><span class="formitemtitle Pw_110">注册日期：</span><asp:Literal runat="server" ID="litCreateDate" /> </li>
          <li><span class="formitemtitle Pw_110">最后登录日期：</span><asp:Literal runat="server" ID="litLastLoginDate" /> </li>
      </ul>
      <ul class="btn Pa_110">
        <asp:Button runat="server" ID="btnEdit" CssClass="submit_DAqueding" Text="编辑此会员" />
        </ul>
      </div>

      </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddArticleCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddArticleCategory" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>添加文章分类</h1>
        </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100"><em >*</em>分类名称：</span>
           <asp:TextBox ID="txtArticleCategoryiesName" CssClass="forminput" runat="server"></asp:TextBox>
           <p id="ctl00_contentHolder_txtArticleCategoryiesNameTip">分类名称不能为空，长度限制在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">分类图标：</span>
            <asp:FileUpload ID="fileUpload" CssClass="forminput" runat="server" />
          </li>
          <li> <span class="formitemtitle Pw_100">分类介绍：</span>
           <asp:TextBox ID="txtArticleCategoryiesDesc" TextMode="MultiLine" CssClass="forminput" Width="300px" Height="70px" runat="server"></asp:TextBox>
           <p id="ctl00_contentHolder_txtArticleCategoryiesDescTip">分类介绍最多只能输入300个字符</p>
          </li>
      </ul>
      <ul class="btn Pa_100">
         <asp:Button ID="btnSubmitArticleCategory" runat="server" OnClientClick="return PageIsValid();" Text="添　加"  CssClass="submit_DAqueding"/> 
        </ul>
      </div>

      </div>
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
</asp:Content>

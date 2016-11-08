<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditArticleCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditArticleCategory" %>
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
            <h1>编辑文章分类</h1>
        </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100"><em >*</em>分类名称：</span>
           <asp:TextBox ID="txtArticleCategoryiesName" CssClass="forminput" runat="server"></asp:TextBox>
           <p id="ctl00_contentHolder_txtArticleCategoryiesNameTip">分类名称不能为空，长度限制在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">分类图标：</span>
            <asp:FileUpload ID="fileUpload" CssClass="forminput" runat="server" />
            <div class="Pa_100 Pg_8 clear">
              <table width="150" border="0" cellspacing="0">
                <tr>
                  <td width="100"><Hi:HiImage runat="server" ID="imgPic"  CssClass="Img100_60" /></td>
                  <td width="55" align="center"><Hi:ImageLinkButton Id="btnPicDelete" runat="server" IsShow="true"  Text="删除" /></td>                  
                </tr>
                 <tr><td width="160" colspan="2"></td>
                </tr>
              </table>
            </div>
          </li>
          <li> <span class="formitemtitle Pw_100">分类介绍：</span>
           <asp:TextBox ID="txtArticleCategoryiesDesc" TextMode="MultiLine" CssClass="forminput" Width="300px" Height="70px" runat="server"></asp:TextBox>
           <p id="ctl00_contentHolder_txtArticleCategoryiesDescTip">分类介绍最多只能输入300个字符</p>
          </li>
      </ul>
      <ul class="btn Pa_100">
         <asp:Button ID="btnSubmitArticleCategory" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"/> 
        </ul>
      </div>

      </div>
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function InitValidators()
{
    initValid(new InputValidator('ctl00_contentHolder_txtArticleCategoryiesName', 1, 60, false, null, '分类名称不能为空，长度限制在60个字符以内'))
    initValid(new InputValidator('ctl00_contentHolder_txtArticleCategoryiesDesc', 0, 300, true, null, '分类介绍最多只能输入300个字符'))
}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditMyArticle.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyArticle" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="MyArticleList.aspx"><span>文章管理</span></a></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑文章内容</h1>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100">所属分类：<em >*</em></span><abbr class="formselect">
            <Hi:DistributorArticleCategoryDropDownList ID="dropArticleCategory" AllowNull="true" runat="server"></Hi:DistributorArticleCategoryDropDownList>
          </abbr>
          <a href="AddMyArticleCategory.aspx">添加文章分类 </a>
          </li>
          <li> <span class="formitemtitle Pw_100">主题：<em >*</em></span>
            <asp:TextBox ID="txtArticleTitle" runat="server" CssClass="forminput"></asp:TextBox>　　<input type="checkbox" id="ckrrelease" checked="checked" runat="server"/>是否立即发布
            <p id="ctl00_contentHolder_txtArticleTitleTip">限制在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">搜索描述：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaDescription" />
              <p id="ctl00_contentHolder_txtMetaDescriptionTip">搜索描述的长度限制在260个字符以内</p>
            </li>
	        <li> <span class="formitemtitle Pw_100">搜索关键字：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaKeywords" />
              <p id="ctl00_contentHolder_txtMetaKeywordsTip">搜索关键字的长度限制在160个字符以内</p>
	        </li>
          <li> <span class="formitemtitle Pw_100">文章图标：</span>
            <asp:FileUpload ID="fileUpload" CssClass="forminput" runat="server" />
            <div>
              <table width="300" border="0" cellspacing="0">
                <tr>
                <td width="80"> <Hi:HiImage runat="server" ID="imgPic" CssClass="Img100_60" /></td><td width="80" align="left"> <Hi:ImageLinkButton Id="btnPicDelete" runat="server" IsShow="true"  Text="删除" /></td></tr>
                  <tr><td width="160" colspan="2"></td>
                </tr>
              </table>
            </div>
          </li>
          <li> <span class="formitemtitle Pw_100">摘要：</span>
            <asp:TextBox ID="txtShortDesc" TextMode="MultiLine" CssClass="forminput" Height="70" runat="server"></asp:TextBox>
          </li>
          <li> <span class="formitemtitle Pw_100">内容：<em >*</em></span>
            <span><Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fcContent" runat="server" Width="550px"  height="200px" /></span>
          </li>
      </ul>
      <ul class="btn Pa_100 clearfix">
        <asp:Button ID="btnAddArticle" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"/>
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
function InitValidators()
{
   
    initValid(new InputValidator('ctl00_contentHolder_txtArticleTitle', 1, 60, false, null, '文章标题不能为空，长度限制在60个字符以内'))
    initValid(new InputValidator('ctl00_contentHolder_txtShortDesc', 0, 300, true, null, '文章摘要的长度限制在300个字符以内'))
    initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1, 260, true, null, '搜索描述不能为空，长度限制在260个字符以内'))
    initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 0, 160, true, null, '搜索关键字的长度限制在160个字符以内'))
}
$(document).ready(function(){ InitValidators(); });
</script>

</asp:Content>

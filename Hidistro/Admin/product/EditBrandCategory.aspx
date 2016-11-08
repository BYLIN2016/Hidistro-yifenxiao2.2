<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditBrandCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditBrandCategory" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>修改品牌分类</h1>
            <span>管理商品所属的各个品牌，如果在上架商品时给商品指定了品牌分类，则商品可以按品牌分类浏览 </span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100"><em >*</em>品牌名称：</span>
            <asp:TextBox ID="txtBrandName" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtBrandNameTip">品牌名称不能为空，长度限制在30个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100"><em>*</em>品牌Logo：</span>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />           
               <asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" />     
          </li>
          <li> <span class="formitemtitle Pw_100">&nbsp;</span>
           <table width="300" border="0" cellspacing="0">
                  <tr>
                    <td valign="middle" style="padding-top:10px;"><Hi:HiImage ID="imgLogo" runat="server" Width="180" Height="40" />&nbsp;<Hi:ImageLinkButton ID="btnDeleteLogo"  runat="server" Text="删除" IsShow="true" /></td>
                  </tr>
                  <tr><td align="left"></td></tr>
                </table>
           </li>
          <li> <span class="formitemtitle Pw_100">品牌官方地址：</span>
            <asp:TextBox ID="txtCompanyUrl" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtCompanyUrlTip">品牌官方网站的网址必须以http://开头，长度限制在100个字符以内</p>
          </li>
           <li style="margin-bottom:0px;"> <span class="formitemtitle Pw_100">URL重写名称：</span>
            <asp:TextBox ID="txtReUrl" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtReUrlTip"></p>
          </li>
           <li style="margin-bottom:0px;"> <span class="formitemtitle Pw_100">搜索关键字：</span>
            <asp:TextBox ID="txtkeyword" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtkeywordTip">长度限制在160个字符以内</p>
          </li>
            <li style="margin-bottom:0px;"> <span class="formitemtitle Pw_100">关键字描述：</span>
            <asp:TextBox ID="txtMetaDescription" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtMetaDescriptionTip">长度限制在260个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">品牌介绍：</span>
          <Kindeditor:KindeditorControl ID="fckDescription" runat="server"   Height="300"/>
          </li>
           <li> <span class="formitemtitle Pw_100">关联商品类型：</span>
                  <span style="float:left;"><Hi:ProductTypesCheckBoxList runat="server" ID="chlistProductTypes" Width="100%"  /></span>
                </li>
      </ul>
      <ul class="btntf Pa_100 clear">
        <asp:Button ID="btnUpdateBrandCategory" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtBrandName', 1,30, false, null, '品牌名称不能为空，长度限制在60个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtCompanyUrl', 0, 100, true, '^(http)://.*', '品牌官方网站的网址必须以http://开头，长度限制在100个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtReUrl', 0, 50, true, '([a-zA-Z])+(([a-zA-Z_-])?)+', 'URL重写长度限制在60个字符以内，必须为字母开头且只包含字母-和_'))
                initValid(new InputValidator('ctl00_contentHolder_txtkeyword', 0, 160, true, null, '让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在160个字符以内'))
                 initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1,260, true, null, '长度限制在260个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
       </script>
</asp:Content>

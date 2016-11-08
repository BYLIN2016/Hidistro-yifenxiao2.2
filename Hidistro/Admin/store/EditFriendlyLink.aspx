<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditFriendlyLink.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditFriendlyLink" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑友情链接</h1>
            <span>管理店铺的所有友情链接，您可以添加、修改或删除友情链接 </span>
        </div>
      <div class="formitem validator4">
        <ul>
          <li>
          <span class="formitemtitle Pw_110"><em >*</em>网站名称：</span>
           <asp:TextBox ID="txtaddTitle" runat="server" CssClass="forminput" style="width:281px;"></asp:TextBox>
           <p id="txtaddTitleTip" runat="server">长度限制在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">友情链接Logo：</span>
          <asp:FileUpload ID="uploadImageUrl" Width="281" runat="server"  CssClass="forminput" style=" cursor:pointer"/><br />
                <div>
              <table width="300" border="0" cellspacing="0">
                <tr>
                <td width="80"> <Hi:HiImage runat="server" ID="imgPic" CssClass="Img100_60"/></td><td width="80" align="left"> <Hi:ImageLinkButton Id="btnPicDelete" runat="server" IsShow="true"  Text="删除" /></td></tr>
                  <tr><td width="160" colspan="2"></td>
                </tr>
              </table>
            </div>
          </li>
          <li> <span class="formitemtitle Pw_110">网站地址：</span>
            <asp:TextBox ID="txtaddLinkUrl" runat="server" CssClass="forminput" style="width:281px;"></asp:TextBox>
            <p id="ctl00_contentHolder_txtaddLinkUrlTip">&nbsp;</p>
          </li>
          <li> <span class="formitemtitle Pw_110">是否显示：</span>
            <Hi:YesNoRadioButtonList id="radioShowLinks" RepeatLayout="Flow" runat="server" />
          </li>
      </ul>
      <ul class="btn Pa_110 clear">
       <asp:Button ID="btnSubmitLinks" runat="server" Text="确 定"  OnClientClick="return PageIsValid()"  CssClass="submit_DAqueding"/> 
      
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
        initValid(new InputValidator('ctl00_contentHolder_txtaddTitle', 0, 60, false, null, '友情链接网站的名称，长度限制在60个字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtaddLinkUrl', 0, 255, true, '^(http://).*(\.).*', '请输入带http的完整格式的URL地址'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>


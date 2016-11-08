<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="AddMyFriendlyLink.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.AddMyFriendlyLink" %>
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
                        <li><a href="MyFriendlyLinks.aspx"><span>友情链接管理</span></a></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>添加友情链接</h1>
        </div>
      <div class="formitem validator4">
        <ul>
          <li> <span class="formitemtitle Pw_110">网站名称：<em >*</em></span>
           <asp:TextBox ID="txtaddTitle" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="txtaddTitleTip" runat="server">长度限制在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">友情链接Logo：</span>
          <asp:FileUpload ID="uploadImageUrl" Width="281" runat="server" />
          <li> <span class="formitemtitle Pw_110">网站地址：</span>
            <asp:TextBox ID="txtaddLinkUrl" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="txtaddLinkUrlTip" runat="server">请输入带http的完整格式的URL地址</p>
          </li>
          <li> <span class="formitemtitle Pw_110">是否显示：</span>
            <Hi:YesNoRadioButtonList id="radioShowLinks" RepeatLayout="Flow" runat="server" />
          </li>
      </ul>
      <ul class="btn Pa_110">
        <li> <asp:Button ID="btnSubmitLinks" runat="server" Text="添 加"  OnClientClick="return PageIsValid()"  CssClass="submit_DAqueding"/>         
       
       </li>
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

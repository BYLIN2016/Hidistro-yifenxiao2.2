<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetSEO.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetSEO" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server"> 
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtSiteDescription', 1, 100, false, null, '简单介绍，长度限制在100字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaDescription', 1, 260, false, '', '店铺描述,长度必须控制在260个字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaKeywords', 1, 160, true, null, '搜索关键字，长度必须控制在160个字符以内'))

    }
    $(document).ready(function () { InitValidators(); });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>SEO页面设置</h1>
        <span>对网站优化设置</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
              <ul>
               <li> <span class="formitemtitle Pw_198">简单介绍：</span>
              <asp:TextBox ID="txtSiteDescription" CssClass="forminput formwidth" runat="server"></asp:TextBox>
              <span id="txtSiteDescriptionTip" runat="server"></span>
            </li>
            <li> <span class="formitemtitle Pw_198">店铺描述：</span>
              <asp:TextBox ID="txtSearchMetaDescription" runat="server" CssClass="forminput formwidth"></asp:TextBox>
              <span id="txtSearchMetaDescriptionTip" runat="server"></span>
            </li>
            <li><span class="formitemtitle Pw_198">搜索关键字：</span>
              <asp:TextBox ID="txtSearchMetaKeywords" CssClass="forminput formwidth" runat="server" />
              <span id="txtSearchMetaKeywordsTip" runat="server"></span>
            </li>
              </ul>
              <div class="clear"></div>
              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
	          </ul>
          </div>
      </div>
</div>  

</asp:Content>

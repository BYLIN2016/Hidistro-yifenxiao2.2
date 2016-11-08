<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditAffiche" MasterPageFile="~/Admin/Admin.Master" %>
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
          <div class="title">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑公告</h1>
            <span>修改公告内容</span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li><span class="formitemtitle Pw_100"><em >*</em>标题：</span>
            <asp:TextBox ID="txtAfficheTitle" runat="server" CssClass="forminput" />
            <p id="ctl00_contentHolder_txtAfficheTitleTip">长度限制在60个字符以内</p>
          </li>
          <li style="color:Red; margin-left:100px; height:25px; line-height:25px; margin-bottom:0px;">如您要为某些关键词添加当前公告内链地址，只需在插入超链接中的URL地址框填写 # 即可。注意：#前无需添加http://</li>
          <li><span class="formitemtitle Pw_100"><em >*</em>公告内容：</span>
            <span><Kindeditor:KindeditorControl ID="fcContent" runat="server"    Height="250px" /></span>
          </li>
      </ul>
      <ul class="btn Pa_100 clearfix">
        <asp:Button ID="btnEditAffiche" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding" />
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
        initValid(new InputValidator('ctl00_contentHolder_txtAfficheTitle', 1, 60, false, null, '公告标题不能为空，长度限制在60个字符以内'))
    }
    $(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>


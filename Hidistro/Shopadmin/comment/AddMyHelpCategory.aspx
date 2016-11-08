<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="AddMyHelpCategory.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.AddMyHelpCategory" %>
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
                        <li><a href="MyHelpCategories.aspx"><span>帮助分类管理</span></a></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>添加帮助分类</h1>
        </div>
      <div class="formitem  validator3">
        <ul>
          <li> <span class="formitemtitle Pw_128">分类名称：<em >*</em></span>
            <asp:TextBox ID="txtHelpCategoryName" CssClass="forminput" runat="server"></asp:TextBox>
            <p id="ctl00_contentHolder_txtHelpCategoryNameTip">分类名称不能为空，长度限制在60个字符以内</p>
          </li>
          <li><span class="formitemtitle Pw_128">分类图标：</span>
            <asp:FileUpload ID="fileUpload" CssClass="forminput" runat="server" />  
		 </li>          <li> <span class="formitemtitle Pw_128">显示在底部帮助：</span>
            <Hi:YesNoRadioButtonList id="radioShowFooter" runat="server" RepeatLayout="Flow" />
          </li>
          <li><span class="formitemtitle Pw_128">分类介绍：</span>
           <asp:TextBox ID="txtHelpCategoryDesc" TextMode="MultiLine" Width="250px" Height="70px" runat="server"></asp:TextBox>
           <p id="ctl00_contentHolder_txtHelpCategoryDescTip">分类介绍最多只能输入300个字符</p>
          </li>
      </ul>
      <ul class="btn Pa_128">
        <asp:Button ID="btnSubmitHelpCategory" runat="server" OnClientClick="return PageIsValid();" Text="添 加"  CssClass="submit_DAqueding"  />
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
    initValid(new InputValidator('ctl00_contentHolder_txtHelpCategoryName', 1, 60, false, null, '分类名称不能为空，长度限制在60个字符以内'))
    initValid(new InputValidator('ctl00_contentHolder_txtHelpCategoryDesc',0, 300, true, null, '分类介绍最多只能输入300个字符'))
}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>

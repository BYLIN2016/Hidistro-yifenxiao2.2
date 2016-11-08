<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyEmailSettings.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyEmailSettings"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>邮件设置管理 </h1>
     <span>邮件设置提供修改邮件连接信息</span></div>
    <div class="datafrom">
<div class="formitem">
<div class="emailhead">
<p>选择ASP.NET邮件组件方式进行正确设置，用以开通自动向用户发送如注册、订单付款等邮件</p>
<p>如果您需要经常面向大量用户邮箱进行邮件群发，建议你开通更高品质的<font style="color:Red">EDM邮件营销服务。</font><a href="http://zzfw.hishop.com.cn/edm/" target="_blank" >点此开通</a></p>
</div>
          <ul id="pluginContainer">
          <li><span class="formitemtitle Pw_140">发送方式：</span>
               <select id="ddlEmails" name="ddlEmails"></select>
            </li>
            <li rowtype="attributeTemplate" style="display: none"><span class="formitemtitle Pw_140">$Name$：</span>
                $Input$
            </li>
          </ul>
          <ul class="btntf Pa_140">
		     <asp:Button ID="btnChangeEmailSettings" runat="server" Text="保 存"  CssClass="submit_DAqueding float"></asp:Button >
      </ul>
</div>
<div class="formitem">
<ul  style="padding-top:10px">
            <li><span class="formitemtitle Pw_140">测试邮箱：</span>
              <asp:TextBox runat="server" ID="txtTestEmail" CssClass="forminput" />
            </li>
            <li class="clear"></li>
          </ul>
          <ul class="btntf Pa_140">
             <asp:Button ID="btnTestEmailSettings" OnClientClick="return TestCheck();" runat="server" Text="发送测试邮件"  CssClass="submit_DAqueding inbnt"> </asp:Button >	
      </ul>
</div>
      </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div> 
  <asp:HiddenField runat="server" ID="txtSelectedName" />
  <asp:HiddenField runat="server" ID="txtConfigData" />
  <Hi:Script ID="Script1" runat="server" Src="/utility/plugin.js" />   
  <script type="text/javascript">
      $(document).ready(function() {
          pluginContainer = $("#pluginContainer");
          templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
          dropPlugins = $("#ddlEmails");
          selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
          configDataCtl = $("#<%=txtConfigData.ClientID %>");

          // 绑定邮件类型列表
          $(dropPlugins).append($("<option value=\"\">-请选择发送方式-</option>"));
          $.ajax({
              url: "PluginHandler.aspx?type=EmailSender&action=getlist",
              type: 'GET',
              async: false,
              dataType: 'json',
              timeout: 10000,
              success: function(resultData) {
                  if (resultData.qty == 0)
                      return;

                  $.each(resultData.items, function(i, item) {
                      if (item.FullName == $(selectedNameCtl).val())
                          $(dropPlugins).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.FullName, item.DisplayName)));
                      else
                          $(dropPlugins).append($(String.format("<option value=\"{0}\">{1}</option>", item.FullName, item.DisplayName)));
                  });
              }
          });

          $(dropPlugins).bind("change", function() { SelectPlugin("EmailSender"); });

          if ($(selectedNameCtl).val().length > 0) {
              SelectPlugin("EmailSender");
          }
      });

      function TestCheck() {
          if ($(dropPlugins).val() == "") {
              alert("请先选择发送方式并填写配置信息");
              return false;
          }
          return true;
      }
</script>
  </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SMSSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SMSSettings" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>
                手机短信设置
            </h1>
            <span>配置您所获取手机短信平台的信息，系统将使用这些信息自动为客户发送手机短信。
            </span>
            <asp:Label ID="lbNum" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>  &nbsp;<a href="http://sms.kuaidiantong.cn/SMSPackList.aspx" target="frammain" >购买</a></div>
        <div class="datafrom">
            <div class="formitem">
          <ul id="pluginContainer" class="attributeContent2">
          <li style="margin-bottom:10px;"><span class="formitemtitle Pw_140">发送方式：</span>
               <select id="ddlSms" name="ddlSms"></select>
                <a href="http://sms.kuaidiantong.cn/" target="frammain" >点击此处</a>登录后（没有账号先注册）获取短信接口的Appkey和Appsecret
            </li>
            <li rowtype="attributeTemplate" style="display: none;margin-bottom:10px;"><span class="formitemtitle Pw_140">$Name$：</span>
                $Input$
            </li>
          </ul>
          <ul class="btntf Pa_140">
		     <asp:Button ID="btnSaveSMSSettings" runat="server" OnClientClick="return Save();" Text="保 存" CssClass="submit_DAqueding float" />
      </ul>
</div>
<div class="formitem">
<ul>
            <li style="margin-bottom:10px;"><span class="formitemtitle Pw_140">接收手机号：</span>
              <asp:TextBox ID="txtTestCellPhone" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_140">发送内容：</span>
              <asp:TextBox ID="txtTestSubject" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li class="clear"></li>
          </ul>
          <ul class="btntf Pa_140">
             <asp:Button ID="btnTestSend" runat="server" OnClientClick="return TestCheck();" Text="测试发送" CssClass="submit_DAqueding inbnt">
          </asp:Button>
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
          dropPlugins = $("#ddlSms");
          selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
          configDataCtl = $("#<%=txtConfigData.ClientID %>");

          // 绑定短信类型列表
          $(dropPlugins).append($("<option value=\"\">-请选择发送方式-</option>"));
          $.ajax({
              url: "PluginHandler.aspx?type=SMSSender&action=getlist",
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

          //$(dropPlugins).bind("change", function() { SelectPlugin("SMSSender"); });
          $(dropPlugins).attr("disabled", "disabled");

          if ($(selectedNameCtl).val().length > 0) {
              SelectPlugin("SMSSender");
          }
      });

      function TestCheck() {
          if ($(dropPlugins).val() == "") {
              alert("请先选择发送方式并填写配置信息");
              return false;
          }
          if ($("#Appkey").val().length == 0) {
            alert("Appkey必须填");
            return false;
          }
          if($("#Appsecret").val().length == 0){
            alert("Appsecret必须填");
            return false;
          }
          if ($("#ctl00_contentHolder_txtTestCellPhone").val().length == 0) {
            alert("请输入接收手机号码");
            return false;
          }
          if($("#ctl00_contentHolder_txtTestSubject").val().length == 0){
            alert("请输入发送内容");
            return false;
          }
          $(dropPlugins).removeAttr("disabled");
          return true;
      }
      
      function Save() {
          if ($("#Appkey").val().length == 0) {
            alert("Appkey必须填");
            return false;
          }
          if($("#Appsecret").val().length == 0){
            alert("Appsecret必须填");
            return false;
          }
          $(dropPlugins).removeAttr("disabled");
          return true;
      }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditMyPaymentType.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyPaymentType" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix validator4">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="MyPaymentTypes.aspx"><span>支付方式管理</span></a></li>
                  </ul>
  </div>
		<div class="columnright">
		  <div class="title title_height"> 
            <em><img src="../images/05.gif" width="32" height="32" /></em>
		    <h1>编辑支付方式</h1>	
		  </div>
		  <div class="formitem">
		    <ul>
		      <li>
		      <span class="formitemtitle Pw_110">支付接口类型：<em >*</em></span>
		        <abbr class="formselect">
		            <select id="ddlPayments" name="ddlPayments"></select>
                </abbr>
	          </li>
		      <li> <span class="formitemtitle Pw_110">支付方式名称：<em >*</em></span>
		        <asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox>
		         <p id="ctl00_contentHolder_txtNameTip">名称不能为空，在1至60个字符之间</p>
	          </li>
	          </ul>
	          </div>

	          <div class="formitem">
	          <ul id="pluginContainer">
	          <li rowtype="attributeTemplate" style="display:none;">
	           <span class="formitemtitle Pw_128">$Name$：</span>
		        $Input$
	          </li>
	          </ul>
	          </div>

	          <div class="formitem">
	          <ul>
		      <li> 
              <span class="formitemtitle Pw_110 ">手续费：<em>*</em></span>
              <asp:TextBox ID="txtCharge" runat="server"   CssClass="forminput"></asp:TextBox> <asp:CheckBox ID="chkIsPercent" runat="server" Text="百分比" />
              <p id="ctl00_contentHolder_txtChargeTip">支付手续费大小0-10000000之间</p>
	          </li>
	          <li>
              <span class="formitemtitle Pw_110 " style="width:130px;">用于预付款充值：<em >*</em></span>
              <Hi:YesNoRadioButtonList runat="server" ID="radiIsUseInpour" RepeatLayout="Flow" />
               <p id="P1">用于预付款充值的支付方式最好是即时到帐的支付方式，方便立即查看预付款</p>
	          </li>
           <li class="clearfix"> <span class="formitemtitle Pw_110">备注：</span>
         <span><Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fcContent" runat="server" Width="550px"  height="200px" /></span>
          </li>
          <li>&nbsp;</li>
	        </ul>
            <div class="clear"></div>
    <ul class="btn Pa_110">
     <asp:Button ID="btnUpdate" runat="server" OnClientClick="return Save();" Text="保 存"  CssClass="submit_DAqueding"/>
            </ul>
	      </div>
  </div>
</div>
<asp:HiddenField runat="server" ID="txtSelectedName" />
  <asp:HiddenField runat="server" ID="txtConfigData" />
  <Hi:Script ID="Script1" runat="server" Src="/utility/plugin.js" />   
  <script type="text/javascript">
      $(document).ready(function() {
          pluginContainer = $("#pluginContainer");
          templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
          dropPlugins = $("#ddlPayments");
          selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
          configDataCtl = $("#<%=txtConfigData.ClientID %>");

          // 绑定支付接口类型列表
          $(dropPlugins).append($("<option value=\"\">-请选择接口类型-</option>"));
          $.ajax({
              url: "PluginHandler.aspx?type=PaymentRequest&action=getlist",
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
          $(dropPlugins).attr("disabled", "disabled");

          if ($(selectedNameCtl).val().length > 0) {
              SelectPlugin("PaymentRequest");
          }
      });

      function Save() {
          if (PageIsValid()) {
              $(dropPlugins).removeAttr("disabled");
              return true;
          }
          return false;
      }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
            function InitValidators()
            {
                // 支付方式名称
                initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 60, false, null, '支付方式名称不能为空，长度限制在1-60个字符之间'));
                // 支付手续费
                initValid(new InputValidator('ctl00_contentHolder_txtCharge', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '支付手续费必须大于0'));
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCharge', 0,'10000000', '支付手续费大小0-10000000之间'));
            }
            $(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>
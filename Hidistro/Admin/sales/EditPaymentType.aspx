<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditPaymentType" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">

<div class="areacolumn clearfix validator4">
		<div class="columnright">
		  <div class="title"> 
            <em><img src="../images/05.gif" width="32" height="32" /></em>
		    <h1>编辑支付方式</h1>	
		    <span>修改支付方式的各项参数</span>
		  </div>
          <div class="datafrom">
		        <div class="formitem validator1">
		            <ul>
		            <li>
		          <span class="formitemtitle Pw_110"><em >*</em>支付接口类型：</span>
		            <abbr class="formselect">
		                <select id="ddlPayments" name="ddlPayments"></select>
                    </abbr>
	              </li>
		          <li style=" margin-bottom:0px;"> <span class="formitemtitle Pw_110"><em >*</em>支付方式名称：</span>
		            <asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox>
		            <p id="ctl00_contentHolder_txtNameTip" style="margin-left:118px;">名称不能为空，在1至60个字符之间</p>
	              </li>
	              </ul>
	              </div>
              </div>
	          <div class="formitem2">
                <div class="formitem2" style="padding:0px 0 0 45px;">
	                  <ul id="pluginContainer" class="attributeContent2">
	                  <li rowtype="attributeTemplate" style="display:none; clear: both; margin-top:18px; overflow:hidden;" >
	                   <span class="formitemtitle Pw_110" style=" display:block;float:left; font-size:14px; text-align:right">$Name$：</span>
		                $Input$
	                  </li>
	                  </ul>
                    </div>
	          </div>
	          <div class="formitem">
	          <ul>
		      <li> 
              <span class="formitemtitle Pw_110 "><em >*</em>手续费：</span>
              <asp:TextBox ID="txtCharge" runat="server"   CssClass="forminput"></asp:TextBox> <asp:CheckBox ID="chkIsPercent" runat="server" Text="百分比" />
              <p id="ctl00_contentHolder_txtChargeTip" style="margin-left:118px;">支付手续费大小0-10000000之间</p>
	          </li>
	          <li>
              <span class="formitemtitle Pw_110 " style="width:130px;"><em >*</em>用于预付款充值：</span>
              <Hi:YesNoRadioButtonList runat="server" ID="radiIsUseInpour" RepeatLayout="Flow" />
               <p id="P1" style="margin-left:118px;">用于预付款充值的支付方式最好是即时到帐的支付方式，方便立即查看预付款</p>
	          </li>
              <li  style=" margin-bottom:0px;">
              <span class="formitemtitle Pw_110 " style="width:130px;"><em >*</em>用于采购单付款：</span>
              <Hi:YesNoRadioButtonList runat="server" ID="radiIsUseInDistributor" RepeatLayout="Flow" />
               <p id="P2"></p>
	          </li>
           <li class="clearfix"> <span class="formitemtitle Pw_110">备注：</span>
         <span><Kindeditor:KindeditorControl ID="fcContent" runat="server"    height="200px" /></span>
          </li>
          <li class="clear"></li>
	        </ul>
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

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

        <script type="text/javascript" language="javascript">
            function InitValidators()
            {
                // 支付方式名称
                initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 60, false, null, '支付方式名称不能为空，长度限制在1-60个字符之间'));
                // 支付手续费
                initValid(new InputValidator('ctl00_contentHolder_txtCharge', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            }
            $(document).ready(function(){ InitValidators(); });
</script>

</asp:Content>
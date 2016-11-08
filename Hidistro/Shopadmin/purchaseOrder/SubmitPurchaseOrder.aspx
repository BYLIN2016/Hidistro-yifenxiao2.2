<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SubmitPurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SubmitPurchaseOrder" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register src="../../Templates/master/default/Ascx/tags/Common_SubmmintOrder/Skin-Common_CopyShippingAddress.ascx" tagname="Skin" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
		<li class="optionstar"><a href="PurchaseProduct.aspx"><span>1.选择商品</span></a></li>
		<li><a href="SelectedPurchaseProducts.aspx"  class="optionnext"><span>2.确认已选商品</span></a></li>
			<li  class="menucurrent"><a href="#"><span class="optioncenter">3.填写收货信息</span></a></li>	
		</ul>
	</div>
	
<div class="dataarea mainwidth databody">
    <div class="title  m_none td_bottom">
	    <h1>提交采购单 - 填写收货人信息 </h1>    
      </div>
    
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>
	       <h2 class="colorE">注意：正确填写收货人地址，以便尽快收到商品！</h2>
	        <li> 
	         <span class="formitemtitle Pw_198">收货人姓名：<em>*</em></span>
	          <asp:TextBox ID="txtShipTo" runat ="server" CssClass="forminput"></asp:TextBox>
	           <p runat="server" id="txtShipToTip">姓名长度在2-20个字符以内,只能以字母或汉字开头</p>
	        </li>
	        <li><span class="formitemtitle Pw_198">采购模式：</span><input type="checkbox" id="radSell" name="radmode" onclick="ChooseMode();" />代销模式</li>
	        <li id="liaddress" style="padding-left:90px;font-size:14px;display:none;">
	        			    <input type="hidden" id="hdcopyshipping" value="ctl00_contentHolder_txtShipTo,ctl00_contentHolder_txtMobile,ctl00_contentHolder_txtTel,ctl00_contentHolder_txtZipcode,ctl00_contentHolder_txtAddress" />
	            <uc1:Skin ID="Skin1" runat="server" />
	        </li>
	        <li> <span class="formitemtitle Pw_198">收货人地址：<em>*</em></span>
	        <abbr class="formselect">
	          <Hi:RegionSelector runat="server" ID="rsddlRegion" />
	          </abbr>
	          </li>
	        <li> <span class="formitemtitle Pw_198">详细地址：<em>*</em></span>
              <asp:TextBox ID="txtAddress" CssClass="textform" Width="350px"  runat="server" />
	          	           <p runat="server" id="txtAddressTip">详细地址不能为空，限制在100个字符以内</p>
	        </li>
	        <li> <span class="formitemtitle Pw_198">邮政编码：</span>
	         <asp:TextBox ID="txtZipcode" runat ="server" CssClass="forminput"></asp:TextBox>
            </li>
	        <li> <span class="formitemtitle Pw_198">电话号码：</span>
	         <asp:TextBox ID="txtTel" runat ="server" CssClass="forminput"></asp:TextBox>
	          <p runat="server" id="txtTelTip">电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”</p>
	         
            </li>
	        <li> <span class="formitemtitle Pw_198">手机号码：</span>
	          <asp:TextBox ID="txtMobile" runat ="server" CssClass="forminput"></asp:TextBox>
	           <p runat="server" id="txtMobileTip">手机号码长度限制在3-20个字符之间,只能输入数字</p>
	        </li>
	        <li><span class="formitemtitle Pw_198">配送方式：<em>*</em></span>
	           <span style="float:left;">
	   <%--        <Hi:Common_ShippingModeList runat="server" />--%>
	          <Hi:ShippingModeRadioButtonList ID="radioShippingMode" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" AutoPostBack="false" /></span>           
	        </li>
            <li><span class="formitemtitle Pw_198">支付方式：<em>*</em></span>
	           <span style="float:left;">
	   <%--        <Hi:Common_ShippingModeList runat="server" />--%>
	          <Hi:DistributorPaymentRadioButtonList ID="radioPaymentMode" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" AutoPostBack="false" /></span>           
	        </li>
	        <li><span class="formitemtitle Pw_198">留言：</span>
	          <asp:TextBox ID="txtRemark" runat ="server" TextMode="MultiLine" Width="400px" Height="60px" CssClass="forminput"></asp:TextBox>
	           <p>如果您对采购单有其他要求可在此填写</p>    
	        </li>
	       <li class="clear"></li>
          </ul>
	      <ul class="btntf Pa_140 clear">
	      <asp:Button ID="btnSubmit" Text="确认提交" CssClass="submit_DAqueding inbnt" OnClientClick="return ThePageIsValid();" runat="server"  />
          </ul>
        </div>
      </div>
</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
    function ThePageIsValid() {
        if ($("#ctl00_contentHolder_txtTel").val().length == 0 && $("#ctl00_contentHolder_txtMobile").val().length == 0) {
            alert("请输入电话号码或手机号码");
            return false;
        }
        return PageIsValid();
    }
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtShipTo', 2, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '姓名长度在2-20个字符以内,只能以字母或汉字开头'));
        initValid(new InputValidator('ctl00_contentHolder_txtAddress', 1, 100, false, null, '详细地址不能为空，必须控制在100个字符以内'));

        initValid(new InputValidator('ctl00_contentHolder_txtTel', 3, 20, true, '^[0-9-]*$', '电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”'));
        initValid(new InputValidator('ctl00_contentHolder_txtMobile', 3, 20, true, '^[0-9]*$', '手机号码长度限制在3-20个字符之间,只能输入数字'));

    }

    function ValidationModeName() {
        var reason = document.getElementById("ctl00_contentHolder_radioShippingMode").value;
        if (reason == "undefined") {
            alert("请选择配送方式");
            return false;
        }

        return true;
    }

        $(document).ready(function() { InitValidators(); });

        function ChooseMode() {
            if ($("#radSell").attr("checked") == "checked") {
                $("#liaddress").show();
                $("#tab_pasteaddress").show(); 
            }
            else{
                $("#liaddress").hide();
                $("#tab_pasteaddress").hide(); 
            }
        }
    </script>
</asp:Content>

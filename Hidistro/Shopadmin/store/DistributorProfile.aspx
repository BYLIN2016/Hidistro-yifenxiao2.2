<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="DistributorProfile.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.DistributorProfile" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
	  <ul>
	    <li class="menucurrent"><a href="#"><span>个人资料</span></a></li>
	    <li><a href="EditLoginPassword.aspx"><span>登录密码</span></a></li>
	    <li><a href="EditTradePassword.aspx"><span>交易密码</span></a></li>
	    <li class="optionend"><a href="EditPasswordProtection.aspx"><span>密保问题与答案</span></a></li>
      </ul>
</div>
<div class="dataarea mainwidth">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
	  <div class="areaform Pa_100 validator2">
	    <ul>
	      <li><span class="formitemtitle Pw_100 ">真实姓名：</span>
	        <asp:TextBox runat="server" ID="txtRealName" CssClass="forminput"></asp:TextBox> 
	        <p id="ctl00_contentHolder_txtRealNameTip">姓名长度在20个字符以内</p>
          </li>
	      <li> <span class="formitemtitle Pw_100">公司名称：</span>
	         <asp:TextBox runat="server" ID="txtCompanyName" CssClass="forminput"></asp:TextBox>
	         <p runat="server" id="txtCompanyNameTip">公司名称长度在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">电子邮件：<em>*</em></span>
	         <asp:TextBox runat="server" ID="txtprivateEmail" CssClass="forminput"></asp:TextBox>
	         <p runat="server" id="txtprivateEmailTip">请输入正确电子邮件，长度在1-256个字符以内</p>
          </li>
	      <li> <span class="formitemtitle Pw_100">省/市：</span> <abbr class="formselect">
	        <Hi:RegionSelector runat="server" ID="rsddlRegion" />
	          </abbr></li>
	          
	       <li> <span class="formitemtitle Pw_100">详细地址：</span>
	       <asp:TextBox runat="server" ID="txtAddress" CssClass="forminput"></asp:TextBox>
	       <p runat="server" id="txtAddressTip">详细地址必须控制在100个字符以内</p>
	      </li>
	      
	      <li> <span class="formitemtitle Pw_100">邮编：</span>
	       <asp:TextBox runat="server" ID="txtZipcode" CssClass="forminput"></asp:TextBox>
	       <p runat="server" id="txtZipcodeTip">邮编限制在3-10个字符以内</p>
	      </li>
	      <li> <span class="formitemtitle Pw_100">QQ：</span>
	        <asp:TextBox runat="server" ID="txtQQ" CssClass="forminput"></asp:TextBox>
	        <p runat="server" id="txtQQTip">QQ号长度限制在3-20个字符之间，只能输入数字</p>
          </li>
          <li> <span class="formitemtitle Pw_100">旺旺：</span>
	        <asp:TextBox runat="server" ID="txtWangwang" CssClass="forminput"></asp:TextBox>
	        <p runat="server" id="txtWangwangTip">请输入正确旺旺帐号，长度在3-20个字符以内</p>
          </li>
	      <li> <span class="formitemtitle Pw_100">MSN：</span>
	       <asp:TextBox runat="server" ID="txtMSN" CssClass="forminput"></asp:TextBox>
	       <p runat="server" id="txtMSNTip">请输入正确MSN帐号，长度在1-256个字符以内</p>
	      </li>
	      <li> <span class="formitemtitle Pw_100">固定电话：</span>
	        <asp:TextBox runat="server" ID="txtTel" CssClass="forminput"></asp:TextBox>
	        <p runat="server" id="txtTelTip">电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”</p>
          </li>
	      <li> <span class="formitemtitle Pw_100">手机号码：</span>
	        <asp:TextBox runat="server" ID="txtCellPhone" CssClass="forminput"></asp:TextBox>
	        <p runat="server" id="txtCellPhoneTip">手机号码长度限制在3-20个字符之间,只能输入数字</p>
          </li>
	      </ul>
	    <ul class="btn Pa_100">
	      <asp:Button runat="server" ID="btnSave" CssClass="submit_DAqueding inbnt" Text="提 交"  OnClientClick="return PageIsValid();"/>
        </ul>
  </div>
	  <!--数据列表底部功能区域-->
</div>
<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtRealName', 0, 20, false, null, '姓名长度在20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtCompanyName', 0, 60, true, null, '公司名称长度在60个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtprivateEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入正确电子邮件，长度在1-256个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtZipcode', 3,10, true, null, '邮编限制在3-10个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 0, 100, true, null, '详细地址必须控制在100个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtQQ', 3, 20, true, '^[0-9]*$', 'QQ号长度限制在3-20个字符之间，只能输入数字'));
            initValid(new InputValidator('ctl00_contentHolder_txtWangwang', 3, 20, true, null, '请输入正确旺旺帐号，长度在3-20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMSN', 1, 256, true, '([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3}){1,2})', '请输入正确MSN帐号，长度在1-256个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 3, 20, true,'^[0-9-]*$', '电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”'));
            initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 3, 20, true, '^[0-9]*$', '手机号码长度限制在3-20个字符之间,只能输入数字'));
            
        }
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>

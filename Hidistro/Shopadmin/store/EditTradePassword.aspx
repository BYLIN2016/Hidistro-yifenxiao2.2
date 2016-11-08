<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditTradePassword.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditTradePassword" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
	  <ul>
        <li class="optionstar"><a href="DistributorProfile.aspx" ><span>个人资料</span></a></li>
        <li><a href="EditLoginPassword.aspx" class="optionnext"><span>登录密码</span></a></li>
        <li class="menucurrent"><a href="#"><span class="optioncenter">交易密码</span></a></li>
	    <li class="optionend"><a href="EditPasswordProtection.aspx"><span>密保问题与答案</span></a></li>
      </ul>
</div>
<div class="dataarea mainwidth">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
	  <div class="areaform Pa_100 validator3">
	    <ul>
	      <li> <span class="formitemtitle Pw_128">旧交易密码：<em>*</em></span>
	        <asp:TextBox ID="txtOldTradePassword" runat="server" TextMode="Password" CssClass="forminput"></asp:TextBox>
	         <p id="txtOldTradePasswordTip" runat="server">请输入旧交易密码</p>
          </li>
          <li> <span class="formitemtitle Pw_128">新交易密码：<em>*</em></span>
             <asp:TextBox ID="txtNewTradePassword" runat="server" TextMode="Password" CssClass="forminput"></asp:TextBox>
             <p id="txtNewTradePasswordTip" runat="server">新交易密码不能为空，长度在6-20个字符之间</p>
          </li>
	      <li> <span class="formitemtitle Pw_128">重复新交易密码：<em>*</em></span>
	        <asp:TextBox ID="txtTradePasswordCompare" runat="server" TextMode="Password" CssClass="forminput"></asp:TextBox>
	        <p id="txtTradePasswordCompareTip" runat="server">请重新输入</p>
	      </li>
	      </ul>
	    <ul class="btn Pa_128">
	      <asp:Button ID="btnEditTradePassword" OnClientClick="return PageIsValid();" Text="确定修改" CssClass="submit_DAqueding inbnt" runat="server"/></div>
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
        initValid(new InputValidator('ctl00_contentHolder_txtOldTradePassword', 6, 20, false, null, '请输入旧交易密码'));
        initValid(new InputValidator('ctl00_contentHolder_txtNewTradePassword', 6, 20, false, null, '新交易密码不能为空，长度在6-20个字符之间'));
            initValid(new CompareValidator('ctl00_contentHolder_txtTradePasswordCompare', 'ctl00_contentHolder_txtNewTradePassword', '两次输入的密码不一致请重新输入'));
        }
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>

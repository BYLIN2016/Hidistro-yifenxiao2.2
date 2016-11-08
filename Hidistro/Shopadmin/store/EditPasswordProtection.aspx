<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditPasswordProtection.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditPasswordProtection" %>
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
        <li class="optionstar"><a href="DistributorProfile.aspx"><span>个人资料</span></a></li>
        <li><a href="EditLoginPassword.aspx"><span>登录密码</span></a></li>
        <li><a href="EditTradePassword.aspx" class="optionnext"><span>交易密码</span></a></li>
        <li class="menucurrent"><a href="#" class="optioncurrentend"><span class="optioncenter">密保问题与答案</span></a></li>
      </ul>
</div>
<div class="dataarea mainwidth">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
	  <div class="areaform Pa_100 validator3">
	    <ul runat="server" id="ulOld">
	      <li> <span class="formitemtitle Pw_128">原来密保问题：<em>*</em></span>
	        <asp:Literal runat="server" ID="litOldQuestion" />
          </li>
          <li> <span class="formitemtitle Pw_128">原来密保答案：<em>*</em></span>
            <asp:TextBox ID="txtOldAnswer" runat="server" CssClass="forminput"></asp:TextBox>
          </li>
        </ul>
	    <ul>
	      <li> <span class="formitemtitle Pw_128">新密保问题：<em>*</em></span>
	       <asp:TextBox ID="txtNewQuestion" runat="server" CssClass="forminput"></asp:TextBox>
	       <p id="txtNewQuestionTip" runat="server">新密保问题不能为空,长度限制在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_128">新密保答案：<em>*</em></span>
            <asp:TextBox ID="txtNewAnswer" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="txtNewAnswerTip" runat="server">新密保答案不能为空，长度在60个字符之间</p>
          </li>
        </ul>
	    <ul class="btn Pa_128">
	      <asp:Button ID="btnEditProtection" OnClientClick="return PageIsValid();" Text="确定修改" CssClass="submit_DAqueding inbnt" runat="server"/>
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
    initValid(new InputValidator('ctl00_contentHolder_txtNewQuestion', 1, 60, false, null, '新密保问题不能为空,长度限制在60个字符以内'));
    initValid(new InputValidator('ctl00_contentHolder_txtNewAnswer', 1, 60, false, null, '新密保答案不能为空，长度在60个字符之间'));        
        }
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>

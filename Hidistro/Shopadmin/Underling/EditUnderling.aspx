<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditUnderling.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditUnderling"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="ManageUnderlings.aspx"><span>会员管理</span></a></li>
                  </ul>
</div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑会员信息</h1>
          </div>
          <div class="formtab Pg_45">
                   <ul>
                      <li class="visited">基本信息</li>                                      
                       <li><a href='<%="EditUnderlingLoginPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'><span>登录密码</span></a></li>
		              <li><a href='<%="EditUnderlingTransactionPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'><span>交易密码</span></a></li>
            </ul>
          </div>
      <div class="formitem validator4 clearfix">
        <ul>
          <li> <span class="formitemtitle Pw_110">会员名：</span>
               <strong class="colorE"><asp:Literal ID="lblLoginNameValue" runat="server"></asp:Literal></strong></li>
          <li> <span class="formitemtitle Pw_110">账户状态：</span>
                <abbr class="formselect">
                   <Hi:ApprovedDropDownList runat="server" ID="ddlApproved" AllowNull="false" />
                 </abbr>
          </li>
          <li> <span class="formitemtitle Pw_110">会员等级：</span>
            <abbr class="formselect">
            <Hi:UnderlingGradeDropDownList ID="drpMemberRankList" runat="server" AllowNull="false" />
          </abbr></li>
          <li> <span class="formitemtitle Pw_110">姓名：</span>
            <asp:TextBox ID="txtRealName" runat="server" CssClass="forminput"></asp:TextBox>
            <p runat="server" id="txtRealNameTip">姓名长度在20个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">生日：</span>
            <UI:WebCalendar ID="calBirthday" runat="server" CssClass="forminput" BeginYear="1900" EndYear="2020" />
          </li>
          <li> <span class="formitemtitle Pw_110">性别：</span>
            <Hi:GenderRadioButtonList runat="server" ID="gender" RepeatLayout="Flow" RepeatDirection="Horizontal" />
          </li>
          <li> <span class="formitemtitle Pw_110">电子邮件地址：<em>*</em></span>
            <asp:TextBox ID="txtprivateEmail" runat="server" CssClass="forminput"></asp:TextBox>
            <p runat="server" id="txtprivateEmailTip">请输入正确电子邮件，长度在1-256个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">详细地址：</span>
            <Hi:RegionSelector runat="server" ID="rsddlRegion" />
          </li>
          <li> <span class="formitemtitle Pw_110">街道地址：</span>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput"></asp:TextBox>
            <p runat="server" id="txtAddressTip">街道地址必须控制在100个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">旺旺：</span>
            <asp:TextBox runat="server" ID="txtWangwang"  CssClass="forminput"/>
            <p runat="server" id="txtWangwangTip">旺旺长度限制在3-60个字符之间</p>
          </li>
          <li> <span class="formitemtitle Pw_110">QQ：</span>
            <asp:TextBox ID="txtQQ" runat="server" CssClass="forminput"></asp:TextBox>
            <p runat="server" id="txtQQTip">QQ号长度限制在3-20个字符之间，只能输入数字</p>
          </li>
          <li> <span class="formitemtitle Pw_110">MSN：</span>
            <asp:TextBox ID="txtMSN" runat="server" CssClass="forminput"></asp:TextBox>
            <p runat="server" id="txtMSNTip">请输入正确MSN帐号，长度在1-256个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">电话号码：</span>
            <asp:TextBox ID="txtTel" runat="server" CssClass="forminput"></asp:TextBox>
            <p runat="server" id="txtTelTip">电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”</p>
          </li>
          <li> <span class="formitemtitle Pw_110">手机号码：</span>
            <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput"></asp:TextBox>
            <p runat="server" id="txtCellPhoneTip">手机号码长度限制在3-20个字符之间,只能输入数字</p>
          </li>
          <li> <span class="formitemtitle Pw_110">注册日期：</span>
            <Hi:FormatedTimeLabel ID="lblRegsTimeValue" runat="server" />
          </li>
          <li> <span class="formitemtitle Pw_110">最后登录日期：</span>
             <Hi:FormatedTimeLabel ID="lblLastLoginTimeValue" runat="server" />
          </li>
          <li> <span class="formitemtitle Pw_110">总消费金额：</span>
             <asp:Literal ID="lblTotalAmountValue" runat="server"></asp:Literal>
          </li>
      </ul>
      <ul class="btn Pa_110">
        <asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtRealName', 0, 20, true, null, '姓名长度在20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtprivateEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入正确电子邮件，长度在1-256个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 0, 100, true, null, '详细地址必须控制在100个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtQQ', 3, 20, true, '^[0-9]*$', 'QQ号长度限制在3-20个字符之间，只能输入数字'));
            initValid(new InputValidator('ctl00_contentHolder_txtWangwang', 3, 60, true, null, '旺旺长度限制在3-60个字符之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtMSN', 1, 256, true, '([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3}){1,2})', '请输入正确MSN帐号，长度在1-256个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 3, 20, true,'^[0-9-]*$', '电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”'));
            initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 3, 20, true,'^[0-9]*$', '手机号码长度限制在3-20个字符之间,只能输入数字'));
        }
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>

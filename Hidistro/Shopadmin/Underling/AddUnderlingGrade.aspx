<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="AddUnderlingGrade.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.AddUnderlingGrade" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="UnderlingGrades.aspx"><span>会员等级管理</span></a></li>
                  </ul>
</div>
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>添加会员等级</h1>
            <span>使用会员等级区分买家的级别，不同级别的买家可以享受不同的折扣率.. </span>
          </div>
      <div class="formitem validator4">
        <ul>
          <li> <span class="formitemtitle Pw_110">会员等级名称：<em >*</em></span>
            <asp:TextBox ID="txtRankName" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtRankNameTip">会员等级名称不能为空，长度限制在20字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">积分满足点数：<em >*</em></span>
            <asp:TextBox ID="txtPoint" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtPointTip">设置会员的积分达到多少分以后自动升级到此等级，为大于等于0的整数</p>
          </li>
          <li> <span class="formitemtitle Pw_110">会员等级价格：<em >*</em></span>
             <span><table width="400" border="0" cellspacing="0">
                          <tr>
                            <td width="59">一口价  ×</td>
                            <td width="39"><asp:TextBox ID="txtValue" CssClass="forminput" Width="90px" runat="server"  /></td>
                            <td width="25" align="center"> %</td>
                            <td width="269" align="center">&nbsp;</td>
                          </tr>
                        </table>
                    </span><br />
		    <p id="ctl00_contentHolder_txtValueTip">级折扣为不能为空，且是数字</p>
          </li>
          <li> <span class="formitemtitle Pw_110">设为默认：</span>
           <Hi:YesNoRadioButtonList id="chkIsDefault" runat="server" RepeatLayout="Flow" />
          </li>
          <li> <span class="formitemtitle Pw_110">备注：</span>
            <asp:TextBox ID="txtRankDesc" runat="server" TextMode="MultiLine"  CssClass="forminput" Width="450" Height="120"></asp:TextBox>
            <p id="ctl00_contentHolder_txtRankDescTip"></p>
          </li>
      </ul>
      <ul class="btn Pa_110 clear">
        <asp:Button ID="btnSubmitMemberRanks" OnClientClick="return PageIsValid();" Text="确 定" CssClass="submit_DAqueding" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
  <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtRankName', 1, 60, false, null, '会员等级名称不能为空，长度限制在60个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtPoint', 1, 10, false, '-?[0-9]\\d*', '设置会员的积分达到多少分以后自动升级到此等级，为大于等于0的整数'));
                appendValid(new NumberRangeValidator('ctl00_contentHolder_txtPoint', 0, 2147483647, '设置会员的积分达到多少分以后自动升级到此等级，为大于等于0的整数'));
                initValid(new InputValidator('ctl00_contentHolder_txtValue', 1, 10, false, '-?[0-9]\\d*', '等级折扣为不能为空，且是数字'));
                appendValid(new NumberRangeValidator('ctl00_contentHolder_txtValue', 1, 100, '等级折扣必须在1-100之间'));
                initValid(new InputValidator('ctl00_contentHolder_txtRankDesc', 0, 100, true, null, '备注的长度限制在100个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>

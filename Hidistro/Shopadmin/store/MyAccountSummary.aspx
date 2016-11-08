<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyAccountSummary.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyAccountSummary" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth td_top_ccc">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="toptitle"><em><img src="../images/09.gif" width="32" height="32" /></em>
        <h1 >账户概要</h1>
        <span >查看自己的预付款账户概要.</span> </div>
  <!--数据列表区域-->
<div class="datalist">
	      <table width="200" border="0" cellspacing="0">
	        <tr class="table_title">
	          <td colspan="5" class="td_right td_right_fff">预付款账户余额</td>
            </tr>
	        <tr>
	          <td width="11%" align="right">预付款总额：</td>
	          <td colspan="2"> <strong class="colorA fonts"><Hi:FormatedMoneyLabel ID="lblAccountAmount" runat="server" /></strong></td>
	          <td width="12%" align="right"><span class="Name"><a href="MyBalanceDetails.aspx">查看帐户明细</a></span></td>
	          <td width="59%">&nbsp;</td>
            </tr>
	        <tr>
	          <td align="right">可用余额：</td>
	          <td width="9%"><strong class="colorB fonts"><Hi:FormatedMoneyLabel ID="lblUseableBalance" runat="server" /></strong></td>
	          <td width="9%" class="colorA">余额不足?</td>
	          <td align="right"><span class="Name"><a href="ReCharge.aspx">立即充值</a></span></td>
	          <td>&nbsp;</td>
            </tr>
	        <tr>
	          <td align="right">提现冻结金额：</td>
	          <td><strong class="colorQ"><Hi:FormatedMoneyLabel ID="lblFreezeBalance" runat="server" /></strong></td>
	          <td class="colorA">&nbsp;</td>
	          <td align="right">&nbsp;</td>
	          <td>&nbsp;</td>
            </tr>
          </table>
</div>
	  <!--数据列表底部功能区域-->
</div>
<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

	   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

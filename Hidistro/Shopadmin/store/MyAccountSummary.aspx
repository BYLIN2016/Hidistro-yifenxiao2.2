<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyAccountSummary.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyAccountSummary" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth td_top_ccc">
	  <!--����-->
	  <!--����-->
	  <!--�����б�����-->
  <div class="toptitle"><em><img src="../images/09.gif" width="32" height="32" /></em>
        <h1 >�˻���Ҫ</h1>
        <span >�鿴�Լ���Ԥ�����˻���Ҫ.</span> </div>
  <!--�����б�����-->
<div class="datalist">
	      <table width="200" border="0" cellspacing="0">
	        <tr class="table_title">
	          <td colspan="5" class="td_right td_right_fff">Ԥ�����˻����</td>
            </tr>
	        <tr>
	          <td width="11%" align="right">Ԥ�����ܶ</td>
	          <td colspan="2"> <strong class="colorA fonts"><Hi:FormatedMoneyLabel ID="lblAccountAmount" runat="server" /></strong></td>
	          <td width="12%" align="right"><span class="Name"><a href="MyBalanceDetails.aspx">�鿴�ʻ���ϸ</a></span></td>
	          <td width="59%">&nbsp;</td>
            </tr>
	        <tr>
	          <td align="right">������</td>
	          <td width="9%"><strong class="colorB fonts"><Hi:FormatedMoneyLabel ID="lblUseableBalance" runat="server" /></strong></td>
	          <td width="9%" class="colorA">����?</td>
	          <td align="right"><span class="Name"><a href="ReCharge.aspx">������ֵ</a></span></td>
	          <td>&nbsp;</td>
            </tr>
	        <tr>
	          <td align="right">���ֶ����</td>
	          <td><strong class="colorQ"><Hi:FormatedMoneyLabel ID="lblFreezeBalance" runat="server" /></strong></td>
	          <td class="colorA">&nbsp;</td>
	          <td align="right">&nbsp;</td>
	          <td>&nbsp;</td>
            </tr>
          </table>
</div>
	  <!--�����б�ײ���������-->
</div>
<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--����logo����-->
</div>

	   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

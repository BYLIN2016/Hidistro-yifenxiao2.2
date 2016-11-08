<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ReChargeConfirm.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ReChargeConfirm" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth td_top_ccc">
	  <!--搜索-->
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="toptitle td_bottom_ccc"><em><img src="../images/09.gif" width="32" height="32" /></em>
    <h1 >预付款充值</h1>
  <span >通过在线支付方式为自己的预付款帐户充值预付款.</span></div>
<div class="blank12 clearfix"></div>
  <div class="areaform validator1">
	    <ul>
	      
	      <li class="Pa_15"><span class="formitemtitle Pw_198">用户名：</span>
          <strong><asp:Literal runat="server" ID="litRealName"></asp:Literal></strong></li>
          <li><span class="formitemtitle Pw_198">充值方式：<em>*</em></span>
	       <asp:Literal ID="lblPaymentName" runat="server"></asp:Literal>
            </li>
            <li><span class="formitemtitle Pw_198">充值金额：<em>*</em></span>
               <Hi:FormatedMoneyLabel ID="lblBlance" runat="server" /> (手续费：<asp:Literal runat="server" ID="litPayCharge"></asp:Literal>)
            </li>
        </ul>
  </div>
  <div class="btn Pa_198">
    <asp:Button runat="server" ID="btnConfirm"  CssClass="submit_DAqueding" Text="确认充值" />
  </div>
<div class="btn Pa_198">
  <!--数据列表底部功能区域-->
</div>
</div>
</asp:Content>

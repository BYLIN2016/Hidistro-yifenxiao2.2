<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TaoBaoNote.aspx.cs" Inherits="Hidistro.UI.Web.Admin.TaoBaoNote" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>订单处理精灵介绍</h1>
  <span>您可以向您的分销商（淘宝卖家）介绍订单处理精灵，让他们来订购，以便分销您的商品到各个淘宝店铺。</span>
</div>
		<!--搜索-->
	
		<!--数据列表区域-->
		<div class="datalist datafrom">
		 <div class="formitem">
                <span style="margin-bottom:20px;">什么是订单处理精灵？<br/>订单处理精灵是淘宝卖家与独立批发网站（供应商）达成协议，由供应商提供商品数据给淘宝卖家发布到自己的淘宝店铺进行销售，商品批量同步，订单批量同步，发货信息同步。详情请 <a href="http://fuwu.taobao.com/ser/detail.htm?spm=0.0.0.0.xKlmti&service_code=ts-1818696&tracelog=other_serv" target="_blank">查看</a>
                </span>
                <div class="clear">
                <ul>
                    <li>
                        怎样订购订单处理精灵？ <br/>订单处理精灵是一款收费的淘宝应用，您可以让您的分销商（淘宝卖家）马上订购使用。
                        <input type="button" class="btnmessage" value="订购网址" onclick="javascript:window.open('http://fuwu.taobao.com/ser/detail.htm?spm=0.0.0.0.xKlmti&service_code=ts-1818696&tracelog=other_serv');" />
                      </li>
                      <li>
                        怎样使用订单处理精灵？ <br/>当您的分销商订购完订单处理精灵后，还需要和独立商城绑定才能使用。<br/>
                        让您的分销商登录进入分销商后台，点击页面顶部的 <img src="../images/tubiao.jpg" alt="同步淘宝" style="border-width:0px;" />小图标，打开订单处理精灵登录授权页面，登录他的淘宝后，便和独立商城绑定起来了。
                        </li>
                        <li>
                            选择淘宝代销配送方式：
                            <Hi:ShippingModeDropDownList runat="server" ID="dropShippingType" NullToDisplay="请选择配置方式" />
                            <asp:Button ID="btnSave" runat="server" CssClass="submit_DAqueding inbnt" Text="保 存"/>
                        </li>
                </ul>

         </div>

           
        </div>
		<!--数据列表底部功能区域-->

	</div>

	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">	
</asp:Content>  

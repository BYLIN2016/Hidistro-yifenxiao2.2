<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProductComplete.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddProductComplete" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>添加新商品成功</h1>
            <span>商品添加成功，您还可以进行以下操作：</span>
        </div>
          <div class="formitem">
          <span class="msg">商品添加成功！</span>
         </div>
          <div class="Pg_15 Pg_45 fonts"><span class="float">你可以</span>
            <asp:HyperLink ID="hlinkProductDetails" runat="server" Target="_blank" Text="查看" />
             商品
        </div>
          <div class="Pg_15 Pg_45 fonts">您还可以继续到  <span class="Name">
            <asp:HyperLink ID="hlinkAddProduct" runat="server" Text="在当前分类下添加商品" />
            </span></div>
		  <div class="Pg_15 Pg_45 fonts">或者  <span class="Name"><a href="SelectCategory.aspx">重新选择分类添加商品</a> </span></div>
		  <div class="Pg_15 Pg_45 fonts">您可以随时到  <span class="Name"><a href="ProductOnSales.aspx">商品列表</a></span>  去编辑商品。</div>
      </div>
        
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

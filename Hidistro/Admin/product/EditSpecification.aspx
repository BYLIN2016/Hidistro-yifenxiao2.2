<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditSpecification.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditSpecification" %>
<%@ Register TagPrefix="cc1" TagName="SpecificationView" Src="~/Admin/product/ascx/SpecificationView.ascx" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>编辑商品类型</h1>
            <span>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，一个商品类型下可添加多种属性.一种是供客户查看的扩展属性,如图书类型商品的作者，出版社等，一种是供客户可选的规格,如服装类型商品的颜色、尺码。</span>
      </div>
        <div class="formtab">
                   <ul>
                      <li ><a href='<%= Globals.GetAdminAbsolutePath("/product/EditProductType.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>基本设置</a></li>                                      
                      <li><a href='<%= Globals.GetAdminAbsolutePath("/product/EditAttribute.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>扩展属性</a></li>
                      <li class="visited">规 格</li>
            </ul>
          </div>
            <cc1:SpecificationView runat="server" ID="specificationView" />
  </div>        
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
  
</asp:Content>

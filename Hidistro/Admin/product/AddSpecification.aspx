<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddSpecification.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.AddSpecification" %>
<%@ Register TagPrefix="cc1" TagName="SpecificationView" Src="~/Admin/product/ascx/SpecificationView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        #newPreview { FILTER: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)}
    </style>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
    <div class="columnright">
        <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1><table><tr><td>添加新的商品类型</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/tjsplx.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
            <span>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，一个商品类型下可添加多种属性.一种是供客户查看的扩展属性,如图书类型商品的作者，出版社等，一种是供客户可选的规格,如服装类型商品的颜色、尺码。</span>
        </div>
        <div class="Steps">
          <ul>
              <li class="fui">第一步：添加类型名称</li>    
              <li class="iocns "></li>
              <li class="fui">第二步：添加扩展属性</li>
              <li class="iocns "></li>
              <li class="huang">第三步：添加规格</li>
          </ul>
        </div>
         <cc1:SpecificationView runat="server" ID="specificationView" />
         
        <div class="Pg_15">
              <asp:Button ID="btnFilish" runat="server" CssClass="submit_DAqueding" Text="下一步" />
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
           function InitValidators() {
               initValid(new InputValidator('ctl00_contentHolder_specificationView_txtName', 1, 30, false, null, '规格名称长度在1至30个字符之间'));
           }
           $(document).ready(function() { InitValidators(); });
       </script>
</asp:Content>



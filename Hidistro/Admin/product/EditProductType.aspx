<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditProductType.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditProductType" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
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
          <div class="formtab Pg_45">
           <ul>
              <li class="visited">基本设置</li>                                      
              <li>
                <a href='<%= Globals.GetAdminAbsolutePath("/product/EditAttribute.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>扩展属性</a>
              </li>
              <li>
                <a href='<%= Globals.GetAdminAbsolutePath("/product/EditSpecification.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>规 格</a>
              </li>
            </ul>
          </div>
          <div class="formitem validator2">
            <ul>
              <li> <span class="formitemtitle Pw_100">商品类型名称：</span>
                <asp:TextBox ID="txtTypeName" CssClass="forminput" runat="server" Width="320"></asp:TextBox>
                <p id="txtTypeNameTip" runat="server">商品类型名称不能为空，长度限制在1-30个字符之间</p>
              </li>
               <li> <span class="formitemtitle Pw_100">关联品牌：</span>
                  <span><Hi:BrandCategoriesCheckBoxList runat="server" ID="chlistBrand" /></span>
                </li>
              <li> <span class="formitemtitle Pw_100">备注：</span>
                <asp:TextBox ID="txtRemark" TextMode="MultiLine" Width="320" Height="90" runat="server" ></asp:TextBox>
                <p id="txtRemarkTip" runat="server">备注的长度限制在0-100个字符之间</p>
              </li>
            </ul>
            <ul class="btn Pa_100">
            <asp:Button ID="btnEditProductType" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"  />
            </ul>
          </div>
  </div>        
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function InitValidators()
{
    initValid(new InputValidator('ctl00_contentHolder_txtTypeName', 1, 30, false, null, '商品类型名称不能为空，长度限制在1-30个字符之间'))
    initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 100, true, null, '备注的长度限制在0-100个字符之间'))
}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>

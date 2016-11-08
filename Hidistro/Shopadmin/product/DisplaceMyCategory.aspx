<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Shopadmin.DisplaceMyCategory" MasterPageFile="~/Shopadmin/Shopadmin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="ManageMyCategories.aspx"><span>商品分类管理</span></a></li>
                  </ul>
        </div>
      <div class="columnright">
          <div class="title"> 
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>商品分类批量替换</h1>
            <span>将某一分类的商品批量替换到另一个商品分类中</span></div>
        <div class="formitem">
            <ul>
              <li> <span class="formitemtitle Pw_160">需要替换的商品分类：<em >*</em></span>
                   <abbr class="formselect">
                    <Hi:DistributorProductCategoriesDropDownList ID="dropCategoryFrom" Width="239px" runat="server" AutoDataBind="false" />
                   </abbr>
              </li>
              <li><span class="formitemtitle Pw_160">替换至：<em >*</em></span>
                   <abbr class="formselect">
                   <Hi:DistributorProductCategoriesDropDownList ID="dropCategoryTo" Width="239px" runat="server" AutoDataBind="true" />
                   </abbr>

              </li>
            </ul>
            <ul class="btn Pa_160">
            <asp:Button ID="btnSaveCategory" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding" />
           </ul>
       </div>  
  </div>
</div>

</asp:Content>


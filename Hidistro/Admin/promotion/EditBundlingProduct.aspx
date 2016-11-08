<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditBundlingProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.EditBundlingProduct" %>
<%@ Register Assembly="Hidistro.UI.SaleSystem.Tags" Namespace="Hidistro.UI.SaleSystem.Tags" TagPrefix="Hi" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
   <script type="text/javascript" src="../js/BundlingProduct.js"></script>
<div class="areacolumn clearfix">

      <div class="columnright">
          <div class="title">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1>修改捆绑商品</h1>
            <span>修改捆绑销售商品</span>
          </div>
      <div class="formitem validator5">
        <ul>
          <li> <span class="formitemtitle Pw_140"><em >*</em>捆绑商品名称：</span>
          <asp:TextBox ID="txtBindName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtBindNameTip">名称不能为空，在1至60个字符之间</p>
          </li>
          
               <li> <span class="formitemtitle Pw_140"><em >*</em>是否上架：</span>
         <Hi:YesNoRadioButtonList  ID="radstock" runat="server" RepeatLayout="Flow" />
          </li>
          <li style="display:none;">         
             <span class="formitemtitle Pw_140">捆绑商品库存：</span>
             <asp:TextBox ID="txtNum" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtNumTip">库存必须为整数类型
              </p>
          </li>
          <li>
            <span class="formitemtitle Pw_140"><em >*</em>原价格：</span> <span id="totalprice"></span>  
          </li>
          <li><span class="formitemtitle Pw_140"><em >*</em>捆绑销售价：</span>
            <asp:TextBox ID="txtSalePrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtSalePriceTip">捆绑销售金额只能是数值，0.01-10000000，且不能超过2位小数</p>
          </li>
          <li ><span class="formitemtitle Pw_140">捆绑商品简介：</span>
                <Hi:TrimTextBox runat="server" Rows="6" Columns="76"  MaxLength="300"    ID="txtShortDescription" TextMode="MultiLine" />
                <p  id="ctl00_contentHolder_txtShortDescriptionTip" class="Pa_198">限定在300个字符以内</p>
            </li>          
       <li><span class="formitemtitle Pw_140 "><em >*</em>选择商品：</span><span style="cursor:pointer; color:blue; font-size:14px"  onclick="ShowAddDiv();">请点此选择</span>
               <p id="P1">捆绑商品必须为两件或者两件以上</p>
          </li>    
          <li class="binditems">
          <table width="100%" id="addlist">
          <tr class="table_title"><th>商品名</th><th>sku信息</th><th>价格</th><th>数量</th><th>操作</th></tr>
                 <asp:Repeater ID="Rpbinditems" runat="server" >
              <ItemTemplate> <tr name='appendlist'><td ><%#Eval("ProductName")%></td><td >
                  <Hi:SkuContentLabel runat="server" ID="litSkuContent" Text='<%#Eval("SkuId") %>' />
                 </td><td ><%#Eval("ProductPrice") %></td><td ><input type='text' value='<%#Eval("ProductNum") %>' name='txtNum'/></td><td style='display:none'><%#Eval("productid") %>|<%#Eval("skuid") %></td><td ><span  id='<%#Eval("skuid") %>'  style='cursor:pointer;color:blue' onclick='Remove(this)'>删除</span></td></tr>
</ItemTemplate>
              </asp:Repeater>
          </table>    
           <input   id="selectProductsinfo"  name="selectProductsinfo"  type="hidden" />
          </li>      
     
           <li> <asp:Button ID="btnEditBindProduct" runat="server" Text="确定" 
                   OnClientClick="return PageIsValid()&&CollectInfos();"  CssClass="submit_DAqueding" 
                   onclick="btnEditBindProduct_Click"  /></li>
      </ul>
      </div>

      </div>
       <div class="Pop_up" id="divWindow" style="display:none;">
    <div> <h1>查找商品</h1><div class="img_datala"><a onclick="CloseDiv('divWindow')" href="#"><img height="20" width="38" src="../images/icon_dalata.gif" style="cursor: pointer;"></a></div></div>
          <div style="width:100%">
              <table style="width:100%;border-bottom:0px;">
                <tbody>
                    <tr>
                        <td nowrap="nowrap">商品名称：</td>
                        <td><input id="serachName" class="forminput"/></td>
                        <td nowrap="nowrap"><Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" /></td>
                        <td><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" CssClass="forminput" /></td>
                        <td><input type="button" class="searchbutton" value="查找" onclick="search()" id="btnSearch" /></td>
                    </tr>
                </tbody>
            </table> 
             <table cellpadding="0" cellspacing="0" width="100%" id="serachResult"  class="panel-body"  style="width:100%;border-collapse:collapse;">
             </table>
       </div>
        <div class="page">
               <input  type="hidden" name="currentPage" id="currentPage" />
               共<span id="sp_pagetotal">15</span>页　第<span id="sp_pageindex">5</span>页<span onclick="prevPage()" id="prevLink" >上一页</span><span id="nextLink" onclick="nextPage()">下一页</span>
         </div>
        <div style="text-align:center; clear:both"><input type="button"  onclick="selectProduct()"    id="btnAdd" value="确&nbsp;&nbsp;定"  class="submit_DAqueding"  />　　　　
        <input type="button" onclick="javascript:CloseDiv('divWindow')" class="submit_DAqueding" value="取&nbsp;&nbsp;消" /></div>
</div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" >
function InitValidators() {
initValid(new InputValidator('ctl00_contentHolder_txtBindName', 1, 60, false, null, '捆绑商品的名称,在1至60个字符之间'));
initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '销售价格只能是数值，0.01-10000000，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0.01, 10000000.00, '销售价格只能是数值，0.01-10000000，且不能超过2位小数'));
 initValid(new InputValidator('ctl00_contentHolder_txtNum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，库存只能输入整数型数值'))
 appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNum', 1, 9999999, '输入的数值超出了系统表示范围'));
 initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '商品简介必须限制在300个字符以内'));       
}
$(document).ready(function () { InitValidators(); GetTotalPrice(); });
</script>
</asp:Content>
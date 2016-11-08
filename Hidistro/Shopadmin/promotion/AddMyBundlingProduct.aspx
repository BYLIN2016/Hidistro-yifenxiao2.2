<%@ Page Title="" Language="C#" MasterPageFile="~/ShopAdmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="AddMyBundlingProduct.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.AddMyBundlingProduct" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="../script/BundlingProduct.js">
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
<div class="columnleft clearfix"><ul><li><a href="MyBundlingProducts.aspx"><span>捆绑商品管理</span></a></li></ul></div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1 class="title_line">添加捆绑商品</h1>
          </div>
      <div class="formitem validator5">
        <ul>
          <li> <span class="formitemtitle Pw_140">捆绑商品名称：<em >*</em></span>
          <asp:TextBox ID="txtBindName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtBindNameTip">名称不能为空，在1至60个字符之间</p>
          </li>
          
               <li> <span class="formitemtitle Pw_140">是否上架：<em >*</em></span>
         <Hi:YesNoRadioButtonList  ID="radstock" runat="server" RepeatLayout="Flow" />
          </li>
          <li style="display:none;">
             <span class="formitemtitle Pw_140">捆绑商品库存：</span>
             <asp:TextBox ID="txtNum" runat="server"  Text="0" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtNumTip">库存必须为整数类型
              </p>
          </li>
          <li>
            <span class="formitemtitle Pw_140">原价格：<em >*</em></span>    <span id="totalprice"></span>
          </li>
          <li><span class="formitemtitle Pw_140">捆绑销售价：<em >*</em></span>
            <asp:TextBox ID="txtSalePrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtSalePriceTip">捆绑销售金额只能是数值，0.01-10000000，且不能超过2位小数</p>
          </li>
              <li ><span class="formitemtitle Pw_140">捆绑商品简介：</span>
                <Hi:TrimTextBox runat="server" Rows="6" Columns="56"  MaxLength="300"    ID="txtShortDescription" TextMode="MultiLine" />
                <p  id="ctl00_contentHolder_txtShortDescriptionTip" class="Pa_198">限定在300个字符以内</p>
            </li>       
       <li><span class="formitemtitle Pw_140 ">选择商品：<em >*</em></span><span style="cursor:pointer; color:blue; font-size:14px"  onclick="ShowAddDiv();">请点此选择</span>
               <p id="P1">捆绑商品必须为两件或者两件以上</p>
          </li>    
          <li class="binditems">
          <table width="100%" id="addlist">
          <tr class="table_title"><th class="td_right td_left" scope="co1">商品名</th><th class="td_right td_left" scope="co1">sku信息</th><th class="td_right td_left" scope="co1">价格</th><th class="td_right td_left" scope="co1">数量</th><th class="td_left td_right_fff" scope="co1">操作</th></tr>
          </table>    
           <input   id="selectProductsinfo"  name="selectProductsinfo"  type="hidden" />
          </li>      
     
           <li> <asp:Button ID="btnAddBindProduct" runat="server" Text="确定" 
                   OnClientClick="return PageIsValid()&&CollectInfos(); "  CssClass="submit_DAqueding" 
                   onclick="btnAddBindProduct_Click"  /></li>
      </ul>
      </div>

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
$(document).ready(function () {
    InitValidators();
});
 </script>
</asp:Content>

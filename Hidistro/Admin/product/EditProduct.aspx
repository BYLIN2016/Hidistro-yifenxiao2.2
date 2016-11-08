<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="EditProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditProduct" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery_hashtable.js" />
     <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-powerFloat-min.js" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>编辑商品信息</h1>
	    <span>商品信息修改</span>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>
          <li><h2 class="colorE">基本信息</h2></li>
	        <li>
	            <span class="formitemtitle Pw_198">所属商品分类：</span>
                <span class="colorE float fonts"><asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span>
                [<asp:HyperLink runat="server" ID="lnkEditCategory" CssClass="a" Text="编辑"></asp:HyperLink>]
            </li>
	        <li>
	            <span class="formitemtitle Pw_198">商品类型：</span>
                <abbr class="formselect"><Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--请选择--" /></abbr>
	            品牌：<abbr class="formselect"><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandCategories" NullToDisplay="--请选择--" /></abbr>
            </li>
            <li> <span class="formitemtitle Pw_198">所属产品线：</span>
	              <abbr class="formselect">
	                <Hi:ProductLineDropDownList runat="server" ID="dropProductLines" NullToDisplay="--请选择--" />
              </abbr>
                <p id="pShowProductLineChage" style="display:none;">由于产品线的变更，将影响<asp:HyperLink Target="_blank" ID="hlinkDistributor" runat="server"  />，建议稍后调整产品线授权</p>
	       <input type="hidden" value="0" id="hdlineId" runat="server" />
	        </li>
	        <li class=" clearfix"> <span class="formitemtitle Pw_198">商品名称：<em >*</em></span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" Width="350px"/>
              <p id="ctl00_contentHolder_txtProductNameTip">限定在60个字符</p>
	        </li>
	        <li> <span class="formitemtitle Pw_198">排序：<em >*</em></span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtDisplaySequence" />
              <p id="ctl00_contentHolder_txtDisplaySequenceTip">商品显示顺序，越大排在越前</p>
	        </li>
	        <li><span class="formitemtitle Pw_198">商家编码：</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductCode" />
                <p id="ctl00_contentHolder_txtProductCodeTip">长度不能超过20个字符</p>
            </li>
	        <li><span class="formitemtitle Pw_198">计量单位：</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtUnit" />
                <p id="ctl00_contentHolder_txtUnitTip">必须限制在20个字符以内且只能是英文和中文例:g/元</p>
            </li>
            <li><span class="formitemtitle Pw_198">市场价：</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />元
                <p id="ctl00_contentHolder_txtMarketPriceTip">本站会员所看到的商品市场价</p>
	        </li>
	        <li><span class="formitemtitle Pw_198">分销商最低零售价：<em >*</em></span>
            <span style="float:left"><table style="border:none;" width="150px">
            <tr><td style="border:none;"><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtLowestSalePrice" /></td>
            <td style="border:none; width:10px;"> 元</td></tr>
            </table>
            </span>                
             </li>
	        <li><h2 class="colorE">扩展属性</h2></li>
	        <li id="attributeRow" style="display:none;"><span class="formitemtitle Pw_198">商品属性：</span>
	        <div class="attributeContent" id="attributeContent"></div>
            <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
            </li>
	        <li id="skuCodeRow"><span class="formitemtitle Pw_198">货号：</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSku" />
                <p id="ctl00_contentHolder_txtSkuTip">限定在20个字符</p>
            </li>
	        <li id="salePriceRow"><span class="formitemtitle Pw_198">一口价：<em >*</em></span>
              <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSalePrice" />元<input type="button" onclick="editProductMemberPrice();" value="编辑会员价" />
              <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
              <p id="ctl00_contentHolder_txtSalePriceTip">本站会员所看到的商品零售价</p>
	        </li>
	        <li id="costPriceRow"><span class="formitemtitle Pw_198">成本价：</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtCostPrice" />元
	            <p id="ctl00_contentHolder_txtCostPriceTip">商品的成本价</p>
	        </li>
            <li id="purchasePriceRow"><span class="formitemtitle Pw_198">分销商采购价：<em >*</em></span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtPurchasePrice" />元<input type="button" onclick="editProductDistributorPrice();" value="编辑分销等级价"/>
                <Hi:TrimTextBox runat="server" ID="txtDistributorPrices" TextMode="MultiLine" style="display:none"></Hi:TrimTextBox>
                <p id="ctl00_contentHolder_txtPurchasePriceTip">分销商的采购价</p>
            </li>
            <li id="qtyRow"><span class="formitemtitle Pw_198">商品库存：<em >*</em></span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtStock" /></li>
            <li id="alertQtyRow"><span class="formitemtitle Pw_198">警戒库存：<em >*</em></span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtAlertStock" /></li>
            <li id="weightRow"><span class="formitemtitle Pw_198">商品重量：</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtWeight" />克</li>
            <li id="skuTitle" style="display:none;"><h2 class="colorE">商品规格</h2></li>
            <li id="enableSkuRow" style="display:none;"><span class="formitemtitle Pw_198">规格：</span><input id="btnEnableSku" type="button" value="开启规格" /> 
                开启规格前先填写以上信息，可自动复制信息到每个规格</li>
            <li id="skuRow"  style="display:none;">
                <p id="skuContent">
                   
                    <input type="button" id="btnshowSkuValue" value="生成部分规格" />
                    <input type="button" id="btnAddItem" value="增加一个规格" />
                    <input type="button" id="btnCloseSku" value="关闭规格" />
                     <input type="button" id="btnGenerateAll" value="生成所有规格" />
                </p>
                <p id="skuFieldHolder" style="margin:0px auto;display:none;"></p>
                <div id="skuTableHolder">
                </div>
                <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" style="display:none" ></Hi:TrimTextBox>
                <asp:CheckBox runat="server" ID="chkSkuEnabled" style="display:none;" />
            </li>
            <li><h2 class="colorE">图片和描述</h2></li>
            <li><span class="formitemtitle Pw_198">商品图片：</span>
                 <div class="uploadimages">
                    <Hi:ImageUploader runat="server" ID="uploader1" />
                </div>
                <div class="uploadimages">
                    <Hi:ImageUploader runat="server" ID="uploader2" />
                </div>
                <div class="uploadimages">
                    <Hi:ImageUploader runat="server" ID="uploader3" />
                </div>
                <div class="uploadimages">
                    <Hi:ImageUploader runat="server" ID="uploader4" />
                </div>
                <div class="uploadimages">
                    <Hi:ImageUploader runat="server" ID="uploader5" />
                </div>
                <p class="Pa_198 clearfix"m_none>图片应小于120k，jpg,gif,jpeg,png或bmp格式。建议为500x500像素</p>
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_198">商品简介：</span>
                <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="txtShortDescription" TextMode="MultiLine" />
                <p class="Pa_198">限定在300个字符以内</p>
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_198">商品描述：</span>
                <Kindeditor:KindeditorControl ID="fckDescription" runat="server"  Height="300"/>
                <p style="color:Red;"><asp:CheckBox runat="server" ID="ckbIsDownPic" Text="是否下载商品描述外站图片" /></p>
                <p class="Pa_198">如果勾选此选项时，商品里面有外站的图片则会下载到本地，相反则不会，由于要下载图片，如果图片过多或图片很大，需要下载的时间就多，请慎重选择。</p>
            </li>
	        <li><h2 class="colorE clear"> 相关设置</h2></li>
	        <li>
			  <span class="formitemtitle Pw_198">商品销售状态：</span>
				 <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus"  Text="出售中"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus"  Text="下架区"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"  Text="仓库中"></asp:RadioButton>
 			</li>
            <li class="clearfix">
			    <span class="formitemtitle Pw_198">铺货设置：</span>
				<asp:CheckBox runat="server" ID="chkPenetration" Text="立刻铺货" />
            </li>
            <li class="clearfix" id="l_tags" runat="server">
			   <span class="formitemtitle Pw_198">商品标签定义：<a id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="add">添加</a></span>
			   
			   <div id="div_tags"> <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral></div>
			   <div id="div_addtag" style="display:none"><input type="text" id="txtaddtag" /><input type="button" value="保存" onclick="return AddAjaxTags()" /></div>
			     <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox> 
            </li>
	        <li class="clearfix"><h2 class="colorE">搜索优化</h2></li>
	        <li> <span class="formitemtitle Pw_198">详细页标题：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtTitle" Width="350px"/>
              <p id="ctl00_contentHolder_txtTitleTip">详细页标题限制在100字符以内</p>
            </li>
	        <li> <span class="formitemtitle Pw_198">详细页描述：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaDescription" Width="350px"/>
              <p id="ctl00_contentHolder_txtMetaDescriptionTip">详细页描述限制在260字符以内</p>
            </li>
	        <li> <span class="formitemtitle Pw_198">详细页搜索关键字：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaKeywords" Width="350px"/>
              <p id="ctl00_contentHolder_txtMetaKeywordsTip">详细页搜索关键字限制在160字符以内</p>
	        </li>
	      </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnSave" Text="保 存" OnClientClick="return doSubmit();" CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>
<div class="Pop_up" id="priceBox" style="display: none;">
    <h1>
        <span id="popTitle"></span>
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" alt="关闭" width="38" height="20" />
   </div>
    <div class="mianform ">
        <div id="priceContent">
        </div>
        <div style="margin-top:10px;text-align:center;">
            <input type="button" value="确定" onclick="doneEditPrice();" />
        </div>
    </div>
</div>

<div class="Pop_up" id="skuValueBox" style="display: none;">
    <h1>
        <span>选择要生成的规格</span>
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" alt="关闭" width="38" height="20" />
   </div>
   
    <div class="mianform ">
        <div id="skuItems" >
        </div>
        <div style="margin-top:10px;text-align:center;">
            <input type="button" value="确定" id="btnGenerate" />
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '商品名称字符长度在1-60之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtDisplaySequence', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtDisplaySequence', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 20, true, null, '商家编码的长度不能超过20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtPurchasePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchasePrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtLowestSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtLowestSalePrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 20, true, null, '货号的长度不能超过20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStock', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtStock', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtAlertStock', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtAlertStock', 0, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 20, true, '[a-zA-Z\/\u4e00-\u9fa5]*$', '必须限制在20个字符以内且只能是英文和中文例:g/元'))
            initValid(new InputValidator('ctl00_contentHolder_txtWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtWeight', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '商品简介必须限制在300个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 0, 100, true, null, '详细页标题长度限制在100个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 0, 260, true, null, '详细页描述长度限制在260个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 0, 160, true, null, '详细页搜索关键字长度限制在160个字符以内'));
        }
        $(document).ready(function() { InitValidators(); });
    </script>
    <script type="text/javascript" src="attributes.helper.js"></script>
    <script type="text/javascript" src="grade.price.helper.js"></script>
        <script type="text/javascript" src="producttag.helper.js"></script>
</asp:Content>
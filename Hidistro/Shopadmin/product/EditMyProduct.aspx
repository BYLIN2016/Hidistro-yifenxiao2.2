<%@ Page Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true"  CodeBehind="EditMyProduct.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyProduct" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
   
    <div class="dataarea mainwidth databody">
	  <div class="title  m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>编辑商品</h1>
        <span>商品基本信息</span>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>
          <h2 class="colorE">1.商品基本信息</h2>
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
	        <li class="m_none" id="attributeRow">        
	          <span class="formitemtitle Pw_198">商品属性：</span>
               <Hi:ProductAttributeDisplay ID="ProductAttributeDisplay1" runat="server" />
            </li>
	        <li class=" clearfix"> <span class="formitemtitle Pw_198">商品名称：<em >*</em></span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" MaxLength="30" Width="350px" />
              <p id="ctl00_contentHolder_txtProductNameTip">限定在30个字符</p>
	        </li>
	        
	        
             <li> <span class="formitemtitle Pw_198">排序：<em >*</em></span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtDisplaySequence" />
              <p id="ctl00_contentHolder_txtDisplaySequenceTip">商品显示顺序，越大排在越前,如果非数字则改商品将排在最后</p>
	        </li>  
	        
            <li> <span class="formitemtitle Pw_198">所属产品线：</span>
	              <abbr class="formselect">
	                <Hi:ProductLineDropDownList runat="server" ID="dropProductLines" NullToDisplay="--请选择--" />
              </abbr>
	        </li>
            <li><span class="formitemtitle Pw_198">分销商最低零售价：</span>
                <asp:Literal runat="server"  ID="litLowestSalePrice" />
             </li>
             <li><span class="formitemtitle Pw_198">市场价：</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />元
                <p id="ctl00_contentHolder_txtMarketPriceTip">本站会员所看到的商品市场价</p>
	        </li>
            <li id="skuRow" >
                <span class="formitemtitle Pw_198">规格：</span>
               <span class="content "> <Hi:ProductSKUDisplay CssClass="colorQ Pg_20" HeadRowClass="table_title" HeadColumnClass="td_right td_left" ColumnClass="td_right td_left" runat="server"  /></span>
                <Hi:TrimTextBox runat="server" ID="txtSkuPrice" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
            </li>
            <li style="clear:both" class="clearfix"><span class="formitemtitle Pw_198">商家编码：</span>
                <asp:Literal runat="server" ID="litProductCode" />
            </li>
            <li><span class="formitemtitle Pw_198">计量单位：</span><asp:Literal runat="server" ID="litUnit" /></li>
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
                <p class="Pa_198 clearfix"m_none>图片应小于120k，jpg或gif格式。建议为500x500像素</p>
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_198">商品简介：</span>
                <Hi:TrimTextBox runat="server" Rows="6" Columns="76" ID="txtShortDescription" TextMode="MultiLine" />
                <p class="Pa_198">限定在300个字符以内</p>
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_198">商品描述：</span>
                <span style="float:left;"><Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fckDescription" runat="server" Width="633px" Height="300px"/></span>
                <p style="clear:both;" class="Pa_198"></p>
            </li>
            
	        <h2 class="colorE">2. 相关设置</h2>
	        <li>
			  <span class="formitemtitle Pw_198">商品销售状态：</span>
				 <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus"  Text="出售中"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus"  Text="下架区"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"  Text="仓库中"></asp:RadioButton>
 			</li>
 			 <li id="li_tags" runat="server">
	        <span class="formitemtitle Pw_198">商品标签定义：</span>
			   
			   <div id="div_tags"> <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral></div>
			     <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox> 
            </li>
	        <li>
	          <h2 class="colorE">3. 搜索优化</h2>
	        </li>
	        <li> <span class="formitemtitle Pw_198">详细页标题：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtTitle" />
              <p id="ctl00_contentHolder_txtTitleTip">详细页标题限定在100个字符</p>
            </li>
	        <li> <span class="formitemtitle Pw_198">详细页描述：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaDescription" />
              <p id="ctl00_contentHolder_txtMetaDescriptionTip">详细页描述限定在260个字符</p>
            </li>
	        <li> <span class="formitemtitle Pw_198">详细页搜索关键字：</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaKeywords" />
              <p id="ctl00_contentHolder_txtMetaKeywordsTip">详细页搜索关键字限定在160个字符</p>
	        </li>
	      </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnUpdate" Text="保 存" OnClientClick="return LoadSkuPrice();"  CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
      <script type="text/javascript" src="producttag.helper.js"></script>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 30, false, null, '店铺名称为必填项，长度限制在30字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtDisplaySequence', 1, 10, false, null, '请输入数字,排序不能为空，商品显示顺序，越大排在越前'))
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 100, true, null, '详细页标题限定在100个字符'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1, 260, true, null, '详细页描述限定在260个字符'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 1, 160, true, null, '详细页搜索关键字限定在160个字符'))
            if ($("#attributeContent").val() == 0) {
                
                document.getElementById("attributeRow").style.display = "none";
            }
            if ($("#skuContent").val() == 0) {
                document.getElementById("skuRow").style.display = "none";
            }
        }
        $(document).ready(function() { InitValidators(); });
        
        function LoadSkuPrice(){
            var skuPriceXml = "<xml><skuPrices>";
            var skuPriceItems = $(".skuPriceItem");
            
             $.each(skuPriceItems, function(i, att) {
                var skuId = $(att).attr("id");
                var price = $("#" + skuId).val();
                skuPriceXml += String.format("<item skuId=\"{0}\" price=\"{1}\" \/>", skuId, price);                
            });
            
            skuPriceXml += "<\/skuPrices><\/xml>";
            $("#ctl00_contentHolder_txtSkuPrice").val(skuPriceXml);
            return PageIsValid();
        }
    </script>

</asp:Content>

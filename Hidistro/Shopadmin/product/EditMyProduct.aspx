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
	    <h1>�༭��Ʒ</h1>
        <span>��Ʒ������Ϣ</span>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>
          <h2 class="colorE">1.��Ʒ������Ϣ</h2>
	        <li>
	            <span class="formitemtitle Pw_198">������Ʒ���ࣺ</span>
                <span class="colorE float fonts"><asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span>
                [<asp:HyperLink runat="server" ID="lnkEditCategory" CssClass="a" Text="�༭"></asp:HyperLink>]
            </li>
	        <li>
	            <span class="formitemtitle Pw_198">��Ʒ���ͣ�</span>
                <abbr class="formselect"><Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--��ѡ��--" /></abbr>
	            Ʒ�ƣ�<abbr class="formselect"><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandCategories" NullToDisplay="--��ѡ��--" /></abbr>
            </li>
	        <li class="m_none" id="attributeRow">        
	          <span class="formitemtitle Pw_198">��Ʒ���ԣ�</span>
               <Hi:ProductAttributeDisplay ID="ProductAttributeDisplay1" runat="server" />
            </li>
	        <li class=" clearfix"> <span class="formitemtitle Pw_198">��Ʒ���ƣ�<em >*</em></span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" MaxLength="30" Width="350px" />
              <p id="ctl00_contentHolder_txtProductNameTip">�޶���30���ַ�</p>
	        </li>
	        
	        
             <li> <span class="formitemtitle Pw_198">����<em >*</em></span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtDisplaySequence" />
              <p id="ctl00_contentHolder_txtDisplaySequenceTip">��Ʒ��ʾ˳��Խ������Խǰ,��������������Ʒ���������</p>
	        </li>  
	        
            <li> <span class="formitemtitle Pw_198">������Ʒ�ߣ�</span>
	              <abbr class="formselect">
	                <Hi:ProductLineDropDownList runat="server" ID="dropProductLines" NullToDisplay="--��ѡ��--" />
              </abbr>
	        </li>
            <li><span class="formitemtitle Pw_198">������������ۼۣ�</span>
                <asp:Literal runat="server"  ID="litLowestSalePrice" />
             </li>
             <li><span class="formitemtitle Pw_198">�г��ۣ�</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />Ԫ
                <p id="ctl00_contentHolder_txtMarketPriceTip">��վ��Ա����������Ʒ�г���</p>
	        </li>
            <li id="skuRow" >
                <span class="formitemtitle Pw_198">���</span>
               <span class="content "> <Hi:ProductSKUDisplay CssClass="colorQ Pg_20" HeadRowClass="table_title" HeadColumnClass="td_right td_left" ColumnClass="td_right td_left" runat="server"  /></span>
                <Hi:TrimTextBox runat="server" ID="txtSkuPrice" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
            </li>
            <li style="clear:both" class="clearfix"><span class="formitemtitle Pw_198">�̼ұ��룺</span>
                <asp:Literal runat="server" ID="litProductCode" />
            </li>
            <li><span class="formitemtitle Pw_198">������λ��</span><asp:Literal runat="server" ID="litUnit" /></li>
            <li><span class="formitemtitle Pw_198">��ƷͼƬ��</span>
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
                <p class="Pa_198 clearfix"m_none>ͼƬӦС��120k��jpg��gif��ʽ������Ϊ500x500����</p>
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_198">��Ʒ��飺</span>
                <Hi:TrimTextBox runat="server" Rows="6" Columns="76" ID="txtShortDescription" TextMode="MultiLine" />
                <p class="Pa_198">�޶���300���ַ�����</p>
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_198">��Ʒ������</span>
                <span style="float:left;"><Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fckDescription" runat="server" Width="633px" Height="300px"/></span>
                <p style="clear:both;" class="Pa_198"></p>
            </li>
            
	        <h2 class="colorE">2. �������</h2>
	        <li>
			  <span class="formitemtitle Pw_198">��Ʒ����״̬��</span>
				 <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus"  Text="������"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus"  Text="�¼���"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"  Text="�ֿ���"></asp:RadioButton>
 			</li>
 			 <li id="li_tags" runat="server">
	        <span class="formitemtitle Pw_198">��Ʒ��ǩ���壺</span>
			   
			   <div id="div_tags"> <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral></div>
			     <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox> 
            </li>
	        <li>
	          <h2 class="colorE">3. �����Ż�</h2>
	        </li>
	        <li> <span class="formitemtitle Pw_198">��ϸҳ���⣺</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtTitle" />
              <p id="ctl00_contentHolder_txtTitleTip">��ϸҳ�����޶���100���ַ�</p>
            </li>
	        <li> <span class="formitemtitle Pw_198">��ϸҳ������</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaDescription" />
              <p id="ctl00_contentHolder_txtMetaDescriptionTip">��ϸҳ�����޶���260���ַ�</p>
            </li>
	        <li> <span class="formitemtitle Pw_198">��ϸҳ�����ؼ��֣�</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaKeywords" />
              <p id="ctl00_contentHolder_txtMetaKeywordsTip">��ϸҳ�����ؼ����޶���160���ַ�</p>
	        </li>
	      </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnUpdate" Text="�� ��" OnClientClick="return LoadSkuPrice();"  CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
      <script type="text/javascript" src="producttag.helper.js"></script>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 30, false, null, '��������Ϊ���������������30�ַ�����'))
            initValid(new InputValidator('ctl00_contentHolder_txtDisplaySequence', 1, 10, false, null, '����������,������Ϊ�գ���Ʒ��ʾ˳��Խ������Խǰ'))
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 100, true, null, '��ϸҳ�����޶���100���ַ�'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1, 260, true, null, '��ϸҳ�����޶���260���ַ�'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 1, 160, true, null, '��ϸҳ�����ؼ����޶���160���ַ�'))
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

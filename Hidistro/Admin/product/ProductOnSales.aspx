<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductOnSales.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductOnSales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth databody">
      <div class="title">
          <em><img src="../images/03.gif" width="32" height="32" /></em>
          <h1>��Ʒ����</h1>
          <span>���������е���Ʒ�������Զ���Ʒ����������Ҳ�ܶ���Ʒ���б༭���ϼܡ��¼ܡ���⡢�̻��������̻��Ȳ���</span>
      </div>
      <div class="datalist">
		<!--����-->
		<div class="searcharea clearfix" style="padding:10px 0px 3px 0px;">
			<ul>
				<li><span>��Ʒ���ƣ�</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--��ѡ����Ʒ����--" width="150" />
					</abbr>
				</li>
				<li style="margin-left:12px;">
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" runat="server" IsShowNoset="true" NullToDisplay="--��ѡ���Ʒ��--" width="153" />
					</abbr>
				</li>
			    <li>
					<abbr class="formselect">
						<Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--��ѡ��Ʒ��--"  width="153" ></Hi:BrandCategoriesDropDownList>
					</abbr>
				</li>
               <li><abbr class="formselect">
						<Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--��ѡ���ǩ--"   width="153"></Hi:ProductTagsDropDownList>
					</abbr></li>
               <li>
					<abbr class="formselect">
						<Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--��ѡ������--" width="153"/>
					</abbr>
				</li>
			</ul>
		</div>
		<div class="searcharea clearfix" style="padding:3px 0px 10px 0px;">
		    <ul>
		     <li><span>�̼ұ��룺</span><span> <asp:TextBox ID="txtSKU" Width="74" runat="server" CssClass="forminput" /></span></li>
		        <li><span>���ʱ�䣺</span></li>
		        <li>
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		            <span class="Pg_1010">��</span>
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </li>
		        <li>
                
                    <span class="formselect"><Hi:DistributorDropDownList runat="server" ID="dropDistributor"  width="153" nullToDisplay="--��ѡ�������--"/></span>
            
                </li>
                <li>
                    <asp:CheckBox runat="server" ID="chkIsAlert" Text="��汨��" />
                </li>
		        <li><asp:Button ID="btnSearch" runat="server" Text="��ѯ" CssClass="searchbutton"/></li>
		    </ul>
		</div>
		<!--����-->
        <div class="functionHandleArea clearfix">
			<!--��ҳ����-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
            </div>
			</div>
			<!--����-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">           
				<ul>
					<li class="batchHandleButton">
					    <span class="signicon"></span>
					    <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span>
					    <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">��ѡ</a></span>
                        <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="ɾ��" IsShow="true" DeleteMsg="ȷ��Ҫ����Ʒ�������վ����վ�������ƷҲ��ɾ����" /></span>
                        <span class="downproduct"><asp:HyperLink Target="_blank" runat="server" ID="btnDownTaobao" Text="�����Ա���Ʒ" /></span>
                        <select id="dropBatchOperation">
                            <option value="">��������..</option>
                            <option value="1">��Ʒ�ϼ�</option>
                            <option value="2">��Ʒ�¼�</option>
                            <option value="3">��Ʒ���</option>
                            <option value="4">��Ʒ�̻�</option>
                            <option value="5">��Ʒ�����̻�</option>
                            <option value="10">����������Ϣ</option>
                            <option value="11">������ʾ��������</option>
                            <option value="12">�������</option>
                            <option value="13">������Ա���ۼ�</option>
                            <option value="14">���������̲ɹ���</option>
                            <option value="15">������Ʒ������ǩ</option>
                            <option value="16">������Ʒ��Ʒ��</option>
                            
                    </select>  
                    </li>
				</ul>
			</div>
            <div class="filterClass"> 
                <span><b>����״̬��</b></span> 
                <span class="formselect"><Hi:SaleStatusDropDownList AutoPostBack="true" ID="dropSaleStatus"  runat="server" /></span> 
                <span style="margin-left:10px;"><b>�̻�״̬��</b></span> 
                <span class="formselect"><Hi:PenetrationStatusDropDownList AutoPostBack="true" ID="dropPenetrationStatus" runat="server" /></span> 
           </div>
		</div>		
		<!--�����б�����-->
	  
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc"  AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="ѡ��" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>                               
                        <asp:BoundField HeaderText="����" DataField="DisplaySequence" ItemStyle-Width="35px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="42%" HeaderText="��Ʒ" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">�̼ұ��룺<%# Eval("ProductCode") %> ��棺<%# Eval("Stock") %> �ɱ���<%# Eval("CostPrice", "{0:f2}")%> </span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                         <asp:TemplateField ItemStyle-Width="22%" HeaderText="��Ʒ�۸�" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>   
                                <span class="Name">һ�ڼۣ�<%# Eval("SalePrice", "{0:f2}")%>  �ɹ��ۣ�<%# Eval("PurchasePrice", "{0:f2}")%></span>
                                <span class="colorC">����������ۼۣ�<%# Eval("LowestSalePrice", "{0:f2}")%> 
                                    �г��ۣ�<asp:Literal ID="litMarketPrice" runat ="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                            </itemtemplate>
                        </asp:TemplateField>     
                        <asp:TemplateField HeaderText="��Ʒ״̬"  ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <span><asp:Literal ID="litSaleStatus" runat ="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal>/<asp:Literal ID="litPenetrationStatus" runat ="server" Text='<%#Eval("PenetrationStatus").ToString() =="1"?"���̻�":"δ�̻�"%>'></asp:Literal></span>
                            </itemtemplate>
                        </asp:TemplateField>             
                        <asp:TemplateField HeaderText="����" ItemStyle-Width="180px" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <span class="submit_bianji"><a href="<%#"EditProduct.aspx?productId="+Eval("ProductId")%>">�༭</a></span>
                                 <span class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:CollectionProduct('<%# "EditReleteProducts.aspx?productId="+Eval("ProductId")%>')">�����Ʒ</a></span>
			                  <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDel" CommandName="Delete" runat="server" Text="ɾ��" IsShow="true" DeleteMsg="ȷ��Ҫ����Ʒ�������վ����վ�������ƷҲ��ɾ����" /></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
		  <div class="blank12 clearfix"></div>
      </div>
        <div class="page">
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
				</div>
			</div>
		</div>
      </div>

	</div>
<%-- �ϼ���Ʒ--%>
<div  id="divOnSaleProduct" style="display: none;">
    <div class="frame-content">
    <p><em>ȷ��Ҫ�ϼ���Ʒ���ϼܺ���Ʒ��ǰ̨����</em></p>
    </div>
</div>

<%-- �¼���Ʒ--%>
<div  id="divUnSaleProduct" style="display: none;">
    <div class="frame-content">
    ͬʱ�����̻���<asp:CheckBox ID="chkDeleteImage" Text="�����̻�" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
    <p><em>��ѡ�����̻�ʱ��������վ���ڴ���Ʒ��Ϣ�Լ��������Ϣ������ɾ��</em></p>
    </div>
</div>

<%-- �����Ʒ--%>
<div id="divInStockProduct" style="display: none;">
    <div class="frame-content">
        ͬʱ�����̻���<asp:CheckBox ID="chkInstock" Text="�����̻�" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
        <p><em>��ѡ�����̻�ʱ��������վ���ڴ���Ʒ��Ϣ�Լ��������Ϣ������ɾ����</em></p>
    </div>
</div>

<%-- �̻� --%>
<div id="divPenetrationProduct" style="display: none;">
    <div class="frame-content">
        <p><em>ѡ����Ʒ�̻��󣬷����̽�����������Щ��Ʒ�������ۣ�ȷ���̻���</em></p>
    </div>
</div>

<%-- �����̻� --%>
<div id="divCancleProduct" style="display: none;">
    <div class="frame-content">
        <p><em>ѡ����Ʒ�����̻��󣬽�ɾ�������������ص���Щ��Ʒ��ȷ��������</em></p>
    </div>
</div>

<%-- ��Ʒ��ǩ--%>
<div id="divTagsProduct" style="display: none;">
    <div class="frame-content">
     <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
    </div>
</div>
<%-- ��Ʒ��--%>
<div id="divProductLine" style="display: none;">
    <div class="frame-content">
    <span class="formitemtitle Pw_198">�ƶ���Ʒ����Ʒ�ߣ�</span>
      <abbr class="formselect"><Hi:ProductLineDropDownList ID="dropProductLines" NullToDisplay="--��ѡ��--"  runat="server"   onchange="javascript:SetProductLine(this)" /></abbr>
    </div>
</div>

<div style="display:none">
<asp:Button ID="btnUpdateProductTags" runat="server" Text="������Ʒ��ǩ" CssClass="submit_DAqueding" />
<Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>
<asp:Button ID="btnInStock" runat="server" Text="�����Ʒ" CssClass="submit_DAqueding" />
<asp:Button ID="btnUnSale" runat="server" Text="�¼���Ʒ" CssClass="submit_DAqueding"/>
<asp:Button ID="btnUpSale" runat="server" Text="�ϼ���Ʒ" CssClass="submit_DAqueding"/>
<asp:Button ID="btnPenetration" runat="server" Text="�̻�" CssClass="submit_DAqueding"/>
<asp:Button ID="btnCancle" runat="server" Text="�����̻�" CssClass="submit_DAqueding"/>
<asp:Button ID="btnUpdateLine" runat="server" Text="������Ʒ��" CssClass="submit_DAqueding"/>
<input type="hidden" id="hdProductLine" value="0" runat="server" />
<input type="hidden" id="hdPenetrationStatus" value="1" runat="server" />
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="producttag.helper.js"></script>
<script type="text/javascript" >
    $(document).ready(function () {
        $("#dropBatchOperation").bind("change", function () { SelectOperation(); });
    });

    function SelectOperation() {
        var Operation = $("#dropBatchOperation").val();
        var productIds = GetProductId();
        if (productIds.length > 0) {
            switch (Operation) {
                case "1":
                    formtype = "onsale";
                    arrytext = null;
                    DialogShow("��Ʒ�ϼ�", "productonsale", "divOnSaleProduct", "ctl00_contentHolder_btnUpSale");
                    break;
                case "2":
                    formtype = "unsale";
                    arrytext = null;
                    DialogShow("��Ʒ�¼�", "productunsale", "divUnSaleProduct", "ctl00_contentHolder_btnUnSale");
                    break;
                case "3":
                    formtype = "instock";
                    arrytext = null;
                    DialogShow("��Ʒ���", "productinstock", "divInStockProduct", "ctl00_contentHolder_btnInStock");
                    break;
                case "4":
                    formtype = "penetration";
                    arrytext = null;
                    DialogShow("��Ʒ�̻�", "productPenetration", "divPenetrationProduct", "ctl00_contentHolder_btnPenetration");
                    break;
                case "5":
                    formtype = "cancle";
                    arrytext = null;
                    DialogShow("��Ʒ�����̻�", "productCancle", "divCancleProduct", "ctl00_contentHolder_btnCancle");
                    break;
                case "10":
                    DialogFrame("product/EditBaseInfo.aspx?ProductIds=" + productIds, "������Ʒ������Ϣ", null, null);
                    break;
                case "11":
                    DialogFrame("product/EditSaleCounts.aspx?ProductIds=" + productIds, "����ǰ̨��ʾ����������", null, null);
                    break;
                case "12":
                    DialogFrame("product/EditStocks.aspx?ProductIds=" + productIds, "�������", 880, null);
                    break;
                case "13":
                    DialogFrame("product/EditMemberPrices.aspx?ProductIds=" + productIds, "������Ա���ۼ�", 1000, null);
                    break;
                case "14":
                    DialogFrame("product/EditDistributorPrices.aspx?ProductIds=" + productIds, "���������̲ɹ���", 1000, null);
                    break;
                case "15":
                    formtype = "tag";
                    setArryText('ctl00_contentHolder_txtProductTag', "");
                    DialogShow("������Ʒ��ǩ", "producttag", "divTagsProduct", "ctl00_contentHolder_btnUpdateProductTags");
                    break;
                case "16":
                    formtype = "line";
                    arrytext = null;
                    DialogShow("���ò�Ʒ��", "productline", "divProductLine", "ctl00_contentHolder_btnUpdateLine");
                    break;
            }
        }
        $("#dropBatchOperation").val("");
    }
            
    function GetProductId(){
        var v_str = "";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
            alert("��ѡ����Ʒ");
            return "";
        }
        return v_str.substring(0, v_str.length - 1);
    }

    function SetPenetrationStatus(checkobj) {
        if (checkobj.checked) {
            $("#ctl00_contentHolder_hdPenetrationStatus").val("1");
        } else {
            $("#ctl00_contentHolder_hdPenetrationStatus").val("0");
        }
    }

    function SetProductLine(dropobj) {
        $("#ctl00_contentHolder_hdProductLine").val($(dropobj).val());
    }
    
    function CollectionProduct(url) {
        DialogFrame("product/"+url, "�����Ʒ");
    }
    
    function validatorForm() {
        switch (formtype) {
            case "tag":
                if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                    alert("��ѡ����Ʒ��ǩ");
                    return false;
                }
                break;
            case "onsale":
                setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                break;
            case "unsale":
                setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                break;
            case "instock":
                setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                break;
            case "penetration":
                setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                break;
            case "cancle":
                setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                break;
            case "line":
                setArryText('ctl00_contentHolder_hdProductLine', $("#ctl00_contentHolder_hdProductLine").val());
                break;
        };
        return true;
    }

</script>
</asp:Content>
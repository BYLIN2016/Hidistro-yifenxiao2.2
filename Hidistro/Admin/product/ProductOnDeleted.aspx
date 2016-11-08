<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductOnDeleted.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductOnDeleted" %>
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
          <h1>商品回收站</h1>
          <span>商品回收站，您可以对回收站中的商品彻底删除，也可以对商品还原到出售中、下架区及仓库中</span>
      </div>
     <div class="datalist">
		<!--搜索-->
		<div class="searcharea clearfix" style="padding:10px 0px 3px 0px;">
			<ul>
				<li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" width="150"/>
					</abbr>
				</li>
				<li style="margin-left:12px;">
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" runat="server" NullToDisplay="--请选择产品线--" width="153"/>
					</abbr>
				</li>
				   <li>
					<abbr class="formselect">
						<Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" width="153"></Hi:BrandCategoriesDropDownList>
					</abbr>
				</li>                
				</ul>
		</div>
		<div class="searcharea clearfix" style="padding:3px 0px 10px 0px;">
		    <ul>
                <li><span>商家编码：</span><span> <asp:TextBox ID="txtSKU" Width="74" runat="server" CssClass="forminput" /></span></li>
		        <li><span>添加时间：</span></li>
		        <li>
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		            <span class="Pg_1010">至</span>
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </li>
		        <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
		    </ul>
		</div>
		<!--结束-->
         <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
            </div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					    <span class="signicon"></span>
					    <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					    <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                        <span class="deleteall"><a href="javascript:void(0)" onclick="deleteProducts();">彻底删除</a></span>
                        <span class="downproduct"><asp:LinkButton runat="server" ID="btnUpShelf" Text="还原到出售中"  /></span>    
                        <span class="downproduct"><asp:LinkButton runat="server" ID="btnOffShelf" Text="还原到下架区"  /></span>    
                        <span class="downproduct"><asp:LinkButton runat="server" ID="btnInStock" Text="还原到仓库里"  /></span>               
                    </li>
				</ul>
			</div>
		</div>		
		<!--数据列表区域-->
	 
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc"  AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>                               
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="40px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="35%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="库存"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>
                        <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="80"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left"  />
                        <Hi:MoneyColumnForAdmin HeaderText="成本价" ItemStyle-Width="80"  DataField="CostPrice" HeaderStyle-CssClass="td_right td_left"  />        
                        <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="80"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />   
                        <Hi:MoneyColumnForAdmin HeaderText="采购价" ItemStyle-Width="80"  DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />                        
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="12%" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
			                  <span class="submit_shanchu"><a href="javascript:void(0)" onclick="deleteProduct('<%# Eval("ProductId") %>');">彻底删除</a></span>
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

<%--彻底删除--%>
<div id="deleteProduct" style="display: none;">
    <div class="frame-content">
        是否删除图片：<asp:CheckBox ID="chkDeleteImage" Text="删除图片" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
    </div>
</div>

<div style="display:none">
<asp:Button ID="btnOK" runat="server" Text="彻底删除商品" CssClass="submit_DAqueding" />
 <input type="hidden" id="hdPenetrationStatus" value="1" runat="server" />
<input runat="server" type="hidden" id="currentProductId" />
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function deleteProduct(productId) {
        var v_str = "";
        arrytext = null;
        $("#ctl00_contentHolder_currentProductId").val(productId);
        DialogShow("彻底删除商品", "productdel", "deleteProduct", "ctl00_contentHolder_btnOK");
    }


    function deleteProducts(){
        var v_str = "";
        arrytext = null;
        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
            alert("请选择商品");
            return false;
        }
        $("#ctl00_contentHolder_currentProductId").val(v_str.substring(0, v_str.length - 1));
        DialogShow("彻底删除商品", "productdel", "deleteProduct", "ctl00_contentHolder_btnOK");
    }


    function validatorForm() {
        if ($("#ctl00_contentHolder_currentProductId").val().length <= 0) {
            alert("请选择要删除的商品");
            return false;
        }
        setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
        return true;
    }

    function SetPenetrationStatus(checkobj) {
        if (checkobj.checked) {
            $("#ctl00_contentHolder_hdPenetrationStatus").val("1");
        } else {
            $("#ctl00_contentHolder_hdPenetrationStatus").val("0");
        }
    }
</script>
</asp:Content>
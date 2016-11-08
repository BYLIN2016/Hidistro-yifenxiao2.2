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
          <h1>商品管理</h1>
          <span>店铺中所有的商品，您可以对商品进行搜索，也能对商品进行编辑、上架、下架、入库、铺货、撤销铺货等操作</span>
      </div>
      <div class="datalist">
		<!--搜索-->
		<div class="searcharea clearfix" style="padding:10px 0px 3px 0px;">
			<ul>
				<li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" width="150" />
					</abbr>
				</li>
				<li style="margin-left:12px;">
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" runat="server" IsShowNoset="true" NullToDisplay="--请选择产品线--" width="153" />
					</abbr>
				</li>
			    <li>
					<abbr class="formselect">
						<Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择品牌--"  width="153" ></Hi:BrandCategoriesDropDownList>
					</abbr>
				</li>
               <li><abbr class="formselect">
						<Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--请选择标签--"   width="153"></Hi:ProductTagsDropDownList>
					</abbr></li>
               <li>
					<abbr class="formselect">
						<Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--请选择类型--" width="153"/>
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
		        <li>
                
                    <span class="formselect"><Hi:DistributorDropDownList runat="server" ID="dropDistributor"  width="153" nullToDisplay="--请选择分销商--"/></span>
            
                </li>
                <li>
                    <asp:CheckBox runat="server" ID="chkIsAlert" Text="库存报警" />
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
                        <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？子站的相关商品也将删除！" /></span>
                        <span class="downproduct"><asp:HyperLink Target="_blank" runat="server" ID="btnDownTaobao" Text="下载淘宝商品" /></span>
                        <select id="dropBatchOperation">
                            <option value="">批量操作..</option>
                            <option value="1">商品上架</option>
                            <option value="2">商品下架</option>
                            <option value="3">商品入库</option>
                            <option value="4">商品铺货</option>
                            <option value="5">商品撤销铺货</option>
                            <option value="10">调整基本信息</option>
                            <option value="11">调整显示销售数量</option>
                            <option value="12">调整库存</option>
                            <option value="13">调整会员零售价</option>
                            <option value="14">调整分销商采购价</option>
                            <option value="15">调整商品关联标签</option>
                            <option value="16">调整商品产品线</option>
                            
                    </select>  
                    </li>
				</ul>
			</div>
            <div class="filterClass"> 
                <span><b>出售状态：</b></span> 
                <span class="formselect"><Hi:SaleStatusDropDownList AutoPostBack="true" ID="dropSaleStatus"  runat="server" /></span> 
                <span style="margin-left:10px;"><b>铺货状态：</b></span> 
                <span class="formselect"><Hi:PenetrationStatusDropDownList AutoPostBack="true" ID="dropPenetrationStatus" runat="server" /></span> 
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
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="35px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="42%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %> 库存：<%# Eval("Stock") %> 成本：<%# Eval("CostPrice", "{0:f2}")%> </span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                         <asp:TemplateField ItemStyle-Width="22%" HeaderText="商品价格" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>   
                                <span class="Name">一口价：<%# Eval("SalePrice", "{0:f2}")%>  采购价：<%# Eval("PurchasePrice", "{0:f2}")%></span>
                                <span class="colorC">分销最低零售价：<%# Eval("LowestSalePrice", "{0:f2}")%> 
                                    市场价：<asp:Literal ID="litMarketPrice" runat ="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                            </itemtemplate>
                        </asp:TemplateField>     
                        <asp:TemplateField HeaderText="商品状态"  ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <span><asp:Literal ID="litSaleStatus" runat ="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal>/<asp:Literal ID="litPenetrationStatus" runat ="server" Text='<%#Eval("PenetrationStatus").ToString() =="1"?"已铺货":"未铺货"%>'></asp:Literal></span>
                            </itemtemplate>
                        </asp:TemplateField>             
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="180px" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <span class="submit_bianji"><a href="<%#"EditProduct.aspx?productId="+Eval("ProductId")%>">编辑</a></span>
                                 <span class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:CollectionProduct('<%# "EditReleteProducts.aspx?productId="+Eval("ProductId")%>')">相关商品</a></span>
			                  <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDel" CommandName="Delete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？子站的相关商品也将删除！" /></span>
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
<%-- 上架商品--%>
<div  id="divOnSaleProduct" style="display: none;">
    <div class="frame-content">
    <p><em>确定要上架商品？上架后商品将前台出售</em></p>
    </div>
</div>

<%-- 下架商品--%>
<div  id="divUnSaleProduct" style="display: none;">
    <div class="frame-content">
    同时撤销铺货：<asp:CheckBox ID="chkDeleteImage" Text="撤销铺货" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
    <p><em>当选择撤销铺货时，所有子站关于此商品信息以及促销活动信息都将被删除</em></p>
    </div>
</div>

<%-- 入库商品--%>
<div id="divInStockProduct" style="display: none;">
    <div class="frame-content">
        同时撤销铺货：<asp:CheckBox ID="chkInstock" Text="撤销铺货" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
        <p><em>当选择撤销铺货时，所有子站关于此商品信息以及促销活动信息都将被删除。</em></p>
    </div>
</div>

<%-- 铺货 --%>
<div id="divPenetrationProduct" style="display: none;">
    <div class="frame-content">
        <p><em>选中商品铺货后，分销商将可以下载这些商品进行销售，确定铺货吗？</em></p>
    </div>
</div>

<%-- 撤销铺货 --%>
<div id="divCancleProduct" style="display: none;">
    <div class="frame-content">
        <p><em>选中商品撤销铺货后，将删除分销商已下载的这些商品，确定撤销吗？</em></p>
    </div>
</div>

<%-- 商品标签--%>
<div id="divTagsProduct" style="display: none;">
    <div class="frame-content">
     <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
    </div>
</div>
<%-- 产品线--%>
<div id="divProductLine" style="display: none;">
    <div class="frame-content">
    <span class="formitemtitle Pw_198">移动商品到产品线：</span>
      <abbr class="formselect"><Hi:ProductLineDropDownList ID="dropProductLines" NullToDisplay="--请选择--"  runat="server"   onchange="javascript:SetProductLine(this)" /></abbr>
    </div>
</div>

<div style="display:none">
<asp:Button ID="btnUpdateProductTags" runat="server" Text="调整商品标签" CssClass="submit_DAqueding" />
<Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>
<asp:Button ID="btnInStock" runat="server" Text="入库商品" CssClass="submit_DAqueding" />
<asp:Button ID="btnUnSale" runat="server" Text="下架商品" CssClass="submit_DAqueding"/>
<asp:Button ID="btnUpSale" runat="server" Text="上架商品" CssClass="submit_DAqueding"/>
<asp:Button ID="btnPenetration" runat="server" Text="铺货" CssClass="submit_DAqueding"/>
<asp:Button ID="btnCancle" runat="server" Text="撤销铺货" CssClass="submit_DAqueding"/>
<asp:Button ID="btnUpdateLine" runat="server" Text="调整产品线" CssClass="submit_DAqueding"/>
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
                    DialogShow("商品上架", "productonsale", "divOnSaleProduct", "ctl00_contentHolder_btnUpSale");
                    break;
                case "2":
                    formtype = "unsale";
                    arrytext = null;
                    DialogShow("商品下架", "productunsale", "divUnSaleProduct", "ctl00_contentHolder_btnUnSale");
                    break;
                case "3":
                    formtype = "instock";
                    arrytext = null;
                    DialogShow("商品入库", "productinstock", "divInStockProduct", "ctl00_contentHolder_btnInStock");
                    break;
                case "4":
                    formtype = "penetration";
                    arrytext = null;
                    DialogShow("商品铺货", "productPenetration", "divPenetrationProduct", "ctl00_contentHolder_btnPenetration");
                    break;
                case "5":
                    formtype = "cancle";
                    arrytext = null;
                    DialogShow("商品撤销铺货", "productCancle", "divCancleProduct", "ctl00_contentHolder_btnCancle");
                    break;
                case "10":
                    DialogFrame("product/EditBaseInfo.aspx?ProductIds=" + productIds, "调整商品基本信息", null, null);
                    break;
                case "11":
                    DialogFrame("product/EditSaleCounts.aspx?ProductIds=" + productIds, "调整前台显示的销售数量", null, null);
                    break;
                case "12":
                    DialogFrame("product/EditStocks.aspx?ProductIds=" + productIds, "调整库存", 880, null);
                    break;
                case "13":
                    DialogFrame("product/EditMemberPrices.aspx?ProductIds=" + productIds, "调整会员零售价", 1000, null);
                    break;
                case "14":
                    DialogFrame("product/EditDistributorPrices.aspx?ProductIds=" + productIds, "调整分销商采购价", 1000, null);
                    break;
                case "15":
                    formtype = "tag";
                    setArryText('ctl00_contentHolder_txtProductTag', "");
                    DialogShow("设置商品标签", "producttag", "divTagsProduct", "ctl00_contentHolder_btnUpdateProductTags");
                    break;
                case "16":
                    formtype = "line";
                    arrytext = null;
                    DialogShow("设置产品线", "productline", "divProductLine", "ctl00_contentHolder_btnUpdateLine");
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
            alert("请选择商品");
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
        DialogFrame("product/"+url, "相关商品");
    }
    
    function validatorForm() {
        switch (formtype) {
            case "tag":
                if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                    alert("请选择商品标签");
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
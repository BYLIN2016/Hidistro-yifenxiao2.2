<%@ Page Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true" CodeBehind="MyProductOnSales.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyProductOnSales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth td_top_ccc">
      <div class="toptitle">
          <em><img src="../images/03.gif" width="32" height="32" /></em>
          <h1 class="title_height"><strong>我的分销商品</strong></h1>
          <span class="title_height">店铺中所有的商品，您可以对商品进行搜索，也能对商品进行编辑、上架、下架、入库等操作</span> 
        </div>
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" />
					</abbr>
				</li>
				 <li><abbr class="formselect">
						<Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--请选择商品标签--" CssClass="forminput"></Hi:ProductTagsDropDownList>
					</abbr></li>
                <li><span>商家编码：</span><span> <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></span></li>
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
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true"  /></span>
                    <span class="submit_btnshangjia"><asp:LinkButton runat="server" ID="btnUpShelf" Text="上架" /></span>
                    <span class="submit_btnxiajia"><asp:LinkButton runat="server" ID="btnOffShelf" Text="下架"  /></span>
                    <span class="submit_btnxiajia"><asp:LinkButton runat="server" ID="btnInStock" Text="入库"  /></span>
                    <span class=""><a href="javascript:void(0)" onclick="EditSaleCounts()">调整显示销售数量</a></span>  
                    <span class=""><a href="javascript:void(0)" onclick="EditMemberPrices()">调整会员零售价</a></span>  
                    <span class=""><a href="javascript:void(0)" onclick="ShowEditDiv()">修改商品名称</a></span>  
                     <span class=""><a href="javascript:void(0)" onclick="EditProdcutTag()">调整商品关联标签</a></span>  
                    </li>
				</ul>
			</div>
             <div class="filterClass"> 
                <span><b>状态：</b></span> 
                <span class="formselect"><Hi:SaleStatusDropDownList AutoPostBack="true" ID="dropSaleStatus"  runat="server" /></span> 
           </div>
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="false" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc" SortOrderBy="DisplaySequence" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                     <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>    
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="45px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="45%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <Hi:DistributorProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                <Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                </Hi:DistributorProductDetailsLink>
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name">  <Hi:DistributorProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode")%> 库存：<%# Eval("Stock")%> 最低零售价：<%#Eval("LowestSalePrice", "{0:F2}")%></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                         
                         <Hi:MoneyColumnForAdmin HeaderText=" 一口价" ItemStyle-Width="60" DataField="SalePrice"  HeaderStyle-CssClass="td_right td_left"  />
                        <Hi:MoneyColumnForAdmin HeaderText="采购价" ItemStyle-Width="60" DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />                    
                         <asp:TemplateField HeaderText="差价" ItemStyle-Width="60" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>                           
                             <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Convert.ToDecimal(Eval("SalePrice")) -  Convert.ToDecimal(Eval("PurchasePrice"))%>' runat="server"></Hi:FormatedMoneyLabel>
                          </itemtemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderText="状态"  ItemStyle-Width="60" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <span><asp:Literal ID="litSaleStatus" runat ="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal></span>
                            </itemtemplate>
                        </asp:TemplateField>                                                                                         
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="18%" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <span class="submit_bianji"><a target="_blank" href='<%#"EditMyProduct.aspx?productId="+Eval("ProductId")%>'>编辑</a></span>
                                <span class="submit_bianji"><a href="<%#"EditMyReleteProducts.aspx?productId="+Eval("ProductId")%>" target="_blank">相关商品</a></span>
			                  <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="删除" IsShow="true"  /></span>
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
	
	<div class="Pop_up" id="EditProductNames"  style=" display:none;">
      <h1>批量修改商品名称 </h1>
      <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
      <div class="mianform validator2">
        <ul>
            <li> 
                增加前缀 <asp:TextBox ID="txtPrefix" runat="server" Width="80px"  MaxLength="20"/> 
                增加后缀 <asp:TextBox ID="txtSuffix" runat="server" Width="80px"  MaxLength="20"/>
                <asp:Button ID="btnAddOK" runat="server" Text="确定"  CssClass="searchbutton"/> 
            </li>
            <li> 
                查找字符串 <asp:TextBox ID="txtOleWord" runat="server" Width="80px" /> 替换成 <asp:TextBox ID="txtNewWord" runat="server" Width="80px" />
                <asp:Button ID="btnReplaceOK" runat="server" Text="确定"  CssClass="searchbutton"/> 
            </li>
        </ul>
      </div>
</div>
<div class="Pop_up" id="TagsProduct" style="display: none;">
    <h1>
        商品标签
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform" style="text-align:center;">
     <div id="div_tags"> <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral></div>
			     <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox> 
    <p> <asp:Button ID="btnUpdateProductTags" runat="server" Text="确定" CssClass="submit_DAqueding" /></p>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
      <script type="text/javascript" src="producttag.helper.js"></script>
<script type="text/javascript" >
    function EditSaleCounts() {
        var productIds = GetProductId();
        if (productIds.length > 0)
            window.open("EditMySaleCounts.aspx?ProductIds=" + productIds);
    } 
      
    function EditMemberPrices(){
        var productIds = GetProductId();
        if(productIds.length > 0)
            window.open("EditMyMemberPrices.aspx?ProductIds=" + productIds);
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
    
    function ShowEditDiv(id, categoryId, keywords) {
        var productIds = GetProductId();
        if(productIds.length > 0)
            DivWindowOpen(550, 180, 'EditProductNames');

    }
       function EditProdcutTag(){
        DivWindowOpen(400,200,'TagsProduct');
    }
</script>
</asp:Content>
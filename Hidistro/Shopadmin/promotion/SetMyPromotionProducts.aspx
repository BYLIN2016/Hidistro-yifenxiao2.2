<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SetMyPromotionProducts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SetMyPromotionProducts" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<script type="text/javascript">
    function checkAll() {
        var isCheck = $("#chkCheckAll").attr("checked");

        $("input[type='checkbox'][name='chkSerachResult']").attr("checked", isCheck);
    }

    function ShowAddDiv() {
        var activId = $("#ctl00_contentHolder_hdactivy").val().replace(/\s/g, "");
        if (activId != "" && parseInt(activId) > 0) {
            DialogFrame("SearchPromotionProduct.aspx?activityId=" + activId, "添加促销商品", 1020, null);
        }
//        $("#serachResult").empty();
//        $("#prevLink").css("display", "none");
//        $("#nextLink").css("display", "none");
//        $("#pages").text("");
//        DivWindowOpen(750, 580, 'searchwindow');
//        GetSearchData(1);
 
    }

    function nextPage() {
        var index = $("#currentPage").val()
        index = Number(index) + 1;
        if (index <= 0)
            return;
        $("#currentPage").val(index);
        GetSearchData(index);
    }

    function prevPage() {
        var index = $("#currentPage").val()
        index -= 1;
        if (index <= 0) {
            return;
        }
        $("#currentPage").val(index);
        GetSearchData(index);
    }

    function search() {
        $("#currentPage").val(1);
        GetSearchData(1);
    }

    function GetSearchData(pageindex) {
        var searchname = $("#serachName").val();
        var categoryId=$("#ctl00_contentHolder_dropCategories").val();
        var brandId = $("#ctl00_contentHolder_dropBrandList").val();
        var ajaxurl = "SetMyPromotionProducts.aspx?isCallback=true&serachName=" + searchname + "&categoryId="+categoryId+"&brandId="+brandId+"&page="+pageindex+"&date="+new Date();
        $.ajax({
            url: ajaxurl,
            type: 'GET',
            dataType:'json',
            success: function (datasource) {
                var currentPage = $("#currentPage").val();
                var pageCount = Math.ceil(json.recCount / 15);
                if (currentPage >= pageCount)
                    $("#nextLink").css("display", "none");
                else
                    $("#nextLink").css("display", "");
                if (currentPage <= 1)
                    $("#prevLink").css("display", "none");
                else
                    $("#prevLink").css("display", "");
                $("#sp_pagetotal").text(pageCount);
                $("#sp_pageindex").text(pageindex);

                $("#serachResult").empty();
                ;
                $("#serachResult").append("<tr class=\"datagrid-header\"><td><input id='chkCheckAll' onclick='checkAll()' type='checkbox' />商品名称</td><td>价格</td><td>库存</td></tr>")
                $.each(datasource.data, function (i, item) {
                    if (item != undefined && item.sku != "") {
                        var str = String.format("<tr class=\"datagrid-body\"><td align='left'><input type='checkbox'  name='chkSerachResult' value='{0}' />{1}</td>", item.ProductId, item.Name);
                        str += String.format("<td>{0}</td>", item.Price);
                        str += String.format("<td>{0}</td></tr>", item.Stock);
                        $("#serachResult").append(str);
                    }
                });
            },
            error: function (xm, msg) {
               alert(msg);
            }
        });
    }
</script>
<div class="dataarea mainwidth databody">
    <input type="hidden" id="hdactivy" runat="server" />
    <div class="title td_bottom m_none">
          <em><img src="../images/03.gif" width="32" height="32" /></em>
           <h1>促销活动“<asp:Literal runat="server" ID="litPromotionName" />“包括的商品 </h1>
	        <span>每件商品只能参加其中一个促销活动，如果一件商品在别的促销活动中已经被选，这些将不再参与</span>
      </div>
  
		    <!-- 添加按钮-->
    <div class="btn">
		      <div style="float:right; margin-right:15px;" class="delete"><Hi:ImageLinkButton ID="btnDeleteAll"  runat="server" Text="清空" IsShow="true" DeleteMsg="确定要清空这些促销商品吗？" /></div>
              <a href="javascript:ShowAddDiv();" class="submit_jia">添加促销商品</a>
	        </div>
	    <!--结束-->
		    <!--数据列表区域-->
    <div class="datalist">
	        <UI:Grid ID="grdPromotionProducts" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ProductId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                <Columns> 
                        <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                            <ItemTemplate>
		                         <div style="float:left; margin-right:10px;">
                                    <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                            <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                     </a> 
                                     </div>
                                     <div style="float:left;">
                                     <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                      <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                     </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="库存"  ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left">
                                <itemtemplate>
                                 <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                              </itemtemplate>
                        </asp:TemplateField>
                        <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="15%"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left"  />       
                        <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="15%"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />        
                         <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" HeaderStyle-Width="15%">
                            <ItemStyle />
                            <itemtemplate> 
			                    <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" IsShow="true" CommandName="Delete" ></Hi:ImageLinkButton></span>
                            </itemtemplate>
                        </asp:TemplateField>
                </Columns>
            </UI:Grid>
      </div>

    <div style="padding:10px 0px 10px 15px">
              <div style="margin:0 auto;width:200px;"><asp:Button ID="btnFinesh" runat="server" Text="完成" CssClass="submit_DAqueding inbnt" /></div>
        </div>	
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

   
</asp:Content>

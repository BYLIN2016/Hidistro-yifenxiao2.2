<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductSaleStatistics" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->
	<div class="dataarea mainwidth databody">
<div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>商品访问与购买比</h1>
  <span>查询商品的访问次数和购买次数比，默认排序访问购买率从高到低(注：统计的商品不包括成功退款订单中的商品)</span>
</div>

      <div>
        <!--数据列表区域-->
	  <div class="datalist">
      		<!-
		<!--搜索-->
		<!--结束-->
      <div class="functionHandleArea clearfix m_none">
			<!--分页功能-->
		<div class="pageHandleArea">
				<ul>
				   <input type="hidden" runat="server" id="hidPageSize" />
				   <input type="hidden" runat="server" id="hidPageIndex" />
					<li class="paginalNum"><span>排行：</span><a id="top5" href="ProductSaleStatistics.aspx?pageSize=5&pageindex=1">前5位</a><a href="ProductSaleStatistics.aspx?pageSize=10&pageindex=1" id="top10" >前10位</a><a id="top15" href="ProductSaleStatistics.aspx?pageSize=15&pageindex=1">前15位</a></li>
				</ul>
		  </div>
		  <!--结束-->
		</div>
	   <UI:Grid ID="grdProductSaleStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                    <asp:TemplateField HeaderText="排行" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("IndexId"))==1?"../images/0001.gif":Convert.ToInt32(Eval("IndexId"))==2?"../images/0002.gif":"../images/0003.gif" %>'
                                     Visible='<%#Convert.ToInt32(Eval("IndexId"))<4 %>' /> <strong><%#Eval("IndexId")%></strong>
                                </ItemTemplate>
                            </asp:TemplateField>  
                        <asp:BoundField HeaderText="商品名称" DataField="ProductName" HeaderStyle-CssClass="td_right td_left"/>    
                        <asp:BoundField HeaderText="访问次数" DataField="VistiCounts" HeaderStyle-CssClass="td_right td_left" />
                         <asp:TemplateField HeaderText="购买次数" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <asp:Label ID="lblBuyCount"  runat="Server" Text='<%# Eval("BuyCount") %>' />
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="访问购买率"  HeaderStyle-CssClass="td_right td_right_fff">
                            <ItemTemplate>
                                <asp:Literal ID="lblProductSalePercentage" Text='<%# Convert.ToDecimal(Eval("BuyPercentage")).ToString("F") %>' runat="Server"/>%
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
	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitPage() {
        if ($("#ctl00_contentHolder_hidPageSize").val() == "5" && $("#ctl00_contentHolder_hidPageIndex").val() == "1") {
            document.getElementById("top5").className = 'selectthis';
        }
        if ($("#ctl00_contentHolder_hidPageSize").val() == "10" && $("#ctl00_contentHolder_hidPageIndex").val() == "1") {
            document.getElementById("top10").className = 'selectthis';
        }
        if ($("#ctl00_contentHolder_hidPageSize").val() == "15" && $("#ctl00_contentHolder_hidPageIndex").val() == "1") {
            document.getElementById("top15").className = 'selectthis';
        }
        if ($("#ctl00_contentHolder_hidPageSize").val() == "" && $("#ctl00_contentHolder_hidPageIndex").val() == "") {
            document.getElementById("top10").className = 'selectthis';
        }
    }
    $(document).ready(function() { InitPage(); });
</script>

</asp:Content>


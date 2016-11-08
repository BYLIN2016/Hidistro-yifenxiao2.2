<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ProductSaleStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ProductSaleStatistics"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
			<li class="optionstar"><a href="../sales/SaleReport.aspx"><span>生意报告</span></a></li>
			<li><a href="../sales/OrderStatistics.aspx"><span>订单统计</span></a></li>
            <li><a href="../sales/SaleDetails.aspx"><span>销售明细</span></a></li>
            <li><a href="../sales/SaleTargets.aspx"><span>销售指标分析</span></a></li>
            <li><a href="ProductSaleRanking.aspx" class="optionnext"><span>销售排行</span></a></li>
            <li class="menucurrent"><a href="#" class="optioncurrentend"><span class="optioncenter">商品购买与访问次数</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
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
      <div>
        <!--数据列表区域-->
	  <div class="datalist">
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

<%@ Page Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true" CodeBehind="ExportToPP.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.product.ExportToPP" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
			<li class="optionstar"><a href="exporttotb.aspx" class="optionnext"><span>导出为淘宝数据包</span></a></li>
			<li class="menucurrent"><a class="optioncurrentend"><span class="optioncenter">导出为拍拍数据包</span></a></li>
		</ul>
	</div>
<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" runat="server" NullToDisplay="--请选择产品线--" />
					</abbr>
				</li>
                <li><span>商家编码：</span><span> <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></span></li>
			</ul>
		</div>
		<div class="searcharea clearfix">
		    <ul>
		        <li><span>添加时间：</span></li>
		        <li>
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
		           至
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
		        </li>
		         <li><asp:Button ID="btnSearch" runat="server" Text="筛选" CssClass="searchbutton"/></li>
		    </ul>
		</div>
		<!--结束-->
         <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea" style="_width:500px;width:500px;">
				<div>导出数量：<asp:Label runat="server" ID="lblTotals"></asp:Label>件</div>
				<ul>
					<li><span>导出版本：</span></li>
					<li><asp:DropDownList runat="server" ID="dropExportVersions"></asp:DropDownList></li>
					<li><span style="margin-left:10px;"><asp:CheckBox runat="server" ID="chkExportStock" Text="导出库存数量" /></span></li>
				</ul>
			</div>
			<div class="pageNumber">
                <asp:Button runat="server" ID="btnExport" Text="导 出" CssClass="submit_DAqueding inbnt" />
			</div>
			<!--结束-->
			<div class="blank8 clearfix"></div>
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc"  AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <asp:TemplateField HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
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
                        <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="80"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />   
                        <Hi:MoneyColumnForAdmin HeaderText="采购价" ItemStyle-Width="80"  DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />                        
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="80" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
			                  <span class="submit_shanchu"><asp:LinkButton ID="btnRemove" runat="server" CommandName="Remove" Text="除外" /></span>
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("#ctl00_contentHolder_radCategory").bind("click", function() { $("#liCategory").show(); $("#liLine").hide(); });
            $("#ctl00_contentHolder_radLine").bind("click", function() { $("#liLine").show(); $("#liCategory").hide(); });
        });
   </script>
</asp:Content>
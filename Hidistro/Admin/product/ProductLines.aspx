<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductLines" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1><table><tr><td>产品线管理</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/cpxsz.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
  <span>产品线是一系列用于给不同分销商授权的商品组合，主要用于不同分销商所分销的商品的控制。</span></div>
  
		<!-- 添加按钮-->

<!--结束-->
		<!--数据列表区域-->
	<div class="datalist">
		<div class="searcharea clearfix br_search">
			<ul>
				<li class="batchHandleButton"><a href="AddProductLine.aspx" class="submit_jia">添加新的产品线</a></li>
			</ul>
		</div>
	<UI:Grid ID="grdProductLine" DataKeyNames="LineId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns> 
                    <asp:TemplateField HeaderText="产品线名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lbProductLineName" Text='<%# Eval("Name") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="供货商" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplierName" Text='<%# Eval("SupplierName") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="包含产品数量" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                        <table cellpadding="0" cellspacing="0" style="border:none;">
                                <tr><td style="border:none; width:10px;"><asp:HyperLink ID="lkProductSummary" NavigateUrl='<%# "ProductCatalog.aspx?LineId="+Eval("LineId")%>' runat="server"><span class="spanD"><%# Eval("ProductCount") %></span></asp:HyperLink> </td>
                                   <td style="border:none;"><span class="submit_xugai"><asp:HyperLink ID="HyperLink1" NavigateUrl='<%#"ProductOnSales.aspx?LineId="+Eval("LineId") %>' runat="server">铺货管理</asp:HyperLink></span></td></tr>
                            </table>                             
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left" >
                         <ItemTemplate>
                            <span class="submit_bianji"><asp:HyperLink ID="lkChange" runat="server" Text="转移商品" NavigateUrl='<%# "ChangeProductLine.aspx?LineId="+Eval("LineId")%>'></asp:HyperLink> </span>
                            <span class="submit_bianji"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditProductLine.aspx?LineId="+Eval("LineId")%>'></asp:HyperLink> </span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" IsShow="true" ID="Delete" Text="删除" CommandName="Delete"  DeleteMsg="确定要删除选择的产品线吗？" /></span>
                         </ItemTemplate>
                     </asp:TemplateField> 
                    </Columns>
                </UI:Grid>
	</div>
</div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="AuthorizeProductLines.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.AuthorizeProductLines" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title td_bottom m_none">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>授权产品线</h1>
  <span>供应商授予我分销权限的产品线</span></div>
  
		<!-- 添加按钮-->
	<!--结束-->
		<!--数据列表区域-->
	<div class="datalist">
  <UI:Grid ID="grdProductLine" DataKeyNames="LineId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns> 

                    <asp:TemplateField HeaderText="产品线名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lbProductLineName" Text='<%# Eval("Name") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="包含产品数量" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                                <asp:HyperLink ID="lkProductSummary" NavigateUrl='<%# "AuthorizeProducts_NoSite.aspx?LineId="+Eval("LineId")+"&lineName="+ Server.UrlEncode(Eval("Name").ToString())%>' runat="server"><span class="spanD"><%# Eval("ProductCount") %></span></asp:HyperLink> 
                        </ItemTemplate>
                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" >
                         <ItemTemplate>
                            <span class="Name"><asp:HyperLink ID="lkView" runat="server" Text="查看产品线商品" NavigateUrl='<%# "AuthorizeProducts_NoSite.aspx?LineId="+Eval("LineId")+"&lineName="+Server.UrlEncode(Eval("Name").ToString())%>'></asp:HyperLink> </span>
                         </ItemTemplate>
                     </asp:TemplateField> 
                     
                    </Columns>
                </UI:Grid>
  
  </div>
	</div>
		<!--数据列表底部功能区域-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

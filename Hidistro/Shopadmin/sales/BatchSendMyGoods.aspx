<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="BatchSendMyGoods.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.sales.BatchSendMyGoods" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/05.gif" width="32" height="32" /></em>
        <h1>订单批量发货</h1>
        <span>这里的订单将执行批量发货操作</span></div>
	
	 <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                </div>
			</div>
	 </div>
	<!--数据列表区域-->
    <div class="datalist">
     <UI:Grid ID="grdOrderGoods" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="OrderId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="订单编号">
                    <itemtemplate>
                         <a href='<%# "MyOrderDetails.aspx?OrderId="+Eval("OrderId") %>' target="_blank">详情</a>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="收货人">
                     <itemtemplate>
                           <asp:Literal runat="server" Text='<%# Eval("ShipTo") %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="地区">
                     <itemtemplate>                       
                         <asp:Literal runat="server" Text='<%# Eval("ShippingRegion") %>' />
                </itemtemplate> 
                </asp:TemplateField>
                <asp:TemplateField HeaderText="详细地址" ItemStyle-Width="80px">
                     <itemtemplate>                       
                           <asp:Literal runat="server" Text='<%# Eval("Address") %>' />
                     </itemtemplate> 
                </asp:TemplateField>
                <UI:DropdownColumn HeaderText="配送方式" ItemStyle-Width="100px"
                      JustForEditItem="false" ID="dropShippId" DataKey="ShippingModeId" DataTextField="Name"
                      DataValueField="ModeId" AllowNull="false" >
                </UI:DropdownColumn>
                <asp:TemplateField HeaderText="发货单号" ItemStyle-Width="80px">
                     <itemtemplate>
                         <asp:TextBox runat="server" ID="txtShippOrderNumber" Text='<%# Eval("ShipOrderNumber") %>' />
                     </itemtemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
        
     </div>
	 <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager2" />
                </div>
			</div>
	 </div>
     <div class="blank5 clearfix"></div>
     <div style="padding-left:380px;"><asp:Button runat="server" ID="btnBatchSendGoods" Text="批量发货" CssClass="submit_DAqueding" /></div>
</div>
</asp:Content>

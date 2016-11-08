<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPurchasePackingOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.purchaseOrder.PrintPurchasePackingOrder" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Import Namespace="Hidistro.Entities.Sales"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<%@ Import Namespace="Hidistro.Membership.Context" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <Hi:PageTitle ID="PageTitle1" runat="server" />
    <Hi:HeadContainer runat="server" />
    <link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/admin/css/windows.css" type="text/css" media="screen" />
     <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    <style type="text/css" media="print">.Noprn { DISPLAY: none }</style>
    <style type="text/css" media="screen">
        .Order_Info{ padding: 5px 5px 5px 5px; border-top:solid 2px black;}
        .Order_Info table tr td{ border-width:0px; width:300px;}
        .Order_Item{ padding: 5px 5px 5px 5px; border-top:solid 2px black;}
        .Order_Item table tr td{  border-left-width:0px; border-right-width:0px;}
        .Order_Item table tr th{ border-left-width:0px; border-right-width:0px; text-align:left;}
    </style>
</head>
<body>
   
    <form id="form1" runat="server">
     <div class="dataarea mainwidth" style="border-style:none;">
             <div class="Order_Info">
            <table cellpadding="3" cellspacing="0" width="100%">
                <tr>
                    <td>订购日期：<asp:Literal runat="server" ID="litOrderDate" /></td>
                    <td>订单号：<asp:Literal ID="litOrderId" runat="server" /></td>
                    <td>配送方式：<asp:Literal ID="litShipperMode" runat="server" /></td>
                </tr>
                <tr>
                    <td>支付方式：<asp:Literal ID="litPayType" runat="server" />预付款支付</td>
                    <td>发货单号：<asp:Literal ID="litShippNo" runat="server" /></td>
                    <td>收货人：<asp:Literal ID="litSkipTo" runat="server" /></td>
                </tr>
                <tr>
                    <td>收货地址：<asp:Literal ID="litAddress" runat="server" /></td>
                    <td>邮政编码：<asp:Literal ID="litZipCode" runat="server" /></td>
                    <td>手机号码：<asp:Literal ID="litCellPhone" runat="server" /></td>
                </tr>
                <tr>
                    <td>电话号码：<asp:Literal ID="litTelPhone" runat="server" /></td>
                    <td>备注：<asp:Literal ID="litRemark" runat="server" /></td>
                    <td>订单状态：<asp:Literal ID="litOrderStatus" runat="server" /></td>
                </tr>
            </table>
        </div>
        <div class="Order_Item">
            <div>
                <UI:Grid runat="server" ID="grdOrderItems" AutoGenerateColumns="false" Width="100%" BorderWidth="0">
                    <Columns>
                        <asp:TemplateField HeaderText="商品图片">
                            <ItemTemplate>
                                <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="货号">
                            <ItemTemplate>
                                <span class="colorC">货号：<%#Eval("sku") %>
	                                <%# Eval("SKUContent") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemDescription" HeaderText="商品" />
                     
                         <asp:TemplateField HeaderText="商品单价">
                            <ItemTemplate>
	                                <%# (Eval("ItemListPrice")==null||decimal.Parse(Eval("ItemListPrice").ToString())<=0)?"":Eval("ItemListPrice")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="Quantity" HeaderText="数量" />
                        <asp:TemplateField HeaderText="小计">
                            <ItemTemplate>
                                <%# (decimal)Eval("ItemListPrice") * (int)Eval("Quantity")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
            </div>
            <div>
                <UI:Grid runat="server" ID="grdOrderGifts" AutoGenerateColumns="false" Width="100%" BorderWidth="0" >
                    <Columns>
                        <asp:TemplateField HeaderText="商品图片">
                            <ItemTemplate>
                                <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GiftName" HeaderText="礼品" />
                        <asp:BoundField DataField="Quantity" HeaderText="购买数量" />
                    </Columns>
                </UI:Grid>
            </div>
        <div></div>
        <div class="Order_Item" style="height:40px; margin-top:13px;"><strong>签名：</strong></div>
    </div>
        <div style="text-align: center;margin-top:10px;margin-bottom: 20px;">
            <input class="Noprn" type="button" onclick="window.print()" value="打印" style=" width:100px; height:27px;" />
        </div>
    </form>
</body>
</html>

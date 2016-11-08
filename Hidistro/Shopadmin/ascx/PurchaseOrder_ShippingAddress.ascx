<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrder_ShippingAddress.ascx.cs" Inherits="Hidistro.UI.Web.Shopadmin.PurchaseOrder_ShippingAddress" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<h1>物流信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
        <tr id="tr_company" runat="server" visible="false">
            <td align="right">物流公司：</td>
            <td colspan="2"><asp:Literal ID="litCompanyName" runat="server" /></td>
          </tr>
          <tr>
            <td width="15%" align="right">收货地址：</td>
            <td width="60%"><asp:Literal ID="lblShipAddress" runat="server" /></td>
            <td width="25%">&nbsp;</td>
          </tr>
          <tr>
            <td align="right">送货上门时间：</td>
            <td colspan="2" width="85%"><asp:Literal ID="litShipToDate" runat="server" /></td>
          </tr>
          <tr>
            <td align="right">配送方式：</td>
            <td colspan="2"><asp:Literal ID="litModeName" runat="server" /><asp:Literal ID="ltrShipNum" runat="server"></asp:Literal></td>
          </tr>
       
          <tr>
            <td align="right">成交时间：</td>
            <td colspan="2"><Hi:FormatedTimeLabel ID="lblPurchaseDate" runat="server" /></td>
          </tr>
          <tr>
            <td align="right">买家留言：</td>
            <td colspan="2"><asp:TextBox ID="txtRemark"  runat="server" TextMode="MultiLine" Width="400px" Height="60px" CssClass="forminput" /></td>
          </tr>
          <tr>
            <td align="right">&nbsp</td>
            <td colspan="2"><asp:Button ID="btnSaveRemark" runat="server" Text="修改" /></td>
          </tr>
          
        </table>
        </div>

    <!--物流信息-->
    <asp:Panel ID="plExpress" runat="server" Visible="false" style="widht:730px;margin-bottom:10px;">
    <table width="100%" border="0" cellspacing="1" cellpadding="0" style="padding:20px" class="bTop_Margin10 OrderDetails_table2">
        <th style="text-align:left; font-size:14px;">快递单物流信息</th>
        <tr>
        <td class="cGray"  ><div id="spExpressData" >正在加载中....</div></td>
        </tr>
        <tr>
        <td class="cGray">
            <a href='http://www.kuaidi100.com' target='_blank' id="power" runat="server" visible="false" style="color:Red;">此物流信息由快递100提供</a>
        </td>
        </tr>
    </table>  
    <script type="text/javascript">
        $(function () {
            var PurchaseOrderId = window.location.search;
            PurchaseOrderId = PurchaseOrderId.substring(PurchaseOrderId.indexOf("=") + 1);
            
            $.ajax({
                url: "ExpressData.aspx?PurchaseOrderId=" + PurchaseOrderId,
                type: 'POST', dataType: 'json',
                async: true,
                success: function (resultData) {
                    $('#spExpressData').html(resultData.Express);
                }
            });
        })
    </script>
    </asp:Panel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrder_ShippingAddress.ascx.cs" Inherits="Hidistro.UI.Web.Admin.PurchaseOrder_ShippingAddress" %>
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
            <td width="25%"><span class="Name">
                  <asp:Label ID="lkBtnEditShippingAddress" runat="server" Visible="false"><a href="javascript:DialogFrame('purchaseOrder/MondifyAddressFrame.aspx?action=update&PurchaseOrderId=<%=Page.Request.QueryString["PurchaseOrderId"] %>','修改收货地址',645,315)">修改收货地址</a></asp:Label>
         </span></td>
          </tr>
           <tr>
            <td align="right">送货上门时间：</td>
            <td colspan="2" width="85%"><asp:Literal ID="litShipToDate" runat="server" /></td>
          </tr>
          <tr>
            <td align="right">配送方式：</td>
            <td colspan="2"><asp:Literal ID="litModeName" runat="server" /><%=edit %></td>
          </tr>
          <tr>
            <td align="right">成交时间：</td>
            <td colspan="2"><Hi:FormatedTimeLabel ID="lblPurchaseDate" runat="server" /></td>
          </tr>
           <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2" ><asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
        </table>
        </div>

    <%--    修改发货单号--%>
<div  id="updatetag_div"  style="display:none;">
      <div class="frame-content">
            <p><span class="frame-span frame-input90">发货单号：<em>*</em></span> <asp:TextBox ID="txtpost" CssClass="forminput" runat="server" />  </p>
      </div>
</div>

<div style="display:none">
<input type="hidden" id="hdtagId" runat="server" />
  <asp:Button ID="btnupdatepost" runat="server" Text="修 改"  CssClass="submit_DAqueding"/>
</div>

<script>
    function ShowPurchaseOrder() {
        formtype = "changeorder";
        arrytext = null;
        DialogShow("修改发货单号", 'changepurcharorder', 'updatetag_div', 'ctl00_contentHolder_shippingAddress_btnupdatepost');
    }

    //验证
    function validatorForm() {
        switch (formtype) {
            case "changeorder":
                if ($("#ctl00_contentHolder_shippingAddress_txtpost").val().replace(/\s/g, "")=="") {
                    alert("发货单号不允许为空！");
                    return false;
                }
                setArryText("ctl00_contentHolder_shippingAddress_txtpost", $("#ctl00_contentHolder_shippingAddress_txtpost").val());
                break;
            case "remark":
                arrytext = null;
                $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                if ($radioId == null || $radioId == "undefined") {
                    alert('请先标记备注');
                    return false;
                }
                setArryText($radioId.id, "true");
                setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                break;
            case "updateprice":
                setArryText("ctl00_contentHolder_txtPurchaseOrderDiscount", $("#ctl00_contentHolder_txtPurchaseOrderDiscount").val());
                break;
            case "updatemode":
                return ValidationShippingMode();
                break;
            case "close":
                return ValidationCloseReason();
                break;
        };
        return true;
    }
</script>

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
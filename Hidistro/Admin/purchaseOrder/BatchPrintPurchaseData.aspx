<%@ Page Language="C#"  MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BatchPrintPurchaseData.aspx.cs" Inherits="Hidistro.UI.Web.Admin.purchaseOrder.BatchPrintPurchaseData" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Import Namespace="Hidistro.Entities.Sales"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
   <div>
     <asp:Panel runat="server" ID="pnlTask">
       <div style="padding:9px 9px 9px 9px;">
            <span>采购单总数：</span><span style="padding-right:25px;"><strong><asp:Literal runat="server" ID="litNumber" /></strong></span> 
        </div>
       </asp:Panel>
       <asp:Panel runat="server" ID="pnlTaskEmpty">
       </asp:Panel>
   </div>
   <div class="title  m_none td_bottom" style="padding:5px 0px 5px 12px"> 
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td> <em><img src="../images/04.gif" width="32" height="32" /></em></td>
        <td width="100%"><h1>发货人信息</h1></td>
      </tr>
    </table> 
  </div> 
  <asp:Panel runat="server" ID="pnlPOEmpty">
    <div class="datafrom" style="padding:5px 5px 5px 5px;">
        <strong>注意：</strong>
        <span style="color:Red;">
        您所要打印的是某个分销商的采购单，这里的发货点最好选择那个分销商的发货点（默认是主站的默认发货点）</span>
    </div>
  </asp:Panel>
    <div class="datafrom" style="padding-bottom:1px;">
    <asp:Panel runat="server" ID="pnlShipper">
      <table width="100%" border="0" cellspacing="1" cellpadding="0" class="PrintDataTable">
   <tr>
    <th><span>发货点选择：</span></th>
    <td colspan="3"><Hi:ShippersDropDownList runat="server" ID="ddlShoperTag" AutoPostBack="true" /></td>
  </tr>
  <tr>
    <th><span>发货人姓名：<em>*</em></span></th>
    <td colspan="3"><asp:TextBox runat="server" ID="txtShipTo" CssClass="forminput" /></td>
  </tr>
  <tr>
    <th><span>省区：<em>*</em></span></th>
    <td colspan="3"><Hi:RegionSelector ProvinceTitle="" runat="server" id="dropRegions" /></td>
    </tr>
  <tr>
    <th width="15%"><span>详细地址：<em>*</em></span></th>
    <td width="35%"><asp:TextBox runat="server" ID="txtAddress"  CssClass="forminput" Width="280"/></td>
    <th width="15%" class="leftb"><span>邮　编：</span></th>
    <td width="35%"><asp:TextBox runat="server" ID="txtZipcode"  CssClass="forminput" /></td>
  </tr>
  <tr>
    <th><span>联系电话：</span></th>
    <td><asp:TextBox runat="server" ID="txtTelphone"  CssClass="forminput"/></td>
    <th class="leftb"><span>手　机：</span></th>
    <td><asp:TextBox runat="server" ID="txtCellphone" CssClass="forminput"/></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td colspan="3">
      <a class="linkSub float">
       <asp:Button ID="btnUpdateAddrdss" runat="server"  OnClientClick="return DoValid();"  Text="修改发货地址"/>
      </a>
      <span class="fonts colorB">(您可以将编辑过的发货点信息进行更新)</span>
    </td>
    </tr>
</table>
</asp:Panel>
    <asp:Panel runat="server" ID="pnlEmptySender">
        <span><a href="/admin/sales/AddShipper.aspx">您还没有添加发货人信息，请先点击这里添加发货人信息!</a></span>
    </asp:Panel>
    </div>
   
   <div class="title  m_none td_bottom" style="padding:5px 0px 5px 12px"> 
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><em><img height="32" width="32" src="../images/10.gif"></em></td>
        <td width="100%"><h1>选择快递单模板</h1></td>
      </tr>
    </table> 
  </div> 
  <div class="datafrom" style="padding-bottom:1px;">
    <asp:Panel runat="server" ID="pnlTemplates">
    <table width="100%" border="0" cellspacing="1" cellpadding="0" class="PrintDataTable">
   <tr>
    <td colspan="3">
     <span>选择模版:<em>*</em></span>
      </td>
      <td>
     <asp:DropDownList runat="server" ID="ddlTemplates"></asp:DropDownList>
     </td>
     <td>快递单起始编号:</td>
     <td>
        <asp:TextBox runat="server" ID="txtStartCode"/>   
     </td>
     <td>
        <asp:Button runat="server" ID="btnPrint" CssClass="printSub" OnClientClick="return DoPrint();" />
     </td>
     </div>
    </tr>
    </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlEmptyTemplates">
        <span><a href="AddExpressTemplate.aspx">您还没有添加快递单模板，请先点击这里添加快递单模板!</a></span>
    </asp:Panel>
  </div>
  <script language="javascript" type="text/javascript">
        function DoValid() {
            // 检查收货人姓名
            var v = $("#ctl00_contentHolder_txtShipTo").val();
            var len = v.length;

            if (len < 1 || len > 30) {
                alert("请填写发货人姓名，长度在30个字符之间");
                return false;
            }

            // 检查省区选择
            if ($("#regionSelectorValue").val() == "") {
                alert("请选择发货人所在的地区");
                return false;
            }

            // 检查详细地址
            v = $("#ctl00_contentHolder_txtAddress").val();
            len = v.length;
            if (len < 1 || len > 200) {
                alert("详细地址不能为空，长度限制在200个字符以内");
                return false;
            }

         

            // 检查电话
            v = $("#ctl00_contentHolder_txtTelphone").val();
            len = v.length;
            if ((len > 0) && (len < 3 || len > 20)) {
                alert("发货人的电话号码(区号-电话号码-分机)，限制在3-20字符");
                return false;
            }

            // 检查手机
            v = $("#ctl00_contentHolder_txtCellphone").val();
            len = v.length;
            if ((len > 0) && (len < 3 || len > 20)) {
                alert("发货人的手机号码，限制在3-20字符");
                return false;
            }

            // 电话和手机二者必填一
            if ($("#ctl00_contentHolder_txtTelphone").val().length == 0 && $("#ctl00_contentHolder_txtCellphone").val().length == 0) {
                alert("电话号码和手机号码必须填写其中一项");
                return false;
            }

            return true;
        }

        function DoPrint() {
            if (DoValid()) {
                if ($("#ctl00_contentHolder_ddlTemplates").val().length == 0) {
                    alert("请选择一个快递单模板");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtStartCode").val().length == 0) {
                    alert("请录入快递单起始编号");
                    return false;
                }
                var mailNo = parseInt($("#ctl00_contentHolder_txtStartCode").val(), 10);
                if (isNaN(mailNo)) {
                    alert("快递单起始编号须为自然数");
                    return false;
                }
                if (mailNo <= 0) {
                    alert("快递单起始编号须为数字格式！");
                    return false;
                }
                var url = "PrintPurchase.aspx?mailNo=" + $("#ctl00_contentHolder_txtStartCode").val() + "&template=" + $("#ctl00_contentHolder_ddlTemplates").val() + "&shipperId=" + $("#ctl00_contentHolder_ddlShoperTag").val() + "&purchaseOrderIds=<%= orderIds %>";
                document.location.href = url;
                return false;
            }
            return false;
        }
 </script>
</div>
</asp:Content>

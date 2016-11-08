<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true"
    CodeBehind="ChoosePayment.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.purchaseOrder.ChoosePayment" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function CloseDiv(id) {
            var div = document.getElementById(id);
            div.style.display = "none";
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom title_height">
            <em>
                <img src="../images/003.gif" width="32" height="32" /></em>
            <h1 class="title_line">
                给采购单付款
            </h1>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle Pw_160">支付方式：<em>*</em></span> <span style="float: left;">
                        <Hi:DistributorPaymentRadioButtonList ID="radioPaymentMode" runat="server" RepeatDirection="Horizontal"
                            RepeatColumns="1" AutoPostBack="false" />
                    </span></li>
                </ul>
                <ul class="btntf Pa_140 clear">
                    <asp:Button ID="btnSubmit" Text="确认提交" CssClass="submit_DAqueding inbnt" OnClientClick="return ThePageIsValid();"
                        runat="server" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>

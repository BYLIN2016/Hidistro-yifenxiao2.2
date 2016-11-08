<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMemberPrices.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditMemberPrices" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1 class="title_line">批量修改商品会员零售价格</h1>
	    <span class="font">如果会员等级价没填，系统会自动按等级折扣计算；您可以对已选的这些商品直接调价或按公式调价，也可以手工输入您想要的价格后在页底处保存设置</span>
     </div>
     <div class="searcharea clearfix">
        <ul>
            <li>直接调价：<Hi:MemberPriceDropDownList ID="ddlMemberPrice" runat="server" AllowNull="false" /> = <asp:TextBox ID="txtTargetPrice" runat="server" Width="80px" /></li>
            <li><asp:Button ID="btnTargetOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
        </ul>
        <ul>
            <li>公式调价：<Hi:MemberPriceDropDownList ID="ddlMemberPrice2" runat="server" AllowNull="false" /> = <Hi:MemberPriceDropDownList ID="ddlSalePrice" runat="server" AllowNull="false" /> </li>
            <li><Hi:OperationDropDownList ID="ddlOperation" runat="server" AllowNull="false"  /><asp:TextBox ID="txtOperationPrice" runat="server" Width="80px" /></li>
			<li><asp:Button ID="btnOperationOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
		</ul>
     </div>
    <div class="datalist">
	     <Hi:GridSkuMemberPriceTable runat="server" />
            </div>          
        <div class="Pg_15 Pg_010" style="text-align:center;">
            <Hi:TrimTextBox runat="server" ID="txtPrices" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
            <asp:Button ID="btnSavePrice" runat="server" OnClientClick="return loadSkuPrice();" Text="保存设置"  CssClass="submit_DAqueding"/></div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function loadSkuPrice() {
        if(!checkPrice())
            return false;
        
        var skuPriceXml = "<xml><skuPrices>";
        $.each($(".SkuPriceRow"), function() {
            var skuId = $(this).attr("skuId");
            var costPrice = $("#tdCostPrice_" + skuId).val();
            var salePrice = $("#tdSalePrice_" + skuId).val();
            var itemXml = String.format("<item skuId=\"{0}\" costPrice=\"{1}\" salePrice=\"{2}\">", skuId, costPrice, salePrice);
            itemXml += "<skuMemberPrices>";
            
            $(String.format("input[type='text'][name='tdMemberPrice_{0}']", skuId)).each(function(rowIndex, rowItem){
                var id = $(this).attr("id");
                var gradeId = id.substring(0, id.indexOf("_"));
                var memberPrice = $(this).val();    
                if(memberPrice != "")            
                    itemXml += String.format("<priceItme gradeId=\"{0}\" memberPrice=\"{1}\" \/>", gradeId, memberPrice);
            });
            
            itemXml += "<\/skuMemberPrices>";            
            itemXml += "<\/item>";
            skuPriceXml += itemXml;
        });
        skuPriceXml += "<\/skuPrices><\/xml>";
        $("#ctl00_contentHolder_txtPrices").val(skuPriceXml);
        return true;
    }
    
    function checkPrice() {
        var validated = true;
        var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");

         $.each($(".SkuPriceRow"), function() {
            var skuId = $(this).attr("skuId");
            var costPrice = $("#tdCostPrice_" + skuId).val();
            var salePrice = $("#tdSalePrice_" + skuId).val();

            // 检查必填项是否填了
            if (salePrice.length == 0) {
                alert("商品规格的一口价为必填项！");
                $("#tdSalePrice_" + skuId).focus();
                validated = false;
                return false;
            }
            
            if (!exp.test(salePrice)) {
                alert("商品规格的一口价输入有误");
                $("#tdSalePrice_" + skuId).focus();
                validated = false;
                return false;
            }
            
            var num = parseFloat(salePrice);
            if (num > 10000000 || num <= 0) {
                alert("商品规格的一口价超出了系统表示范围！");
                $("#tdSalePrice_" + skuId).focus();
                validated = false;
                return false;
            }   

            if (costPrice.length > 0) {
                // 检查输入的是否是有效的金额
                if (!exp.test(costPrice)) {
                    alert("商品规格的成本价输入有误！");
                    $("#tdCostPrice_" + skuId).focus();
                    validated = false;
                    return false;
                }

                // 检查金额是否超过了系统范围
                var num = parseFloat(costPrice);
                if (num > 10000000 || num < 0) {
                    alert("商品规格的成本价超出了系统表示范围！");
                    $("#tdCostPrice_" + skuId).focus();
                    validated = false;
                    return false;
                }
            }
            
            $(String.format("input[type='text'][name='tdMemberPrice_{0}']", skuId)).each(function(rowIndex, rowItem){
                var id = $(this).attr("id");
                var memberPrice = $(this).val();    
                if(memberPrice.length > 0)
                {
                    // 检查输入的是否是有效的金额
                    if (!exp.test(memberPrice)) {
                        alert("商品规格的会员等级价输入有误！");
                        $(this).focus();
                        validated = false;
                        return false;
                    }

                    // 检查金额是否超过了系统范围
                    var num = parseFloat(memberPrice);
                    if (!((num >= 0.01) && (num <= 10000000))) {
                        alert("商品规格的会员等级价超出了系统表示范围！");
                        $(this).focus();
                        validated = false;
                        return false;
                    }
                }  
                if(validated == false)
                    return false;               
            });
            if(validated == false)
                return false;     
        });

    return validated;
}
</script>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/ShopAdmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditeMyOrderPromotions.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditeMyOrderPromotions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="cc1" TagName="MyPromotionView" Src="~/Shopadmin/promotion/Ascx/MyPromotionView.ascx" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_promotionView_txtPromoteSalesName', 1, 60, false, null, '促销活动的名称，在1至60个字符之间'));
    }

    $(document).ready(function() {
        InitValidators();
        SelectPromoteType();
        ShowPromotion(false);
        $("input[type='radio'][name='radPromoteType']").bind("click", function() { ShowPromotion(true); });
    });

    function SelectPromoteType() {
        var promoteType = $("#ctl00_contentHolder_txtPromoteType").val();
        if (promoteType == 11)
            $("#radPromoteType_FullAmountDiscount").attr("checked", true);
        else if (promoteType == 12)
            $("#radPromoteType_FullAmountReduced").attr("checked", true);
        else if (promoteType == 13)
            $("#radPromoteType_FullQuantityDiscount").attr("checked", true);
        else if (promoteType == 14)
            $("#radPromoteType_FullQuantityReduced").attr("checked", true);
        else if (promoteType == 15)
            $("#radPromoteType_FullAmountSentGift").attr("checked", true);
        else if (promoteType == 16)
            $("#radPromoteType_FullAmountSentTimesPoint").attr("checked", true);
        else if (promoteType == 17)
            $("#radPromoteType_FullAmountSentFreight").attr("checked", true);
    }

    function ShowPromotion(isClick) {
        $("#liPromoteTypeDiscount").hide();
        if (isClick) {
            $("#ctl00_contentHolder_txtCondition").val("");
            $("#ctl00_contentHolder_txtDiscountValue").val("");
        }

        if ($("#radPromoteType_FullAmountDiscount").attr("checked") == "checked") {
            $("#liPromoteTypeDiscount").show();
            $("#spCondition").show();
            $("#spDiscountValue").show();
            $("#lblConditionTip").html("满足金额(元)：")
            $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
        }
        else if ($("#radPromoteType_FullAmountReduced").attr("checked") == "checked") {
            $("#liPromoteTypeDiscount").show();
            $("#spCondition").show();
            $("#spDiscountValue").show();
            $("#lblConditionTip").html("满足金额(元)：")
            $("#lblDiscountValueTip").html("立减金额(元)：");
        }
        else if ($("#radPromoteType_FullQuantityDiscount").attr("checked") == "checked") {
            $("#liPromoteTypeDiscount").show();
            $("#spCondition").show();
            $("#spDiscountValue").show();
            $("#lblConditionTip").html("满足数量(件)：")
            $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
        }
        else if ($("#radPromoteType_FullQuantityReduced").attr("checked") == "checked") {
            $("#liPromoteTypeDiscount").show();
            $("#spCondition").show();
            $("#spDiscountValue").show();
            $("#lblConditionTip").html("满足数量(件)：")
            $("#lblDiscountValueTip").html("立减金额(元)：");
        }
        else if ($("#radPromoteType_FullAmountSentGift").attr("checked") == "checked") {
            $("#liPromoteTypeDiscount").show();
            $("#spCondition").show();
            $("#spDiscountValue").hide();
            $("#lblConditionTip").html("满足金额(元)：");
        }
        else if ($("#radPromoteType_FullAmountSentTimesPoint").attr("checked") == "checked") {
            $("#liPromoteTypeDiscount").show();
            $("#spCondition").show();
            $("#spDiscountValue").show();
            $("#lblConditionTip").html("满足金额(元)：");
            $("#lblDiscountValueTip").html("倍数：");
        }
        else if ($("#radPromoteType_FullAmountSentFreight").attr("checked") == "checked") {
            $("#liPromoteTypeDiscount").show();
            $("#spCondition").show();
            $("#spDiscountValue").hide();
            $("#lblConditionTip").html("满足金额(元)：");
        }
    }

    function Valid() {
        var promoteType = $("input[type='radio'][name='radPromoteType']:checked").val();
        var condition = $("#ctl00_contentHolder_txtCondition").val();
        var discountValue = $("#ctl00_contentHolder_txtDiscountValue").val();
        var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");
        var numexp = new RegExp("^-?[0-9]\\d*$", "i");

        if (promoteType == undefined) {
            alert("请选择促销活动类型！")
            return false;
        }

        $("#ctl00_contentHolder_txtPromoteType").val(promoteType);

        if (promoteType == 11) {
            if (condition.length == 0) {
                alert("请输入满足金额！");
                return false;
            }
            if (!exp.test(condition)) {
                alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                return false;
            }
            if (discountValue.length == 0) {
                alert("请输入折扣值(一般在0.01-1之间)！");
                return false;
            }
            if (!exp.test(discountValue)) {
                alert("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                return false;
            }
            var num = parseFloat(discountValue);
            if (num < 0.01 || num > 1) {
                alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                return false;
            }
        }
        else if (promoteType == 12) {
            if (condition.length == 0) {
                alert("请输入满足金额！");
                return false;
            }
            if (!exp.test(condition)) {
                alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                return false;
            }
            if (discountValue.length == 0) {
                alert("请输入立减金额(元)！");
                return false;
            }
            if (!exp.test(discountValue)) {
                alert("输入立减金额(元)有误(必须是数值)，请重新输入正确的立减金额！");
                return false;
            }
        }
        else if (promoteType == 13) {
            if (condition.length == 0) {
                alert("请输入满足数量！");
                return false;
            }
            if (!numexp.test(condition)) {
                alert("输入满足数量有误(必须是数值)，请重新输入正确的满足数量！");
                return false;
            }
            if (discountValue.length == 0) {
                alert("请输入折扣值(一般在0.01-1之间)！");
                return false;
            }
            if (!exp.test(discountValue)) {
                alert("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                return false;
            }
            var num = parseFloat(discountValue);
            if (num < 0.01 || num > 1) {
                alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                return false;
            }
        }
        else if (promoteType == 14) {
            if (condition.length == 0) {
                alert("请输入满足数量！");
                return false;
            }
            if (!numexp.test(condition)) {
                alert("输入满足数量有误(必须是数值)，请重新输入正确的满足数量！");
                return false;
            }

            if (discountValue.length == 0) {
                alert("请输入折扣值(一般在0.01-1之间)！");
                return false;
            }
            if (!exp.test(discountValue)) {
                alert("折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                return false;
            }
        }
        else if (promoteType == 15) {
            if (condition.length == 0) {
                alert("请输入满足金额！");
                return false;
            }
            if (!exp.test(condition)) {
                alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                return false;
            }
        }
        else if (promoteType == 16) {
            if (condition.length == 0) {
                alert("请输入满足金额！");
                return false;
            }
            if (!exp.test(condition)) {
                alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                return false;
            }
            if (discountValue.length == 0) {
                alert("请输入倍数！");
                return false;
            }
            if (!exp.test(condition)) {
                alert("输入的倍数有误(必须是数值)，请重新输入倍数！");
                return false;
            }
        }
        else if (promoteType == 17) {
            if (condition.length == 0) {
                alert("请输入满足金额！");
                return false;
            }
            if (!exp.test(condition)) {
                alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                return false;
            }
        }

        if (!PageIsValid())
            return false;
        if ($("#ctl00_contentHolder_promotionView_calendarStartDate").val() == "") {
            alert("请选择促销活动开始时间！")
            return false;
        }
        if ($("#ctl00_contentHolder_promotionView_calendarEndDate").val() == "") {
            alert("请选择促销活动结束时间！")
            return false;
        }

        return true;
    }
</script>


<div class="areacolumn clearfix">
<div class="columnleft clearfix"><ul><li><a href="MyOrderPromtions.aspx"><span>订单促销活动</span></a></li></ul></div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑订单促销活动</h1>
          </div>
      <div class="formitem validator2">
            <ul> 
            <li> <span class="formitemtitle Pw_110">促销活动类型：<em>*</em></span>
                <Hi:PromoteTypeRadioButtonList IsProductPromote="false" IsSubSite="true" runat="server" ID="radPromoteType" />
                <Hi:TrimTextBox runat="server" ID="txtPromoteType" style="display:none;"></Hi:TrimTextBox>
              </li>  
               <li id="liPromoteTypeDiscount" style="display:none;"> <span class="formitemtitle Pw_110"></span>
               <table><tr>
               <td> <span id="spCondition"><span id="lblConditionTip"></span><asp:TextBox ID="txtCondition" runat="server" CssClass="forminput"></asp:TextBox></span></td>
               <td><span id="spDiscountValue"><span id="lblDiscountValueTip"></span><asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox></span></td>
               </tr></table>
               </li>                 
              <cc1:MyPromotionView runat="server" ID="promotionView"  />                 
        </ul>
          <ul class="btntf Pa_110 clear">
          <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Valid();" CssClass="submit_DAqueding"/>
        </ul>
      </div>

      </div>

</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

   
</asp:Content>

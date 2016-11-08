<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProductPromotion.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddProductPromotion" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="cc1" TagName="PromotionView" Src="~/Admin/promotion/Ascx/PromotionView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function InitValidators(){
    initValid(new InputValidator('ctl00_contentHolder_promotionView_txtPromoteSalesName', 1, 60, false, null, '促销活动的名称，在1至60个字符之间'));
}

$(document).ready(function () {
    InitValidators();
    SelectPromoteType();
    ShowPromotion(false);
    $("input[type='radio'][name='radPromoteType']").bind("click", function () { ShowPromotion(true); });
});

function SelectPromoteType(){
    var promoteType = $("#ctl00_contentHolder_txtPromoteType").val();
    if(promoteType == 1)
        $("#radPromoteType_Discount").attr("checked", true);
    else if(promoteType == 2)
        $("#radPromoteType_Amount").attr("checked", true);
    else if(promoteType == 3)
        $("#radPromoteType_Reduced").attr("checked", true);
    else if(promoteType == 4)
        $("#radPromoteType_QuantityDiscount").attr("checked", true);
    else if(promoteType == 5)
        $("#radPromoteType_SentGift").attr("checked", true);
    else if(promoteType == 6)
        $("#radPromoteType_SentProduct").attr("checked", true);
}

function ShowPromotion(isClick){
    $("#liPromoteTypeDiscount").hide();
    if(isClick){
        $("#ctl00_contentHolder_txtCondition").val("");
        $("#ctl00_contentHolder_txtDiscountValue").val("");
    }
    if($("#radPromoteType_Discount").attr("checked") == "checked"){
        $("#liPromoteTypeDiscount").show();
        $("#spCondition").hide();
        $("#spDiscountValue").show();
        $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
    }
    else if($("#radPromoteType_Amount").attr("checked") == "checked"){
        $("#liPromoteTypeDiscount").show();
        $("#spCondition").hide();
        $("#spDiscountValue").show();
        $("#lblDiscountValueTip").html("出售金额(元)：");
    }
    else if($("#radPromoteType_Reduced").attr("checked") == "checked"){
        $("#liPromoteTypeDiscount").show();
        $("#spCondition").hide();
        $("#spDiscountValue").show();
        $("#lblDiscountValueTip").html("立减金额(元)：");
    }
    else if($("#radPromoteType_QuantityDiscount").attr("checked") == "checked"){
        $("#liPromoteTypeDiscount").show();
        $("#spCondition").show();
        $("#spDiscountValue").show();
        $("#lblConditionTip").html("购买(件)：");
        $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
    }
    else if($("#radPromoteType_SentProduct").attr("checked") == "checked"){
        $("#liPromoteTypeDiscount").show();
        $("#spCondition").show();
        $("#spDiscountValue").show();
        $("#lblConditionTip").html("购买(件)：");
        $("#lblDiscountValueTip").html("赠送(件)：");
    }    
}

function Valid(){   
    var promoteType = $("input[type='radio'][name='radPromoteType']:checked").val();
    var condition = $("#ctl00_contentHolder_txtCondition").val();
    var discountValue = $("#ctl00_contentHolder_txtDiscountValue").val();
    var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");
    var numexp = new RegExp("^-?[0-9]\\d*$", "i");
    
    if(promoteType == undefined){
        alert("请选择促销活动类型！")
        return false;
    } 
    
    $("#ctl00_contentHolder_txtPromoteType").val(promoteType);
    
    if(promoteType == 1){        
        if(discountValue.length == 0){
            alert("请输入折扣值(一般在0.01-1之间)！");
            return false;
        }
        if (!exp.test(discountValue)){
            alert("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
            return false;
        }
        var num = parseFloat(discountValue);
        if (num < 0.01 || num > 1) {
            alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
            return false;
        }
    }
    else if(promoteType == 2){        
        if(discountValue.length == 0){
            alert("请输入出售金额(元)！");
            return false;
        }
        if (!exp.test(discountValue)){
            alert("输入出售金额(元)有误(必须是数值)，请重新输入正确的出售金额！");
            return false;
        }
    }
    else if(promoteType == 3){        
        if(discountValue.length == 0){
            alert("请输入立减金额(元)！");
            return false;
        }
        if (!exp.test(discountValue)){
            alert("输入立减金额(元)有误(必须是数值)，请重新输入正确的立减金额！");
            return false;
        }
    }
    else if(promoteType == 4){      
        if(condition.length == 0){
            alert("请输入购买数量！");
            return false;
        }
        
        if (!numexp.test(condition)){
            alert("输入购买数量有误(必须是正数)，请重新输入正确的购买数量！");
            return false;
        }
          
        if(discountValue.length == 0){
            alert("请输入折扣值(一般在0.01-1之间)！");
            return false;
        }
        if (!exp.test(discountValue)){
            alert("折扣值有误(必须是数值)，请重新输入正确的折扣值！");
            return false;
        }
        var num = parseFloat(discountValue);
        if (num < 0.01 || num > 1) {
            alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
            return false;
        }
    }
    else if(promoteType == 6){      
        if(condition.length == 0){
            alert("请输入购买数量！");
            return false;
        }
        
        if (!numexp.test(condition)){
            alert("输入购买数量有误(必须是数值)，请重新输入正确的购买数量！");
            return false;
        }
          
        if(discountValue.length == 0){
            alert("请输入赠送数量(件)！");
            return false;
        }
        if (!numexp.test(discountValue)){
            alert("输入赠送数量(件)有误(必须是数值)，请重新输入正确的赠送数量！");
            return false;
        }
    }
    
    if(!PageIsValid())
        return false;
    if($("#ctl00_contentHolder_promotionView_calendarStartDate").val() == ""){
        alert("请选择促销活动开始时间！")
        return false;
    }
    if($("#ctl00_contentHolder_promotionView_calendarEndDate").val() == ""){
        alert("请选择促销活动结束时间！")
        return false;
    }
    
    return true;
}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix validator4">
	<div class="columnright">
		<div class="title"> 
		  <em><img src="../images/07.gif" width="32" height="32" /></em>
		  <h1 id="h1" runat="server">添加商品促销活动</h1>
		  <span id="span1" runat="server">添加商品促销活动</span>
	   </div>
	       <div class="datafrom">
    <div class="formitem validator4">
        <ul> 
            <li> <span class="formitemtitle Pw_110"><em>*</em>促销活动类型：</span>
                <Hi:PromoteTypeRadioButtonList IsProductPromote="true" runat="server" ID="radPromoteType" />
                <Hi:TrimTextBox runat="server" ID="txtPromoteType" style="display:none;"></Hi:TrimTextBox>
              </li>  
               <li id="liPromoteTypeDiscount" style="display:none;">
                    <table>
                    <tr>
                    <td><span id="spCondition"><span id="lblConditionTip" class="formitemtitle Pw_110"></span><asp:TextBox ID="txtCondition" runat="server" CssClass="forminput"></asp:TextBox></span>　　</td>
                    <td> <span id="spDiscountValue"><span id="lblDiscountValueTip" class="formitemtitle Pw_110" style="width:230px;"></span><asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox></span></td>
                    </tr></table>

               </li>                 
              <cc1:PromotionView runat="server" ID="promotionView"  />                
        </ul>
        <ul class="btntf Pa_110 clear" style="margin-left:8px;">
         <asp:Button ID="btnNext" runat="server" Text="下一步,添加促销商品" OnClientClick="return Valid();" CssClass="submit_jixu inbnt m_none"/>
        </ul>
      </div>
    </div>
	</div>
</div>
</asp:Content>

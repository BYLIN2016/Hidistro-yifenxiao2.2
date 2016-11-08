<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditMyOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyOrder" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">  
<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <em><img src="../images/05.gif" width="32" height="32" /></em>
      <h1 class="title_line">修改订单</h1>
</div>
    <div class="list">
    <h1>商品列表</h1> 
      <UI:Grid ID="grdProducts" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" DataKeyNames="SkuId" SortOrderBy="SKU" SortOrder="DESC" GridLines="None" AllowSorting="true" Width="100%">
                
             <Columns>
                <asp:TemplateField HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                      <div style="float:left;width:60px;height:60px;"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank"><Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" /></a></div>     
                      <div style="float:left;margin-left:10px;line-height:22px;"><span><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank"><%# Eval("ItemDescription") %></a>
                       <br/>货号：<asp:Literal ID="litSku" runat="server" Text='<%# Eval("Sku") %>' /><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("SKUContent") %>' /> 
                      </div>
                    </ItemTemplate>
                </asp:TemplateField>               
                
                <asp:TemplateField HeaderText="商品单价"  HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="15%">
                    <itemtemplate>
	                    <Hi:FormatedMoneyLabel ID="productPrice" runat="server" Money='<%# Eval("ItemListPrice") %>'></Hi:FormatedMoneyLabel>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="购买数量"  HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="20%">
                       <itemtemplate>                     
                            <asp:TextBox  Columns="6" ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>'  CssClass="forminput leftR10" TagPrice="inputValue"></asp:TextBox>  
                             <span class="submit_jiage"><asp:LinkButton ID="setQuantity" runat="server" Text="修改" CommandName="setQuantity"  ></asp:LinkButton></span>                   
                      </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="发货数量"  HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="10%">
                    <itemtemplate>
                       <asp:Literal ID="litShipmentQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>' ></asp:Literal>      
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="总价"  HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="10%">
                    <itemtemplate>
                    <div class="color_36c">
            <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
            </div>
                        <strong class="colorG"><Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff"  HeaderStyle-Width="15%">
                  
                    <itemtemplate> 
			            <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" ></Hi:ImageLinkButton></span>
                    </itemtemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>   
	  <div class="Price">
<table width="200" border="0" cellspacing="0">
	       <tr class="bg">
	      <td class="Pg_top td_none" width="88%" align="right" >商品金额（元）：</td>
	      <td class="Pg_top td_none" width="12%" ><strong class="fonts colorG"><Hi:FormatedMoneyLabel ID="lblAllPrice" runat="server"  /></strong></td>
        </tr>
	    <tr class="bg">
	      <td class="Pg_bot" align="right">商品总重量（克）：</td>
	      <td class="Pg_bot" ><strong class="fonts "><asp:Label ID="lblWeight" runat="server"  /></strong></td>
        </tr>
        </table>
	  </div>
      <h1>礼品列表</h1>
      <asp:GridView ID="grdOrderGift" runat="server" AutoGenerateColumns="false" DataKeyNames="GiftId" SortOrderBy="GiftId" SortOrder="DESC" GridLines="None" HeaderStyle-CssClass="table_title" AllowSorting="true" Width="100%" >
            <Columns>
                <asp:TemplateField  HeaderText="礼品" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="20%">
                    <ItemTemplate>
                      <Hi:HiImage ID="HiImage1" AutoResize="true" Width="60" Height="60" runat="server" DataField="ThumbnailsUrl" />
                      <span><asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span>                           
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="礼品单价"  HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="10%">
                    <itemtemplate>
	                    <Hi:FormatedMoneyLabel ID="giftPrice" runat="server" Money='<%# Eval("CostPrice") %>'></Hi:FormatedMoneyLabel>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="礼品赠送数量"  HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="15%">
                       <itemtemplate>                     
                            <asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'  ></asp:Literal>                   
                      </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="礼品发货数量" HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="10%">
                    <itemtemplate>
                       <asp:Literal ID="litShipmentQuantity" runat="server" Text='<%# Eval("Quantity") %>' ></asp:Literal>      
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="礼品总金额" HeaderStyle-CssClass="td_right td_left"  HeaderStyle-Width="10%">
                    <itemtemplate>
                        <Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" Money='<%# (decimal)Eval("CostPrice")*(int)Eval("Quantity") %>' />
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff"  HeaderStyle-Width="10%">                  
                    <itemtemplate> 
			           <span class="submit_shanchu"> <Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" ></Hi:ImageLinkButton></span>
                    </itemtemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        
     
<h1>订单实收款结算</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">打折优惠(元)：<br /></td>
            <td width="11%" class="a_none"><span class="colorB"><Hi:FormatedMoneyLabel  ID="fullDiscountAmount" runat="server"  /></span></td>
            <td width="74%" class="a_none"><span class="Name"><asp:HyperLink Target="_blank" ID="lkbtnFullDiscount" runat="server" /></span></td>
          </tr>
          <tr>
            <td align="right">满额免费用活动(元)：</td>
            <td colspan="2" class="a_none"><asp:HyperLink Target="_blank" ID="lkbtnFullFree" runat="server" /></td>
          </tr>
          <tr>
            <td align="right">运费(元)： </td>
            <td class="a_none"><asp:TextBox  ID="txtAdjustedFreight" runat="server"  CssClass="forminput" width="70" /></td>
            <td class="a_none"><asp:Literal ID="litShipModeName" runat="server"/> </td>
          </tr>
          <tr>
            <td align="right">支付手续费(元)：</td>
            <td class="a_none"><asp:TextBox  ID="txtAdjustedPayCharge" runat="server" CssClass="forminput" width="70" /></td>
            <td class="a_none"><asp:Literal ID="litPayName" runat="server"/></td>
          </tr>
          <tr>
            <td align="right">优惠券折扣(元)：</td>
            <td colspan="2" class="a_none" ><Hi:FormatedMoneyLabel ID="couponAmount" runat="server"  /></td>
          </tr>
          <tr>
            <td align="right">涨价或减价(元)：</td>
            <td class="a_none"><asp:TextBox ID="txtAdjustedDiscount" runat="server"  CssClass="forminput" width="70" /></td>
            <td class="a_none">为负代表折扣，为正代表涨价 </td>
          </tr>
          <asp:Literal ID="litTax" runat="server" />
          <asp:Literal ID="litInvoiceTitle" runat="server" />
          <tr>
            <td align="right">订单可得积分：</td>
            <td colspan="2" class="a_none"><asp:Literal ID="litIntegral" runat ="server"></asp:Literal>&nbsp;<asp:HyperLink ID="hlkSentTimesPointPromotion" runat="server" Target="_blank" /></td>
          </tr>
          <tr class="bg">
            <td align="right" class="colorG">订单实收款(元)：</td>
            <td colspan="2" class="a_none"> <strong class="colorG fonts"><asp:Literal ID="litTotal" runat ="server"/></strong></td>
          </tr>
        </table>
</div>
        <div class="bnt Pa_140 Pg_15 Pg_18">
        <asp:Button ID="btnUpdateOrderAmount" OnClientClick="return PageIsValid();" runat="server" Text="保存修改"  CssClass="submit_DAqueding" style="float:left"></asp:Button>
            </div>
    </div>
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

        <script type="text/javascript" language="javascript">
function InitValidators()
{

initValid(new InputValidator('ctl00_contentHolder_txtAdjustedPayCharge', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '支付手续费只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAdjustedPayCharge', 0, 10000000, '支付手续费只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtAdjustedFreight', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '运费只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAdjustedFreight', 0, 10000000, '运费只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtAdjustedDiscount', 1, 10, false,  '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '订单折扣只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAdjustedDiscount', -10000000, 10000000, '订单折扣只能是数值，不能超过10000000，且不能超过2位小数'));

}
$(document).ready(function(){ 
   InitValidators(); 
   
   // 给输入值加限制
     $(".list table tr td input").each(function (index, domEle){
	   if($(this).attr("TagPrice")=="inputValue")
	     {
			$(this).keyup(function(e)
			{
			   //var key = window.event?e.keyCode:e.which;
			    var inputValue=$(this).val();
				inputValue=inputValue.replace(/[^\d]/g,'');
			   $(this).val(inputValue);
		    });		
	     }		
	 })
   
});
</script>
</asp:Content>

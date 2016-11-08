<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditMyGroupBuy.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyGroupBuy" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" src="groupbuy.helper.js"></script>
  <script type="text/javascript" language="javascript">
      function InitValidators() {

          initValid(new InputValidator('ctl00_contentHolder_txtNeedPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，违约金只能输入数值'))
          appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNeedPrice', 1, 9999999, '输入的数值超出了系统表示范围'));          

          initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，限购数量只能输入整数型数值'))
          appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCount', 1, 9999999, '输入的数值超出了系统表示范围'));
          
          initValid(new InputValidator('ctl00_contentHolder_txtCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，限购数量只能输入整数型数值'))
          appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCount', 1, 9999999, '输入的数值超出了系统表示范围'));  
          
          initValid(new InputValidator('ctl00_contentHolder_txtPrice',1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '团购价格只能输入数值,且不能超过2位小数'))
          appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPrice',0.01,9999999, '输入的数值超出了系统表示范围'));

         
      }
      $(document).ready(function () {
          InitValidators();
          $("#li_price").hide();
          $.ajax({
              url: "EditMyGroupBuy.aspx",
              data:
                      {
                          isCallback: "true",
                          productId: $("#ctl00_contentHolder_dropGroupBuyProduct").val()
                      },
              type: 'GET', dataType: 'json', timeout: 10000,
              async: false,
              success: function (resultData) {
                  if (resultData.Status == "OK") {
                      var price = resultData.Price;
                      $("#ctl00_contentHolder_lblPrice").html(price);
                      $("#li_price").show();
                  }
              }
          });
          $("#ctl00_contentHolder_dropGroupBuyProduct").change(function () {
              var pId = $(this).val();
              if (pId == "") {
                  $("#li_price").hide();
              }
              else {
                  $.ajax({
                      url: "EditMyGroupBuy.aspx",
                      data:
                      {
                          isCallback: "true",
                          productId: pId
                      },
                      type: 'GET', dataType: 'json', timeout: 10000,
                      async: false,
                      success: function (resultData) {
                          if (resultData.Status == "OK") {
                              var price = resultData.Price;
                              $("#ctl00_contentHolder_lblPrice").html(price);
                              $("#li_price").show();
                          }
                      }
                  });
              }
          });
      });     
  </script>     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
<div class="columnleft clearfix"><ul><li><a href="MyGroupBuys.aspx"><span>团购活动管理</span></a></li></ul></div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑团购活动</h1>
          </div>          
       <div class="formitem validator5" style="padding-left:15px;">      
		<ul style="background:#FFFADF;border:1px solid #ccc;color:#f60;padding-top:15px;height:40px;">
		   	<table border="0" cellspacing="5" cellpadding="0" style="width:60%;" class=float">
                  <tr>
                    <td><span class="formitemtitle Pw_100">商品名称：</span></td>
                    <td><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></td>
                    <td><abbr class="formselect">
						 <Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" />
					     </abbr></td>
                    <td ><span class="formitemtitle Pw_100" style="white-space:nowrap;">商家编码：</span></td>
                    <td><asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></td>
                    <td><input type="button" id="Button1" value="查询" onclick="ResetGroupBuyProducts()" class="searchbutton"/></td>
                  </tr>
            </table>
    	</ul>    	
    	<ul>
        <li></li>
        <li><span class="formitemtitle Pw_140">团购商品：</span>
			<abbr class="formselect">
						<Hi:DistributorGroupBuyProductDropDownList ID="dropGroupBuyProduct" runat="server" />
					</abbr>
					<p id="P1">当此团购活动有会员已订购时，商品不能再进行编辑</p>
			</li>
            <li id="li_price"><span class="formitemtitle Pw_140">一口价：</span>
			<abbr class="formselect">
						<asp:Label ID="lblPrice" runat="server"></asp:Label>
					</abbr>
					<p id="P4"></p>
			</li>
			<li> <span class="formitemtitle Pw_140">开始日期：<em >*</em></span>
          <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarStartDate" /><Hi:HourDropDownList ID="drophours" runat="server" />
          <p id="P3">当达到开始日期时，活动会自动变为正在参与活动状态。</p>
          </li>
          <li> <span class="formitemtitle Pw_140">结束日期：<em >*</em></span>
          <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarEndDate" /><Hi:HourDropDownList ID="HourDropDownList1" runat="server" />
          <p id="P2">当达到结束日期时，活动会自动变为结束未处理状态。</p>
          </li>
          <li>
             <span class="formitemtitle Pw_140">违约金：</span>
             <asp:TextBox ID="txtNeedPrice" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtNeedPriceTip">违约将扣除的金额，不填表示没有违约金,客户在管理员主动将活动设置为失败的情况下不视为违约。</p>
          </li>
          <li><span class="formitemtitle Pw_140">限购总数量：<em >*</em></span>
            <asp:TextBox ID="txtMaxCount" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtMaxCountTip">此次活动可购买的商品总数量,不能为空,订购达到此上限时，活动会自动变为结束未处理状态。</p>
          </li>
           <li><span class="formitemtitle Pw_140">团购满足数量：<em >*</em></span>
            <asp:TextBox ID="txtCount" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtCountTip">满足此次活动的最低商品订购数量,不能为空。</p>
          </li>
          <li><span class="formitemtitle Pw_140">团购价格：<em >*</em></span>
            <asp:TextBox ID="txtPrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtPriceTip">达到订购数量后享受的团购价格,不能为空。</p>
          </li>
          <li>
	       <span class="formitemtitle Pw_140">活动说明：</span>
		     <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Columns="50" Rows="5" CssClass="forminput"></asp:TextBox>
	     </li>
	     <li></li>
      </ul>
      <ul class="btn Pa_100 clear">
      <li>
         <asp:Button ID="btnUpdateGroupBuy" runat="server" Text="保存" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding float"  />
         <asp:Button ID="btnFinish" runat="server" Text="结束活动" Visible="false"  CssClass="submit_DAqueding float"  />
         <asp:Button ID="btnSuccess" runat="server" Text="活动成功" Visible="false"  CssClass="submit_DAqueding float"  />
         <asp:Button ID="btnFail" runat="server" Text="活动失败"  Visible="false"   CssClass="submit_DAqueding float"  />
         </li>
        </ul>
      </div>      
  </div>
  </div>

</asp:Content>

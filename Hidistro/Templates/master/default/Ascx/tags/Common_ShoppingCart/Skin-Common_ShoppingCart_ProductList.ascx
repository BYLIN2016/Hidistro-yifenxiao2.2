<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Panel ID="pnlShopProductCart" runat="server">
<asp:DataList ID="dataListShoppingCrat" runat="server" Width="100%">
    <HeaderTemplate>
  <table width="100%" cellspacing="0" cellpadding="0" border="0" class="cart_commodit_th">
                            <tbody>
                                 <tr>	
                                    <th width="30"><input type="checkbox" /></th>
                                    <th width="80">商品图片</th>
                                    <th width="350">商品名称</th>
                                    <th width="150">商品单价</th>
                                    <th width="120">购买数量</th>
                                    <th width="150">小计</th>
                                    <th width="150">操作</th>
                                </tr>
    </HeaderTemplate>
    <ItemTemplate>
	<tr><td colspan="7">
       <table width="100%" cellspacing="0" cellpadding="0" border="0"  class="cart_commodit_con">
            <tbody>
                <tr>
                    <td width="30" align="center"><input type="checkbox" id="ck_<%# Eval("SkuId") %>" name="ck_productId" value="<%# Eval("SkuId") %>" /></td>
                    <td width="80" align="center" >
                        <span class="cart_commodit_span" >
                            <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" ImageLink="true">
<Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl60" runat="server" />
</Hi:ProductDetailsLink>
                        </span>
                    </td>
                    <td width="350" align="left" style="text-align:left; padding-left:25px;">
                        <div class="cart_commodit_name">
                            <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" />
                        </div >
                        <div class="cart_commodit_para">   <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></div>                 	
                    </td>
                    <td width="150" align="center"><span class="cart_commodit_price">￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" runat="server" Money='<%# Eval("MemberPrice") %>' /></span></td>
                    <td width="120" align="center">
                     <asp:Literal runat="server" ID="litSkuId" Text='<%# Eval("SkuId")%>' Visible="false"></asp:Literal>
                     <asp:TextBox runat="server" ID="txtBuyNum" Text='<%# Eval("Quantity")%>' Width="30" CssClass="cart_txtbuynum" inputTagID='<%# Eval("SKU")%>' />
                     <asp:Button ID="Button1" runat="server" CssClass="cart_update" Text="更新"  CommandName="updateBuyNum" SubmitTagID='<%# Eval("SKU")%>' />
                   <div style="margin-top:5px;"><asp:Literal ID="litGiveQuantity" Text='<%# (int)Eval("Quantity")==(int)Eval("ShippQuantity")?"":"赠送："+((int)Eval("ShippQuantity")-(int)Eval("Quantity")) %>' runat="server" /></div>
                      <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                    Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                    </td>
                    <td width="150" align="center"><span class="cart_commodit_price2">￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" runat="server" Money='<%# Eval("SubTotal") %>' /></span></td>
                    <td width="150" align="center">
                         <asp:Button ID="Button2" runat="server" Cssclass="cart_commodit_del" CommandName="delete"
                    Text="删除" />
                    </td>
                </tr>
            </tbody>
         </table>
            </td>
	</tr>
       
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:DataList>
  <script type="text/javascript">
      $(document).ready(function() {
          $("input").each(function(i, obj) {
              var inputTagObj = $(this).attr("inputTagID");
              if (inputTagObj) {
                  //按下回车键
                  $(this).keydown(function(obj) {
                      var key = window.event ? obj.keyCode : obj.which;
                      if (key == 13) {
                          $("input").each(function(i, submitObj) {
                              var submitTagObj = $(submitObj).attr("SubmitTagID");
                              if (submitTagObj == inputTagObj) { $(submitObj).focus(); }
                          })
                      }
                  })
                  //失去焦点
                  $(this).blur(function(obj) {
                      $("input").each(function(i, submitObj) {
                          var submitTagObj = $(submitObj).attr("SubmitTagID");
                          if (submitTagObj == inputTagObj) { $(submitObj).focus(); }
                      })
                  })
              }
          })
      });

      $("#ShoppingCart_Common_ShoppingCart_ProductList___dataListShoppingCrat input[type='checkbox']").first().bind("click", function () {
          $("#ShoppingCart_Common_ShoppingCart_ProductList___dataListShoppingCrat input[type='checkbox']").not(this).attr("checked",this.checked);
      });
          
      </script>
</asp:Panel>
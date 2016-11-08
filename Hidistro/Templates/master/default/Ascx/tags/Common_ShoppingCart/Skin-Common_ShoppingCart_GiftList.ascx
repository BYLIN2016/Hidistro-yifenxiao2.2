<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlFreeShopGiftCart" runat="server">
 <h2 class="giftbanner">����������Ʒ</h2>
 <div class="cart_commodit">
<asp:DataList ID="dataShopFreeGift" runat="server" Width="100%">
    <HeaderTemplate>
        <table border="0" cellpadding="0" cellspacing="0" class="cart_commodit_th"  Width="100%">
            <tr>
                <th width="30">&nbsp;</th>
                <th width="80">
                    ��ƷͼƬ
                </th>
                <th width="350">
                    ��Ʒ����
                </th>
                <th width="150">
                    �һ��������
                </th>
                <th width="120">
                    ����
                </th>
                <th width="150">
                    ����С��
                </th>
                <th width="150">
                    ����
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
	<tr><td colspan="7">
	<table  border="0" cellpadding="0" cellspacing="0" class="cart_commodit_con" Width="100%">
          <tr>
            <td width="30" align="center">&nbsp;</td>
            <td width="80" align="center" >
                <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'>
                        <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl60" runat="server" />
                </a>
            </td>
            <td  width="350" align="left" style="text-align:left; padding-left:25px;" >
               <div class="cart_commodit_name">
               <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><%# Eval("Name") %></a>
               </div>
            </td>
            <td width="150" align="center">
            <span class="cart_commodit_price"><%# Eval("NeedPoint")%></span>
            </td>
            <td width="120" align="center">
                <%# Eval("Quantity")%>            </td>
            <td  width="150" align="center" >
                <asp:Literal ID="Literal1" runat="server"  Text='<%# Eval("SubPointTotal") %>' /><br />
                 <%# Eval("PromoType").ToString().Equals("5") ? "<img src=\""+Globals.ApplicationPath+"/Utility/pics/mjcx.png\" />" : ""%>
            </td>
            <td width="150" align="center">
                <asp:Button ID="btnFeeDel" runat="server"   Cssclass="cart_commodit_del" CommandArgument='<%# Eval("GiftId") %>'  CommandName="delete"
                    Text="ɾ��" />
            </td>
        </tr>
	</table>
            </td>
	</tr>
       
    </ItemTemplate>
    <FooterTemplate>
        </table>
       
    </FooterTemplate>
</asp:DataList>
</div>
</asp:Panel>
 

<asp:Panel ID="pnlShopGiftCart" runat="server">
<h2 class="giftbanner">���ֶһ���Ʒ</h2>
<div class="cart_commodit">
<asp:DataList ID="dataListGiftShoppingCrat" runat="server" Width="100%">
    <HeaderTemplate>
        <table border="0" cellpadding="0" cellspacing="0" class="cart_commodit_th"  Width="100%">
            <tr>
                <th width="30">&nbsp;</th>
                <th align="center" width="80">
                    ��ƷͼƬ
                </th>
                <th align="center"  width="350">
                    ��Ʒ����
                </th>
                <th align="center"  width="150">
                    �һ��������
                </th>
                <th align="center"  width="120">
                    ����
                </th>
                <th align="center" width="150">
                    ����С��
                </th>
                <th align="center" width="150">
                    ����
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
	<tr><td colspan="7">
	<table  border="0" cellpadding="0" cellspacing="0" class="cart_commodit_con" Width="100%">
          <tr>
            <td  width="30" align="center">&nbsp;</td>
            <td width="80" align="center" >
                <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'>
                        <Hi:ListImage DataField="ThumbnailUrl60" runat="server" />
                </a>
            </td>
            <td width="350" align="left" style="text-align:left; padding-left:25px;" >
              <div class="cart_commodit_name">
               <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><%# Eval("Name") %></a>
               </div>
            </td>
            <td  width="150" align="center">
            <span class="cart_commodit_price"><%# Eval("NeedPoint")%></span>
            </td>
            <td   width="120" align="center">
                <asp:Literal runat="server" ID="litGiftId" Text='<%# Eval("GiftId")%>' Visible="false"></asp:Literal>
                <asp:TextBox runat="server" ID="txtBuyNum" Text='<%# Eval("Quantity")%>' Width="30"
                     CssClass="cart_txtbuynum"  inputTagID='<%# Eval("GiftId")%>' />
                <asp:Button runat="server" CssClass="cart_update"  CommandName="updateBuyNum"  Text=" " SubmitTagID='<%# Eval("GiftId")%>' />
            </td>
            <td  width="150" align="center" >
                <asp:Literal runat="server"  Text='<%# Eval("SubPointTotal") %>' /><br />
                 <%# Eval("PromoType").ToString().Equals("5") ? "<img src=\"/Templates/master/default/images/mjcx.png\" />" : ""%>
            </td>
            <td width="150" align="center">
                <asp:Button runat="server"   Cssclass="cart_commodit_del"  CommandName="delete"
                    Text="ɾ��" />
            </td>
        </tr>
	</table>
            </td>
	</tr>
       
    </ItemTemplate>
    <FooterTemplate>
        </table>
          <script type="text/javascript">
              $(document).ready(function() {
                  $("input").each(function(i, obj) {
                      var inputTagObj = $(this).attr("inputTagID");
                      if (inputTagObj) {
                          //���»س���
                          $(this).keydown(function(obj) {
                              var key = window.event ? obj.keyCode : obj.which;
                              if (key == 13) {
                                  $("input").each(function(i, submitObj) {
                                      var submitTagObj = $(submitObj).attr("SubmitTagID");
                                      if (submitTagObj == inputTagObj) { $(submitObj).focus(); }
                                  })
                              }
                          })
                          //ʧȥ����
                          $(this).blur(function(obj) {
                              $("input").each(function(i, submitObj) {
                                  var submitTagObj = $(submitObj).attr("SubmitTagID");
                                  if (submitTagObj == inputTagObj) { $(submitObj).focus(); }
                              })
                          })
                      }
                  })
              })
      </script>
    </FooterTemplate>
</asp:DataList>
</div>
</asp:Panel>

  
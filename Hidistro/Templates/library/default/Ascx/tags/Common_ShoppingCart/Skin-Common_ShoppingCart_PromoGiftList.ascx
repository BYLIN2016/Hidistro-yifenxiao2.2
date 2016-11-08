<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlPromoGift" runat="server">
<div class="promogift">
<div style="width:980px; overflow:hidden;"><span class="remain">您还可以选择<em><asp:Literal ID="lit_promonum" runat="server">3</asp:Literal></em>件礼品哦</span><span class="amount"><span>总共可选择<em><asp:LinkButton runat="server"><asp:Literal ID="lit_promosumnum" runat="server">7</asp:Literal></asp:LinkButton></em>件礼品！<a id="a_change" href="#" class="down">我要选择</a></span></span></div>
  <div class="divpromgift">
<ul id="ul_promgift" style="display:none;">
 <asp:Repeater ID="rp_promogift" runat="server">
 <ItemTemplate>
     <li class="li_promgift">
        <div class="cart_commodit_span"><Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl100" runat="server" /></div>
        <div class="cart_commodit_name"><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><%# Eval("Name") %></a></div>
        <span class="cart_commodit_price">¥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" runat="server" Money='<%# Eval("MarketPrice") %>' /></span>
        <div class="xuanze"><asp:LinkButton ID="linkChange" runat="server" Text="选择" CssClass="a_buy" CommandName="change" CommandArgument='<%# Eval("GiftId") %>'></asp:LinkButton></div>
     </li>
 </ItemTemplate>
 </asp:Repeater>
</ul>
</div>
</div>
<script>
    $("#a_change").toggle(function() {
        $(this).addClass("up");
        $("#ul_promgift").slideToggle("slow");

    }, function() {
        $(this).removeClass();
        $(this).addClass("down");
        $("#ul_promgift").slideToggle("hide");
    });

    if ($(".remain em:eq(0)").text() != "0") {
        $("#a_change").addClass("up");
        $("#ul_promgift").slideToggle("slow");
    }
</script>
</asp:Panel>
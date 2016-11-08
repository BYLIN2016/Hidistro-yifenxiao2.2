<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<li>
    <div class="buy_product">
        <div class="buy_pic">
            <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'>
                   <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl220" CustomToolTip="ProductName" />
            </a>
        </div>
        <div class="buy_name">
            <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><%# Eval("Name") %></a>
        </div>
        <div class="buy_price">
            <em style="font-size:12px;">所需积分：<span><%# Eval("NeedPoint") %></span></em>
            <p>
            <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'>去看看</a>
            </p>
        </div>
        <div class="buy_andere">
            <p>
                市场价：<del>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></del></p>
         </div>
    </div>
</li>

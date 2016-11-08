<%@ Control Language="C#" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<style type="text/css">
.zoom-section{clear:both; }
*html .zoom-section{display:inline;clear:both;}
.zoom-desc{width:388px; padding:12px; height:60px; overflow:hidden;}
.zoom-desc p{ width:450px;  overflow:hidden; }
.zoom-desc a{ float:left; width:60px; height:60px;  display:block; border:1px solid #dad8d9; margin-right:18px; background:#fff;}
.zoom-desc a.hover{border:2px solid #ca1622;}
.zoom-small-image{float:left;  width:410px; height:410px; border:1px solid #d9d9d9; background:#fff;  }
.zoom-small-image a{ height:410px; }

.zoom-tiny-image{margin:0px;}
.zoom-tiny-image:hover{border:1px solid #cc000;}
</style>
<table cellpadding="0" cellspacing="0" border="0" width="410">

    <tr class="product_preview3">
        <td>
            <div class="zoom-section">
                <div class="zoom-section">
                    <div class="zoom-small-image">
                        <asp:HyperLink runat="server" CssClass="cloud-zoom" ID='zoom1' rel="adjustX:10, adjustY:-4"
                            ClientIDMode="Static">
                            <table cellspacing="0" cellpadding="0" border="0" width="100%" height="100%">
                                <tr>
                                    <td align="center" valign="middle">
                                        <Hi:HiImage ID="imgBig" runat="server" title="Optional title display" AlternateText="" />
                                    </td>
                                </tr>
                            </table>
                        </asp:HyperLink>
                    </div>
                    <div class="zoom-desc">
                        <p>
                            <asp:HyperLink ID="iptPicUrl1" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall1" runat="server" alt="Thumbnail 1" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl2" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall2" runat="server" alt="Thumbnail 2" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl3" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall3" runat="server" alt="Thumbnail 3" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl4" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall4" runat="server" alt="Thumbnail 4" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl5" runat="server" CssClass="cloud-zoom-gallery" title="">
                                <Hi:HiImage ID="imgSmall5" runat="server" alt="Thumbnail 5" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                        </p>
                    </div>
                    </div>
                    
                </div>
        </td>
    </tr>
    <tr>
        <td height="10">
        </td>
    </tr>
    <tr>
        <td class="product_share">
            <h2 style="float: left;">
                分享：</h2>
            <div class="jiathis_style">
                <a class="jiathis_button_qzone"></a><a class="jiathis_button_tsina"></a><a class="jiathis_button_tqq">
                </a><a class="jiathis_button_renren"></a><a class="jiathis_button_kaixin001"></a>
                <a href="http://www.jiathis.com/share" class="jiathis jiathis_txt jtico jtico_jiathis"
                    target="_blank"></a><a class="jiathis_counter_style"></a>
            </div>
            <script type="text/javascript" src="http://v3.jiathis.com/code/jia.js?uid=1350294149446525"
                charset="utf-8"></script>
        </td>
    </tr>
</table>


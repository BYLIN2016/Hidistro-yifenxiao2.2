<%@ Control Language="C#"%>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<style type="text/css">
.zoom-section{clear:both; }
*html .zoom-section{display:inline;clear:both;}
.zoom-desc{width:388px; padding:12px; height:60px; overflow:hidden; clear:both; background:#F4F0F1}
.zoom-desc p{ width:450px;  overflow:hidden; }
.zoom-desc a{ float:left; width:60px; height:60px;  display:block; border:1px solid #dad8d9; margin-right:18px; background:#fff;}
.zoom-desc a.hover{border:2px solid #ca1622;}
.zoom-small-image{float:left;  width:410px; height:410px; border:1px solid #d9d9d9; border-left:0px; background:#fff;  }
.zoom-small-image a{ height:410px; }
</style>

<div class="zoom-section">    	  
    <div class="zoom-small-image">
        <asp:HyperLink runat="server"  CssClass="cloud-zoom" ID='zoom1' rel="position:'inside',showTitle:false,zoomWidth:300,zoomHeight:300,adjustX:40,adjustY:40" ClientIDMode="Static">
            <table cellspacing="0" cellpadding="0" border="0" width="100%" height="100%">
                <tr>
                    <td align="center" valign="middle">
                        <Hi:HiImage Id="imgBig" runat="server" title="Optional title display" AlternateText="" />
                    </td>
                </tr>
            </table>
        </asp:HyperLink>
    </div>
    <div class="zoom-desc">    
	    <p>
            <asp:HyperLink ID="iptPicUrl1" runat="server" CssClass="cloud-zoom-gallery"  title="">
                <Hi:HiImage Id="imgSmall1" runat="server"  alt = "Thumbnail 1" />
            </asp:HyperLink>
	        <asp:HyperLink ID="iptPicUrl2" runat="server" CssClass="cloud-zoom-gallery"  title="">
                <Hi:HiImage Id="imgSmall2" runat="server" alt = "Thumbnail 2" CssClass="zoom-tiny-image" />
            </asp:HyperLink>                  
	        <asp:HyperLink ID="iptPicUrl3" runat="server" CssClass="cloud-zoom-gallery"  title="">
                <Hi:HiImage Id="imgSmall3" runat="server" alt = "Thumbnail 3" CssClass="zoom-tiny-image" />
            </asp:HyperLink>
            <asp:HyperLink ID="iptPicUrl4" runat="server" CssClass="cloud-zoom-gallery"  title="">
                <Hi:HiImage Id="imgSmall4" runat="server" alt = "Thumbnail 4" CssClass="zoom-tiny-image" />
            </asp:HyperLink>
            <asp:HyperLink ID="iptPicUrl5" runat="server" CssClass="cloud-zoom-gallery"  title="">
                <Hi:HiImage Id="imgSmall5" runat="server" alt = "Thumbnail 5" CssClass="zoom-tiny-image" />
            </asp:HyperLink>
        </p>
    </div>
    <div style="margin-top:15px; float:right"><Hi:Common_ProductImageAlbum ID="hlinkmoreImage"  ImageUrl="/Templates/master/default/images/more_img_icon.jpg"  runat="server" /></div>
</div>

<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="footer" id="footer">
    <div class="footer_ad">
        <Hi:Common_ImageAd runat="server" AdId="8" />
    </div>
    <div class="footer_p1">
        <div class="guide">
            <ul>
                <Hi:Common_Help runat="server" TemplateFile="/ascx/tags/Common_Comment/Skin-Common_Help.ascx" />
            </ul>
        </div>
        <div class="online_kf">
            <Hi:Common_ImageAd runat="server" AdId="12" />
        </div>
    </div>
    <div class="footer_custom">
        <div>
            <Hi:PageFooter ID="PageFooter1" runat="server" />
        </div>
        <div>
            <Hi:CnzzShow ID="CnzzShow1" runat="server" />
        </div>
    </div>
</div>

</body>
</html> 
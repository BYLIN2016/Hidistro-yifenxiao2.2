namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Common_OnlineServer : AscxTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            Literal literal = (Literal) this.FindControl("litOnlineServer");
            literal.Text = HiContext.Current.SiteSettings.HtmlOnlineServiceCode;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_Comment/Skin-Common_OnlineServer.ascx";
            }
            base.OnInit(e);
        }
    }
}


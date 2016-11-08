namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_Footer : AscxTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-Common_Footer.ascx";
            }
            base.OnInit(e);
        }
    }
}


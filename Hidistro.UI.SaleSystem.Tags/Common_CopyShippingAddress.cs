namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_CopyShippingAddress : AscxTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_CopyShippingAddress.ascx";
            }
            base.OnInit(e);
        }
    }
}


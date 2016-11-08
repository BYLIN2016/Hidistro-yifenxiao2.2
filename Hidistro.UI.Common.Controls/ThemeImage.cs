namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class ThemeImage : HtmlImage
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (base.Src.StartsWith("~"))
            {
                base.Src = base.ResolveUrl(base.Src);
            }
            else if (base.Src.StartsWith("/"))
            {
                base.Src = HiContext.Current.GetSkinPath() + base.Src;
            }
            else if (base.Src.ToLower().StartsWith("http://"))
            {
                base.Src = base.ResolveUrl(base.Src);
            }
            else
            {
                base.Src = HiContext.Current.GetSkinPath() + "/" + base.Src;
            }
            base.Render(writer);
        }
    }
}


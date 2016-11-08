namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class HiImageButton : ImageButton
    {
        public EventHandler Click;

        private void HiImageButton_Click(object sender, ImageClickEventArgs e)
        {
            this.OnClick(e);
        }

        protected override void OnClick(ImageClickEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.Click += new ImageClickEventHandler(this.HiImageButton_Click);
            base.OnLoad(e);
        }

        public override string ImageUrl
        {
            get
            {
                return base.ImageUrl;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string relativeUrl = value;
                    if (relativeUrl.StartsWith("~"))
                    {
                        relativeUrl = base.ResolveUrl(relativeUrl);
                    }
                    else if (relativeUrl.StartsWith("/"))
                    {
                        relativeUrl = HiContext.Current.GetSkinPath() + relativeUrl;
                    }
                    else
                    {
                        relativeUrl = HiContext.Current.GetSkinPath() + "/" + relativeUrl;
                    }
                    base.ImageUrl = relativeUrl;
                }
                else
                {
                    base.ImageUrl = null;
                }
            }
        }
    }
}


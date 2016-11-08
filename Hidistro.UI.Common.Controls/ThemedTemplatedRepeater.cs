namespace Hidistro.UI.Common.Controls
{
    using ASPNET.WebControls;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI.WebControls;

    public class ThemedTemplatedRepeater : Repeater
    {
        private string skinName = string.Empty;

        protected override void CreateChildControls()
        {
            if ((this.ItemTemplate == null) && !string.IsNullOrEmpty(this.TemplateFile))
            {
                this.ItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
            }
        }

        public string TemplateFile
        {
            get
            {
                if (!string.IsNullOrEmpty(this.skinName) && !Utils.IsUrlAbsolute(this.skinName.ToLower()))
                {
                    return (Utils.ApplicationPath + this.skinName);
                }
                return this.skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.StartsWith("/"))
                    {
                        this.skinName = HiContext.Current.GetSkinPath() + value;
                    }
                    else
                    {
                        this.skinName = HiContext.Current.GetSkinPath() + "/" + value;
                    }
                }
                if (!this.skinName.StartsWith("/templates"))
                {
                    this.skinName = this.skinName.Substring(this.skinName.IndexOf("/templates"));
                }
            }
        }
    }
}


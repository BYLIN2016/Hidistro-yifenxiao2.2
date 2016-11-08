namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Globalization;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class AscxTemplatedWebControl : TemplatedWebControl
    {
        private string skinName;

        protected AscxTemplatedWebControl()
        {
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.LoadThemedControl())
            {
                throw new SkinNotFoundException(this.SkinPath);
            }
            this.AttachChildControls();
        }

        protected virtual bool LoadThemedControl()
        {
            if (this.SkinFileExists && (this.Page != null))
            {
                Control child = this.Page.LoadControl(this.SkinPath);
                child.ID = "_";
                this.Controls.Add(child);
                return true;
            }
            return false;
        }

        private bool SkinFileExists
        {
            get
            {
                return !string.IsNullOrEmpty(this.SkinName);
            }
        }

        public virtual string SkinName
        {
            get
            {
                return this.skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower(CultureInfo.InvariantCulture);
                    if (value.EndsWith(".ascx"))
                    {
                        this.skinName = value;
                    }
                }
            }
        }

        protected virtual string SkinPath
        {
            get
            {
                if (this.SkinName.StartsWith("/"))
                {
                    return (HiContext.Current.GetSkinPath() + this.SkinName);
                }
                return (HiContext.Current.GetSkinPath() + "/" + this.SkinName);
            }
        }
    }
}


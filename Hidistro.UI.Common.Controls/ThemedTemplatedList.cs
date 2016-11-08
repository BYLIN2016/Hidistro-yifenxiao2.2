namespace Hidistro.UI.Common.Controls
{
    using ASPNET.WebControls;
    using Hidistro.Membership.Context;
    using System;

    public class ThemedTemplatedList : TemplatedList
    {
        public override string TemplateFile
        {
            get
            {
                return base.TemplateFile;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.StartsWith("/"))
                    {
                        base.TemplateFile = HiContext.Current.GetSkinPath() + value;
                    }
                    else
                    {
                        base.TemplateFile = HiContext.Current.GetSkinPath() + "/" + value;
                    }
                }
                if (!base.TemplateFile.StartsWith("/templates"))
                {
                    base.TemplateFile = base.TemplateFile.Substring(base.TemplateFile.IndexOf("/templates"));
                }
            }
        }
    }
}


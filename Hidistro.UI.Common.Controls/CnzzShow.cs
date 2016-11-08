namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;

    public class CnzzShow : LiteralControl
    {
        protected override void OnLoad(EventArgs e)
        {
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if ((siteSettings.EnabledCnzz && !string.IsNullOrEmpty(siteSettings.CnzzPassword)) && !string.IsNullOrEmpty(siteSettings.CnzzUsername))
            {
                base.Text = "<script src='http://pw.cnzz.com/c.php?id=" + siteSettings.CnzzUsername + "&l=2' language='JavaScript' charset='gb2312'></script>";
            }
            base.OnLoad(e);
        }
    }
}


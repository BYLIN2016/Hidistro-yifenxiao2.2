namespace Hidistro.UI.Web.Admin.distribution
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [PrivilegeCheck(Privilege.MakeProductsPack)]
    public class UploadTaoBaoCsv : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.Redirect("http://order1.kuaidiangtong.com/ImporterTaoBaoCSV.aspx?SiteUrl=" + HiContext.Current.SiteUrl);
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    [PrivilegeCheck(Privilege.ExpressTemplates)]
    public class AddSampleExpressTemplate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = this.Page.Request.QueryString["ExpressName"];
            string str2 = this.Page.Request.QueryString["XmlFile"];
            if ((string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2)) || !str2.EndsWith(".xml"))
            {
                base.GotoResourceNotFound();
            }
        }
    }
}


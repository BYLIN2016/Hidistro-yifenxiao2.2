namespace Hidistro.UI.Web.Admin.product
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.product.ascx;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddProductType)]
    public class AddAttribute : AdminPage
    {
        protected AttributeView attributeView;
        protected Button btnNext;

        private void btnNext_Click(object sender, EventArgs e)
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/AddSpecification.aspx?typeId=" + this.Page.Request.QueryString["typeId"]), true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
        }
    }
}


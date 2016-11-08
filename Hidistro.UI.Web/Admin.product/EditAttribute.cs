namespace Hidistro.UI.Web.Admin.product
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.product.ascx;
    using System;

    [PrivilegeCheck(Privilege.EditProductType)]
    public class EditAttribute : AdminPage
    {
        protected AttributeView attributeView;

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}


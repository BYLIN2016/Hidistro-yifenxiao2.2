namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Subsites.Utility;
    using System;

    public class AuthorizeProductLines : DistributorPage
    {
        protected Grid grdProductLine;

        private void BindData()
        {
            this.grdProductLine.DataSource = SubSiteProducthelper.GetAuthorizeProductLines();
            this.grdProductLine.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    [PrivilegeCheck(Privilege.SaleTargets)]
    public class SaleTargets : AdminPage
    {
        protected Grid grdOrderAvPrice;
        protected Grid grdOrderTranslatePercentage;
        protected Grid grdUserOrderAvNumb;
        protected Grid grdUserOrderPercentage;
        protected Grid grdVisitOrderAvPrice;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                DbQueryResult saleTargets = SalesHelper.GetSaleTargets();
                this.grdOrderAvPrice.DataSource = saleTargets.Data;
                this.grdOrderAvPrice.DataBind();
                this.grdOrderTranslatePercentage.DataSource = saleTargets.Data;
                this.grdOrderTranslatePercentage.DataBind();
                this.grdUserOrderAvNumb.DataSource = saleTargets.Data;
                this.grdUserOrderAvNumb.DataBind();
                this.grdVisitOrderAvPrice.DataSource = saleTargets.Data;
                this.grdVisitOrderAvPrice.DataBind();
                this.grdUserOrderPercentage.DataSource = saleTargets.Data;
                this.grdUserOrderPercentage.DataBind();
            }
        }
    }
}


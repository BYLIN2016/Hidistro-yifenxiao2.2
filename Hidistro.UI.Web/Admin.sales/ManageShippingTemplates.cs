namespace Hidistro.UI.Web.Admin.sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ShippingTemplets)]
    public class ManageShippingTemplates : AdminPage
    {
        protected Grid grdShippingTemplates;
        protected Pager pager;

        private void BindShippingTemplates()
        {
            Pagination pagin = new Pagination();
            pagin.PageIndex = this.pager.PageIndex;
            pagin.PageSize = this.pager.PageSize;
            pagin.IsCount = true;
            pagin.SortBy = "TemplateId";
            pagin.SortOrder = SortAction.Desc;
            DbQueryResult shippingTemplates = SalesHelper.GetShippingTemplates(pagin);
            this.grdShippingTemplates.DataSource = shippingTemplates.Data;
            this.grdShippingTemplates.DataBind();
            this.pager.TotalRecords = shippingTemplates.TotalRecords;
        }

        private void grdShippingTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int result = 0;
            if (e.CommandName == "DEL_Template")
            {
                int.TryParse(e.CommandArgument.ToString(), out result);
                if (result > 0)
                {
                    SalesHelper.DeleteShippingTemplate(result);
                    this.BindShippingTemplates();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdShippingTemplates.RowCommand += new GridViewCommandEventHandler(this.grdShippingTemplates_RowCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindShippingTemplates();
            }
        }
    }
}


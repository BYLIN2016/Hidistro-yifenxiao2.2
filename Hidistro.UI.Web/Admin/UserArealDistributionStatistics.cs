namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MemberArealDistributionStatistics)]
    public class UserArealDistributionStatistics : AdminPage
    {
        protected Grid grdUserStatistics;

        private void BindUserStatistics()
        {
            int totalProductSaleVisits = 0;
            Pagination page = new Pagination();
            page.SortBy = this.grdUserStatistics.SortOrderBy;
            if (this.grdUserStatistics.SortOrder.ToLower() == "desc")
            {
                page.SortOrder = SortAction.Desc;
            }
            this.grdUserStatistics.DataSource = SalesHelper.GetUserStatistics(page, out totalProductSaleVisits);
            this.grdUserStatistics.DataBind();
        }

        private void grdUserStatistics_ReBindData(object sender)
        {
            this.BindUserStatistics();
        }

        private void grdUserStatistics_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int currentRegionId = int.Parse(this.grdUserStatistics.DataKeys[e.Row.RowIndex].Value.ToString(), NumberStyles.None);
                Label label = (Label) e.Row.FindControl("lblReionName");
                if ((currentRegionId != 0) && (label != null))
                {
                    label.Text = RegionHelper.GetFullRegion(currentRegionId, "");
                }
                if ((currentRegionId == 0) && (label != null))
                {
                    label.Text = "其它";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdUserStatistics.RowDataBound += new GridViewRowEventHandler(this.grdUserStatistics_RowDataBound);
            this.grdUserStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdUserStatistics_ReBindData);
            this.grdUserStatistics.RowDataBound += new GridViewRowEventHandler(this.grdUserStatistics_RowDataBound);
            if (!base.IsPostBack)
            {
                this.BindUserStatistics();
            }
        }
    }
}


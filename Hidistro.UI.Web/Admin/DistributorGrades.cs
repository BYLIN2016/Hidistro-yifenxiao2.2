namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorGrades)]
    public class DistributorGrades : AdminPage
    {
        protected Grid grdDistributorRankList;

        private void BindDistributorRanks()
        {
            this.grdDistributorRankList.DataSource = DistributorHelper.GetDistributorGrades();
            this.grdDistributorRankList.DataBind();
        }

        private void grdDistributorRankList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int gradeId = (int) this.grdDistributorRankList.DataKeys[e.RowIndex].Value;
            if (DistributorHelper.DeleteDistributorGrade(gradeId))
            {
                this.BindDistributorRanks();
                this.ShowMsg("已经成功删除选择的分销商等级", true);
            }
            else
            {
                this.ShowMsg("不能删除有分销商的分销商等级", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdDistributorRankList.RowDeleting += new GridViewDeleteEventHandler(this.grdDistributorRankList_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDistributorRanks();
            }
        }
    }
}


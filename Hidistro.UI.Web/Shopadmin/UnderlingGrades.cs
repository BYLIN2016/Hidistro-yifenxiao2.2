namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UnderlingGrades : DistributorPage
    {
        protected Grid grdUnderlingGrades;

        private void BindUnderlingGrades()
        {
            this.grdUnderlingGrades.DataSource = UnderlingHelper.GetUnderlingGrades();
            this.grdUnderlingGrades.DataBind();
        }

        private void grdUnderlingGrades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
                int gradeId = (int) this.grdUnderlingGrades.DataKeys[namingContainer.RowIndex].Value;
                UnderlingHelper.SetDefalutUnderlingGrade(gradeId);
                this.BindUnderlingGrades();
            }
        }

        private void grdUnderlingGrades_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int gradeId = (int) this.grdUnderlingGrades.DataKeys[e.RowIndex].Value;
            if (UnderlingHelper.DeleteUnderlingGrade(gradeId))
            {
                this.BindUnderlingGrades();
                this.ShowMsg("已经成功删除选择的会员等级", true);
            }
            else
            {
                this.ShowMsg("不能删除默认的会员等级或有会员的等级", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdUnderlingGrades.RowDeleting += new GridViewDeleteEventHandler(this.grdUnderlingGrades_RowDeleting);
            this.grdUnderlingGrades.RowCommand += new GridViewCommandEventHandler(this.grdUnderlingGrades_RowCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindUnderlingGrades();
            }
        }
    }
}


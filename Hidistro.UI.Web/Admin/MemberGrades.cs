namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MemberGrades)]
    public class MemberGrades : AdminPage
    {
        protected Grid grdMemberRankList;

        private void BindMemberRanks()
        {
            this.grdMemberRankList.DataSource = MemberHelper.GetMemberGrades();
            this.grdMemberRankList.DataBind();
        }

        private void grdMemberRankList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
                int gradeId = (int) this.grdMemberRankList.DataKeys[namingContainer.RowIndex].Value;
                if (!MemberHelper.GetMemberGrade(gradeId).IsDefault)
                {
                    MemberHelper.SetDefalutMemberGrade(gradeId);
                    this.BindMemberRanks();
                }
            }
        }

        private void grdMemberRankList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int gradeId = (int) this.grdMemberRankList.DataKeys[e.RowIndex].Value;
            if (MemberHelper.DeleteMemberGrade(gradeId))
            {
                this.BindMemberRanks();
                this.ShowMsg("已经成功删除选择的会员等级", true);
            }
            else
            {
                this.ShowMsg("不能删除默认的会员等级或有会员的等级", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdMemberRankList.RowDeleting += new GridViewDeleteEventHandler(this.grdMemberRankList_RowDeleting);
            this.grdMemberRankList.RowCommand += new GridViewCommandEventHandler(this.grdMemberRankList_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindMemberRanks();
            }
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditDistributor)]
    public class EditDistributorSettings : AdminPage
    {
        protected Button btnEditDistributorSettings;
        protected ProductLineCheckBoxList chkListProductLine;
        protected DistributorGradeDropDownList drpDistributorGrade;
        protected Literal litUserName;
        protected TextBox txtRemark;
        protected HtmlGenericControl txtRemarkTip;
        private int userId;
        protected Hidistro.UI.Common.Controls.WangWangConversations WangWangConversations;

        private void btnEditDistributorSettings_Click(object sender, EventArgs e)
        {
            if (this.txtRemark.Text.Trim().Length > 300)
            {
                this.ShowMsg("合作备忘录的长度限制在300个字符以内", false);
                this.chkListProductLine.DataBind();
                this.LoadControl();
            }
            else if (this.chkListProductLine.SelectedValue.Count == 0)
            {
                this.ShowMsg("请选择至少一个授权产品线", false);
                this.chkListProductLine.DataBind();
                this.LoadControl();
            }
            else if (DistributorHelper.UpdateDistributorSettings(this.userId, this.drpDistributorGrade.SelectedValue.Value, this.txtRemark.Text.Trim()))
            {
                if (DistributorHelper.AddDistributorProductLines(this.userId, this.chkListProductLine.SelectedValue))
                {
                    ProductHelper.DeleteNotinProductLines(this.userId);
                    this.ShowMsg("成功的修改了分销商基本设置", true);
                }
            }
            else
            {
                this.ShowMsg("修改失败", false);
            }
        }

        private void LoadControl()
        {
            Distributor distributor = DistributorHelper.GetDistributor(this.userId);
            if (distributor == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.litUserName.Text = distributor.Username;
                this.WangWangConversations.WangWangAccounts = distributor.Wangwang;
                this.drpDistributorGrade.SelectedValue = new int?(distributor.GradeId);
                this.txtRemark.Text = distributor.Remark;
                IList<int> distributorProductLines = DistributorHelper.GetDistributorProductLines(this.userId);
                this.chkListProductLine.SelectedValue = distributorProductLines;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnEditDistributorSettings.Click += new EventHandler(this.btnEditDistributorSettings_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                this.drpDistributorGrade.AllowNull = false;
                this.drpDistributorGrade.DataBind();
                this.chkListProductLine.DataBind();
                this.LoadControl();
            }
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorRequests)]
    public class AcceptDistributorRequest : AdminPage
    {
        protected Button btnAddDistrbutor;
        protected ProductLineCheckBoxList chklProductLine;
        protected DistributorGradeDropDownList dropDistributorGrade;
        protected Literal litName;
        protected TextBox txtRemark;
        private int userId;

        private void btnAddDistrbutor_Click(object sender, EventArgs e)
        {
            if (this.chklProductLine.SelectedValue.Count == 0)
            {
                this.ShowMsg("至少选择一个产品线", false);
            }
            else
            {
                Distributor distributor = DistributorHelper.GetDistributor(this.userId);
                distributor.GradeId = this.dropDistributorGrade.SelectedValue.Value;
                distributor.Remark = this.txtRemark.Text;
                distributor.IsApproved = true;
                if (this.ValidationDistributor(distributor) && DistributorHelper.AcceptDistributorRequest(distributor, this.chklProductLine.SelectedValue))
                {
                    Messenger.AcceptRequest(distributor);
                    this.CloseWindow();
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAddDistrbutor.Click += new EventHandler(this.btnAddDistrbutor_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!this.Page.IsPostBack)
            {
                Distributor distributor = DistributorHelper.GetDistributor(this.userId);
                if (distributor == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.litName.Text = distributor.Username;
                    this.dropDistributorGrade.AllowNull = false;
                    this.dropDistributorGrade.DataBind();
                    this.chklProductLine.DataBind();
                }
            }
        }

        private bool ValidationDistributor(Distributor distributor)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Distributor>(distributor, new string[] { "ValDistributor" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditDistributorGrade)]
    public class EditDistributorGrades : AdminPage
    {
        protected Button btnEditDistrbutor;
        private int gradeId;
        protected TextBox txtRankDesc;
        protected HtmlGenericControl txtRankDescTip;
        protected TextBox txtRankName;
        protected HtmlGenericControl txtRankNameTip;
        protected TextBox txtValue;
        protected HtmlGenericControl txtValueTip;

        private void btnEditDistrbutor_Click(object sender, EventArgs e)
        {
            int num;
            DistributorGradeInfo distributorGrade = new DistributorGradeInfo();
            distributorGrade.Name = this.txtRankName.Text.Trim();
            distributorGrade.Description = this.txtRankDesc.Text.Trim();
            distributorGrade.GradeId = this.gradeId;
            if (int.TryParse(this.txtValue.Text, out num) && !this.txtValue.Text.Contains("."))
            {
                distributorGrade.Discount = num;
            }
            else
            {
                this.ShowMsg("等级折扣必须为正整数", false);
                return;
            }
            if (this.ValidationMemberGrade(distributorGrade))
            {
                if (DistributorHelper.UpdateDistributorGrade(distributorGrade))
                {
                    this.ShowMsg("修改分销商等级成功", true);
                }
                else
                {
                    this.ShowMsg("修改分销商等级失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnEditDistrbutor.Click += new EventHandler(this.btnEditDistrbutor_Click);
            if (!int.TryParse(base.Request.QueryString["GradeId"], out this.gradeId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                DistributorGradeInfo distributorGradeInfo = DistributorHelper.GetDistributorGradeInfo(this.gradeId);
                if (distributorGradeInfo == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(distributorGradeInfo, false);
                    this.txtRankName.Text = distributorGradeInfo.Name;
                    this.txtValue.Text = distributorGradeInfo.Discount.ToString();
                    this.txtRankDesc.Text = distributorGradeInfo.Description;
                }
            }
        }

        private bool ValidationMemberGrade(DistributorGradeInfo distributorGrade)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<DistributorGradeInfo>(distributorGrade, new string[] { "ValDistributorGrade" });
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


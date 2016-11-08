namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddDistributorGrade)]
    public class AddDistributorGrades : AdminPage
    {
        protected Button btnAddDistrbutor;
        protected TextBox txtRankDesc;
        protected HtmlGenericControl txtRankDescTip;
        protected TextBox txtRankName;
        protected HtmlGenericControl txtRankNameTip;
        protected TextBox txtValue;
        protected HtmlGenericControl txtValueTip;

        private void btnAddDistrbutor_Click(object sender, EventArgs e)
        {
            if (DistributorHelper.ExistGradeName(this.txtRankName.Text.Trim()))
            {
                this.ShowMsg("已经存在相同名称的分销商等级", false);
            }
            else
            {
                int num;
                DistributorGradeInfo distributorGrade = new DistributorGradeInfo();
                distributorGrade.Name = this.txtRankName.Text.Trim();
                distributorGrade.Description = this.txtRankDesc.Text.Trim();
                if (int.TryParse(this.txtValue.Text, out num) && !this.txtValue.Text.Contains("."))
                {
                    distributorGrade.Discount = num;
                }
                else
                {
                    this.ShowMsg("等级折扣必须只能为正整数", false);
                    return;
                }
                if (this.ValidationMemberGrade(distributorGrade))
                {
                    if (DistributorHelper.AddDistributorGrade(distributorGrade))
                    {
                        this.ResetText();
                        this.ShowMsg("成功添加了一个分销商等级", true);
                    }
                    else
                    {
                        this.ShowMsg("添加分销商等级失败", false);
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAddDistrbutor.Click += new EventHandler(this.btnAddDistrbutor_Click);
        }

        private void ResetText()
        {
            this.txtRankName.Text = string.Empty;
            this.txtRankDesc.Text = string.Empty;
            this.txtValue.Text = string.Empty;
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


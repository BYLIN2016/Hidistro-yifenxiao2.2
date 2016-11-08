namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Members;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class AddUnderlingGrade : DistributorPage
    {
        protected Button btnSubmitMemberRanks;
        protected YesNoRadioButtonList chkIsDefault;
        protected TextBox txtPoint;
        protected TextBox txtRankDesc;
        protected TextBox txtRankName;
        protected TextBox txtValue;

        private void btnSubmitMemberRanks_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (this.txtValue.Text.Trim().Contains("."))
            {
                this.ShowMsg("折扣必须为正整数", false);
            }
            else
            {
                int num;
                int num2;
                MemberGradeInfo memberGrade = new MemberGradeInfo();
                memberGrade.Name = this.txtRankName.Text.Trim();
                memberGrade.Description = this.txtRankDesc.Text.Trim();
                if (int.TryParse(this.txtPoint.Text.Trim(), out num))
                {
                    memberGrade.Points = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("积分设置无效或不能为空，必须大于等于0的整数");
                }
                memberGrade.IsDefault = this.chkIsDefault.SelectedValue;
                if (int.TryParse(this.txtValue.Text, out num2))
                {
                    memberGrade.Discount = num2;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("等级折扣设置无效或不能为空，等级折扣必须在1-100之间");
                }
                if (!string.IsNullOrEmpty(str))
                {
                    this.ShowMsg(str, false);
                }
                else if (this.ValidationMemberGrade(memberGrade))
                {
                    if (UnderlingHelper.HasSamePointMemberGrade(memberGrade))
                    {
                        this.ShowMsg("已经存在相同积分的等级，每个会员等级的积分不能相同", false);
                    }
                    else if (UnderlingHelper.CreateUnderlingGrade(memberGrade))
                    {
                        this.ShowMsg("成功添加了一个会员等级", true);
                    }
                    else
                    {
                        this.ShowMsg("添加会员等级失败", false);
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSubmitMemberRanks.Click += new EventHandler(this.btnSubmitMemberRanks_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private bool ValidationMemberGrade(MemberGradeInfo memberGrade)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<MemberGradeInfo>(memberGrade, new string[] { "ValMemberGrade" });
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


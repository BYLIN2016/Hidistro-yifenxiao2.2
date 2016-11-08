namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditUnderling : DistributorPage
    {
        protected Button btnEditUser;
        protected WebCalendar calBirthday;
        private int currentUserId;
        protected ApprovedDropDownList ddlApproved;
        protected UnderlingGradeDropDownList drpMemberRankList;
        protected GenderRadioButtonList gender;
        protected FormatedTimeLabel lblLastLoginTimeValue;
        protected Literal lblLoginNameValue;
        protected FormatedTimeLabel lblRegsTimeValue;
        protected Literal lblTotalAmountValue;
        protected RegionSelector rsddlRegion;
        protected TextBox txtAddress;
        protected HtmlGenericControl txtAddressTip;
        protected TextBox txtCellPhone;
        protected HtmlGenericControl txtCellPhoneTip;
        protected TextBox txtMSN;
        protected HtmlGenericControl txtMSNTip;
        protected TextBox txtprivateEmail;
        protected HtmlGenericControl txtprivateEmailTip;
        protected TextBox txtQQ;
        protected HtmlGenericControl txtQQTip;
        protected TextBox txtRealName;
        protected HtmlGenericControl txtRealNameTip;
        protected TextBox txtTel;
        protected HtmlGenericControl txtTelTip;
        protected TextBox txtWangwang;
        protected HtmlGenericControl txtWangwangTip;

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            Member member = UnderlingHelper.GetMember(this.currentUserId);
            member.IsApproved = this.ddlApproved.SelectedValue.Value;
            member.GradeId = this.drpMemberRankList.SelectedValue.Value;
            member.Wangwang = Globals.HtmlEncode(this.txtWangwang.Text.Trim());
            member.RealName = this.txtRealName.Text.Trim();
            if (this.rsddlRegion.GetSelectedRegionId().HasValue)
            {
                member.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
            }
            member.Address = Globals.HtmlEncode(this.txtAddress.Text);
            member.QQ = this.txtQQ.Text;
            member.MSN = this.txtMSN.Text;
            member.TelPhone = this.txtTel.Text;
            member.CellPhone = this.txtCellPhone.Text;
            if (this.calBirthday.SelectedDate.HasValue)
            {
                member.BirthDate = new DateTime?(this.calBirthday.SelectedDate.Value);
            }
            member.Email = this.txtprivateEmail.Text;
            member.Gender = this.gender.SelectedValue;
            if (this.ValidationMember(member))
            {
                if (UnderlingHelper.Update(member))
                {
                    this.ShowMsg("成功修改了当前会员的个人资料", true);
                }
                else
                {
                    this.ShowMsg("当前会员的个人信息修改失败", false);
                }
            }
        }

        private void LoadMemberInfo()
        {
            Member member = UnderlingHelper.GetMember(this.currentUserId);
            if (member == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.drpMemberRankList.SelectedValue = new int?(member.GradeId);
                this.lblLoginNameValue.Text = member.Username;
                this.lblRegsTimeValue.Time = member.CreateDate;
                this.lblLastLoginTimeValue.Time = member.LastLoginDate;
                this.lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
                this.txtRealName.Text = member.RealName;
                this.calBirthday.SelectedDate = member.BirthDate;
                this.rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
                this.txtAddress.Text = Globals.HtmlDecode(member.Address);
                this.txtQQ.Text = member.QQ;
                this.txtMSN.Text = member.MSN;
                this.txtTel.Text = member.TelPhone;
                this.txtCellPhone.Text = member.CellPhone;
                this.txtprivateEmail.Text = member.Email;
                this.gender.SelectedValue = member.Gender;
                this.ddlApproved.SelectedValue = new bool?(member.IsApproved);
                this.txtWangwang.Text = Globals.HtmlDecode(member.Wangwang);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditUser.Click += new EventHandler(this.btnEditUser_Click);
                if (!this.Page.IsPostBack)
                {
                    this.ddlApproved.DataBind();
                    this.drpMemberRankList.AllowNull = false;
                    this.drpMemberRankList.DataBind();
                    this.LoadMemberInfo();
                }
            }
        }

        private bool ValidationMember(Member member)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Member>(member, new string[] { "ValMember" });
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


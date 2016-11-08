namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditMember)]
    public class EditMember : AdminPage
    {
        protected Button btnEditUser;
        protected WebCalendar calBirthday;
        private int currentUserId;
        protected ApprovedDropDownList ddlApproved;
        protected MemberGradeDropDownList drpMemberRankList;
        protected GenderRadioButtonList gender;
        protected FormatedTimeLabel lblLastLoginTimeValue;
        protected Literal lblLoginNameValue;
        protected FormatedTimeLabel lblRegsTimeValue;
        protected Literal lblTotalAmountValue;
        protected RegionSelector rsddlRegion;
        protected TextBox txtAddress;
        protected TextBox txtCellPhone;
        protected TextBox txtMSN;
        protected TextBox txtprivateEmail;
        protected TextBox txtQQ;
        protected TextBox txtRealName;
        protected TextBox txtTel;
        protected TextBox txtWangwang;

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            Member member = MemberHelper.GetMember(this.currentUserId);
            member.IsApproved = this.ddlApproved.SelectedValue.Value;
            member.Wangwang = Globals.HtmlEncode(this.txtWangwang.Text.Trim());
            member.GradeId = this.drpMemberRankList.SelectedValue.Value;
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
                if (MemberHelper.Update(member))
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
            Member member = MemberHelper.GetMember(this.currentUserId);
            if (member == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.ddlApproved.SelectedValue = new bool?(member.IsApproved);
                this.drpMemberRankList.SelectedValue = new int?(member.GradeId);
                this.lblLoginNameValue.Text = member.Username;
                this.lblRegsTimeValue.Time = member.CreateDate;
                this.lblLastLoginTimeValue.Time = member.LastLoginDate;
                this.lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
                this.txtRealName.Text = member.RealName;
                this.calBirthday.SelectedDate = member.BirthDate;
                this.txtAddress.Text = Globals.HtmlDecode(member.Address);
                this.rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
                this.txtQQ.Text = member.QQ;
                this.txtMSN.Text = member.MSN;
                this.txtTel.Text = member.TelPhone;
                this.txtCellPhone.Text = member.CellPhone;
                this.txtprivateEmail.Text = member.Email;
                this.gender.SelectedValue = member.Gender;
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
                    this.drpMemberRankList.AllowNull = false;
                    this.drpMemberRankList.DataBind();
                    this.ddlApproved.AllowNull = false;
                    this.ddlApproved.DataBind();
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


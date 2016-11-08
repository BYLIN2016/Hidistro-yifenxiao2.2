namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class UserProfile : MemberTemplatedWebControl
    {
        private IButton btnOK1;
        private WebCalendar calendDate;
        private RegionSelector dropRegionsSelect;
        private GenderRadioButtonList gender;
        private SmallStatusMessage Statuses;
        private TextBox txtAddress;
        private TextBox txtEmail;
        private TextBox txtHandSet;
        private TextBox txtMSN;
        private TextBox txtQQ;
        private TextBox txtRealName;
        private TextBox txtTel;

        protected override void AttachChildControls()
        {
            this.txtRealName = (TextBox) this.FindControl("txtRealName");
            this.txtEmail = (TextBox) this.FindControl("txtEmail");
            this.dropRegionsSelect = (RegionSelector) this.FindControl("dropRegions");
            this.gender = (GenderRadioButtonList) this.FindControl("gender");
            this.calendDate = (WebCalendar) this.FindControl("calendDate");
            this.txtAddress = (TextBox) this.FindControl("txtAddress");
            this.txtQQ = (TextBox) this.FindControl("txtQQ");
            this.txtMSN = (TextBox) this.FindControl("txtMSN");
            this.txtTel = (TextBox) this.FindControl("txtTel");
            this.txtHandSet = (TextBox) this.FindControl("txtHandSet");
            this.btnOK1 = ButtonManager.Create(this.FindControl("btnOK1"));
            this.Statuses = (SmallStatusMessage) this.FindControl("Statuses");
            this.btnOK1.Click += new EventHandler(this.btnOK1_Click);
            PageTitle.AddSiteNameTitle("个人信息", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                Member user = HiContext.Current.User as Member;
                if (user != null)
                {
                    this.BindData(user);
                }
            }
        }

        private void BindData(Member user)
        {
            this.txtRealName.Text = Globals.HtmlDecode(user.RealName);
            this.txtEmail.Text = Globals.HtmlDecode(user.Email);
            this.gender.SelectedValue = user.Gender;
            DateTime? birthDate = user.BirthDate;
            DateTime minValue = DateTime.MinValue;
            if (birthDate.HasValue ? (birthDate.GetValueOrDefault() > minValue) : false)
            {
                this.calendDate.SelectedDate = user.BirthDate;
            }
            this.dropRegionsSelect.SetSelectedRegionId(new int?(user.RegionId));
            this.txtAddress.Text = Globals.HtmlDecode(user.Address);
            this.txtQQ.Text = Globals.HtmlDecode(user.QQ);
            this.txtMSN.Text = Globals.HtmlDecode(user.MSN);
            this.txtTel.Text = Globals.HtmlDecode(user.TelPhone);
            this.txtHandSet.Text = Globals.HtmlDecode(user.CellPhone);
        }

        private void btnOK1_Click(object sender, EventArgs e)
        {
            Member user = Users.GetUser(HiContext.Current.User.UserId, true) as Member;
            if (string.IsNullOrEmpty(this.txtEmail.Text))
            {
                this.ShowMessage("邮箱不能为空", false);
            }
            else
            {
                user.RealName = Globals.HtmlEncode(this.txtRealName.Text);
                user.Email = Globals.HtmlEncode(this.txtEmail.Text);
                if (!this.dropRegionsSelect.GetSelectedRegionId().HasValue)
                {
                    user.RegionId = 0;
                }
                else
                {
                    user.RegionId = this.dropRegionsSelect.GetSelectedRegionId().Value;
                    user.TopRegionId = RegionHelper.GetTopRegionId(user.RegionId);
                }
                user.Gender = this.gender.SelectedValue;
                user.BirthDate = this.calendDate.SelectedDate;
                user.Address = Globals.HtmlEncode(this.txtAddress.Text);
                user.QQ = Globals.HtmlEncode(this.txtQQ.Text);
                user.MSN = Globals.HtmlEncode(this.txtMSN.Text);
                user.TelPhone = Globals.HtmlEncode(this.txtTel.Text);
                user.CellPhone = Globals.HtmlEncode(this.txtHandSet.Text);
                if (this.ValidationMember(user))
                {
                    if (Users.UpdateUser(user))
                    {
                        this.ShowMessage("成功的修改了用户的个人资料", true);
                    }
                    else
                    {
                        this.ShowMessage("修改用户个人资料失败", false);
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserProfile.html";
            }
            base.OnInit(e);
        }

        protected virtual void ShowMsgs(SmallStatusMessage state, string msg, bool success)
        {
            if (state != null)
            {
                state.Success = success;
                state.Text = msg;
                state.Visible = true;
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
                this.ShowMessage(msg, false);
            }
            return results.IsValid;
        }
    }
}


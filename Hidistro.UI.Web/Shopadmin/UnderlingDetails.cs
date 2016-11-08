namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class UnderlingDetails : DistributorPage
    {
        protected Button btnEdit;
        private int currentUserId;
        protected Literal lblUserLink;
        protected Literal litAddress;
        protected Literal litBirthDate;
        protected Literal litCellPhone;
        protected Literal litCreateDate;
        protected Literal litEmail;
        protected Literal litGender;
        protected Literal litGrade;
        protected Literal litIsApproved;
        protected Literal litLastLoginDate;
        protected Literal litMSN;
        protected Literal litQQ;
        protected Literal litRealName;
        protected Literal litTelPhone;
        protected Literal litUserName;
        protected Literal litWangwang;

        private void btnEdit_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("EditUnderling.aspx?userId=" + this.Page.Request.QueryString["userId"]);
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
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                this.lblUserLink.Text = string.Concat(new object[] { "http://", siteSettings.SiteUrl, Globals.ApplicationPath, "/?ReferralUserId=", member.UserId });
                this.litUserName.Text = member.Username;
                this.litIsApproved.Text = member.IsApproved ? "通过" : "禁止";
                this.litGrade.Text = UnderlingHelper.GetMemberGrade(member.GradeId).Name;
                this.litCreateDate.Text = member.CreateDate.ToString();
                this.litLastLoginDate.Text = member.LastLoginDate.ToString();
                this.litRealName.Text = member.RealName;
                this.litBirthDate.Text = member.BirthDate.ToString();
                this.litAddress.Text = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
                this.litQQ.Text = member.QQ;
                this.litMSN.Text = member.MSN;
                this.litTelPhone.Text = member.TelPhone;
                this.litCellPhone.Text = member.CellPhone;
                this.litEmail.Text = member.Email;
                if (member.Gender == Gender.Female)
                {
                    this.litGender.Text = "女";
                }
                else if (member.Gender == Gender.Male)
                {
                    this.litGender.Text = "男";
                }
                else
                {
                    this.litGender.Text = "保密";
                }
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
                this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
                if (!this.Page.IsPostBack)
                {
                    this.LoadMemberInfo();
                }
            }
        }
    }
}


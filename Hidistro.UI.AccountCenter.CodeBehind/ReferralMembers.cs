namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class ReferralMembers : MemberTemplatedWebControl
    {
        private IButton btnSearchButton;
        private Common_Referral_MemberList grdReferralmembers;
        private Pager pager;
        private TextBox txtSearchText;

        protected override void AttachChildControls()
        {
            this.txtSearchText = (TextBox) this.FindControl("txtSearchText");
            this.btnSearchButton = ButtonManager.Create(this.FindControl("btnSearchButton"));
            this.grdReferralmembers = (Common_Referral_MemberList) this.FindControl("Common_Referral_MemberList");
            this.pager = (Pager) this.FindControl("pager");
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            if (!this.Page.IsPostBack)
            {
                PageTitle.AddSiteNameTitle("会员中心首页", HiContext.Current.Context);
                MemberQuery query = new MemberQuery();
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["username"]))
                {
                    query.Username = this.Page.Server.UrlDecode(this.Page.Request.QueryString["username"]);
                }
                query.PageIndex = this.pager.PageIndex;
                query.PageSize = this.pager.PageSize;
                DbQueryResult myReferralMembers = PersonalHelper.GetMyReferralMembers(query);
                this.grdReferralmembers.DataSource = myReferralMembers.Data;
                this.grdReferralmembers.DataBind();
                this.txtSearchText.Text = query.Username;
                this.pager.TotalRecords = myReferralMembers.TotalRecords;
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadReferralMembers(true);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-ReferralMembers.html";
            }
            base.OnInit(e);
        }

        private void ReloadReferralMembers(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("username", this.txtSearchText.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}


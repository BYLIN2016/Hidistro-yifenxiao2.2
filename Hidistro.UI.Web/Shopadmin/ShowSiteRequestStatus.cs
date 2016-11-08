namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Distribution;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ShowSiteRequestStatus : DistributorPage
    {
        protected Button btnRequestAgain;
        protected HtmlGenericControl divRequestAgain;
        protected HtmlGenericControl liFail;
        protected HtmlGenericControl liSuccess;
        protected Literal litFirstUrl;
        protected Literal litRefuseReason;
        protected HtmlGenericControl liWait;
        protected TextBox txtFirstSiteUrl;
        protected HtmlGenericControl txtFirstSiteUrlTip;

        private void btnRequestAgain_Click(object sender, EventArgs e)
        {
            SubsiteStoreHelper.DeleteSiteRequest();
            SiteRequestInfo target = new SiteRequestInfo();
            target.FirstSiteUrl = this.txtFirstSiteUrl.Text.Trim();
            target.RequestTime = DateTime.Now;
            target.RequestStatus = SiteRequestStatus.Dealing;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteRequestInfo>(target, new string[] { "ValSiteRequest" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else
            {
                SiteRequestInfo mySiteRequest = SubsiteStoreHelper.GetMySiteRequest();
                if ((mySiteRequest != null) && (mySiteRequest.RequestStatus == SiteRequestStatus.Dealing))
                {
                    this.ShowMsg("你上一条申请还未处理，请联系供应商", false);
                }
                else if (SubsiteStoreHelper.AddSiteRequest(target))
                {
                    base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ShowSiteRequestStatus.aspx");
                }
                else
                {
                    this.ShowMsg("站点申请提交失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnRequestAgain.Click += new EventHandler(this.btnRequestAgain_Click);
            if (!this.Page.IsPostBack)
            {
                SiteRequestInfo mySiteRequest = SubsiteStoreHelper.GetMySiteRequest();
                if (mySiteRequest == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.litFirstUrl.Text = mySiteRequest.FirstSiteUrl;
                    this.litRefuseReason.Text = mySiteRequest.RefuseReason;
                    if (mySiteRequest.RequestStatus == SiteRequestStatus.Dealing)
                    {
                        this.liWait.Visible = true;
                    }
                    else if (mySiteRequest.RequestStatus == SiteRequestStatus.Fail)
                    {
                        this.liFail.Visible = true;
                        this.divRequestAgain.Visible = true;
                    }
                    else if (mySiteRequest.RequestStatus == SiteRequestStatus.Success)
                    {
                        this.liSuccess.Visible = true;
                    }
                }
            }
        }
    }
}


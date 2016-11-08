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

    public class SiteRequest : DistributorPage
    {
        protected Button btnAddRequest;
        protected Literal litServerIp;
        protected TextBox txtFirstSiteUrl;
        protected HtmlGenericControl txtFirstSiteUrlTip;

        private void btnAddRequest_Click(object sender, EventArgs e)
        {
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
                    this.ShowMsg("您上一条申请还未处理，请联系供应商", false);
                }
                else if (SubsiteStoreHelper.IsExitSiteUrl(target.FirstSiteUrl))
                {
                    this.ShowMsg("您输入的域名已经被其它分销商绑定了，请重新输入", false);
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
            this.btnAddRequest.Click += new EventHandler(this.btnAddRequest_Click);
            if (!base.IsPostBack)
            {
                this.ProcessRequestStatus();
                this.litServerIp.Text = base.Request.ServerVariables.Get("Local_Addr").ToString();
            }
        }

        private void ProcessRequestStatus()
        {
            if (SubsiteStoreHelper.GetMySiteRequest() != null)
            {
                base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ShowSiteRequestStatus.aspx");
            }
        }
    }
}


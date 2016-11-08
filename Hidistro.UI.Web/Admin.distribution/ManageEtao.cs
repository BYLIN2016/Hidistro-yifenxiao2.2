namespace Hidistro.UI.Web.Admin.distribution
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ManageDistributorSites)]
    public class ManageEtao : AdminPage
    {
        protected Button btnSearch;
        protected Grid grdDistributorSites;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlGenericControl spanDistributorName;
        private string trueName;
        protected TextBox txtDistributorName;
        protected TextBox txtTrueName;
        private string userName;

        private void BindRequests()
        {
            Pagination pagination = new Pagination();
            pagination.PageIndex = this.pager.PageIndex;
            pagination.PageSize = this.pager.PageSize;
            int total = 0;
            DataTable table = DistributorHelper.GetEtaoSites(pagination, this.userName, this.trueName, out total);
            this.grdDistributorSites.DataSource = table;
            this.grdDistributorSites.DataBind();
            this.pager.TotalRecords = total;
            this.pager1.TotalRecords = total;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        protected void grdDistributorSites_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "open")
            {
                int num = Convert.ToInt32(e.CommandArgument);
                int userId = Convert.ToInt32(this.grdDistributorSites.DataKeys[num].Value);
                Literal literal = (Literal) this.grdDistributorSites.Rows[num].FindControl("litState");
                if (literal.Text == "开启")
                {
                    if (!DistributorHelper.CloseEtao(userId))
                    {
                        this.ShowMsg("暂停失败", false);
                    }
                }
                else if (!DistributorHelper.OpenEtao(userId))
                {
                    this.ShowMsg("开启失败", false);
                }
            }
            this.BindRequests();
        }

        protected void grdDistributorSites_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = (Literal) e.Row.FindControl("litState");
                ImageLinkButton button = (ImageLinkButton) e.Row.FindControl("btnIsOpen");
                button.CommandArgument = e.Row.RowIndex.ToString();
                if (literal.Text == "True")
                {
                    literal.Text = "开启";
                    button.Text = "暂停";
                    button.DeleteMsg = "暂停后，该分销子站将不能更新一淘Feed";
                }
                else
                {
                    literal.Text = "暂停";
                    button.Text = "开启";
                    button.DeleteMsg = "开启后，该分销子站将可以重新生成一淘Feed，确认要开启吗？";
                }
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = this.Page.Request.QueryString["userName"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["trueName"]))
                {
                    this.trueName = this.Page.Request.QueryString["trueName"];
                }
                this.txtDistributorName.Text = this.userName;
                this.txtTrueName.Text = this.trueName;
            }
            else
            {
                this.userName = this.txtDistributorName.Text.Trim();
                this.trueName = this.txtTrueName.Text.Trim();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["showMessage"]) && (base.Request["showMessage"] == "true"))
            {
                int result = 0;
                if (string.IsNullOrEmpty(base.Request["userId"]) || !int.TryParse(base.Request["userId"], out result))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(result);
                if (siteSettings == null)
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                Distributor distributor = DistributorHelper.GetDistributor(siteSettings.UserId.Value);
                if (distributor == null)
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
                builder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
                builder.AppendFormat(",\"CompanyName\":\"{0}\"", distributor.CompanyName);
                builder.AppendFormat(",\"Email\":\"{0}\"", distributor.Email);
                builder.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor.RegionId, string.Empty));
                builder.AppendFormat(",\"Address\":\"{0}\"", distributor.Address);
                builder.AppendFormat(",\"QQ\":\"{0}\"", distributor.QQ);
                builder.AppendFormat(",\"MSN\":\"{0}\"", distributor.MSN);
                builder.AppendFormat(",\"PostCode\":\"{0}\"", distributor.Zipcode);
                builder.AppendFormat(",\"Wangwang\":\"{0}\"", distributor.Wangwang);
                builder.AppendFormat(",\"CellPhone\":\"{0}\"", distributor.CellPhone);
                builder.AppendFormat(",\"Telephone\":\"{0}\"", distributor.TelPhone);
                builder.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor.CreateDate);
                builder.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor.LastLoginDate);
                builder.AppendFormat(",\"Domain1\":\"{0}\"", siteSettings.SiteUrl);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindRequests();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.txtDistributorName.Text.Trim());
            queryStrings.Add("trueName", this.txtTrueName.Text.Trim());
            queryStrings.Add("pageSize", this.hrefPageSize.SelectedSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}


namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MyBalanceDetails : DistributorPage
    {
        protected Button btnQueryBalanceDetails;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? dataEnd;
        private DateTime? dataStart;
        protected TradeTypeDropDownList dropTradeType;
        protected Grid grdBalanceDetails;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private int typeId;

        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("typeId", ((int) this.dropTradeType.SelectedValue).ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("dataStart", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void GetBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.TradeType = (TradeTypes) this.typeId;
            query.FromDate = this.dataStart;
            query.ToDate = this.dataEnd;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            DbQueryResult myBalanceDetails = SubsiteStoreHelper.GetMyBalanceDetails(query);
            this.grdBalanceDetails.DataSource = myBalanceDetails.Data;
            this.grdBalanceDetails.DataBind();
            this.pager.TotalRecords = myBalanceDetails.TotalRecords;
            this.pager1.TotalRecords = myBalanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
                {
                    int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
                {
                    this.dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
                {
                    this.dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
                }
                this.dropTradeType.DataBind();
                this.dropTradeType.SelectedValue = (TradeTypes) this.typeId;
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
            }
            else
            {
                this.typeId = (int) this.dropTradeType.SelectedValue;
                this.dataStart = this.calendarStart.SelectedDate;
                this.dataEnd = this.calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryBalanceDetails.Click += new EventHandler(this.btnQueryBalanceDetails_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.GetBalanceDetails();
            }
        }
    }
}


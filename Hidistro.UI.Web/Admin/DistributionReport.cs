namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributionReport)]
    public class DistributionReport : AdminPage
    {
        protected LinkButton btnCreateReportOfDay;
        protected LinkButton btnCreateReportOfMonth;
        protected Button btnQueryDaySaleTotal;
        protected Button btnQueryMonthSaleTotal;
        private int dayMonth = DateTime.Now.Month;
        private SaleStatisticsType dayType = SaleStatisticsType.SaleCounts;
        private int dayYear = DateTime.Now.Year;
        protected YearDropDownList dropDayForYear;
        protected YearDropDownList dropMonthForYaer;
        protected MonthDropDownList dropMoth;
        protected GridView grdDayDistributionTotalStatistics;
        protected Grid grdMonthDistributionTotalStatistics;
        protected Label lblDayAllTotal;
        protected Label lblDayMaxTotal;
        protected Label lblMonthAllTotal;
        protected Label lblMonthMaxTotal;
        protected Literal litDayAllTotal;
        protected Literal litDayMaxTotal;
        protected Literal litMonthAllTotal;
        protected Literal litMonthMaxTotal;
        private SaleStatisticsType monthType = SaleStatisticsType.SaleCounts;
        private int monthYear = DateTime.Now.Year;
        protected SaleStatisticsTypeRadioButtonList radioDayForSaleType;
        protected SaleStatisticsTypeRadioButtonList radioMonthForSaleType;

        private void BindDaySaleTotalStatistics()
        {
            DataTable table = DistributorHelper.GetDayDistributionTotal(this.dayYear, this.dayMonth, this.dayType);
            if (this.radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                this.grdDayDistributionTotalStatistics.Columns[1].Visible = true;
                this.grdDayDistributionTotalStatistics.Columns[1].HeaderText = this.radioDayForSaleType.SelectedItem.Text;
                this.grdDayDistributionTotalStatistics.Columns[2].Visible = false;
            }
            else
            {
                this.grdDayDistributionTotalStatistics.Columns[1].Visible = false;
                this.grdDayDistributionTotalStatistics.Columns[2].Visible = true;
                this.grdDayDistributionTotalStatistics.Columns[2].HeaderText = this.radioDayForSaleType.SelectedItem.Text;
            }
            this.grdDayDistributionTotalStatistics.DataSource = table;
            this.grdDayDistributionTotalStatistics.DataBind();
            this.TableOfDay = table;
            this.lblDayAllTotal.Text = string.Format("总{0}：", this.radioDayForSaleType.SelectedItem.Text);
            decimal money = DistributorHelper.GetMonthDistributionTotal(this.dayYear, this.dayMonth, this.dayType);
            if (this.radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                this.litDayAllTotal.Text = money.ToString();
            }
            else
            {
                this.litDayAllTotal.Text = Globals.FormatMoney(money);
            }
            this.lblDayMaxTotal.Text = string.Format("最高峰{0}：", this.radioDayForSaleType.SelectedItem.Text);
            decimal num2 = 0M;
            foreach (DataRow row in table.Rows)
            {
                if (((decimal) row["SaleTotal"]) > num2)
                {
                    num2 = (decimal) row["SaleTotal"];
                }
            }
            if (this.radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                this.litDayMaxTotal.Text = num2.ToString();
            }
            else
            {
                this.litDayMaxTotal.Text = Globals.FormatMoney(num2);
            }
        }

        private void BindMonthSaleTotalStatistics()
        {
            DataTable monthDistributionTotal = DistributorHelper.GetMonthDistributionTotal(this.monthYear, this.monthType);
            if (this.radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                this.grdMonthDistributionTotalStatistics.Columns[1].Visible = true;
                this.grdMonthDistributionTotalStatistics.Columns[1].HeaderText = this.radioMonthForSaleType.SelectedItem.Text;
                this.grdMonthDistributionTotalStatistics.Columns[2].Visible = false;
            }
            else
            {
                this.grdMonthDistributionTotalStatistics.Columns[1].Visible = false;
                this.grdMonthDistributionTotalStatistics.Columns[2].Visible = true;
                this.grdMonthDistributionTotalStatistics.Columns[2].HeaderText = this.radioMonthForSaleType.SelectedItem.Text;
            }
            this.grdMonthDistributionTotalStatistics.DataSource = monthDistributionTotal;
            this.grdMonthDistributionTotalStatistics.DataBind();
            this.TableOfMonth = monthDistributionTotal;
            this.lblMonthAllTotal.Text = string.Format("总{0}：", this.radioMonthForSaleType.SelectedItem.Text);
            decimal yearDistributionTotal = DistributorHelper.GetYearDistributionTotal(this.monthYear, this.monthType);
            if (this.radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                this.litMonthAllTotal.Text = yearDistributionTotal.ToString();
            }
            else
            {
                this.litMonthAllTotal.Text = Globals.FormatMoney(yearDistributionTotal);
            }
            this.lblMonthMaxTotal.Text = string.Format("最高峰{0}：", this.radioMonthForSaleType.SelectedItem.Text);
            decimal money = 0M;
            foreach (DataRow row in monthDistributionTotal.Rows)
            {
                if (((decimal) row["SaleTotal"]) > money)
                {
                    money = (decimal) row["SaleTotal"];
                }
            }
            if (this.radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                this.litMonthMaxTotal.Text = money.ToString();
            }
            else
            {
                this.litMonthMaxTotal.Text = Globals.FormatMoney(money);
            }
        }

        private void btnCreateReportOfDay_Click(object sender, EventArgs e)
        {
            string s = (((((((string.Empty + string.Format("总{0}：", this.radioDayForSaleType.SelectedItem.Text)) + "," + this.litDayAllTotal.Text) + ",\"\"") + "," + string.Format("最高峰{0}：", this.radioDayForSaleType.SelectedItem.Text)) + "," + this.litDayMaxTotal.Text + "\r\n\r\n") + "日期") + "," + this.radioDayForSaleType.SelectedItem.Text) + ",比例\r\n";
            foreach (DataRow row in this.TableOfDay.Rows)
            {
                s = s + row["Date"].ToString();
                s = s + "," + row["SaleTotal"].ToString();
                s = s + "," + row["Percentage"].ToString() + "%\r\n";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=SaleTotalStatistics.csv");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        private void btnCreateReportOfMonth_Click(object sender, EventArgs e)
        {
            string s = (((((((string.Empty + string.Format("总{0}：", this.radioMonthForSaleType.SelectedItem.Text)) + "," + this.litMonthAllTotal.Text) + ",\"\"") + "," + string.Format("最高峰{0}：", this.radioMonthForSaleType.SelectedItem.Text)) + "," + this.litMonthMaxTotal.Text + "\r\n\r\n") + "月份") + "," + this.radioMonthForSaleType.SelectedItem.Text) + ",比例\r\n";
            foreach (DataRow row in this.TableOfMonth.Rows)
            {
                s = s + row["Date"].ToString();
                s = s + "," + row["SaleTotal"].ToString();
                s = s + "," + row["Percentage"].ToString() + "%\r\n";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=SaleTotalStatistics.csv");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        private void btnQueryDaySaleTotal_Click(object sender, EventArgs e)
        {
            this.ReBind();
        }

        private void btnQueryMonthSaleTotal_Click(object sender, EventArgs e)
        {
            this.ReBind();
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["monthYear"]))
                {
                    int.TryParse(this.Page.Request.QueryString["monthYear"], out this.monthYear);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["monthType"]))
                {
                    this.monthType = (SaleStatisticsType) Convert.ToInt32(this.Page.Request.QueryString["monthType"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dayYear"]))
                {
                    int.TryParse(this.Page.Request.QueryString["dayYear"], out this.dayYear);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dayMonth"]))
                {
                    int.TryParse(this.Page.Request.QueryString["dayMonth"], out this.dayMonth);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dayType"]))
                {
                    this.dayType = (SaleStatisticsType) Convert.ToInt32(this.Page.Request.QueryString["dayType"]);
                }
                this.dropDayForYear.SelectedValue = this.dayYear;
                this.dropMoth.SelectedValue = this.dayMonth;
                this.radioDayForSaleType.SelectedValue = this.dayType;
                this.dropMonthForYaer.SelectedValue = this.monthYear;
                this.radioMonthForSaleType.SelectedValue = this.monthType;
            }
            else
            {
                this.monthYear = this.dropMonthForYaer.SelectedValue;
                this.monthType = this.radioMonthForSaleType.SelectedValue;
                this.dayYear = this.dropDayForYear.SelectedValue;
                this.dayMonth = this.dropMoth.SelectedValue;
                this.dayType = this.radioDayForSaleType.SelectedValue;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryMonthSaleTotal.Click += new EventHandler(this.btnQueryMonthSaleTotal_Click);
            this.btnCreateReportOfMonth.Click += new EventHandler(this.btnCreateReportOfMonth_Click);
            this.btnQueryDaySaleTotal.Click += new EventHandler(this.btnQueryDaySaleTotal_Click);
            this.btnCreateReportOfDay.Click += new EventHandler(this.btnCreateReportOfDay_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindMonthSaleTotalStatistics();
                this.BindDaySaleTotalStatistics();
            }
        }

        private void ReBind()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("monthYear", this.dropMonthForYaer.SelectedValue.ToString());
            queryStrings.Add("monthType", ((int) this.radioMonthForSaleType.SelectedValue).ToString());
            queryStrings.Add("dayYear", this.dropDayForYear.SelectedValue.ToString());
            queryStrings.Add("dayMonth", this.dropMoth.SelectedValue.ToString());
            queryStrings.Add("dayType", ((int) this.radioDayForSaleType.SelectedValue).ToString());
            base.ReloadPage(queryStrings);
        }

        public DataTable TableOfDay
        {
            get
            {
                if (this.ViewState["TableOfDay"] != null)
                {
                    return (DataTable) this.ViewState["TableOfDay"];
                }
                return null;
            }
            set
            {
                this.ViewState["TableOfDay"] = value;
            }
        }

        public DataTable TableOfMonth
        {
            get
            {
                if (this.ViewState["TableOfMonth"] != null)
                {
                    return (DataTable) this.ViewState["TableOfMonth"];
                }
                return null;
            }
            set
            {
                this.ViewState["TableOfMonth"] = value;
            }
        }
    }
}


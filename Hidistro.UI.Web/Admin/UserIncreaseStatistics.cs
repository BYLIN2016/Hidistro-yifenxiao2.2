namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.UserIncreaseStatistics)]
    public class UserIncreaseStatistics : AdminPage
    {
        protected Button btnOfMonth;
        protected Button btnOfYear;
        protected MonthDropDownList drpMonthOfMonth;
        protected YearDropDownList drpYearOfMonth;
        protected YearDropDownList drpYearOfYear;
        protected HtmlImage imgChartOfMonth;
        protected HtmlImage imgChartOfSevenDay;
        protected HtmlImage imgChartOfYear;
        protected Literal litlOfMonth;
        protected Literal litlOfYear;

        private void BindDaysAddUser()
        {
            IList<UserStatisticsForDate> list = SalesHelper.GetUserAdd(null, null, 7);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                if (string.IsNullOrEmpty(str))
                {
                    if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                    {
                        str = str + ((DateTime.Now.Month > 9) ? ((DateTime.Now.Month - 1)).ToString(CultureInfo.InvariantCulture) : ("0" + ((DateTime.Now.Month - 1)).ToString(CultureInfo.InvariantCulture) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)))));
                    }
                    else
                    {
                        str = str + ((DateTime.Now.Month > 9) ? DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) : ("0" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)))));
                    }
                }
                else if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                {
                    string str3 = str;
                    str = str3 + "|" + ((DateTime.Now.Month > 10) ? ((DateTime.Now.Month - 1)).ToString(CultureInfo.InvariantCulture) : ("0" + ((DateTime.Now.Month - 1)).ToString(CultureInfo.InvariantCulture))) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    string str4 = str;
                    str = str4 + "|" + ((DateTime.Now.Month > 10) ? DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) : ("0" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture))) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)));
                }
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + date.UserCounts;
                }
                else
                {
                    str2 = str2 + "|" + date.UserCounts;
                }
            }
            this.imgChartOfSevenDay.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", str, str2);
        }

        private void BindMonthAddUser()
        {
            IList<UserStatisticsForDate> list = SalesHelper.GetUserAdd(new int?(this.drpYearOfMonth.SelectedValue), new int?(this.drpMonthOfMonth.SelectedValue), null);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = str + date.TimePoint;
                }
                else
                {
                    str = str + "|" + date.TimePoint;
                }
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + date.UserCounts;
                }
                else
                {
                    str2 = str2 + "|" + date.UserCounts;
                }
            }
            this.imgChartOfMonth.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", str, str2);
            this.litlOfMonth.Text = this.drpYearOfMonth.SelectedValue.ToString(CultureInfo.InvariantCulture) + "年" + this.drpMonthOfMonth.SelectedValue.ToString(CultureInfo.InvariantCulture) + "月";
        }

        private void BindYearAddUser()
        {
            IList<UserStatisticsForDate> list = SalesHelper.GetUserAdd(new int?(this.drpYearOfYear.SelectedValue), null, null);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = str + date.TimePoint;
                }
                else
                {
                    str = str + "|" + date.TimePoint;
                }
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + date.UserCounts;
                }
                else
                {
                    str2 = str2 + "|" + date.UserCounts;
                }
            }
            this.imgChartOfYear.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", str, str2);
            this.litlOfYear.Text = this.drpYearOfYear.SelectedValue.ToString(CultureInfo.InvariantCulture) + "年";
        }

        private void btnOfMonth_Click(object sender, EventArgs e)
        {
            this.BindMonthAddUser();
        }

        private void btnOfYear_Click(object sender, EventArgs e)
        {
            this.BindYearAddUser();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnOfMonth.Click += new EventHandler(this.btnOfMonth_Click);
            this.btnOfYear.Click += new EventHandler(this.btnOfYear_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindDaysAddUser();
                this.BindMonthAddUser();
                this.BindYearAddUser();
            }
        }
    }
}


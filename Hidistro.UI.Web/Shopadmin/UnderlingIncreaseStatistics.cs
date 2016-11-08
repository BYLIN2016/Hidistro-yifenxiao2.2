namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UnderlingIncreaseStatistics : DistributorPage
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

        private void BindMonthUserIncrease()
        {
            IList<UserStatisticsForDate> list = UnderlingHelper.GetUserIncrease(new int?(this.drpYearOfMonth.SelectedValue), new int?(this.drpMonthOfMonth.SelectedValue), null);
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
            this.litlOfMonth.Text = this.drpYearOfMonth.SelectedValue.ToString() + "年" + this.drpMonthOfMonth.SelectedValue.ToString() + "月";
        }

        private void BindWeekUserIncrease()
        {
            IList<UserStatisticsForDate> list = UnderlingHelper.GetUserIncrease(null, null, 7);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                if (string.IsNullOrEmpty(str))
                {
                    if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                    {
                        str = str + ((DateTime.Now.Month > 9) ? ((DateTime.Now.Month - 1)).ToString() : ("0" + ((DateTime.Now.Month - 1)).ToString() + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()))));
                    }
                    else
                    {
                        str = str + ((DateTime.Now.Month > 9) ? DateTime.Now.Month.ToString() : ("0" + DateTime.Now.Month.ToString() + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()))));
                    }
                }
                else if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                {
                    string str3 = str;
                    str = str3 + "|" + ((DateTime.Now.Month > 10) ? ((DateTime.Now.Month - 1)).ToString() : ("0" + ((DateTime.Now.Month - 1)).ToString())) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()));
                }
                else
                {
                    string str4 = str;
                    str = str4 + "|" + ((DateTime.Now.Month > 10) ? DateTime.Now.Month.ToString() : ("0" + DateTime.Now.Month.ToString())) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()));
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

        private void BindYearUserIncrease()
        {
            IList<UserStatisticsForDate> list = UnderlingHelper.GetUserIncrease(new int?(this.drpYearOfYear.SelectedValue), null, null);
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
            this.litlOfYear.Text = this.drpYearOfYear.SelectedValue.ToString() + "年";
        }

        protected void btnOfMonth_Click(object sender, EventArgs e)
        {
            this.BindMonthUserIncrease();
        }

        protected void btnOfYear_Click(object sender, EventArgs e)
        {
            this.BindYearUserIncrease();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnOfMonth.Click += new EventHandler(this.btnOfMonth_Click);
            this.btnOfYear.Click += new EventHandler(this.btnOfYear_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindWeekUserIncrease();
                this.BindMonthUserIncrease();
                this.BindYearUserIncrease();
            }
        }
    }
}


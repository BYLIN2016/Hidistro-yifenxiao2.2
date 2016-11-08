namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class EditMyCountDown : DistributorPage
    {
        protected Button btnUpdateCountDown;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private int countDownId;
        protected DistributorProductCategoriesDropDownList dropCategories;
        protected DistributorGroupBuyProductDropDownList dropGroupBuyProduct;
        protected HourDropDownList drophours;
        protected HourDropDownList HourDropDownList1;
        protected Label lblPrice;
        protected TextBox txtContent;
        protected TextBox txtMaxCount;
        protected TextBox txtPrice;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void btnUpdateGroupBuy_Click(object sender, EventArgs e)
        {
            int num2;
            CountDownInfo countDownInfo = new CountDownInfo();
            countDownInfo.CountDownId = this.countDownId;
            string str = string.Empty;
            if (this.dropGroupBuyProduct.SelectedValue > 0)
            {
                if ((SubsitePromoteHelper.GetCountDownInfo(this.countDownId).ProductId != this.dropGroupBuyProduct.SelectedValue.Value) && SubsitePromoteHelper.ProductCountDownExist(this.dropGroupBuyProduct.SelectedValue.Value))
                {
                    this.ShowMsg("已经存在此商品的限时抢购活动", false);
                    return;
                }
                countDownInfo.ProductId = this.dropGroupBuyProduct.SelectedValue.Value;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请选择限时抢购商品");
            }
            if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择结束日期");
            }
            else
            {
                countDownInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double) this.HourDropDownList1.SelectedValue.Value);
                if (DateTime.Compare(countDownInfo.EndDate, DateTime.Now) <= 0)
                {
                    str = str + Formatter.FormatErrorMessage("结束日期必须要晚于今天日期");
                }
                else if (DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double) this.drophours.SelectedValue.Value), countDownInfo.EndDate) >= 0)
                {
                    str = str + Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
                }
                else
                {
                    countDownInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double) this.drophours.SelectedValue.Value);
                }
            }
            if (!string.IsNullOrEmpty(this.txtPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.txtPrice.Text.Trim(), out num))
                {
                    countDownInfo.CountDownPrice = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("价格填写格式不正确");
                }
            }
            if (int.TryParse(this.txtMaxCount.Text.Trim(), out num2))
            {
                countDownInfo.MaxCount = num2;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
            }
            else
            {
                countDownInfo.Content = this.txtContent.Text;
                if (SubsitePromoteHelper.UpdateCountDown(countDownInfo))
                {
                    this.ShowMsg("编辑限时抢购活动成功", true);
                }
                else
                {
                    this.ShowMsg("编辑限时抢购活动失败", true);
                }
            }
        }

        private void LoadCountDown(CountDownInfo countDownInfo)
        {
            this.txtPrice.Text = countDownInfo.CountDownPrice.ToString("f2");
            this.txtContent.Text = countDownInfo.Content;
            this.calendarEndDate.SelectedDate = new DateTime?(countDownInfo.EndDate.Date);
            this.HourDropDownList1.SelectedValue = new int?(countDownInfo.EndDate.Hour);
            this.calendarStartDate.SelectedDate = new DateTime?(countDownInfo.StartDate.Date);
            this.drophours.SelectedValue = new int?(countDownInfo.StartDate.Hour);
            this.dropGroupBuyProduct.SelectedValue = new int?(countDownInfo.ProductId);
            this.txtMaxCount.Text = Convert.ToString(countDownInfo.MaxCount);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["CountDownId"], out this.countDownId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdateCountDown.Click += new EventHandler(this.btnUpdateGroupBuy_Click);
                if (!base.IsPostBack)
                {
                    this.dropGroupBuyProduct.DataBind();
                    this.dropCategories.DataBind();
                    this.HourDropDownList1.DataBind();
                    this.drophours.DataBind();
                    CountDownInfo countDownInfo = SubsitePromoteHelper.GetCountDownInfo(this.countDownId);
                    if (countDownInfo == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.LoadCountDown(countDownInfo);
                    }
                }
            }
        }
    }
}


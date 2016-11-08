namespace Hidistro.UI.Web.Shopadmin.promotion.Ascx
{
    using ASPNET.WebControls;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Subsites.Utility;
    using kindeditor.Net;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MyPromotionView : UserControl
    {
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected UnderlingGradeCheckBoxList chklMemberGrade;
        protected KindeditorControl fckDescription;
        private PromotionInfo promotion;
        protected TextBox txtPromoteSalesName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.chklMemberGrade.DataBind();
                if (this.promotion != null)
                {
                    this.txtPromoteSalesName.Text = this.promotion.Name;
                    this.calendarStartDate.SelectedDate = new DateTime?(this.promotion.StartDate);
                    this.calendarEndDate.SelectedDate = new DateTime?(this.promotion.EndDate);
                    this.chklMemberGrade.SelectedValue = this.promotion.MemberGradeIds;
                    this.fckDescription.Text = this.promotion.Description;
                }
            }
        }

        public PromotionInfo Promotion
        {
            get
            {
                PromotionInfo info = new PromotionInfo();
                info.Name = this.txtPromoteSalesName.Text;
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    info.StartDate = this.calendarStartDate.SelectedDate.Value;
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    info.EndDate = this.calendarEndDate.SelectedDate.Value;
                }
                info.MemberGradeIds = this.chklMemberGrade.SelectedValue;
                info.Description = this.fckDescription.Text;
                return info;
            }
            set
            {
                this.promotion = value;
            }
        }
    }
}


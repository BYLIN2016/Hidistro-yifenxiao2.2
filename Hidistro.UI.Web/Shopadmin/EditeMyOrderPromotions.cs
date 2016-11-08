namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hidistro.UI.Web.Shopadmin.promotion.Ascx;
    using System;
    using System.Web.UI.WebControls;

    public class EditeMyOrderPromotions : DistributorPage
    {
        private int activityId;
        protected Button btnSave;
        protected MyPromotionView promotionView;
        protected PromoteTypeRadioButtonList radPromoteType;
        protected TextBox txtCondition;
        protected TextBox txtDiscountValue;
        protected TrimTextBox txtPromoteType;

        private void btnSave_Click(object sender, EventArgs e)
        {
            PromotionInfo promotion = this.promotionView.Promotion;
            promotion.ActivityId = this.activityId;
            if (promotion.MemberGradeIds.Count <= 0)
            {
                this.ShowMsg("必须选择一个适合的客户", false);
            }
            else if (promotion.StartDate.CompareTo(promotion.EndDate) > 0)
            {
                this.ShowMsg("开始日期应该小于结束日期", false);
            }
            else
            {
                promotion.PromoteType = (PromoteType) int.Parse(this.txtPromoteType.Text);
                decimal result = 0M;
                decimal num2 = 0M;
                decimal.TryParse(this.txtCondition.Text.Trim(), out result);
                decimal.TryParse(this.txtDiscountValue.Text.Trim(), out num2);
                promotion.Condition = result;
                promotion.DiscountValue = num2;
                switch (SubsitePromoteHelper.EditPromotion(promotion))
                {
                    case -1:
                        this.ShowMsg("编辑促销活动失败，可能是信填写有误，请重试", false);
                        return;

                    case -2:
                        this.ShowMsg("编辑促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
                        return;

                    case 0:
                        this.ShowMsg("编辑促销活动失败，请重试", false);
                        return;
                }
                this.ShowMsg("编辑促销活动成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSave.Click += new EventHandler(this.btnSave_Click);
                if (!this.Page.IsPostBack)
                {
                    PromotionInfo promotion = SubsitePromoteHelper.GetPromotion(this.activityId);
                    this.promotionView.Promotion = promotion;
                    this.txtPromoteType.Text = ((int) promotion.PromoteType).ToString();
                    if ((promotion.PromoteType == PromoteType.FullQuantityDiscount) || (promotion.PromoteType == PromoteType.FullQuantityReduced))
                    {
                        this.txtCondition.Text = promotion.Condition.ToString("F0");
                    }
                    else
                    {
                        this.txtCondition.Text = promotion.Condition.ToString("F2");
                    }
                    this.txtDiscountValue.Text = promotion.DiscountValue.ToString("F2");
                }
            }
        }
    }
}


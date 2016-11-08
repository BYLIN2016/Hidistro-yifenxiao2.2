namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.promotion.Ascx;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddProductPromotion : AdminPage
    {
        protected Button btnNext;
        protected HtmlGenericControl h1;
        public bool isWholesale;
        protected PromotionView promotionView;
        protected PromoteTypeRadioButtonList radPromoteType;
        protected HtmlGenericControl span1;
        protected TextBox txtCondition;
        protected TextBox txtDiscountValue;
        protected TrimTextBox txtPromoteType;

        private void btnNext_Click(object sender, EventArgs e)
        {
            PromotionInfo promotion = this.promotionView.Promotion;
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
                int num3 = PromoteHelper.AddPromotion(promotion);
                switch (num3)
                {
                    case -1:
                        this.ShowMsg("添加促销活动失败，可能是信填写有误，请重试", false);
                        return;

                    case -2:
                        this.ShowMsg("添加促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
                        return;

                    case 0:
                        this.ShowMsg("添加促销活动失败，请重试", false);
                        return;
                }
                base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Concat(new object[] { "/promotion/SetPromotionProducts.aspx?ActivityId=", num3, "&isWholesale=", this.isWholesale })), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
            if (this.isWholesale)
            {
                this.radPromoteType.IsWholesale = true;
                this.h1.InnerText = "添加批发规则";
                this.span1.InnerText = "添加批发规则";
            }
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
        }
    }
}


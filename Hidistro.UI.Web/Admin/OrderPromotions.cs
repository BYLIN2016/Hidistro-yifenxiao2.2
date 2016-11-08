namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.OrderPromotion)]
    public class OrderPromotions : AdminPage
    {
        protected Grid grdPromoteSales;
        protected HyperLink hlinkAddPromotion;
        public bool isWholesale;
        protected Literal litDec;
        protected Literal litTite;

        private void BindProductPromotions()
        {
            this.grdPromoteSales.DataSource = PromoteHelper.GetPromotions(false, this.isWholesale);
            this.grdPromoteSales.DataBind();
        }

        private void grdPromoteSales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int activityId = int.Parse(this.grdPromoteSales.DataKeys[e.Row.RowIndex].Value.ToString());
                Label label = (Label) e.Row.FindControl("lblmemberGrades");
                Label label2 = (Label) e.Row.FindControl("lblPromoteType");
                Literal literal = (Literal) e.Row.FindControl("ltrPromotionInfo");
                IList<MemberGradeInfo> promoteMemberGrades = PromoteHelper.GetPromoteMemberGrades(activityId);
                string str = string.Empty;
                foreach (MemberGradeInfo info in promoteMemberGrades)
                {
                    str = str + info.Name + ",";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Remove(str.Length - 1);
                }
                label.Text = str;
                switch (((PromoteType) ((int) DataBinder.Eval(e.Row.DataItem, "PromoteType"))))
                {
                    case PromoteType.FullAmountDiscount:
                        label2.Text = "满额打折";
                        literal.Text = string.Format("满足金额：{0} 折扣值：{1}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"), DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.FullAmountReduced:
                        label2.Text = "满额优惠金额";
                        literal.Text = string.Format("满足金额：{0} 优惠金额：{1}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"), DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.FullQuantityDiscount:
                        label2.Text = "满量打折";
                        literal.Text = string.Format("满足数量：{0} 折扣值：{1}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.FullQuantityReduced:
                        label2.Text = "满量优惠金额";
                        literal.Text = string.Format("满足数量：{0}，优惠金额：{1}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.FullAmountSentGift:
                        label2.Text = "满额送礼品";
                        literal.Text = string.Format("满足金额：{0} <a  href=\"javascript:DialogFrame('promotion/gifts.aspx?isPromotion=true','查看促销礼品',null,null)\">查看促销礼品</a>", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"));
                        return;

                    case PromoteType.FullAmountSentTimesPoint:
                        label2.Text = "满额送倍数积分";
                        literal.Text = string.Format("满足金额：{0}，倍数：{1}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"), DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.FullAmountSentFreight:
                        label2.Text = "满额免运费";
                        literal.Text = string.Format("满足金额：{0}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"));
                        return;

                    default:
                        return;
                }
            }
        }

        private void grdPromoteSales_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int activityId = (int) this.grdPromoteSales.DataKeys[e.RowIndex].Value;
            if (PromoteHelper.DeletePromotion(activityId))
            {
                this.ShowMsg("成功删除了选择的促销活动", true);
                this.BindProductPromotions();
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
            if (this.isWholesale)
            {
                this.litTite.Text = "混批规则";
                this.litDec.Text = "针对订单中的所有商品满足一定数量时的打折促销，您可以添加新的混批促销活动或管理当前的混批促销活动";
                this.hlinkAddPromotion.Text = "添加新的混批规则";
                this.hlinkAddPromotion.NavigateUrl = "AddOrderPromotion.aspx?isWholesale=true";
            }
            this.grdPromoteSales.RowDataBound += new GridViewRowEventHandler(this.grdPromoteSales_RowDataBound);
            this.grdPromoteSales.RowDeleting += new GridViewDeleteEventHandler(this.grdPromoteSales_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindProductPromotions();
            }
        }
    }
}


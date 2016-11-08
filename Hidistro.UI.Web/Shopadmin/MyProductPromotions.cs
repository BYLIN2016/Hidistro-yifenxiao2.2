namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MyProductPromotions : DistributorPage
    {
        protected Grid grdPromoteSales;

        private void BindProductPromotions()
        {
            this.grdPromoteSales.DataSource = SubsitePromoteHelper.GetPromotions(true);
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
                IList<MemberGradeInfo> promoteMemberGrades = SubsitePromoteHelper.GetPromoteMemberGrades(activityId);
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
                    case PromoteType.Discount:
                        label2.Text = "直接打折";
                        literal.Text = string.Format("折扣值：{0}", DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.Amount:
                        label2.Text = "固定金额出售";
                        literal.Text = string.Format("固定金额值：{0}", DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.Reduced:
                        label2.Text = "优惠金额出售";
                        literal.Text = string.Format("优惠金额值：{0}", DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.QuantityDiscount:
                        label2.Text = "按批发数量打折";
                        literal.Text = string.Format("购买数量：{0} 折扣值：{1}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;

                    case PromoteType.SentGift:
                        label2.Text = "买商品送礼品";
                        literal.Text = "<a target=\"_blank\" href=\"mygiftlist.aspx?isPromotion=true\">查看促销礼品</a>";
                        return;

                    case PromoteType.SentProduct:
                        label2.Text = "有买有送";
                        literal.Text = string.Format("购买数量：{0} 赠送数量：{1}", DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f0}"));
                        return;

                    default:
                        return;
                }
            }
        }

        private void grdPromoteSales_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int activityId = (int) this.grdPromoteSales.DataKeys[e.RowIndex].Value;
            if (SubsitePromoteHelper.DeletePromotion(activityId))
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
            this.grdPromoteSales.RowDataBound += new GridViewRowEventHandler(this.grdPromoteSales_RowDataBound);
            this.grdPromoteSales.RowDeleting += new GridViewDeleteEventHandler(this.grdPromoteSales_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindProductPromotions();
            }
        }
    }
}


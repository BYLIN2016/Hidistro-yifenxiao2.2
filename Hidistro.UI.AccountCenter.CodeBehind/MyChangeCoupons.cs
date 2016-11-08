namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI.WebControls;

    public class MyChangeCoupons : MemberTemplatedWebControl
    {
        private Common_Coupon_ChangeCouponList changeCoupons;

        protected override void AttachChildControls()
        {
            this.changeCoupons = (Common_Coupon_ChangeCouponList) this.FindControl("Common_Coupon_ChangeCouponList");
            this.changeCoupons._ItemCommand += new Common_Coupon_ChangeCouponList.CommandEventHandler(this.changeCoupons_ItemCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindCoupons();
            }
        }

        private void BindCoupons()
        {
            this.changeCoupons.DataSource = TradeHelper.GetChangeCoupons();
            this.changeCoupons.DataBind();
        }

        private void changeCoupons_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int couponId = (int) this.changeCoupons.DataKeys[e.Item.ItemIndex];
            if (e.CommandName == "Change")
            {
                Literal literal = (Literal) e.Item.FindControl("litNeedPoint");
                int needPoint = int.Parse(literal.Text);
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (needPoint > user.Points)
                {
                    this.ShowMessage("当前积分不够兑换此优惠券", false);
                }
                else if (TradeHelper.PointChageCoupon(couponId, needPoint, user.Points))
                {
                    this.ShowMessage("兑换成功，请查看您的优惠券", true);
                }
                else
                {
                    this.ShowMessage("兑换失败", false);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-MyChangeCoupons.html";
            }
            base.OnInit(e);
        }
    }
}


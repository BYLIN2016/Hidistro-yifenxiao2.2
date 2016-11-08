namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class MyCoupons : MemberTemplatedWebControl
    {
        private IButton btnAddCoupon;
        private Common_Coupon_CouponList coupons;
        private SmallStatusMessage status;
        private TextBox txtCoupon;

        protected override void AttachChildControls()
        {
            this.coupons = (Common_Coupon_CouponList) this.FindControl("Common_Coupons_CouponsList");
            this.txtCoupon = (TextBox) this.FindControl("txtCoupon");
            this.status = (SmallStatusMessage) this.FindControl("status");
            this.btnAddCoupon = ButtonManager.Create(this.FindControl("btnAddCoupon"));
            this.btnAddCoupon.Click += new EventHandler(this.btnAddCoupon_Click);
            new HyperLink();
            if (!this.Page.IsPostBack)
            {
                this.BindCoupons();
            }
        }

        private void BindCoupons()
        {
            this.coupons.DataSource = TradeHelper.GetUserCoupons(HiContext.Current.User.UserId);
            this.coupons.DataBind();
        }

        private void btnAddCoupon_Click(object sender, EventArgs e)
        {
            string text = this.txtCoupon.Text;
            if (!TradeHelper.ExitCouponClaimCode(text))
            {
                this.ShowMessage("你输入的优惠券号码无效，请重试", false);
            }
            else if (TradeHelper.AddClaimCodeToUser(text, HiContext.Current.User.UserId) > 0)
            {
                this.BindCoupons();
                this.txtCoupon.Text = string.Empty;
                this.ShowMessage("成功的添加了优惠券到你的账户", true);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-MyCoupons.html";
            }
            base.OnInit(e);
        }
    }
}


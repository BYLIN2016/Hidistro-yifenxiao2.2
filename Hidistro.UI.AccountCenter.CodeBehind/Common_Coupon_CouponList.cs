namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_Coupon_CouponList : AscxTemplatedWebControl
    {
        private DataList dataListCoupon;
        public const string TagID = "Common_Coupons_CouponsList";

        public Common_Coupon_CouponList()
        {
            base.ID = "Common_Coupons_CouponsList";
        }

        protected override void AttachChildControls()
        {
            this.dataListCoupon = (DataList) this.FindControl("dataListCoupon");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListCoupon.DataSource != null)
            {
                this.dataListCoupon.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_CouponList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListCoupon.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListCoupon.DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
    }
}


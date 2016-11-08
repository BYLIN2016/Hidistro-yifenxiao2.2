namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Member;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Sales;
    
    public class Common_ShippingAddress : AscxTemplatedWebControl
    {
        private Repeater rp_shippingaddress;
        public const string TagID = "rp_Common_ShippingAddress";

        public Common_ShippingAddress()
        {
            base.ID = "rp_Common_ShippingAddress";
        }

        protected override void AttachChildControls()
        {
            this.rp_shippingaddress = (Repeater) this.FindControl("rp_shippgaddress");
            this.GetShippingAddress();
        }

        private void GetShippingAddress()
        {
            if (HiContext.Current.User.UserId > 0)
            {
                IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(HiContext.Current.User.UserId);
                if (shippingAddresses.Count > 0)
                {
                    this.rp_shippingaddress.DataSource = shippingAddresses;
                    this.rp_shippingaddress.DataBind();
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_ShippingAddress.ascx";
            }
            base.OnInit(e);
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


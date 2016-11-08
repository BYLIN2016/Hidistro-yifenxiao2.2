namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Member;
    using System;
    using System.Web.UI.WebControls;

    public class Common_ShippingAddressRadioButtonList : RadioButtonList
    {
        public const string TagID = "Common_ShippingAddressesRadioButtonList";

        public Common_ShippingAddressRadioButtonList()
        {
            base.ID = "Common_ShippingAddressesRadioButtonList";
        }

        public override void DataBind()
        {
            foreach (ShippingAddressInfo info in MemberProcessor.GetShippingAddresses(HiContext.Current.User.UserId))
            {
                this.Items.Add(new ListItem(info.Address + "(收货人：" + info.ShipTo + ")", info.ShippingId.ToString()));
            }
        }

        public int SelectedValue
        {
            get
            {
                return int.Parse(base.SelectedValue);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
            }
        }
    }
}


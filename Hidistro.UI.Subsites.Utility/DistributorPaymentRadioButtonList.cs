namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.WebControls;

    public class DistributorPaymentRadioButtonList : RadioButtonList
    {
        public DistributorPaymentRadioButtonList()
        {
            base.Items.Clear();
            foreach (PaymentModeInfo info in SubsiteStoreHelper.GetDistributorPaymentModes())
            {
                if (info.Gateway.ToLower().Equals("hishop.plugins.payment.alipay_shortcut.shortcutrequest"))
                {
                    HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                    if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                    {
                        this.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.ModeId.ToString(CultureInfo.InvariantCulture)));
                    }
                }
                else
                {
                    this.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.ModeId.ToString(CultureInfo.InvariantCulture)));
                }
            }
            this.SelectedIndex = 0;
            this.RepeatDirection = RepeatDirection.Horizontal;
        }
    }
}


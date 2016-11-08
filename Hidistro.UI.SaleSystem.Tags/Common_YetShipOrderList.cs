namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_YetShipOrderList : ThemedTemplatedRepeater
    {
        protected override void OnLoad(EventArgs e)
        {
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (siteSettings.OrderShowDays > 0)
            {
                base.DataSource = ShoppingProcessor.GetYetShipOrders(siteSettings.OrderShowDays);
                base.DataBind();
            }
        }
    }
}


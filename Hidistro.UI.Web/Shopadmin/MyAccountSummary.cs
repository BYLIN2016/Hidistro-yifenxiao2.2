namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Members;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;

    public class MyAccountSummary : DistributorPage
    {
        protected FormatedMoneyLabel lblAccountAmount;
        protected FormatedMoneyLabel lblFreezeBalance;
        protected FormatedMoneyLabel lblUseableBalance;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
                this.lblAccountAmount.Money = myAccountSummary.AccountAmount;
                this.lblFreezeBalance.Money = myAccountSummary.FreezeBalance;
                this.lblUseableBalance.Money = myAccountSummary.UseableBalance;
            }
        }
    }
}


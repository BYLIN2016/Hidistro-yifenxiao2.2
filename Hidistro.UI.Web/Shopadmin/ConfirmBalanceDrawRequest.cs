namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ConfirmBalanceDrawRequest : DistributorPage
    {
        protected Button btnOK;
        protected FormatedMoneyLabel lblAmount;
        protected Literal litBankCode;
        protected Literal litBankName;
        protected Literal litName;
        protected Literal litRealName;
        protected HtmlGenericControl message;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SubsiteStoreHelper.DistroHasDrawRequest())
            {
                this.ShowMsg("对不起，您的上一笔提现申请尚未进行处理", false);
            }
            else if (this.Session["BalanceDrawRequest"] != null)
            {
                BalanceDrawRequestInfo balanceDrawRequest = (BalanceDrawRequestInfo) this.Session["BalanceDrawRequest"];
                if (SubsiteStoreHelper.BalanceDrawRequest(balanceDrawRequest))
                {
                    this.message.Visible = true;
                }
                else
                {
                    this.ShowMsg("写入提现信息失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!base.IsPostBack)
            {
                BalanceDrawRequestInfo info = new BalanceDrawRequestInfo();
                if (this.Session["BalanceDrawRequest"] != null)
                {
                    info = (BalanceDrawRequestInfo) this.Session["BalanceDrawRequest"];
                    Distributor user = Users.GetUser(info.UserId) as Distributor;
                    this.litRealName.Text = user.RealName;
                    this.litName.Text = info.AccountName;
                    this.litBankName.Text = info.BankName;
                    this.litBankCode.Text = info.MerchantCode;
                    this.lblAmount.Money = info.Amount;
                }
                else
                {
                    base.GotoResourceNotFound();
                }
            }
        }
    }
}


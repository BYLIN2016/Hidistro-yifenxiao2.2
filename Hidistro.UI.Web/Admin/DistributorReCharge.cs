namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorReCharge)]
    public class DistributorReCharge : AdminPage
    {
        protected Button btnReChargeOK;
        protected FormatedMoneyLabel lblUseableBalance;
        protected Literal litUserNames;
        protected TextBox txtReCharge;
        protected HtmlGenericControl txtReChargeTip;
        protected TextBox txtRemarks;
        protected HtmlGenericControl txtRemarksTip;
        private int userId;

        private void btnReChargeOK_Click(object sender, EventArgs e)
        {
            decimal num;
            int length = 0;
            if (this.txtReCharge.Text.Trim().IndexOf(".") > 0)
            {
                length = this.txtReCharge.Text.Trim().Substring(this.txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
            }
            if (!decimal.TryParse(this.txtReCharge.Text.Trim(), out num) || (length > 2))
            {
                this.ShowMsg("本次充值要给当前客户加款的金额只能是数值，且不能超过2位小数", false);
            }
            else if ((num < -10000000M) || (num > 10000000M))
            {
                this.ShowMsg("金额大小必须在正负1000万之间", false);
            }
            else
            {
                Distributor user = Users.GetUser(this.userId, false) as Distributor;
                if (user == null)
                {
                    this.ShowMsg("此分销商已经不存在", false);
                }
                else
                {
                    decimal num3 = num + user.Balance;
                    BalanceDetailInfo target = new BalanceDetailInfo();
                    target.UserId = this.userId;
                    target.UserName = user.Username;
                    target.TradeDate = DateTime.Now;
                    target.TradeType = TradeTypes.BackgroundAddmoney;
                    target.Income = new decimal?(num);
                    target.Balance = num3;
                    target.Remark = Globals.HtmlEncode(this.txtRemarks.Text.Trim());
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<BalanceDetailInfo>(target, new string[] { "ValBalanceDetail" });
                    string msg = string.Empty;
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                        {
                            msg = msg + Formatter.FormatErrorMessage(result.Message);
                        }
                        this.ShowMsg(msg, false);
                    }
                    else
                    {
                        if (DistributorHelper.AddBalance(target, num))
                        {
                            this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"本次充值已成功，充值金额：{0}\");window.location.href=\"DistributorReCharge.aspx?userId={1}\"</script>", num, this.userId));
                        }
                        this.txtReCharge.Text = string.Empty;
                        this.txtRemarks.Text = string.Empty;
                        this.lblUseableBalance.Money = num3;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnReChargeOK.Click += new EventHandler(this.btnReChargeOK_Click);
                if (!this.Page.IsPostBack)
                {
                    Distributor distributor = DistributorHelper.GetDistributor(this.userId);
                    if (distributor == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litUserNames.Text = distributor.Username;
                        this.lblUseableBalance.Money = distributor.Balance - distributor.RequestBalance;
                    }
                }
            }
        }
    }
}


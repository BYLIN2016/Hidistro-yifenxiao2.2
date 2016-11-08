namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class UpdateTranPassword : MemberTemplatedWebControl
    {
        private IButton btnOK2;
        private SmallStatusMessage StatusTransactionPass;
        private TextBox txtNewTransactionPassWord;
        private TextBox txtNewTransactionPassWord2;
        private TextBox txtOldTransactionPassWord;

        protected override void AttachChildControls()
        {
            this.txtOldTransactionPassWord = (TextBox) this.FindControl("txtOldTransactionPassWord");
            this.txtNewTransactionPassWord = (TextBox) this.FindControl("txtNewTransactionPassWord");
            this.txtNewTransactionPassWord2 = (TextBox) this.FindControl("txtNewTransactionPassWord2");
            this.btnOK2 = ButtonManager.Create(this.FindControl("btnOK2"));
            this.StatusTransactionPass = (SmallStatusMessage) this.FindControl("StatusTransactionPass");
            PageTitle.AddSiteNameTitle("修改交易密码", HiContext.Current.Context);
            this.btnOK2.Click += new EventHandler(this.btnOK2_Click);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", HttpContext.Current.Request.Url));
                }
            }
        }

        private void btnOK2_Click(object sender, EventArgs e)
        {
            Member user = HiContext.Current.User as Member;
            if ((user.MembershipUser != null) && user.MembershipUser.IsLockedOut)
            {
                this.ShowMessage(this.StatusTransactionPass, "你已经被管理员锁定", false);
            }
            else
            {
                user.TradePassword = this.txtOldTransactionPassWord.Text;
                if (user.ChangeTradePassword(this.txtOldTransactionPassWord.Text, this.txtNewTransactionPassWord.Text))
                {
                    Messenger.UserDealPasswordChanged(user, this.txtNewTransactionPassWord.Text);
                    user.OnDealPasswordChanged(new UserEventArgs(user.Username, null, this.txtNewTransactionPassWord.Text));
                    this.ShowMessage(this.StatusTransactionPass, "你已经成功的修改了交易密码", true);
                }
                else
                {
                    this.ShowMessage(this.StatusTransactionPass, "修改交易密码失败", false);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UpdateTranPassword.html";
            }
            base.OnInit(e);
        }

        protected virtual void ShowMessage(SmallStatusMessage state, string msg, bool success)
        {
            if (state != null)
            {
                state.Success = success;
                state.Text = msg;
                state.Visible = true;
            }
        }
    }
}


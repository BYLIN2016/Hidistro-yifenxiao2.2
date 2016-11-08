namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class OpenBalance : MemberTemplatedWebControl
    {
        private IButton btnOpen;
        private TextBox txtTranPassword;
        private TextBox txtTranPassword2;

        protected override void AttachChildControls()
        {
            this.txtTranPassword = (TextBox) this.FindControl("txtTranPassword");
            this.txtTranPassword2 = (TextBox) this.FindControl("txtTranPassword2");
            this.btnOpen = ButtonManager.Create(this.FindControl("btnOpen"));
            PageTitle.AddSiteNameTitle("开启预付款账户", HiContext.Current.Context);
            this.btnOpen.Click += new EventHandler(this.btnOpen_Click);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (user.IsOpenBalance)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/MyBalanceDetails.aspx", new object[0]));
                }
            }
        }

        protected void btnOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTranPassword.Text))
            {
                this.ShowMessage("请输入交易密码", false);
            }
            else if ((this.txtTranPassword.Text.Length < 6) || (this.txtTranPassword.Text.Length > 20))
            {
                this.ShowMessage("交易密码限制为6-20个字符", false);
            }
            else if (string.IsNullOrEmpty(this.txtTranPassword2.Text))
            {
                this.ShowMessage("请确认交易密码", false);
            }
            else if (string.Compare(this.txtTranPassword2.Text, this.txtTranPassword.Text) != 0)
            {
                this.ShowMessage("两次输入的交易密码不一致", false);
            }
            else
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (user.OpenBalance(this.txtTranPassword.Text))
                {
                    if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReturnUrl"]))
                    {
                        this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/MyBalanceDetails.aspx", new object[0]));
                    }
                    else
                    {
                        this.Page.Response.Redirect(this.Page.Request.QueryString["ReturnUrl"]);
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-OpenBalance.html";
            }
            base.OnInit(e);
        }
    }
}


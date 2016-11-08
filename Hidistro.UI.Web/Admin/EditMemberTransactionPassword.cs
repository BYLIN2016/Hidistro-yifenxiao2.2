namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditMember)]
    public class EditMemberTransactionPassword : AdminPage
    {
        protected Button btnEditUser;
        private int currentUserId;
        protected Literal litlUserName;
        protected TextBox txtTransactionPassWord;
        protected TextBox txtTransactionPassWordCompare;
        protected HtmlGenericControl txtTransactionPassWordCompareTip;
        protected HtmlGenericControl txtTransactionPassWordTip;

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            Member user = MemberHelper.GetMember(this.currentUserId);
            if (!user.IsOpenBalance)
            {
                this.ShowMsg("该会员没有开启预付款账户，无法修改交易密码", false);
            }
            else if ((string.IsNullOrEmpty(this.txtTransactionPassWord.Text) || (this.txtTransactionPassWord.Text.Length > 20)) || (this.txtTransactionPassWord.Text.Length < 6))
            {
                this.ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (this.txtTransactionPassWord.Text != this.txtTransactionPassWordCompare.Text)
            {
                this.ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangeTradePassword(this.txtTransactionPassWord.Text))
            {
                Messenger.UserDealPasswordChanged(user, this.txtTransactionPassWord.Text);
                user.OnDealPasswordChanged(new UserEventArgs(user.Username, null, this.txtTransactionPassWord.Text));
                this.ShowMsg("交易密码修改成功", true);
            }
            else
            {
                this.ShowMsg("交易密码修改失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditUser.Click += new EventHandler(this.btnEditUser_Click);
                if (!this.Page.IsPostBack)
                {
                    Member member = MemberHelper.GetMember(this.currentUserId);
                    if (member == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litlUserName.Text = member.Username;
                    }
                }
            }
        }
    }
}


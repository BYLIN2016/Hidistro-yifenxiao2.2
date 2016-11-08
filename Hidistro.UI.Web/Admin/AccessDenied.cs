namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class AccessDenied : AdminPage
    {
        protected Literal litMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.litMessage.Text = string.Format("您登录的管理员帐号 “{0}” 没有权限访问当前页面或进行当前操作", HiContext.Current.User.Username);
        }
    }
}


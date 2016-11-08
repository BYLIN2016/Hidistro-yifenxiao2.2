namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_CurrentUser : Literal
    {
        private string dafaultText = "您好，欢迎光临商城！";

        protected override void Render(HtmlTextWriter writer)
        {
            IUser user = HiContext.Current.User;
            if (!user.IsAnonymous)
            {
                base.Text = "您好，" + user.Username;
            }
            else
            {
                base.Text = this.DafaultText;
            }
            base.Render(writer);
        }

        public string DafaultText
        {
            get
            {
                return this.dafaultText;
            }
            set
            {
                this.dafaultText = value;
            }
        }
    }
}


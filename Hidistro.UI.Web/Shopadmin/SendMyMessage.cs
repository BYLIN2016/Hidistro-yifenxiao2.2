namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SendMyMessage : DistributorPage
    {
        protected Button btnRefer;
        protected TextBox txtContent;
        protected TextBox txtTitle;
        protected HtmlGenericControl txtTitleTip;
        private int userId;

        private void btnRefer_Click(object sender, EventArgs e)
        {
            if (this.ValidateValues())
            {
                this.Session["Title"] = Globals.UrlEncode(this.txtTitle.Text.Replace("\r\n", ""));
                this.Session["Content"] = Globals.UrlEncode(this.txtContent.Text.Replace("\r\n", ""));
                if (this.userId == 0)
                {
                    base.Response.Redirect(Globals.ApplicationPath + "/shopadmin/comment/SendMyMessageSelectUser.aspx", true);
                }
                else if (this.userId > 0)
                {
                    base.Response.Redirect(Globals.ApplicationPath + string.Format("/shopadmin/comment/SendMyMessageSelectUser.aspx?UserId={0}", this.userId), true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
        }

        private bool ValidateValues()
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()) || (this.txtTitle.Text.Trim().Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(this.txtContent.Text.Trim()) || (this.txtContent.Text.Trim().Length > 300))
            {
                str = str + Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}


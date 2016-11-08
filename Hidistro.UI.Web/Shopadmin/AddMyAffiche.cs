namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class AddMyAffiche : DistributorPage
    {
        protected Button btnAddAffiche;
        protected KindeditorControl fcContent;
        protected TextBox txtAfficheTitle;

        private void btnAddAffiche_Click(object sender, EventArgs e)
        {
            AfficheInfo target = new AfficheInfo();
            target.Title = this.txtAfficheTitle.Text.Trim();
            target.Content = this.fcContent.Text;
            target.AddedDate = DateTime.Now;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<AfficheInfo>(target, new string[] { "ValAfficheInfo" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else if (SubsiteCommentsHelper.CreateAffiche(target))
            {
                this.txtAfficheTitle.Text = string.Empty;
                this.fcContent.Text = string.Empty;
                this.ShowMsg("成功发布了一条公告", true);
            }
            else
            {
                this.ShowMsg("添加公告失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddAffiche.Click += new EventHandler(this.btnAddAffiche_Click);
        }
    }
}


namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class EditMyAffiche : DistributorPage
    {
        private int afficheId;
        protected Button btnEditAffiche;
        protected KindeditorControl fcContent;
        protected TextBox txtAfficheTitle;

        private void btnEditAffiche_Click(object sender, EventArgs e)
        {
            AfficheInfo target = new AfficheInfo();
            target.AfficheId = this.afficheId;
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
            else
            {
                target.AfficheId = this.afficheId;
                if (SubsiteCommentsHelper.UpdateAffiche(target))
                {
                    this.ShowMsg("成功修改了当前公告信息", true);
                }
                else
                {
                    this.ShowMsg("修改公告信息错误", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["afficheId"], out this.afficheId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditAffiche.Click += new EventHandler(this.btnEditAffiche_Click);
                if (!this.Page.IsPostBack)
                {
                    AfficheInfo affiche = SubsiteCommentsHelper.GetAffiche(this.afficheId);
                    if (affiche == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(affiche, false);
                        this.txtAfficheTitle.Text = affiche.Title;
                        this.fcContent.Text = affiche.Content;
                    }
                }
            }
        }
    }
}


namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class HelpDetails : HtmlTemplatedWebControl
    {
        private HtmlAnchor aFront;
        private HtmlAnchor aNext;
        private int helpId;
        private Label lblFront;
        private Label lblFrontTitle;
        private Label lblNext;
        private Label lblNextTitle;
        private FormatedTimeLabel litHelpAddedDate;
        private Literal litHelpContent;
        private Literal litHelpDescription;
        private Literal litHelpTitle;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["helpId"], out this.helpId))
            {
                base.GotoResourceNotFound();
            }
            this.litHelpAddedDate = (FormatedTimeLabel) this.FindControl("litHelpAddedDate");
            this.litHelpDescription = (Literal) this.FindControl("litHelpDescription");
            this.litHelpContent = (Literal) this.FindControl("litHelpContent");
            this.litHelpTitle = (Literal) this.FindControl("litHelpTitle");
            this.lblFront = (Label) this.FindControl("lblFront");
            this.lblNext = (Label) this.FindControl("lblNext");
            this.lblFrontTitle = (Label) this.FindControl("lblFrontTitle");
            this.lblNextTitle = (Label) this.FindControl("lblNextTitle");
            this.aFront = (HtmlAnchor) this.FindControl("front");
            this.aNext = (HtmlAnchor) this.FindControl("next");
            if (!this.Page.IsPostBack)
            {
                HelpInfo help = CommentBrowser.GetHelp(this.helpId);
                if (help != null)
                {
                    PageTitle.AddSiteNameTitle(help.Title, HiContext.Current.Context);
                    if (!string.IsNullOrEmpty(help.MetaKeywords))
                    {
                        MetaTags.AddMetaKeywords(help.MetaKeywords, HiContext.Current.Context);
                    }
                    if (!string.IsNullOrEmpty(help.MetaDescription))
                    {
                        MetaTags.AddMetaDescription(help.MetaDescription, HiContext.Current.Context);
                    }
                    this.litHelpTitle.Text = help.Title;
                    this.litHelpDescription.Text = help.Description;
                    string str = HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails", new object[] { this.helpId });
                    this.litHelpContent.Text = help.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
                    this.litHelpAddedDate.Time = help.AddedDate;
                    HelpInfo info2 = CommentBrowser.GetFrontOrNextHelp(this.helpId, help.CategoryId, "Front");
                    if ((info2 != null) && (info2.HelpId > 0))
                    {
                        if (this.lblFront != null)
                        {
                            this.lblFront.Visible = true;
                            this.aFront.HRef = "HelpDetails.aspx?helpId=" + info2.HelpId;
                            this.lblFrontTitle.Text = info2.Title;
                        }
                    }
                    else if (this.lblFront != null)
                    {
                        this.lblFront.Visible = false;
                    }
                    HelpInfo info3 = CommentBrowser.GetFrontOrNextHelp(this.helpId, help.CategoryId, "Next");
                    if ((info3 != null) && (info3.HelpId > 0))
                    {
                        if (this.lblNext != null)
                        {
                            this.lblNext.Visible = true;
                            this.aNext.HRef = "HelpDetails.aspx?helpId=" + info3.HelpId;
                            this.lblNextTitle.Text = info3.Title;
                        }
                    }
                    else if (this.lblNext != null)
                    {
                        this.lblNext.Visible = false;
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-HelpDetails.html";
            }
            base.OnInit(e);
        }
    }
}


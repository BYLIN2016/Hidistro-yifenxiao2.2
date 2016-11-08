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
    public class AffichesDetails : HtmlTemplatedWebControl
    {
        private int affichesId;
        private HtmlAnchor aFront;
        private HtmlAnchor aNext;
        private Label lblFront;
        private Label lblFrontTitle;
        private Label lblNext;
        private Label lblNextTitle;
        private FormatedTimeLabel litAffichesAddedDate;
        private Literal litContent;
        private Literal litTilte;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["AfficheId"], out this.affichesId))
            {
                base.GotoResourceNotFound();
            }
            this.litAffichesAddedDate = (FormatedTimeLabel) this.FindControl("litAffichesAddedDate");
            this.litContent = (Literal) this.FindControl("litContent");
            this.litTilte = (Literal) this.FindControl("litTilte");
            this.lblFront = (Label) this.FindControl("lblFront");
            this.lblNext = (Label) this.FindControl("lblNext");
            this.aFront = (HtmlAnchor) this.FindControl("front");
            this.aNext = (HtmlAnchor) this.FindControl("next");
            this.lblFrontTitle = (Label) this.FindControl("lblFrontTitle");
            this.lblNextTitle = (Label) this.FindControl("lblNextTitle");
            if (!this.Page.IsPostBack)
            {
                AfficheInfo affiche = CommentBrowser.GetAffiche(this.affichesId);
                if (affiche != null)
                {
                    PageTitle.AddSiteNameTitle(affiche.Title, HiContext.Current.Context);
                    this.litTilte.Text = affiche.Title;
                    string str = HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("AffichesDetails", new object[] { this.affichesId });
                    this.litContent.Text = affiche.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
                    this.litAffichesAddedDate.Time = affiche.AddedDate;
                    AfficheInfo frontOrNextAffiche = CommentBrowser.GetFrontOrNextAffiche(this.affichesId, "Front");
                    AfficheInfo info3 = CommentBrowser.GetFrontOrNextAffiche(this.affichesId, "Next");
                    if ((frontOrNextAffiche != null) && (frontOrNextAffiche.AfficheId > 0))
                    {
                        if (this.lblFront != null)
                        {
                            this.lblFront.Visible = true;
                            this.aFront.HRef = "AffichesDetails.aspx?afficheId=" + frontOrNextAffiche.AfficheId;
                            this.lblFrontTitle.Text = frontOrNextAffiche.Title;
                        }
                    }
                    else if (this.lblFront != null)
                    {
                        this.lblFront.Visible = false;
                    }
                    if ((info3 != null) && (info3.AfficheId > 0))
                    {
                        if (this.lblNext != null)
                        {
                            this.lblNext.Visible = true;
                            this.aNext.HRef = "AffichesDetails.aspx?afficheId=" + info3.AfficheId;
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
                this.SkinName = "Skin-AffichesDetails.html";
            }
            base.OnInit(e);
        }
    }
}


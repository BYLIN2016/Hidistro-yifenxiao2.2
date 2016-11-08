namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class FavourableDetails : HtmlTemplatedWebControl
    {
        private int activityId;
        private HtmlAnchor aFront;
        private HtmlAnchor aNext;
        private HyperLink hlkLink;
        private Label lblFront;
        private Label lblFrontTitle;
        private Label lblNext;
        private Label lblNextTitle;
        private Literal litHelpDescription;
        private Literal litHelpTitle;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
            {
                base.GotoResourceNotFound();
            }
            this.hlkLink = (HyperLink) this.FindControl("hlkLink");
            this.litHelpDescription = (Literal) this.FindControl("litHelpDescription");
            this.litHelpTitle = (Literal) this.FindControl("litHelpTitle");
            this.lblFront = (Label) this.FindControl("lblFront");
            this.lblNext = (Label) this.FindControl("lblNext");
            this.lblFrontTitle = (Label) this.FindControl("lblFrontTitle");
            this.lblNextTitle = (Label) this.FindControl("lblNextTitle");
            this.aFront = (HtmlAnchor) this.FindControl("front");
            this.aNext = (HtmlAnchor) this.FindControl("next");
            if (!this.Page.IsPostBack && (this.activityId > 0))
            {
                PromotionInfo promote = CommentBrowser.GetPromote(this.activityId);
                if (promote == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    PageTitle.AddSiteNameTitle(promote.Name, HiContext.Current.Context);
                    this.litHelpTitle.Text = promote.Name;
                    this.litHelpDescription.Text = promote.Description;
                    PromotionInfo frontOrNextPromoteInfo = CommentBrowser.GetFrontOrNextPromoteInfo(promote, "Front");
                    if (frontOrNextPromoteInfo != null)
                    {
                        if (this.lblFront != null)
                        {
                            this.lblFront.Visible = true;
                            this.aFront.HRef = "FavourableDetails.aspx?activityId=" + frontOrNextPromoteInfo.ActivityId;
                            this.lblFrontTitle.Text = frontOrNextPromoteInfo.Name;
                        }
                    }
                    else if (this.lblFront != null)
                    {
                        this.lblFront.Visible = false;
                    }
                    PromotionInfo info3 = CommentBrowser.GetFrontOrNextPromoteInfo(promote, "Next");
                    if (info3 != null)
                    {
                        if (this.lblNext != null)
                        {
                            this.lblNext.Visible = true;
                            this.aNext.HRef = "FavourableDetails.aspx?activityId=" + info3.ActivityId;
                            this.lblNextTitle.Text = info3.Name;
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
                this.SkinName = "Skin-FavourableDetails.html";
            }
            base.OnInit(e);
        }
    }
}


namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class UserProductReviews : MemberTemplatedWebControl
    {
        private ThemedTemplatedList dlstPts;
        private Literal litReviewCount;
        private Pager pager;

        protected override void AttachChildControls()
        {
            this.dlstPts = (ThemedTemplatedList) this.FindControl("dlstPts");
            this.pager = (Pager) this.FindControl("pager");
            this.litReviewCount = (Literal) this.FindControl("litReviewCount");
            PageTitle.AddSiteNameTitle("我参与的评论", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                if (this.litReviewCount != null)
                {
                    this.litReviewCount.Text = CommentsHelper.GetUserProductReviewsCount().ToString();
                }
                this.BindPtAndReviewsAndReplys();
            }
        }

        private void BindPtAndReviewsAndReplys()
        {
            UserProductReviewAndReplyQuery query = new UserProductReviewAndReplyQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            int total = 0;
            DataSet userProductReviewsAndReplys = CommentsHelper.GetUserProductReviewsAndReplys(query, out total);
            this.dlstPts.DataSource = userProductReviewsAndReplys.Tables[0].DefaultView;
            this.dlstPts.DataBind();
            this.pager.TotalRecords = total;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserProductReviews.html";
            }
            base.OnInit(e);
        }
    }
}


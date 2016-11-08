namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VoteResult : HtmlTemplatedWebControl
    {
        private Label lblTotal;
        private Literal lblVoteName;
        private ThemedTemplatedRepeater rptVoteItem;
        private long voteId;

        protected override void AttachChildControls()
        {
            if (!long.TryParse(this.Page.Request.QueryString["VoteId"], out this.voteId))
            {
                base.GotoResourceNotFound();
            }
            this.lblTotal = (Label) this.FindControl("lblTotal");
            this.lblVoteName = (Literal) this.FindControl("lblVoteName");
            this.rptVoteItem = (ThemedTemplatedRepeater) this.FindControl("rptVoteItem");
            if (!this.Page.IsPostBack)
            {
                VoteInfo voteById = CommentBrowser.GetVoteById(this.voteId);
                if (voteById != null)
                {
                    PageTitle.AddSiteNameTitle(voteById.VoteName, HiContext.Current.Context);
                    this.Vote(voteById);
                    VoteInfo vote = CommentBrowser.GetVoteById(this.voteId);
                    this.BindVoteItem(vote);
                }
            }
        }

        private void BindVoteItem(VoteInfo vote)
        {
            int num = 0;
            if (vote != null)
            {
                this.lblVoteName.Text = vote.VoteName;
                IList<VoteItemInfo> voteItems = CommentBrowser.GetVoteItems(this.voteId);
                for (int i = 0; i < voteItems.Count; i++)
                {
                    if (vote.VoteCounts != 0)
                    {
                        voteItems[i].Percentage = (voteItems[i].ItemCount / vote.VoteCounts) * 100M;
                        num += voteItems[i].ItemCount;
                    }
                    else
                    {
                        voteItems[i].Percentage = 0M;
                    }
                }
                this.rptVoteItem.DataSource = voteItems;
                this.rptVoteItem.DataBind();
                if (this.lblTotal != null)
                {
                    this.lblTotal.Text = num.ToString();
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VoteResult.html";
            }
            base.OnInit(e);
        }

        private void Vote(VoteInfo vote)
        {
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies[this.voteId.ToString()];
            if (cookie == null)
            {
                if (this.Page.Request.Params["VoteItemId"] != null)
                {
                    string str = this.Page.Request.Params["VoteItemId"];
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] strArray = str.Split(new char[] { ',' });
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(strArray[i]) && ((i + 1) <= vote.MaxCheck))
                            {
                                long voteItemId = Convert.ToInt64(strArray[i]);
                                VoteItemInfo voteItemById = CommentBrowser.GetVoteItemById(voteItemId);
                                if (vote.VoteId == voteItemById.VoteId)
                                {
                                    CommentBrowser.Vote(voteItemId);
                                }
                            }
                        }
                        cookie = new HttpCookie(this.voteId.ToString());
                        cookie.Expires = DateTime.Now.AddYears(100);
                        cookie.Value = this.voteId.ToString();
                        HiContext.Current.Context.Response.Cookies.Add(cookie);
                        this.ShowMessage("投票成功", true);
                    }
                }
            }
            else if ((cookie != null) && !string.IsNullOrEmpty(this.Page.Request.QueryString["VoteItemId"].ToString()))
            {
                this.ShowMessage("该用户已经投过票了", false);
            }
        }
    }
}


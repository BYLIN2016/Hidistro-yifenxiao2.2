namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Votes)]
    public class Votes : AdminPage
    {
        protected DataList dlstVote;

        private void BindVote()
        {
            this.dlstVote.DataSource = StoreHelper.GetVotes();
            this.dlstVote.DataBind();
        }

        private void dlstVote_DeleteCommand(object sender, DataListCommandEventArgs e)
        {
            if (StoreHelper.DeleteVote(Convert.ToInt64(this.dlstVote.DataKeys[e.Item.ItemIndex])) > 0)
            {
                this.BindVote();
                this.ShowMsg("成功删除了选择的投票", true);
            }
            else
            {
                this.ShowMsg("删除投票失败", false);
            }
        }

        private void dlstVote_EditCommand(object sender, DataListCommandEventArgs e)
        {
            this.dlstVote.EditItemIndex = e.Item.ItemIndex;
            this.BindVote();
        }

        private void dlstVote_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if ((e.CommandName != "Sort") && (e.CommandName == "IsBackup"))
            {
                StoreHelper.SetVoteIsBackup(Convert.ToInt64(this.dlstVote.DataKeys[e.Item.ItemIndex]));
                this.BindVote();
            }
        }

        private void dlstVote_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                long voteId = Convert.ToInt64(this.dlstVote.DataKeys[e.Item.ItemIndex]);
                VoteInfo voteById = StoreHelper.GetVoteById(voteId);
                IList<VoteItemInfo> voteItems = StoreHelper.GetVoteItems(voteId);
                for (int i = 0; i < voteItems.Count; i++)
                {
                    if (voteById.VoteCounts != 0)
                    {
                        voteItems[i].Percentage = decimal.Parse(((voteItems[i].ItemCount / voteById.VoteCounts) * 100M).ToString("F", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        voteItems[i].Percentage = 0M;
                    }
                }
                GridView view = (GridView) e.Item.FindControl("grdVoteItem");
                if (view != null)
                {
                    view.DataSource = voteItems;
                    view.DataBind();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstVote.ItemDataBound += new DataListItemEventHandler(this.dlstVote_ItemDataBound);
            this.dlstVote.DeleteCommand += new DataListCommandEventHandler(this.dlstVote_DeleteCommand);
            this.dlstVote.ItemCommand += new DataListCommandEventHandler(this.dlstVote_ItemCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindVote();
            }
        }
    }
}


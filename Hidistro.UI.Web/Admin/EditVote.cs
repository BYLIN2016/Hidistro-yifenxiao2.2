namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Votes)]
    public class EditVote : AdminPage
    {
        protected Button btnEditVote;
        protected TextBox txtAddVoteName;
        protected TextBox txtMaxCheck;
        protected TextBox txtValues;
        private long voteId;

        private void btnEditVote_Click(object sender, EventArgs e)
        {
            if (StoreHelper.GetVoteCounts(this.voteId) > 0)
            {
                this.ShowMsg("投票已经开始，不能再对投票选项进行任何操作", false);
            }
            else
            {
                int num;
                VoteInfo vote = new VoteInfo();
                vote.VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim());
                vote.VoteId = this.voteId;
                if (int.TryParse(this.txtMaxCheck.Text.Trim(), out num))
                {
                    vote.MaxCheck = num;
                }
                else
                {
                    vote.MaxCheck = -2147483648;
                }
                IList<VoteItemInfo> list = null;
                if (!string.IsNullOrEmpty(this.txtValues.Text.Trim()))
                {
                    list = new List<VoteItemInfo>();
                    string[] strArray = this.txtValues.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        VoteItemInfo item = new VoteItemInfo();
                        if (strArray[i].Length > 60)
                        {
                            this.ShowMsg("投票选项长度限制在60个字符以内", false);
                            return;
                        }
                        item.VoteItemName = Globals.HtmlEncode(strArray[i]);
                        list.Add(item);
                    }
                }
                else
                {
                    this.ShowMsg("投票选项不能为空", false);
                    return;
                }
                vote.VoteItems = list;
                if (this.ValidationVote(vote))
                {
                    if (StoreHelper.UpdateVote(vote))
                    {
                        this.ShowMsg("修改投票成功", true);
                    }
                    else
                    {
                        this.ShowMsg("修改投票失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(this.Page.Request.QueryString["VoteId"], out this.voteId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditVote.Click += new EventHandler(this.btnEditVote_Click);
                if (!this.Page.IsPostBack)
                {
                    VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);
                    IList<VoteItemInfo> voteItems = StoreHelper.GetVoteItems(this.voteId);
                    if (voteById == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.txtAddVoteName.Text = Globals.HtmlDecode(voteById.VoteName);
                        this.txtMaxCheck.Text = voteById.MaxCheck.ToString();
                        string str = "";
                        foreach (VoteItemInfo info2 in voteItems)
                        {
                            str = str + Globals.HtmlDecode(info2.VoteItemName) + "\r\n";
                        }
                        this.txtValues.Text = str;
                    }
                }
            }
        }

        private bool ValidationVote(VoteInfo vote)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<VoteInfo>(vote, new string[] { "ValVote" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}


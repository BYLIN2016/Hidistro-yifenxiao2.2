namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddMyVote : DistributorPage
    {
        protected Button btnAddVote;
        protected CheckBox checkIsBackup;
        protected TextBox txtAddVoteName;
        protected HtmlGenericControl txtAddVoteNameTip;
        protected TextBox txtMaxCheck;
        protected HtmlGenericControl txtMaxCheckTip;
        protected TextBox txtValues;
        protected HtmlGenericControl txtValuesTip;

        private void btnAddVote_Click(object sender, EventArgs e)
        {
            int num;
            VoteInfo vote = new VoteInfo();
            vote.VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim());
            vote.IsBackup = this.checkIsBackup.Checked;
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
                if (SubsiteStoreHelper.CreateVote(vote) > 0)
                {
                    this.ShowMsg("成功的添加了一个投票", true);
                    this.txtAddVoteName.Text = string.Empty;
                    this.checkIsBackup.Checked = false;
                    this.txtMaxCheck.Text = string.Empty;
                    this.txtValues.Text = string.Empty;
                }
                else
                {
                    this.ShowMsg("添加投票失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddVote.Click += new EventHandler(this.btnAddVote_Click);
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


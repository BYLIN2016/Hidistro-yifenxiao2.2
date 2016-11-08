namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    public class Common_Vote : ThemedTemplatedRepeater
    {
        protected override void OnLoad(EventArgs e)
        {
            base.DataSource = CommentBrowser.GetVoteByIsShow();
            base.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (base.Items.Count != 0)
            {
                base.Render(writer);
                string str = (((((((string.Empty + "<script language=\"jscript\" type=\"text/javascript\">") + "function setcheckbox(checkbox){" + "var group = document.getElementsByName(checkbox.name);") + "var voteValue = document.getElementById(checkbox.name + '_Value');" + "var maxVote = parseInt(document.getElementById(checkbox.name + '_MaxVote').value);") + "voteValue.value =''; var n = 0;" + "for (index = 0;index < group.length;index ++){") + "if (group[index].checked){n++; voteValue.value += group[index].value + ',';}}" + "if (n > maxVote){var msg='") + "最多能投票：" + "'; alert(msg + maxVote); checkbox.checked = false; setcheckbox(checkbox);}}") + "function voteOption(voteId, voteItemId) {" + "window.document.location.href = applicationPath + \"/VoteResult.aspx?VoteId=\" + voteId + \"&&VoteItemId=\" + voteItemId;") + "}" + "</script>";
                writer.Write(str);
            }
        }
    }
}


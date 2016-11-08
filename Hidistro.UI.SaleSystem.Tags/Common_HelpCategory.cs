namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_HelpCategory : ThemedTemplatedRepeater
    {
        protected override void OnInit(EventArgs e)
        {
            base.DataSource = CommentBrowser.GetHelpTitleList();
            base.DataBind();
        }
    }
}


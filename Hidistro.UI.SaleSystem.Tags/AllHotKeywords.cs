namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class AllHotKeywords : ThemedTemplatedRepeater
    {
        protected override void OnLoad(EventArgs e)
        {
            base.DataSource = CommentBrowser.GetAllHotKeywords();
            base.DataBind();
        }
    }
}


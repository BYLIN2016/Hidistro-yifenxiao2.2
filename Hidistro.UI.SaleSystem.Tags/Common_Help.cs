namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_Help : ThemedTemplatedRepeater
    {
        protected override void OnLoad(EventArgs e)
        {
            base.DataSource = CommentBrowser.GetHelps();
            base.DataBind();
        }
    }
}


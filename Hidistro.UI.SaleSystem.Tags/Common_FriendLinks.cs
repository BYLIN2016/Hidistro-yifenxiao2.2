namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_FriendLinks : ThemedTemplatedRepeater
    {
        private int? maxNum;

        protected override void OnLoad(EventArgs e)
        {
            base.DataSource = CommentBrowser.GetFriendlyLinksIsVisible(this.MaxNum);
            base.DataBind();
        }

        public int? MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
    }
}


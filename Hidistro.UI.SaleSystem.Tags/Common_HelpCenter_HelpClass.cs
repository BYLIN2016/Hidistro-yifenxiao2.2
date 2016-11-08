namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Hidistro.Entities.Comments;

    public class Common_HelpCenter_HelpClass : ThemedTemplatedRepeater
    {
        
        private int _MaxNum;
        public const string TagID = "list_Common_HelpCenter_HelpClass";

        public Common_HelpCenter_HelpClass()
        {
            base.ID = "list_Common_HelpCenter_HelpClass";
        }

        private IList<HelpCategoryInfo> GetDataSource()
        {
            IList<HelpCategoryInfo> helpCategorys = new List<HelpCategoryInfo>();
            helpCategorys = CommentBrowser.GetHelpCategorys();
            if ((this.MaxNum > 0) && (this.MaxNum < helpCategorys.Count))
            {
                for (int i = helpCategorys.Count - 1; i >= this.MaxNum; i--)
                {
                    helpCategorys.RemoveAt(i);
                }
            }
            return helpCategorys;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.DataSource = this.GetDataSource();
                base.DataBind();
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public int MaxNum
        {
            
            get
            {
                return _MaxNum;
            }
            
            set
            {
                _MaxNum = value;
            }
        }
    }
}


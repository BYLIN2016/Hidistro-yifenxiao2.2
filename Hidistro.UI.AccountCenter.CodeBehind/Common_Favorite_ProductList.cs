namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_Favorite_ProductList : AscxTemplatedWebControl
    {
        private DataList dtlstFavorite;
        private CommandEventHandler ItemCommand;
        private int repeatColumns = 1;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;
        public const string TagID = "list_Common_Favorite_ProList";

        public event CommandEventHandler _ItemCommand
        {
            add
            {
                CommandEventHandler handler2;
                CommandEventHandler itemCommand = this.ItemCommand;
                do
                {
                    handler2 = itemCommand;
                    CommandEventHandler handler3 = (CommandEventHandler) Delegate.Combine(handler2, value);
                    itemCommand = Interlocked.CompareExchange<CommandEventHandler>(ref this.ItemCommand, handler3, handler2);
                }
                while (itemCommand != handler2);
            }
            remove
            {
                CommandEventHandler handler2;
                CommandEventHandler itemCommand = this.ItemCommand;
                do
                {
                    handler2 = itemCommand;
                    CommandEventHandler handler3 = (CommandEventHandler) Delegate.Remove(handler2, value);
                    itemCommand = Interlocked.CompareExchange<CommandEventHandler>(ref this.ItemCommand, handler3, handler2);
                }
                while (itemCommand != handler2);
            }
        }

        public Common_Favorite_ProductList()
        {
            base.ID = "list_Common_Favorite_ProList";
        }

        protected override void AttachChildControls()
        {
            this.dtlstFavorite = (DataList) this.FindControl("dtlstFavorite");
            this.dtlstFavorite.RepeatDirection = this.RepeatDirection;
            this.dtlstFavorite.RepeatColumns = this.RepeatColumns;
            this.dtlstFavorite.ItemCommand += new DataListCommandEventHandler(this.dtlstFavorite_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.dtlstFavorite.DataSource = this.DataSource;
            this.dtlstFavorite.DataBind();
        }

        private void dtlstFavorite_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Favorite_ProductList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyCollection DataKeys
        {
            get
            {
                return this.dtlstFavorite.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dtlstFavorite.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dtlstFavorite.DataSource = value;
            }
        }

        public int EditItemIndex
        {
            get
            {
                return this.dtlstFavorite.EditItemIndex;
            }
            set
            {
                this.dtlstFavorite.EditItemIndex = value;
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

        public DataListItemCollection Items
        {
            get
            {
                return this.dtlstFavorite.Items;
            }
        }

        public int RepeatColumns
        {
            get
            {
                return this.repeatColumns;
            }
            set
            {
                this.repeatColumns = value;
            }
        }

        public System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return this.repeatDirection;
            }
            set
            {
                this.repeatDirection = value;
            }
        }

        public delegate void CommandEventHandler(object sender, DataListCommandEventArgs e);
    }
}


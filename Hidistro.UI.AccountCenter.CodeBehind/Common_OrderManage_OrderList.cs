namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_OrderManage_OrderList : AscxTemplatedWebControl
    {
        private CommandEventHandler ItemCommand;
        private DataBindEventHandler ItemDataBound;
        private Grid listOrders;
        private ReBindDataEventHandler ReBindData;
        public const string TagID = "Common_OrderManage_OrderList";

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

        public event DataBindEventHandler _ItemDataBound
        {
            add
            {
                DataBindEventHandler handler2;
                DataBindEventHandler itemDataBound = this.ItemDataBound;
                do
                {
                    handler2 = itemDataBound;
                    DataBindEventHandler handler3 = (DataBindEventHandler) Delegate.Combine(handler2, value);
                    itemDataBound = Interlocked.CompareExchange<DataBindEventHandler>(ref this.ItemDataBound, handler3, handler2);
                }
                while (itemDataBound != handler2);
            }
            remove
            {
                DataBindEventHandler handler2;
                DataBindEventHandler itemDataBound = this.ItemDataBound;
                do
                {
                    handler2 = itemDataBound;
                    DataBindEventHandler handler3 = (DataBindEventHandler) Delegate.Remove(handler2, value);
                    itemDataBound = Interlocked.CompareExchange<DataBindEventHandler>(ref this.ItemDataBound, handler3, handler2);
                }
                while (itemDataBound != handler2);
            }
        }

        public event ReBindDataEventHandler _ReBindData
        {
            add
            {
                ReBindDataEventHandler handler2;
                ReBindDataEventHandler reBindData = this.ReBindData;
                do
                {
                    handler2 = reBindData;
                    ReBindDataEventHandler handler3 = (ReBindDataEventHandler) Delegate.Combine(handler2, value);
                    reBindData = Interlocked.CompareExchange<ReBindDataEventHandler>(ref this.ReBindData, handler3, handler2);
                }
                while (reBindData != handler2);
            }
            remove
            {
                ReBindDataEventHandler handler2;
                ReBindDataEventHandler reBindData = this.ReBindData;
                do
                {
                    handler2 = reBindData;
                    ReBindDataEventHandler handler3 = (ReBindDataEventHandler) Delegate.Remove(handler2, value);
                    reBindData = Interlocked.CompareExchange<ReBindDataEventHandler>(ref this.ReBindData, handler3, handler2);
                }
                while (reBindData != handler2);
            }
        }

        public Common_OrderManage_OrderList()
        {
            base.ID = "Common_OrderManage_OrderList";
        }

        protected override void AttachChildControls()
        {
            this.listOrders = (Grid) this.FindControl("listOrders");
            this.listOrders.RowDataBound += new GridViewRowEventHandler(this.listOrders_ItemDataBound);
            this.listOrders.RowCommand += new GridViewCommandEventHandler(this.listOrders_RowCommand);
            this.listOrders.ReBindData += new Grid.ReBindDataEventHandler(this.listOrders_ReBindData);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.listOrders.DataSource = this.DataSource;
            this.listOrders.DataBind();
        }

        private void listOrders_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            this.ItemDataBound(sender, e);
        }

        private void listOrders_ReBindData(object sender)
        {
            this.ReBindData(sender);
        }

        private void listOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.ItemCommand(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_OrderList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyArray DataKeys
        {
            get
            {
                return this.listOrders.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.listOrders.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.listOrders.DataSource = value;
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

        public SortAction SortOrder
        {
            get
            {
                return SortAction.Desc;
            }
        }

        public string SortOrderBy
        {
            get
            {
                if (this.listOrders != null)
                {
                    return this.listOrders.SortOrderBy;
                }
                return string.Empty;
            }
        }

        public delegate void CommandEventHandler(object sender, GridViewCommandEventArgs e);

        public delegate void DataBindEventHandler(object sender, GridViewRowEventArgs e);

        public delegate void ReBindDataEventHandler(object sender);
    }
}


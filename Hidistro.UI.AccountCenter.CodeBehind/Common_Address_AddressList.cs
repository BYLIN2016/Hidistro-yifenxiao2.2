namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_Address_AddressList : AscxTemplatedWebControl
    {
        private DataList dtlstRegionsSelect;
        private CommandEventHandler ItemCommand;
        public const string TagID = "list_Common_Consignee_ConsigneeList";

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

        public Common_Address_AddressList()
        {
            base.ID = "list_Common_Consignee_ConsigneeList";
        }

        protected override void AttachChildControls()
        {
            this.dtlstRegionsSelect = (DataList) this.FindControl("dtlstRegionsSelect");
            this.dtlstRegionsSelect.ItemCommand += new DataListCommandEventHandler(this.dtlstRegionsSelect_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.dtlstRegionsSelect.DataSource = this.DataSource;
            this.dtlstRegionsSelect.DataBind();
        }

        private void dtlstRegionsSelect_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Address_AddressList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyCollection DataKeys
        {
            get
            {
                return this.dtlstRegionsSelect.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dtlstRegionsSelect.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dtlstRegionsSelect.DataSource = value;
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
                return this.dtlstRegionsSelect.Items;
            }
        }

        public delegate void CommandEventHandler(object sender, DataListCommandEventArgs e);
    }
}


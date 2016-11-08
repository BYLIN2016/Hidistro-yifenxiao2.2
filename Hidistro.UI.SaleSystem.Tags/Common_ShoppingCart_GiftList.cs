namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_ShoppingCart_GiftList : AscxTemplatedWebControl
    {
        private DataList dataListGiftShoppingCrat;
        private DataList dataShopFreeGift;
        private DataListCommandEventHandler FreeItemCommand;
        private DataListCommandEventHandler ItemCommand;
        private Panel pnlFreeShopGiftCart;
        private Panel pnlShopGiftCart;
        public const string TagID = "Common_ShoppingCart_GiftList";

        public event DataListCommandEventHandler _FreeItemCommand
        {
            add
            {
                DataListCommandEventHandler handler2;
                DataListCommandEventHandler freeItemCommand = this.FreeItemCommand;
                do
                {
                    handler2 = freeItemCommand;
                    DataListCommandEventHandler handler3 = (DataListCommandEventHandler) Delegate.Combine(handler2, value);
                    freeItemCommand = Interlocked.CompareExchange<DataListCommandEventHandler>(ref this.FreeItemCommand, handler3, handler2);
                }
                while (freeItemCommand != handler2);
            }
            remove
            {
                DataListCommandEventHandler handler2;
                DataListCommandEventHandler freeItemCommand = this.FreeItemCommand;
                do
                {
                    handler2 = freeItemCommand;
                    DataListCommandEventHandler handler3 = (DataListCommandEventHandler) Delegate.Remove(handler2, value);
                    freeItemCommand = Interlocked.CompareExchange<DataListCommandEventHandler>(ref this.FreeItemCommand, handler3, handler2);
                }
                while (freeItemCommand != handler2);
            }
        }

        public event DataListCommandEventHandler _ItemCommand
        {
            add
            {
                DataListCommandEventHandler handler2;
                DataListCommandEventHandler itemCommand = this.ItemCommand;
                do
                {
                    handler2 = itemCommand;
                    DataListCommandEventHandler handler3 = (DataListCommandEventHandler) Delegate.Combine(handler2, value);
                    itemCommand = Interlocked.CompareExchange<DataListCommandEventHandler>(ref this.ItemCommand, handler3, handler2);
                }
                while (itemCommand != handler2);
            }
            remove
            {
                DataListCommandEventHandler handler2;
                DataListCommandEventHandler itemCommand = this.ItemCommand;
                do
                {
                    handler2 = itemCommand;
                    DataListCommandEventHandler handler3 = (DataListCommandEventHandler) Delegate.Remove(handler2, value);
                    itemCommand = Interlocked.CompareExchange<DataListCommandEventHandler>(ref this.ItemCommand, handler3, handler2);
                }
                while (itemCommand != handler2);
            }
        }

        public Common_ShoppingCart_GiftList()
        {
            base.ID = "Common_ShoppingCart_GiftList";
        }

        protected override void AttachChildControls()
        {
            this.dataListGiftShoppingCrat = (DataList) this.FindControl("dataListGiftShoppingCrat");
            this.dataShopFreeGift = (DataList) this.FindControl("dataShopFreeGift");
            this.pnlFreeShopGiftCart = (Panel) this.FindControl("pnlFreeShopGiftCart");
            this.pnlFreeShopGiftCart.Visible = false;
            this.pnlShopGiftCart = (Panel) this.FindControl("pnlShopGiftCart");
            this.pnlShopGiftCart.Visible = false;
            this.dataListGiftShoppingCrat.ItemCommand += new DataListCommandEventHandler(this.dataListGiftShoppingCrat_ItemCommand);
            this.dataShopFreeGift.ItemCommand += new DataListCommandEventHandler(this.dataShopFreeGift_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListGiftShoppingCrat.DataSource != null)
            {
                this.dataListGiftShoppingCrat.DataBind();
            }
            if (this.dataShopFreeGift.DataSource != null)
            {
                this.dataShopFreeGift.DataBind();
            }
        }

        private void dataListGiftShoppingCrat_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        private void dataShopFreeGift_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.FreeItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_GiftList.ascx";
            }
            base.OnInit(e);
        }

        public void ShowGiftCart(bool pointgift, bool freegift)
        {
            this.pnlShopGiftCart.Visible = pointgift;
            this.pnlFreeShopGiftCart.Visible = freegift;
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListGiftShoppingCrat.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListGiftShoppingCrat.DataSource = value;
            }
        }

        [Browsable(false)]
        public object FreeDataSource
        {
            get
            {
                return this.dataShopFreeGift.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataShopFreeGift.DataSource = value;
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
    }
}


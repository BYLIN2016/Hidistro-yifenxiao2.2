namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_Coupon_ChangeCouponList : AscxTemplatedWebControl
    {
        private DataList dataListCoupon;
        private CommandEventHandler ItemCommand;
        public const string TagID = "Common_Coupon_ChangeCouponList";

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

        public Common_Coupon_ChangeCouponList()
        {
            base.ID = "Common_Coupon_ChangeCouponList";
        }

        protected override void AttachChildControls()
        {
            this.dataListCoupon = (DataList) this.FindControl("dataListCoupon");
            this.dataListCoupon.ItemCommand += new DataListCommandEventHandler(this.dataListCoupon_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListCoupon.DataSource != null)
            {
                this.dataListCoupon.DataBind();
            }
        }

        private void dataListCoupon_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_ChangeCouponList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyCollection DataKeys
        {
            get
            {
                return this.dataListCoupon.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListCoupon.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListCoupon.DataSource = value;
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

        public delegate void CommandEventHandler(object sender, DataListCommandEventArgs e);
    }
}


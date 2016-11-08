namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_OrderManage_ReviewsOrderItems : AscxTemplatedWebControl
    {
        private Repeater rp_orderItem;
        public const string TagID = "Common_OrderManage_ReviewsOrderItems";

        public Common_OrderManage_ReviewsOrderItems()
        {
            base.ID = "Common_OrderManage_ReviewsOrderItems";
        }

        protected override void AttachChildControls()
        {
            this.rp_orderItem = (Repeater) this.FindControl("rp_orderItem");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.rp_orderItem.DataSource != null)
            {
                this.rp_orderItem.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_ReviewsOrderItems.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.rp_orderItem.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.rp_orderItem.DataSource = value;
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

        public RepeaterItemCollection Items
        {
            get
            {
                return this.rp_orderItem.Items;
            }
        }
    }
}


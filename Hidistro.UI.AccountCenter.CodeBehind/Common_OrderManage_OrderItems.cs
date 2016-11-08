namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_OrderManage_OrderItems : AscxTemplatedWebControl
    {
        private DataList dataListOrderItems;
        public const string TagID = "Common_OrderManage_OrderItems";

        public Common_OrderManage_OrderItems()
        {
            base.ID = "Common_OrderManage_OrderItems";
        }

        protected override void AttachChildControls()
        {
            this.dataListOrderItems = (DataList) this.FindControl("dataListOrderItems");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListOrderItems.DataSource != null)
            {
                this.dataListOrderItems.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_OrderItems.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListOrderItems.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListOrderItems.DataSource = value;
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


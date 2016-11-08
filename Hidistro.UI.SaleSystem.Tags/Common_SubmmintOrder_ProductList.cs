namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_SubmmintOrder_ProductList : AscxTemplatedWebControl
    {
        private Repeater dataListShoppingCrat;
        private Panel pnlShopProductCart;
        public const string TagID = "Common_SubmmintOrder_ProductList";

        public Common_SubmmintOrder_ProductList()
        {
            base.ID = "Common_SubmmintOrder_ProductList";
        }

        protected override void AttachChildControls()
        {
            this.dataListShoppingCrat = (Repeater) this.FindControl("dataListShoppingCrat");
            this.pnlShopProductCart = (Panel) this.FindControl("pnlShopProductCart");
            this.pnlShopProductCart.Visible = false;
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListShoppingCrat.DataSource != null)
            {
                this.dataListShoppingCrat.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_SubmmintOrder_ProductList.ascx";
            }
            base.OnInit(e);
        }

        public void ShowProductCart()
        {
            if (this.DataSource == null)
            {
                this.pnlShopProductCart.Visible = false;
            }
            else
            {
                this.pnlShopProductCart.Visible = true;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListShoppingCrat.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListShoppingCrat.DataSource = value;
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


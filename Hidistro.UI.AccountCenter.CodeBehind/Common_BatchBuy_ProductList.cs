namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_BatchBuy_ProductList : AscxTemplatedWebControl
    {
        private Grid grdProducts;
        public const string TagID = "Common_BatchBuy_ProductList";

        public Common_BatchBuy_ProductList()
        {
            base.ID = "Common_BatchBuy_ProductList";
        }

        protected override void AttachChildControls()
        {
            this.grdProducts = (Grid) this.FindControl("grdProducts");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.grdProducts.DataSource != null)
            {
                this.grdProducts.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_BatchBuy_ProductList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyArray DataKeys
        {
            get
            {
                return this.grdProducts.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.grdProducts.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.grdProducts.DataSource = value;
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

        public GridViewRowCollection Rows
        {
            get
            {
                return this.grdProducts.Rows;
            }
        }
    }
}


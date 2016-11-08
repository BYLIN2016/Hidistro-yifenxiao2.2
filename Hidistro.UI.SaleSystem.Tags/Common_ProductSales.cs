namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class Common_ProductSales : AscxTemplatedWebControl
    {
        public int maxNum = 6;
        private Repeater rp_productsales;

        protected override void AttachChildControls()
        {
            this.rp_productsales = (Repeater) this.FindControl("rp_productsales");
            DataTable lineItems = ProductBrowser.GetLineItems(Convert.ToInt32(this.Page.Request.QueryString["productId"]), this.maxNum);
            foreach (DataRow row in lineItems.Rows)
            {
                string str = (string) row["Username"];
                if (str.ToLower() == "anonymous")
                {
                    row["Username"] = "匿名用户";
                }
                else
                {
                    row["Username"] = str.Substring(0, 1) + "**" + str.Substring(str.Length - 1);
                }
            }
            this.rp_productsales.DataSource = lineItems;
            this.rp_productsales.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ViewProduct/Skin-Common_ProductSales.ascx";
            }
            base.OnInit(e);
        }

        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
    }
}


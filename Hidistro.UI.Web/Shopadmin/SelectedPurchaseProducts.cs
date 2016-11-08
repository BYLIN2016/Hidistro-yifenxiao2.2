namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class SelectedPurchaseProducts : DistributorPage
    {
        protected Grid grdSelectedProducts;
        protected Literal litPurchaseCount;

        private void BindAddedData()
        {
            IList<PurchaseShoppingCartItemInfo> purchaseShoppingCartItemInfos = SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos();
            int num = 0;
            decimal num2 = 0M;
            foreach (PurchaseShoppingCartItemInfo info in purchaseShoppingCartItemInfos)
            {
                num += info.Quantity;
                num2 += info.GetSubTotal();
            }
            this.grdSelectedProducts.DataSource = purchaseShoppingCartItemInfos;
            this.grdSelectedProducts.DataBind();
            this.litPurchaseCount.Text = string.Format("总共采购商品{0}件；总采购金额{1}元。", num, num2.ToString("F2"));
        }

        private void grdSelectedProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem((string) this.grdSelectedProducts.DataKeys[e.RowIndex].Value))
            {
                this.BindAddedData();
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdSelectedProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdSelectedProducts_RowDeleting);
            if (!base.IsPostBack)
            {
                this.BindAddedData();
            }
        }
    }
}


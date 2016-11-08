namespace Hidistro.UI.Web.Admin.product
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditStocks : AdminPage
    {
        protected Button btnOperationOK;
        protected Button btnSaveStock;
        protected Button btnTargetOK;
        protected Grid grdSelectedProducts;
        private string productIds = string.Empty;
        protected TextBox txtAddStock;
        protected TextBox txtTagetStock;

        private void BindProduct()
        {
            string str = this.Page.Request.QueryString["ProductIds"];
            if (!string.IsNullOrEmpty(str))
            {
                this.grdSelectedProducts.DataSource = ProductHelper.GetSkuStocks(str);
                this.grdSelectedProducts.DataBind();
            }
        }

        private void btnOperationOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsg("没有要修改的商品", false);
            }
            else
            {
                int result = 0;
                if (!int.TryParse(this.txtAddStock.Text, out result))
                {
                    this.ShowMsg("请输入正确的库存格式", false);
                }
                else if (ProductHelper.AddSkuStock(this.productIds, result))
                {
                    this.BindProduct();
                    this.ShowMsg("修改商品的库存成功", true);
                }
                else
                {
                    this.ShowMsg("修改商品的库存失败", false);
                }
            }
        }

        private void btnSaveStock_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> skuStocks = null;
            if (this.grdSelectedProducts.Rows.Count > 0)
            {
                skuStocks = new Dictionary<string, int>();
                foreach (GridViewRow row in this.grdSelectedProducts.Rows)
                {
                    int result = 0;
                    TextBox box = row.FindControl("txtStock") as TextBox;
                    if (int.TryParse(box.Text, out result))
                    {
                        string key = (string) this.grdSelectedProducts.DataKeys[row.RowIndex].Value;
                        skuStocks.Add(key, result);
                    }
                }
                if (skuStocks.Count > 0)
                {
                    if (ProductHelper.UpdateSkuStock(skuStocks))
                    {
                        this.CloseWindow();
                    }
                    else
                    {
                        this.ShowMsg("批量修改库存失败", false);
                    }
                }
                this.BindProduct();
            }
        }

        private void btnTargetOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsg("没有要修改的商品", false);
            }
            else
            {
                int result = 0;
                if (!int.TryParse(this.txtTagetStock.Text, out result))
                {
                    this.ShowMsg("请输入正确的库存格式", false);
                }
                else if (result < 0)
                {
                    this.ShowMsg("商品库存不能小于0", false);
                }
                else if (ProductHelper.UpdateSkuStock(this.productIds, result))
                {
                    this.BindProduct();
                    this.ShowMsg("修改商品的库存成功", true);
                }
                else
                {
                    this.ShowMsg("修改商品的库存失败", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.productIds = this.Page.Request.QueryString["productIds"];
            this.btnSaveStock.Click += new EventHandler(this.btnSaveStock_Click);
            this.btnTargetOK.Click += new EventHandler(this.btnTargetOK_Click);
            this.btnOperationOK.Click += new EventHandler(this.btnOperationOK_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }
    }
}


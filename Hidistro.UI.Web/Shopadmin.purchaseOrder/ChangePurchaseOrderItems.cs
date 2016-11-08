namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Commodities;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ChangePurchaseOrderItems : DistributorPage
    {
        protected Button btnSearch;
        protected AuthorizeProductLineDropDownList ddlProductLine;
        protected Grid grdAuthorizeProducts;
        protected Grid grdOrderItems;
        protected PageSize hrefPageSize;
        private string isAdd = string.Empty;
        protected Pager pager;
        protected Pager pager1;
        protected Panel pnlEmpty;
        protected Panel pnlHasStatus;
        private string productCode;
        private int? productLineId;
        private string productName;
        private string purchaseOrderId = string.Empty;
        protected TextBox txtProductCode;
        protected TextBox txtProductName;

        private void BindData()
        {
            ProductQuery entity = new ProductQuery();
            entity.PageSize = this.pager.PageSize;
            entity.PageIndex = this.pager.PageIndex;
            entity.ProductCode = this.productCode;
            entity.Keywords = this.productName;
            entity.ProductLineId = this.productLineId;
            if (this.grdAuthorizeProducts.SortOrder.ToLower() == "desc")
            {
                entity.SortOrder = SortAction.Desc;
            }
            entity.SortBy = this.grdAuthorizeProducts.SortOrderBy;
            Globals.EntityCoding(entity, true);
            DbQueryResult submitPuchaseProducts = SubSiteProducthelper.GetSubmitPuchaseProducts(entity);
            this.grdAuthorizeProducts.DataSource = submitPuchaseProducts.Data;
            this.grdAuthorizeProducts.DataBind();
            this.pager.TotalRecords = submitPuchaseProducts.TotalRecords;
            this.pager1.TotalRecords = submitPuchaseProducts.TotalRecords;
        }

        private void BindOrderItems()
        {
            PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
            {
                this.pnlEmpty.Visible = true;
                this.pnlHasStatus.Visible = false;
            }
            else
            {
                this.pnlHasStatus.Visible = true;
                this.pnlEmpty.Visible = false;
                if ((purchaseOrder != null) && (purchaseOrder.PurchaseOrderItems.Count > 0))
                {
                    this.grdOrderItems.DataSource = purchaseOrder.PurchaseOrderItems;
                    this.grdOrderItems.DataBind();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBindData(true, false);
        }

        private void grdAuthorizeProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable skusByProductId = SubSiteProducthelper.GetSkusByProductId(Convert.ToInt32(this.grdAuthorizeProducts.DataKeys[e.Row.RowIndex].Value));
                Grid grid = (Grid) e.Row.FindControl("grdSkus");
                if (grid != null)
                {
                    grid.DataSource = skusByProductId;
                    grid.DataBind();
                }
            }
        }

        private void grdOrderItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string skuId = e.CommandArgument.ToString();
            if (e.CommandName == "UPDATE_ITEMS")
            {
                if (SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId).PurchaseOrderItems.Count <= 1)
                {
                    this.ShowMsg("采购单的最后一件商品不允许删除", false);
                    return;
                }
                SubsiteSalesHelper.DeletePurchaseOrderItem(this.purchaseOrderId, skuId);
            }
            else if (e.CommandName == "UPDATE_QUANTITY")
            {
                int num2;
                Grid grid1 = (Grid) sender;
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                TextBox box = (TextBox) this.grdOrderItems.Rows[rowIndex].Cells[3].FindControl("txtItemNumber");
                if (!int.TryParse(box.Text.Trim(), out num2))
                {
                    this.ShowMsg("商品数量填写错误", false);
                    return;
                }
                int skuStock = SubsiteSalesHelper.GetSkuStock(skuId);
                if (num2 > skuStock)
                {
                    this.ShowMsg("此商品库存不够", false);
                    return;
                }
                if (num2 <= 0)
                {
                    this.ShowMsg("商品购买数量不能小于等于0", false);
                    return;
                }
                SubsiteSalesHelper.UpdatePurchaseOrderItemQuantity(this.purchaseOrderId, skuId, num2);
            }
            this.UpdatePurchaseOrder();
            this.BindOrderItems();
            this.ShowMsg("商品数量更新成功！", true);
            this.pager1.TotalRecords = this.pager.TotalRecords = 0;
            this.grdAuthorizeProducts.DataSource = null;
            this.grdAuthorizeProducts.DataBind();
        }

        public void grdSkus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int num2;
            Grid grid = (Grid) sender;
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            string skuId = (string) grid.DataKeys[rowIndex].Value;
            TextBox box = (TextBox) grid.Rows[rowIndex].Cells[1].FindControl("txtNum");
            LinkButton button = (LinkButton) grid.Rows[rowIndex].Cells[2].FindControl("lbtnAdd");
            if ((!int.TryParse(box.Text.Trim(), out num2) || (int.Parse(box.Text.Trim()) <= 0)) || box.Text.Trim().Contains("."))
            {
                this.ShowMsg("数量不能为空,必需为大于零的正整数", false);
            }
            else if ((e.CommandName == "add") && (button.Text == "添加"))
            {
                PurchaseShoppingCartItemInfo item = new PurchaseShoppingCartItemInfo();
                DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(skuId);
                if (num2 > ((int) skuContentBySku.Rows[0]["Stock"]))
                {
                    this.ShowMsg("商品库存不够", false);
                }
                else
                {
                    foreach (DataRow row in skuContentBySku.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                        {
                            object sKUContent = item.SKUContent;
                            item.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                        }
                    }
                    item.SkuId = skuId;
                    item.ProductId = (int) skuContentBySku.Rows[0]["ProductId"];
                    if (skuContentBySku.Rows[0]["SKU"] != DBNull.Value)
                    {
                        item.SKU = (string) skuContentBySku.Rows[0]["SKU"];
                    }
                    if (skuContentBySku.Rows[0]["Weight"] != DBNull.Value)
                    {
                        item.ItemWeight = (decimal) skuContentBySku.Rows[0]["Weight"];
                    }
                    item.ItemPurchasePrice = (decimal) skuContentBySku.Rows[0]["PurchasePrice"];
                    item.Quantity = num2;
                    item.ItemListPrice = (decimal) skuContentBySku.Rows[0]["SalePrice"];
                    item.ItemDescription = (string) skuContentBySku.Rows[0]["ProductName"];
                    if (skuContentBySku.Rows[0]["CostPrice"] != DBNull.Value)
                    {
                        item.CostPrice = (decimal) skuContentBySku.Rows[0]["CostPrice"];
                    }
                    if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
                    {
                        item.ThumbnailsUrl = (string) skuContentBySku.Rows[0]["ThumbnailUrl40"];
                    }
                    if (SubsiteSalesHelper.AddPurchaseOrderItem(item, this.purchaseOrderId))
                    {
                        this.UpdatePurchaseOrder();
                        this.BindOrderItems();
                        this.ReBindData(true, false);
                    }
                    else
                    {
                        this.ShowMsg("添加商品失败", false);
                    }
                }
            }
        }

        private void LoadParameters()
        {
            this.purchaseOrderId = this.Page.Request.QueryString["PurchaseOrderId"];
            this.isAdd = this.Page.Request.QueryString["isAdd"];
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
                {
                    this.productName = base.Server.UrlDecode(this.Page.Request.QueryString["productName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productLineId"]))
                {
                    this.productLineId = new int?(int.Parse(this.Page.Request.QueryString["productLineId"], NumberStyles.None));
                }
                this.txtProductCode.Text = this.productCode;
                this.txtProductName.Text = this.productName;
            }
            else
            {
                this.productCode = this.txtProductCode.Text;
                this.productName = this.txtProductName.Text;
                this.productLineId = this.ddlProductLine.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdOrderItems.RowCommand += new GridViewCommandEventHandler(this.grdOrderItems_RowCommand);
            this.grdAuthorizeProducts.RowDataBound += new GridViewRowEventHandler(this.grdAuthorizeProducts_RowDataBound);
            if (!this.Page.IsPostBack)
            {
                this.ddlProductLine.DataBind();
                this.ddlProductLine.SelectedValue = this.productLineId;
                this.BindOrderItems();
                if (!string.IsNullOrEmpty(this.isAdd))
                {
                    this.BindData();
                }
            }
        }

        private void ReBindData(bool isSearch, bool reBindByGet)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(this.txtProductCode.Text))
            {
                queryStrings.Add("productCode", this.txtProductCode.Text);
            }
            if (!string.IsNullOrEmpty(this.txtProductName.Text))
            {
                queryStrings.Add("productName", this.txtProductName.Text);
            }
            queryStrings.Add("productLineId", this.ddlProductLine.SelectedValue.ToString());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            queryStrings.Add("PurchaseOrderId", this.purchaseOrderId);
            queryStrings.Add("isAdd", "true");
            base.ReloadPage(queryStrings);
        }

        private void UpdatePurchaseOrder()
        {
            decimal num = 0M;
            PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
            foreach (PurchaseOrderItemInfo info2 in purchaseOrder.PurchaseOrderItems)
            {
                new PurchaseOrderItemInfo();
                num += info2.ItemWeight * info2.Quantity;
            }
            purchaseOrder.Weight = num;
            SubsiteSalesHelper.UpdatePurchaseOrder(purchaseOrder);
        }
    }
}


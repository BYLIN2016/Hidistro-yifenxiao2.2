namespace Hidistro.UI.Web.Shopadmin
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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class PurchaseProduct : DistributorPage
    {
        protected Button btnSearch;
        protected AuthorizeProductLineDropDownList ddlProductLine;
        protected Grid grdAuthorizeProducts;
        protected PageSize hrefPageSize;
        protected LinkButton lkbtnAdddCheck;
        protected LinkButton lkbtncancleCheck;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        private int? productLineId;
        private string productName;
        protected TextBox txtProductCode;
        protected TextBox txtProductName;

        private PurchaseShoppingCartItemInfo AddPurchaseShoppingCartItemInfo(string skuId, int quantity)
        {
            PurchaseShoppingCartItemInfo info = new PurchaseShoppingCartItemInfo();
            DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(skuId);
            if (quantity > ((int) skuContentBySku.Rows[0]["Stock"]))
            {
                return null;
            }
            foreach (DataRow row in skuContentBySku.Rows)
            {
                if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                {
                    object sKUContent = info.SKUContent;
                    info.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                }
            }
            info.SkuId = skuId;
            info.ProductId = (int) skuContentBySku.Rows[0]["ProductId"];
            if (skuContentBySku.Rows[0]["SKU"] != DBNull.Value)
            {
                info.SKU = (string) skuContentBySku.Rows[0]["SKU"];
            }
            if (skuContentBySku.Rows[0]["Weight"] != DBNull.Value)
            {
                info.ItemWeight = (decimal) skuContentBySku.Rows[0]["Weight"];
            }
            info.ItemPurchasePrice = (decimal) skuContentBySku.Rows[0]["PurchasePrice"];
            info.Quantity = quantity;
            info.ItemListPrice = (decimal) skuContentBySku.Rows[0]["SalePrice"];
            info.ItemDescription = (string) skuContentBySku.Rows[0]["ProductName"];
            if (skuContentBySku.Rows[0]["CostPrice"] != DBNull.Value)
            {
                info.CostPrice = (decimal) skuContentBySku.Rows[0]["CostPrice"];
            }
            if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
            {
                info.ThumbnailsUrl = (string) skuContentBySku.Rows[0]["ThumbnailUrl40"];
            }
            return info;
        }

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
            else if (e.CommandName == "add")
            {
                if (button.Text == "添加")
                {
                    PurchaseShoppingCartItemInfo item = new PurchaseShoppingCartItemInfo();
                    item = this.AddPurchaseShoppingCartItemInfo(skuId, num2);
                    if (item == null)
                    {
                        this.ShowMsg("商品库存不够", false);
                    }
                    else if (SubsiteSalesHelper.AddPurchaseItem(item))
                    {
                        this.BindData();
                    }
                    else
                    {
                        this.ShowMsg("添加商品失败", false);
                    }
                }
                else if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem(skuId))
                {
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("删除失败", false);
                }
            }
        }

        public void grdSkus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Grid grid = (Grid) sender;
                string str = grid.DataKeys[e.Row.RowIndex].Value.ToString();
                LinkButton button = (LinkButton) e.Row.FindControl("lbtnAdd");
                foreach (PurchaseShoppingCartItemInfo info in SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos())
                {
                    if (info.SkuId == str)
                    {
                        button.Text = "取消";
                        button.Attributes.Add("style", "color:Red");
                    }
                }
            }
        }

        private void lkbtnAdddCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = true;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (GridViewRow row in this.grdAuthorizeProducts.Rows)
            {
                GridView view = row.FindControl("grdSkus") as GridView;
                foreach (GridViewRow row2 in view.Rows)
                {
                    CheckBox box = (CheckBox) row2.FindControl("checkboxCol");
                    TextBox box2 = row2.FindControl("txtNum") as TextBox;
                    if ((box != null) && box.Checked)
                    {
                        int num2;
                        num++;
                        if ((!int.TryParse(box2.Text.Trim(), out num2) || (int.Parse(box2.Text.Trim()) <= 0)) || box2.Text.Trim().Contains("."))
                        {
                            flag = false;
                            break;
                        }
                        dictionary.Add(view.DataKeys[row2.RowIndex].Value.ToString(), num2);
                    }
                }
                if (!flag)
                {
                    break;
                }
            }
            if (num == 0)
            {
                this.ShowMsg("请先选择要添加的商品", false);
            }
            else if (!flag)
            {
                this.ShowMsg("数量不能为空,必需为大于零的正整数", false);
            }
            else
            {
                int num3 = 0;
                foreach (KeyValuePair<string, int> pair in dictionary)
                {
                    PurchaseShoppingCartItemInfo item = new PurchaseShoppingCartItemInfo();
                    item = this.AddPurchaseShoppingCartItemInfo(pair.Key, Convert.ToInt32(pair.Value));
                    if (item == null)
                    {
                        this.ShowMsg("商品库存不够", false);
                        break;
                    }
                    if (SubsiteSalesHelper.AddPurchaseItem(item))
                    {
                        num3++;
                    }
                }
                if (num3 > 0)
                {
                    this.ShowMsg(string.Format("成功添加了{0}件商品", num3), true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("添加商品失败", false);
                }
            }
        }

        private void lkbtncancleCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = true;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (GridViewRow row in this.grdAuthorizeProducts.Rows)
            {
                GridView view = row.FindControl("grdSkus") as GridView;
                foreach (GridViewRow row2 in view.Rows)
                {
                    CheckBox box = (CheckBox) row2.FindControl("checkboxCol");
                    TextBox box2 = row2.FindControl("txtNum") as TextBox;
                    if ((box != null) && box.Checked)
                    {
                        int num2;
                        num++;
                        if ((!int.TryParse(box2.Text.Trim(), out num2) || (int.Parse(box2.Text.Trim()) <= 0)) || box2.Text.Trim().Contains("."))
                        {
                            flag = false;
                            break;
                        }
                        dictionary.Add((string) view.DataKeys[row2.RowIndex].Value, num2);
                    }
                }
                if (!flag)
                {
                    break;
                }
            }
            if (num == 0)
            {
                this.ShowMsg("请先选择要添加的商品", false);
            }
            else if (!flag)
            {
                this.ShowMsg("数量不能为空,必需为大于零的正整数", false);
            }
            else
            {
                int num3 = 0;
                foreach (KeyValuePair<string, int> pair in dictionary)
                {
                    if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem(pair.Key))
                    {
                        num3++;
                    }
                }
                if (num3 > 0)
                {
                    this.ShowMsg(string.Format("成功取消了{0}件商品", num3), true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("取消商品失败", false);
                }
            }
        }

        private void LoadParameters()
        {
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
            this.lkbtnAdddCheck.Click += new EventHandler(this.lkbtnAdddCheck_Click);
            this.lkbtncancleCheck.Click += new EventHandler(this.lkbtncancleCheck_Click);
            this.grdAuthorizeProducts.RowDataBound += new GridViewRowEventHandler(this.grdAuthorizeProducts_RowDataBound);
            if (!base.IsPostBack)
            {
                this.ddlProductLine.DataBind();
                this.ddlProductLine.SelectedValue = this.productLineId;
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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
            base.ReloadPage(queryStrings);
        }
    }
}


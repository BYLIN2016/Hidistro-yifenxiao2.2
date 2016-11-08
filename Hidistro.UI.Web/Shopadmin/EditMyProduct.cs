namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class EditMyProduct : DistributorPage
    {
        protected Button btnUpdate;
        private int categoryId;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductLineDropDownList dropProductLines;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl fckDescription;
        protected HtmlGenericControl li_tags;
        protected Literal litCategoryName;
        protected Literal litLowestSalePrice;
        protected Literal litProductCode;
        protected ProductTagsLiteral litralProductTag;
        protected Literal litUnit;
        protected HyperLink lnkEditCategory;
        protected ProductAttributeDisplay ProductAttributeDisplay1;
        private int productId;
        protected RadioButton radInStock;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;
        protected TrimTextBox txtDisplaySequence;
        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox txtMetaDescription;
        protected TrimTextBox txtMetaKeywords;
        protected TrimTextBox txtProductName;
        protected TrimTextBox txtProductTag;
        protected TrimTextBox txtShortDescription;
        protected TrimTextBox txtSkuPrice;
        protected TrimTextBox txtTitle;
        protected ImageUploader uploader1;
        protected ImageUploader uploader2;
        protected ImageUploader uploader3;
        protected ImageUploader uploader4;
        protected ImageUploader uploader5;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            IList<int> list2;
            if (this.categoryId == 0)
            {
                this.categoryId = (int) this.ViewState["ProductCategoryId"];
            }
            if (this.categoryId == 0)
            {
                this.ShowMsg("请选择商品分类", false);
                return;
            }
            ProductInfo product = new ProductInfo();
            product.ProductId = this.productId;
            product.CategoryId = this.categoryId;
            CategoryInfo category = SubsiteCatalogHelper.GetCategory(product.CategoryId);
            if (category != null)
            {
                product.MainCategoryPath = category.Path + "|";
            }
            product.ProductName = this.txtProductName.Text;
            product.ShortDescription = this.txtShortDescription.Text;
            product.Description = this.fckDescription.Text;
            product.Title = this.txtTitle.Text;
            product.MetaDescription = this.txtMetaDescription.Text;
            product.MetaKeywords = this.txtMetaKeywords.Text;
            if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
            {
                product.MarketPrice = new decimal?(decimal.Parse(this.txtMarketPrice.Text));
            }
            Dictionary<string, decimal> skuSalePrice = null;
            if (!string.IsNullOrEmpty(this.txtSkuPrice.Text))
            {
                skuSalePrice = this.GetSkuPrices();
            }
            ProductSaleStatus onStock = ProductSaleStatus.OnStock;
            if (this.radInStock.Checked)
            {
                onStock = ProductSaleStatus.OnStock;
            }
            if (this.radUnSales.Checked)
            {
                onStock = ProductSaleStatus.UnSale;
            }
            if (this.radOnSales.Checked)
            {
                onStock = ProductSaleStatus.OnSale;
            }
            if (onStock != ProductSaleStatus.OnSale)
            {
                goto Label_0221;
            }
            bool flag = false;
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(this.txtSkuPrice.Text);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list != null) && (list.Count > 0))
                {
                    foreach (XmlNode node in list)
                    {
                        if (decimal.Parse(node.Attributes["price"].Value) < decimal.Parse(this.litLowestSalePrice.Text))
                        {
                            flag = true;
                            goto Label_0210;
                        }
                    }
                }
            }
            catch
            {
            }
        Label_0210:
            if (flag)
            {
                this.ShowMsg("此商品的一口价已经低于了最低零售价,不允许上架", false);
                return;
            }
        Label_0221:
            list2 = new List<int>();
            if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
            {
                string str = this.txtProductTag.Text.Trim();
                string[] strArray = null;
                if (str.Contains(","))
                {
                    strArray = str.Split(new char[] { ',' });
                }
                else
                {
                    strArray = new string[] { str };
                }
                foreach (string str2 in strArray)
                {
                    list2.Add(Convert.ToInt32(str2));
                }
            }
            product.SaleStatus = onStock;
            int result = 0;
            int.TryParse(this.txtDisplaySequence.Text, out result);
            product.DisplaySequence = result;
            if (SubSiteProducthelper.UpdateProduct(product, skuSalePrice, list2))
            {
                this.litralProductTag.SelectedValue = list2;
                this.ShowMsg("修改商品成功", true);
            }
        }

        private Dictionary<string, decimal> GetSkuPrices()
        {
            XmlDocument document = new XmlDocument();
            Dictionary<string, decimal> dictionary = null;
            try
            {
                document.LoadXml(this.txtSkuPrice.Text);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }
                IList<SKUItem> skus = SubSiteProducthelper.GetSkus(this.productId.ToString());
                dictionary = new Dictionary<string, decimal>();
                foreach (XmlNode node in list)
                {
                    string key = node.Attributes["skuId"].Value;
                    decimal num = decimal.Parse(node.Attributes["price"].Value);
                    foreach (SKUItem item in skus)
                    {
                        if ((item.SkuId == key) && (item.SalePrice != num))
                        {
                            dictionary.Add(key, num);
                        }
                    }
                }
            }
            catch
            {
            }
            return dictionary;
        }

        private void LoadProudct(ProductInfo product)
        {
            this.txtProductName.Text = product.ProductName;
            this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
            this.litLowestSalePrice.Text = product.LowestSalePrice.ToString("F2");
            if (product.MarketPrice.HasValue)
            {
                this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            this.litProductCode.Text = product.ProductCode;
            this.litUnit.Text = product.Unit;
            this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
            this.txtTitle.Text = product.Title;
            this.txtMetaDescription.Text = product.MetaDescription;
            this.txtMetaKeywords.Text = product.MetaKeywords;
            if (product.SaleStatus == ProductSaleStatus.OnSale)
            {
                this.radOnSales.Checked = true;
            }
            else if (product.SaleStatus == ProductSaleStatus.UnSale)
            {
                this.radUnSales.Checked = true;
            }
            else
            {
                this.radInStock.Checked = true;
            }
            this.uploader1.UploadedImageUrl = product.ImageUrl1;
            this.uploader2.UploadedImageUrl = product.ImageUrl2;
            this.uploader3.UploadedImageUrl = product.ImageUrl3;
            this.uploader4.UploadedImageUrl = product.ImageUrl4;
            this.uploader5.UploadedImageUrl = product.ImageUrl5;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["ProductId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
                if (!this.Page.IsPostBack)
                {
                    ProductInfo product = SubSiteProducthelper.GetProduct(this.productId);
                    if (product == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                        {
                            this.litCategoryName.Text = SubsiteCatalogHelper.GetFullCategory(this.categoryId);
                            this.ViewState["ProductCategoryId"] = this.categoryId;
                            this.lnkEditCategory.NavigateUrl = "SelectMyCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            this.litCategoryName.Text = SubsiteCatalogHelper.GetFullCategory(product.CategoryId);
                            this.ViewState["ProductCategoryId"] = product.CategoryId;
                            this.lnkEditCategory.NavigateUrl = "SelectMyCategory.aspx?categoryId=" + product.CategoryId.ToString(CultureInfo.InvariantCulture);
                        }
                        this.lnkEditCategory.NavigateUrl = this.lnkEditCategory.NavigateUrl + "&productId=" + product.ProductId.ToString(CultureInfo.InvariantCulture);
                        IList<int> productTags = new List<int>();
                        productTags = SubSiteProducthelper.GetProductTags(this.productId);
                        this.litralProductTag.SelectedValue = productTags;
                        if (productTags.Count > 0)
                        {
                            foreach (int num in productTags)
                            {
                                this.txtProductTag.Text = this.txtProductTag.Text + num.ToString() + ",";
                            }
                            this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
                        }
                        this.dropProductTypes.Enabled = false;
                        this.dropProductTypes.DataBind();
                        this.dropProductTypes.SelectedValue = product.TypeId;
                        this.dropProductLines.Enabled = false;
                        this.dropProductLines.DataBind();
                        this.dropProductLines.SelectedValue = new int?(product.LineId);
                        this.dropBrandCategories.Enabled = false;
                        this.dropBrandCategories.DataBind();
                        this.dropBrandCategories.SelectedValue = product.BrandId;
                        this.LoadProudct(product);
                    }
                }
            }
        }
    }
}


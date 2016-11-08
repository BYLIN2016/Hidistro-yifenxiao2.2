namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddProducts)]
    public class AddProduct : ProductBasePage
    {
        protected Button btnAdd;
        private int categoryId;
        protected CheckBox chkPenetration;
        protected CheckBox chkSkuEnabled;
        protected CheckBox ckbIsDownPic;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductLineDropDownList dropProductLines;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl editDescription;
        protected HtmlGenericControl l_tags;
        protected Literal litCategoryName;
        protected ProductTagsLiteral litralProductTag;
        protected HyperLink lnkEditCategory;
        protected RadioButton radInStock;
        protected YesNoRadioButtonList radlEnableMemberDiscount;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;
        protected Script Script1;
        protected Script Script2;
        protected TrimTextBox txtAlertStock;
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtDistributorPrices;
        protected TrimTextBox txtLowestSalePrice;
        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox txtMemberPrices;
        protected TrimTextBox txtMetaDescription;
        protected TrimTextBox txtMetaKeywords;
        protected TrimTextBox txtProductCode;
        protected TrimTextBox txtProductName;
        protected TrimTextBox txtProductTag;
        protected TrimTextBox txtPurchasePrice;
        protected TrimTextBox txtSalePrice;
        protected TrimTextBox txtShortDescription;
        protected TrimTextBox txtSku;
        protected TrimTextBox txtSkus;
        protected TrimTextBox txtStock;
        protected TrimTextBox txtTitle;
        protected TrimTextBox txtUnit;
        protected TrimTextBox txtWeight;
        protected ImageUploader uploader1;
        protected ImageUploader uploader2;
        protected ImageUploader uploader3;
        protected ImageUploader uploader4;
        protected ImageUploader uploader5;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            int num3;
            decimal num4;
            decimal num5;
            decimal num6;
            decimal? nullable;
            decimal? nullable2;
            int? nullable3;
            if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num4, out num5, out num6, out nullable, out nullable2, out num, out num2, out nullable3, out num3))
            {
                if (!this.chkSkuEnabled.Checked)
                {
                    if (num6 <= 0M)
                    {
                        this.ShowMsg("商品一口价必须大于0", false);
                        return;
                    }
                    if (nullable.HasValue && (nullable.Value >= num6))
                    {
                        this.ShowMsg("商品成本价必须小于商品一口价", false);
                        return;
                    }
                    if (num4 > num5)
                    {
                        this.ShowMsg("分销商采购价必须要小于其最低零售价", false);
                        return;
                    }
                }
                string text = this.editDescription.Text;
                if (this.ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                ProductInfo target = new ProductInfo {
                    CategoryId = this.categoryId,
                    TypeId = this.dropProductTypes.SelectedValue,
                    ProductName = this.txtProductName.Text,
                    ProductCode = this.txtProductCode.Text,
                    LineId = num3,
                    LowestSalePrice = num5,
                    MarketPrice = nullable2,
                    Unit = this.txtUnit.Text,
                    ImageUrl1 = this.uploader1.UploadedImageUrl,
                    ImageUrl2 = this.uploader2.UploadedImageUrl,
                    ImageUrl3 = this.uploader3.UploadedImageUrl,
                    ImageUrl4 = this.uploader4.UploadedImageUrl,
                    ImageUrl5 = this.uploader5.UploadedImageUrl,
                    ThumbnailUrl40 = this.uploader1.ThumbnailUrl40,
                    ThumbnailUrl60 = this.uploader1.ThumbnailUrl60,
                    ThumbnailUrl100 = this.uploader1.ThumbnailUrl100,
                    ThumbnailUrl160 = this.uploader1.ThumbnailUrl160,
                    ThumbnailUrl180 = this.uploader1.ThumbnailUrl180,
                    ThumbnailUrl220 = this.uploader1.ThumbnailUrl220,
                    ThumbnailUrl310 = this.uploader1.ThumbnailUrl310,
                    ThumbnailUrl410 = this.uploader1.ThumbnailUrl410,
                    ShortDescription = this.txtShortDescription.Text,
                    Description = (!string.IsNullOrEmpty(text) && (text.Length > 0)) ? text : null,
                    PenetrationStatus = this.chkPenetration.Checked ? PenetrationStatus.Already : PenetrationStatus.Notyet,
                    Title = this.txtTitle.Text,
                    MetaDescription = this.txtMetaDescription.Text,
                    MetaKeywords = this.txtMetaKeywords.Text,
                    AddedDate = DateTime.Now,
                    BrandId = this.dropBrandCategories.SelectedValue,
                    MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|"
                };
                ProductSaleStatus onSale = ProductSaleStatus.OnSale;
                if (this.radInStock.Checked)
                {
                    onSale = ProductSaleStatus.OnStock;
                }
                if (this.radUnSales.Checked)
                {
                    onSale = ProductSaleStatus.UnSale;
                }
                if (this.radOnSales.Checked)
                {
                    onSale = ProductSaleStatus.OnSale;
                }
                target.SaleStatus = onSale;
                Dictionary<string, SKUItem> skus = null;
                Dictionary<int, IList<int>> attrs = null;
                if (this.chkSkuEnabled.Checked)
                {
                    target.HasSKU = true;
                    skus = base.GetSkus(this.txtSkus.Text);
                }
                else
                {
                    Dictionary<string, SKUItem> dictionary3 = new Dictionary<string, SKUItem>();
                    SKUItem item = new SKUItem {
                        SkuId = "0",
                        SKU = this.txtSku.Text,
                        SalePrice = num6,
                        CostPrice = nullable.HasValue ? nullable.Value : 0M,
                        PurchasePrice = num4,
                        Stock = num,
                        AlertStock = num2,
                        Weight = nullable3.HasValue ? nullable3.Value : 0
                    };
                    dictionary3.Add("0", item);
                    skus = dictionary3;
                    if (this.txtMemberPrices.Text.Length > 0)
                    {
                        base.GetMemberPrices(skus["0"], this.txtMemberPrices.Text);
                    }
                    if (this.txtDistributorPrices.Text.Length > 0)
                    {
                        base.GetDistributorPrices(skus["0"], this.txtDistributorPrices.Text);
                    }
                }
                if (!string.IsNullOrEmpty(this.txtAttributes.Text) && (this.txtAttributes.Text.Length > 0))
                {
                    attrs = base.GetAttributes(this.txtAttributes.Text);
                }
                ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(target, new string[] { "AddProduct" });
                if (!validateResults.IsValid)
                {
                    this.ShowMsg(validateResults);
                }
                else
                {
                    IList<int> tagsId = new List<int>();
                    if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
                    {
                        string str2 = this.txtProductTag.Text.Trim();
                        string[] strArray = null;
                        if (str2.Contains(","))
                        {
                            strArray = str2.Split(new char[] { ',' });
                        }
                        else
                        {
                            strArray = new string[] { str2 };
                        }
                        foreach (string str3 in strArray)
                        {
                            tagsId.Add(Convert.ToInt32(str3));
                        }
                    }
                    switch (ProductHelper.AddProduct(target, skus, attrs, tagsId))
                    {
                        case ProductActionStatus.Success:
                            this.ShowMsg("添加商品成功", true);
                            base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddProductComplete.aspx?categoryId={0}&productId={1}", this.categoryId, target.ProductId)), true);
                            return;

                        case ProductActionStatus.AttributeError:
                            this.ShowMsg("添加商品失败，保存商品属性时出错", false);
                            return;

                        case ProductActionStatus.DuplicateName:
                            this.ShowMsg("添加商品失败，商品名称不能重复", false);
                            return;

                        case ProductActionStatus.DuplicateSKU:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.SKUError:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.ProductTagEroor:
                            this.ShowMsg("添加商品失败，保存商品标签时出错", false);
                            return;
                    }
                    this.ShowMsg("添加商品失败，未知错误", false);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                base.DoCallback();
            }
            else if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
            }
            if (this.txtProductName.Text.Trim() == "")
            {
                this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                if (category == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.litralProductTag.Text))
                    {
                        this.l_tags.Visible = true;
                    }
                    this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
                    this.dropProductTypes.DataBind();
                    this.dropProductTypes.SelectedValue = category.AssociatedProductType;
                    this.dropProductLines.DataBind();
                    this.dropBrandCategories.DataBind();
                    this.txtProductCode.Text = this.txtSku.Text = category.SKUPrefix + new Random(DateTime.Now.Millisecond).Next(1, 0x1869f).ToString(CultureInfo.InvariantCulture).PadLeft(5, '0');
                }
            }
        }

        private bool ValidateConverts(bool skuEnabled, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out int? weight, out int lineId)
        {
            int num4;
            decimal num6;
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            lineId = num4 = 0;
            stock = alertStock = num4;
            salePrice = num6 = 0M;
            purchasePrice = lowestSalePrice = num6;
            if (!this.dropProductLines.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择商品所属的产品线");
            }
            else
            {
                lineId = this.dropProductLines.SelectedValue.Value;
            }
            if (this.txtProductCode.Text.Length > 20)
            {
                str = str + Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
            }
            if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.txtMarketPrice.Text, out num))
                {
                    marketPrice = new decimal?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的市场价");
                }
            }
            if (string.IsNullOrEmpty(this.txtLowestSalePrice.Text) || !decimal.TryParse(this.txtLowestSalePrice.Text, out lowestSalePrice))
            {
                str = str + Formatter.FormatErrorMessage("请正确填写分销商最低零售价");
            }
            if (!skuEnabled)
            {
                if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品一口价");
                }
                if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
                {
                    decimal num2;
                    if (decimal.TryParse(this.txtCostPrice.Text, out num2))
                    {
                        costPrice = new decimal?(num2);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的成本价");
                    }
                }
                if (string.IsNullOrEmpty(this.txtPurchasePrice.Text) || !decimal.TryParse(this.txtPurchasePrice.Text, out purchasePrice))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写分销商采购价格");
                }
                if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的库存数量");
                }
                if (string.IsNullOrEmpty(this.txtAlertStock.Text) || !int.TryParse(this.txtAlertStock.Text, out alertStock))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的警戒库存");
                }
                if (!string.IsNullOrEmpty(this.txtWeight.Text))
                {
                    int num3;
                    if (int.TryParse(this.txtWeight.Text, out num3))
                    {
                        weight = new int?(num3);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的重量");
                    }
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}


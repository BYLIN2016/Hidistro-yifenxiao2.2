namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditProduct : ProductBasePage
    {
        protected Button btnSave;
        private int categoryId;
        protected CheckBox chkPenetration;
        protected CheckBox chkSkuEnabled;
        protected CheckBox ckbIsDownPic;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductLineDropDownList dropProductLines;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl fckDescription;
        protected HtmlInputHidden hdlineId;
        protected HyperLink hlinkDistributor;
        protected HtmlGenericControl l_tags;
        protected Literal litCategoryName;
        protected ProductTagsLiteral litralProductTag;
        protected HyperLink lnkEditCategory;
        private int productId;
        protected RadioButton radInStock;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;
        protected Script Script1;
        protected Script Script2;
        private string toline = "";
        protected TrimTextBox txtAlertStock;
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtDisplaySequence;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            int num3;
            decimal num4;
            decimal num5;
            decimal num6;
            decimal? nullable;
            decimal? nullable2;
            decimal? nullable3;
            if (this.categoryId == 0)
            {
                this.categoryId = (int) this.ViewState["ProductCategoryId"];
            }
            if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num, out num4, out num5, out num6, out nullable, out nullable2, out num2, out num3, out nullable3))
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
                        this.ShowMsg("分销商采购价必须要小于等于其最低零售价", false);
                        return;
                    }
                }
                string text = this.fckDescription.Text;
                if (this.ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                ProductInfo info3 = new ProductInfo();
                info3.ProductId = this.productId;
                info3.CategoryId = this.categoryId;
                info3.TypeId = this.dropProductTypes.SelectedValue;
                info3.ProductName = this.txtProductName.Text;
                info3.ProductCode = this.txtProductCode.Text;
                info3.DisplaySequence = num;
                info3.LineId = this.dropProductLines.SelectedValue.Value;
                info3.LowestSalePrice = num5;
                info3.MarketPrice = nullable2;
                info3.Unit = this.txtUnit.Text;
                info3.ImageUrl1 = this.uploader1.UploadedImageUrl;
                info3.ImageUrl2 = this.uploader2.UploadedImageUrl;
                info3.ImageUrl3 = this.uploader3.UploadedImageUrl;
                info3.ImageUrl4 = this.uploader4.UploadedImageUrl;
                info3.ImageUrl5 = this.uploader5.UploadedImageUrl;
                info3.ThumbnailUrl40 = this.uploader1.ThumbnailUrl40;
                info3.ThumbnailUrl60 = this.uploader1.ThumbnailUrl60;
                info3.ThumbnailUrl100 = this.uploader1.ThumbnailUrl100;
                info3.ThumbnailUrl160 = this.uploader1.ThumbnailUrl160;
                info3.ThumbnailUrl180 = this.uploader1.ThumbnailUrl180;
                info3.ThumbnailUrl220 = this.uploader1.ThumbnailUrl220;
                info3.ThumbnailUrl310 = this.uploader1.ThumbnailUrl310;
                info3.ThumbnailUrl410 = this.uploader1.ThumbnailUrl410;
                info3.ShortDescription = this.txtShortDescription.Text;
                info3.Description = (!string.IsNullOrEmpty(text) && (text.Length > 0)) ? text : null;
                info3.PenetrationStatus = this.chkPenetration.Checked ? PenetrationStatus.Already : PenetrationStatus.Notyet;
                info3.Title = this.txtTitle.Text;
                info3.MetaDescription = this.txtMetaDescription.Text;
                info3.MetaKeywords = this.txtMetaKeywords.Text;
                info3.AddedDate = DateTime.Now;
                info3.BrandId = this.dropBrandCategories.SelectedValue;
                ProductInfo target = info3;
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
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                if (category != null)
                {
                    target.MainCategoryPath = category.Path + "|";
                }
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
                    SKUItem item = new SKUItem();
                    item.SkuId = "0";
                    item.SKU = this.txtSku.Text;
                    item.SalePrice = num6;
                    item.CostPrice = nullable.HasValue ? nullable.Value : 0M;
                    item.PurchasePrice = num4;
                    item.Stock = num2;
                    item.AlertStock = num3;
                    item.Weight = nullable3.HasValue ? nullable3.Value : 0M;
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
                ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(target);
                if (!validateResults.IsValid)
                {
                    this.ShowMsg(validateResults);
                }
                else
                {
                    if (this.ViewState["distributorUserIds"] == null)
                    {
                        this.ViewState["distributorUserIds"] = new List<int>();
                    }
                    int type = 0;
                    if (((target.LineId > 0) && (int.Parse(this.hdlineId.Value) > 0)) && (target.LineId != int.Parse(this.hdlineId.Value)))
                    {
                        type = 6;
                    }
                    if (!this.chkPenetration.Checked)
                    {
                        type = 5;
                    }
                    if (type == 5)
                    {
                        SendMessageHelper.SendMessageToDistributors(target.ProductId.ToString(), type);
                        ProductHelper.CanclePenetrationProducts(this.productId.ToString());
                    }
                    else if (type == 6)
                    {
                        this.toline = this.dropProductLines.SelectedItem.Text;
                        SendMessageHelper.SendMessageToDistributors(this.hdlineId.Value + "|" + this.toline, type);
                    }
                    IList<int> tagIds = new List<int>();
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
                            tagIds.Add(Convert.ToInt32(str3));
                        }
                    }
                    switch (ProductHelper.UpdateProduct(target, skus, attrs, (IList<int>) this.ViewState["distributorUserIds"], tagIds))
                    {
                        case ProductActionStatus.Success:
                            this.litralProductTag.SelectedValue = tagIds;
                            this.ShowMsg("修改商品成功", true);
                            return;

                        case ProductActionStatus.AttributeError:
                            this.ShowMsg("修改商品失败，保存商品属性时出错", false);
                            return;

                        case ProductActionStatus.DuplicateName:
                            this.ShowMsg("修改商品失败，商品名称不能重复", false);
                            return;

                        case ProductActionStatus.DuplicateSKU:
                            this.ShowMsg("修改商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.SKUError:
                            this.ShowMsg("修改商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.OffShelfError:
                            this.ShowMsg("修改商品失败， 子站没在零售价范围内的商品无法下架", false);
                            return;

                        case ProductActionStatus.ProductTagEroor:
                            this.ShowMsg("修改商品失败，保存商品标签时出错", false);
                            return;
                    }
                    this.ShowMsg("修改商品失败，未知错误", false);
                }
            }
        }

        private void LoadProduct(ProductInfo product, Dictionary<int, IList<int>> attrs)
        {
            this.dropProductTypes.SelectedValue = product.TypeId;
            this.dropProductLines.SelectedValue = new int?(product.LineId);
            this.hdlineId.Value = product.LineId.ToString();
            this.dropBrandCategories.SelectedValue = product.BrandId;
            this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
            this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
            this.txtProductCode.Text = product.ProductCode;
            this.txtUnit.Text = product.Unit;
            if (product.MarketPrice.HasValue)
            {
                this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
            this.txtTitle.Text = product.Title;
            this.txtMetaDescription.Text = product.MetaDescription;
            this.txtMetaKeywords.Text = product.MetaKeywords;
            this.txtLowestSalePrice.Text = product.LowestSalePrice.ToString("F2");
            this.chkPenetration.Checked = product.PenetrationStatus == PenetrationStatus.Already;
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
            if ((attrs != null) && (attrs.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<xml><attributes>");
                foreach (int num in attrs.Keys)
                {
                    builder.Append("<item attributeId=\"").Append(num.ToString(CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int) ProductTypeHelper.GetAttribute(num).UsageMode).ToString()).Append("\" >");
                    foreach (int num2 in attrs[num])
                    {
                        builder.Append("<attValue valueId=\"").Append(num2.ToString(CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    builder.Append("</item>");
                }
                builder.Append("</attributes></xml>");
                this.txtAttributes.Text = builder.ToString();
            }
            this.chkSkuEnabled.Checked = product.HasSKU;
            if (product.HasSKU)
            {
                StringBuilder builder2 = new StringBuilder();
                builder2.Append("<xml><productSkus>");
                foreach (string str in product.Skus.Keys)
                {
                    SKUItem item = product.Skus[str];
                    string str2 = ("<item skuCode=\"" + item.SKU + "\" salePrice=\"" + item.SalePrice.ToString("F2") + "\" costPrice=\"" + ((item.CostPrice > 0M) ? item.CostPrice.ToString("F2") : "") + "\" purchasePrice=\"" + item.PurchasePrice.ToString("F2") + "\" qty=\"" + item.Stock.ToString(CultureInfo.InvariantCulture) + "\" alertQty=\"" + item.AlertStock.ToString(CultureInfo.InvariantCulture) + "\" weight=\"" + ((item.Weight > 0M) ? item.Weight.ToString("F2") : "") + "\">") + "<skuFields>";
                    foreach (int num3 in item.SkuItems.Keys)
                    {
                        string[] strArray2 = new string[] { "<sku attributeId=\"", num3.ToString(CultureInfo.InvariantCulture), "\" valueId=\"", item.SkuItems[num3].ToString(CultureInfo.InvariantCulture), "\" />" };
                        string str3 = string.Concat(strArray2);
                        str2 = str2 + str3;
                    }
                    str2 = str2 + "</skuFields>";
                    if (item.MemberPrices.Count > 0)
                    {
                        str2 = str2 + "<memberPrices>";
                        foreach (int num4 in item.MemberPrices.Keys)
                        {
                            decimal num20 = item.MemberPrices[num4];
                            str2 = str2 + string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", num4.ToString(CultureInfo.InvariantCulture), num20.ToString("F2"));
                        }
                        str2 = str2 + "</memberPrices>";
                    }
                    if (item.DistributorPrices.Count > 0)
                    {
                        str2 = str2 + "<distributorPrices>";
                        foreach (int num5 in item.DistributorPrices.Keys)
                        {
                            decimal num21 = item.DistributorPrices[num5];
                            str2 = str2 + string.Format("<distributorGrande id=\"{0}\" price=\"{1}\" />", num5.ToString(CultureInfo.InvariantCulture), num21.ToString("F2"));
                        }
                        str2 = str2 + "</distributorPrices>";
                    }
                    str2 = str2 + "</item>";
                    builder2.Append(str2);
                }
                builder2.Append("</productSkus></xml>");
                this.txtSkus.Text = builder2.ToString();
            }
            SKUItem defaultSku = product.DefaultSku;
            this.txtSku.Text = product.SKU;
            this.txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
            this.txtCostPrice.Text = (defaultSku.CostPrice > 0M) ? defaultSku.CostPrice.ToString("F2") : "";
            this.txtPurchasePrice.Text = defaultSku.PurchasePrice.ToString("F2");
            this.txtStock.Text = defaultSku.Stock.ToString(CultureInfo.InvariantCulture);
            this.txtAlertStock.Text = defaultSku.AlertStock.ToString(CultureInfo.InvariantCulture);
            this.txtWeight.Text = (defaultSku.Weight > 0M) ? defaultSku.Weight.ToString("F2") : "";
            if (defaultSku.MemberPrices.Count > 0)
            {
                this.txtMemberPrices.Text = "<xml><gradePrices>";
                foreach (int num6 in defaultSku.MemberPrices.Keys)
                {
                    decimal num28 = defaultSku.MemberPrices[num6];
                    this.txtMemberPrices.Text = this.txtMemberPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num6.ToString(CultureInfo.InvariantCulture), num28.ToString("F2"));
                }
                this.txtMemberPrices.Text = this.txtMemberPrices.Text + "</gradePrices></xml>";
            }
            if (defaultSku.DistributorPrices.Count > 0)
            {
                this.txtDistributorPrices.Text = "<xml><gradePrices>";
                foreach (int num7 in defaultSku.DistributorPrices.Keys)
                {
                    decimal num29 = defaultSku.DistributorPrices[num7];
                    this.txtDistributorPrices.Text = this.txtDistributorPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num7.ToString(CultureInfo.InvariantCulture), num29.ToString("F2"));
                }
                this.txtDistributorPrices.Text = this.txtDistributorPrices.Text + "</gradePrices></xml>";
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(base.Request.QueryString["productId"], out this.productId);
            int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
            if (!this.Page.IsPostBack)
            {
                Dictionary<int, IList<int>> dictionary;
                IList<int> distributorUserIds = null;
                IList<int> tagsId = null;
                ProductInfo product = ProductHelper.GetProductDetails(this.productId, out dictionary, out distributorUserIds, out tagsId);
                if (product == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                        this.ViewState["ProductCategoryId"] = this.categoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(product.CategoryId);
                        this.ViewState["ProductCategoryId"] = product.CategoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + product.CategoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    this.lnkEditCategory.NavigateUrl = this.lnkEditCategory.NavigateUrl + "&productId=" + product.ProductId.ToString(CultureInfo.InvariantCulture);
                    if ((distributorUserIds != null) && (distributorUserIds.Count > 0))
                    {
                        this.ViewState["distributorUserIds"] = distributorUserIds;
                        this.hlinkDistributor.NavigateUrl = "../distribution/ManageDistributor.aspx?LineId=" + product.LineId.ToString(CultureInfo.InvariantCulture);
                        this.hlinkDistributor.Text = string.Format("{0}位分销商", distributorUserIds.Count);
                    }
                    this.litralProductTag.SelectedValue = tagsId;
                    if (tagsId.Count > 0)
                    {
                        foreach (int num in tagsId)
                        {
                            this.txtProductTag.Text = this.txtProductTag.Text + num.ToString() + ",";
                        }
                        this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
                    }
                    this.dropProductTypes.DataBind();
                    this.dropProductLines.DataBind();
                    this.dropBrandCategories.DataBind();
                    this.LoadProduct(product, dictionary);
                }
            }
        }

        private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out decimal? weight)
        {
            int num4;
            decimal num6;
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            alertStock = num4 = 0;
            displaySequence = stock = num4;
            salePrice = num6 = 0M;
            purchasePrice = lowestSalePrice = num6;
            if (!this.dropProductLines.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择产品线");
            }
            if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
            {
                str = str + Formatter.FormatErrorMessage("请正确填写商品排序");
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
                    decimal num3;
                    if (decimal.TryParse(this.txtWeight.Text, out num3))
                    {
                        weight = new decimal?(num3);
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


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Gifts)]
    public class AddGift : AdminPage
    {
        protected Button btnCreate;
        protected HtmlInputCheckBox chkDownLoad;
        protected HtmlInputCheckBox chkPromotion;
        protected KindeditorControl fcDescription;
        protected TextBox txtCostPrice;
        protected TextBox txtGiftName;
        protected TextBox txtGiftTitle;
        protected TextBox txtMarketPrice;
        protected TextBox txtNeedPoint;
        protected TextBox txtPurchasePrice;
        protected TextBox txtShortDescription;
        protected TextBox txtTitleDescription;
        protected TextBox txtTitleKeywords;
        protected TextBox txtUnit;
        protected ImageUploader uploader1;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            decimal? nullable;
            decimal num;
            decimal? nullable2;
            int num2;
            if (this.ValidateValues(out nullable, out num, out nullable2, out num2))
            {
                GiftInfo info2 = new GiftInfo();
                info2.PurchasePrice = num;
                info2.CostPrice = nullable;
                info2.MarketPrice = nullable2;
                info2.NeedPoint = num2;
                info2.Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim());
                info2.Unit = this.txtUnit.Text.Trim();
                info2.ShortDescription = Globals.HtmlEncode(this.txtShortDescription.Text.Trim());
                info2.LongDescription = string.IsNullOrEmpty(this.fcDescription.Text) ? null : this.fcDescription.Text.Trim();
                info2.Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim());
                info2.Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim());
                info2.Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim());
                info2.IsDownLoad = this.chkDownLoad.Checked;
                info2.IsPromotion = this.chkPromotion.Checked;
                GiftInfo target = info2;
                target.ImageUrl = this.uploader1.UploadedImageUrl;
                target.ThumbnailUrl40 = this.uploader1.ThumbnailUrl40;
                target.ThumbnailUrl60 = this.uploader1.ThumbnailUrl60;
                target.ThumbnailUrl100 = this.uploader1.ThumbnailUrl100;
                target.ThumbnailUrl160 = this.uploader1.ThumbnailUrl160;
                target.ThumbnailUrl180 = this.uploader1.ThumbnailUrl180;
                target.ThumbnailUrl220 = this.uploader1.ThumbnailUrl220;
                target.ThumbnailUrl310 = this.uploader1.ThumbnailUrl310;
                target.ThumbnailUrl410 = this.uploader1.ThumbnailUrl410;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<GiftInfo>(target, new string[] { "ValGift" });
                string str = string.Empty;
                if (target.PurchasePrice < target.CostPrice)
                {
                    str = str + Formatter.FormatErrorMessage("礼品采购价不能小于成本价");
                }
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        str = str + Formatter.FormatErrorMessage(result.Message);
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    this.ShowMsg(str, false);
                }
                else
                {
                    switch (GiftHelper.AddGift(target))
                    {
                        case GiftActionStatus.Success:
                            this.ShowMsg("成功的添加了一件礼品", true);
                            return;

                        case GiftActionStatus.UnknowError:
                            this.ShowMsg("未知错误", false);
                            return;

                        case GiftActionStatus.DuplicateSKU:
                            this.ShowMsg("已经存在相同的商家编码", false);
                            return;

                        case GiftActionStatus.DuplicateName:
                            this.ShowMsg("已经存在相同的礼品名称", false);
                            break;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
        }

        private bool ValidateValues(out decimal? costPrice, out decimal purchasePrice, out decimal? marketPrice, out int needPoint)
        {
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            if (!string.IsNullOrEmpty(this.txtCostPrice.Text.Trim()))
            {
                decimal num;
                if (decimal.TryParse(this.txtCostPrice.Text.Trim(), out num))
                {
                    costPrice = new decimal?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("成本价金额无效，大小在10000000以内");
                }
            }
            if (!decimal.TryParse(this.txtPurchasePrice.Text.Trim(), out purchasePrice))
            {
                str = str + Formatter.FormatErrorMessage("采购价金额无效，大小在10000000以内");
            }
            if (!string.IsNullOrEmpty(this.txtMarketPrice.Text.Trim()))
            {
                decimal num2;
                if (decimal.TryParse(this.txtMarketPrice.Text.Trim(), out num2))
                {
                    marketPrice = new decimal?(num2);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("市场参考价金额无效，大小在10000000以内");
                }
            }
            if (!int.TryParse(this.txtNeedPoint.Text.Trim(), out needPoint))
            {
                str = str + Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
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


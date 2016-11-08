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
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Gifts)]
    public class EditGift : AdminPage
    {
        protected Button btnUpdate;
        protected HtmlInputCheckBox chkDownLoad;
        protected HtmlInputCheckBox chkPromotion;
        protected KindeditorControl fcDescription;
        private int giftId;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            decimal? nullable;
            decimal num;
            decimal? nullable2;
            int num2;
            GiftInfo giftDetails = GiftHelper.GetGiftDetails(this.giftId);
            new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
            if (this.ValidateValues(out nullable, out num, out nullable2, out num2))
            {
                giftDetails.PurchasePrice = num;
                giftDetails.CostPrice = nullable;
                giftDetails.MarketPrice = nullable2;
                giftDetails.NeedPoint = num2;
                giftDetails.Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim());
                giftDetails.Unit = this.txtUnit.Text.Trim();
                giftDetails.ShortDescription = Globals.HtmlEncode(this.txtShortDescription.Text.Trim());
                giftDetails.LongDescription = this.fcDescription.Text.Trim();
                giftDetails.Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim());
                giftDetails.Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim());
                giftDetails.Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim());
                giftDetails.IsDownLoad = this.chkDownLoad.Checked;
                giftDetails.IsPromotion = this.chkPromotion.Checked;
                giftDetails.ImageUrl = this.uploader1.UploadedImageUrl;
                giftDetails.ThumbnailUrl40 = this.uploader1.ThumbnailUrl40;
                giftDetails.ThumbnailUrl60 = this.uploader1.ThumbnailUrl60;
                giftDetails.ThumbnailUrl100 = this.uploader1.ThumbnailUrl100;
                giftDetails.ThumbnailUrl160 = this.uploader1.ThumbnailUrl160;
                giftDetails.ThumbnailUrl180 = this.uploader1.ThumbnailUrl180;
                giftDetails.ThumbnailUrl220 = this.uploader1.ThumbnailUrl220;
                giftDetails.ThumbnailUrl310 = this.uploader1.ThumbnailUrl310;
                giftDetails.ThumbnailUrl410 = this.uploader1.ThumbnailUrl410;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<GiftInfo>(giftDetails, new string[] { "ValGift" });
                string str = string.Empty;
                if (giftDetails.PurchasePrice < giftDetails.CostPrice)
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
                    switch (GiftHelper.UpdateGift(giftDetails))
                    {
                        case GiftActionStatus.Success:
                            this.ShowMsg("成功修改了一件礼品的基本信息", true);
                            return;

                        case GiftActionStatus.DuplicateName:
                            this.ShowMsg("已经存在相同的礼品名称", false);
                            return;

                        case GiftActionStatus.DuplicateSKU:
                            this.ShowMsg("已经存在相同的商家编码", false);
                            return;

                        case GiftActionStatus.UnknowError:
                            this.ShowMsg("未知错误", false);
                            return;
                    }
                }
            }
        }

        private void LoadGift()
        {
            GiftInfo giftDetails = GiftHelper.GetGiftDetails(this.giftId);
            if (giftDetails == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Globals.EntityCoding(giftDetails, false);
                this.txtGiftName.Text = Globals.HtmlDecode(giftDetails.Name);
                this.txtPurchasePrice.Text = string.Format("{0:F2}", giftDetails.PurchasePrice);
                this.txtNeedPoint.Text = giftDetails.NeedPoint.ToString();
                if (!string.IsNullOrEmpty(giftDetails.Unit))
                {
                    this.txtUnit.Text = giftDetails.Unit;
                }
                if (giftDetails.CostPrice.HasValue)
                {
                    this.txtCostPrice.Text = string.Format("{0:F2}", giftDetails.CostPrice);
                }
                if (giftDetails.MarketPrice.HasValue)
                {
                    this.txtMarketPrice.Text = string.Format("{0:F2}", giftDetails.MarketPrice);
                }
                if (!string.IsNullOrEmpty(giftDetails.ShortDescription))
                {
                    this.txtShortDescription.Text = Globals.HtmlDecode(giftDetails.ShortDescription);
                }
                if (!string.IsNullOrEmpty(giftDetails.LongDescription))
                {
                    this.fcDescription.Text = giftDetails.LongDescription;
                }
                if (!string.IsNullOrEmpty(giftDetails.Title))
                {
                    this.txtGiftTitle.Text = Globals.HtmlDecode(giftDetails.Title);
                }
                if (!string.IsNullOrEmpty(giftDetails.Meta_Description))
                {
                    this.txtTitleDescription.Text = Globals.HtmlDecode(giftDetails.Meta_Description);
                }
                if (!string.IsNullOrEmpty(giftDetails.Meta_Keywords))
                {
                    this.txtTitleKeywords.Text = Globals.HtmlDecode(giftDetails.Meta_Keywords);
                }
                if (!string.IsNullOrEmpty(giftDetails.ImageUrl))
                {
                    this.uploader1.UploadedImageUrl = giftDetails.ImageUrl;
                }
                this.chkDownLoad.Checked = giftDetails.IsDownLoad;
                this.chkPromotion.Checked = giftDetails.IsPromotion;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["giftId"], out this.giftId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
                if (!this.Page.IsPostBack)
                {
                    this.LoadGift();
                }
            }
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


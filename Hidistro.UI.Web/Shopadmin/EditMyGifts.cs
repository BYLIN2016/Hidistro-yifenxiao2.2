namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditMyGifts : DistributorPage
    {
        protected Button btnUpdate;
        protected HtmlInputCheckBox ckpromotion;
        protected Label fcDescription;
        private int Id;
        protected Label lblMarketPrice;
        protected Label lblPurchasePrice;
        protected Label lblShortDescription;
        protected Label lblUnit;
        protected TextBox txtGiftName;
        protected TextBox txtGiftTitle;
        protected TextBox txtNeedPoint;
        protected TextBox txtTitleDescription;
        protected TextBox txtTitleKeywords;
        protected ImageUploader uploader1;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int num;
            GiftInfo entity = new GiftInfo();
            if (!int.TryParse(this.txtNeedPoint.Text.Trim(), out num))
            {
                this.ShowMsg("兑换所需积分不能为空，大小0-10000之间", false);
            }
            else
            {
                entity.GiftId = this.Id;
                entity.NeedPoint = num;
                entity.Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim());
                entity.Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim());
                entity.Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim());
                entity.Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim());
                entity.IsPromotion = this.ckpromotion.Checked;
                Globals.EntityCoding(entity, false);
                if (SubsiteGiftHelper.UpdateMyGifts(entity))
                {
                    this.ShowMsg("成功修改了一件礼品的基本信息", true);
                }
                else
                {
                    this.ShowMsg("修改件礼品的基本信息失败", true);
                }
            }
        }

        private void LoadGift()
        {
            GiftInfo myGiftsDetails = SubsiteGiftHelper.GetMyGiftsDetails(this.Id);
            this.txtGiftName.Text = Globals.HtmlDecode(myGiftsDetails.Name);
            this.lblPurchasePrice.Text = string.Format("{0:F2}", myGiftsDetails.PurchasePrice);
            this.txtNeedPoint.Text = myGiftsDetails.NeedPoint.ToString();
            if (!string.IsNullOrEmpty(myGiftsDetails.Unit))
            {
                this.lblUnit.Text = myGiftsDetails.Unit;
            }
            if (myGiftsDetails.MarketPrice.HasValue)
            {
                this.lblMarketPrice.Text = string.Format("{0:F2}", myGiftsDetails.MarketPrice);
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.ShortDescription))
            {
                this.lblShortDescription.Text = Globals.HtmlDecode(myGiftsDetails.ShortDescription);
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.LongDescription))
            {
                this.fcDescription.Text = myGiftsDetails.LongDescription;
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.Title))
            {
                this.txtGiftTitle.Text = Globals.HtmlDecode(myGiftsDetails.Title);
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.Meta_Description))
            {
                this.txtTitleDescription.Text = Globals.HtmlDecode(myGiftsDetails.Meta_Description);
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.Meta_Keywords))
            {
                this.txtTitleKeywords.Text = Globals.HtmlDecode(myGiftsDetails.Meta_Keywords);
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.ImageUrl))
            {
                this.uploader1.UploadedImageUrl = myGiftsDetails.ImageUrl;
            }
            if (myGiftsDetails.IsPromotion)
            {
                this.ckpromotion.Checked = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["GiftId"], out this.Id))
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
    }
}


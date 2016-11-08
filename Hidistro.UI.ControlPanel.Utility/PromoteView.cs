namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using kindeditor.Net;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class PromoteView : TemplatedWebControl
    {
        private string errors;
        private KindeditorControl fckDescription;
        private bool isValid = true;
        private PromotionInfo promotion;
        private TextBox txtPromoteSalesName;

        protected override void AttachChildControls()
        {
            this.txtPromoteSalesName = (TextBox) this.FindControl("txtPromoteSalesName");
            this.fckDescription = (KindeditorControl) this.FindControl("fckDescription");
            if (!this.Page.IsPostBack && (this.promotion != null))
            {
                this.txtPromoteSalesName.Text = this.promotion.Name;
                this.fckDescription.Text = this.promotion.Description;
            }
        }

        public void Reset()
        {
            this.txtPromoteSalesName.Text = string.Empty;
            this.fckDescription.Text = string.Empty;
        }

        public string CurrentErrors
        {
            get
            {
                return this.errors;
            }
        }

        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
        }

        public PromotionInfo Item
        {
            get
            {
                this.errors = string.Empty;
                PromotionInfo info = new PromotionInfo();
                info.Name = this.txtPromoteSalesName.Text;
                info.Description = this.fckDescription.Text;
                if (string.IsNullOrEmpty(info.Description))
                {
                    this.isValid = false;
                    this.errors = this.errors + "促销活详细信息为必填项，请填写好";
                }
                return info;
            }
            set
            {
                this.promotion = value;
            }
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductConsultationsManage)]
    public class ReplyProductConsultations : AdminPage
    {
        protected Button btnReplyProductConsultation;
        private int consultationId;
        protected KindeditorControl fckReplyText;
        protected FormatedTimeLabel lblTime;
        protected Literal litConsultationText;
        protected Literal litUserName;

        protected void btnReplyProductConsultation_Click(object sender, EventArgs e)
        {
            ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
            if (string.IsNullOrEmpty(this.fckReplyText.Text))
            {
                productConsultation.ReplyText = null;
            }
            else
            {
                productConsultation.ReplyText = this.fckReplyText.Text;
            }
            productConsultation.ReplyUserId = new int?(HiContext.Current.User.UserId);
            productConsultation.ReplyDate = new DateTime?(DateTime.Now);
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductConsultationInfo>(productConsultation, new string[] { "Reply" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
            {
                this.fckReplyText.Text = string.Empty;
                this.CloseWindow();
            }
            else
            {
                this.ShowMsg("回复商品咨询失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["ConsultationId"], out this.consultationId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnReplyProductConsultation.Click += new EventHandler(this.btnReplyProductConsultation_Click);
                if (!this.Page.IsPostBack)
                {
                    ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
                    if (productConsultation == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litUserName.Text = productConsultation.UserName;
                        this.litConsultationText.Text = productConsultation.ConsultationText;
                        this.lblTime.Time = productConsultation.ConsultationDate;
                    }
                }
            }
        }
    }
}


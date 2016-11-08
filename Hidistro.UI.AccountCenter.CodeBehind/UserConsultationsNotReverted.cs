namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;

    public class UserConsultationsNotReverted : MemberTemplatedWebControl
    {
        private ThemedTemplatedList dlstPtConsultationReply;
        private Pager pagerConsultationReply;

        protected override void AttachChildControls()
        {
            this.dlstPtConsultationReply = (ThemedTemplatedList) this.FindControl("dlstPtConsultationReply");
            this.pagerConsultationReply = (Pager) this.FindControl("pagerConsultationReply");
            PageTitle.AddSiteNameTitle("咨询/未回复", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                this.BindPtConsultationReply();
            }
        }

        private void BindPtConsultationReply()
        {
            ProductConsultationAndReplyQuery query = new ProductConsultationAndReplyQuery();
            query.PageIndex = this.pagerConsultationReply.PageIndex;
            query.UserId = HiContext.Current.User.UserId;
            query.Type = ConsultationReplyType.NoReply;
            int total = 0;
            DataSet productConsultationsAndReplys = CommentsHelper.GetProductConsultationsAndReplys(query, out total);
            this.dlstPtConsultationReply.DataSource = productConsultationsAndReplys.Tables[0].DefaultView;
            this.dlstPtConsultationReply.DataBind();
            this.pagerConsultationReply.TotalRecords = total;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserConsultationsNotReverted.html";
            }
            base.OnInit(e);
        }
    }
}


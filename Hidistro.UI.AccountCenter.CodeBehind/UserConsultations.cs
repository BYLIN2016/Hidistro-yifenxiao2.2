namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Comments;
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;

    public class UserConsultations : MemberTemplatedWebControl
    {
        private ThemedTemplatedList dlstPtConsultationReplyed;
        private Pager pagerConsultationReplyed;

        protected override void AttachChildControls()
        {
            this.dlstPtConsultationReplyed = (ThemedTemplatedList) this.FindControl("dlstPtConsultationReplyed");
            this.pagerConsultationReplyed = (Pager) this.FindControl("pagerConsultationReplyed");
            PageTitle.AddSiteNameTitle("咨询/已回复", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                PersonalHelper.ViewProductConsultations();
                this.BindPtConsultationReplyed();
            }
        }

        private void BindPtConsultationReplyed()
        {
            ProductConsultationAndReplyQuery query = new ProductConsultationAndReplyQuery();
            query.PageIndex = this.pagerConsultationReplyed.PageIndex;
            query.UserId = HiContext.Current.User.UserId;
            query.Type = ConsultationReplyType.Replyed;
            int total = 0;
            DataSet productConsultationsAndReplys = CommentsHelper.GetProductConsultationsAndReplys(query, out total);
            this.dlstPtConsultationReplyed.DataSource = productConsultationsAndReplys.Tables[0].DefaultView;
            this.dlstPtConsultationReplyed.DataBind();
            this.pagerConsultationReplyed.TotalRecords = total;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserConsultations.html";
            }
            base.OnInit(e);
        }
    }
}


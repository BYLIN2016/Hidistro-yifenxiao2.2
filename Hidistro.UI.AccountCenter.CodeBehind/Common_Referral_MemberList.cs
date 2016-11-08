namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_Referral_MemberList : AscxTemplatedWebControl
    {
        private DataList dataListPointDetails;
        public const string TagID = "Common_Referral_MemberList";

        public Common_Referral_MemberList()
        {
            base.ID = "Common_Referral_MemberList";
        }

        protected override void AttachChildControls()
        {
            this.dataListPointDetails = (DataList) this.FindControl("dataListPointDetails");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListPointDetails.DataSource != null)
            {
                this.dataListPointDetails.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Referral_MemberList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListPointDetails.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListPointDetails.DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
    }
}


namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_Advance_AccountList : AscxTemplatedWebControl
    {
        private DataList dataListAccountDetails;
        public const string TagID = "Common_Advance_AccountList";

        public Common_Advance_AccountList()
        {
            base.ID = "Common_Advance_AccountList";
        }

        protected override void AttachChildControls()
        {
            this.dataListAccountDetails = (DataList) this.FindControl("dataListAccountDetails");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListAccountDetails.DataSource != null)
            {
                this.dataListAccountDetails.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Advance_AccountList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListAccountDetails.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListAccountDetails.DataSource = value;
            }
        }
    }
}


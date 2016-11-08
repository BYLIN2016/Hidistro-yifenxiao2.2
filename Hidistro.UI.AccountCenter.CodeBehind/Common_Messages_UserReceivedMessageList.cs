namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;

    public class Common_Messages_UserReceivedMessageList : AscxTemplatedWebControl
    {
        private Grid gridMessageList;
        public const string TagID = "Grid_Common_Messages_UserReceivedMessageList";

        public Common_Messages_UserReceivedMessageList()
        {
            base.ID = "Grid_Common_Messages_UserReceivedMessageList";
        }

        protected override void AttachChildControls()
        {
            this.gridMessageList = (Grid) this.FindControl("gridMessageList");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.gridMessageList.DataSource != null)
            {
                this.gridMessageList.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Messages_UserReceivedMessageList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.gridMessageList.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.gridMessageList.DataSource = value;
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


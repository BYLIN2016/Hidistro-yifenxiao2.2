namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_OrderManage_ReplaceApply : AscxTemplatedWebControl
    {
        private Grid listReplace;
        public const string TagID = "Common_OrderManage_ReplaceApply";

        public Common_OrderManage_ReplaceApply()
        {
            base.ID = "Common_OrderManage_ReplaceApply";
        }

        protected override void AttachChildControls()
        {
            this.listReplace = (Grid) this.FindControl("listReplace");
            this.listReplace.RowDataBound += new GridViewRowEventHandler(this.listReplace_RowDataBound);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.listReplace.DataSource = this.DataSource;
            this.listReplace.DataBind();
        }

        private void listReplace_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label label = (Label) e.Row.FindControl("lblHandleStatus");
                if (label.Text == "0")
                {
                    label.Text = "待处理";
                }
                else if (label.Text == "1")
                {
                    label.Text = "已处理";
                }
                else
                {
                    label.Text = "已拒绝";
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_ReplaceApply.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyArray DataKeys
        {
            get
            {
                return this.listReplace.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.listReplace.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.listReplace.DataSource = value;
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

        public SortAction SortOrder
        {
            get
            {
                return SortAction.Desc;
            }
        }

        public string SortOrderBy
        {
            get
            {
                if (this.listReplace != null)
                {
                    return this.listReplace.SortOrderBy;
                }
                return string.Empty;
            }
        }
    }
}


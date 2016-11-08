namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_OrderManage_RefundApply : AscxTemplatedWebControl
    {
        private Grid listRefunds;
        public const string TagID = "Common_OrderManage_RefundApply";

        public Common_OrderManage_RefundApply()
        {
            base.ID = "Common_OrderManage_RefundApply";
        }

        protected override void AttachChildControls()
        {
            this.listRefunds = (Grid) this.FindControl("listRefunds");
            this.listRefunds.RowDataBound += new GridViewRowEventHandler(this.listRefunds_RowDataBound);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.listRefunds.DataSource = this.DataSource;
            this.listRefunds.DataBind();
        }

        private void listRefunds_RowDataBound(object sender, GridViewRowEventArgs e)
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
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_RefundApply.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyArray DataKeys
        {
            get
            {
                return this.listRefunds.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.listRefunds.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.listRefunds.DataSource = value;
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
                if (this.listRefunds != null)
                {
                    return this.listRefunds.SortOrderBy;
                }
                return string.Empty;
            }
        }
    }
}


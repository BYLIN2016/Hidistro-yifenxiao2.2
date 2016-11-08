namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_OrderManage_ReturnsApply : AscxTemplatedWebControl
    {
        private Grid listReturns;
        public const string TagID = "Common_OrderManage_ReturnsApply";

        public Common_OrderManage_ReturnsApply()
        {
            base.ID = "Common_OrderManage_ReturnsApply";
        }

        protected override void AttachChildControls()
        {
            this.listReturns = (Grid) this.FindControl("listReturns");
            this.listReturns.RowDataBound += new GridViewRowEventHandler(this.listReturns_RowDataBound);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.listReturns.DataSource = this.DataSource;
            this.listReturns.DataBind();
        }

        private void listReturns_RowDataBound(object sender, GridViewRowEventArgs e)
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
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_ReturnsApply.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyArray DataKeys
        {
            get
            {
                return this.listReturns.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.listReturns.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.listReturns.DataSource = value;
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
                if (this.listReturns != null)
                {
                    return this.listReturns.SortOrderBy;
                }
                return string.Empty;
            }
        }
    }
}


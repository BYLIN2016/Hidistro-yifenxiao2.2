namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Shippers)]
    public class Shippers : AdminPage
    {
        protected Grid grdShippers;

        private void BindShippers()
        {
            this.grdShippers.DataSource = SalesHelper.GetShippers(false);
            this.grdShippers.DataBind();
        }

        private void grdShippers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
                int shipperId = (int) this.grdShippers.DataKeys[namingContainer.RowIndex].Value;
                if (!SalesHelper.GetShipper(shipperId).IsDefault)
                {
                    SalesHelper.SetDefalutShipper(shipperId);
                    this.BindShippers();
                }
            }
        }

        private void grdShippers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int shipperId = (int) this.grdShippers.DataKeys[e.RowIndex].Value;
            if (SalesHelper.GetShipper(shipperId).IsDefault)
            {
                this.ShowMsg("不能删除默认的发货信息", false);
            }
            else if (SalesHelper.DeleteShipper(shipperId))
            {
                this.BindShippers();
                this.ShowMsg("已经成功删除选择的发货信息", true);
            }
            else
            {
                this.ShowMsg("不能删除默认的发货信息", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdShippers.RowDeleting += new GridViewDeleteEventHandler(this.grdShippers_RowDeleting);
            this.grdShippers.RowCommand += new GridViewCommandEventHandler(this.grdShippers_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindShippers();
            }
        }
    }
}


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

    [PrivilegeCheck(Privilege.PaymentModes)]
    public class PaymentTypes : AdminPage
    {
        protected GridView grdPaymentMode;
        protected Pager pager1;

        private void BindData()
        {
            this.grdPaymentMode.DataSource = SalesHelper.GetPaymentModes();
            this.grdPaymentMode.DataBind();
        }

        private void grdPaymentMode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int modeId = (int) this.grdPaymentMode.DataKeys[rowIndex].Value;
                int displaySequence = Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceModeId = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if (rowIndex < (this.grdPaymentMode.Rows.Count - 1))
                    {
                        replaceModeId = (int) this.grdPaymentMode.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex > 0))
                {
                    replaceModeId = (int) this.grdPaymentMode.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if ((replaceModeId > 0) && (replaceDisplaySequence > 0))
                {
                    SalesHelper.SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
                    this.BindData();
                }
            }
        }

        private void grdPaymentMode_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SalesHelper.DeletePaymentMode((int) this.grdPaymentMode.DataKeys[e.RowIndex].Value))
            {
                this.BindData();
                this.ShowMsg("成功删除了一个支付方式", true);
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdPaymentMode.RowDeleting += new GridViewDeleteEventHandler(this.grdPaymentMode_RowDeleting);
            this.grdPaymentMode.RowCommand += new GridViewCommandEventHandler(this.grdPaymentMode_RowCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}


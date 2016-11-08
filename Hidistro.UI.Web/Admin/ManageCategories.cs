namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ManageCategories : AdminPage
    {
        protected LinkButton btnOrder;
        protected Grid grdTopCategries;

        private void BindData()
        {
            this.grdTopCategries.DataSource = CatalogHelper.GetSequenceCategories();
            this.grdTopCategries.DataBind();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdTopCategries.Rows)
            {
                int result = 0;
                TextBox box = (TextBox) row.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    int categoryId = (int) this.grdTopCategries.DataKeys[row.RowIndex].Value;
                    if (CatalogHelper.GetCategory(categoryId).DisplaySequence != result)
                    {
                        CatalogHelper.SwapCategorySequence(categoryId, result);
                    }
                }
            }
            HiCache.Remove("DataCache-Categories");
            this.BindData();
        }

        private void grdTopCategries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int) this.grdTopCategries.DataKeys[rowIndex].Value;
            if (e.CommandName == "DeleteCagetory")
            {
                if (CatalogHelper.DeleteCategory(categoryId))
                {
                    this.ShowMsg("成功删除了指定的分类", true);
                }
                else
                {
                    this.ShowMsg("分类删除失败，未知错误", false);
                }
            }
            this.BindData();
        }

        private void grdTopCategries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int num = (int) DataBinder.Eval(e.Row.DataItem, "Depth");
                string str = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                if (num == 1)
                {
                    str = "<b>" + str + "</b>";
                }
                else
                {
                    HtmlGenericControl control = e.Row.FindControl("spShowImage") as HtmlGenericControl;
                    control.Visible = false;
                }
                for (int i = 1; i < num; i++)
                {
                    str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + str;
                }
                Literal literal = e.Row.FindControl("lblCategoryName") as Literal;
                literal.Text = str;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdTopCategries.RowCommand += new GridViewCommandEventHandler(this.grdTopCategries_RowCommand);
            this.grdTopCategries.RowDataBound += new GridViewRowEventHandler(this.grdTopCategries_RowDataBound);
            this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}


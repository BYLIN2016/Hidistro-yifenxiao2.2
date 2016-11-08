namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageMyCategories : DistributorPage
    {
        protected LinkButton btnDownload;
        protected LinkButton btnOrder;
        protected Grid grdTopCategries;

        private void BindData()
        {
            this.grdTopCategries.DataSource = SubsiteCatalogHelper.GetSequenceCategories();
            this.grdTopCategries.DataBind();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.grdTopCategries.Rows.Count > 0)
            {
                this.ShowMsg("子站已有商品分类，请先删除所有商品分类再下载！", false);
            }
            else if (SubsiteCatalogHelper.DownloadCategory() > 0)
            {
                this.grdTopCategries.SelectedIndex = -1;
                this.BindData();
                this.ShowMsg("成功下载了主站分类", true);
            }
            else
            {
                this.ShowMsg("商品没有铺货，主站商品分类下载失败", false);
            }
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
                    if (SubsiteCatalogHelper.GetCategory(categoryId).DisplaySequence != result)
                    {
                        SubsiteCatalogHelper.SwapCategorySequence(categoryId, result);
                    }
                }
            }
            HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            this.BindData();
        }

        private void grdTopCategries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int) this.grdTopCategries.DataKeys[rowIndex].Value;
            if (e.CommandName == "DeleteCategory")
            {
                if (SubsiteCatalogHelper.DeleteCategory(categoryId))
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
                    str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + str;
                }
                Literal literal = (Literal) e.Row.FindControl("lblCategoryName");
                literal.Text = str;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdTopCategries.RowCommand += new GridViewCommandEventHandler(this.grdTopCategries_RowCommand);
            this.grdTopCategries.RowDataBound += new GridViewRowEventHandler(this.grdTopCategries_RowDataBound);
            this.btnDownload.Click += new EventHandler(this.btnDownload_Click);
            this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}


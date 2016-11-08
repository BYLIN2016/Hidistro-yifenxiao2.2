namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ArticleCategories)]
    public class ArticleCategories : AdminPage
    {
        protected LinkButton btnorder;
        protected Grid grdArticleCategories;

        private void BindArticleCategory()
        {
            this.grdArticleCategories.DataSource = ArticleHelper.GetMainArticleCategories();
            this.grdArticleCategories.DataBind();
        }

        protected void btnorder_Click(object sender, EventArgs e)
        {
            int num = 0;
            for (int i = 0; i < this.grdArticleCategories.Rows.Count; i++)
            {
                int categoryId = (int) this.grdArticleCategories.DataKeys[i].Value;
                int replaceDisplaySequence = int.Parse((this.grdArticleCategories.Rows[i].Cells[3].Controls[1] as HtmlInputText).Value);
                ArticleHelper.SwapArticleCategorySequence(categoryId, 0, 0, replaceDisplaySequence);
                num++;
            }
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void grdArticleCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int) this.grdArticleCategories.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((this.grdArticleCategories.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
            int replaceCategoryId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (this.grdArticleCategories.Rows.Count - 1))
                {
                    replaceCategoryId = (int) this.grdArticleCategories.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((this.grdArticleCategories.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceCategoryId = (int) this.grdArticleCategories.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((this.grdArticleCategories.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
            }
            if (replaceCategoryId > 0)
            {
                ArticleHelper.SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
            }
            if (e.CommandName == "Delete")
            {
                ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(categoryId);
                if (ArticleHelper.DeleteArticleCategory(categoryId))
                {
                    ResourcesHelper.DeleteImage(articleCategory.IconUrl);
                }
            }
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdArticleCategories.RowCommand += new GridViewCommandEventHandler(this.grdArticleCategories_RowCommand);
            this.btnorder.Click += new EventHandler(this.btnorder_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindArticleCategory();
            }
        }
    }
}


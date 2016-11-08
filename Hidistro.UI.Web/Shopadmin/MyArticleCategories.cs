namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MyArticleCategories : DistributorPage
    {
        protected Grid grdArticleCategories;

        private void BindArticleCategory()
        {
            this.grdArticleCategories.DataSource = SubsiteCommentsHelper.GetMainArticleCategories();
            this.grdArticleCategories.DataBind();
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
                SubsiteCommentsHelper.SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
                this.BindArticleCategory();
            }
        }

        private void grdArticleCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int categoryId = (int) this.grdArticleCategories.DataKeys[e.RowIndex].Value;
            ArticleCategoryInfo articleCategory = SubsiteCommentsHelper.GetArticleCategory(categoryId);
            if (SubsiteCommentsHelper.DeleteArticleCategory(categoryId))
            {
                ResourcesHelper.DeleteImage(articleCategory.IconUrl);
                this.ShowMsg("成功删除了指定的文章分类", true);
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
            this.BindArticleCategory();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdArticleCategories.RowDeleting += new GridViewDeleteEventHandler(this.grdArticleCategories_RowDeleting);
            this.grdArticleCategories.RowCommand += new GridViewCommandEventHandler(this.grdArticleCategories_RowCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindArticleCategory();
            }
        }
    }
}


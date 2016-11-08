namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.HelpCategories)]
    public class HelpCategories : AdminPage
    {
        protected LinkButton btnorder;
        protected Grid grdHelpCategories;

        private void BindHelpCategory()
        {
            IList<HelpCategoryInfo> helpCategorys = ArticleHelper.GetHelpCategorys();
            this.grdHelpCategories.DataSource = helpCategorys;
            this.grdHelpCategories.DataBind();
        }

        protected void btnorder_Click(object sender, EventArgs e)
        {
            int num = 0;
            for (int i = 0; i < this.grdHelpCategories.Rows.Count; i++)
            {
                int categoryId = (int) this.grdHelpCategories.DataKeys[i].Value;
                int replaceDisplaySequence = int.Parse((this.grdHelpCategories.Rows[i].Cells[2].Controls[1] as HtmlInputText).Value);
                ArticleHelper.SwapHelpCategorySequence(categoryId, 0, 0, replaceDisplaySequence);
                num++;
            }
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void grdHelpCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int) this.grdHelpCategories.DataKeys[rowIndex].Value;
            if (e.CommandName == "Delete")
            {
                ArticleHelper.DeleteHelpCategory(categoryId);
            }
            else if (e.CommandName == "SetYesOrNo")
            {
                HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(categoryId);
                if (helpCategory.IsShowFooter)
                {
                    helpCategory.IsShowFooter = false;
                }
                else
                {
                    helpCategory.IsShowFooter = true;
                }
                ArticleHelper.UpdateHelpCategory(helpCategory);
            }
            else
            {
                int displaySequence = int.Parse((this.grdHelpCategories.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceCategoryId = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if (rowIndex < (this.grdHelpCategories.Rows.Count - 1))
                    {
                        replaceCategoryId = (int) this.grdHelpCategories.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = int.Parse((this.grdHelpCategories.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex > 0))
                {
                    replaceCategoryId = (int) this.grdHelpCategories.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = int.Parse((this.grdHelpCategories.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if (replaceCategoryId > 0)
                {
                    ArticleHelper.SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
                }
            }
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdHelpCategories.RowCommand += new GridViewCommandEventHandler(this.grdHelpCategories_RowCommand);
            this.btnorder.Click += new EventHandler(this.btnorder_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindHelpCategory();
            }
        }
    }
}


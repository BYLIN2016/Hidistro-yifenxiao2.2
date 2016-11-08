namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MyHelpCategories : DistributorPage
    {
        protected Grid grdHelpCategories;

        private void BindHelpCategory()
        {
            IList<HelpCategoryInfo> helpCategorys = SubsiteCommentsHelper.GetHelpCategorys();
            this.grdHelpCategories.DataSource = helpCategorys;
            this.grdHelpCategories.DataBind();
        }

        private void grdHelpCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int) this.grdHelpCategories.DataKeys[rowIndex].Value;
            if (e.CommandName == "SetYesOrNo")
            {
                HelpCategoryInfo helpCategory = SubsiteCommentsHelper.GetHelpCategory(categoryId);
                if (helpCategory.IsShowFooter)
                {
                    helpCategory.IsShowFooter = false;
                }
                else
                {
                    helpCategory.IsShowFooter = true;
                }
                SubsiteCommentsHelper.UpdateHelpCategory(helpCategory);
                this.BindHelpCategory();
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
                    SubsiteCommentsHelper.SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
                    this.BindHelpCategory();
                }
            }
        }

        private void grdHelpCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsiteCommentsHelper.DeleteHelpCategory((int) this.grdHelpCategories.DataKeys[e.RowIndex].Value))
            {
                this.BindHelpCategory();
                this.ShowMsg("成功删除了选择的帮助分类", true);
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdHelpCategories.RowCommand += new GridViewCommandEventHandler(this.grdHelpCategories_RowCommand);
            this.grdHelpCategories.RowDeleting += new GridViewDeleteEventHandler(this.grdHelpCategories_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindHelpCategory();
            }
        }
    }
}


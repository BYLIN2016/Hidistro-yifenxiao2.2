namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.BrandCategories)]
    public class BrandCategories : AdminPage
    {
        protected LinkButton btnorder;
        protected Button btnSearchButton;
        protected Grid grdBrandCategriesList;
        protected TextBox txtSearchText;

        private void BindBrandCategories()
        {
            this.grdBrandCategriesList.DataSource = CatalogHelper.GetBrandCategories();
            this.grdBrandCategriesList.DataBind();
        }

        protected void btnorder_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = true;
                for (int i = 0; i < this.grdBrandCategriesList.Rows.Count; i++)
                {
                    int barndId = (int) this.grdBrandCategriesList.DataKeys[i].Value;
                    int displaysequence = int.Parse((this.grdBrandCategriesList.Rows[i].Cells[4].Controls[1] as HtmlInputText).Value);
                    if (!CatalogHelper.UpdateBrandCategoryDisplaySequence(barndId, displaysequence))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    this.ShowMsg("批量更新排序成功！", true);
                    this.BindBrandCategories();
                }
                else
                {
                    this.ShowMsg("批量更新排序失败！", false);
                }
            }
            catch (Exception exception)
            {
                this.ShowMsg("批量更新排序失败！" + exception.Message, false);
            }
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            string brandName = this.txtSearchText.Text.Trim();
            this.grdBrandCategriesList.DataSource = CatalogHelper.GetBrandCategories(brandName);
            this.grdBrandCategriesList.DataBind();
        }

        protected void grdBrandCategriesList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int brandId = (int) this.grdBrandCategriesList.DataKeys[rowIndex].Value;
            if (e.CommandName == "Rise")
            {
                if (rowIndex != this.grdBrandCategriesList.Rows.Count)
                {
                    CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Asc);
                    this.BindBrandCategories();
                }
            }
            else if (e.CommandName == "Fall")
            {
                CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Desc);
                this.BindBrandCategories();
            }
        }

        protected void grdBrandCategriesList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int brandId = (int) this.grdBrandCategriesList.DataKeys[e.RowIndex].Value;
            if (CatalogHelper.BrandHvaeProducts(brandId))
            {
                this.ShowMsg("选择的品牌分类下还有商品，删除失败", false);
            }
            else
            {
                if (CatalogHelper.DeleteBrandCategory(brandId))
                {
                    this.ShowMsg("成功删除品牌分类", true);
                }
                else
                {
                    this.ShowMsg("删除品牌分类失败", false);
                }
                this.BindBrandCategories();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdBrandCategriesList.RowDeleting += new GridViewDeleteEventHandler(this.grdBrandCategriesList_RowDeleting);
            this.grdBrandCategriesList.RowCommand += new GridViewCommandEventHandler(this.grdBrandCategriesList_RowCommand);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnorder.Click += new EventHandler(this.btnorder_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindBrandCategories();
            }
        }
    }
}


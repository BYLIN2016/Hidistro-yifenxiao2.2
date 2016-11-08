namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class DisplaceCategory : AdminPage
    {
        protected Button btnSaveCategory;
        protected ProductCategoriesDropDownList dropCategoryFrom;
        protected ProductCategoriesDropDownList dropCategoryTo;

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (!this.dropCategoryFrom.SelectedValue.HasValue || !this.dropCategoryTo.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择需要替换的商品分类或需要替换至的商品分类", false);
            }
            else if (this.dropCategoryFrom.SelectedValue.Value == this.dropCategoryTo.SelectedValue.Value)
            {
                this.ShowMsg("请选择不同的商品分类进行替换", false);
            }
            else if (CatalogHelper.DisplaceCategory(this.dropCategoryFrom.SelectedValue.Value, this.dropCategoryTo.SelectedValue.Value) == 0)
            {
                this.ShowMsg("此分类下没有可以替换的商品", false);
            }
            else
            {
                this.CloseWindow();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveCategory.Click += new EventHandler(this.btnSaveCategory_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategoryFrom.DataBind();
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
                {
                    int result = 0;
                    if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                    {
                        this.dropCategoryFrom.SelectedValue = new int?(result);
                    }
                }
            }
        }
    }
}


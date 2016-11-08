namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_ProductCategoryList : ThemedTemplatedRepeater
    {
        
        private bool _IsShowSubCategory;
        private int categoryId;
        private int maxNum = 0x3e8;

        private void BindList()
        {
            if (this.categoryId != 0)
            {
                base.DataSource = CategoryBrowser.GetMaxSubCategories(this.categoryId, this.MaxNum);
                base.DataBind();
            }
            else
            {
                base.DataSource = CategoryBrowser.GetMaxMainCategories(this.MaxNum);
                base.DataBind();
            }
        }

        private void Common_ProductCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int categoryId = ((CategoryInfo) e.Item.DataItem).CategoryId;
            Repeater repeater = (Repeater) e.Item.Controls[0].FindControl("rptSubCategries");
            if (repeater != null)
            {
                repeater.DataSource = CategoryBrowser.GetMaxSubCategories(categoryId, 0x3e8);
                repeater.DataBind();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.ItemDataBound += new RepeaterItemEventHandler(this.Common_ProductCategoryList_ItemDataBound);
            if (this.IsShowSubCategory)
            {
                int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            }
            this.BindList();
        }

        public bool IsShowSubCategory
        {
            
            get
            {
                return _IsShowSubCategory;
            }
            
            set
            {
                _IsShowSubCategory = value;
            }
        }

        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
    }
}


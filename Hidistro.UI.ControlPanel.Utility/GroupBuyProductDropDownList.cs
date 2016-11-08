namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class GroupBuyProductDropDownList : DropDownList
    {
        private int? categoryId;
        private string productCode;
        private string productName;

        public override void DataBind()
        {
            this.Items.Clear();
            ProductQuery query = new ProductQuery();
            query.Keywords = this.productName;
            query.ProductCode = this.productCode;
            query.CategoryId = this.categoryId;
            query.SaleStatus = ProductSaleStatus.OnSale;
            if (this.categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            DataTable groupBuyProducts = ProductHelper.GetGroupBuyProducts(query);
            base.Items.Add(new ListItem("--请选择--", string.Empty));
            foreach (DataRow row in groupBuyProducts.Rows)
            {
                base.Items.Add(new ListItem(row["ProductName"].ToString(), row["ProductId"].ToString()));
            }
        }

        public int? CategoryId
        {
            get
            {
                return this.categoryId;
            }
            set
            {
                this.categoryId = value;
            }
        }

        public string ProductCode
        {
            get
            {
                return this.productCode;
            }
            set
            {
                this.productCode = value;
            }
        }

        public string ProductName
        {
            get
            {
                return this.productName;
            }
            set
            {
                this.productName = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}


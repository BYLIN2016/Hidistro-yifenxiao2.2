namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class Common_CategoriesDropDownList : DropDownList
    {
        private bool m_AutoDataBind;
        private string m_NullToDisplay = "";
        private string strDepth = "　　";

        public override void DataBind()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            DataTable categories = CategoryBrowser.GetCategories();
            for (int i = 0; i < categories.Rows.Count; i++)
            {
                int num2 = (int) categories.Rows[i]["CategoryId"];
                this.Items.Add(new ListItem(this.FormatDepth((int) categories.Rows[i]["Depth"], Globals.HtmlDecode((string) categories.Rows[i]["Name"])), num2.ToString(CultureInfo.InvariantCulture)));
            }
        }

        private string FormatDepth(int depth, string categoryName)
        {
            for (int i = 1; i < depth; i++)
            {
                categoryName = this.strDepth + categoryName;
            }
            return categoryName;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.AutoDataBind && !this.Page.IsPostBack)
            {
                this.DataBind();
            }
        }

        public bool AutoDataBind
        {
            get
            {
                return this.m_AutoDataBind;
            }
            set
            {
                this.m_AutoDataBind = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.m_NullToDisplay;
            }
            set
            {
                this.m_NullToDisplay = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(base.SelectedValue))
                {
                    return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
                }
                return null;
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


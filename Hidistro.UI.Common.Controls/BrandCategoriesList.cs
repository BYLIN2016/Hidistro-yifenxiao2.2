namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class BrandCategoriesList : ListBox
    {
        public override void DataBind()
        {
            this.Items.Clear();
            base.Items.Add(new ListItem("--任意--", "0"));
            DataTable table = new DataTable();
            foreach (DataRow row in ControlProvider.Instance().GetBrandCategories().Rows)
            {
                int num = (int) row["BrandId"];
                this.Items.Add(new ListItem((string) row["BrandName"], num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}


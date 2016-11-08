namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SkuContentLabel : Literal
    {
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable skuContentBySku = ControlProvider.Instance().GetSkuContentBySku(base.Text);
            string str = string.Empty;
            foreach (DataRow row in skuContentBySku.Rows)
            {
                if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, row["AttributeName"], ":", row["ValueStr"], "; " });
                }
            }
            base.Text = str;
            base.Render(writer);
        }
    }
}


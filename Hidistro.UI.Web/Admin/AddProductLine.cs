namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddProductLine)]
    public class AddProductLine : AdminPage
    {
        protected Button btnCreate;
        protected DropDownList dropSuppliers;
        protected TextBox txtProductLineName;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            ProductLineInfo info2 = new ProductLineInfo();
            info2.Name = this.txtProductLineName.Text.Trim();
            info2.SupplierName = (this.dropSuppliers.SelectedValue.Length > 0) ? this.dropSuppliers.SelectedValue : null;
            ProductLineInfo target = info2;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductLineInfo>(target, new string[] { "ValProductLine" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else if (ProductLineHelper.AddProductLine(target))
            {
                this.Reset();
                this.ShowMsg("成功的添加了一条产品线", true);
            }
            else
            {
                this.ShowMsg("添加产品线失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropSuppliers.Items.Add(new ListItem("-请选择-", ""));
                foreach (string str in ProductLineHelper.GetSuppliers())
                {
                    this.dropSuppliers.Items.Add(new ListItem(str, str));
                }
            }
        }

        private void Reset()
        {
            this.txtProductLineName.Text = string.Empty;
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProductLine)]
    public class EditProductLine : AdminPage
    {
        protected Button btnSave;
        protected DropDownList dropSuppliers;
        private int lineId;
        protected TextBox txtProductLineName;

        private void btnSave_Click(object sender, EventArgs e)
        {
            ProductLineInfo info2 = new ProductLineInfo();
            info2.LineId = this.lineId;
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
            else if (ProductLineHelper.UpdateProductLine(target))
            {
                this.ShowMsg("已经成功修改当前产品线信息", true);
            }
            else
            {
                this.ShowMsg("修改产品线失败", false);
            }
        }

        private void LoadControl()
        {
            ProductLineInfo productLine = ProductLineHelper.GetProductLine(this.lineId);
            if (productLine == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Globals.EntityCoding(productLine, false);
                this.txtProductLineName.Text = productLine.Name;
                this.dropSuppliers.SelectedValue = productLine.SupplierName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["LineId"], out this.lineId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSave.Click += new EventHandler(this.btnSave_Click);
                if (!base.IsPostBack)
                {
                    this.dropSuppliers.Items.Add(new ListItem("-请选择-", ""));
                    foreach (string str in ProductLineHelper.GetSuppliers())
                    {
                        this.dropSuppliers.Items.Add(new ListItem(str, str));
                    }
                    this.LoadControl();
                }
            }
        }
    }
}


namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using kindeditor.Net;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ShippingModes)]
    public class AddShippingType : AdminPage
    {
        protected Button btnCreate;
        protected ExpressCheckBoxList expressCheckBoxList;
        protected KindeditorControl fck;
        protected Script Script1;
        protected ShippingTemplatesDropDownList shippingTemplatesDropDownList;
        protected TextBox txtModeName;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            ShippingModeInfo shippingMode = new ShippingModeInfo();
            shippingMode.Name = Globals.HtmlEncode(this.txtModeName.Text.Trim());
            shippingMode.Description = this.fck.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            if (this.shippingTemplatesDropDownList.SelectedValue.HasValue)
            {
                shippingMode.TemplateId = this.shippingTemplatesDropDownList.SelectedValue.Value;
            }
            else
            {
                this.ShowMsg("请选择运费模板", false);
                return;
            }
            foreach (ListItem item in this.expressCheckBoxList.Items)
            {
                if (item.Selected)
                {
                    shippingMode.ExpressCompany.Add(item.Value);
                }
            }
            if (shippingMode.ExpressCompany.Count == 0)
            {
                this.ShowMsg("至少要选择一个配送公司", false);
            }
            else if (SalesHelper.CreateShippingMode(shippingMode))
            {
                this.ClearControlValue();
                this.ShowMsg("成功添加了一个配送方式", true);
            }
            else
            {
                this.ShowMsg("添加失败，请确定填写了所有必填项", false);
            }
        }

        private void ClearControlValue()
        {
            this.txtModeName.Text = string.Empty;
            this.fck.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                this.shippingTemplatesDropDownList.DataBind();
                this.expressCheckBoxList.BindExpressCheckBoxList();
            }
        }
    }
}


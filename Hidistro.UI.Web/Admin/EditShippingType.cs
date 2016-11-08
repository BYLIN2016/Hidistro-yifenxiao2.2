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
    public class EditShippingType : AdminPage
    {
        protected Button btnUpDate;
        protected ExpressCheckBoxList expressCheckBoxList;
        protected KindeditorControl fck;
        private int modeId;
        protected ShippingTemplatesDropDownList shippingTemplatesDropDownList;
        protected Hidistro.UI.Common.Controls.Style Style1;
        protected TextBox txtModeName;

        private void BindControl(ShippingModeInfo modeItem)
        {
            this.txtModeName.Text = Globals.HtmlDecode(modeItem.Name);
            this.fck.Text = modeItem.Description;
            if (modeItem.TemplateId > 0)
            {
                this.shippingTemplatesDropDownList.SelectedValue = new int?(modeItem.TemplateId);
            }
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            ShippingModeInfo shippingMode = new ShippingModeInfo();
            shippingMode.Name = Globals.HtmlEncode(this.txtModeName.Text.Trim());
            shippingMode.ModeId = this.modeId;
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
            else if (SalesHelper.UpdateShippMode(shippingMode))
            {
                this.Page.Response.Redirect("EditShippingType.aspx?modeId=" + shippingMode.ModeId + "&isUpdate=true");
            }
            else
            {
                this.ShowMsg("更新失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["modeId"], out this.modeId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpDate.Click += new EventHandler(this.btnUpDate_Click);
                if (!this.Page.IsPostBack)
                {
                    if ((this.Page.Request.QueryString["isUpdate"] != null) && (this.Page.Request.QueryString["isUpdate"] == "true"))
                    {
                        this.ShowMsg("成功修改了一个配送方式", true);
                    }
                    ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.modeId, true);
                    if (shippingMode == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.BindControl(shippingMode);
                        this.shippingTemplatesDropDownList.DataBind();
                        this.BindControl(shippingMode);
                        this.expressCheckBoxList.ExpressCompany = shippingMode.ExpressCompany;
                        this.expressCheckBoxList.DataBind();
                    }
                }
            }
        }
    }
}


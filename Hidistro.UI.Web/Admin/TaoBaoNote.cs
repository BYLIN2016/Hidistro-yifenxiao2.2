namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.TaobaoNote)]
    public class TaoBaoNote : AdminPage
    {
        protected Button btnSave;
        protected ShippingModeDropDownList dropShippingType;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.dropShippingType.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择一个配送方式", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                masterSettings.TaobaoShippingType = this.dropShippingType.SelectedValue.Value;
                SettingsManager.Save(masterSettings);
                this.ShowMsg("成功的保存了淘宝代销配送方式", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropShippingType.DataBind();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.dropShippingType.SelectedValue = new int?(masterSettings.TaobaoShippingType);
            }
        }
    }
}


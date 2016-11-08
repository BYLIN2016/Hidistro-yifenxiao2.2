namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class DeductSettings : AdminPage
    {
        protected Button btnOK;
        protected TextBox txtDeduct;
        protected HtmlGenericControl txtDeductTip;

        private void btnOK_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (!int.TryParse(this.txtDeduct.Text.Trim(), out result))
            {
                this.ShowMsg("您输入的推荐人提成比例格式不对！", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                masterSettings.ReferralDeduct = result;
                SettingsManager.Save(masterSettings);
                this.ShowMsg("成功修改了推荐人提成比例", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtDeduct.Text = masterSettings.ReferralDeduct.ToString();
            }
        }
    }
}


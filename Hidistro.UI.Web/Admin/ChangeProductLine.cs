namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductLines)]
    public class ChangeProductLine : AdminPage
    {
        protected Button btnSaveCategory;
        protected ProductLineDropDownList dropProductLineFrom;
        protected ProductLineDropDownList dropProductLineFromTo;

        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (!this.dropProductLineFrom.SelectedValue.HasValue || !this.dropProductLineFromTo.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择需要替换的产品线或需要替换至的产品线", false);
            }
            else if (this.dropProductLineFrom.SelectedValue.Value == this.dropProductLineFromTo.SelectedValue.Value)
            {
                this.ShowMsg("请选择不同的产品进行替换", false);
            }
            else
            {
                string text = this.dropProductLineFrom.SelectedItem.Text;
                string str2 = this.dropProductLineFromTo.SelectedItem.Text;
                string str3 = this.dropProductLineFrom.SelectedValue.ToString();
                SendMessageHelper.SendMessageToDistributors(str3 + "|" + text + "|" + str2, 4);
                if (!ProductLineHelper.ReplaceProductLine(Convert.ToInt32(str3), Convert.ToInt32(this.dropProductLineFromTo.SelectedValue)))
                {
                    this.ShowMsg("产品线批量转移商品失败", false);
                }
                else
                {
                    this.ShowMsg("产品线批量转移商品成功", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveCategory.Click += new EventHandler(this.btnSaveCategory_Click);
            if (!base.IsPostBack)
            {
                this.dropProductLineFrom.DataBind();
                this.dropProductLineFromTo.DataBind();
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["LineId"]))
                {
                    int result = 0;
                    if (int.TryParse(this.Page.Request.QueryString["LineId"], out result))
                    {
                        this.dropProductLineFrom.SelectedValue = new int?(result);
                    }
                }
            }
        }
    }
}


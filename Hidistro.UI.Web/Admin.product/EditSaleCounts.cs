namespace Hidistro.UI.Web.Admin.product
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditSaleCounts : AdminPage
    {
        protected Button btnAddOK;
        protected Button btnOperationOK;
        protected Button btnSaveInfo;
        protected OperationDropDownList ddlOperation;
        protected Grid grdSelectedProducts;
        private string productIds = string.Empty;
        protected TextBox txtOperationSaleCounts;
        protected TextBox txtSaleCounts;

        private void BindProduct()
        {
            string str = this.Page.Request.QueryString["ProductIds"];
            if (!string.IsNullOrEmpty(str))
            {
                this.grdSelectedProducts.DataSource = ProductHelper.GetProductBaseInfo(str);
                this.grdSelectedProducts.DataBind();
            }
        }

        private void btnAddOK_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (!int.TryParse(this.txtSaleCounts.Text.Trim(), out result) || (result < 0))
            {
                this.ShowMsg("销售数量只能是正整数，请输入正确的销售数量", false);
            }
            else
            {
                if (ProductHelper.UpdateShowSaleCounts(this.productIds, result))
                {
                    this.ShowMsg("成功调整了前台显示的销售数量", true);
                }
                else
                {
                    this.ShowMsg("调整前台显示的销售数量失败", false);
                }
                this.BindProduct();
            }
        }

        private void btnOperationOK_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (!int.TryParse(this.txtOperationSaleCounts.Text.Trim(), out result) || (result < 0))
            {
                this.ShowMsg("销售数量只能是正整数，请输入正确的销售数量", false);
            }
            else
            {
                if (ProductHelper.UpdateShowSaleCounts(this.productIds, result, this.ddlOperation.SelectedValue))
                {
                    this.ShowMsg("成功调整了前台显示的销售数量", true);
                }
                else
                {
                    this.ShowMsg("调整前台显示的销售数量失败", false);
                }
                this.BindProduct();
            }
        }

        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ShowSaleCounts");
            if (this.grdSelectedProducts.Rows.Count > 0)
            {
                int result = 0;
                foreach (GridViewRow row in this.grdSelectedProducts.Rows)
                {
                    int num = (int) this.grdSelectedProducts.DataKeys[row.RowIndex].Value;
                    TextBox box = row.FindControl("txtShowSaleCounts") as TextBox;
                    if (int.TryParse(box.Text.Trim(), out result) && (result >= 0))
                    {
                        DataRow row2 = dt.NewRow();
                        row2["ProductId"] = num;
                        row2["ShowSaleCounts"] = result;
                        dt.Rows.Add(row2);
                    }
                }
                if (ProductHelper.UpdateShowSaleCounts(dt))
                {
                    this.ShowMsg("成功调整了前台显示的销售数量", true);
                }
                else
                {
                    this.ShowMsg("调整前台显示的销售数量失败", false);
                }
                this.BindProduct();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.productIds = this.Page.Request.QueryString["productIds"];
            this.btnSaveInfo.Click += new EventHandler(this.btnSaveInfo_Click);
            this.btnAddOK.Click += new EventHandler(this.btnAddOK_Click);
            this.btnOperationOK.Click += new EventHandler(this.btnOperationOK_Click);
            if (!this.Page.IsPostBack)
            {
                this.ddlOperation.DataBind();
                this.ddlOperation.SelectedValue = "+";
                this.BindProduct();
            }
        }
    }
}


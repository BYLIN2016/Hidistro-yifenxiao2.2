namespace Hidistro.UI.Web.Admin.product
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditDistributorPrices : AdminPage
    {
        protected Button btnOperationOK;
        protected Button btnSavePrice;
        protected Button btnTargetOK;
        protected DistributorPriceDropDownList ddlDistributorPrice;
        protected DistributorPriceDropDownList ddlDistributorPrice2;
        protected OperationDropDownList ddlOperation;
        protected DistributorPriceDropDownList ddlPurchasePrice;
        private string productIds = string.Empty;
        protected TextBox txtOperationPrice;
        protected TrimTextBox txtPrices;
        protected TextBox txtTargetPrice;

        private void btnOperationOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsg("没有要修改的商品", false);
            }
            else if (!this.ddlDistributorPrice2.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择要修改的价格", false);
            }
            else if (((this.ddlDistributorPrice2.SelectedValue.Value == -2) || (this.ddlDistributorPrice2.SelectedValue.Value == -4)) && ((this.ddlPurchasePrice.SelectedValue.Value != -2) && (this.ddlPurchasePrice.SelectedValue.Value != -4)))
            {
                this.ShowMsg("采购价或成本价不能用分销等级价作为标准来按公式计算", false);
            }
            else
            {
                decimal result = 0M;
                if (!decimal.TryParse(this.txtOperationPrice.Text.Trim(), out result))
                {
                    this.ShowMsg("请输入正确的价格", false);
                }
                else if ((this.ddlOperation.SelectedValue == "*") && (result <= 0M))
                {
                    this.ShowMsg("必须乘以一个正数", false);
                }
                else
                {
                    if ((this.ddlOperation.SelectedValue == "+") && (result < 0M))
                    {
                        decimal checkPrice = -result;
                        if (ProductHelper.CheckPrice(this.productIds, this.ddlPurchasePrice.SelectedValue.Value, checkPrice, false))
                        {
                            this.ShowMsg("加了一个太小的负数，导致价格中有负数的情况", false);
                            return;
                        }
                    }
                    if (ProductHelper.UpdateSkuDistributorPrices(this.productIds, this.ddlDistributorPrice2.SelectedValue.Value, this.ddlPurchasePrice.SelectedValue.Value, this.ddlOperation.SelectedValue, result))
                    {
                        this.ShowMsg("修改商品的价格成功", true);
                    }
                }
            }
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {
            DataSet skuPrices = this.GetSkuPrices();
            if (((skuPrices == null) || (skuPrices.Tables["skuPriceTable"] == null)) || (skuPrices.Tables["skuPriceTable"].Rows.Count == 0))
            {
                this.ShowMsg("没有任何要修改的项", false);
            }
            else if (ProductHelper.UpdateSkuDistributorPrices(skuPrices))
            {
                this.CloseWindow();
            }
        }

        private void btnTargetOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsg("没有要修改的商品", false);
            }
            else if (!this.ddlDistributorPrice.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择要修改的价格", false);
            }
            else
            {
                decimal result = 0M;
                if (!decimal.TryParse(this.txtTargetPrice.Text.Trim(), out result))
                {
                    this.ShowMsg("请输入正确的价格", false);
                }
                else if (result <= 0M)
                {
                    this.ShowMsg("直接调价必须输入正数", false);
                }
                else if (ProductHelper.UpdateSkuDistributorPrices(this.productIds, this.ddlDistributorPrice.SelectedValue.Value, result))
                {
                    this.ShowMsg("修改成功", true);
                }
            }
        }

        private DataSet GetSkuPrices()
        {
            DataSet set = null;
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(this.txtPrices.Text);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }
                set = new DataSet();
                DataTable table = new DataTable("skuPriceTable");
                table.Columns.Add("skuId");
                table.Columns.Add("costPrice");
                table.Columns.Add("purchasePrice");
                DataTable table2 = new DataTable("skuDistributorPriceTable");
                table2.Columns.Add("skuId");
                table2.Columns.Add("gradeId");
                table2.Columns.Add("distributorPrice");
                foreach (XmlNode node in list)
                {
                    DataRow row = table.NewRow();
                    row["skuId"] = node.Attributes["skuId"].Value;
                    row["costPrice"] = string.IsNullOrEmpty(node.Attributes["costPrice"].Value) ? 0M : decimal.Parse(node.Attributes["costPrice"].Value);
                    row["purchasePrice"] = decimal.Parse(node.Attributes["purchasePrice"].Value);
                    table.Rows.Add(row);
                    foreach (XmlNode node2 in node.SelectSingleNode("skuDistributorPrices").ChildNodes)
                    {
                        DataRow row2 = table2.NewRow();
                        row2["skuId"] = node.Attributes["skuId"].Value;
                        row2["gradeId"] = int.Parse(node2.Attributes["gradeId"].Value);
                        row2["distributorPrice"] = decimal.Parse(node2.Attributes["distributorPrice"].Value);
                        table2.Rows.Add(row2);
                    }
                }
                set.Tables.Add(table);
                set.Tables.Add(table2);
            }
            catch
            {
            }
            return set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.productIds = this.Page.Request.QueryString["productIds"];
            this.btnSavePrice.Click += new EventHandler(this.btnSavePrice_Click);
            this.btnTargetOK.Click += new EventHandler(this.btnTargetOK_Click);
            this.btnOperationOK.Click += new EventHandler(this.btnOperationOK_Click);
            if (!this.Page.IsPostBack)
            {
                this.ddlDistributorPrice.DataBind();
                this.ddlDistributorPrice.SelectedValue = -4;
                this.ddlDistributorPrice2.DataBind();
                this.ddlDistributorPrice2.SelectedValue = -4;
                this.ddlPurchasePrice.DataBind();
                this.ddlPurchasePrice.SelectedValue = -4;
                this.ddlOperation.DataBind();
                this.ddlOperation.SelectedValue = "+";
            }
        }
    }
}


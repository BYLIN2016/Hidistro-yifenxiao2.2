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
    public class EditMemberPrices : AdminPage
    {
        protected Button btnOperationOK;
        protected Button btnSavePrice;
        protected Button btnTargetOK;
        protected MemberPriceDropDownList ddlMemberPrice;
        protected MemberPriceDropDownList ddlMemberPrice2;
        protected OperationDropDownList ddlOperation;
        protected MemberPriceDropDownList ddlSalePrice;
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
            else if (!this.ddlMemberPrice2.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择要修改的价格", false);
            }
            else if (((this.ddlMemberPrice2.SelectedValue.Value == -2) || (this.ddlMemberPrice2.SelectedValue.Value == -3)) && ((this.ddlSalePrice.SelectedValue.Value != -2) && (this.ddlSalePrice.SelectedValue.Value != -3)))
            {
                this.ShowMsg("一口价或成本价不能用会员等级价作为标准来按公式计算", false);
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
                        if (ProductHelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue.Value, checkPrice, true))
                        {
                            this.ShowMsg("加了一个太小的负数，导致价格中有负数的情况", false);
                            return;
                        }
                    }
                    if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice2.SelectedValue.Value, this.ddlSalePrice.SelectedValue.Value, this.ddlOperation.SelectedValue, result))
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
            else if (ProductHelper.UpdateSkuMemberPrices(skuPrices))
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
            else if (!this.ddlMemberPrice.SelectedValue.HasValue)
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
                else if (result > 10000000M)
                {
                    this.ShowMsg("直接调价超出了系统表示范围", false);
                }
                else if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice.SelectedValue.Value, result))
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
                table.Columns.Add("salePrice");
                DataTable table2 = new DataTable("skuMemberPriceTable");
                table2.Columns.Add("skuId");
                table2.Columns.Add("gradeId");
                table2.Columns.Add("memberPrice");
                foreach (XmlNode node in list)
                {
                    DataRow row = table.NewRow();
                    row["skuId"] = node.Attributes["skuId"].Value;
                    row["costPrice"] = string.IsNullOrEmpty(node.Attributes["costPrice"].Value) ? 0M : decimal.Parse(node.Attributes["costPrice"].Value);
                    row["salePrice"] = decimal.Parse(node.Attributes["salePrice"].Value);
                    table.Rows.Add(row);
                    foreach (XmlNode node2 in node.SelectSingleNode("skuMemberPrices").ChildNodes)
                    {
                        DataRow row2 = table2.NewRow();
                        row2["skuId"] = node.Attributes["skuId"].Value;
                        row2["gradeId"] = int.Parse(node2.Attributes["gradeId"].Value);
                        row2["memberPrice"] = decimal.Parse(node2.Attributes["memberPrice"].Value);
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
                this.ddlMemberPrice.DataBind();
                this.ddlMemberPrice.SelectedValue = -3;
                this.ddlMemberPrice2.DataBind();
                this.ddlMemberPrice2.SelectedValue = -3;
                this.ddlSalePrice.DataBind();
                this.ddlSalePrice.SelectedValue = -3;
                this.ddlOperation.DataBind();
                this.ddlOperation.SelectedValue = "+";
            }
        }
    }
}


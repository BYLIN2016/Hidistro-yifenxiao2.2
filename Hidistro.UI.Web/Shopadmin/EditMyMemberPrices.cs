namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class EditMyMemberPrices : DistributorPage
    {
        protected Button btnOperationOK;
        protected Button btnSavePrice;
        protected Button btnTargetOK;
        protected OperationDropDownList ddlOperation;
        protected SalePriceDropDownList ddlSalePrice;
        protected UnderlingPriceDropDownList ddlUnderlingPrice;
        protected UnderlingPriceDropDownList ddlUnderlingPrice2;
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
            else if (!this.ddlUnderlingPrice.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择要修改的价格", false);
            }
            else if (!this.ddlUnderlingPrice2.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择要修改的价格", false);
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
                        if (SubSiteProducthelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue, checkPrice))
                        {
                            this.ShowMsg("加了一个太小的负数，导致价格中有负数的情况", false);
                            return;
                        }
                    }
                    if (SubSiteProducthelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue, result, this.ddlOperation.SelectedValue))
                    {
                        this.ShowMsg("公式调价的计算结果导致价格超出了系统表示范围", false);
                    }
                    else if (SubSiteProducthelper.UpdateSkuUnderlingPrices(this.productIds, this.ddlUnderlingPrice2.SelectedValue.Value, this.ddlSalePrice.SelectedValue, this.ddlOperation.SelectedValue, result))
                    {
                        this.ShowMsg("修改商品的价格成功", true);
                    }
                }
            }
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {
            string skuIds = string.Empty;
            DataSet skuPrices = this.GetSkuPrices(out skuIds);
            if (string.IsNullOrEmpty(skuIds))
            {
                this.ShowMsg("没有任何要修改的项", false);
            }
            else if (SubSiteProducthelper.UpdateSkuUnderlingPrices(skuPrices, skuIds))
            {
                this.ShowMsg("修改商品的价格成功", true);
            }
        }

        private void btnTargetOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsg("没有要修改的商品", false);
            }
            else if (!this.ddlUnderlingPrice.SelectedValue.HasValue)
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
                else if (SubSiteProducthelper.UpdateSkuUnderlingPrices(this.productIds, this.ddlUnderlingPrice.SelectedValue.Value, result))
                {
                    this.ShowMsg("修改商品的价格成功", true);
                }
            }
        }

        private DataSet GetSkuPrices(out string skuIds)
        {
            DataSet set = null;
            skuIds = string.Empty;
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(this.txtPrices.Text);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }
                IList<SKUItem> skus = SubSiteProducthelper.GetSkus(this.productIds);
                set = new DataSet();
                DataTable table = new DataTable("skuPriceTable");
                table.Columns.Add("skuId");
                table.Columns.Add("salePrice");
                DataTable table2 = new DataTable("skuMemberPriceTable");
                table2.Columns.Add("skuId");
                table2.Columns.Add("gradeId");
                table2.Columns.Add("memberPrice");
                foreach (XmlNode node in list)
                {
                    string str = node.Attributes["skuId"].Value;
                    decimal num = decimal.Parse(node.Attributes["salePrice"].Value);
                    skuIds = skuIds + "'" + str + "',";
                    foreach (SKUItem item in skus)
                    {
                        if ((item.SkuId == str) && (item.SalePrice != num))
                        {
                            DataRow row = table.NewRow();
                            row["skuId"] = str;
                            row["salePrice"] = num;
                            table.Rows.Add(row);
                        }
                    }
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
                if (skuIds.Length > 1)
                {
                    skuIds = skuIds.Remove(skuIds.Length - 1);
                }
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
                this.ddlUnderlingPrice.DataBind();
                this.ddlUnderlingPrice.SelectedValue = -3;
                this.ddlUnderlingPrice2.DataBind();
                this.ddlUnderlingPrice2.SelectedValue = -3;
                this.ddlSalePrice.DataBind();
                this.ddlSalePrice.SelectedValue = "CostPrice";
                this.ddlOperation.DataBind();
                this.ddlOperation.SelectedValue = "+";
            }
        }
    }
}


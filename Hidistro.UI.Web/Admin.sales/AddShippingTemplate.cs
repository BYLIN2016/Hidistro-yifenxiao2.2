namespace Hidistro.UI.Web.Admin.sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddShippingTemplate : AdminPage
    {
        protected Button btnAdd;
        protected Button btnCreate;
        protected Grid grdRegion;
        protected RegionArea regionArea;
        protected Script Script1;
        protected Hidistro.UI.Common.Controls.Style Style1;
        protected TextBox txtAddPrice;
        protected TextBox txtAddRegionPrice;
        protected TextBox txtAddWeight;
        protected TextBox txtModeName;
        protected TextBox txtPrice;
        protected HtmlInputText txtRegion;
        protected TextBox txtRegion_Id;
        protected TextBox txtRegionPrice;
        protected TextBox txtWeight;

        private void BindRegion()
        {
            this.grdRegion.DataSource = this.RegionList;
            this.grdRegion.DataBind();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            decimal num;
            decimal num2;
            if (this.ValidateValues(out num, out num2))
            {
                Region item = new Region();
                item.RegionsId = this.txtRegion_Id.Text;
                item.Regions = this.txtRegion.Value;
                item.RegionPrice = num;
                item.RegionAddPrice = num2;
                this.RegionList.Add(item);
                this.BindRegion();
                this.txtRegion_Id.Text = string.Empty;
                this.txtRegion.Value = string.Empty;
                this.txtRegionPrice.Text = "0";
                this.txtAddRegionPrice.Text = "0";
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            decimal num;
            decimal? nullable;
            decimal num2;
            decimal? nullable2;
            if (this.ValidateRegionValues(out num, out nullable, out num2, out nullable2))
            {
                new List<ShippingModeGroupInfo>();
                ShippingModeInfo shippingMode = new ShippingModeInfo();
                shippingMode.Name = Globals.HtmlEncode(this.txtModeName.Text.Trim());
                shippingMode.Weight = num;
                shippingMode.AddWeight = nullable;
                shippingMode.Price = num2;
                shippingMode.AddPrice = nullable2;
                foreach (GridViewRow row in this.grdRegion.Rows)
                {
                    decimal result = 0M;
                    decimal num4 = 0M;
                    decimal.TryParse(((TextBox) row.FindControl("txtModeRegionPrice")).Text, out result);
                    decimal.TryParse(((TextBox) row.FindControl("txtModeRegionAddPrice")).Text, out num4);
                    ShippingModeGroupInfo item = new ShippingModeGroupInfo();
                    item.Price = result;
                    item.AddPrice = num4;
                    TextBox box = (TextBox) this.grdRegion.Rows[row.RowIndex].FindControl("txtRegionvalue_Id");
                    if (!string.IsNullOrEmpty(box.Text))
                    {
                        foreach (string str in box.Text.Split(new char[] { ',' }))
                        {
                            ShippingRegionInfo info3 = new ShippingRegionInfo();
                            info3.RegionId = Convert.ToInt32(str.Trim());
                            item.ModeRegions.Add(info3);
                        }
                    }
                    shippingMode.ModeGroup.Add(item);
                }
                if (SalesHelper.CreateShippingTemplate(shippingMode))
                {
                    if (!string.IsNullOrEmpty(this.Page.Request.QueryString["source"]) && (this.Page.Request.QueryString["source"] == "add"))
                    {
                        this.CloseWindow();
                    }
                    else
                    {
                        this.ClearControlValue();
                        this.ShowMsg("成功添加了一个配送方式模板", true);
                    }
                }
                else
                {
                    this.ShowMsg("您添加的地区有重复", false);
                }
            }
        }

        private void ClearControlValue()
        {
            this.txtAddPrice.Text = string.Empty;
            this.txtAddRegionPrice.Text = string.Empty;
            this.txtAddWeight.Text = string.Empty;
            this.txtModeName.Text = string.Empty;
            this.txtPrice.Text = string.Empty;
            this.txtRegion.Value = string.Empty;
            this.txtRegion_Id.Text = string.Empty;
            this.txtRegionPrice.Text = string.Empty;
            this.txtWeight.Text = string.Empty;
        }

        private void grdRegion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.RegionList.RemoveAt(e.RowIndex);
            this.BindRegion();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.grdRegion.RowDeleting += new GridViewDeleteEventHandler(this.grdRegion_RowDeleting);
        }

        private bool ValidateRegionValues(out decimal weight, out decimal? addWeight, out decimal price, out decimal? addPrice)
        {
            string str = string.Empty;
            addWeight = 0;
            addPrice = 0;
            if (!decimal.TryParse(this.txtWeight.Text.Trim(), out weight))
            {
                str = str + Formatter.FormatErrorMessage("起步重量不能为空,必须为正整数,限制在100千克以内");
            }
            if (!string.IsNullOrEmpty(this.txtAddWeight.Text.Trim()))
            {
                decimal num;
                if (decimal.TryParse(this.txtAddWeight.Text.Trim(), out num))
                {
                    addWeight = new decimal?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("加价重量不能为空,必须为正整数,限制在100千克以内");
                }
            }
            if (!decimal.TryParse(this.txtPrice.Text.Trim(), out price))
            {
                str = str + Formatter.FormatErrorMessage("默认起步价必须为非负数字,限制在1000万以内");
            }
            if (!string.IsNullOrEmpty(this.txtAddPrice.Text.Trim()))
            {
                decimal num2;
                if (decimal.TryParse(this.txtAddPrice.Text.Trim(), out num2))
                {
                    addPrice = new decimal?(num2);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("默认加价必须为非负数字,限制在1000万以内");
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }

        private bool ValidateValues(out decimal regionPrice, out decimal addRegionPrice)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(this.txtRegion_Id.Text))
            {
                str = str + Formatter.FormatErrorMessage("到达地不能为空");
            }
            if (string.IsNullOrEmpty(this.txtRegionPrice.Text))
            {
                str = str + Formatter.FormatErrorMessage("起步价不能为空");
                regionPrice = 0M;
            }
            else if (!decimal.TryParse(this.txtRegionPrice.Text.Trim(), out regionPrice))
            {
                str = str + Formatter.FormatErrorMessage("起步价只能为非负数字");
            }
            else if (decimal.Parse(this.txtRegionPrice.Text.Trim()) > 10000000M)
            {
                str = str + Formatter.FormatErrorMessage("起步价限制在1000万以内");
            }
            if (string.IsNullOrEmpty(this.txtAddRegionPrice.Text))
            {
                str = str + Formatter.FormatErrorMessage("加价不能为空");
                addRegionPrice = 0M;
            }
            else if (!decimal.TryParse(this.txtAddRegionPrice.Text.Trim(), out addRegionPrice))
            {
                str = str + Formatter.FormatErrorMessage("加价只能为非负数字");
            }
            else if (decimal.Parse(this.txtAddRegionPrice.Text.Trim()) > 10000000M)
            {
                str = str + Formatter.FormatErrorMessage("加价限制在1000万以内");
            }
            string.IsNullOrEmpty(str);
            return true;
        }

        private IList<Region> RegionList
        {
            get
            {
                if (this.ViewState["Region"] == null)
                {
                    this.ViewState["Region"] = new List<Region>();
                }
                return (IList<Region>) this.ViewState["Region"];
            }
        }

        [Serializable]
        public class Region
        {
            private decimal regionAddPrice;
            private decimal regionPrice;
            private string regions;
            private string regionsId;

            public decimal RegionAddPrice
            {
                get
                {
                    return this.regionAddPrice;
                }
                set
                {
                    this.regionAddPrice = value;
                }
            }

            public decimal RegionPrice
            {
                get
                {
                    return this.regionPrice;
                }
                set
                {
                    this.regionPrice = value;
                }
            }

            public string Regions
            {
                get
                {
                    return this.regions;
                }
                set
                {
                    this.regions = value;
                }
            }

            public string RegionsId
            {
                get
                {
                    return this.regionsId;
                }
                set
                {
                    this.regionsId = value;
                }
            }
        }
    }
}


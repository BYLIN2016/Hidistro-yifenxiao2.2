namespace Hidistro.UI.Web.Admin.sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditShippingTemplate : AdminPage
    {
        protected Button btnAdd;
        protected Button btnUpdate;
        protected Grid grdRegion;
        protected RegionArea regionArea;
        protected Script Script1;
        protected Hidistro.UI.Common.Controls.Style Style1;
        private int templateId;
        protected TextBox txtAddPrice;
        protected TextBox txtAddRegionPrice;
        protected TextBox txtAddWeight;
        protected TextBox txtModeName;
        protected TextBox txtPrice;
        protected HtmlInputText txtRegion;
        protected TextBox txtRegion_Id;
        protected TextBox txtRegionPrice;
        protected TextBox txtWeight;

        private void BindControl(ShippingModeInfo modeItem)
        {
            this.txtModeName.Text = Globals.HtmlDecode(modeItem.Name);
            this.txtWeight.Text = modeItem.Weight.ToString("F2");
            this.txtAddWeight.Text = modeItem.AddWeight.Value.ToString("F2");
            if (modeItem.AddPrice.HasValue)
            {
                this.txtAddPrice.Text = modeItem.AddPrice.Value.ToString("F2");
            }
            this.txtPrice.Text = modeItem.Price.ToString("F2");
            this.RegionList.Clear();
            if ((modeItem.ModeGroup != null) && (modeItem.ModeGroup.Count > 0))
            {
                foreach (ShippingModeGroupInfo info in modeItem.ModeGroup)
                {
                    Region item = new Region();
                    item.RegionPrice = decimal.Parse(info.Price.ToString("F2"));
                    item.RegionAddPrice = decimal.Parse(info.AddPrice.ToString("F2"));
                    StringBuilder builder = new StringBuilder();
                    StringBuilder builder2 = new StringBuilder();
                    foreach (ShippingRegionInfo info2 in info.ModeRegions)
                    {
                        builder.Append(info2.RegionId + ",");
                        builder2.Append(RegionHelper.GetFullRegion(info2.RegionId, ",") + ",");
                    }
                    if (!string.IsNullOrEmpty(builder.ToString()))
                    {
                        item.RegionsId = builder.ToString().Substring(0, builder.ToString().Length - 1);
                    }
                    if (!string.IsNullOrEmpty(builder2.ToString()))
                    {
                        item.Regions = builder2.ToString().Substring(0, builder2.ToString().Length - 1);
                    }
                    this.RegionList.Add(item);
                }
            }
        }

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

        private void btnUpdate_Click(object sender, EventArgs e)
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
                shippingMode.TemplateId = this.templateId;
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
                if (SalesHelper.UpdateShippingTemplate(shippingMode))
                {
                    this.Page.Response.Redirect("EditShippingTemplate.aspx?TemplateId=" + shippingMode.TemplateId + "&isUpdate=true");
                }
                else
                {
                    this.ShowMsg("您添加的地区有重复", false);
                }
            }
        }

        private void grdRegion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.RegionList.RemoveAt(e.RowIndex);
            this.BindRegion();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["TemplateId"], out this.templateId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
                this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
                this.grdRegion.RowDeleting += new GridViewDeleteEventHandler(this.grdRegion_RowDeleting);
                if (!this.Page.IsPostBack)
                {
                    if ((this.Page.Request.QueryString["isUpdate"] != null) && (this.Page.Request.QueryString["isUpdate"] == "true"))
                    {
                        this.ShowMsg("成功修改了一个配送方式", true);
                    }
                    ShippingModeInfo shippingTemplate = SalesHelper.GetShippingTemplate(this.templateId, true);
                    if (shippingTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.BindControl(shippingTemplate);
                        this.BindRegion();
                    }
                }
            }
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
                str = str + Formatter.FormatErrorMessage("默认起步价不能为空,必须为非负数字,限制在1000万以内");
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
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
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


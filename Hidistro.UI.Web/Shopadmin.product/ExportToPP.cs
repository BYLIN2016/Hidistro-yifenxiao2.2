namespace Hidistro.UI.Web.Shopadmin.product
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.TransferManager;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ExportToPP : DistributorPage
    {
        private DateTime? _endDate;
        private int? _lineId;
        private string _productCode;
        private string _productName;
        private DateTime? _startDate;
        protected Button btnExport;
        protected Button btnSearch;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected CheckBox chkExportStock;
        protected DropDownList dropExportVersions;
        protected ProductLineDropDownList dropLines;
        protected Grid grdProducts;
        protected Label lblTotals;
        protected Pager pager;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void BindExporter()
        {
            this.dropExportVersions.Items.Clear();
            this.dropExportVersions.Items.Add(new ListItem("-请选择-", ""));
            Dictionary<string, string> exportAdapters = TransferHelper.GetExportAdapters(new YfxTarget("1.2"), "拍拍助理");
            foreach (string str in exportAdapters.Keys)
            {
                this.dropExportVersions.Items.Add(new ListItem(exportAdapters[str], str));
            }
        }

        private void BindProducts()
        {
            DbQueryResult exportProducts = SubSiteProducthelper.GetExportProducts(this.GetQuery(), (string) this.ViewState["RemoveProductIds"]);
            this.grdProducts.DataSource = exportProducts.Data;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = exportProducts.TotalRecords;
            this.lblTotals.Text = exportProducts.TotalRecords.ToString(CultureInfo.InvariantCulture);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string selectedValue = this.dropExportVersions.SelectedValue;
            if (string.IsNullOrEmpty(selectedValue) || (selectedValue.Length == 0))
            {
                this.ShowMsg("请选择一个导出版本", false);
            }
            else
            {
                bool includeCostPrice = false;
                bool includeStock = this.chkExportStock.Checked;
                bool flag3 = true;
                string str2 = "http://" + HttpContext.Current.Request.Url.Host + ((HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + HttpContext.Current.Request.Url.Port)) + Globals.ApplicationPath;
                string applicationPath = Globals.ApplicationPath;
                DataSet set = SubSiteProducthelper.GetExportProducts(this.GetQuery(), includeCostPrice, includeStock, (string) this.ViewState["RemoveProductIds"]);
                TransferHelper.GetExporter(selectedValue, new object[] { set, includeCostPrice, includeStock, flag3, str2, applicationPath }).DoExport();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReSearchProducts();
        }

        private AdvancedProductQuery GetQuery()
        {
            AdvancedProductQuery query2 = new AdvancedProductQuery();
            query2.Keywords = this._productName;
            query2.ProductCode = this._productCode;
            query2.ProductLineId = this._lineId;
            query2.PageSize = this.pager.PageSize;
            query2.PageIndex = this.pager.PageIndex;
            query2.SaleStatus = ProductSaleStatus.OnSale;
            query2.SortOrder = SortAction.Desc;
            query2.SortBy = "DisplaySequence";
            query2.StartDate = this._startDate;
            query2.EndDate = this._endDate;
            AdvancedProductQuery entity = query2;
            Globals.EntityCoding(entity, true);
            return entity;
        }

        private void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int num2 = (int) this.grdProducts.DataKeys[rowIndex].Value;
                string str = (string) this.ViewState["RemoveProductIds"];
                if (string.IsNullOrEmpty(str))
                {
                    str = num2.ToString();
                }
                else
                {
                    str = str + "," + num2.ToString();
                }
                this.ViewState["RemoveProductIds"] = str;
                this.BindProducts();
            }
        }

        private void LoadParameters()
        {
            int num;
            DateTime time;
            DateTime time2;
            this._productName = this.txtSearchText.Text.Trim();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this._productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
                this.txtSearchText.Text = this._productName;
            }
            this._productCode = this.txtSKU.Text.Trim();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this._productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
                this.txtSKU.Text = this._productCode;
            }
            this._lineId = this.dropLines.SelectedValue;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lineId"]) && int.TryParse(this.Page.Request.QueryString["lineId"], out num))
            {
                this._lineId = new int?(num);
                this.dropLines.SelectedValue = this._lineId;
            }
            this._startDate = this.calendarStartDate.SelectedDate;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]) && DateTime.TryParse(this.Page.Request.QueryString["startDate"], out time))
            {
                this._startDate = new DateTime?(time);
                this.calendarStartDate.SelectedDate = this._startDate;
            }
            this._endDate = this.calendarEndDate.SelectedDate;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]) && DateTime.TryParse(this.Page.Request.QueryString["endDate"], out time2))
            {
                this._endDate = new DateTime?(time2);
                this.calendarEndDate.SelectedDate = this._endDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.grdProducts.RowCommand += new GridViewCommandEventHandler(this.grdProducts_RowCommand);
            if (!this.Page.IsPostBack)
            {
                this.dropLines.DataBind();
                this.BindExporter();
            }
            this.LoadParameters();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindProducts();
            }
        }

        private void ReSearchProducts()
        {
            NameValueCollection values2 = new NameValueCollection();
            values2.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            values2.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
            values2.Add("pageSize", this.pager.PageSize.ToString());
            NameValueCollection queryStrings = values2;
            if (this.dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", this.dropLines.SelectedValue.ToString());
            }
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}


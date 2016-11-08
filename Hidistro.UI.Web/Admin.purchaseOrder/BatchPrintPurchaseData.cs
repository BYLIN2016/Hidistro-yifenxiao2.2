namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;

    public class BatchPrintPurchaseData : AdminPage
    {
        private DirectoryInfo _baseDir;
        private readonly Encoding _encoding = Encoding.Unicode;
        private string _flag;
        private DirectoryInfo _flexDir;
        private DirectoryInfo _workDir;
        private string _zipFilename;
        protected Button btnPrint;
        protected Button btnUpdateAddrdss;
        protected ShippersDropDownList ddlShoperTag;
        protected DropDownList ddlTemplates;
        protected RegionSelector dropRegions;
        private const string ExpressName = "post.xml";
        private static bool isPO = true;
        protected Literal litNumber;
        protected static string orderIds = string.Empty;
        private const string OrdersName = "orders.xml";
        private static string picPath = string.Empty;
        protected Panel pnlEmptySender;
        protected Panel pnlEmptyTemplates;
        protected Panel pnlPOEmpty;
        protected Panel pnlShipper;
        protected Panel pnlTask;
        protected Panel pnlTaskEmpty;
        protected Panel pnlTemplates;
        private static int taskId = 0;
        protected TextBox txtAddress;
        protected TextBox txtCellphone;
        protected TextBox txtShipTo;
        protected TextBox txtStartCode;
        protected TextBox txtTelphone;
        protected TextBox txtZipcode;

        private void btnUpdateAddrdss_Click(object sender, EventArgs e)
        {
            if (!this.dropRegions.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择发货地区！", false);
            }
            else if (this.UpdateAddress())
            {
                this.ShowMsg("修改成功", true);
            }
            else
            {
                this.ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
            }
        }

        private decimal CalculateOrderTotal(DataRow order, DataSet ds)
        {
            decimal result = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            decimal num4 = 0M;
            bool flag = false;
            decimal.TryParse(order["AdjustedFreight"].ToString(), out result);
            decimal.TryParse(order["AdjustedPayCharge"].ToString(), out num2);
            string str = order["CouponCode"].ToString();
            decimal.TryParse(order["CouponValue"].ToString(), out num3);
            decimal.TryParse(order["AdjustedDiscount"].ToString(), out num4);
            bool.TryParse(order["OrderOptionFree"].ToString(), out flag);
            DataRow[] orderGift = ds.Tables[3].Select("OrderId='" + order["orderId"] + "'");
            DataRow[] orderLine = ds.Tables[1].Select("OrderId='" + order["orderId"] + "'");
            DataRow[] orderOption = ds.Tables[2].Select("OrderId='" + order["orderId"] + "'");
            decimal num5 = this.GetAmount(orderGift, orderLine, order) + result;
            num5 += num2;
            num5 += this.GetOptionPrice(orderOption, flag);
            if (!string.IsNullOrEmpty(str))
            {
                num5 -= num3;
            }
            return (num5 + num4);
        }

        private void ddlShoperTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadShipper();
        }

        private ShippersInfo ForDistorShipper(DataSet ds, DataRow order)
        {
            int result = 0;
            int.TryParse(order["DistributorId"].ToString(), out result);
            if ((result <= 0) && (ds.Tables.Count > 4))
            {
                return null;
            }
            DataRow[] rowArray = ds.Tables[4].Select("DistributorUserId=" + result);
            if (rowArray.Length <= 0)
            {
                return null;
            }
            ShippersInfo info = new ShippersInfo();
            DataRow row = rowArray[0];
            if (row["Address"] != DBNull.Value)
            {
                info.Address = (string) row["Address"];
            }
            if (row["CellPhone"] != DBNull.Value)
            {
                info.CellPhone = (string) row["CellPhone"];
            }
            if (row["RegionId"] != DBNull.Value)
            {
                info.RegionId = (int) row["RegionId"];
            }
            if (row["Remark"] != DBNull.Value)
            {
                info.Remark = (string) row["Remark"];
            }
            if (row["ShipperName"] != DBNull.Value)
            {
                info.ShipperName = (string) row["ShipperName"];
            }
            if (row["ShipperTag"] != DBNull.Value)
            {
                info.ShipperTag = (string) row["ShipperTag"];
            }
            if (row["TelPhone"] != DBNull.Value)
            {
                info.TelPhone = (string) row["TelPhone"];
            }
            if (row["Zipcode"] != DBNull.Value)
            {
                info.Zipcode = (string) row["Zipcode"];
            }
            return info;
        }

        public decimal GetAmount(DataRow[] orderGift, DataRow[] orderLine, DataRow order)
        {
            return (this.GetGoodDiscountAmount(order, orderLine) + this.GetGiftAmount(orderGift));
        }

        public decimal GetGiftAmount(DataRow[] rows)
        {
            decimal num = 0M;
            foreach (DataRow row in rows)
            {
                num += decimal.Parse(row["CostPrice"].ToString());
            }
            return num;
        }

        public decimal GetGoodDiscountAmount(DataRow order, DataRow[] orderLine)
        {
            decimal result = 0M;
            decimal.TryParse(order["DiscountAmount"].ToString(), out result);
            return this.GetGoodsAmount(orderLine);
        }

        public decimal GetGoodsAmount(DataRow[] rows)
        {
            decimal num = 0M;
            foreach (DataRow row in rows)
            {
                num += decimal.Parse(row["ItemAdjustedPrice"].ToString()) * int.Parse(row["Quantity"].ToString());
            }
            return num;
        }

        public decimal GetOptionAmout(DataRow[] orderOption)
        {
            decimal num = 0M;
            foreach (DataRow row in orderOption)
            {
                num += decimal.Parse(row["AdjustedPrice"].ToString());
            }
            return num;
        }

        public decimal GetOptionPrice(DataRow[] orderOption, bool OrderOptonFree)
        {
            if (!OrderOptonFree)
            {
                return this.GetOptionAmout(orderOption);
            }
            return 0M;
        }

        private void LoadShipper()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                this.txtAddress.Text = shipper.Address;
                this.txtCellphone.Text = shipper.CellPhone;
                this.txtShipTo.Text = shipper.ShipperName;
                this.txtTelphone.Text = shipper.TelPhone;
                this.txtZipcode.Text = shipper.Zipcode;
                this.dropRegions.SetSelectedRegionId(new int?(shipper.RegionId));
                this.pnlEmptySender.Visible = false;
                this.pnlShipper.Visible = true;
            }
            else
            {
                this.pnlShipper.Visible = false;
                this.pnlEmptySender.Visible = true;
            }
        }

        private void LoadTemplates()
        {
            DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
            if ((isUserExpressTemplates != null) && (isUserExpressTemplates.Rows.Count > 0))
            {
                this.ddlTemplates.Items.Add(new ListItem("-请选择-", ""));
                foreach (DataRow row in isUserExpressTemplates.Rows)
                {
                    this.ddlTemplates.Items.Add(new ListItem(row["ExpressName"].ToString(), row["XmlFile"].ToString()));
                }
                this.pnlEmptyTemplates.Visible = false;
                this.pnlTemplates.Visible = true;
            }
            else
            {
                this.pnlEmptyTemplates.Visible = true;
                this.pnlTemplates.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(this.Page.Request.QueryString["taskId"], out taskId);
            if (!string.IsNullOrEmpty(base.Request["PurchaseOrderIds"]))
            {
                orderIds = base.Request["PurchaseOrderIds"];
                this.litNumber.Text = orderIds.Trim(new char[] { ',' }).Split(new char[] { ',' }).Length.ToString();
            }
            this._flexDir = new DirectoryInfo(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/master/flex/"));
            this.ddlShoperTag.SelectedIndexChanged += new EventHandler(this.ddlShoperTag_SelectedIndexChanged);
            this.btnUpdateAddrdss.Click += new EventHandler(this.btnUpdateAddrdss_Click);
            if (!this.Page.IsPostBack)
            {
                this.ddlShoperTag.IncludeDistributor = true;
                this.ddlShoperTag.DataBind();
                foreach (ShippersInfo info in SalesHelper.GetShippers(true))
                {
                    if (info.IsDefault)
                    {
                        this.ddlShoperTag.SelectedValue = info.ShipperId;
                    }
                }
                this.LoadShipper();
                this.LoadTemplates();
            }
        }

        private bool UpdateAddress()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                shipper.Address = this.txtAddress.Text;
                shipper.CellPhone = this.txtCellphone.Text;
                shipper.RegionId = this.dropRegions.GetSelectedRegionId().Value;
                shipper.ShipperName = this.txtShipTo.Text;
                shipper.TelPhone = this.txtTelphone.Text;
                shipper.Zipcode = this.txtZipcode.Text;
                return SalesHelper.UpdateShipper(shipper);
            }
            return false;
        }

        private string WriteOrderInfo(DataRow order, ShippersInfo shipper, DataTable dtLine, DataSet ds)
        {
            string[] strArray = order["shippingRegion"].ToString().Split(new char[] { '，' });
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<order>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ShipTo"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["TelPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["CellPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ZipCode"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Address"]);
            builder.AppendLine("</item>");
            string str = string.Empty;
            if (strArray.Length > 0)
            {
                str = strArray[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 1)
            {
                str = strArray[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 2)
            {
                str = strArray[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            string[] strArray2 = new string[] { "", "", "" };
            if (shipper != null)
            {
                strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[] { '-' });
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.ShipperName : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.CellPhone : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.TelPhone : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.Address : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.Zipcode : "");
            builder.AppendLine("</item>");
            string str2 = string.Empty;
            if (strArray2.Length > 0)
            {
                str2 = strArray2[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str2);
            builder.AppendLine("</item>");
            str2 = string.Empty;
            if (strArray2.Length > 1)
            {
                str2 = strArray2[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str2);
            builder.AppendLine("</item>");
            str2 = string.Empty;
            if (strArray2.Length > 2)
            {
                str2 = strArray2[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str2);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-订单号</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["OrderId"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-总金额</name>");
            builder.AppendFormat("<rename>{0}</rename>", this.CalculateOrderTotal(order, ds));
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-物品总重量</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Weight"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-备注</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Remark"]);
            builder.AppendLine("</item>");
            DataRow[] rowArray = dtLine.Select(" OrderId='" + order["OrderId"] + "'");
            string str3 = string.Empty;
            if (rowArray.Length > 0)
            {
                foreach (DataRow row in rowArray)
                {
                    str3 = string.Concat(new object[] { str3, "货号 ", row["SKU"], " \x00d7", row["ShipmentQuantity"], "\n" });
                }
                str3 = str3.Replace("；", "");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-详情</name>");
            builder.AppendFormat("<rename>{0}</rename>", str3);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-送货时间</name>");
            builder.AppendFormat("<rename></rename>", new object[0]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>网店名称</name>");
            builder.AppendFormat("<rename>{0}</rename>", HiContext.Current.SiteSettings.SiteName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>自定义内容</name>");
            builder.AppendFormat("<rename>{0}</rename>", "null");
            builder.AppendLine("</item>");
            builder.AppendLine("</order>");
            return builder.ToString();
        }

        private string WritePurchaseOrderInfo(DataRow order, ShippersInfo shipper, DataTable dtLine, DataSet ds)
        {
            string[] strArray = order["shippingRegion"].ToString().Split(new char[] { '，' });
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<order>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ShipTo"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["TelPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["CellPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ZipCode"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Address"]);
            builder.AppendLine("</item>");
            string str = string.Empty;
            if (strArray.Length > 0)
            {
                str = strArray[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 1)
            {
                str = strArray[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 2)
            {
                str = strArray[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            int currentRegionId = 0;
            string shipperName = string.Empty;
            string cellPhone = string.Empty;
            string telPhone = string.Empty;
            string address = string.Empty;
            string zipcode = string.Empty;
            ShippersInfo info = this.ForDistorShipper(ds, order);
            if (info != null)
            {
                shipperName = info.ShipperName;
                cellPhone = info.CellPhone;
                telPhone = info.TelPhone;
                address = info.Address;
                zipcode = info.Zipcode;
                currentRegionId = info.RegionId;
            }
            else if (shipper != null)
            {
                shipperName = shipper.ShipperName;
                cellPhone = shipper.CellPhone;
                telPhone = shipper.TelPhone;
                address = shipper.Address;
                zipcode = shipper.Zipcode;
                currentRegionId = shipper.RegionId;
            }
            string[] strArray2 = new string[] { "", "", "" };
            if (currentRegionId > 0)
            {
                strArray2 = RegionHelper.GetFullRegion(currentRegionId, "-").Split(new char[] { '-' });
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", shipperName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", cellPhone);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", telPhone);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", address);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", zipcode);
            builder.AppendLine("</item>");
            string str7 = string.Empty;
            if (strArray2.Length > 0)
            {
                str7 = strArray2[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str7);
            builder.AppendLine("</item>");
            str7 = string.Empty;
            if (strArray2.Length > 1)
            {
                str7 = strArray2[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str7);
            builder.AppendLine("</item>");
            str7 = string.Empty;
            if (strArray2.Length > 2)
            {
                str7 = strArray2[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str7);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-订单号</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["OrderId"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-总金额</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["OrderTotal"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-物品总重量</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Weight"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-备注</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Remark"]);
            builder.AppendLine("</item>");
            DataRow[] rowArray = dtLine.Select(" PurchaseOrderId='" + order["PurchaseOrderId"] + "'");
            string str8 = string.Empty;
            if (rowArray.Length > 0)
            {
                foreach (DataRow row in rowArray)
                {
                    str8 = string.Concat(new object[] { str8, "货号 ", row["SKU"], " \x00d7", row["Quantity"], "\n" });
                }
                str8 = str8.Replace("；", "");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-详情</name>");
            builder.AppendFormat("<rename>{0}</rename>", str8);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-送货时间</name>");
            builder.AppendFormat("<rename></rename>", new object[0]);
            builder.AppendLine("</item>");
            SiteSettings siteSettings = SettingsManager.GetSiteSettings((int) order["DistributorId"]);
            builder.AppendLine("<item>");
            builder.AppendLine("<name>网店名称</name>");
            builder.AppendFormat("<rename>{0}</rename>", (siteSettings != null) ? siteSettings.SiteName : HiContext.Current.SiteSettings.SiteName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>自定义内容</name>");
            builder.AppendFormat("<rename>{0}</rename>", "null");
            builder.AppendLine("</item>");
            builder.AppendLine("</order>");
            return builder.ToString();
        }
    }
}


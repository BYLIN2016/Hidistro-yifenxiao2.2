namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Xml;

    public class PrintPurchase : Page
    {
        protected HtmlGenericControl divContent;
        protected HtmlHead Head1;
        protected string height = "";
        protected string mailNo = "";
        protected string orderIds = "";
        protected string templateName = "";
        protected string width = "";

        private decimal CalculateOrderTotal(DataRow order, DataSet ds)
        {
            decimal result = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            decimal.TryParse(order["AdjustedFreight"].ToString(), out result);
            decimal.TryParse(order["AdjustedDiscount"].ToString(), out num3);
            DataRow[] orderGift = ds.Tables[2].Select("PurchaseOrderId='" + order["PurchaseOrderId"] + "'");
            DataRow[] orderLine = ds.Tables[1].Select("PurchaseOrderId='" + order["PurchaseOrderId"] + "'");
            decimal num4 = this.GetAmount(orderGift, orderLine, order) + result;
            num4 += num2;
            return (num4 + num3);
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
            return this.GetGoodsAmount(orderLine);
        }

        public decimal GetGoodsAmount(DataRow[] rows)
        {
            decimal num = 0M;
            foreach (DataRow row in rows)
            {
                num += decimal.Parse(row["ItemPurchasePrice"].ToString()) * int.Parse(row["Quantity"].ToString());
            }
            return num;
        }

        private DataSet GetPrintData(string orderIds)
        {
            orderIds = "'" + orderIds.Replace(",", "','") + "'";
            return SalesHelper.GetPurchaseOrdersAndLines(orderIds);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.mailNo = base.Request["mailNo"];
                int shipperId = int.Parse(base.Request["shipperId"]);
                this.orderIds = base.Request["purchaseOrderIds"].Trim(new char[] { ',' });
                string path = HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", base.Request["template"]));
                if (File.Exists(path))
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(path);
                    XmlNode node = document.DocumentElement.SelectSingleNode("//printer");
                    this.templateName = node.SelectSingleNode("kind").InnerText;
                    string innerText = node.SelectSingleNode("pic").InnerText;
                    string str3 = node.SelectSingleNode("size").InnerText;
                    this.width = str3.Split(new char[] { ':' })[0];
                    this.height = str3.Split(new char[] { ':' })[1];
                    DataSet printData = this.GetPrintData(this.orderIds);
                    int num2 = 0;
                    foreach (DataRow row in printData.Tables[0].Rows)
                    {
                        HtmlGenericControl child = new HtmlGenericControl("div");
                        if (!string.IsNullOrEmpty(innerText) && (innerText != "noimage"))
                        {
                            using (Image image = Image.FromFile(HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", innerText))))
                            {
                                child.Attributes["style"] = string.Format("background-image: url(../../Storage/master/flex/{0}); width: {1}px; height: {2}px;text-align: center; position: relative;", innerText, image.Width, image.Height);
                            }
                        }
                        DataTable table = printData.Tables[1];
                        ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
                        string[] strArray = row["shippingRegion"].ToString().Split(new char[] { '，' });
                        foreach (XmlNode node2 in node.SelectNodes("item"))
                        {
                            StringBuilder builder = new StringBuilder(node2.SelectSingleNode("name").InnerText);
                            builder.Replace("收货人-姓名", row["ShipTo"].ToString());
                            builder.Replace("收货人-电话", row["TelPhone"].ToString());
                            builder.Replace("收货人-手机", row["CellPhone"].ToString());
                            builder.Replace("收货人-邮编", row["ZipCode"].ToString());
                            builder.Replace("收货人-地址", row["Address"].ToString());
                            string newValue = string.Empty;
                            if (strArray.Length > 0)
                            {
                                newValue = strArray[0];
                            }
                            builder.Replace("收货人-地区1级", newValue);
                            newValue = string.Empty;
                            if (strArray.Length > 1)
                            {
                                newValue = strArray[1];
                            }
                            builder.Replace("收货人-地区2级", newValue);
                            newValue = string.Empty;
                            if (strArray.Length > 2)
                            {
                                newValue = strArray[2];
                            }
                            builder.Replace("收货人-地区3级", newValue);
                            string[] strArray2 = new string[] { "", "", "" };
                            if (shipper != null)
                            {
                                strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[] { '-' });
                            }
                            builder.Replace("发货人-姓名", (shipper != null) ? shipper.ShipperName : "");
                            builder.Replace("发货人-手机", (shipper != null) ? shipper.CellPhone : "");
                            builder.Replace("发货人-电话", (shipper != null) ? shipper.TelPhone : "");
                            builder.Replace("发货人-地址", (shipper != null) ? shipper.Address : "");
                            builder.Replace("发货人-邮编", (shipper != null) ? shipper.Zipcode : "");
                            string str6 = string.Empty;
                            if (strArray2.Length > 0)
                            {
                                str6 = strArray2[0];
                            }
                            builder.Replace("发货人-地区1级", str6);
                            str6 = string.Empty;
                            if (strArray2.Length > 1)
                            {
                                str6 = strArray2[1];
                            }
                            builder.Replace("发货人-地区2级", str6);
                            str6 = string.Empty;
                            if (strArray2.Length > 2)
                            {
                                str6 = strArray2[2];
                            }
                            builder.Replace("发货人-地区3级", str6);
                            builder.Replace("订单-订单号", "订单号：" + row["PurchaseOrderId"].ToString());
                            builder.Replace("订单-总金额", this.CalculateOrderTotal(row, printData).ToString());
                            builder.Replace("订单-物品总重量", row["Weight"].ToString());
                            builder.Replace("订单-备注", row["Remark"].ToString());
                            DataRow[] rowArray = table.Select(" PurchaseOrderId='" + row["PurchaseOrderId"] + "'");
                            string str7 = string.Empty;
                            if (rowArray.Length > 0)
                            {
                                foreach (DataRow row2 in rowArray)
                                {
                                    str7 = string.Concat(new object[] { str7, "货号 ", row2["SKU"], " \x00d7", row2["Quantity"], "\n" });
                                }
                                str7 = str7.Replace("；", "");
                            }
                            builder.Replace("订单-详情", str7);
                            builder.Replace("订单-送货时间", "");
                            builder.Replace("网店名称", HiContext.Current.SiteSettings.SiteName);
                            builder.Replace("自定义内容", "");
                            string str4 = builder.ToString();
                            string str8 = node2.SelectSingleNode("font").InnerText;
                            string text1 = node2.SelectSingleNode("fontsize").InnerText;
                            string str9 = node2.SelectSingleNode("position").InnerText;
                            string str10 = node2.SelectSingleNode("align").InnerText;
                            string str11 = str9.Split(new char[] { ':' })[0];
                            string str12 = str9.Split(new char[] { ':' })[1];
                            string str13 = str9.Split(new char[] { ':' })[2];
                            string str14 = str9.Split(new char[] { ':' })[3];
                            HtmlGenericControl control2 = new HtmlGenericControl("div");
                            control2.Visible = true;
                            control2.InnerText = str4.Split(new char[] { '_' })[0];
                            control2.Style["font-family"] = str8;
                            control2.Style["font-size"] = "16px";
                            control2.Style["width"] = str11 + "px";
                            control2.Style["height"] = str12 + "px";
                            control2.Style["text-align"] = str10;
                            control2.Style["position"] = "absolute";
                            control2.Style["left"] = str13 + "px";
                            control2.Style["top"] = str14 + "px";
                            control2.Style["padding"] = "0";
                            control2.Style["margin-left"] = "0px";
                            control2.Style["margin-top"] = "0px";
                            child.Controls.Add(control2);
                        }
                        this.divContent.Controls.Add(child);
                        num2++;
                        if (num2 < printData.Tables[0].Rows.Count)
                        {
                            HtmlGenericControl control3 = new HtmlGenericControl("div");
                            control3.Attributes["class"] = "PageNext";
                            this.divContent.Controls.Add(control3);
                        }
                    }
                }
            }
        }
    }
}


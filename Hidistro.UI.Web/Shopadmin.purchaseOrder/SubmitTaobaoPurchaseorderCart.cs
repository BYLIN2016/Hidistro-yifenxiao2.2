namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Commodities;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class SubmitTaobaoPurchaseorderCart : DistributorPage
    {
        protected HtmlForm aspnetForm;
        protected Button btnMatch;
        protected Button btnSubmit;
        protected HtmlHead Head1;
        protected HeadContainer HeadContainer1;
        protected ShippingModeRadioButtonList radioShippingMode;
        protected Repeater rpTaobaoOrder;
        protected RegionSelector rsddlRegion;
        protected Script Script1;
        protected Script Script2;
        protected Script Script3;
        protected Hidistro.UI.Common.Controls.Style Style1;
        protected Hidistro.UI.Common.Controls.Style Style2;
        protected Hidistro.UI.Common.Controls.Style Style3;
        protected Hidistro.UI.Common.Controls.Style Style4;
        private IList<tbOrder> tbOrders;
        protected TextBox txtAddress;
        protected HtmlGenericControl txtAddressTip;
        protected TextBox txtMobile;
        protected HtmlGenericControl txtMobileTip;
        protected TextBox txtShipTo;
        protected HtmlGenericControl txtShipToTip;
        protected TextBox txtTel;
        protected HtmlGenericControl txtTelTip;
        protected TextBox txtZipcode;
        protected HtmlGenericControl txtZipcodeTip;

        protected void btnMatch_Click(object sender, EventArgs e)
        {
            if (base.Request.Form["radioSerachResult"] != null)
            {
                DataTable puchaseProduct = SubSiteProducthelper.GetPuchaseProduct(base.Request.Form["radioSerachResult"].Trim());
                if ((puchaseProduct != null) && (puchaseProduct.Rows.Count > 0))
                {
                    string str2 = base.Request.Form["serachProductId"].Trim();
                    string str3 = str2.Substring(0, str2.IndexOf('_'));
                    foreach (tbOrder order in this.tbOrders)
                    {
                        if (order.orderId == str3)
                        {
                            foreach (tbOrderItem item in order.items)
                            {
                                if (item.id == str2)
                                {
                                    double num;
                                    int num2;
                                    double.TryParse(item.localPrice, out num);
                                    int.TryParse(item.number, out num2);
                                    order.orderCost -= num * num2;
                                    item.localSkuId = puchaseProduct.Rows[0]["SkuId"].ToString();
                                    item.localSKU = puchaseProduct.Rows[0]["SKU"].ToString();
                                    item.localProductId = puchaseProduct.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = puchaseProduct.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = puchaseProduct.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = puchaseProduct.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = puchaseProduct.Rows[0]["Stock"].ToString();
                                    double.TryParse(item.localPrice, out num);
                                    order.orderCost += num * num2;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                this.pageDataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.ValidateCreateOrder())
            {
                string str2 = "";
                PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
                Distributor user = Users.GetUser(HiContext.Current.User.UserId) as Distributor;
                string str3 = this.GeneratePurchaseOrderId();
                purchaseOrderInfo.PurchaseOrderId = str3;
                decimal totalWeight = 0M;
                for (int i = 0; i < this.rpTaobaoOrder.Items.Count; i++)
                {
                    CheckBox box = (CheckBox) this.rpTaobaoOrder.Items[i].FindControl("chkTbOrder");
                    if (box.Checked)
                    {
                        str2 = str2 + this.tbOrders[i].orderId + ",";
                        Repeater repeater = (Repeater) this.rpTaobaoOrder.Items[i].FindControl("reOrderItems");
                        IList<tbOrderItem> items = this.tbOrders[i].items;
                        for (int j = 0; j < repeater.Items.Count; j++)
                        {
                            if (items[j].localSkuId.Trim() == "")
                            {
                                string msg = string.Format("在授权给分销商的商品中没有找到淘宝商品：{0}！请重新查找", items[j].title);
                                this.ShowMsg(msg, false);
                                return;
                            }
                            string localSkuId = items[j].localSkuId;
                            TextBox box2 = (TextBox) repeater.Items[j].FindControl("productNumber");
                            int num4 = Convert.ToInt32(box2.Text);
                            bool flag = false;
                            foreach (PurchaseOrderItemInfo info2 in purchaseOrderInfo.PurchaseOrderItems)
                            {
                                if (info2.SKU == localSkuId)
                                {
                                    flag = true;
                                    info2.Quantity += num4;
                                    totalWeight += info2.ItemWeight * num4;
                                }
                            }
                            if (!flag)
                            {
                                DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(localSkuId);
                                PurchaseOrderItemInfo item = new PurchaseOrderItemInfo();
                                if (num4 > ((int) skuContentBySku.Rows[0]["Stock"]))
                                {
                                    this.ShowMsg("商品库存不够", false);
                                    return;
                                }
                                foreach (DataRow row in skuContentBySku.Rows)
                                {
                                    if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                                    {
                                        object sKUContent = item.SKUContent;
                                        item.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                                    }
                                }
                                item.PurchaseOrderId = str3;
                                item.SkuId = localSkuId;
                                item.ProductId = (int) skuContentBySku.Rows[0]["ProductId"];
                                if (skuContentBySku.Rows[0]["SKU"] != DBNull.Value)
                                {
                                    item.SKU = (string) skuContentBySku.Rows[0]["SKU"];
                                }
                                if (skuContentBySku.Rows[0]["Weight"] != DBNull.Value)
                                {
                                    item.ItemWeight = (decimal) skuContentBySku.Rows[0]["Weight"];
                                }
                                item.ItemPurchasePrice = (decimal) skuContentBySku.Rows[0]["PurchasePrice"];
                                item.Quantity = num4;
                                item.ItemListPrice = (decimal) skuContentBySku.Rows[0]["SalePrice"];
                                if (skuContentBySku.Rows[0]["CostPrice"] != DBNull.Value)
                                {
                                    item.ItemCostPrice = (decimal) skuContentBySku.Rows[0]["CostPrice"];
                                }
                                item.ItemDescription = (string) skuContentBySku.Rows[0]["ProductName"];
                                item.ItemHomeSiteDescription = (string) skuContentBySku.Rows[0]["ProductName"];
                                if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
                                {
                                    item.ThumbnailsUrl = (string) skuContentBySku.Rows[0]["ThumbnailUrl40"];
                                }
                                totalWeight += item.ItemWeight * num4;
                                purchaseOrderInfo.PurchaseOrderItems.Add(item);
                            }
                        }
                    }
                }
                if (str2 == "")
                {
                    this.ShowMsg("至少选择一个淘宝订单！！", false);
                }
                else
                {
                    ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
                    purchaseOrderInfo.ShipTo = this.txtShipTo.Text.Trim();
                    if (this.rsddlRegion.GetSelectedRegionId().HasValue)
                    {
                        purchaseOrderInfo.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                    }
                    purchaseOrderInfo.Address = this.txtAddress.Text.Trim();
                    purchaseOrderInfo.TelPhone = this.txtTel.Text.Trim();
                    purchaseOrderInfo.ZipCode = this.txtZipcode.Text.Trim();
                    purchaseOrderInfo.CellPhone = this.txtMobile.Text.Trim();
                    purchaseOrderInfo.OrderId = null;
                    purchaseOrderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
                    purchaseOrderInfo.RealModeName = shippingMode.Name;
                    purchaseOrderInfo.ShippingModeId = this.radioShippingMode.SelectedValue.Value;
                    purchaseOrderInfo.ModeName = shippingMode.Name;
                    purchaseOrderInfo.AdjustedFreight = SubsiteSalesHelper.CalcFreight(purchaseOrderInfo.RegionId, totalWeight, shippingMode);
                    purchaseOrderInfo.Freight = purchaseOrderInfo.AdjustedFreight;
                    purchaseOrderInfo.ShippingRegion = this.rsddlRegion.SelectedRegions;
                    purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
                    purchaseOrderInfo.DistributorId = user.UserId;
                    purchaseOrderInfo.Distributorname = user.Username;
                    purchaseOrderInfo.DistributorEmail = user.Email;
                    purchaseOrderInfo.DistributorRealName = user.RealName;
                    purchaseOrderInfo.DistributorQQ = user.QQ;
                    purchaseOrderInfo.DistributorWangwang = user.Wangwang;
                    purchaseOrderInfo.DistributorMSN = user.MSN;
                    purchaseOrderInfo.RefundStatus = RefundStatus.None;
                    purchaseOrderInfo.Weight = totalWeight;
                    purchaseOrderInfo.Remark = null;
                    purchaseOrderInfo.TaobaoOrderId = str2;
                    if (purchaseOrderInfo.PurchaseOrderItems.Count == 0)
                    {
                        this.ShowMsg("您暂时未选择您要添加的商品", false);
                    }
                    else if (SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
                    {
                        SubsiteSalesHelper.ClearPurchaseShoppingCart();
                        this.ResponseCookies();
                        base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ChoosePayment.aspx?PurchaseOrderId=" + purchaseOrderInfo.PurchaseOrderId);
                    }
                    else
                    {
                        this.ShowMsg("提交采购单失败", false);
                    }
                }
            }
        }

        protected void ButtonAdd_Command(object sender, CommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            string[] strArray = str.Split(new char[] { '_' });
            foreach (tbOrder order in this.tbOrders)
            {
                if (order.orderId == strArray[0])
                {
                    foreach (tbOrderItem item in order.items)
                    {
                        if (!(item.id == str))
                        {
                            continue;
                        }
                        if (item.number != "")
                        {
                            if (Convert.ToInt32(item.number) >= Convert.ToInt32(item.localStock))
                            {
                                this.ShowMsg("库存不足，请检查后再下单", false);
                            }
                            else
                            {
                                item.number = (Convert.ToInt32(item.number) + 1).ToString();
                            }
                        }
                        else
                        {
                            item.number = "1";
                        }
                        if ((item.localSkuId != "") && (item.localPrice != ""))
                        {
                            order.orderCost += Convert.ToDouble(item.localPrice);
                        }
                        break;
                    }
                    break;
                }
            }
            this.pageDataBind();
        }

        protected void ButtonDec_Command(object sender, CommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            string[] strArray = str.Split(new char[] { '_' });
            foreach (tbOrder order in this.tbOrders)
            {
                if (order.orderId == strArray[0])
                {
                    foreach (tbOrderItem item in order.items)
                    {
                        if (item.id == str)
                        {
                            if ((item.number != "") && (Convert.ToInt32(item.number) > 1))
                            {
                                item.number = (Convert.ToInt32(item.number) - 1).ToString();
                                if ((item.localSkuId != "") && (item.localPrice != ""))
                                {
                                    order.orderCost -= Convert.ToDouble(item.localPrice);
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            this.pageDataBind();
        }

        protected void ButtonDelete_Command(object sender, CommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            string[] strArray = str.Split(new char[] { '_' });
            foreach (tbOrder order in this.tbOrders)
            {
                if (order.orderId == strArray[0])
                {
                    if (order.items.Count <= 1)
                    {
                        this.ShowMsg("每个淘宝订单至少保留一件商品！", false);
                        return;
                    }
                    foreach (tbOrderItem item in order.items)
                    {
                        if (item.id == str)
                        {
                            order.items.Remove(item);
                            if ((item.localSkuId != "") && (item.localPrice != ""))
                            {
                                order.orderCost -= Convert.ToDouble(item.localPrice);
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            this.pageDataBind();
        }

        private string GeneratePurchaseOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return ("MPO" + DateTime.Now.ToString("yyyyMMdd") + str);
        }

        protected string isFindProduct(string productId, int type)
        {
            if (productId.Trim() == "")
            {
                if (type != 0)
                {
                    return "block";
                }
                return "none";
            }
            if (type != 1)
            {
                return "block";
            }
            return "none";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.Compare(base.Request.RequestType, "post", true) != 0)
            {
                this.btnSubmit.Enabled = false;
            }
            else if (base.IsPostBack)
            {
                this.tbOrders = (IList<tbOrder>) this.Session["tbOrders"];
            }
            else
            {
                XmlDocument document = new XmlDocument();
                if (string.IsNullOrEmpty(base.Request.Form["data"]))
                {
                    this.ShowMsg("数据丢失，请关闭此页重新操作", false);
                }
                else
                {
                    document.LoadXml(base.Request.Form["data"]);
                    this.tbOrders = new List<tbOrder>();
                    XmlNodeList list = document.FirstChild.SelectNodes("order");
                    string innerText = null;
                    string str2 = null;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string str3;
                        tbOrder order = new tbOrder();
                        XmlNode node = list.Item(i);
                        if (innerText == null)
                        {
                            innerText = node.SelectSingleNode("ship_addr").InnerText;
                            str2 = node.SelectSingleNode("ship_name").InnerText;
                        }
                        else
                        {
                            str3 = node.SelectSingleNode("ship_addr").InnerText;
                            string str4 = node.SelectSingleNode("ship_name").InnerText;
                            if ((innerText != str3) || (str2 != str4))
                            {
                                this.ShowMsg("收货人地址/姓名不一致不能合并下单！", false);
                                break;
                            }
                            str2 = str4;
                            innerText = str3;
                        }
                        string[] strArray = innerText.Split(new char[] { ' ' });
                        this.txtShipTo.Text = str2;
                        if (strArray.Length >= 4)
                        {
                            str3 = strArray[0] + "," + strArray[1] + "," + strArray[2];
                            this.rsddlRegion.SelectedRegions = str3;
                            this.txtAddress.Text = strArray[3];
                        }
                        this.txtZipcode.Text = node.SelectSingleNode("ship_zipcode").InnerText;
                        this.txtTel.Text = node.SelectSingleNode("ship_tel").InnerText;
                        this.txtMobile.Text = node.SelectSingleNode("ship_mobile").InnerText;
                        this.radioShippingMode.DataBind();
                        if (this.radioShippingMode.Items.Count > 0)
                        {
                            this.radioShippingMode.Items[0].Selected = true;
                        }
                        order.orderId = node.SelectSingleNode("order_id").InnerText;
                        order.buyer = node.SelectSingleNode("buyer").InnerText;
                        order.createTime = node.SelectSingleNode("createtime").InnerText;
                        order.orderMemo = node.SelectSingleNode("order_memo").InnerText;
                        XmlNode node2 = node.SelectSingleNode("items");
                        double num2 = 0.0;
                        for (int j = 0; j < node2.ChildNodes.Count; j++)
                        {
                            tbOrderItem item = new tbOrderItem();
                            item.id = string.Format("{0}_{1}", order.orderId, j);
                            item.title = node2.ChildNodes[j].SelectSingleNode("title").InnerText;
                            item.spec = node2.ChildNodes[j].SelectSingleNode("spec").InnerText;
                            item.price = node2.ChildNodes[j].SelectSingleNode("price").InnerText;
                            item.number = node2.ChildNodes[j].SelectSingleNode("number").InnerText;
                            if (string.IsNullOrEmpty(item.number))
                            {
                                item.number = "1";
                            }
                            item.url = node2.ChildNodes[j].SelectSingleNode("url").InnerText;
                            HttpRequest request = HttpContext.Current.Request;
                            if (request.Cookies[Globals.UrlEncode(item.title + item.spec)] != null)
                            {
                                ProductQuery query = new ProductQuery();
                                query.PageSize = 1;
                                query.PageIndex = 1;
                                query.ProductCode = Globals.UrlDecode(request.Cookies[Globals.UrlEncode(item.title + item.spec)].Value);
                                int count = 0;
                                DataTable puchaseProducts = SubSiteProducthelper.GetPuchaseProducts(query, out count);
                                if (puchaseProducts.Rows.Count > 0)
                                {
                                    item.localSkuId = puchaseProducts.Rows[0]["SkuId"].ToString();
                                    item.localSKU = puchaseProducts.Rows[0]["SKU"].ToString();
                                    item.localProductId = puchaseProducts.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = puchaseProducts.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = puchaseProducts.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = puchaseProducts.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = puchaseProducts.Rows[0]["Stock"].ToString();
                                    num2 += Convert.ToDouble(puchaseProducts.Rows[0]["PurchasePrice"]) * Convert.ToInt32(item.number);
                                }
                            }
                            else
                            {
                                ProductQuery query2 = new ProductQuery();
                                query2.PageSize = 1;
                                query2.PageIndex = 1;
                                query2.Keywords = item.title;
                                int num5 = 0;
                                DataTable table2 = SubSiteProducthelper.GetPuchaseProducts(query2, out num5);
                                if (num5 == 1)
                                {
                                    item.localSkuId = table2.Rows[0]["SkuId"].ToString();
                                    item.localSKU = table2.Rows[0]["SKU"].ToString();
                                    item.localProductId = table2.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = table2.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = table2.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = table2.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = table2.Rows[0]["Stock"].ToString();
                                    num2 += Convert.ToDouble(table2.Rows[0]["PurchasePrice"]) * Convert.ToInt32(item.number);
                                }
                            }
                            order.items.Add(item);
                            order.orderCost = num2;
                        }
                        this.tbOrders.Add(order);
                    }
                    this.Session["tbOrders"] = this.tbOrders;
                    this.pageDataBind();
                }
            }
        }

        private void pageDataBind()
        {
            this.rpTaobaoOrder.DataSource = this.tbOrders;
            this.rpTaobaoOrder.DataBind();
        }

        public static void RemoveCookie(string cookieName, string key)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                    {
                        cookie.Values.Remove(key);
                    }
                    else
                    {
                        response.Cookies.Remove(cookieName);
                    }
                }
            }
        }

        private void ResponseCookies()
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;
            for (int i = 0; i < this.rpTaobaoOrder.Items.Count; i++)
            {
                Repeater repeater = (Repeater) this.rpTaobaoOrder.Items[i].FindControl("reOrderItems");
                for (int j = 0; j < repeater.Items.Count; j++)
                {
                    CheckBox box = (CheckBox) repeater.Items[j].FindControl("chkSave");
                    if (box.Checked)
                    {
                        Label label = (Label) repeater.Items[j].FindControl("lblSKU");
                        Label label2 = (Label) repeater.Items[j].FindControl("lblTitle");
                        Label label3 = (Label) repeater.Items[j].FindControl("lblSpec");
                        if (request.Cookies[Globals.UrlEncode(label2.Text + label3.Text)] != null)
                        {
                            response.Cookies.Remove(Globals.UrlEncode(label2.Text + label3.Text));
                        }
                        HttpCookie cookie = new HttpCookie(Globals.UrlEncode(label2.Text + label3.Text));
                        cookie.Value = Globals.UrlEncode(label.Text);
                        cookie.Path = "/";
                        cookie.Expires = DateTime.Now.AddDays(3650.0);
                        response.Cookies.Add(cookie);
                    }
                }
            }
        }

        protected void rpTaobaoOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                IList<tbOrder> dataSource = (IList<tbOrder>) this.rpTaobaoOrder.DataSource;
                Repeater repeater = (Repeater) e.Item.FindControl("reOrderItems");
                repeater.DataSource = dataSource[e.Item.ItemIndex].items;
                repeater.DataBind();
            }
        }

        private bool ValidateCreateOrder()
        {
            string str = string.Empty;
            if (!this.rsddlRegion.GetSelectedRegionId().HasValue || (this.rsddlRegion.GetSelectedRegionId().Value == 0))
            {
                str = str + Formatter.FormatErrorMessage("请选择收货地址");
            }
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if (string.IsNullOrEmpty(this.txtShipTo.Text) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
            {
                str = str + Formatter.FormatErrorMessage("请输入正确的收货人姓名");
            }
            if (!string.IsNullOrEmpty(this.txtShipTo.Text) && ((this.txtShipTo.Text.Trim().Length < 2) || (this.txtShipTo.Text.Trim().Length > 20)))
            {
                str = str + Formatter.FormatErrorMessage("收货人姓名的长度限制在2-20个字符");
            }
            if (string.IsNullOrEmpty(this.txtAddress.Text.Trim()) || (this.txtAddress.Text.Trim().Length > 100))
            {
                str = str + Formatter.FormatErrorMessage("请输入收货人详细地址,在100个字符以内");
            }
            regex = new Regex("^[0-9]*$");
            if ((string.IsNullOrEmpty(this.txtZipcode.Text.Trim()) || (this.txtZipcode.Text.Trim().Length > 10)) || ((this.txtZipcode.Text.Trim().Length < 6) || !regex.IsMatch(this.txtZipcode.Text.Trim())))
            {
                str = str + Formatter.FormatErrorMessage("请输入收货人邮政编码,在6-10个数字之间");
            }
            regex = new Regex("^[0-9]*$");
            if (!string.IsNullOrEmpty(this.txtMobile.Text.Trim()) && ((!regex.IsMatch(this.txtMobile.Text.Trim()) || (this.txtMobile.Text.Trim().Length > 20)) || (this.txtMobile.Text.Trim().Length < 3)))
            {
                str = str + Formatter.FormatErrorMessage("手机号码长度限制在3-20个字符之间,只能输入数字");
            }
            regex = new Regex("^[0-9-]*$");
            if (!string.IsNullOrEmpty(this.txtTel.Text.Trim()) && ((!regex.IsMatch(this.txtTel.Text.Trim()) || (this.txtTel.Text.Trim().Length > 20)) || (this.txtTel.Text.Trim().Length < 3)))
            {
                str = str + Formatter.FormatErrorMessage("电话号码长度限制在3-20个字符之间,只能输入数字和字符“-”");
            }
            if (!this.radioShippingMode.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择配送方式");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }

        private class tbOrder
        {
            
            private string _buyer;
            
            private string _createTime;
            
            private double _orderCost;
            
            private string _orderId;
            
            private string _orderMemo;
            public IList<SubmitTaobaoPurchaseorderCart.tbOrderItem> items;

            public tbOrder()
            {
                this.orderId = "";
                this.buyer = "";
                this.createTime = "";
                this.orderMemo = "";
                this.orderCost = 0.0;
                this.items = new List<SubmitTaobaoPurchaseorderCart.tbOrderItem>();
            }

            public string buyer
            {
                
                get
                {
                    return _buyer;
                }
                
                set
                {
                    _buyer = value;
                }
            }

            public string createTime
            {
                
                get
                {
                    return _createTime;
                }
                
                set
                {
                    _createTime = value;
                }
            }

            public double orderCost
            {
                
                get
                {
                    return _orderCost;
                }
                
                set
                {
                    _orderCost = value;
                }
            }

            public string orderId
            {
                
                get
                {
                    return _orderId;
                }
                
                set
                {
                    _orderId = value;
                }
            }

            public string orderMemo
            {
                
                get
                {
                    return _orderMemo;
                }
                
                set
                {
                    _orderMemo = value;
                }
            }
        }

        private class tbOrderItem
        {
            
            private string _bn;
            
            private string _id;
            
            private string _localPrice;
            
            private string _localProductId;
            
            private string _localProductName;
            
            private string _localSKU;
            
            private string _localSkuId;
            
            private string _localStock;
            
            private string _memo;
            
            private string _number;
            
            private string _price;
            
            private string _spec;
            
            private string _thumbnailUrl100;
            
            private string _title;
            
            private string _url;

            public tbOrderItem()
            {
                this.id = "";
                this.title = "";
                this.spec = "";
                this.price = "";
                this.number = "0";
                this.bn = "";
                this.memo = "";
                this.url = "";
                this.localSkuId = "";
                this.localSKU = "";
                this.localStock = "0";
                this.localPrice = "0";
                this.localProductId = "";
                this.localProductName = "";
                this.thumbnailUrl100 = "";
            }

            public string bn
            {
                
                get
                {
                    return _bn;
                }
                
                set
                {
                    _bn = value;
                }
            }

            public string id
            {
                
                get
                {
                    return _id;
                }
                
                set
                {
                    _id = value;
                }
            }

            public string localPrice
            {
                
                get
                {
                    return _localPrice;
                }
                
                set
                {
                    _localPrice = value;
                }
            }

            public string localProductId
            {
                
                get
                {
                    return _localProductId;
                }
                
                set
                {
                    _localProductId = value;
                }
            }

            public string localProductName
            {
                
                get
                {
                    return _localProductName;
                }
                
                set
                {
                    _localProductName = value;
                }
            }

            public string localSKU
            {
                
                get
                {
                    return _localSKU;
                }
                
                set
                {
                    _localSKU = value;
                }
            }

            public string localSkuId
            {
                
                get
                {
                    return _localSkuId;
                }
                
                set
                {
                    _localSkuId = value;
                }
            }

            public string localStock
            {
                
                get
                {
                    return _localStock;
                }
                
                set
                {
                    _localStock = value;
                }
            }

            public string memo
            {
                
                get
                {
                    return _memo;
                }
                
                set
                {
                    _memo = value;
                }
            }

            public string number
            {
                
                get
                {
                    return _number;
                }
                
                set
                {
                    _number = value;
                }
            }

            public string price
            {
                
                get
                {
                    return _price;
                }
                
                set
                {
                    _price = value;
                }
            }

            public string spec
            {
                
                get
                {
                    return _spec;
                }
                
                set
                {
                    _spec = value;
                }
            }

            public string thumbnailUrl100
            {
                
                get
                {
                    return _thumbnailUrl100;
                }
                
                set
                {
                    _thumbnailUrl100 = value;
                }
            }

            public string title
            {
                
                get
                {
                    return _title;
                }
                
                set
                {
                    _title = value;
                }
            }

            public string url
            {
                
                get
                {
                    return _url;
                }
                
                set
                {
                    _url = value;
                }
            }
        }
    }
}


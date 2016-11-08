namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ManagePurchaseorder)]
    public class ManagePurchaseOrder : AdminPage
    {
        protected Button btnAcceptRefund;
        protected Button btnAcceptReplace;
        protected Button btnAcceptReturn;
        protected Button btnCloseOrder;
        protected Button btnClosePurchaseOrder;
        protected Button btnEditOrder;
        protected Button btnOrderGoods;
        protected Button btnProductGoods;
        protected Button btnRefuseRefund;
        protected Button btnRefuseReplace;
        protected Button btnRefuseReturn;
        protected Button btnRemark;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected ClosePurchaseOrderReasonDropDownList ddlCloseReason;
        protected DropDownList ddlIsPrinted;
        protected DataList dlstPurchaseOrders;
        protected HtmlInputHidden hidAdminRemark;
        protected HtmlInputHidden hidOrderTotal;
        protected HtmlInputHidden hidPurchaseOrderId;
        protected HtmlInputHidden hidRefundMoney;
        protected HtmlInputHidden hidRefundType;
        protected HyperLink hlinkAllOrder;
        protected HyperLink hlinkClose;
        protected HyperLink hlinkHistory;
        protected HyperLink hlinkNotPay;
        protected HyperLink hlinkSendGoods;
        protected HyperLink hlinkTradeFinished;
        protected HyperLink hlinkYetPay;
        protected PageSize hrefPageSize;
        protected Label lblAddress;
        protected Label lblContacts;
        protected Label lblEmail;
        protected HtmlGenericControl lblpurchaseDateForRemark;
        protected Label lblPurchaseOrderAmount;
        protected Label lblPurchaseOrderAmount1;
        protected Label lblPurchaseOrderAmount2;
        protected Label lblPurchaseOrderAmount3;
        protected HtmlInputHidden lblPurchaseOrderId;
        protected Label lblPurchaseOrderTotal;
        protected FormatedMoneyLabel lblpurchaseTotalForRemark;
        protected Label lblRefundRemark;
        protected Label lblRefundType;
        protected Label lblStatus;
        protected Label lblTelephone;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected Pager pager;
        protected Pager pager1;
        protected Label refund_lblPurchaseOrderId;
        protected Label replace_lblAddress;
        protected Label replace_lblComments;
        protected Label replace_lblContacts;
        protected Label replace_lblEmail;
        protected Label replace_lblOrderId;
        protected Label replace_lblOrderTotal;
        protected Label replace_lblPostCode;
        protected Label replace_lblTelephone;
        protected TextBox replace_txtAdminRemark;
        protected Label return_lblAddress;
        protected Label return_lblContacts;
        protected Label return_lblEmail;
        protected Label return_lblPurchaseOrderId;
        protected Label return_lblPurchaseOrderTotal;
        protected Label return_lblRefundType;
        protected Label return_lblReturnRemark;
        protected Label return_lblTelephone;
        protected TextBox return_txtAdminRemark;
        protected TextBox return_txtRefundMoney;
        protected ShippingModeDropDownList shippingModeDropDownList;
        protected HtmlGenericControl spanOrderId;
        protected HtmlGenericControl spanpurcharseOrderId;
        protected TextBox txtAdminRemark;
        protected TextBox txtDistributorName;
        protected TextBox txtOrderId;
        protected TextBox txtProductName;
        protected TextBox txtPurchaseOrderDiscount;
        protected TextBox txtPurchaseOrderId;
        protected TextBox txtRemark;
        protected TextBox txtShopTo;

        private void BindPurchaseOrders()
        {
            PurchaseOrderQuery purchaseOrderQuery = this.GetPurchaseOrderQuery();
            purchaseOrderQuery.SortBy = "PurchaseDate";
            purchaseOrderQuery.SortOrder = SortAction.Desc;
            DbQueryResult purchaseOrders = SalesHelper.GetPurchaseOrders(purchaseOrderQuery);
            this.dlstPurchaseOrders.DataSource = purchaseOrders.Data;
            this.dlstPurchaseOrders.DataBind();
            this.pager.TotalRecords = purchaseOrders.TotalRecords;
            this.pager1.TotalRecords = purchaseOrders.TotalRecords;
            this.txtOrderId.Text = purchaseOrderQuery.OrderId;
            this.txtProductName.Text = purchaseOrderQuery.ProductName;
            this.txtDistributorName.Text = purchaseOrderQuery.DistributorName;
            this.txtPurchaseOrderId.Text = purchaseOrderQuery.PurchaseOrderId;
            this.txtShopTo.Text = purchaseOrderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = purchaseOrderQuery.StartDate;
            this.calendarEndDate.SelectedDate = purchaseOrderQuery.EndDate;
            this.lblStatus.Text = ((int) purchaseOrderQuery.PurchaseStatus).ToString();
            this.shippingModeDropDownList.SelectedValue = purchaseOrderQuery.ShippingModeId;
            if (purchaseOrderQuery.IsPrinted.HasValue)
            {
                this.ddlIsPrinted.SelectedValue = purchaseOrderQuery.IsPrinted.Value.ToString();
            }
        }

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseRefund(SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value), HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
            this.BindPurchaseOrders();
            this.ShowMsg("成功的确认了采购单退款", true);
        }

        private void btnAcceptReplace_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, true);
            this.BindPurchaseOrders();
            this.ShowMsg("成功的确认了采购单换货", true);
        }

        private void btnAcceptReturn_Click(object sender, EventArgs e)
        {
            decimal num;
            if (!decimal.TryParse(this.hidRefundMoney.Value, out num))
            {
                this.ShowMsg("请输入正确的退款金额", false);
            }
            else
            {
                SalesHelper.CheckPurchaseReturn(this.hidPurchaseOrderId.Value, HiContext.Current.User.Username, num, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
                this.BindPurchaseOrders();
                this.ShowMsg("成功的确认了采购单退货", true);
            }
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
            purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
            if (SalesHelper.ClosePurchaseOrder(purchaseOrder))
            {
                this.BindPurchaseOrders();
                this.ShowMsg("关闭采购单成功", true);
            }
            else
            {
                this.ShowMsg("关闭采购单失败", false);
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            decimal num;
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.lblPurchaseOrderId.Value);
            string msg = string.Empty;
            if (this.ValidateValues(out num))
            {
                purchaseOrder.AdjustedDiscount = num;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<PurchaseOrderInfo>(purchaseOrder, new string[] { "ValPurchaseOrder" });
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                        this.ShowMsg(msg, false);
                        return;
                    }
                }
                if (purchaseOrder.GetPurchaseTotal() >= 0M)
                {
                    if (SalesHelper.UpdatePurchaseOrderAmount(purchaseOrder))
                    {
                        this.BindPurchaseOrders();
                        this.ShowMsg("修改成功", true);
                    }
                    else
                    {
                        this.ShowMsg("修改失败", false);
                    }
                }
                else
                {
                    this.ShowMsg("折扣值不能使得采购单总金额为负", false);
                }
            }
        }

        private void btnOrderGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的采购单", false);
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    list.Add("'" + str2 + "'");
                }
                DataSet purchaseOrderGoods = OrderHelper.GetPurchaseOrderGoods(string.Join(",", list.ToArray()));
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>采购单编号</td>");
                if (purchaseOrderGoods.Tables[1].Rows.Count <= 0)
                {
                    builder.AppendLine("<td>商品名称</td>");
                }
                else
                {
                    builder.AppendLine("<td>商品(礼品)名称</td>");
                }
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("<td>备注</td>");
                builder.AppendLine("</tr>");
                foreach (DataRow row in purchaseOrderGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["PurchaseOrderId"] + "</td>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("<td>" + row["Remark"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                foreach (DataRow row2 in purchaseOrderGoods.Tables[1].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row2["GiftPurchaseOrderId"] + "</td>");
                    builder.AppendLine("<td>" + row2["GiftName"] + "[礼品]</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td>" + row2["Quantity"] + "</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=purchaseordergoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要打印的采购单", false);
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/ChoosePrintOrders.aspx?OrderIds=" + str));
            }
        }

        private void btnProductGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的采购单", false);
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    list.Add("'" + str2 + "'");
                }
                DataSet purchaseProductGoods = OrderHelper.GetPurchaseProductGoods(string.Join(",", list.ToArray()));
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                if (purchaseProductGoods.Tables[1].Rows.Count <= 0)
                {
                    builder.AppendLine("<td>商品名称</td>");
                }
                else
                {
                    builder.AppendLine("<td>商品(礼品)名称</td>");
                }
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("</tr>");
                foreach (DataRow row in purchaseProductGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                foreach (DataRow row2 in purchaseProductGoods.Tables[1].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td>" + row2["GiftName"] + "[礼品]</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td>" + row2["Quantity"] + "</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void btnRefuseRefund_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseRefund(SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value), HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindPurchaseOrders();
            this.ShowMsg("成功的拒绝了采购单退款", true);
        }

        private void btnRefuseReplace_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, false);
            this.BindPurchaseOrders();
            this.ShowMsg("成功的拒绝了采购单换货", true);
        }

        private void btnRefuseReturn_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseReturn(this.hidPurchaseOrderId.Value, HiContext.Current.User.Username, 0M, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindPurchaseOrders();
            this.ShowMsg("成功的拒绝了采购单退货", true);
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (!regex.IsMatch(this.txtRemark.Text))
                {
                    this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                }
                else
                {
                    PurchaseOrderInfo purchaseOrder = new PurchaseOrderInfo();
                    purchaseOrder.PurchaseOrderId = this.hidPurchaseOrderId.Value;
                    if (this.orderRemarkImageForRemark.SelectedItem != null)
                    {
                        purchaseOrder.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                    }
                    purchaseOrder.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                    if (SalesHelper.SavePurchaseOrderRemark(purchaseOrder))
                    {
                        this.BindPurchaseOrders();
                        this.ShowMsg("保存备忘录成功", true);
                    }
                    else
                    {
                        this.ShowMsg("保存失败", false);
                    }
                }
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBinderPurchaseOrders(true);
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要发货的订单", false);
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/purchaseOrder/BatchSendPurchaseOrderGoods.aspx?PurchaseOrderIds=" + str));
            }
        }

        private void dlstPurchaseOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(e.CommandArgument.ToString());
            if (purchaseOrder != null)
            {
                if ((e.CommandName == "FINISH_TRADE") && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
                {
                    if (SalesHelper.ConfirmPurchaseOrderFinish(purchaseOrder))
                    {
                        this.BindPurchaseOrders();
                        this.ShowMsg("成功的完成了该采购单", true);
                    }
                    else
                    {
                        this.ShowMsg("完成采购单失败", false);
                    }
                }
                if ((e.CommandName == "CONFIRM_PAY") && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_CONFIRM_PAY))
                {
                    if (SalesHelper.ConfirmPayPurchaseOrder(purchaseOrder))
                    {
                        PurchaseDebitNote note = new PurchaseDebitNote();
                        note.NoteId = Globals.GetGenerateId();
                        note.PurchaseOrderId = e.CommandArgument.ToString();
                        note.Operator = HiContext.Current.User.Username;
                        note.Remark = "后台" + note.Operator + "收款成功";
                        SalesHelper.SavePurchaseDebitNote(note);
                        this.BindPurchaseOrders();
                        this.ShowMsg("成功的确认了采购单收款", true);
                    }
                    else
                    {
                        this.ShowMsg("确认采购单收款失败", false);
                    }
                }
            }
        }

        private void dlstPurchaseOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Label label = (Label) e.Item.FindControl("lkbtnSendGoods");
                object obj2 = DataBinder.Eval(e.Item.DataItem, "Gateway");
                string str = "";
                if ((obj2 != null) && !(obj2 is DBNull))
                {
                    str = obj2.ToString();
                }
                ImageLinkButton button = (ImageLinkButton) e.Item.FindControl("lkbtnConfirmPurchaseOrder");
                HtmlGenericControl control = (HtmlGenericControl) e.Item.FindControl("lkbtnEditPurchaseOrder");
                Literal literal = (Literal) e.Item.FindControl("litClosePurchaseOrder");
                ImageLinkButton button2 = (ImageLinkButton) e.Item.FindControl("lkbtnPayOrder");
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Item.DataItem, "PurchaseStatus");
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("lkbtnCheckPurchaseRefund");
                HtmlAnchor anchor2 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckPurchaseReturn");
                HtmlAnchor anchor3 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckPurchaseReplace");
                switch (status)
                {
                    case OrderStatus.WaitBuyerPay:
                        literal.Visible = true;
                        control.Visible = true;
                        if (str != "hishop.plugins.payment.podrequest")
                        {
                            button2.Visible = true;
                        }
                        break;

                    case OrderStatus.ApplyForRefund:
                        anchor.Visible = true;
                        break;

                    case OrderStatus.ApplyForReturns:
                        anchor2.Visible = true;
                        break;

                    case OrderStatus.ApplyForReplacement:
                        anchor3.Visible = true;
                        break;
                }
                SalesHelper.GetPurchaseOrder(this.dlstPurchaseOrders.DataKeys[e.Item.ItemIndex].ToString());
                label.Visible = (status == OrderStatus.BuyerAlreadyPaid) || ((status == OrderStatus.WaitBuyerPay) && (str == "hishop.plugins.payment.podrequest"));
                button.Visible = status == OrderStatus.SellerAlreadySent;
            }
        }

        private PurchaseOrderQuery GetPurchaseOrderQuery()
        {
            PurchaseOrderQuery query = new PurchaseOrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
            {
                query.PurchaseOrderId = Globals.UrlDecode(this.Page.Request.QueryString["PurchaseOrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShopTo"]))
            {
                query.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShopTo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["DistributorName"]))
            {
                query.DistributorName = Globals.UrlDecode(this.Page.Request.QueryString["DistributorName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["PurchaseStatus"], out result))
                {
                    query.PurchaseStatus = (OrderStatus) result;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
            {
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out num2))
                {
                    query.IsPrinted = new int?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
            {
                int num3 = 0;
                if (int.TryParse(this.Page.Request.QueryString["ModeId"], out num3))
                {
                    query.ShippingModeId = new int?(num3);
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "PurchaseDate";
            return query;
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的采购单", false);
            }
            else
            {
                int num = SalesHelper.DeletePurchaseOrders("'" + str.Replace(",", "','") + "'");
                this.BindPurchaseOrders();
                this.ShowMsg(string.Format("成功删除了{0}个采购单", num), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstPurchaseOrders.ItemDataBound += new DataListItemEventHandler(this.dlstPurchaseOrders_ItemDataBound);
            this.dlstPurchaseOrders.ItemCommand += new DataListCommandEventHandler(this.dlstPurchaseOrders_ItemCommand);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
            this.btnClosePurchaseOrder.Click += new EventHandler(this.btnClosePurchaseOrder_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnEditOrder.Click += new EventHandler(this.btnEditOrder_Click);
            this.btnOrderGoods.Click += new EventHandler(this.btnOrderGoods_Click);
            this.btnProductGoods.Click += new EventHandler(this.btnProductGoods_Click);
            this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
            this.btnAcceptReturn.Click += new EventHandler(this.btnAcceptReturn_Click);
            this.btnRefuseReturn.Click += new EventHandler(this.btnRefuseReturn_Click);
            this.btnAcceptReplace.Click += new EventHandler(this.btnAcceptReplace_Click);
            this.btnRefuseReplace.Click += new EventHandler(this.btnRefuseReplace_Click);
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int num;
                string str;
                string str2;
                if (string.IsNullOrEmpty(base.Request["purchaseOrderId"]))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(base.Request["purchaseOrderId"]);
                StringBuilder builder = new StringBuilder();
                if (base.Request["type"] == "refund")
                {
                    SalesHelper.GetPurchaseRefundType(base.Request["purchaseOrderId"], out num, out str2);
                }
                else if (base.Request["type"] == "return")
                {
                    SalesHelper.GetPurchaseRefundTypeFromReturn(base.Request["purchaseOrderId"], out num, out str2);
                }
                else
                {
                    num = 0;
                    str2 = "";
                }
                if (num == 1)
                {
                    str = "退到预存款";
                }
                else
                {
                    str = "银行转帐";
                }
                builder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(purchaseOrder.GetPurchaseTotal()));
                if (base.Request["type"] == "replace")
                {
                    string purchaseReplaceComments = SalesHelper.GetPurchaseReplaceComments(base.Request["purchaseOrderId"]);
                    builder.AppendFormat(",\"Comments\":\"{0}\"", purchaseReplaceComments.Replace("\r\n", ""));
                }
                else
                {
                    builder.AppendFormat(",\"RefundType\":\"{0}\"", num);
                    builder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", str);
                }
                builder.AppendFormat(",\"Remark\":\"{0}\"", str2.Replace("\r\n", ""));
                builder.AppendFormat(",\"Contacts\":\"{0}\"", purchaseOrder.DistributorRealName);
                builder.AppendFormat(",\"Email\":\"{0}\"", purchaseOrder.DistributorEmail);
                builder.AppendFormat(",\"Telephone\":\"{0}\"", purchaseOrder.TelPhone);
                builder.AppendFormat(",\"Address\":\"{0}\"", purchaseOrder.Address);
                builder.AppendFormat(",\"PostCode\":\"{0}\"", purchaseOrder.ZipCode);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            if (!this.Page.IsPostBack)
            {
                this.shippingModeDropDownList.DataBind();
                this.ddlIsPrinted.Items.Clear();
                this.ddlIsPrinted.Items.Add(new ListItem("全部", string.Empty));
                this.ddlIsPrinted.Items.Add(new ListItem("已打印", "1"));
                this.ddlIsPrinted.Items.Add(new ListItem("未打印", "0"));
                this.SetPurchaseOrderStatusLink();
                this.BindPurchaseOrders();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReBinderPurchaseOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", this.txtOrderId.Text.Trim());
            queryStrings.Add("PurchaseOrderId", this.txtPurchaseOrderId.Text.Trim());
            queryStrings.Add("ShopTo", this.txtShopTo.Text.Trim());
            queryStrings.Add("ProductName", this.txtProductName.Text.Trim());
            queryStrings.Add("DistributorName", this.txtDistributorName.Text.Trim());
            queryStrings.Add("PurchaseStatus", this.lblStatus.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            if (this.shippingModeDropDownList.SelectedValue.HasValue)
            {
                queryStrings.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
            {
                queryStrings.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
            }
            base.ReloadPage(queryStrings);
        }

        private void SetPurchaseOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Admin/purchaseOrder/ManagePurchaseOrder.aspx?PurchaseStatus={0}";
            this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
            this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
            this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            this.hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
            this.hlinkClose.NavigateUrl = string.Format(format, 4);
            this.hlinkHistory.NavigateUrl = string.Format(format, 0x63);
        }

        private bool ValidateValues(out decimal discountAmout)
        {
            string str = string.Empty;
            int length = 0;
            if (this.txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") > 0)
            {
                length = this.txtPurchaseOrderDiscount.Text.Trim().Substring(this.txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") + 1).Length;
            }
            if (!decimal.TryParse(this.txtPurchaseOrderDiscount.Text.Trim(), out discountAmout) || (length > 2))
            {
                str = str + Formatter.FormatErrorMessage("采购单折扣填写错误,采购单折扣只能是数值，且不能超过2位小数");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}


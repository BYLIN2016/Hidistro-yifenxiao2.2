namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class OrderReviews : MemberTemplatedWebControl
    {
        private IButton btnRefer;
        private OrderStatusLabel lblOrderStatus;
        private FormatedMoneyLabel lbltotalPrice;
        private FormatedTimeLabel litAddDate;
        private Literal litCloseReason;
        private Literal litOrderId;
        private Literal litWeight;
        private string orderId;
        private Common_OrderManage_ReviewsOrderItems orderItems;

        protected override void AttachChildControls()
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
            {
                base.GotoResourceNotFound();
            }
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.orderItems = (Common_OrderManage_ReviewsOrderItems) this.FindControl("Common_OrderManage_ReviewsOrderItems");
            this.litWeight = (Literal) this.FindControl("litWeight");
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            this.lbltotalPrice = (FormatedMoneyLabel) this.FindControl("lbltotalPrice");
            this.litAddDate = (FormatedTimeLabel) this.FindControl("litAddDate");
            this.lblOrderStatus = (OrderStatusLabel) this.FindControl("lblOrderStatus");
            this.litCloseReason = (Literal) this.FindControl("litCloseReason");
            this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            if (!this.Page.IsPostBack && ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling)))
            {
                this.btnRefer.Text = "提交评论";
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
                this.BindOrderItems(orderInfo);
                this.BindOrderBase(orderInfo);
            }
        }

        private void BindOrderBase(OrderInfo order)
        {
            this.litOrderId.Text = order.OrderId;
            this.lbltotalPrice.Money = order.GetTotal();
            this.litAddDate.Time = order.OrderDate;
            this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
            if (order.OrderStatus == OrderStatus.Closed)
            {
                this.litCloseReason.Text = order.CloseReason;
            }
        }

        private void BindOrderItems(OrderInfo order)
        {
            this.orderItems.DataSource = order.LineItems.Values;
            this.orderItems.DataBind();
            this.litWeight.Text = order.Weight.ToString();
        }

        public void btnRefer_Click(object sender, EventArgs e)
        {
            if (this.ValidateConvert())
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (RepeaterItem item in this.orderItems.Items)
                {
                    HtmlTextArea area = item.FindControl("txtcontent") as HtmlTextArea;
                    HtmlInputHidden hidden = item.FindControl("hdproductId") as HtmlInputHidden;
                    if (!string.IsNullOrEmpty(area.Value.Trim()) && !string.IsNullOrEmpty(hidden.Value.Trim()))
                    {
                        dictionary.Add(hidden.Value, area.Value);
                    }
                }
                if (dictionary.Count <= 0)
                {
                    this.ShowMessage("请输入评价内容呀！", false);
                }
                else
                {
                    string msg = "";
                    foreach (KeyValuePair<string, string> pair in dictionary)
                    {
                        int productId = Convert.ToInt32(pair.Key.Split(new char[] { '&' })[0].ToString());
                        string str2 = pair.Value;
                        ProductReviewInfo target = new ProductReviewInfo();
                        target.ReviewDate = DateTime.Now;
                        target.ProductId = productId;
                        target.UserId = HiContext.Current.User.UserId;
                        target.UserName = HiContext.Current.User.Username;
                        target.UserEmail = HiContext.Current.User.Email;
                        target.ReviewText = str2;
                        ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductReviewInfo>(target, new string[] { "Refer" });
                        msg = string.Empty;
                        if (!results.IsValid)
                        {
                            foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                            {
                                msg = msg + Formatter.FormatErrorMessage(result.Message);
                            }
                            break;
                        }
                        if (!ProductProcessor.ProductExists(productId))
                        {
                            msg = "您要评论的商品已经不存在";
                            break;
                        }
                        int buyNum = 0;
                        int reviewNum = 0;
                        ProductBrowser.LoadProductReview(productId, out buyNum, out reviewNum);
                        if (buyNum == 0)
                        {
                            msg = "您没有购买此商品，因此不能进行评论";
                            break;
                        }
                        if (reviewNum >= buyNum)
                        {
                            msg = "您已经对此商品进行了评论，请再次购买后方能再进行评论";
                            break;
                        }
                        if (!ProductProcessor.InsertProductReview(target))
                        {
                            msg = "评论失败，请重试";
                            break;
                        }
                    }
                    if (msg != "")
                    {
                        this.ShowMessage(msg, false);
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "评论成功", Globals.GetSiteUrls().UrlData.FormatUrl("user_UserProductReviews")));
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-OrderReviews.html";
            }
            base.OnInit(e);
        }

        private bool ValidateConvert()
        {
            string str = string.Empty;
            if ((HiContext.Current.User.UserRole != UserRole.Member) && (HiContext.Current.User.UserRole != UserRole.Underling))
            {
                str = str + Formatter.FormatErrorMessage("请填写用户名和密码");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMessage(str, false);
                return false;
            }
            return true;
        }
    }
}


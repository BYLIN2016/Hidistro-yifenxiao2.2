namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI.WebControls;

    public class PaySucceed : MemberTemplatedWebControl
    {
        private HyperLink hlkDetails;
        private Label lblPaystatus;

        protected override void AttachChildControls()
        {
            this.hlkDetails = (HyperLink) this.FindControl("hlkDetails");
            this.lblPaystatus = (Label) this.FindControl("lblPayStatus");
            if (!this.Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
                {
                    this.lblPaystatus.Text = "无效访问";
                    this.hlkDetails.Visible = false;
                }
                else
                {
                    OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.Page.Request.QueryString["orderId"]);
                    if ((orderInfo == null) || (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid))
                    {
                        this.lblPaystatus.Text = "订单不存在或订单状态不是已付款";
                        this.hlkDetails.Visible = false;
                    }
                    else
                    {
                        this.hlkDetails.NavigateUrl = Globals.ApplicationPath + "/user/UserOrders.aspx?orderStatus=" + 2;
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-PaySucceed.html";
            }
            base.OnInit(e);
        }
    }
}


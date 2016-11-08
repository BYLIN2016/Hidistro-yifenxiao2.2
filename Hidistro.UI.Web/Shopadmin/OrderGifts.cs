namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class OrderGifts : DistributorPage
    {
        protected Button btnClear;
        protected Button btnSearch;
        protected DataList dlstGifts;
        protected DataList dlstOrderGifts;
        protected Literal litPageNote;
        protected Literal litPageTitle;
        private OrderInfo order;
        private string orderId;
        protected Pager pager;
        protected Pager pagerOrderGifts;
        protected TextBox txtSearchText;

        private void BindGifts()
        {
            GiftQuery query = new GiftQuery();
            query.Page.PageSize = 10;
            query.Page.PageIndex = this.pager.PageIndex;
            query.Name = this.txtSearchText.Text.Trim();
            DbQueryResult gifts = SubsiteSalesHelper.GetGifts(query);
            this.dlstGifts.DataSource = gifts.Data;
            this.dlstGifts.DataBind();
            this.pager.TotalRecords = gifts.TotalRecords;
        }

        private void BindOrderGifts()
        {
            OrderGiftQuery query = new OrderGiftQuery();
            query.PageSize = 10;
            query.PageIndex = this.pagerOrderGifts.PageIndex;
            query.OrderId = this.orderId;
            DbQueryResult orderGifts = SubsiteSalesHelper.GetOrderGifts(query);
            this.dlstOrderGifts.DataSource = orderGifts.Data;
            this.dlstOrderGifts.DataBind();
            this.pagerOrderGifts.TotalRecords = orderGifts.TotalRecords;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!this.order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_GIFTS))
            {
                this.ShowMsg("当前订单状态没有订单礼品操作", false);
            }
            else if (!SubsiteSalesHelper.ClearOrderGifts(this.order))
            {
                this.ShowMsg("清空礼品列表失败", false);
            }
            else
            {
                this.BindOrderGifts();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindGifts();
        }

        private void dlstGifts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                if (!this.order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_GIFTS))
                {
                    this.ShowMsg("当前订单状态没有订单礼品操作", false);
                }
                else
                {
                    int num3;
                    int itemIndex = e.Item.ItemIndex;
                    int giftId = int.Parse(this.dlstGifts.DataKeys[itemIndex].ToString());
                    TextBox box = this.dlstGifts.Items[itemIndex].FindControl("txtQuantity") as TextBox;
                    if (!int.TryParse(box.Text.Trim(), out num3))
                    {
                        this.ShowMsg("礼品数量填写错误", false);
                    }
                    else if (num3 <= 0)
                    {
                        this.ShowMsg("礼品赠送数量不能为0", false);
                    }
                    else
                    {
                        GiftInfo giftDetails = SubsiteSalesHelper.GetGiftDetails(giftId);
                        if (giftDetails == null)
                        {
                            base.GotoResourceNotFound();
                        }
                        else if (!SubsiteSalesHelper.AddOrderGift(this.order, giftDetails, num3, 0))
                        {
                            this.ShowMsg("添加订单礼品失败", false);
                        }
                        else
                        {
                            this.BindOrderGifts();
                        }
                    }
                }
            }
        }

        private void dlstOrderGifts_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            if (!this.order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_GIFTS))
            {
                this.ShowMsg("当前订单状态没有订单礼品操作", false);
            }
            else
            {
                int itemIndex = e.Item.ItemIndex;
                int giftId = int.Parse(this.dlstOrderGifts.DataKeys[itemIndex].ToString());
                if (!SubsiteSalesHelper.DeleteOrderGift(this.order, giftId))
                {
                    this.ShowMsg("删除订单礼品失败", false);
                }
                this.BindOrderGifts();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.Request.QueryString["OrderId"] == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.orderId = this.Page.Request.QueryString["OrderId"];
                this.order = SubsiteSalesHelper.GetOrderInfo(this.orderId);
                this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
                this.btnClear.Click += new EventHandler(this.btnClear_Click);
                this.dlstGifts.ItemCommand += new DataListCommandEventHandler(this.dlstGifts_ItemCommand);
                this.dlstOrderGifts.DeleteCommand += new DataListCommandEventHandler(this.dlstOrderGifts_DeleteCommand);
                if (!base.IsPostBack)
                {
                    if (this.order == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else if (this.order.OrderStatus != OrderStatus.WaitBuyerPay)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (this.order.Gifts.Count > 0)
                        {
                            this.litPageTitle.Text = "编辑订单礼品";
                            this.litPageNote.Text = "修改赠送给买家的礼品.";
                        }
                        this.BindGifts();
                        this.BindOrderGifts();
                    }
                }
            }
        }
    }
}


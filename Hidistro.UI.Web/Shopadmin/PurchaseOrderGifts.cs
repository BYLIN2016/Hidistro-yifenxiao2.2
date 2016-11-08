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

    public class PurchaseOrderGifts : DistributorPage
    {
        protected Button btnClear;
        protected Button btnSearch;
        protected DataList dlstGifts;
        protected DataList dlstOrderGifts;
        protected Literal litPageNote;
        protected Literal litPageTitle;
        protected Pager pager;
        protected Pager pagerOrderGifts;
        private PurchaseOrderInfo purchaseOrder;
        private string purchaseOrderId;
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
            PurchaseOrderGiftQuery query = new PurchaseOrderGiftQuery();
            query.PageSize = 10;
            query.PageIndex = this.pagerOrderGifts.PageIndex;
            query.PurchaseOrderId = this.purchaseOrderId;
            DbQueryResult purchaseOrderGifts = SubsiteSalesHelper.GetPurchaseOrderGifts(query);
            this.dlstOrderGifts.DataSource = purchaseOrderGifts.Data;
            this.dlstOrderGifts.DataBind();
            this.pagerOrderGifts.TotalRecords = purchaseOrderGifts.TotalRecords;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!this.purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
            {
                this.ShowMsg("当前采购单状态没有订单礼品操作", false);
            }
            else if (!SubsiteSalesHelper.ClearPurchaseOrderGifts(this.purchaseOrder))
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
                if (!this.purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
                {
                    this.ShowMsg("当前采购单状态没有订单礼品操作", false);
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
                        else if (!SubsiteSalesHelper.AddPurchaseOrderGift(this.purchaseOrder, giftDetails, num3))
                        {
                            this.ShowMsg("添加采购单礼品失败", false);
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
            if (!this.purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
            {
                this.ShowMsg("当前采购单状态没有订单礼品操作", false);
            }
            else
            {
                int itemIndex = e.Item.ItemIndex;
                int giftId = int.Parse(this.dlstOrderGifts.DataKeys[itemIndex].ToString());
                if (!SubsiteSalesHelper.DeletePurchaseOrderGift(this.purchaseOrder, giftId))
                {
                    this.ShowMsg("删除采购单礼品失败", false);
                }
                this.BindOrderGifts();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.Request.QueryString["PurchaseOrderId"] == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.purchaseOrderId = this.Page.Request.QueryString["PurchaseOrderId"];
                this.purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
                this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
                this.btnClear.Click += new EventHandler(this.btnClear_Click);
                this.dlstGifts.ItemCommand += new DataListCommandEventHandler(this.dlstGifts_ItemCommand);
                this.dlstOrderGifts.DeleteCommand += new DataListCommandEventHandler(this.dlstOrderGifts_DeleteCommand);
                if (!base.IsPostBack)
                {
                    if (this.purchaseOrder == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else if (this.purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (this.purchaseOrder.PurchaseOrderGifts.Count > 0)
                        {
                            this.litPageTitle.Text = "编辑采购单礼品";
                        }
                        this.BindGifts();
                        this.BindOrderGifts();
                    }
                }
            }
        }
    }
}


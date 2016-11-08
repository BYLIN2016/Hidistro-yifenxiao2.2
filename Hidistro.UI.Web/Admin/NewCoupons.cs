namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Coupons)]
    public class NewCoupons : AdminPage
    {
        protected Button btnExport;
        protected Grid grdCoupons;
        protected Pager pager;
        protected TextBox tbcouponNum;
        protected HiddenField txtcouponid;

        private void BindCoupons()
        {
            Pagination page = new Pagination();
            page.PageSize = this.pager.PageSize;
            page.PageIndex = this.pager.PageIndex;
            DbQueryResult newCoupons = CouponHelper.GetNewCoupons(page);
            this.grdCoupons.DataSource = newCoupons.Data;
            this.grdCoupons.DataBind();
            this.pager.TotalRecords = newCoupons.TotalRecords;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int num;
            if (!int.TryParse(this.tbcouponNum.Text, out num))
            {
                this.ShowMsg("导出数量必须为正数", false);
            }
            else if (num <= 0)
            {
                this.ShowMsg("导出数量必须为正数", false);
            }
            else
            {
                int num2;
                if (!int.TryParse(this.txtcouponid.Value, out num2))
                {
                    this.ShowMsg("参数错误", false);
                }
                else
                {
                    CouponInfo coupon = CouponHelper.GetCoupon(num2);
                    string lotNumber = string.Empty;
                    CouponActionStatus status = CouponHelper.CreateCoupon(coupon, num, out lotNumber);
                    switch (status)
                    {
                        case CouponActionStatus.UnknowError:
                            this.ShowMsg("未知错误", false);
                            return;

                        case CouponActionStatus.CreateClaimCodeError:
                            this.ShowMsg("生成优惠券号码错误", false);
                            return;
                    }
                    if ((status == CouponActionStatus.CreateClaimCodeSuccess) && !string.IsNullOrEmpty(lotNumber))
                    {
                        IList<CouponItemInfo> couponItemInfos = CouponHelper.GetCouponItemInfos(lotNumber);
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                        builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                        builder.AppendLine("<td>优惠券批次号</td>");
                        builder.AppendLine("<td>优惠券号码</td>");
                        builder.AppendLine("<td>优惠券金额</td>");
                        builder.AppendLine("<td>过期时间</td>");
                        builder.AppendLine("</tr>");
                        foreach (CouponItemInfo info2 in couponItemInfos)
                        {
                            builder.AppendLine("<tr>");
                            builder.AppendLine("<td>" + lotNumber + "</td>");
                            builder.AppendLine("<td>" + info2.ClaimCode + "</td>");
                            builder.AppendLine("<td>" + coupon.DiscountValue + "</td>");
                            builder.AppendLine("<td>" + coupon.ClosingTime + "</td>");
                            builder.AppendLine("</tr>");
                        }
                        builder.AppendLine("</table>");
                        this.Page.Response.Clear();
                        this.Page.Response.Buffer = false;
                        this.Page.Response.Charset = "GB2312";
                        this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=CouponsInfo_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                        this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                        this.Page.Response.ContentType = "application/ms-excel";
                        this.Page.EnableViewState = false;
                        this.Page.Response.Write(builder.ToString());
                        this.Page.Response.End();
                    }
                }
            }
        }

        private void grdCoupons_ReBindData(object sender)
        {
            this.BindCoupons();
        }

        private void grdCoupons_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int couponId = (int) this.grdCoupons.DataKeys[e.RowIndex].Value;
            if (CouponHelper.DeleteCoupon(couponId))
            {
                this.BindCoupons();
                this.ShowMsg("成功删除了选定张优惠券", true);
            }
            else
            {
                this.ShowMsg("删除优惠券失败", false);
            }
        }

        protected bool IsCouponEnd(object endtime)
        {
            return (Convert.ToDateTime(endtime).CompareTo(DateTime.Now) > 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdCoupons.RowDeleting += new GridViewDeleteEventHandler(this.grdCoupons_RowDeleting);
            this.grdCoupons.ReBindData += new Grid.ReBindDataEventHandler(this.grdCoupons_ReBindData);
            if (!this.Page.IsPostBack)
            {
                this.BindCoupons();
            }
        }
    }
}


namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UserPoints : MemberTemplatedWebControl
    {
        private Literal litUserPoint;
        private Pager pager;
        private Common_Point_PointList pointList;

        protected override void AttachChildControls()
        {
            this.pointList = (Common_Point_PointList) this.FindControl("Common_Point_PointList");
            this.pager = (Pager) this.FindControl("pager");
            this.litUserPoint = (Literal) this.FindControl("litUserPoint");
            PageTitle.AddSiteNameTitle("我的积分", HiContext.Current.Context);
            this.pointList._ItemDataBound += new DataListItemEventHandler(this.pointList_ItemDataBound);
            if (!this.Page.IsPostBack)
            {
                this.BindPoint();
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (user != null)
                {
                    this.litUserPoint.Text = user.Points.ToString();
                }
            }
        }

        private void BindPoint()
        {
            DbQueryResult userPoints = TradeHelper.GetUserPoints(this.pager.PageIndex);
            this.pointList.DataSource = userPoints.Data;
            this.pointList.DataBind();
            this.pager.TotalRecords = userPoints.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserPoints.html";
            }
            base.OnInit(e);
        }

        protected void pointList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Control control = e.Item.Controls[0];
                Label label = (Label) control.FindControl("lblPointType");
                if (label != null)
                {
                    if (label.Text == "0")
                    {
                        label.Text = "兑换优惠券";
                    }
                    else if (label.Text == "1")
                    {
                        label.Text = "兑换礼品";
                    }
                    else if (label.Text == "2")
                    {
                        label.Text = "购物奖励";
                    }
                    else if (label.Text == "3")
                    {
                        label.Text = "退款扣积分";
                    }
                }
            }
        }
    }
}


namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Common_GoodsList_LatestGroupBuy : AscxTemplatedWebControl
    {
        private int maxNum = 1;
        private Repeater regroupbuy;

        protected override void AttachChildControls()
        {
            this.regroupbuy = (Repeater) this.FindControl("regroupbuy");
            this.regroupbuy.DataSource = ProductBrowser.GetGroupByProductList(this.maxNum);
            this.regroupbuy.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_GoodsList/Skin-Common_GoodsList_LatestGroupBuy.ascx";
            }
            base.OnInit(e);
        }

        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
    }
}


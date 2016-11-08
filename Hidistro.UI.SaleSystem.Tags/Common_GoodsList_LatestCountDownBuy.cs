namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Common_GoodsList_LatestCountDownBuy : AscxTemplatedWebControl
    {
        private int maxNum = 1;
        private Repeater repcountdown;

        protected override void AttachChildControls()
        {
            this.repcountdown = (Repeater) this.FindControl("repcountdown");
            this.repcountdown.DataSource = ProductBrowser.GetCounDownProducList(this.maxNum);
            this.repcountdown.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_GoodsList/Skin-Common_GoodsList_LatestCountDownBuy.ascx";
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


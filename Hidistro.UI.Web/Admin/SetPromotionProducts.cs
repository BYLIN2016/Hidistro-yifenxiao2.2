namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class SetPromotionProducts : AdminPage
    {
        private int activityId;
        protected ImageLinkButton btnDeleteAll;
        protected LinkButton btnFinesh;
        protected Grid grdPromotionProducts;
        protected HtmlInputHidden hdactivy;
        public bool isWholesale;
        protected Literal litPromotionName;

        private void BindPromotionProducts()
        {
            this.grdPromotionProducts.DataSource = PromoteHelper.GetPromotionProducts(this.activityId);
            this.grdPromotionProducts.DataBind();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (PromoteHelper.DeletePromotionProducts(this.activityId, null))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        private void grdPromotionProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (PromoteHelper.DeletePromotionProducts(this.activityId, new int?((int) this.grdPromotionProducts.DataKeys[e.RowIndex].Value)))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.hdactivy.Value = this.activityId.ToString();
                this.btnDeleteAll.Click += new EventHandler(this.btnDeleteAll_Click);
                this.grdPromotionProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdPromotionProducts_RowDeleting);
                if (!this.Page.IsPostBack)
                {
                    this.btnFinesh.PostBackUrl = "ProductPromotions.aspx";
                    bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
                    if (this.isWholesale)
                    {
                        this.btnFinesh.PostBackUrl = "ProductPromotions.aspx?isWholesale=true";
                    }
                    PromotionInfo promotion = PromoteHelper.GetPromotion(this.activityId);
                    if (promotion == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litPromotionName.Text = promotion.Name;
                        this.BindPromotionProducts();
                    }
                }
            }
        }
    }
}


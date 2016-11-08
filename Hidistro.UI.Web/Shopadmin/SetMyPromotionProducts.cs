namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Commodities;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SetMyPromotionProducts : DistributorPage
    {
        private int activityId;
        protected ImageLinkButton btnDeleteAll;
        protected Button btnFinesh;
        protected Grid grdPromotionProducts;
        protected HtmlInputHidden hdactivy;
        protected Literal litPromotionName;

        private void BindPromotionProducts()
        {
            this.grdPromotionProducts.DataSource = SubsitePromoteHelper.GetPromotionProducts(this.activityId);
            this.grdPromotionProducts.DataBind();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (SubsitePromoteHelper.DeletePromotionProducts(this.activityId, null))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        private void btnFinesh_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("myproductpromotions.aspx", true);
        }

        protected void DoCallback()
        {
            this.Page.Response.Clear();
            base.Response.ContentType = "text/json";
            StringBuilder builder = new StringBuilder();
            int num = 0;
            int num2 = 0;
            int num3 = 1;
            int.TryParse(base.Request.Params["categoryId"], out num);
            int.TryParse(base.Request.Params["brandId"], out num2);
            int.TryParse(base.Request.Params["page"], out num3);
            ProductQuery query = new ProductQuery();
            query.PageSize = 15;
            query.PageIndex = num3;
            query.SaleStatus = ProductSaleStatus.OnSale;
            query.IsIncludePromotionProduct = false;
            query.IsIncludeBundlingProduct = false;
            query.Keywords = base.Request.Params["serachName"];
            if (num2 != 0)
            {
                query.BrandId = new int?(num2);
            }
            query.CategoryId = new int?(num);
            if (num != 0)
            {
                query.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(num).Path;
            }
            DbQueryResult products = SubSiteProducthelper.GetProducts(query);
            DataTable data = (DataTable) products.Data;
            builder.Append("{'data':[");
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{'ProductId':'");
                builder.Append(data.Rows[i]["ProductId"].ToString().Trim());
                builder.Append("','Name':'");
                builder.Append(data.Rows[i]["ProductName"].ToString());
                builder.Append("','Price':'");
                builder.Append(((decimal) data.Rows[i]["SalePrice"]).ToString("F2"));
                builder.Append("','Stock':'");
                builder.Append(data.Rows[i]["Stock"].ToString());
                builder.Append("'},");
            }
            builder.Append("],'recCount':'");
            builder.Append(products.TotalRecords);
            builder.Append("'}");
            base.Response.Write(builder.ToString());
            base.Response.End();
        }

        private void grdPromotionProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsitePromoteHelper.DeletePromotionProducts(this.activityId, new int?((int) this.grdPromotionProducts.DataKeys[e.RowIndex].Value)))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                this.DoCallback();
            }
            else if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnFinesh.Click += new EventHandler(this.btnFinesh_Click);
                this.hdactivy.Value = this.activityId.ToString();
                this.btnDeleteAll.Click += new EventHandler(this.btnDeleteAll_Click);
                this.grdPromotionProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdPromotionProducts_RowDeleting);
                if (!this.Page.IsPostBack)
                {
                    PromotionInfo promotion = SubsitePromoteHelper.GetPromotion(this.activityId);
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


namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class BundlingProducts : HtmlTemplatedWebControl
    {
        private Pager pager;
        private ThemedTemplatedRepeater rptProduct;

        protected override void AttachChildControls()
        {
            this.rptProduct = (ThemedTemplatedRepeater) this.FindControl("rptProduct");
            this.pager = (Pager) this.FindControl("pager");
            this.rptProduct.ItemDataBound += new RepeaterItemEventHandler(this.rptProduct_ItemDataBound);
            if (!this.Page.IsPostBack)
            {
                this.BindBundlingProducts();
            }
        }

        public List<BundlingItemInfo> BindBundlingItems(int id)
        {
            return ProductBrowser.GetBundlingItemsByID(id);
        }

        private void BindBundlingProducts()
        {
            BundlingInfoQuery query = new BundlingInfoQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            DbQueryResult bundlingProductList = ProductBrowser.GetBundlingProductList(query);
            this.rptProduct.DataSource = bundlingProductList.Data;
            this.rptProduct.DataBind();
            this.pager.TotalRecords = bundlingProductList.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-BundlingProducts.html";
            }
            base.OnInit(e);
        }

        private void ReloadHelpList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }

        protected void rptProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
            {
                DataRowView dataItem = (DataRowView) e.Item.DataItem;
                Label label = (Label) e.Item.Controls[0].FindControl("lbBundlingID");
                FormatedMoneyLabel label2 = (FormatedMoneyLabel) e.Item.Controls[0].FindControl("lbltotalPrice");
                FormatedMoneyLabel label3 = (FormatedMoneyLabel) e.Item.Controls[0].FindControl("lblbundlingPrice");
                FormatedMoneyLabel label4 = (FormatedMoneyLabel) e.Item.Controls[0].FindControl("lblsaving");
                HyperLink link = (HyperLink) e.Item.Controls[0].FindControl("hlBuy");
                Repeater repeater = (Repeater) e.Item.Controls[0].FindControl("rpbundlingitem");
                List<BundlingItemInfo> bundlingItemsByID = ProductBrowser.GetBundlingItemsByID(Convert.ToInt32(label.Text));
                decimal num = 0M;
                foreach (BundlingItemInfo info in bundlingItemsByID)
                {
                    num += info.ProductNum * info.ProductPrice;
                }
                label2.Money = num;
                label4.Money = Convert.ToDecimal(label2.Money) - Convert.ToDecimal(label3.Money);
                if (!HiContext.Current.SiteSettings.IsDistributorSettings && !HiContext.Current.SiteSettings.IsOpenSiteSale)
                {
                    link.Visible = false;
                }
                repeater.DataSource = bundlingItemsByID;
                repeater.DataBind();
            }
        }
    }
}


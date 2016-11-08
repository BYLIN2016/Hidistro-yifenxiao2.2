namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.BindProduct)]
    public class AddBundlingProduct : AdminPage
    {
        protected Button btnAddBindProduct;
        protected YesNoRadioButtonList radstock;
        protected TextBox txtBindName;
        protected TextBox txtNum;
        protected TextBox txtSalePrice;
        protected TrimTextBox txtShortDescription;

        protected void btnAddBindProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(base.Request.Form["selectProductsinfo"]))
            {
                this.ShowMsg("获取绑定商品信息错误", false);
            }
            else if ((!string.IsNullOrEmpty(this.txtBindName.Text) && !string.IsNullOrEmpty(this.txtNum.Text)) && !string.IsNullOrEmpty(this.txtSalePrice.Text))
            {
                BundlingInfo bundlingInfo = new BundlingInfo();
                bundlingInfo.AddTime = DateTime.Now;
                bundlingInfo.Name = this.txtBindName.Text;
                bundlingInfo.Price = Convert.ToDecimal(this.txtSalePrice.Text);
                bundlingInfo.SaleStatus = Convert.ToInt32(this.radstock.SelectedValue);
                bundlingInfo.Num = Convert.ToInt32(this.txtNum.Text);
                if (!string.IsNullOrEmpty(this.txtShortDescription.Text))
                {
                    bundlingInfo.ShortDescription = this.txtShortDescription.Text;
                }
                string[] strArray = base.Request.Form["selectProductsinfo"].Split(new char[] { ',' });
                List<BundlingItemInfo> list = new List<BundlingItemInfo>();
                foreach (string str2 in strArray)
                {
                    BundlingItemInfo item = new BundlingItemInfo();
                    string[] strArray2 = str2.Split(new char[] { '|' });
                    item.ProductID = Convert.ToInt32(strArray2[0]);
                    item.SkuId = strArray2[1];
                    item.ProductNum = Convert.ToInt32(strArray2[2]);
                    list.Add(item);
                }
                bundlingInfo.BundlingItemInfos = list;
                if (PromoteHelper.AddBundlingProduct(bundlingInfo))
                {
                    this.ShowMsg("添加绑定商品成功", true);
                }
                else
                {
                    this.ShowMsg("添加绑定商品错误", false);
                }
            }
            else
            {
                this.ShowMsg("你的资料填写不完整", false);
            }
        }

        protected void DoCallback()
        {
            this.Page.Response.Clear();
            base.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder();
            int num = 0;
            int num2 = 0;
            int num3 = 1;
            int.TryParse(base.Request.Params["categoryId"], out num);
            int.TryParse(base.Request.Params["brandId"], out num2);
            int.TryParse(base.Request.Params["page"], out num3);
            ProductQuery query = new ProductQuery();
            query.PageSize = 5;
            query.PageIndex = num3;
            query.SaleStatus = ProductSaleStatus.OnSale;
            query.Keywords = base.Request.Params["serachName"];
            if (num2 != 0)
            {
                query.BrandId = new int?(num2);
            }
            query.CategoryId = new int?(num);
            if (num != 0)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
            }
            DbQueryResult products = ProductHelper.GetProducts(query);
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
                DataTable skusByProductId = ProductHelper.GetSkusByProductId((int) data.Rows[i]["ProductId"]);
                builder.Append("','skus':[");
                int count = skusByProductId.Rows.Count;
                for (int j = 0; j < count; j++)
                {
                    builder.Append("{'skuid':'");
                    builder.Append(skusByProductId.Rows[j]["skuid"].ToString());
                    builder.Append("','stock':'");
                    builder.Append(skusByProductId.Rows[j]["stock"].ToString());
                    builder.Append("','skucontent':'");
                    builder.Append(this.GetSkuContent(skusByProductId.Rows[j]["skuid"].ToString()));
                    builder.Append("','saleprice':'");
                    builder.Append(((decimal) skusByProductId.Rows[j]["saleprice"]).ToString("F2"));
                    builder.Append("'},");
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append("]");
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("],'recCount':'");
            builder.Append(products.TotalRecords);
            builder.Append("'}");
            base.Response.Write(builder.ToString());
            base.Response.End();
        }

        public string GetSkuContent(string skuId)
        {
            string str = "";
            if (!string.IsNullOrEmpty(skuId.Trim()))
            {
                foreach (DataRow row in ControlProvider.Instance().GetSkuContentBySku(skuId).Rows)
                {
                    if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                    {
                        object obj2 = str;
                        str = string.Concat(new object[] { obj2, row["AttributeName"], ":", row["ValueStr"], "; " });
                    }
                }
            }
            if (!(str == ""))
            {
                return str;
            }
            return "无规格";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                this.DoCallback();
            }
        }
    }
}


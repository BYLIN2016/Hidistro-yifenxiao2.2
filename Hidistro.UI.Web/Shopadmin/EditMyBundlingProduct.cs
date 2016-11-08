namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class EditMyBundlingProduct : DistributorPage
    {
        protected Button btnEditBindProduct;
        private int bundlingid;
        protected YesNoRadioButtonList radstock;
        protected Repeater Rpbinditems;
        protected TextBox txtBindName;
        protected TextBox txtNum;
        protected TextBox txtSalePrice;
        protected TrimTextBox txtShortDescription;

        protected void BindEditBindProduct()
        {
            BundlingInfo bundlingInfo = SubsitePromoteHelper.GetBundlingInfo(this.bundlingid);
            this.txtBindName.Text = bundlingInfo.Name;
            this.txtNum.Text = bundlingInfo.Num.ToString();
            this.txtSalePrice.Text = bundlingInfo.Price.ToString("F");
            this.radstock.SelectedValue = Convert.ToBoolean(bundlingInfo.SaleStatus);
            this.txtShortDescription.Text = bundlingInfo.ShortDescription;
            this.Rpbinditems.DataSource = bundlingInfo.BundlingItemInfos;
            this.Rpbinditems.DataBind();
        }

        protected void btnEditBindProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(base.Request.Form["selectProductsinfo"]))
            {
                this.ShowMsg("获取绑定商品信息错误", false);
            }
            else if ((!string.IsNullOrEmpty(this.txtBindName.Text) && !string.IsNullOrEmpty(this.txtNum.Text)) && !string.IsNullOrEmpty(this.txtSalePrice.Text))
            {
                BundlingInfo bundlingInfo = new BundlingInfo();
                bundlingInfo.BundlingID = this.bundlingid;
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
                if (SubsitePromoteHelper.UpdateBundlingProduct(bundlingInfo))
                {
                    this.ShowMsg("修改绑定商品成功", true);
                    this.BindEditBindProduct();
                }
                else
                {
                    this.ShowMsg("修改绑定商品错误", false);
                }
            }
            else
            {
                this.ShowMsg("你的资料填写不完整", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["bundlingid"], out this.bundlingid))
            {
                this.ShowMsg("参数提交错误", false);
            }
            else if (!this.Page.IsPostBack)
            {
                this.BindEditBindProduct();
            }
        }
    }
}


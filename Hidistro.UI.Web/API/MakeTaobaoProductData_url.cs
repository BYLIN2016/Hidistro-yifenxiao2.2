namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.HOP;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MakeTaobaoProductData_url : Page
    {
        protected Literal litmsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            TaobaoProductInfo taobaoProduct = new TaobaoProductInfo();
            string s = base.Request.Form["productid"];
            string str2 = base.Request.Form["stuffstatus"];
            string str3 = base.Request.Form["title"];
            long num = long.Parse(base.Request.Form["num"]);
            string str4 = base.Request.Form["locationcity"];
            string str5 = base.Request.Form["LocationState"];
            string str6 = base.Request.Form["Cid"];
            string str7 = base.Request.Form["hasinvoice"];
            string str8 = base.Request.Form["HasWarranty"];
            string str9 = base.Request.Form["props"];
            string str10 = base.Request.Form["inputpids"];
            string str11 = base.Request.Form["inputstr"];
            string str12 = base.Request.Form["skuproperties"];
            string str13 = base.Request.Form["skuquantities"];
            string str14 = base.Request.Form["skuprices"];
            string str15 = base.Request.Form["skuouterids"];
            string str16 = base.Request.Form["freightpayer"];
            if (str16 == "buyer")
            {
                string str17 = base.Request.Form["postfee"];
                string str18 = base.Request.Form["expressfee"];
                string str19 = base.Request.Form["emsfee"];
                taobaoProduct.PostFee = decimal.Parse(str17);
                taobaoProduct.EMSFee = decimal.Parse(str19);
                taobaoProduct.ExpressFee = decimal.Parse(str18);
            }
            taobaoProduct.ProductId = int.Parse(s);
            taobaoProduct.StuffStatus = str2;
            taobaoProduct.PropertyAlias = str9;
            taobaoProduct.ProTitle = str3;
            taobaoProduct.Num = num;
            taobaoProduct.LocationState = str5;
            taobaoProduct.LocationCity = str4;
            taobaoProduct.FreightPayer = str16;
            taobaoProduct.Cid = Convert.ToInt64(str6);
            if (!string.IsNullOrEmpty(str10))
            {
                taobaoProduct.InputPids = str10;
            }
            if (!string.IsNullOrEmpty(str11))
            {
                taobaoProduct.InputStr = str11;
            }
            if (!string.IsNullOrEmpty(str12))
            {
                taobaoProduct.SkuProperties = str12;
            }
            if (!string.IsNullOrEmpty(str14))
            {
                taobaoProduct.SkuPrices = str14;
            }
            if (!string.IsNullOrEmpty(str15))
            {
                taobaoProduct.SkuOuterIds = str15;
            }
            if (!string.IsNullOrEmpty(str13))
            {
                taobaoProduct.SkuQuantities = str13;
            }
            taobaoProduct.HasInvoice = Convert.ToBoolean(str7);
            taobaoProduct.HasWarranty = Convert.ToBoolean(str8);
            taobaoProduct.HasDiscount = false;
            taobaoProduct.ListTime = DateTime.Now;
            if (ProductHelper.UpdateToaobProduct(taobaoProduct))
            {
                this.litmsg.Text = "制作淘宝格式的商品数据成功";
            }
            else
            {
                this.litmsg.Text = "制作淘宝格式的商品数据失败";
            }
        }
    }
}


namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Member;
    using Hidistro.SaleSystem.Shopping;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class SubmmitOrderHandler : IHttpHandler
    {
        private void AddUserShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string erromsg = "";
            if (this.ValShippingAddress(context, ref erromsg))
            {
                if (PersonalHelper.CreateShippingAddress(this.GetShippingAddressInfo(context)))
                {
                    IList<ShippingAddressInfo> shippingAddress = PersonalHelper.GetShippingAddress(HiContext.Current.User.UserId);
                    ShippingAddressInfo info = shippingAddress[shippingAddress.Count - 1];
                    context.Response.Write(string.Concat(new object[] { "{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"", info.ShipTo, "\",\"RegionId\":\"", RegionHelper.GetFullRegion(info.RegionId, " "), "\",\"ShippingAddress\":\"", info.Address, "\",\"ShippingId\":\"", info.ShippingId, "\",\"CellPhone\":\"", info.CellPhone, "\"}}" }));
                }
                else
                {
                    context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经在，请重新输入一次再试\"}");
                }
            }
            else
            {
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + erromsg + "\"}");
            }
        }

        private void CalculateFreight(HttpContext context)
        {
            decimal money = 0M;
            if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]) && !string.IsNullOrEmpty(context.Request["RegionId"]))
            {
                int modeId = int.Parse(context.Request["ModeId"], NumberStyles.None);
                int totalWeight = int.Parse(context.Request["Weight"], NumberStyles.None);
                int regionId = int.Parse(context.Request["RegionId"], NumberStyles.None);
                ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(modeId, true);
                money = ShoppingProcessor.CalcFreight(regionId, totalWeight, shippingMode);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Status\":\"OK\",");
            builder.AppendFormat("\"Price\":\"{0}\"", Globals.FormatMoney(money));
            builder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(builder.ToString());
        }

        private ShippingAddressInfo GetShippingAddressInfo(HttpContext context)
        {
            ShippingAddressInfo info = new ShippingAddressInfo();
            info.UserId = HiContext.Current.User.UserId;
            info.ShipTo = context.Request.Params["ShippingTo"].Trim();
            info.RegionId = Convert.ToInt32(context.Request.Params["RegionId"].Trim());
            info.Address = context.Request.Params["AddressDetails"].Trim();
            info.Zipcode = context.Request.Params["ZipCode"].Trim();
            info.CellPhone = context.Request.Params["CellHphone"].Trim();
            info.TelPhone = context.Request.Params["TelPhone"].Trim();
            return info;
        }

        private void GetUserRegionId(HttpContext context)
        {
            string str = context.Request["Prov"];
            string str2 = context.Request["City"];
            string str3 = context.Request["Areas"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if ((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2)) && !string.IsNullOrEmpty(str3))
            {
                builder.Append("\"Status\":\"OK\",\"RegionId\":\"" + RegionHelper.GetRegionId(str3, str2, str) + "\"}");
            }
            else
            {
                builder.Append("\"Status\":\"NOK\"}");
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(builder);
        }

        private void GetUserShippingAddress(HttpContext context)
        {
            ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(int.Parse(context.Request["ShippingId"], NumberStyles.None));
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (shippingAddress != null)
            {
                builder.Append("\"Status\":\"OK\",");
                builder.AppendFormat("\"ShipTo\":\"{0}\",", Globals.HtmlDecode(shippingAddress.ShipTo));
                builder.AppendFormat("\"Address\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Address));
                builder.AppendFormat("\"Zipcode\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Zipcode));
                builder.AppendFormat("\"CellPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.CellPhone));
                builder.AppendFormat("\"TelPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.TelPhone));
                builder.AppendFormat("\"RegionId\":\"{0}\"", shippingAddress.RegionId);
            }
            else
            {
                builder.Append("\"Status\":\"0\"");
            }
            builder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(builder);
        }

        private void ProcessorPaymentMode(HttpContext context)
        {
            decimal money = 0M;
            if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]))
            {
                int modeId = int.Parse(context.Request["ModeId"], NumberStyles.None);
                decimal cartMoney = decimal.Parse(context.Request["TotalPrice"]);
                money = ShoppingProcessor.GetPaymentMode(modeId).CalcPayCharge(cartMoney);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Status\":\"OK\",");
            builder.AppendFormat("\"Charge\":\"{0}\"", Globals.FormatMoney(money));
            builder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(builder.ToString());
        }

        private void ProcessorUseCoupon(HttpContext context)
        {
            decimal orderAmount = decimal.Parse(context.Request["CartTotal"]);
            string claimCode = context.Request["CouponCode"];
            CouponInfo info = ShoppingProcessor.UseCoupon(orderAmount, claimCode);
            StringBuilder builder = new StringBuilder();
            if (info != null)
            {
                builder.Append("{");
                builder.Append("\"Status\":\"OK\",");
                builder.AppendFormat("\"CouponName\":\"{0}\",", info.Name);
                builder.AppendFormat("\"DiscountValue\":\"{0}\"", Globals.FormatMoney(info.DiscountValue));
                builder.Append("}");
            }
            else
            {
                builder.Append("{");
                builder.Append("\"Status\":\"ERROR\"");
                builder.Append("}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(builder);
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                switch (context.Request["Action"])
                {
                    case "GetUserShippingAddress":
                        this.GetUserShippingAddress(context);
                        return;

                    case "CalculateFreight":
                        this.CalculateFreight(context);
                        return;

                    case "ProcessorPaymentMode":
                        this.ProcessorPaymentMode(context);
                        return;

                    case "ProcessorUseCoupon":
                        this.ProcessorUseCoupon(context);
                        return;

                    case "GetRegionId":
                        this.GetUserRegionId(context);
                        return;

                    case "AddShippingAddress":
                        this.AddUserShippingAddress(context);
                        return;

                    case "UpdateShippingAddress":
                        this.UpdateShippingAddress(context);
                        return;
                }
            }
            catch
            {
            }
        }

        private void UpdateShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string erromsg = "";
            erromsg = "请选择要修改的收货地址";
            if ((this.ValShippingAddress(context, ref erromsg) || string.IsNullOrEmpty(context.Request.Params["ShippingId"])) || (Convert.ToInt32(context.Request.Params["ShippingId"]) > 0))
            {
                ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo(context);
                shippingAddressInfo.ShippingId = Convert.ToInt32(context.Request.Params["ShippingId"]);
                if (PersonalHelper.UpdateShippingAddress(shippingAddressInfo))
                {
                    context.Response.Write(string.Concat(new object[] { "{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"", shippingAddressInfo.ShipTo, "\",\"RegionId\":\"", RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " "), "\",\"ShippingAddress\":\"", shippingAddressInfo.Address, "\",\"ShippingId\":\"", shippingAddressInfo.ShippingId, "\",\"CellPhone\":\"", shippingAddressInfo.CellPhone, "\"}}" }));
                }
                else
                {
                    context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经在，请重新输入一次再试\"}");
                }
            }
            else
            {
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + erromsg + "\"}");
            }
        }

        private bool ValShippingAddress(HttpContext context, ref string erromsg)
        {
            Regex regex = new Regex(@"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*");
            if (string.IsNullOrEmpty(context.Request.Params["ShippingTo"].Trim()) || !regex.IsMatch(context.Request.Params["ShippingTo"].Trim()))
            {
                erromsg = "收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request.Params["AddressDetails"].Trim()))
            {
                erromsg = "详细地址不能为空";
                return false;
            }
            if ((context.Request.Params["AddressDetails"].Trim().Length < 3) || (context.Request.Params["AddressDetails"].Trim().Length > 60))
            {
                erromsg = "详细地址长度在3-60个字符之间";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request.Params["RegionId"].Trim()) || (Convert.ToInt32(context.Request.Params["RegionId"].Trim()) <= 0))
            {
                erromsg = "请选择收货地址";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request.Params["TelPhone"].Trim()) && string.IsNullOrEmpty(context.Request.Params["CellHphone"].Trim().Trim()))
            {
                erromsg = "电话号码和手机二者必填其一";
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request.Params["TelPhone"].Trim()) && ((context.Request.Params["TelPhone"].Trim().Length < 3) || (context.Request.Params["TelPhone"].Trim().Length > 20)))
            {
                erromsg = "电话号码长度限制在3-20个字符之间";
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request.Params["CellHphone"].Trim()) && ((context.Request.Params["CellHphone"].Trim().Length < 3) || (context.Request.Params["CellHphone"].Trim().Length > 20)))
            {
                erromsg = "手机号码长度限制在3-20个字符之间";
                return false;
            }
            if (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) >= HiContext.Current.Config.ShippingAddressQuantity)
            {
                erromsg = string.Format("最多只能添加{0}个收货地址", HiContext.Current.Config.ShippingAddressQuantity);
                return false;
            }
            return true;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}


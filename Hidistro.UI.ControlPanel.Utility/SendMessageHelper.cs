namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Comments;
    using System;
    using System.Collections.Generic;

    public class SendMessageHelper
    {
        public static void SendMessageToDistributors(string productId, int type)
        {
            string format = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            int sumcount = 0;
            string productNameByProductIds = "";
            IList<string> userNameByProductId = new List<string>();
            switch (type)
            {
                case 1:
                    productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                    userNameByProductId = ProductHelper.GetUserNameByProductId(productId.ToString());
                    format = "供应商下架了{0}个商品";
                    str2 = "尊敬的各位分销商，供应商已下架了{0}个商品，如下：";
                    break;

                case 2:
                    productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                    userNameByProductId = ProductHelper.GetUserNameByProductId(productId.ToString());
                    format = "供应商入库了{0}个商品";
                    str2 = "尊敬的各位分销商，供应商已入库了{0}个商品，如下：";
                    break;

                case 3:
                    productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                    userNameByProductId = ProductHelper.GetUserNameByProductId(productId.ToString());
                    format = "供应商删除了{0}个商品";
                    str2 = "尊敬的各位分销商，供应商已删除了{0}个商品，如下：";
                    break;

                case 4:
                    str3 = productId.Split(new char[] { '|' })[1].ToString();
                    str4 = productId.Split(new char[] { '|' })[2].ToString();
                    productNameByProductIds = ProductHelper.GetProductNamesByLineId(Convert.ToInt32(productId.Split(new char[] { '|' })[0].ToString()), out sumcount);
                    userNameByProductId = ProductHelper.GetUserIdByLineId(Convert.ToInt32(productId.Split(new char[] { '|' })[0].ToString()));
                    format = "供应商转移了{0}个商品";
                    str2 = "尊敬的各位分销商，供应商已将整个产品线" + str3 + "的商品转移移至产品线" + str4 + "目录下，共{0}个，如下：";
                    break;

                case 5:
                    productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                    userNameByProductId = ProductHelper.GetUserNameByProductId(productId.ToString());
                    format = "供应商取消了{0}个商品的铺货状态";
                    str2 = "尊敬的各位分销商，供应商已取消了{0}个商品的铺货，如下：";
                    break;

                default:
                    str4 = productId.Split(new char[] { '|' })[1].ToString();
                    productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId.Split(new char[] { '|' })[0].ToString(), out sumcount);
                    userNameByProductId = ProductHelper.GetUserNameByProductId(productId.Split(new char[] { '|' })[0].ToString());
                    format = "供应商转移了{0}个商品至产品线" + str4;
                    str2 = "尊敬的各位分销商，供应商已转移了{0}个商品至产品线" + str4 + "，如下：";
                    break;
            }
            if (sumcount > 0)
            {
                IList<MessageBoxInfo> messageBoxInfos = new List<MessageBoxInfo>();
                foreach (string str6 in userNameByProductId)
                {
                    MessageBoxInfo item = new MessageBoxInfo();
                    item.Accepter = str6;
                    item.Sernder = "admin";
                    item.Title = string.Format(format, sumcount);
                    item.Content = string.Format(str2, sumcount) + productNameByProductIds;
                    messageBoxInfos.Add(item);
                }
                if (messageBoxInfos.Count > 0)
                {
                    NoticeHelper.AddMessageToDistributor(messageBoxInfos);
                }
            }
        }
    }
}


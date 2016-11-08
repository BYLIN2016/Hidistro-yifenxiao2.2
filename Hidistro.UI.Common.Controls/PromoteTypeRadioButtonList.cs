namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class PromoteTypeRadioButtonList : RadioButtonList
    {
        
        private bool _IsProductPromote;
        
        private bool _IsSubSite;
        
        private bool _IsWholesale;

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            if (this.IsProductPromote)
            {
                if (this.IsWholesale)
                {
                    builder.AppendFormat("<input id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />批发打折", 4);
                }
                else
                {
                    builder.AppendFormat("<input id=\"radPromoteType_Discount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />直接打折", 1);
                    builder.AppendFormat("<input id=\"radPromoteType_Amount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />固定金额出售", 2);
                    builder.AppendFormat("<input id=\"radPromoteType_Reduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />减价优惠", 3);
                    builder.AppendFormat("<input id=\"radPromoteType_SentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />买商品赠送礼品", 5);
                    builder.AppendFormat("<input id=\"radPromoteType_SentProduct\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />有买有送", 6);
                    if (this.IsSubSite)
                    {
                        builder.AppendFormat("<input id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />批发打折", 4);
                    }
                }
            }
            else if (this.IsWholesale)
            {
                builder.AppendFormat("<input id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发打折", 13);
                builder.AppendFormat("<input id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发优惠金额", 14);
            }
            else
            {
                builder.AppendFormat("<input id=\"radPromoteType_FullAmountDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额打折", 11);
                builder.AppendFormat("<input id=\"radPromoteType_FullAmountReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额优惠金额", 12);
                if (this.IsSubSite)
                {
                    builder.AppendFormat("<input id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发打折", 13);
                    builder.AppendFormat("<input id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发优惠金额", 14);
                }
                builder.AppendFormat("<input id=\"radPromoteType_FullAmountSentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额送礼品", 15);
                builder.AppendFormat("<input id=\"radPromoteType_FullAmountSentTimesPoint\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额送倍数积分", 0x10);
                builder.AppendFormat("<input id=\"radPromoteType_FullAmountSentFreight\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额免运费", 0x11);
            }
            writer.Write(builder.ToString());
        }

        public bool IsProductPromote
        {
            
            get
            {
                return _IsProductPromote;
            }
            
            set
            {
                _IsProductPromote = value;
            }
        }

        public bool IsSubSite
        {
            
            get
            {
                return _IsSubSite;
            }
            
            set
            {
                _IsSubSite = value;
            }
        }

        public bool IsWholesale
        {
            
            get
            {
                return _IsWholesale;
            }
            
            set
            {
                _IsWholesale = value;
            }
        }
    }
}


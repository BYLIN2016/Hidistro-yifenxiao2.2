namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class SiteSettings
    {
        
        private string _AssistantIv;
        
        private string _AssistantKey;
        
        private string _CheckCode;
        
        private int _CloseOrderDays;
        
        private int _ClosePurchaseOrderDays;
        
        private string _CnzzPassword;
        
        private string _CnzzUsername;
        
        private DateTime? _CreateDate;
        
        private int _DecimalLength;
        
        private string _DefaultProductImage;
        
        private string _DefaultProductThumbnail1;
        
        private string _DefaultProductThumbnail2;
        
        private string _DefaultProductThumbnail3;
        
        private string _DefaultProductThumbnail4;
        
        private string _DefaultProductThumbnail5;
        
        private string _DefaultProductThumbnail6;
        
        private string _DefaultProductThumbnail7;
        
        private string _DefaultProductThumbnail8;
        
        private bool _Disabled;
        
        private string _DistributorRequestInstruction;
        
        private string _DistributorRequestProtocols;
        
        private string _EmailSender;
        
        private string _EmailSettings;
        
        private bool _EnabledCnzz;
        
        private DateTime? _EtaoApplyTime;
        
        private string _EtaoID;
        
        private int _EtaoStatus;
        
        private int _FinishOrderDays;
        
        private int _FinishPurchaseOrderDays;
        
        private string _Footer;
        
        private string _HtmlOnlineServiceCode;
        
        private bool _IsOpenEtao;
        
        private bool _IsOpenSiteSale;
        
        private string _LogoUrl;
        
        private int _OrderShowDays;
        
        private decimal _PointsRate;
        
        private int _ReferralDeduct;
        
        private string _RegisterAgreement;
        
        private DateTime? _RequestDate;
        
        private string _SearchMetaDescription;
        
        private string _SearchMetaKeywords;
        
        private string _SiteDescription;
        
        private string _SiteMapNum;
        
        private string _SiteMapTime;
        
        private string _SiteName;
        
        private string _SiteUrl;
        
        private string _SMSSender;
        
        private string _SMSSettings;
        
        private int _TaobaoShippingType;
        
        private decimal _TaxRate;
        
        private string _Theme;
        
        private int? _UserId;
        
        private string _YourPriceName;

        public SiteSettings(string siteUrl, int? distributorUserId)
        {
            this.SiteUrl = siteUrl;
            this.UserId = distributorUserId;
            this.IsOpenSiteSale = true;
            this.Disabled = false;
            this.SiteDescription = "最安全，最专业的网上商店系统";
            this.Theme = "default";
            this.SiteName = "Hishop";
            this.LogoUrl = "/utility/pics/logo.jpg";
            this.DefaultProductImage = "/utility/pics/none.gif";
            this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
            this.DecimalLength = 2;
            this.PointsRate = 1M;
            this.OrderShowDays = 7;
            this.CloseOrderDays = 3;
            this.ClosePurchaseOrderDays = 5;
            this.FinishOrderDays = 7;
            this.FinishPurchaseOrderDays = 10;
        }

        public static SiteSettings FromXml(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("Settings");
            int? distributorUserId = null;
            SiteSettings settings2 = new SiteSettings(node.SelectSingleNode("SiteUrl").InnerText, distributorUserId);
            settings2.AssistantIv = node.SelectSingleNode("AssistantIv").InnerText;
            settings2.AssistantKey = node.SelectSingleNode("AssistantKey").InnerText;
            settings2.DecimalLength = int.Parse(node.SelectSingleNode("DecimalLength").InnerText);
            settings2.DefaultProductImage = node.SelectSingleNode("DefaultProductImage").InnerText;
            settings2.DefaultProductThumbnail1 = node.SelectSingleNode("DefaultProductThumbnail1").InnerText;
            settings2.DefaultProductThumbnail2 = node.SelectSingleNode("DefaultProductThumbnail2").InnerText;
            settings2.DefaultProductThumbnail3 = node.SelectSingleNode("DefaultProductThumbnail3").InnerText;
            settings2.DefaultProductThumbnail4 = node.SelectSingleNode("DefaultProductThumbnail4").InnerText;
            settings2.DefaultProductThumbnail5 = node.SelectSingleNode("DefaultProductThumbnail5").InnerText;
            settings2.DefaultProductThumbnail6 = node.SelectSingleNode("DefaultProductThumbnail6").InnerText;
            settings2.DefaultProductThumbnail7 = node.SelectSingleNode("DefaultProductThumbnail7").InnerText;
            settings2.DefaultProductThumbnail8 = node.SelectSingleNode("DefaultProductThumbnail8").InnerText;
            settings2.CheckCode = node.SelectSingleNode("CheckCode").InnerText;
            settings2.IsOpenSiteSale = bool.Parse(node.SelectSingleNode("IsOpenSiteSale").InnerText);
            settings2.Disabled = bool.Parse(node.SelectSingleNode("Disabled").InnerText);
            settings2.ReferralDeduct = int.Parse(node.SelectSingleNode("ReferralDeduct").InnerText);
            settings2.Footer = node.SelectSingleNode("Footer").InnerText;
            settings2.RegisterAgreement = node.SelectSingleNode("RegisterAgreement").InnerText;
            settings2.HtmlOnlineServiceCode = node.SelectSingleNode("HtmlOnlineServiceCode").InnerText;
            settings2.LogoUrl = node.SelectSingleNode("LogoUrl").InnerText;
            settings2.OrderShowDays = int.Parse(node.SelectSingleNode("OrderShowDays").InnerText);
            settings2.CloseOrderDays = int.Parse(node.SelectSingleNode("CloseOrderDays").InnerText);
            settings2.ClosePurchaseOrderDays = int.Parse(node.SelectSingleNode("ClosePurchaseOrderDays").InnerText);
            settings2.FinishOrderDays = int.Parse(node.SelectSingleNode("FinishOrderDays").InnerText);
            settings2.FinishPurchaseOrderDays = int.Parse(node.SelectSingleNode("FinishPurchaseOrderDays").InnerText);
            settings2.TaxRate = decimal.Parse(node.SelectSingleNode("TaxRate").InnerText);
            settings2.PointsRate = decimal.Parse(node.SelectSingleNode("PointsRate").InnerText);
            settings2.SearchMetaDescription = node.SelectSingleNode("SearchMetaDescription").InnerText;
            settings2.SearchMetaKeywords = node.SelectSingleNode("SearchMetaKeywords").InnerText;
            settings2.SiteDescription = node.SelectSingleNode("SiteDescription").InnerText;
            settings2.SiteName = node.SelectSingleNode("SiteName").InnerText;
            settings2.SiteUrl = node.SelectSingleNode("SiteUrl").InnerText;
            settings2.UserId = null;
            settings2.Theme = node.SelectSingleNode("Theme").InnerText;
            settings2.YourPriceName = node.SelectSingleNode("YourPriceName").InnerText;
            settings2.DistributorRequestInstruction = node.SelectSingleNode("DistributorRequestInstruction").InnerText;
            settings2.DistributorRequestProtocols = node.SelectSingleNode("DistributorRequestProtocols").InnerText;
            settings2.EmailSender = node.SelectSingleNode("EmailSender").InnerText;
            settings2.EmailSettings = node.SelectSingleNode("EmailSettings").InnerText;
            settings2.SMSSender = node.SelectSingleNode("SMSSender").InnerText;
            settings2.SMSSettings = node.SelectSingleNode("SMSSettings").InnerText;
            settings2.SiteMapTime = node.SelectSingleNode("SiteMapTime").InnerText;
            settings2.SiteMapNum = node.SelectSingleNode(" SiteMapNum").InnerText;
            settings2.TaobaoShippingType = int.Parse(node.SelectSingleNode("TaobaoShippingType").InnerText);
            settings2.EnabledCnzz = bool.Parse(node.SelectSingleNode("EnabledCnzz").InnerText);
            settings2.CnzzUsername = node.SelectSingleNode("CnzzUsername").InnerText;
            settings2.CnzzPassword = node.SelectSingleNode("CnzzPassword").InnerText;
            return settings2;
        }

        private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
        {
            XmlNode newChild = root.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = doc.CreateElement(nodeName);
                root.AppendChild(newChild);
            }
            newChild.InnerText = nodeValue;
        }

        public void WriteToXml(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("Settings");
            SetNodeValue(doc, root, "SiteUrl", this.SiteUrl);
            SetNodeValue(doc, root, "AssistantIv", this.AssistantIv);
            SetNodeValue(doc, root, "AssistantKey", this.AssistantKey);
            SetNodeValue(doc, root, "DecimalLength", this.DecimalLength.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "DefaultProductImage", this.DefaultProductImage);
            SetNodeValue(doc, root, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
            SetNodeValue(doc, root, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
            SetNodeValue(doc, root, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
            SetNodeValue(doc, root, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
            SetNodeValue(doc, root, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
            SetNodeValue(doc, root, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
            SetNodeValue(doc, root, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
            SetNodeValue(doc, root, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
            SetNodeValue(doc, root, "CheckCode", this.CheckCode);
            SetNodeValue(doc, root, "IsOpenSiteSale", this.IsOpenSiteSale ? "true" : "false");
            SetNodeValue(doc, root, "Disabled", this.Disabled ? "true" : "false");
            SetNodeValue(doc, root, "ReferralDeduct", this.ReferralDeduct.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "Footer", this.Footer);
            SetNodeValue(doc, root, "RegisterAgreement", this.RegisterAgreement);
            SetNodeValue(doc, root, "HtmlOnlineServiceCode", this.HtmlOnlineServiceCode);
            SetNodeValue(doc, root, "LogoUrl", this.LogoUrl);
            SetNodeValue(doc, root, "OrderShowDays", this.OrderShowDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "CloseOrderDays", this.CloseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "ClosePurchaseOrderDays", this.ClosePurchaseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishOrderDays", this.FinishOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishPurchaseOrderDays", this.FinishPurchaseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "TaxRate", this.TaxRate.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "PointsRate", this.PointsRate.ToString("F"));
            SetNodeValue(doc, root, "SearchMetaDescription", this.SearchMetaDescription);
            SetNodeValue(doc, root, "SearchMetaKeywords", this.SearchMetaKeywords);
            SetNodeValue(doc, root, "SiteDescription", this.SiteDescription);
            SetNodeValue(doc, root, "SiteName", this.SiteName);
            SetNodeValue(doc, root, "Theme", this.Theme);
            SetNodeValue(doc, root, "YourPriceName", this.YourPriceName);
            SetNodeValue(doc, root, "DistributorRequestInstruction", this.DistributorRequestInstruction);
            SetNodeValue(doc, root, "DistributorRequestProtocols", this.DistributorRequestProtocols);
            SetNodeValue(doc, root, "EmailSender", this.EmailSender);
            SetNodeValue(doc, root, "EmailSettings", this.EmailSettings);
            SetNodeValue(doc, root, "SMSSender", this.SMSSender);
            SetNodeValue(doc, root, "SMSSettings", this.SMSSettings);
            SetNodeValue(doc, root, "SiteMapNum", this.SiteMapNum);
            SetNodeValue(doc, root, "TaobaoShippingType", this.TaobaoShippingType.ToString());
            SetNodeValue(doc, root, "SiteMapTime", this.SiteMapTime);
            SetNodeValue(doc, root, "EnabledCnzz", this.EnabledCnzz ? "true" : "false");
            SetNodeValue(doc, root, "CnzzUsername", this.CnzzUsername);
            SetNodeValue(doc, root, "CnzzPassword", this.CnzzPassword);
        }

        public string AssistantIv
        {
            
            get
            {
                return _AssistantIv;
            }
            
            set
            {
                _AssistantIv = value;
            }
        }

        public string AssistantKey
        {
            
            get
            {
                return _AssistantKey;
            }
            
            set
            {
                _AssistantKey = value;
            }
        }

        public string CheckCode
        {
            
            get
            {
                return _CheckCode;
            }
            
            set
            {
                _CheckCode = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="过期几天自动关闭订单的天数必须在1-90之间")]
        public int CloseOrderDays
        {
            
            get
            {
                return _CloseOrderDays;
            }
            
            set
            {
                _CloseOrderDays = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="过期几天自动关闭采购单的天数必须在1-90之间")]
        public int ClosePurchaseOrderDays
        {
            
            get
            {
                return _ClosePurchaseOrderDays;
            }
            
            set
            {
                _ClosePurchaseOrderDays = value;
            }
        }

        public string CnzzPassword
        {
            
            get
            {
                return _CnzzPassword;
            }
            
            set
            {
                _CnzzPassword = value;
            }
        }

        public string CnzzUsername
        {
            
            get
            {
                return _CnzzUsername;
            }
            
            set
            {
                _CnzzUsername = value;
            }
        }

        public DateTime? CreateDate
        {
            
            get
            {
                return _CreateDate;
            }
            
            set
            {
                _CreateDate = value;
            }
        }

        public int DecimalLength
        {
            
            get
            {
                return _DecimalLength;
            }
            
            set
            {
                _DecimalLength = value;
            }
        }

        public string DefaultProductImage
        {
            
            get
            {
                return _DefaultProductImage;
            }
            
            set
            {
                _DefaultProductImage = value;
            }
        }

        public string DefaultProductThumbnail1
        {
            
            get
            {
                return _DefaultProductThumbnail1;
            }
            
            set
            {
                _DefaultProductThumbnail1 = value;
            }
        }

        public string DefaultProductThumbnail2
        {
            
            get
            {
                return _DefaultProductThumbnail2;
            }
            
            set
            {
                _DefaultProductThumbnail2 = value;
            }
        }

        public string DefaultProductThumbnail3
        {
            
            get
            {
                return _DefaultProductThumbnail3;
            }
            
            set
            {
                _DefaultProductThumbnail3 = value;
            }
        }

        public string DefaultProductThumbnail4
        {
            
            get
            {
                return _DefaultProductThumbnail4;
            }
            
            set
            {
                _DefaultProductThumbnail4 = value;
            }
        }

        public string DefaultProductThumbnail5
        {
            
            get
            {
                return _DefaultProductThumbnail5;
            }
            
            set
            {
                _DefaultProductThumbnail5 = value;
            }
        }

        public string DefaultProductThumbnail6
        {
            
            get
            {
                return _DefaultProductThumbnail6;
            }
            
            set
            {
                _DefaultProductThumbnail6 = value;
            }
        }

        public string DefaultProductThumbnail7
        {
            
            get
            {
                return _DefaultProductThumbnail7;
            }
            
            set
            {
                _DefaultProductThumbnail7 = value;
            }
        }

        public string DefaultProductThumbnail8
        {
            
            get
            {
                return _DefaultProductThumbnail8;
            }
            
            set
            {
                _DefaultProductThumbnail8 = value;
            }
        }

        public bool Disabled
        {
            
            get
            {
                return _Disabled;
            }
            
            set
            {
                _Disabled = value;
            }
        }

        public string DistributorRequestInstruction
        {
            
            get
            {
                return _DistributorRequestInstruction;
            }
            
            set
            {
                _DistributorRequestInstruction = value;
            }
        }

        public string DistributorRequestProtocols
        {
            
            get
            {
                return _DistributorRequestProtocols;
            }
            
            set
            {
                _DistributorRequestProtocols = value;
            }
        }

        public bool EmailEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings)) && (this.EmailSender.Trim().Length > 0)) && (this.EmailSettings.Trim().Length > 0));
            }
        }

        public string EmailSender
        {
            
            get
            {
                return _EmailSender;
            }
            
            set
            {
                _EmailSender = value;
            }
        }

        public string EmailSettings
        {
            
            get
            {
                return _EmailSettings;
            }
            
            set
            {
                _EmailSettings = value;
            }
        }

        public bool EnabledCnzz
        {
            
            get
            {
                return _EnabledCnzz;
            }
            
            set
            {
                _EnabledCnzz = value;
            }
        }

        public DateTime? EtaoApplyTime
        {
            
            get
            {
                return _EtaoApplyTime;
            }
            
            set
            {
                _EtaoApplyTime = value;
            }
        }

        public string EtaoID
        {
            
            get
            {
                return _EtaoID;
            }
            
            set
            {
                _EtaoID = value;
            }
        }

        public int EtaoStatus
        {
            
            get
            {
                return _EtaoStatus;
            }
            
            set
            {
                _EtaoStatus = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="发货几天自动完成订单的天数必须在1-90之间")]
        public int FinishOrderDays
        {
            
            get
            {
                return _FinishOrderDays;
            }
            
            set
            {
                _FinishOrderDays = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="发货几天自动完成采购单的天数必须在1-90之间")]
        public int FinishPurchaseOrderDays
        {
            
            get
            {
                return _FinishPurchaseOrderDays;
            }
            
            set
            {
                _FinishPurchaseOrderDays = value;
            }
        }

        public string Footer
        {
            
            get
            {
                return _Footer;
            }
            
            set
            {
                _Footer = value;
            }
        }

        [StringLengthValidator(0, 0xfa0, Ruleset="ValMasterSettings", MessageTemplate="网页客服代码长度限制在4000个字符以内")]
        public string HtmlOnlineServiceCode
        {
            
            get
            {
                return _HtmlOnlineServiceCode;
            }
            
            set
            {
                _HtmlOnlineServiceCode = value;
            }
        }

        public bool IsDistributorSettings
        {
            get
            {
                return this.UserId.HasValue;
            }
        }

        public bool IsOpenEtao
        {
            
            get
            {
                return _IsOpenEtao;
            }
            
            set
            {
                _IsOpenEtao = value;
            }
        }

        public bool IsOpenSiteSale
        {
            
            get
            {
                return _IsOpenSiteSale;
            }
            
            set
            {
                _IsOpenSiteSale = value;
            }
        }

        public string LogoUrl
        {
            
            get
            {
                return _LogoUrl;
            }
            
            set
            {
                _LogoUrl = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="最近几天内订单的天数必须在1-90之间")]
        public int OrderShowDays
        {
            
            get
            {
                return _OrderShowDays;
            }
            
            set
            {
                _OrderShowDays = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.1", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="几元一积分必须在0.1-10000000之间")]
        public decimal PointsRate
        {
            
            get
            {
                return _PointsRate;
            }
            
            set
            {
                _PointsRate = value;
            }
        }

        public int ReferralDeduct
        {
            
            get
            {
                return _ReferralDeduct;
            }
            
            set
            {
                _ReferralDeduct = value;
            }
        }

        public string RegisterAgreement
        {
            
            get
            {
                return _RegisterAgreement;
            }
            
            set
            {
                _RegisterAgreement = value;
            }
        }

        public DateTime? RequestDate
        {
            
            get
            {
                return _RequestDate;
            }
            
            set
            {
                _RequestDate = value;
            }
        }

        [StringLengthValidator(0, 260, Ruleset="ValMasterSettings", MessageTemplate="店铺描述META_DESCRIPTION的长度限制在260字符以内"), HtmlCoding]
        public string SearchMetaDescription
        {
            
            get
            {
                return _SearchMetaDescription;
            }
            
            set
            {
                _SearchMetaDescription = value;
            }
        }

        [StringLengthValidator(0, 160, Ruleset="ValMasterSettings", MessageTemplate="搜索关键字META_KEYWORDS的长度限制在160字符以内")]
        public string SearchMetaKeywords
        {
            
            get
            {
                return _SearchMetaKeywords;
            }
            
            set
            {
                _SearchMetaKeywords = value;
            }
        }

        [StringLengthValidator(0, 100, Ruleset="ValMasterSettings", MessageTemplate="简单介绍TITLE的长度限制在100字符以内")]
        public string SiteDescription
        {
            
            get
            {
                return _SiteDescription;
            }
            
            set
            {
                _SiteDescription = value;
            }
        }

        public string SiteMapNum
        {
            
            get
            {
                return _SiteMapNum;
            }
            
            set
            {
                _SiteMapNum = value;
            }
        }

        public string SiteMapTime
        {
            
            get
            {
                return _SiteMapTime;
            }
            
            set
            {
                _SiteMapTime = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValMasterSettings", MessageTemplate="店铺名称为必填项，长度限制在60字符以内")]
        public string SiteName
        {
            
            get
            {
                return _SiteName;
            }
            
            set
            {
                _SiteName = value;
            }
        }

        [StringLengthValidator(1, 0x80, Ruleset="ValMasterSettings", MessageTemplate="店铺域名必须控制在128个字符以内")]
        public string SiteUrl
        {
            
            get
            {
                return _SiteUrl;
            }
            
            set
            {
                _SiteUrl = value;
            }
        }

        public bool SMSEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings)) && (this.SMSSender.Trim().Length > 0)) && (this.SMSSettings.Trim().Length > 0));
            }
        }

        public string SMSSender
        {
            
            get
            {
                return _SMSSender;
            }
            
            set
            {
                _SMSSender = value;
            }
        }

        public string SMSSettings
        {
            
            get
            {
                return _SMSSettings;
            }
            
            set
            {
                _SMSSettings = value;
            }
        }

        public int TaobaoShippingType
        {
            
            get
            {
                return _TaobaoShippingType;
            }
            
            set
            {
                _TaobaoShippingType = value;
            }
        }

        [RangeValidator(typeof(decimal), "0", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="税率必须在0-100之间")]
        public decimal TaxRate
        {
            
            get
            {
                return _TaxRate;
            }
            
            set
            {
                _TaxRate = value;
            }
        }

        public string Theme
        {
            
            get
            {
                return _Theme;
            }
            
            set
            {
                _Theme = value;
            }
        }

        public int? UserId
        {
            
            get
            {
                return _UserId;
            }
            
            private set
            {
                _UserId = value;
            }
        }

        [StringLengthValidator(0, 10, Ruleset="ValMasterSettings", MessageTemplate="“您的价”重命名的长度限制在10字符以内")]
        public string YourPriceName
        {
            
            get
            {
                return _YourPriceName;
            }
            
            set
            {
                _YourPriceName = value;
            }
        }
    }
}


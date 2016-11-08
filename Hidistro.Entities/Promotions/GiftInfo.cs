namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class GiftInfo
    {
        
        private decimal? _CostPrice;
        
        private int _GiftId;
        
        private string _ImageUrl;
        
        private bool _IsDownLoad;
        
        private bool _IsPromotion;
        
        private string _LongDescription;
        
        private decimal? _MarketPrice;
        
        private string _Meta_Description;
        
        private string _Meta_Keywords;
        
        private string _Name;
        
        private int _NeedPoint;
        
        private decimal _PurchasePrice;
        
        private string _ShortDescription;
        
        private string _ThumbnailUrl100;
        
        private string _ThumbnailUrl160;
        
        private string _ThumbnailUrl180;
        
        private string _ThumbnailUrl220;
        
        private string _ThumbnailUrl310;
        
        private string _ThumbnailUrl40;
        
        private string _ThumbnailUrl410;
        
        private string _ThumbnailUrl60;
        
        private string _Title;
        
        private string _Unit;

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValGift"), ValidatorComposition(CompositionType.Or, Ruleset="ValGift", MessageTemplate="成本价格，金额大小0.01-1000万之间"), NotNullValidator(Negated=true, Ruleset="ValGift")]
        public decimal? CostPrice
        {
            
            get
            {
                return _CostPrice;
            }
            
            set
            {
                _CostPrice = value;
            }
        }

        public int GiftId
        {
            
            get
            {
                return _GiftId;
            }
            
            set
            {
                _GiftId = value;
            }
        }

        public string ImageUrl
        {
            
            get
            {
                return _ImageUrl;
            }
            
            set
            {
                _ImageUrl = value;
            }
        }

        public bool IsDownLoad
        {
            
            get
            {
                return _IsDownLoad;
            }
            
            set
            {
                _IsDownLoad = value;
            }
        }

        public bool IsPromotion
        {
            
            get
            {
                return _IsPromotion;
            }
            
            set
            {
                _IsPromotion = value;
            }
        }

        public string LongDescription
        {
            
            get
            {
                return _LongDescription;
            }
            
            set
            {
                _LongDescription = value;
            }
        }

        [NotNullValidator(Negated=true, Ruleset="ValGift"), RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValGift"), ValidatorComposition(CompositionType.Or, Ruleset="ValGift", MessageTemplate="市场参考价格，金额大小0.01-1000万之间")]
        public decimal? MarketPrice
        {
            
            get
            {
                return _MarketPrice;
            }
            
            set
            {
                _MarketPrice = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 260, Ruleset="ValGift", MessageTemplate="详细页描述长度限制在0-260个字符之间")]
        public string Meta_Description
        {
            
            get
            {
                return _Meta_Description;
            }
            
            set
            {
                _Meta_Description = value;
            }
        }

        [StringLengthValidator(0, 160, Ruleset="ValGift", MessageTemplate="详细页关键字长度限制在0-160个字符之间"), HtmlCoding]
        public string Meta_Keywords
        {
            
            get
            {
                return _Meta_Keywords;
            }
            
            set
            {
                _Meta_Keywords = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValGift", MessageTemplate="礼品名称不能为空，长度限制在1-60个字符之间"), HtmlCoding]
        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            set
            {
                _Name = value;
            }
        }

        [RangeValidator(0, RangeBoundaryType.Inclusive, 0x2710, RangeBoundaryType.Inclusive, Ruleset="ValGift", MessageTemplate="兑换所需积分不能为空，大小0-10000之间")]
        public int NeedPoint
        {
            
            get
            {
                return _NeedPoint;
            }
            
            set
            {
                _NeedPoint = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValGift", MessageTemplate="采购价格，金额大小0.01-1000万之间")]
        public decimal PurchasePrice
        {
            
            get
            {
                return _PurchasePrice;
            }
            
            set
            {
                _PurchasePrice = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 300, Ruleset="ValGift", MessageTemplate="礼品简单介绍长度限制在0-300个字符之间")]
        public string ShortDescription
        {
            
            get
            {
                return _ShortDescription;
            }
            
            set
            {
                _ShortDescription = value;
            }
        }

        public string ThumbnailUrl100
        {
            
            get
            {
                return _ThumbnailUrl100;
            }
            
            set
            {
                _ThumbnailUrl100 = value;
            }
        }

        public string ThumbnailUrl160
        {
            
            get
            {
                return _ThumbnailUrl160;
            }
            
            set
            {
                _ThumbnailUrl160 = value;
            }
        }

        public string ThumbnailUrl180
        {
            
            get
            {
                return _ThumbnailUrl180;
            }
            
            set
            {
                _ThumbnailUrl180 = value;
            }
        }

        public string ThumbnailUrl220
        {
            
            get
            {
                return _ThumbnailUrl220;
            }
            
            set
            {
                _ThumbnailUrl220 = value;
            }
        }

        public string ThumbnailUrl310
        {
            
            get
            {
                return _ThumbnailUrl310;
            }
            
            set
            {
                _ThumbnailUrl310 = value;
            }
        }

        public string ThumbnailUrl40
        {
            
            get
            {
                return _ThumbnailUrl40;
            }
            
            set
            {
                _ThumbnailUrl40 = value;
            }
        }

        public string ThumbnailUrl410
        {
            
            get
            {
                return _ThumbnailUrl410;
            }
            
            set
            {
                _ThumbnailUrl410 = value;
            }
        }

        public string ThumbnailUrl60
        {
            
            get
            {
                return _ThumbnailUrl60;
            }
            
            set
            {
                _ThumbnailUrl60 = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValGift", MessageTemplate="详细页标题长度限制在0-100个字符之间")]
        public string Title
        {
            
            get
            {
                return _Title;
            }
            
            set
            {
                _Title = value;
            }
        }

        [StringLengthValidator(0, 10, Ruleset="ValGift", MessageTemplate="计量单位长度限制在0-10个字符之间")]
        public string Unit
        {
            
            get
            {
                return _Unit;
            }
            
            set
            {
                _Unit = value;
            }
        }
    }
}


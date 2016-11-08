namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PaymentModeInfo
    {
        
        private decimal _Charge;
        
        private string _Description;
        
        private int _DisplaySequence;
        
        private string _Gateway;
        
        private bool _IsPercent;
        
        private bool _IsUseInDistributor;
        
        private bool _IsUseInpour;
        
        private int _ModeId;
        
        private string _Name;
        
        private string _Settings;

        public decimal CalcPayCharge(decimal cartMoney)
        {
            if (!this.IsPercent)
            {
                return this.Charge;
            }
            return (cartMoney * (this.Charge / 100M));
        }

        public decimal Charge
        {
            
            get
            {
                return _Charge;
            }
            
            set
            {
                _Charge = value;
            }
        }

        public string Description
        {
            
            get
            {
                return _Description;
            }
            
            set
            {
                _Description = value;
            }
        }

        public int DisplaySequence
        {
            
            get
            {
                return _DisplaySequence;
            }
            
            set
            {
                _DisplaySequence = value;
            }
        }

        public string Gateway
        {
            
            get
            {
                return _Gateway;
            }
            
            set
            {
                _Gateway = value;
            }
        }

        public bool IsPercent
        {
            
            get
            {
                return _IsPercent;
            }
            
            set
            {
                _IsPercent = value;
            }
        }

        public bool IsUseInDistributor
        {
            
            get
            {
                return _IsUseInDistributor;
            }
            
            set
            {
                _IsUseInDistributor = value;
            }
        }

        public bool IsUseInpour
        {
            
            get
            {
                return _IsUseInpour;
            }
            
            set
            {
                _IsUseInpour = value;
            }
        }

        public int ModeId
        {
            
            get
            {
                return _ModeId;
            }
            
            set
            {
                _ModeId = value;
            }
        }

        [HtmlCoding]
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

        public string Settings
        {
            
            get
            {
                return _Settings;
            }
            
            set
            {
                _Settings = value;
            }
        }
    }
}


namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    public class PurchaseOrderTaobaoInfo
    {
        
        private string _created;
        
        private string _expire_time;
        
        private string _is_delivery;
        
        private string _isPart;
        
        private string _logi_name;
        
        private string _login_no;
        
        private string _order_id;
        
        private string _status;
        
        private string _time;

        public PurchaseOrderTaobaoInfo()
        {
            this.created = "false";
            this.expire_time = "0";
            this.isPart = "false";
            this.is_delivery = "false";
            this.logi_name = "";
            this.login_no = "";
            this.order_id = "";
            this.status = "未下单";
            this.time = "";
        }

        public string ToJson()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"order_id\":\"");
            builder.Append(this.order_id);
            builder.Append("\",\"created\":\"");
            builder.Append(this.created);
            builder.Append("\",\"expire_time\":\"");
            builder.Append(this.expire_time);
            builder.Append("\",\"isPart\":\"");
            builder.Append(this.isPart);
            builder.Append("\",\"is_delivery\":\"");
            builder.Append(this.is_delivery);
            builder.Append("\",\"logi_name\":\"");
            builder.Append(this.logi_name);
            builder.Append("\",\"login_no\":\"");
            builder.Append(this.login_no);
            builder.Append("\",\"status\":\"");
            builder.Append(this.status);
            builder.Append("\",\"time\":\"");
            builder.Append(this.time);
            builder.Append("\"}");
            return builder.ToString();
        }

        [DataMember]
        public string created
        {
            
            get
            {
                return _created;
            }
            
            set
            {
                _created = value;
            }
        }

        [DataMember]
        public string expire_time
        {
            
            get
            {
                return _expire_time;
            }
            
            set
            {
                _expire_time = value;
            }
        }

        [DataMember]
        public string is_delivery
        {
            
            get
            {
                return _is_delivery;
            }
            
            set
            {
                _is_delivery = value;
            }
        }

        [DataMember]
        public string isPart
        {
            
            get
            {
                return _isPart;
            }
            
            set
            {
                _isPart = value;
            }
        }

        [DataMember]
        public string logi_name
        {
            
            get
            {
                return _logi_name;
            }
            
            set
            {
                _logi_name = value;
            }
        }

        [DataMember]
        public string login_no
        {
            
            get
            {
                return _login_no;
            }
            
            set
            {
                _login_no = value;
            }
        }

        [DataMember]
        public string order_id
        {
            
            get
            {
                return _order_id;
            }
            
            set
            {
                _order_id = value;
            }
        }

        [DataMember]
        public string status
        {
            
            get
            {
                return _status;
            }
            
            set
            {
                _status = value;
            }
        }

        [DataMember]
        public string time
        {
            
            get
            {
                return _time;
            }
            
            set
            {
                _time = value;
            }
        }
    }
}


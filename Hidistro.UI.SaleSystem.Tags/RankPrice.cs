namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class RankPrice : Label
    {
        private string classLogin = string.Empty;
        private string classNoLogin = string.Empty;
        private string priceLogin = string.Empty;
        private string priceNoLogin = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                base.Text = this.PriceNoLogin;
                base.CssClass = this.classNoLogin;
            }
            else
            {
                decimal num;
                if (decimal.TryParse(this.PriceLogin, out num))
                {
                    base.Text = Globals.FormatMoney(num);
                    base.CssClass = this.ClassLogin;
                }
            }
            base.Render(writer);
        }

        public string ClassLogin
        {
            get
            {
                return this.classLogin;
            }
            set
            {
                this.classLogin = value;
            }
        }

        public string ClassNoLogin
        {
            get
            {
                return this.classNoLogin;
            }
            set
            {
                this.classNoLogin = value;
            }
        }

        public string PriceLogin
        {
            get
            {
                return this.priceLogin;
            }
            set
            {
                this.priceLogin = value;
            }
        }

        public string PriceNoLogin
        {
            get
            {
                return this.priceNoLogin;
            }
            set
            {
                this.priceNoLogin = value;
            }
        }
    }
}


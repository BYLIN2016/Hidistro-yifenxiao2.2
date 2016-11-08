namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_ShoppingCart_PromoGiftList : AscxTemplatedWebControl
    {
        private RepeaterCommandEventHandler ItemCommand;
        private Literal lit_promonum;
        private Literal lit_promosumnum;
        private Panel pnlPromoGift;
        private Repeater rp_promogift;
        public const string TagID = "Common_ShoppingCart_PromoGiftList";

        public event RepeaterCommandEventHandler _ItemCommand
        {
            add
            {
                RepeaterCommandEventHandler handler2;
                RepeaterCommandEventHandler itemCommand = this.ItemCommand;
                do
                {
                    handler2 = itemCommand;
                    RepeaterCommandEventHandler handler3 = (RepeaterCommandEventHandler) Delegate.Combine(handler2, value);
                    itemCommand = Interlocked.CompareExchange<RepeaterCommandEventHandler>(ref this.ItemCommand, handler3, handler2);
                }
                while (itemCommand != handler2);
            }
            remove
            {
                RepeaterCommandEventHandler handler2;
                RepeaterCommandEventHandler itemCommand = this.ItemCommand;
                do
                {
                    handler2 = itemCommand;
                    RepeaterCommandEventHandler handler3 = (RepeaterCommandEventHandler) Delegate.Remove(handler2, value);
                    itemCommand = Interlocked.CompareExchange<RepeaterCommandEventHandler>(ref this.ItemCommand, handler3, handler2);
                }
                while (itemCommand != handler2);
            }
        }

        public Common_ShoppingCart_PromoGiftList()
        {
            base.ID = "Common_ShoppingCart_PromoGiftList";
        }

        protected override void AttachChildControls()
        {
            this.rp_promogift = (Repeater) this.FindControl("rp_promogift");
            this.pnlPromoGift = (Panel) this.FindControl("pnlPromoGift");
            this.lit_promonum = (Literal) this.FindControl("lit_promonum");
            this.lit_promosumnum = (Literal) this.FindControl("lit_promosumnum");
            this.pnlPromoGift.Visible = false;
            this.rp_promogift.ItemCommand += new RepeaterCommandEventHandler(this.rp_promogift_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.rp_promogift.DataSource != null)
            {
                this.rp_promogift.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_PromoGiftList.ascx";
            }
            base.OnInit(e);
        }

        private void rp_promogift_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        public void ShowPromoGift(int pronum, int sumnum)
        {
            if (this.DataSource == null)
            {
                this.pnlPromoGift.Visible = false;
            }
            else
            {
                this.pnlPromoGift.Visible = true;
                this.lit_promonum.Text = pronum.ToString();
                this.lit_promosumnum.Text = sumnum.ToString();
            }
        }

        public int CurentNum
        {
            get
            {
                return int.Parse(this.lit_promonum.Text);
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.rp_promogift.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.rp_promogift.DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public int SumNum
        {
            get
            {
                return int.Parse(this.lit_promosumnum.Text);
            }
        }
    }
}


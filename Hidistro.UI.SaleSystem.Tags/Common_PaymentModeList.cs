namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_PaymentModeList : AscxTemplatedWebControl
    {
        private GridView grdPayment;
        public const string TagID = "grd_Common_PaymentModeList";

        public Common_PaymentModeList()
        {
            base.ID = "grd_Common_PaymentModeList";
        }

        protected override void AttachChildControls()
        {
            this.grdPayment = (GridView) this.FindControl("grdPayment");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.grdPayment.DataSource = this.DataSource;
            this.grdPayment.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_PaymentModeList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.grdPayment.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.grdPayment.DataSource = value;
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
    }
}


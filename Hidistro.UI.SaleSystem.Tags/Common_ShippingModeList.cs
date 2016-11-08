namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_ShippingModeList : AscxTemplatedWebControl
    {
        private GridView grdShippingMode;
        public const string TagID = "Common_ShippingModeList";

        public Common_ShippingModeList()
        {
            base.ID = "Common_ShippingModeList";
        }

        protected override void AttachChildControls()
        {
            this.grdShippingMode = (GridView) this.FindControl("grdShippingMode");
            this.grdShippingMode.RowDataBound += new GridViewRowEventHandler(this.grdShippingMode_RowDataBound);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.grdShippingMode.DataSource = this.DataSource;
            this.grdShippingMode.DataBind();
        }

        private void grdShippingMode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int modeId = (int) this.grdShippingMode.DataKeys[e.Row.RowIndex].Value;
                Literal literal = e.Row.FindControl("litExpressCompanyName") as Literal;
                IList<string> expressCompanysByMode = ShoppingProcessor.GetExpressCompanysByMode(modeId);
                string str = string.Empty;
                foreach (string str2 in expressCompanysByMode)
                {
                    str = str + str2 + "，";
                }
                literal.Text = str.Remove(str.Length - 1);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_ShippingModeList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.grdShippingMode.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.grdShippingMode.DataSource = value;
            }
        }
    }
}


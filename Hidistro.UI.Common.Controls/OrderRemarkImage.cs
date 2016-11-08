namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderRemarkImage : Literal
    {
        private string dataField;
        private string imageFormat = "<img border=\"0\" src=\"{0}\"  />";

        protected string GetImageSrc(object managerMark)
        {
            string str = Globals.ApplicationPath + "/Admin/images/";
            switch (((OrderMark) managerMark))
            {
                case OrderMark.Draw:
                    return (str + "iconaf.gif");

                case OrderMark.ExclamationMark:
                    return (str + "iconb.gif");

                case OrderMark.Red:
                    return (str + "iconc.gif");

                case OrderMark.Green:
                    return (str + "icona.gif");

                case OrderMark.Yellow:
                    return (str + "iconad.gif");

                case OrderMark.Gray:
                    return (str + "iconae.gif");
            }
            return string.Format(this.imageFormat, Globals.ApplicationPath + "/Admin/images/xi.gif");
        }

        protected override void OnDataBinding(EventArgs e)
        {
            object managerMark = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
            if ((managerMark != null) && (managerMark != DBNull.Value))
            {
                base.Text = string.Format(this.imageFormat, this.GetImageSrc(managerMark));
            }
            else
            {
                base.Text = string.Format(this.imageFormat, Globals.ApplicationPath + "/Admin/images/xi.gif");
            }
            base.OnDataBinding(e);
        }

        public string DataField
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }
    }
}


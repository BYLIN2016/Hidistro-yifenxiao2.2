namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    using Hidistro.ControlPanel.Sales;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class PrintPurchaseComplete : Page
    {
        protected HtmlForm form1;
        protected string script;

        protected void Page_Load(object sender, EventArgs e)
        {
            string startNumber = base.Request["mailNo"];
            string[] orderIds = base.Request["orderIds"].Split(new char[] { ',' });
            List<string> list = new List<string>();
            foreach (string str2 in orderIds)
            {
                list.Add("'" + str2 + "'");
            }
            SalesHelper.SetPurchaseOrderExpressComputerpe(string.Join(",", list.ToArray()), base.Request["templateName"], base.Request["templateName"]);
            SalesHelper.SetPurchaseOrderShipNumber(orderIds, startNumber);
            SalesHelper.SetPurchaseOrderPrinted(orderIds, true);
        }
    }
}


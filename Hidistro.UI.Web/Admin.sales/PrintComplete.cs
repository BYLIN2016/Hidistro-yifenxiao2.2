namespace Hidistro.UI.Web.Admin.sales
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;

    public class PrintComplete : AdminPage
    {
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
            OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), base.Request["templateName"], base.Request["templateName"]);
            OrderHelper.SetOrderShipNumber(orderIds, startNumber);
            OrderHelper.SetOrderPrinted(orderIds, true);
        }
    }
}


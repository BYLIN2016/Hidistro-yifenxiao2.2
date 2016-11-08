namespace Hidistro.Jobs
{
    using Hidistro.Core.Jobs;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Xml;

    public class CartJob : IJob
    {
        public void Execute(XmlNode node)
        {
            int result = 5;
            XmlAttribute attribute = node.Attributes["expires"];
            if (attribute != null)
            {
                int.TryParse(attribute.Value, out result);
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_ShoppingCarts WHERE AddTime <= @CurrentTime;DELETE FROM Hishop_ShoppingCarts WHERE AddTime <= @CurrentTime;DELETE FROM Hishop_PurchaseShoppingCarts WHERE AddTime <= @CurrentTime;");
            database.AddInParameter(sqlStringCommand, "CurrentTime", DbType.DateTime, DateTime.Now.AddDays((double) -result));
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}


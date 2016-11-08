namespace Hidistro.Jobs
{
    using Hidistro.Core.Jobs;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Xml;

    public class OrderJob : IJob
    {
        public void Execute(XmlNode node)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='过期没付款，自动关闭' WHERE OrderStatus=1 AND OrderDate <= @OrderDate; UPDATE Hishop_PurchaseOrders SET PurchaseStatus=4,CloseReason='过期没付款，自动关闭' WHERE PurchaseStatus=1 AND PurchaseDate <= @PurchaseDate; UPDATE distro_Orders SET OrderStatus=4,CloseReason='过期没付款，自动关闭' WHERE OrderStatus=1 AND OrderDate <= @OrderDate; UPDATE Hishop_Orders SET FinishDate = getdate(), OrderStatus = 5 WHERE OrderStatus=3 AND ShippingDate <= @ShippingDate; UPDATE Hishop_PurchaseOrders SET  PurchaseStatus = 5, FinishDate=getdate() WHERE PurchaseStatus=3 AND ShippingDate <= @PurchaseShippingDate; UPDATE distro_Orders SET OrderStatus=5,FinishDate=getdate() WHERE OrderStatus=3 AND ShippingDate <= @ShippingDate");
            database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double) -masterSettings.CloseOrderDays));
            database.AddInParameter(sqlStringCommand, "PurchaseDate", DbType.DateTime, DateTime.Now.AddDays((double) -masterSettings.ClosePurchaseOrderDays));
            database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now.AddDays((double) -masterSettings.FinishOrderDays));
            database.AddInParameter(sqlStringCommand, "PurchaseShippingDate", DbType.DateTime, DateTime.Now.AddDays((double) -masterSettings.FinishPurchaseOrderDays));
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}


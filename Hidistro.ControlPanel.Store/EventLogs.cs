namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;

    public static class EventLogs
    {
        public static bool DeleteAllLogs()
        {
            return StoreProvider.Instance().DeleteAllLogs();
        }

        public static bool DeleteLog(long logId)
        {
            return StoreProvider.Instance().DeleteLog(logId);
        }

        public static int DeleteLogs(string strIds)
        {
            return StoreProvider.Instance().DeleteLogs(strIds);
        }

        public static DbQueryResult GetLogs(OperationLogQuery query)
        {
            return StoreProvider.Instance().GetLogs(query);
        }

        public static IList<string> GetOperationUseNames()
        {
            return StoreProvider.Instance().GetOperationUserNames();
        }

        public static void WriteOperationLog(Privilege privilege, string description)
        {
            OperationLogEntry entry2 = new OperationLogEntry();
            entry2.AddedTime = DateTime.Now;
            entry2.Privilege = privilege;
            entry2.Description = description;
            entry2.IpAddress = Globals.IPAddress;
            entry2.PageUrl = HiContext.Current.Context.Request.RawUrl;
            entry2.UserName = HiContext.Current.Context.User.Identity.Name;
            OperationLogEntry entry = entry2;
            StoreProvider.Instance().WriteOperationLogEntry(entry);
        }
    }
}


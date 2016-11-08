namespace Hidistro.ControlPanel.Data
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class OpenIdData : OpenIdProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override void DeleteSettings(string openIdType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
            this.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType.ToLower());
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override IList<string> GetConfigedTypes()
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OpenIdType FROM aspnet_OpenIdSettings");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            return list;
        }

        public override OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
        {
            OpenIdSettingsInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
            this.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType.ToLower());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                OpenIdSettingsInfo info2 = new OpenIdSettingsInfo();
                info2.OpenIdType = openIdType;
                info2.Name = (string) reader["Name"];
                info2.Settings = (string) reader["Settings"];
                info = info2;
                if (reader["Description"] != DBNull.Value)
                {
                    info.Description = (string) reader["Description"];
                }
            }
            return info;
        }

        public override void SaveSettings(OpenIdSettingsInfo settings)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_OpenIdSettings_Save");
            this.database.AddInParameter(storedProcCommand, "OpenIdType", DbType.String, settings.OpenIdType.ToLower());
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, settings.Name);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, settings.Description);
            this.database.AddInParameter(storedProcCommand, "Settings", DbType.String, settings.Settings);
            this.database.ExecuteNonQuery(storedProcCommand);
        }
    }
}


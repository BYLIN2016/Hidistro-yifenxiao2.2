namespace Hidistro.SaleSystem.Data
{
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Member;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class MemberData : MemberMasterProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override IList<OpenIdSettingsInfo> GetConfigedItems()
        {
            IList<OpenIdSettingsInfo> list = new List<OpenIdSettingsInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    OpenIdSettingsInfo item = MemberProvider.PopulateOpenIdSettings(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override int GetDefaultMemberGrade()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT GradeId FROM aspnet_MemberGrades WHERE IsDefault = 1");
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override int GetMemberDiscount(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
        {
            OpenIdSettingsInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
            this.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType.ToLower());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = MemberProvider.PopulateOpenIdSettings(reader);
                }
            }
            return info;
        }

        public override ShippingAddressInfo GetShippingAddress(int shippingId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE ShippingId = @ShippingId");
            this.database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, shippingId);
            ShippingAddressInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingAddress(reader);
                }
            }
            return info;
        }

        public override IList<ShippingAddressInfo> GetShippingAddresses(int userId)
        {
            IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingAddress(reader));
                }
            }
            return list;
        }
    }
}


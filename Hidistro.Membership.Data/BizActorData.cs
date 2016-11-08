namespace Hidistro.Membership.Data
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Web.Security;

    public class BizActorData : BizActorProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool ChangeDistributorTradePassword(string username, string oldPassword, string newPassword)
        {
            return this.ChangeTradePassword("aspnet_Distributors", username, oldPassword, newPassword);
        }

        public override bool ChangeMemberTradePassword(string username, string oldPassword, string newPassword)
        {
            return this.ChangeTradePassword("aspnet_Members", username, oldPassword, newPassword);
        }

        private bool ChangeTradePassword(string tableName, string username, string oldPassword, string newPassword)
        {
            string str;
            int num;
            if (!this.CheckTradePassword(tableName, username, oldPassword, out str, out num))
            {
                return false;
            }
            MembershipProvider provider = Membership.Provider;
            if ((newPassword.Length < provider.MinRequiredPasswordLength) || (newPassword.Length > 0x80))
            {
                return false;
            }
            int num2 = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                {
                    num2++;
                }
            }
            if (num2 < provider.MinRequiredNonAlphanumericCharacters)
            {
                return false;
            }
            if ((provider.PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(newPassword, provider.PasswordStrengthRegularExpression))
            {
                return false;
            }
            string str2 = UserHelper.EncodePassword((MembershipPasswordFormat) num, newPassword, str);
            if (str2.Length > 0x80)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE " + tableName + " SET TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
            this.database.AddInParameter(sqlStringCommand, "TradePassword", DbType.String, str2);
            this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", DbType.String, str);
            this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", DbType.Int32, num);
            this.database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ChangeUnderlingTradePassword(string username, string oldPassword, string newPassword)
        {
            return this.ChangeTradePassword("distro_Members", username, oldPassword, newPassword);
        }

        private bool CheckTradePassword(string tableName, string username, string password)
        {
            string str;
            int num;
            return this.CheckTradePassword(tableName, username, password, out str, out num);
        }

        private bool CheckTradePassword(string tableName, string username, string password, out string salt, out int passwordFormat)
        {
            string str;
            bool flag;
            this.GetPasswordWithFormat(tableName, username, out flag, out passwordFormat, out salt, out str);
            if (!flag)
            {
                return false;
            }
            string str2 = UserHelper.EncodePassword((MembershipPasswordFormat) passwordFormat, password, salt);
            return str.Equals(str2);
        }

        public override bool CreateDistributor(Distributor distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Distributors (UserId, GradeId, TradePassword, TradePasswordSalt, TradePasswordFormat, PurchaseOrder, Expenditure, Balance, TopRegionId, RegionId, RealName,CompanyName, Address, Zipcode, TelPhone, CellPhone, QQ, Wangwang, MSN, Remark) VALUES (@UserId, @GradeId, @TradePassword, @TradePasswordSalt, @TradePasswordFormat, @PurchaseOrder, @Expenditure, @Balance, @TopRegionId, @RegionId, @RealName,@CompanyName, @Address, @Zipcode, @TelPhone, @CellPhone, @QQ, @Wangwang, @MSN, @Remark)");
            string salt = UserHelper.CreateSalt();
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, distributor.GradeId);
            this.database.AddInParameter(sqlStringCommand, "TradePassword", DbType.String, UserHelper.EncodePassword(distributor.TradePasswordFormat, distributor.TradePassword, salt));
            this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", DbType.String, salt);
            this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", DbType.Int32, distributor.TradePasswordFormat);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrder", DbType.Int32, distributor.PurchaseOrder);
            this.database.AddInParameter(sqlStringCommand, "Expenditure", DbType.Currency, distributor.Expenditure);
            this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, distributor.Balance);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, distributor.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, distributor.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, distributor.RealName);
            this.database.AddInParameter(sqlStringCommand, "CompanyName", DbType.String, distributor.CompanyName);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, distributor.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, distributor.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, distributor.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, distributor.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, distributor.QQ);
            this.database.AddInParameter(sqlStringCommand, "Wangwang", DbType.String, distributor.Wangwang);
            this.database.AddInParameter(sqlStringCommand, "MSN", DbType.String, distributor.MSN);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, distributor.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool CreateManager(SiteManager manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (UserId) VALUES (@UserId)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, manager.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool CreateMember(Member member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Members (UserId, GradeId,ReferralUserId, TradePassword, TradePasswordSalt, TradePasswordFormat, OrderNumber, Expenditure, Points, Balance, TopRegionId, RegionId, RealName, Address, Zipcode, TelPhone, CellPhone, QQ, Wangwang, MSN) VALUES (@UserId, @GradeId, @ReferralUserId, @TradePassword, @TradePasswordSalt, @TradePasswordFormat, @OrderNumber, @Expenditure, @Points, @Balance, @TopRegionId, @RegionId, @RealName, @Address, @Zipcode, @TelPhone, @CellPhone, @QQ, @Wangwang, @MSN)");
            string salt = UserHelper.CreateSalt();
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, member.ReferralUserId);
            this.database.AddInParameter(sqlStringCommand, "TradePassword", DbType.String, UserHelper.EncodePassword(member.TradePasswordFormat, member.TradePassword, salt));
            this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", DbType.String, salt);
            this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", DbType.Int32, member.TradePasswordFormat);
            this.database.AddInParameter(sqlStringCommand, "OrderNumber", DbType.Int32, member.OrderNumber);
            this.database.AddInParameter(sqlStringCommand, "Expenditure", DbType.Currency, member.Expenditure);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, member.Points);
            this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, member.Balance);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, member.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, member.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, member.RealName);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, member.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, member.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, member.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, member.QQ);
            this.database.AddInParameter(sqlStringCommand, "Wangwang", DbType.String, member.Wangwang);
            this.database.AddInParameter(sqlStringCommand, "MSN", DbType.String, member.MSN);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool CreateUnderling(Member underling)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Members (UserId, GradeId, ParentUserId, ReferralUserId, TradePassword, TradePasswordSalt, TradePasswordFormat, OrderNumber, Expenditure, Points, Balance, TopRegionId, RegionId, RealName, Address, Zipcode, TelPhone, CellPhone, QQ, Wangwang,  MSN) VALUES (@UserId, @GradeId, @ParentUserId, @ReferralUserId, @TradePassword, @TradePasswordSalt, @TradePasswordFormat, @OrderNumber, @Expenditure, @Points, @Balance, @TopRegionId, @RegionId, @RealName, @Address, @Zipcode, @TelPhone, @CellPhone, @QQ, @Wangwang, @MSN)");
            string salt = UserHelper.CreateSalt();
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, underling.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, underling.GradeId);
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, underling.ParentUserId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, underling.ReferralUserId);
            this.database.AddInParameter(sqlStringCommand, "TradePassword", DbType.String, UserHelper.EncodePassword(underling.TradePasswordFormat, underling.TradePassword, salt));
            this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", DbType.String, salt);
            this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", DbType.Int32, underling.TradePasswordFormat);
            this.database.AddInParameter(sqlStringCommand, "OrderNumber", DbType.Int32, underling.OrderNumber);
            this.database.AddInParameter(sqlStringCommand, "Expenditure", DbType.Currency, underling.Expenditure);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, underling.Points);
            this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, underling.Balance);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, underling.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, underling.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, underling.RealName);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, underling.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, underling.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, underling.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, underling.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, underling.QQ);
            this.database.AddInParameter(sqlStringCommand, "Wangwang", DbType.String, underling.Wangwang);
            this.database.AddInParameter(sqlStringCommand, "MSN", DbType.String, underling.MSN);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override Distributor GetDistributor(HiMembershipUser membershipUser)
        {
            Distributor distributor = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Distributors WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, membershipUser.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return distributor;
                }
                distributor = new Distributor(membershipUser);
                distributor.GradeId = (int) reader["GradeId"];
                distributor.TradePassword = (string) reader["TradePassword"];
                distributor.TradePasswordFormat = (MembershipPasswordFormat) ((int) reader["TradePasswordFormat"]);
                distributor.PurchaseOrder = (int) reader["PurchaseOrder"];
                distributor.Expenditure = (decimal) reader["Expenditure"];
                distributor.Balance = (decimal) reader["Balance"];
                distributor.RequestBalance = (decimal) reader["RequestBalance"];
                distributor.MemberCount = (int) reader["MemberCount"];
                if (reader["TopRegionId"] != DBNull.Value)
                {
                    distributor.TopRegionId = (int) reader["TopRegionId"];
                }
                if (reader["RegionId"] != DBNull.Value)
                {
                    distributor.RegionId = (int) reader["RegionId"];
                }
                if (reader["RealName"] != DBNull.Value)
                {
                    distributor.RealName = (string) reader["RealName"];
                }
                if (reader["CompanyName"] != DBNull.Value)
                {
                    distributor.CompanyName = (string) reader["CompanyName"];
                }
                if (reader["Address"] != DBNull.Value)
                {
                    distributor.Address = (string) reader["Address"];
                }
                if (reader["Zipcode"] != DBNull.Value)
                {
                    distributor.Zipcode = (string) reader["Zipcode"];
                }
                if (reader["TelPhone"] != DBNull.Value)
                {
                    distributor.TelPhone = (string) reader["TelPhone"];
                }
                if (reader["CellPhone"] != DBNull.Value)
                {
                    distributor.CellPhone = (string) reader["CellPhone"];
                }
                if (reader["QQ"] != DBNull.Value)
                {
                    distributor.QQ = (string) reader["QQ"];
                }
                if (reader["Wangwang"] != DBNull.Value)
                {
                    distributor.Wangwang = (string) reader["Wangwang"];
                }
                if (reader["MSN"] != DBNull.Value)
                {
                    distributor.MSN = (string) reader["MSN"];
                }
                if (reader["Remark"] != DBNull.Value)
                {
                    distributor.Remark = (string) reader["Remark"];
                }
            }
            return distributor;
        }

        public override SiteManager GetManager(HiMembershipUser membershipUser)
        {
            SiteManager manager = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(UserId) FROM aspnet_Managers WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, membershipUser.UserId);
            if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) == 1)
            {
                manager = new SiteManager(membershipUser);
            }
            return manager;
        }

        public override Member GetMember(HiMembershipUser membershipUser)
        {
            Member member = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, membershipUser.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return member;
                }
                member = new Member(UserRole.Member, membershipUser);
                member.GradeId = (int) reader["GradeId"];
                if (reader["ReferralUserId"] != DBNull.Value)
                {
                    member.ReferralUserId = new int?((int) reader["ReferralUserId"]);
                }
                member.IsOpenBalance = (bool) reader["IsOpenBalance"];
                member.TradePassword = (string) reader["TradePassword"];
                member.TradePasswordFormat = (MembershipPasswordFormat) ((int) reader["TradePasswordFormat"]);
                member.OrderNumber = (int) reader["OrderNumber"];
                member.Expenditure = (decimal) reader["Expenditure"];
                member.Points = (int) reader["Points"];
                member.Balance = (decimal) reader["Balance"];
                member.RequestBalance = (decimal) reader["RequestBalance"];
                if (reader["TopRegionId"] != DBNull.Value)
                {
                    member.TopRegionId = (int) reader["TopRegionId"];
                }
                if (reader["RegionId"] != DBNull.Value)
                {
                    member.RegionId = (int) reader["RegionId"];
                }
                if (reader["RealName"] != DBNull.Value)
                {
                    member.RealName = (string) reader["RealName"];
                }
                if (reader["Address"] != DBNull.Value)
                {
                    member.Address = (string) reader["Address"];
                }
                if (reader["Zipcode"] != DBNull.Value)
                {
                    member.Zipcode = (string) reader["Zipcode"];
                }
                if (reader["TelPhone"] != DBNull.Value)
                {
                    member.TelPhone = (string) reader["TelPhone"];
                }
                if (reader["CellPhone"] != DBNull.Value)
                {
                    member.CellPhone = (string) reader["CellPhone"];
                }
                if (reader["QQ"] != DBNull.Value)
                {
                    member.QQ = (string) reader["QQ"];
                }
                if (reader["Wangwang"] != DBNull.Value)
                {
                    member.Wangwang = (string) reader["Wangwang"];
                }
                if (reader["MSN"] != DBNull.Value)
                {
                    member.MSN = (string) reader["MSN"];
                }
            }
            return member;
        }

        private void GetPasswordWithFormat(string tableName, string username, out bool success, out int passwordFormat, out string passwordSalt, out string passwordFromDb)
        {
            passwordFormat = 0;
            passwordSalt = null;
            passwordFromDb = null;
            success = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT biz.TradePasswordFormat, biz.TradePasswordSalt, biz.TradePassword FROM " + tableName + " AS biz INNER JOIN aspnet_Users AS u ON biz.UserId = u.UserId WHERE u.LoweredUserName = LOWER(@Username)");
            this.database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    passwordFormat = reader.GetInt32(0);
                    passwordSalt = reader.GetString(1);
                    passwordFromDb = reader.GetString(2);
                    success = true;
                }
            }
        }

        public override Member GetUnderling(HiMembershipUser membershipUser)
        {
            Member member = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Members WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, membershipUser.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return member;
                }
                member = new Member(UserRole.Underling, membershipUser);
                member.GradeId = (int) reader["GradeId"];
                member.ParentUserId = new int?((int) reader["ParentUserId"]);
                if (reader["ReferralUserId"] != DBNull.Value)
                {
                    member.ReferralUserId = new int?((int) reader["ReferralUserId"]);
                }
                member.IsOpenBalance = (bool) reader["IsOpenBalance"];
                member.TradePassword = (string) reader["TradePassword"];
                member.TradePasswordFormat = (MembershipPasswordFormat) ((int) reader["TradePasswordFormat"]);
                member.OrderNumber = (int) reader["OrderNumber"];
                member.Expenditure = (decimal) reader["Expenditure"];
                member.Points = (int) reader["Points"];
                member.Balance = (decimal) reader["Balance"];
                member.RequestBalance = (decimal) reader["RequestBalance"];
                if (reader["TopRegionId"] != DBNull.Value)
                {
                    member.TopRegionId = (int) reader["TopRegionId"];
                }
                if (reader["RegionId"] != DBNull.Value)
                {
                    member.RegionId = (int) reader["RegionId"];
                }
                if (reader["RealName"] != DBNull.Value)
                {
                    member.RealName = (string) reader["RealName"];
                }
                if (reader["Address"] != DBNull.Value)
                {
                    member.Address = (string) reader["Address"];
                }
                if (reader["Zipcode"] != DBNull.Value)
                {
                    member.Zipcode = (string) reader["Zipcode"];
                }
                if (reader["TelPhone"] != DBNull.Value)
                {
                    member.TelPhone = (string) reader["TelPhone"];
                }
                if (reader["CellPhone"] != DBNull.Value)
                {
                    member.CellPhone = (string) reader["CellPhone"];
                }
                if (reader["QQ"] != DBNull.Value)
                {
                    member.QQ = (string) reader["QQ"];
                }
                if (reader["Wangwang"] != DBNull.Value)
                {
                    member.Wangwang = (string) reader["Wangwang"];
                }
                if (reader["MSN"] != DBNull.Value)
                {
                    member.MSN = (string) reader["MSN"];
                }
            }
            return member;
        }

        public override bool UpdateDistributor(Distributor distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET GradeId = @GradeId, TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat, TopRegionId = @TopRegionId, RegionId = @RegionId, RealName = @RealName,CompanyName=@CompanyName, Address = @Address, Zipcode = @Zipcode, TelPhone = @TelPhone, CellPhone = @CellPhone, QQ = @QQ, Wangwang = @Wangwang, MSN = @MSN, Remark=@Remark WHERE UserId = @UserId");
            string salt = UserHelper.CreateSalt();
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, distributor.GradeId);
            this.database.AddInParameter(sqlStringCommand, "TradePassword", DbType.String, UserHelper.EncodePassword(distributor.TradePasswordFormat, distributor.TradePassword, salt));
            this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", DbType.String, salt);
            this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", DbType.Int32, distributor.TradePasswordFormat);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, distributor.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, distributor.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, distributor.RealName);
            this.database.AddInParameter(sqlStringCommand, "CompanyName", DbType.String, distributor.CompanyName);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, distributor.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, distributor.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, distributor.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, distributor.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, distributor.QQ);
            this.database.AddInParameter(sqlStringCommand, "Wangwang", DbType.String, distributor.Wangwang);
            this.database.AddInParameter(sqlStringCommand, "MSN", DbType.String, distributor.MSN);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, distributor.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateMember(Member member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId, IsOpenBalance = @IsOpenBalance, TopRegionId=@TopRegionId, RegionId = @RegionId, RealName = @RealName, Address = @Address, Zipcode = @Zipcode, TelPhone = @TelPhone, CellPhone = @CellPhone, QQ = @QQ, Wangwang = @Wangwang, MSN = @MSN WHERE UserId = @UserId");
            string str = UserHelper.CreateSalt();
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            this.database.AddInParameter(sqlStringCommand, "IsOpenBalance", DbType.Boolean, member.IsOpenBalance);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, member.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, member.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, member.RealName);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, member.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, member.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, member.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, member.QQ);
            this.database.AddInParameter(sqlStringCommand, "Wangwang", DbType.String, member.Wangwang);
            this.database.AddInParameter(sqlStringCommand, "MSN", DbType.String, member.MSN);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateUnderling(Member underling)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET GradeId = @GradeId,IsOpenBalance = @IsOpenBalance, TopRegionId=@TopRegionId, RegionId = @RegionId, RealName = @RealName, Address = @Address, Zipcode = @Zipcode, TelPhone = @TelPhone, CellPhone = @CellPhone, QQ = @QQ, Wangwang = @Wangwang, MSN = @MSN WHERE UserId = @UserId");
            string str = UserHelper.CreateSalt();
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, underling.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, underling.GradeId);
            this.database.AddInParameter(sqlStringCommand, "IsOpenBalance", DbType.Boolean, underling.IsOpenBalance);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, underling.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, underling.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, underling.RealName);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, underling.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, underling.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, underling.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, underling.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, underling.QQ);
            this.database.AddInParameter(sqlStringCommand, "Wangwang", DbType.String, underling.Wangwang);
            this.database.AddInParameter(sqlStringCommand, "MSN", DbType.String, underling.MSN);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ValidDistributorTradePassword(string username, string password)
        {
            return this.CheckTradePassword("aspnet_Distributors", username, password);
        }

        public override bool ValidMemberTradePassword(string username, string password)
        {
            return this.CheckTradePassword("aspnet_Members", username, password);
        }

        public override bool ValidUnderlingTradePassword(string username, string password)
        {
            return this.CheckTradePassword("distro_Members", username, password);
        }
    }
}


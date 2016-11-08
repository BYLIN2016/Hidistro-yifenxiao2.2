namespace Hidistro.Membership.Data
{
    using Hidistro.Membership.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class RoleData : MemberRoleProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override void AddDeletePrivilegeInRoles(Guid roleId, string privilege)
        {
            string query = string.Format("DELETE FROM Hishop_PrivilegeInRoles WHERE  RoleId='{0}' ", roleId);
            string[] strArray = privilege.Split(new char[] { ',' });
            if (strArray != null)
            {
                foreach (string str2 in strArray)
                {
                    query = query + string.Format(" INSERT INTO Hishop_PrivilegeInRoles(RoleId,Privilege) VALUES ('{0}', {1})", roleId, str2);
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void DeletePrivilegeInRoles(Guid roleId)
        {
            string query = string.Format("DELETE FROM Hishop_PrivilegeInRoles WHERE  RoleId='{0}' ", roleId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override IList<int> GetPrivilegeByRoles(Guid roleId)
        {
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PrivilegeInRoles WHERE RoleId=@RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Guid, roleId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int) reader["Privilege"]);
                }
            }
            return list;
        }

        public override IList<int> GetPrivilegesForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return new List<int>();
            }
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT pr.Privilege FROM dbo.aspnet_Roles r INNER JOIN dbo.aspnet_UsersInRoles ur ON r.RoleId = ur.RoleId INNER JOIN dbo.Hishop_PrivilegeInRoles pr ON pr.RoleId = r.RoleId WHERE ur.UserId = (SELECT UserId FROM dbo.aspnet_Users WHERE   LoweredUserName = LOWER(@UserName))");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetInt32(0));
                }
                reader.Close();
            }
            return list;
        }

        public override RoleInfo GetRole(Guid roleId, string roleName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT RoleId, RoleName as [Name], r.Description FROM aspnet_Roles r  WHERE 1=1");
            if (roleId != Guid.Empty)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " AND RoleId = @RoleId";
                this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Guid, roleId);
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " AND RoleName = @RoleName";
                this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, roleName);
            }
            RoleInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = MemberRoleProvider.PopulateRoleFromIDataReader(reader);
                }
                reader.Close();
            }
            return info;
        }

        public override ArrayList GetRoles()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT RoleId, RoleName as [Name], Description FROM aspnet_Roles order by RoleId");
            ArrayList list = new ArrayList();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    RoleInfo info = MemberRoleProvider.PopulateRoleFromIDataReader(reader);
                    list.Add(info);
                }
                reader.Close();
            }
            return list;
        }

        public override ArrayList GetRoles(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT R.RoleId, RoleName as [Name], Description FROM aspnet_UsersInRoles U, aspnet_Roles R WHERE U.RoleId = R.RoleId AND U.UserId = @UserId order by R.RoleId");
            ArrayList list = new ArrayList();
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    RoleInfo info = MemberRoleProvider.PopulateRoleFromIDataReader(reader);
                    list.Add(info);
                }
                reader.Close();
            }
            return list;
        }

        public override bool PrivilegeInRoles(Guid roleId, int privilege)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PrivilegeInRoles WHERE RoleId=@RoleId AND Privilege=@Privilege");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Guid, roleId);
            this.database.AddInParameter(sqlStringCommand, "Privilege", DbType.Int32, privilege);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = true;
                }
            }
            return flag;
        }

        public override void UpdateRole(RoleInfo role)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Roles SET RoleName = @Name, LoweredRoleName = Lower(@Name), Description = @Description WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, role.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, role.Description);
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Guid, role.RoleID);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}


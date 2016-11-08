namespace Hidistro.Membership.Core
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Globalization;

    public class RoleInfo : IComparable
    {
        private string description;
        private string name;
        private Guid roleID;

        public RoleInfo()
        {
            this.roleID = Guid.Empty;
        }

        public RoleInfo(Guid roleID, string name)
        {
            this.roleID = Guid.Empty;
            this.roleID = roleID;
            this.name = name;
        }

        public int CompareTo(object obj)
        {
            RoleInfo info = obj as RoleInfo;
            if (info != null)
            {
                if (this.RoleID == info.RoleID)
                {
                    return 0;
                }
                return string.Compare(this.Name, info.Name, true, CultureInfo.InvariantCulture);
            }
            return -1;
        }

        public override bool Equals(object obj)
        {
            RoleInfo info = obj as RoleInfo;
            return (((info != null) && (info.RoleID == this.RoleID)) && (info.Name == this.Name));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [StringLengthValidator(0, 100, Ruleset="ValRoleInfo", MessageTemplate="职能说明的长度限制在100个字符以内")]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public bool IsRoleIDAssigned
        {
            get
            {
                return (this.RoleID != Guid.Empty);
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValRoleInfo", MessageTemplate="部门名称不能为空，长度限制在60个字符以内")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public Guid RoleID
        {
            get
            {
                return this.roleID;
            }
            set
            {
                this.roleID = value;
            }
        }
    }
}


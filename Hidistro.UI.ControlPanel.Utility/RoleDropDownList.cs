namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class RoleDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            ArrayList roles = new ArrayList();
            roles = RoleHelper.GetRoles();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            foreach (RoleInfo info in roles)
            {
                if (!this.IsDefaultRole(info.Name))
                {
                    if (string.Compare(info.Name, RoleHelper.SystemAdministrator, true, CultureInfo.InvariantCulture) == 0)
                    {
                        base.Items.Add(new ListItem("超级管理员", info.RoleID.ToString()));
                    }
                    else
                    {
                        base.Items.Add(new ListItem(info.Name, info.RoleID.ToString()));
                    }
                }
            }
        }

        private bool IsDefaultRole(string roleName)
        {
            if (((string.Compare(roleName, HiContext.Current.Config.RolesConfiguration.Manager, true, CultureInfo.InvariantCulture) != 0) && (string.Compare(roleName, HiContext.Current.Config.RolesConfiguration.Member, true, CultureInfo.InvariantCulture) != 0)) && (string.Compare(roleName, HiContext.Current.Config.RolesConfiguration.Distributor, true, CultureInfo.InvariantCulture) != 0))
            {
                return (string.Compare(roleName, HiContext.Current.Config.RolesConfiguration.Underling, true, CultureInfo.InvariantCulture) == 0);
            }
            return true;
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }

        public Guid SelectedValue
        {
            get
            {
                Guid empty = Guid.Empty;
                if (base.SelectedValue.Length == 0x24)
                {
                    empty = new Guid(base.SelectedValue);
                }
                return empty;
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
            }
        }
    }
}


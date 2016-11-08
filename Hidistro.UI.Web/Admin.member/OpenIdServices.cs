namespace Hidistro.UI.Web.Admin.member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.OpenIdServices)]
    public class OpenIdServices : AdminPage
    {
        protected GridView grdConfigedItems;
        protected GridView grdEmptyList;
        protected Panel pnlConfigedList;
        protected Panel pnlConfigedNote;
        protected Panel pnlEmptyList;
        protected Panel pnlEmptyNote;

        private void BindConfigedList()
        {
            PluginItemCollection configedItems = OpenIdHelper.GetConfigedItems();
            if ((configedItems != null) && (configedItems.Count > 0))
            {
                this.grdConfigedItems.DataSource = configedItems.Items;
                this.grdConfigedItems.DataBind();
                this.pnlConfigedList.Visible = true;
                this.pnlConfigedNote.Visible = false;
            }
            else
            {
                this.pnlConfigedList.Visible = false;
                this.pnlConfigedNote.Visible = true;
            }
        }

        private void BindData()
        {
            this.BindConfigedList();
            this.BindEmptyList();
        }

        private void BindEmptyList()
        {
            PluginItemCollection emptyItems = OpenIdHelper.GetEmptyItems();
            if ((emptyItems != null) && (emptyItems.Count > 0))
            {
                this.grdEmptyList.DataSource = emptyItems.Items;
                this.grdEmptyList.DataBind();
                this.pnlEmptyList.Visible = true;
                this.pnlEmptyNote.Visible = false;
            }
            else
            {
                this.pnlEmptyList.Visible = false;
                this.pnlEmptyNote.Visible = true;
            }
        }

        private void grdConfigedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            OpenIdHelper.DeleteSettings(this.grdConfigedItems.DataKeys[e.RowIndex]["FullName"].ToString());
            this.BindData();
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdConfigedItems.RowDeleting += new GridViewDeleteEventHandler(this.grdConfigedItems_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}


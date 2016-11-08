namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.FriendlyLinks)]
    public class FriendlyLinks : AdminPage
    {
        protected Grid grdGroupList;

        private void BindFriendlyLinks()
        {
            IList<FriendlyLinksInfo> friendlyLinks = StoreHelper.GetFriendlyLinks();
            this.grdGroupList.DataSource = friendlyLinks;
            this.grdGroupList.DataBind();
        }

        private void grdGroupList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int linkId = (int) this.grdGroupList.DataKeys[rowIndex].Value;
            if (e.CommandName == "SetYesOrNo")
            {
                FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(linkId);
                if (friendlyLink.Visible)
                {
                    friendlyLink.Visible = false;
                }
                else
                {
                    friendlyLink.Visible = true;
                }
                StoreHelper.UpdateFriendlyLink(friendlyLink);
                this.BindFriendlyLinks();
            }
            else
            {
                int displaySequence = int.Parse((this.grdGroupList.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceLinkId = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if (rowIndex < (this.grdGroupList.Rows.Count - 1))
                    {
                        replaceLinkId = (int) this.grdGroupList.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = int.Parse((this.grdGroupList.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex > 0))
                {
                    replaceLinkId = (int) this.grdGroupList.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = int.Parse((this.grdGroupList.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if (replaceLinkId > 0)
                {
                    StoreHelper.SwapFriendlyLinkSequence(linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
                    this.BindFriendlyLinks();
                }
            }
        }

        private void grdGroupList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int linkId = (int) this.grdGroupList.DataKeys[e.RowIndex].Value;
            FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(linkId);
            if (StoreHelper.FriendlyLinkDelete(linkId) > 0)
            {
                try
                {
                    StoreHelper.DeleteImage(friendlyLink.ImageUrl);
                }
                catch
                {
                }
                this.BindFriendlyLinks();
                this.ShowMsg("成功删除了选择的友情链接", true);
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdGroupList.RowCommand += new GridViewCommandEventHandler(this.grdGroupList_RowCommand);
            this.grdGroupList.RowDeleting += new GridViewDeleteEventHandler(this.grdGroupList_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindFriendlyLinks();
            }
        }
    }
}


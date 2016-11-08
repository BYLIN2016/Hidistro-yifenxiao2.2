namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Affiches)]
    public class AfficheList : AdminPage
    {
        protected Grid grdAfficheList;
        protected ImageLinkButton ImageLinkButton1;
        protected ImageLinkButton lkbtnDeleteSelect;

        private void BindAffiche()
        {
            this.grdAfficheList.DataSource = NoticeHelper.GetAfficheList();
            this.grdAfficheList.DataBind();
        }

        private void DeleteSelect()
        {
            int item = 0;
            List<int> affiches = new List<int>();
            int num2 = 0;
            foreach (GridViewRow row in this.grdAfficheList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num2++;
                    item = Convert.ToInt32(this.grdAfficheList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    affiches.Add(item);
                }
            }
            if (num2 != 0)
            {
                int num3 = NoticeHelper.DeleteAffiches(affiches);
                this.BindAffiche();
                this.ShowMsg(string.Format("成功删除了选择的{0}条公告", num3), true);
            }
            else
            {
                this.ShowMsg("请先选择要删除的公告", false);
            }
        }

        private void grdAfficheList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (NoticeHelper.DeleteAffiche((int) this.grdAfficheList.DataKeys[e.RowIndex].Value))
            {
                this.BindAffiche();
                this.ShowMsg("成功删除了选择的公告", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void ImageLinkButton1_Click(object sender, EventArgs e)
        {
            this.DeleteSelect();
        }

        private void lkbtnDeleteSelect_Click(object sender, EventArgs e)
        {
            this.DeleteSelect();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdAfficheList.RowDeleting += new GridViewDeleteEventHandler(this.grdAfficheList_RowDeleting);
            this.lkbtnDeleteSelect.Click += new EventHandler(this.lkbtnDeleteSelect_Click);
            this.ImageLinkButton1.Click += new EventHandler(this.ImageLinkButton1_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindAffiche();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
    }
}


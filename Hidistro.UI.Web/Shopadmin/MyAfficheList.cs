namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MyAfficheList : DistributorPage
    {
        protected Grid grdAfficheList;
        protected ImageLinkButton lkbtnDeleteSelect;
        protected ImageLinkButton lkbtnDeleteSelect1;

        private void BindAffiche()
        {
            this.grdAfficheList.DataSource = SubsiteCommentsHelper.GetAfficheList();
            this.grdAfficheList.DataBind();
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void grdAfficheList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsiteCommentsHelper.DeleteAffiche((int) this.grdAfficheList.DataKeys[e.RowIndex].Value))
            {
                this.BindAffiche();
                this.ShowMsg("成功删除了选择的公告", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteSelect_Click(object sender, EventArgs e)
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
                int num3 = SubsiteCommentsHelper.DeleteAffiches(affiches);
                this.BindAffiche();
                this.ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除了\"{0}\"公告", new object[] { num3 }), true);
            }
            else
            {
                this.ShowMsg("请先选择要删除的公告", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdAfficheList.RowDeleting += new GridViewDeleteEventHandler(this.grdAfficheList_RowDeleting);
            this.lkbtnDeleteSelect.Click += new EventHandler(this.lkbtnDeleteSelect_Click);
            this.lkbtnDeleteSelect1.Click += new EventHandler(this.lkbtnDeleteSelect_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindAffiche();
            }
        }
    }
}


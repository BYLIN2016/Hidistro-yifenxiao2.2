namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.Themes)]
    public class SetHeaderMenu : AdminPage
    {
        protected Button btnSave;
        protected Grid grdHeaderMenu;
        protected HyperLink hlinkAddHeaderMenu;
        protected LinkButton lbtnSaveSequence;
        protected Literal litThemName;
        private string themName;
        protected TextBox txtCategoryNum;

        private void BindHeaderMenu()
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", this.themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Title");
            table.Columns.Add("DisplaySequence", typeof(int));
            table.Columns.Add("Url");
            table.Columns.Add("Category");
            table.Columns.Add("Visible");
            XmlNode node = document.SelectSingleNode("root");
            this.txtCategoryNum.Text = node.Attributes["CategoryNum"].Value;
            XmlNodeList childNodes = node.ChildNodes;
            foreach (XmlNode node2 in childNodes)
            {
                DataRow row = table.NewRow();
                row["Id"] = int.Parse(node2.Attributes["Id"].Value);
                row["Title"] = node2.Attributes["Title"].Value;
                row["DisplaySequence"] = int.Parse(node2.Attributes["DisplaySequence"].Value);
                row["Category"] = node2.Attributes["Category"].Value;
                row["Url"] = node2.Attributes["Url"].Value;
                row["Visible"] = node2.Attributes["Visible"].Value;
                table.Rows.Add(row);
            }
            table.DefaultView.Sort = "DisplaySequence ASC";
            this.grdHeaderMenu.DataSource = table;
            this.grdHeaderMenu.DataBind();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int result = 0;
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", this.themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode node = document.SelectSingleNode("root");
            if (!int.TryParse(this.txtCategoryNum.Text, out result))
            {
                this.ShowMsg("请输入有效果的数字", false);
            }
            else
            {
                node.Attributes["CategoryNum"].Value = result.ToString();
                document.Save(filename);
            }
        }

        private void grdHeaderMenu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int num2 = (int) this.grdHeaderMenu.DataKeys[rowIndex].Value;
                string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", this.themName));
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                foreach (XmlNode node in document.SelectSingleNode("root").ChildNodes)
                {
                    if (node.Attributes["Id"].Value == num2.ToString())
                    {
                        if (node.Attributes["Visible"].Value == "true")
                        {
                            node.Attributes["Visible"].Value = "false";
                        }
                        else
                        {
                            node.Attributes["Visible"].Value = "true";
                        }
                        break;
                    }
                }
                document.Save(filename);
                this.BindHeaderMenu();
            }
        }

        private void grdHeaderMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int num = (int) this.grdHeaderMenu.DataKeys[e.Row.RowIndex].Value;
                HyperLink link = e.Row.FindControl("lkbEdit") as HyperLink;
                link.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/store/EditHeaderMenu.aspx?Id={0}&ThemName={1}", num, this.themName));
                Literal literal = e.Row.FindControl("litCategory") as Literal;
                string text = literal.Text;
                if (text == "1")
                {
                    text = "系统页面";
                }
                else if (text == "2")
                {
                    text = "商品搜索链接";
                }
                else
                {
                    text = "自定义链接";
                }
                literal.Text = text;
            }
        }

        private void grdHeaderMenu_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int num = (int) this.grdHeaderMenu.DataKeys[e.RowIndex].Value;
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", this.themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode node = document.SelectSingleNode("root");
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Attributes["Id"].Value == num.ToString())
                {
                    node.RemoveChild(node2);
                    break;
                }
            }
            document.Save(filename);
            this.BindHeaderMenu();
        }

        private void lbtnSaveSequence_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdHeaderMenu.Rows)
            {
                int id = (int) this.grdHeaderMenu.DataKeys[row.RowIndex].Value;
                int result = 0;
                int.TryParse(((TextBox) row.FindControl("txtDisplaySequence")).Text, out result);
                this.SaveHeaderMenu(id, result);
            }
            this.BindHeaderMenu();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.themName = HiContext.Current.SiteSettings.Theme;
            this.grdHeaderMenu.RowDataBound += new GridViewRowEventHandler(this.grdHeaderMenu_RowDataBound);
            this.grdHeaderMenu.RowCommand += new GridViewCommandEventHandler(this.grdHeaderMenu_RowCommand);
            this.grdHeaderMenu.RowDeleting += new GridViewDeleteEventHandler(this.grdHeaderMenu_RowDeleting);
            this.lbtnSaveSequence.Click += new EventHandler(this.lbtnSaveSequence_Click);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            if (!this.Page.IsPostBack)
            {
                this.litThemName.Text = this.themName;
                this.hlinkAddHeaderMenu.NavigateUrl = Globals.GetAdminAbsolutePath("/store/AddHeaderMenu.aspx?ThemName=" + this.themName);
                this.BindHeaderMenu();
            }
        }

        private void SaveHeaderMenu(int id, int displaySequence)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", this.themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlNode node in document.SelectSingleNode("root").ChildNodes)
            {
                if (node.Attributes["Id"].Value == id.ToString())
                {
                    node.Attributes["DisplaySequence"].Value = displaySequence.ToString();
                    break;
                }
            }
            document.Save(filename);
        }
    }
}


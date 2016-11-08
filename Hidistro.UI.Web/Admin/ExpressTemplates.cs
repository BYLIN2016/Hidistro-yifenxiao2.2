namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.ExpressTemplates)]
    public class ExpressTemplates : AdminPage
    {
        protected Grid grdExpressTemplates;

        private void BindExpressTemplates()
        {
            this.grdExpressTemplates.DataSource = SalesHelper.GetExpressTemplates();
            this.grdExpressTemplates.DataBind();
        }

        private void DeleteXmlFile(string xmlfile)
        {
            string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", xmlfile));
            if (File.Exists(path))
            {
                XmlDocument document = new XmlDocument();
                document.Load(path);
                XmlNode node = document.SelectSingleNode("printer/pic");
                string str2 = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", node.InnerText));
                if (File.Exists(str2))
                {
                    File.Delete(str2);
                }
                File.Delete(path);
            }
        }

        private void grdExpressTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
                int expressId = (int) this.grdExpressTemplates.DataKeys[namingContainer.RowIndex].Value;
                SalesHelper.SetExpressIsUse(expressId);
                this.BindExpressTemplates();
            }
        }

        private void grdExpressTemplates_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int expressId = (int) this.grdExpressTemplates.DataKeys[e.RowIndex].Value;
            if (SalesHelper.DeleteExpressTemplate(expressId))
            {
                Literal literal = this.grdExpressTemplates.Rows[e.RowIndex].FindControl("litXmlFile") as Literal;
                this.DeleteXmlFile(literal.Text);
                this.BindExpressTemplates();
                this.ShowMsg("已经成功删除选择的快递单模板", true);
            }
            else
            {
                this.ShowMsg("删除快递单模板失败", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdExpressTemplates.RowDeleting += new GridViewDeleteEventHandler(this.grdExpressTemplates_RowDeleting);
            this.grdExpressTemplates.RowCommand += new GridViewCommandEventHandler(this.grdExpressTemplates_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindExpressTemplates();
            }
        }
    }
}


namespace Hidistro.UI.Web.DialogTemplates
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using kindeditor.Net;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Xml;

    public class CustomContent : Page
    {
        protected KindeditorControl customDescription;
        protected HtmlForm form1;

        private XmlNode BindHtml(string controId, string type)
        {
            XmlNode node = null;
            if (controId.Contains("_"))
            {
                string xmlname = controId.Split(new char[] { '_' })[0];
                int id = Convert.ToInt32(controId.Split(new char[] { '_' })[1].ToString());
                node = this.FindXmlNode(xmlname, id, type);
            }
            return node;
        }

        private XmlNode FindXmlNode(string xmlname, int Id, string type)
        {
            XmlNode node = null;
            string str = xmlname;
            if (str == null)
            {
                return node;
            }
            if (!(str == "ads"))
            {
                if (str != "products")
                {
                    return node;
                }
            }
            else
            {
                return TagsHelper.FindAdNode(Id, type);
            }
            return TagsHelper.FindProductNode(Id, type);
        }

        protected override void OnInit(EventArgs e)
        {
            IUser contexUser = Users.GetContexUser();
            if (!HiContext.Current.SiteSettings.IsDistributorSettings && (contexUser.UserRole != UserRole.SiteManager))
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("login.aspx"), true);
            }
            if (HiContext.Current.SiteSettings.IsDistributorSettings && (contexUser.UserRole != UserRole.Distributor))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "Shopadmin/DistributorLogin.aspx", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!base.IsPostBack && !string.IsNullOrEmpty(base.Request.QueryString["id"].ToString())) && (!string.IsNullOrEmpty(base.Request.QueryString["type"].ToString()) && !string.IsNullOrEmpty(base.Request.QueryString["name"].ToString())))
            {
                string str = base.Request.QueryString["name"].ToString();
                XmlNode node = this.BindHtml(base.Request.QueryString["id"].ToString(), base.Request.QueryString["type"].ToString());
                if ((node != null) && (node.Attributes[str] != null))
                {
                    this.customDescription.Text = Globals.HtmlDecode(Globals.HtmlDecode(node.Attributes[str].Value));
                }
            }
        }
    }
}


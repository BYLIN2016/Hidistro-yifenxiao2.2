namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ShopAdmin : MasterPage
    {
        protected ContentPlaceHolder contentHolder;
        protected HtmlHead Head1;
        protected HeadContainer HeadContainer1;
        protected ContentPlaceHolder headHolder;
        protected HyperLink hlinkHome;
        protected HyperLink hlinkLogout;
        protected HyperLink hlinkToTaobao;
        protected HtmlAnchor hp_message;
        protected HyperLink hpkDefault;
        protected HiImage imgLogo;
        protected Label lblUserName;
        protected Literal mainMenuHolder;
        private const string moduleMenuFormat = "<li><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>";
        protected PageTitle PageTitle1;
        protected Script Script1;
        protected Script Script2;
        protected Script Script3;
        protected Script Script4;
        protected Script Script5;
        protected Script Script6;
        protected Script Script7;
        private const string selectedModuleMenuFormat = "<li class=\"menucurrent\"><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>";
        private const string selectedSubMenuFormat = "<li class=\"visited\"><a href=\"{0}\"><span>{1}</span></a></li>";
        private const string subMenuFormat = "<li><a href=\"{0}\"><span>{1}</span></a></li>";
        protected Literal subMenuHolder;
        protected HtmlForm thisForm;
        protected ContentPlaceHolder validateHolder;

        private static string BuildModuleMenu(string title, string imgUrl, string link, XmlNode currentModuleNode, string shopAdminPath)
        {
            if (((currentModuleNode != null) && (string.Compare(title, currentModuleNode.Attributes["Title"].Value, true) == 0)) && (string.Compare(link, currentModuleNode.Attributes["Link"].Value, true) == 0))
            {
                return string.Format("<li class=\"menucurrent\"><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>", shopAdminPath + link, Globals.ApplicationPath + imgUrl, title);
            }
            return string.Format("<li><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>", shopAdminPath + link, Globals.ApplicationPath + imgUrl, title);
        }

        private static string BuildSubMenu(string title, string link, XmlNode currentItemNode, string shopAdminPath)
        {
            if (((currentItemNode != null) && (string.Compare(title, currentItemNode.Attributes["Title"].Value, true) == 0)) && (string.Compare(link, currentItemNode.Attributes["Link"].Value, true) == 0))
            {
                return string.Format("<li class=\"visited\"><a href=\"{0}\"><span>{1}</span></a></li>", shopAdminPath + link, title);
            }
            return string.Format("<li><a href=\"{0}\"><span>{1}</span></a></li>", shopAdminPath + link, title);
        }

        private static XmlDocument GetMenuDocument()
        {
            XmlDocument document;
            HttpContext context = HiContext.Current.Context;
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if (siteSettings == null)
            {
                document = HiCache.Get("FileCache-ShopadminMenu-NoSite") as XmlDocument;
            }
            else
            {
                document = HiCache.Get("FileCache-ShopadminMenu") as XmlDocument;
            }
            if (document == null)
            {
                string str;
                if (siteSettings == null)
                {
                    str = context.Request.MapPath("~/Shopadmin/Menu_NoSite.config");
                    document = new XmlDocument();
                    document.Load(str);
                    HiCache.Max("FileCache-ShopadminMenu-NoSite", document, new CacheDependency(str));
                    return document;
                }
                str = context.Request.MapPath("~/Shopadmin/Menu.config");
                document = new XmlDocument();
                document.Load(str);
                HiCache.Max("FileCache-ShopadminMenu", document, new CacheDependency(str));
            }
            return document;
        }

        private void LoadMenu()
        {
            string oldValue = (Globals.ApplicationPath + "/Shopadmin/").ToLower();
            string str2 = this.Page.Request.Url.AbsolutePath.ToLower();
            string xpath = string.Format("Menu/Module/Item/PageLink[@Link='{0}']", str2.Replace(oldValue, "").ToLower());
            XmlDocument menuDocument = GetMenuDocument();
            XmlNode node = menuDocument.SelectSingleNode(xpath);
            XmlNode currentItemNode = null;
            XmlNode currentModuleNode = null;
            if (node != null)
            {
                currentItemNode = node.ParentNode;
                currentModuleNode = currentItemNode.ParentNode;
            }
            else
            {
                xpath = string.Format("Menu/Module/Item[@Link='{0}']", str2.Replace(oldValue, "").ToLower());
                currentItemNode = menuDocument.SelectSingleNode(xpath);
                if (currentItemNode != null)
                {
                    currentModuleNode = currentItemNode.ParentNode;
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (XmlNode node4 in menuDocument.SelectNodes("Menu/Module"))
            {
                if (node4.NodeType == XmlNodeType.Element)
                {
                    builder.Append(BuildModuleMenu(node4.Attributes["Title"].Value, node4.Attributes["Image"].Value, node4.Attributes["Link"].Value.ToLower(), currentModuleNode, oldValue));
                    builder.Append(Environment.NewLine);
                }
            }
            this.mainMenuHolder.Text = builder.ToString();
            builder.Remove(0, builder.Length);
            if (currentModuleNode != null)
            {
                foreach (XmlNode node5 in currentModuleNode.ChildNodes)
                {
                    if (node5.NodeType == XmlNodeType.Element)
                    {
                        builder.Append(BuildSubMenu(node5.Attributes["Title"].Value, node5.Attributes["Link"].Value.ToLower(), currentItemNode, oldValue));
                        builder.Append(Environment.NewLine);
                    }
                }
            }
            this.subMenuHolder.Text = builder.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PageTitle.AddTitle(HiContext.Current.SiteSettings.SiteName, this.Context);
            foreach (Control control in this.Page.Header.Controls)
            {
                if (control is HtmlLink)
                {
                    HtmlLink link = control as HtmlLink;
                    if (link.Href.StartsWith("/"))
                    {
                        link.Href = Globals.ApplicationPath + link.Href;
                    }
                    else
                    {
                        link.Href = Globals.ApplicationPath + "/" + link.Href;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadMenu();
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (siteSettings == null)
                {
                    this.hlinkHome.NavigateUrl = Globals.GetSiteUrls().Home;
                    this.hpkDefault.NavigateUrl = "nositedefault.aspx";
                    siteSettings = SettingsManager.GetMasterSettings(true);
                    if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
                    {
                        this.imgLogo.ImageUrl = siteSettings.LogoUrl;
                    }
                }
                else
                {
                    this.hlinkHome.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().Home;
                    this.hpkDefault.NavigateUrl = "Default.aspx";
                    if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
                    {
                        this.imgLogo.ImageUrl = siteSettings.LogoUrl;
                    }
                }
                this.hlinkLogout.NavigateUrl = Globals.ApplicationPath + "/Logout.aspx";
                int isReadMessageToAdmin = SubsiteCommentsHelper.GetIsReadMessageToAdmin();
                if (isReadMessageToAdmin > 0)
                {
                    this.hp_message.Style.Add("color", "red");
                    this.hp_message.InnerText = "站内信(" + isReadMessageToAdmin.ToString() + ")";
                }
                Distributor user = HiContext.Current.User as Distributor;
                if (user != null)
                {
                    this.lblUserName.Text = "欢迎您，" + user.Username;
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    this.hlinkToTaobao.NavigateUrl = string.Format("http://order1.kuaidiangtong.com/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}&DistributorUserId={2}&Email={3}&RealName={4}&CompanyName={5}&Address={6}&TelPhone={7}&QQ={8}&Wangwang={9}", new object[] { masterSettings.SiteUrl, Globals.ApplicationPath, user.UserId, user.Email, user.RealName, user.CompanyName, user.Address, user.TelPhone, user.QQ, user.Wangwang });
                }
            }
        }
    }
}


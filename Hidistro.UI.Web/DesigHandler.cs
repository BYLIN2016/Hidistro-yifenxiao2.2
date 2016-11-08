namespace Hidistro.UI.Web
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using HtmlAgilityPack;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Xml;

    public class DesigHandler : AdminPage, IHttpHandler
    {
        private string configurl = "";
        private XmlNode currennode;
        private string desigtype = "";
        private string elementId = "";
        private string message = "";
        private string modeId = "";
        private string pagename = "";

        public XmlNode FindNode(string configname)
        {
            XmlNode node = null;
            if (this.elementId != "")
            {
                XmlDocument xmlNode = this.GetXmlNode();
                string xpath = "";
                if (configname == "products")
                {
                    xpath = string.Format("//Subject[@SubjectId='{0}']", this.elementId);
                }
                else if (configname == "ads")
                {
                    xpath = string.Format("//Ad[@Id='{0}']", this.elementId);
                }
                else if (configname == "comments")
                {
                    xpath = string.Format("//Comment[@Id='{0}']", this.elementId);
                }
                else
                {
                    xpath = string.Format("//Menu[@Id='{0}']", this.elementId);
                }
                if (xpath != "")
                {
                    node = xmlNode.SelectSingleNode(xpath);
                    XmlAttribute attribute = xmlNode.CreateAttribute("DialogName");
                    attribute.InnerText = this.GetDialoName();
                    node.Attributes.Append(attribute);
                }
            }
            return node;
        }

        public string GetDialoName()
        {
            string str = "error.html";
            if (this.desigtype != "")
            {
                switch (this.desigtype)
                {
                    case "floor":
                        str = "product_floor_edite.html";
                        goto Label_01D2;

                    case "tab":
                        str = "product_tab_edite.html";
                        goto Label_01D2;

                    case "top":
                        str = "product_top_edite.html";
                        goto Label_01D2;

                    case "group":
                        str = "product_group_edite.html";
                        goto Label_01D2;

                    case "simple":
                        str = "simple_edite.html";
                        goto Label_01D2;

                    case "slide":
                        str = "advert_slide_edite.html";
                        goto Label_01D2;

                    case "image":
                        str = "advert_image_edite.html";
                        goto Label_01D2;

                    case "custom":
                        str = "advert_custom_edite.html";
                        goto Label_01D2;

                    case "article":
                        str = "comment_article_edite.html";
                        goto Label_01D2;

                    case "category":
                        str = "comment_category_edite.html";
                        goto Label_01D2;

                    case "brand":
                        str = "comment_brand_edite.html";
                        goto Label_01D2;

                    case "keyword":
                        str = "comment_keyword_edite.html";
                        goto Label_01D2;

                    case "attribute":
                        str = "comment_attribute_edite.html";
                        goto Label_01D2;

                    case "title":
                        str = "comment_title_edite.html";
                        goto Label_01D2;

                    case "morelink":
                        str = "comment_morelink_edite.html";
                        goto Label_01D2;
                }
                str = "error.html";
            }
        Label_01D2:
            return ("DialogTemplates/" + str);
        }

        public HtmlDocument GetHtmlDocument(string url)
        {
            HtmlDocument document = null;
            if (url != "")
            {
                document = new HtmlDocument();
                document.Load(url);
            }
            return document;
        }

        public HtmlDocument GetWebHtmlDocument(string weburl)
        {
            HtmlDocument document = null;
            if (weburl != "")
            {
                document = new HtmlWeb().Load(weburl);
            }
            return document;
        }

        public XmlDocument GetXmlNode()
        {
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(this.configurl))
            {
                document.Load(this.configurl);
            }
            return document;
        }

        public string LoadFirstHtml()
        {
            string str = "";
            HtmlDocument htmlDocument = this.GetHtmlDocument(DesigAttribute.DesigPagePath);
            HtmlDocument webHtmlDocument = this.GetWebHtmlDocument(DesigAttribute.SourcePagePath);
            HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes("//div[@rel=\"desig\"]");
            IList<DesignTempleteInfo> list = new List<DesignTempleteInfo>();
            foreach (HtmlNode node in (IEnumerable<HtmlNode>) nodes)
            {
                HtmlNode elementbyId = webHtmlDocument.GetElementbyId(node.Id);
                if (elementbyId != null)
                {
                    DesignTempleteInfo item = new DesignTempleteInfo();
                    item.TempleteID = node.Id;
                    item.TempleteContent = elementbyId.InnerHtml;
                    list.Add(item);
                }
            }
            if (list.Count > 0)
            {
                str = JavaScriptConvert.SerializeObject(list);
            }
            return str;
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                this.modeId = context.Request.Form["ModelId"];
                string format = "{{\"success\":{0},\"Result\":{1}}}";
                string modeId = this.modeId;
                if (modeId != null)
                {
                    if (!(modeId == "Load"))
                    {
                        if (modeId == "editedialog")
                        {
                            goto Label_009F;
                        }
                    }
                    else
                    {
                        this.pagename = context.Request.Form["PageName"];
                        DesigAttribute.PageName = this.pagename;
                        string str2 = this.LoadFirstHtml();
                        if (!string.IsNullOrEmpty(str2))
                        {
                            this.message = string.Format(format, "true", str2);
                        }
                    }
                }
                goto Label_0251;
            Label_009F:
                this.desigtype = context.Request.Form["Type"];
                if (this.desigtype != "logo")
                {
                    string str3 = context.Request.Form["Elementid"];
                    if ((this.desigtype != "") && (str3.Split(new char[] { '_' }).Length == 2))
                    {
                        this.elementId = str3.Split(new char[] { '_' })[1];
                        this.configurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/config/" + str3.Split(new char[] { '_' })[0] + ".xml");
                        this.currennode = this.FindNode(str3.Split(new char[] { '_' })[0]);
                        if (this.currennode != null)
                        {
                            string str4 = JavaScriptConvert.SerializeXmlNode(this.currennode);
                            str4 = str4.Remove(0, str4.IndexOf(":") + 1).Remove((str4.Length - (str4.IndexOf(":") + 1)) - 1).Replace("@", "");
                            this.message = string.Format(format, "true", str4);
                        }
                    }
                }
                else
                {
                    this.message = string.Format(format, "true", "{\"LogoUrl\":\"" + HiContext.Current.SiteSettings.LogoUrl + "\",\"DialogName\":\"DialogTemplates/advert_logo.html\"}");
                }
            }
            catch (Exception exception)
            {
                this.message = "{\"success\":false,\"Result\":\"未知错误:" + exception.Message + "\"}";
            }
        Label_0251:
            context.Response.ContentType = "text/json";
            context.Response.Write(this.message);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}


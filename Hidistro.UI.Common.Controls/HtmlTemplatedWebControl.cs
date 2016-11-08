namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class HtmlTemplatedWebControl : TemplatedWebControl
    {
        private string skinName;

        protected HtmlTemplatedWebControl()
        {
        }

        private string ControlText()
        {
            if (!this.SkinFileExists)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
            if (builder.Length == 0)
            {
                return null;
            }
            builder.Replace("<%", "").Replace("%>", "");
            string skinPath = HiContext.Current.GetSkinPath();
            builder.Replace("/images/", skinPath + "/images/");
            builder.Replace("/script/", skinPath + "/script/");
            builder.Replace("/style/", skinPath + "/style/");
            builder.Replace("/utility/", Globals.ApplicationPath + "/utility/");
            builder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Kindeditor\" Namespace=\"kindeditor.Net\" Assembly=\"kindeditor.Net\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Validator\" Assembly=\"Hidistro.UI.Common.Validator\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.AccountCenter.CodeBehind\" Assembly=\"Hidistro.UI.AccountCenter.CodeBehind\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
            MatchCollection matchs = Regex.Matches(builder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            for (int i = matchs.Count - 1; i >= 0; i--)
            {
                int startIndex = matchs[i].Groups["UrlName"].Index - 4;
                int length = matchs[i].Groups["UrlName"].Length + 4;
                if (matchs[i].Groups["Param"].Length > 0)
                {
                    length += matchs[i].Groups["Param"].Length + 2;
                }
                builder.Remove(startIndex, length);
                builder.Insert(startIndex, Globals.GetSiteUrls().UrlData.FormatUrl(matchs[i].Groups["UrlName"].Value.Trim(), new object[] { matchs[i].Groups["Param"].Value }));
            }
            return builder.ToString();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.LoadHtmlThemedControl())
            {
                throw new SkinNotFoundException(this.SkinPath);
            }
            this.AttachChildControls();
        }

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return this.Page.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                if (queryStrings[str2] != null)
                {
                    string str = queryStrings[str2].Trim();
                    if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                    {
                        builder.Append(str2).Append("=").Append(this.Page.Server.UrlEncode(str)).Append("&");
                    }
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        protected void GotoResourceNotFound()
        {
            this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx");
        }

        protected bool LoadHtmlThemedControl()
        {
            string str = this.ControlText();
            if (!string.IsNullOrEmpty(str))
            {
                Control child = this.Page.ParseControl(str);
                child.ID = "_";
                this.Controls.Add(child);
                return true;
            }
            return false;
        }

        public void ReloadPage(NameValueCollection queryStrings)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
        }

        private bool SkinFileExists
        {
            get
            {
                return !string.IsNullOrEmpty(this.SkinName);
            }
        }

        public virtual string SkinName
        {
            get
            {
                return this.skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower(CultureInfo.InvariantCulture);
                    if (value.EndsWith(".html"))
                    {
                        this.skinName = value;
                    }
                }
            }
        }

        protected virtual string SkinPath
        {
            get
            {
                string skinPath = HiContext.Current.GetSkinPath();
                if (this.SkinName.StartsWith(skinPath))
                {
                    return this.SkinName;
                }
                if (this.SkinName.StartsWith("/"))
                {
                    return (skinPath + this.SkinName);
                }
                return (skinPath + "/" + this.SkinName);
            }
        }
    }
}


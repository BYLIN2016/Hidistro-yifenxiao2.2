namespace Hidistro.UI.Web
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;

    public static class DesigAttribute
    {
        private static string _pagename;
        public static string DesigPageName;
        public static string SourcePageName = "";

        public static string DesigPagePath
        {
            get
            {
                return Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/" + DesigPageName);
            }
        }

        public static string PageName
        {
            get
            {
                return _pagename;
            }
            set
            {
                string str;
                _pagename = value;
                if ((_pagename != "") && ((str = _pagename) != null))
                {
                    if (!(str == "default"))
                    {
                        if (!(str == "login"))
                        {
                            if (str == "brand")
                            {
                                DesigPageName = "Skin-Desig_Brand.html";
                                SourcePageName = "Brand.aspx";
                            }
                            return;
                        }
                    }
                    else
                    {
                        DesigPageName = "Skin-Desig_Templete.html";
                        SourcePageName = "Default.aspx";
                        return;
                    }
                    DesigPageName = "Skin-Desig_login.html";
                    SourcePageName = "Login.aspx";
                }
            }
        }

        public static string SourcePagePath
        {
            get
            {
                return (HiContext.Current.HostPath + "/" + SourcePageName);
            }
        }
    }
}


namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Subsites.Utility;
    using LitJson;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public class FileManagerJson : DistributorPage
    {
        public void FillTableForPath(string path, string url, string order, Hashtable table)
        {
            string str = "";
            str = base.Server.MapPath(path);
            if (!Directory.Exists(str))
            {
                base.Response.Write("此目录不存在！");
                base.Response.End();
            }
            string[] files = Directory.GetFiles(str);
            switch (order)
            {
                case "uploadtime":
                    Array.Sort(files, new DateTimeSorter(0, true));
                    break;

                case "uploadtime desc":
                    Array.Sort(files, new DateTimeSorter(0, false));
                    break;

                case "lastupdatetime":
                    Array.Sort(files, new DateTimeSorter(1, true));
                    break;

                case "lastupdatetime desc":
                    Array.Sort(files, new DateTimeSorter(1, false));
                    break;

                case "photoname":
                    Array.Sort(files, new NameSorter(true));
                    break;

                case "photoname desc":
                    Array.Sort(files, new NameSorter(false));
                    break;

                case "filesize":
                    Array.Sort(files, new SizeSorter(true));
                    break;

                case "filesize desc":
                    Array.Sort(files, new SizeSorter(false));
                    break;

                default:
                    Array.Sort(files, new NameSorter(true));
                    break;
            }
            table["total_count"] = files.Length;
            List<Hashtable> list = new List<Hashtable>();
            table["file_list"] = list;
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo info = new FileInfo(files[i]);
                Hashtable item = new Hashtable();
                item["cid"] = "-1";
                item["name"] = info.Name;
                item["path"] = url + info.Name;
                item["filesize"] = info.Length;
                item["addedtime"] = info.CreationTime;
                item["updatetime"] = info.LastWriteTime;
                item["filetype"] = info.Extension.Substring(1);
                list.Add(item);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IUser user = Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
            Hashtable table = new Hashtable();
            if (user.UserRole != UserRole.Distributor)
            {
                base.Response.Write("没有权限！");
                base.Response.End();
            }
            else
            {
                string path = "";
                string url = "";
                string str3 = "false";
                if (base.Request.QueryString["isAdvPositions"] != null)
                {
                    str3 = base.Request.QueryString["isAdvPositions"].ToString().ToLower().Trim();
                }
                if (str3 == "false")
                {
                    path = string.Format("~/Storage/sites/{0}/fckfiles/", user.UserId);
                    url = string.Format("/Storage/sites/{0}/fckfiles/", user.UserId);
                }
                else
                {
                    SiteSettings siteSettings = SettingsManager.GetSiteSettings(user.UserId);
                    path = string.Format("~/Templates/sites/{0}/{1}/fckfiles/Files/Image/", user.UserId, siteSettings.Theme);
                    url = string.Format("/Templates/sites/{0}/{1}/fckfiles/Files/Image/", user.UserId, siteSettings.Theme);
                }
                string str4 = base.Request.QueryString["order"];
                str4 = string.IsNullOrEmpty(str4) ? "uploadtime" : str4.ToLower();
                if (base.Request.QueryString["cid"] == null)
                {
                }
                this.FillTableForPath(path, url, str4, table);
                string str6 = base.Request.Url.ToString();
                str6 = str6.Substring(0, str6.IndexOf("/", 7)) + base.Request.ApplicationPath;
                if (str6.EndsWith("/"))
                {
                    str6 = str6.Substring(0, str6.Length - 1);
                }
                table["domain"] = str6;
                base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
                base.Response.Write(JsonMapper.ToJson(table));
                base.Response.End();
            }
        }

        public class DateTimeSorter : IComparer
        {
            private bool ascend;
            private int type;

            public DateTimeSorter(int sortType, bool isAscend)
            {
                this.ascend = isAscend;
                this.type = sortType;
            }

            public int Compare(object x, object y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }
                if (x == null)
                {
                    if (!this.ascend)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (y == null)
                {
                    if (!this.ascend)
                    {
                        return -1;
                    }
                    return 1;
                }
                FileInfo info = new FileInfo(x.ToString());
                FileInfo info2 = new FileInfo(y.ToString());
                if (this.type == 0)
                {
                    if (!this.ascend)
                    {
                        return info2.CreationTime.CompareTo(info.CreationTime);
                    }
                    return info.CreationTime.CompareTo(info2.CreationTime);
                }
                if (!this.ascend)
                {
                    return info2.LastWriteTime.CompareTo(info.LastWriteTime);
                }
                return info.LastWriteTime.CompareTo(info2.LastWriteTime);
            }
        }

        public class NameSorter : IComparer
        {
            private bool ascend;

            public NameSorter(bool isAscend)
            {
                this.ascend = isAscend;
            }

            public int Compare(object x, object y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }
                if (x == null)
                {
                    if (!this.ascend)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (y == null)
                {
                    if (!this.ascend)
                    {
                        return -1;
                    }
                    return 1;
                }
                FileInfo info = new FileInfo(x.ToString());
                FileInfo info2 = new FileInfo(y.ToString());
                if (!this.ascend)
                {
                    return info2.FullName.CompareTo(info.FullName);
                }
                return info.FullName.CompareTo(info2.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            private bool ascend;

            public SizeSorter(bool isAscend)
            {
                this.ascend = isAscend;
            }

            public int Compare(object x, object y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }
                if (x == null)
                {
                    if (!this.ascend)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (y == null)
                {
                    if (!this.ascend)
                    {
                        return -1;
                    }
                    return 1;
                }
                FileInfo info = new FileInfo(x.ToString());
                FileInfo info2 = new FileInfo(y.ToString());
                if (!this.ascend)
                {
                    return info2.Length.CompareTo(info.Length);
                }
                return info.Length.CompareTo(info2.Length);
            }
        }
    }
}


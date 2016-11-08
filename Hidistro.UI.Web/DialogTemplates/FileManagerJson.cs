namespace Hidistro.UI.Web.DialogTemplates
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using LitJson;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    public class FileManagerJson : IHttpHandler
    {
        private string message = "";

        private IList<Hashtable> BindFileCategory()
        {
            List<Hashtable> list = new List<Hashtable>();
            Hashtable item = new Hashtable();
            item["cId"] = "AdvertImg";
            item["cName"] = "广告位图片";
            list.Add(item);
            item = new Hashtable();
            item["cId"] = "TitleImg";
            item["cName"] = "标题图片";
            list.Add(item);
            return list;
        }

        private bool FillTableForPath(string path, string url, string order, Hashtable table, string cid)
        {
            string str = "";
            str = Globals.MapPath(path);
            if (!Directory.Exists(str))
            {
                this.message = "此目录不存在";
                return false;
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
            table["category_list"] = this.BindFileCategory();
            table["total_count"] = files.Length;
            List<Hashtable> list = new List<Hashtable>();
            table["file_list"] = list;
            table["current_cateogry"] = cid;
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo info = new FileInfo(files[i]);
                Hashtable item = new Hashtable();
                item["cid"] = cid;
                item["name"] = info.Name;
                item["path"] = url + info.Name;
                item["filesize"] = info.Length;
                item["addedtime"] = info.CreationTime;
                item["updatetime"] = info.LastWriteTime;
                item["filetype"] = info.Extension.Substring(1);
                list.Add(item);
            }
            return true;
        }

        public void ProcessRequest(HttpContext context)
        {
            IUser user = Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
            Hashtable table = new Hashtable();
            if ((user.UserRole != UserRole.SiteManager) && (user.UserRole != UserRole.Distributor))
            {
                this.message = "没有权限";
            }
            else
            {
                string path = "";
                string url = "";
                string cid = context.Request.QueryString["cid"];
                switch (cid)
                {
                    case null:
                    case "-1":
                        cid = "AdvertImg";
                        break;
                }
                path = string.Format("{0}/UploadImage/" + cid + "/", HiContext.Current.GetSkinPath());
                if (context.Request.ApplicationPath != "/")
                {
                    url = path.Substring(context.Request.ApplicationPath.Length);
                }
                else
                {
                    url = path;
                }
                string str4 = context.Request.QueryString["order"];
                str4 = string.IsNullOrEmpty(str4) ? "uploadtime" : str4.ToLower();
                this.message = "未知错误";
                if (this.FillTableForPath(path, url, str4, table, cid))
                {
                    string str5 = context.Request.Url.ToString();
                    str5 = str5.Substring(0, str5.IndexOf("/", 7)) + context.Request.ApplicationPath;
                    if (str5.EndsWith("/"))
                    {
                        str5 = str5.Substring(0, str5.Length - 1);
                    }
                    table["domain"] = str5;
                    this.message = JsonMapper.ToJson(table);
                }
            }
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

        private class DateTimeSorter : IComparer
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

        private class NameSorter : IComparer
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

        private class SizeSorter : IComparer
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


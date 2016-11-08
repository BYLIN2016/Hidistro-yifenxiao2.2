namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.ControlPanel.Utility;
    using LitJson;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;

    public class FileManagerJson : AdminPage
    {
        public void FillTableForDb(string cid, string order, Hashtable table)
        {
            Database database = DatabaseFactory.CreateDatabase();
            List<Hashtable> list = new List<Hashtable>();
            table["category_list"] = list;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("select CategoryId,CategoryName from Hishop_PhotoCategories order by DisplaySequence");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    Hashtable item = new Hashtable();
                    item["cId"] = reader["CategoryId"];
                    item["cName"] = reader["CategoryName"];
                    list.Add(item);
                }
            }
            List<Hashtable> list2 = new List<Hashtable>();
            table["file_list"] = list2;
            if (cid.Trim() == "-1")
            {
                sqlStringCommand.CommandText = string.Format("select * from Hishop_PhotoGallery order by {1}", cid, order);
            }
            else
            {
                sqlStringCommand.CommandText = string.Format("select * from Hishop_PhotoGallery where CategoryId={0} order by {1}", cid, order);
            }
            using (IDataReader reader2 = database.ExecuteReader(sqlStringCommand))
            {
                while (reader2.Read())
                {
                    Hashtable hashtable2 = new Hashtable();
                    hashtable2["cid"] = reader2["CategoryId"];
                    hashtable2["name"] = reader2["PhotoName"];
                    hashtable2["path"] = reader2["PhotoPath"];
                    hashtable2["filesize"] = reader2["FileSize"];
                    hashtable2["addedtime"] = reader2["UploadTime"];
                    hashtable2["updatetime"] = reader2["LastUpdateTime"];
                    string str = reader2["PhotoPath"].ToString().Trim();
                    hashtable2["filetype"] = str.Substring(str.LastIndexOf('.'));
                    list2.Add(hashtable2);
                }
            }
            table["total_count"] = list2.Count;
            table["current_cateogry"] = int.Parse(cid);
        }

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
            if (user.UserRole != UserRole.SiteManager)
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
                    path = "~/Storage/master/gallery/";
                    url = "/Storage/master/gallery/";
                }
                else
                {
                    path = string.Format("{0}/fckfiles/Files/Image/", HiContext.Current.GetSkinPath());
                    if (base.Request.ApplicationPath != "/")
                    {
                        url = path.Substring(base.Request.ApplicationPath.Length);
                    }
                    else
                    {
                        url = path;
                    }
                }
                string str4 = base.Request.QueryString["order"];
                str4 = string.IsNullOrEmpty(str4) ? "uploadtime" : str4.ToLower();
                string cid = base.Request.QueryString["cid"];
                if (cid == null)
                {
                    cid = "-1";
                }
                if (str3 == "false")
                {
                    this.FillTableForDb(cid, str4, table);
                }
                else
                {
                    this.FillTableForPath(path, url, str4, table);
                }
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


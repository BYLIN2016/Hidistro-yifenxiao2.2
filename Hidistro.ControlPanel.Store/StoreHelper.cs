namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Web;
    using System.Xml;

    public static class StoreHelper
    {
        public static void AddHotkeywords(int categoryId, string keywords)
        {
            StoreProvider.Instance().AddHotkeywords(categoryId, keywords);
        }

        public static string BackupData()
        {
            return StoreProvider.Instance().BackupData(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/"));
        }

        public static bool CreateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            return StoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Create);
        }

        public static int CreateVote(VoteInfo vote)
        {
            int num = 0;
            long num2 = StoreProvider.Instance().CreateVote(vote);
            if (num2 > 0L)
            {
                num = 1;
                if (vote.VoteItems == null)
                {
                    return num;
                }
                foreach (VoteItemInfo info in vote.VoteItems)
                {
                    info.VoteId = num2;
                    info.ItemCount = 0;
                    num += StoreProvider.Instance().CreateVoteItem(info, null);
                }
            }
            return num;
        }

        public static bool DeleteBackupFile(string backupName)
        {
            string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    XmlElement element = (XmlElement) node;
                    if (element.GetAttribute("BackupName") == backupName)
                    {
                        document.SelectSingleNode("root").RemoveChild(node);
                    }
                }
                document.Save(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteHotKeywords(int hid)
        {
            StoreProvider.Instance().DeleteHotKeywords(hid);
        }

        public static void DeleteImage(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    string path = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + imageUrl);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                }
            }
        }

        public static int DeleteVote(long voteId)
        {
            return StoreProvider.Instance().DeleteVote(voteId);
        }

        public static int FriendlyLinkDelete(int linkId)
        {
            return StoreProvider.Instance().FriendlyLinkDelete(linkId);
        }

        public static DataTable GetBackupFiles()
        {
            DataTable table = new DataTable();
            table.Columns.Add("BackupName", typeof(string));
            table.Columns.Add("Version", typeof(string));
            table.Columns.Add("FileSize", typeof(string));
            table.Columns.Add("BackupTime", typeof(string));
            string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                XmlElement element = (XmlElement) node;
                DataRow row = table.NewRow();
                row["BackupName"] = element.GetAttribute("BackupName");
                row["Version"] = element.GetAttribute("Version");
                row["FileSize"] = element.GetAttribute("FileSize");
                row["BackupTime"] = element.GetAttribute("BackupTime");
                table.Rows.Add(row);
            }
            return table;
        }

        public static FriendlyLinksInfo GetFriendlyLink(int linkId)
        {
            return StoreProvider.Instance().GetFriendlyLink(linkId);
        }

        public static IList<FriendlyLinksInfo> GetFriendlyLinks()
        {
            return StoreProvider.Instance().GetFriendlyLinks();
        }

        public static string GetHotkeyword(int id)
        {
            return StoreProvider.Instance().GetHotkeyword(id);
        }

        public static DataTable GetHotKeywords()
        {
            return StoreProvider.Instance().GetHotKeywords();
        }

        public static VoteInfo GetVoteById(long voteId)
        {
            return StoreProvider.Instance().GetVoteById(voteId);
        }

        public static int GetVoteCounts(long voteId)
        {
            return StoreProvider.Instance().GetVoteCounts(voteId);
        }

        public static IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            return StoreProvider.Instance().GetVoteItems(voteId);
        }

        public static DataSet GetVotes()
        {
            return StoreProvider.Instance().GetVotes();
        }

        public static bool InserBackInfo(string fileName, string version, long fileSize)
        {
            string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNode node = document.SelectSingleNode("root");
                XmlElement newChild = document.CreateElement("backupfile");
                newChild.SetAttribute("BackupName", fileName);
                newChild.SetAttribute("Version", version.ToString());
                newChild.SetAttribute("FileSize", fileSize.ToString());
                newChild.SetAttribute("BackupTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                node.AppendChild(newChild);
                document.Save(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RestoreData(string bakFullName)
        {
            bool flag = StoreProvider.Instance().RestoreData(bakFullName);
            StoreProvider.Instance().Restor();
            return flag;
        }

        public static int SetVoteIsBackup(long voteId)
        {
            return StoreProvider.Instance().SetVoteIsBackup(voteId);
        }

        public static void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
        {
            StoreProvider.Instance().SwapFriendlyLinkSequence(linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            StoreProvider.Instance().SwapHotWordsSequence(hid, replaceHid, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            return StoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Update);
        }

        public static void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
        {
            StoreProvider.Instance().UpdateHotWords(hid, categoryId, hotKeyWords);
        }

        public static bool UpdateVote(VoteInfo vote)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!StoreProvider.Instance().UpdateVote(vote, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!StoreProvider.Instance().DeleteVoteItem(vote.VoteId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    int num = 0;
                    if (vote.VoteItems != null)
                    {
                        foreach (VoteItemInfo info in vote.VoteItems)
                        {
                            info.VoteId = vote.VoteId;
                            info.ItemCount = 0;
                            num += StoreProvider.Instance().CreateVoteItem(info, dbTran);
                        }
                        if (num < vote.VoteItems.Count)
                        {
                            dbTran.Rollback();
                            return false;
                        }
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static string UploadLinkImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/link/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadLogo(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}


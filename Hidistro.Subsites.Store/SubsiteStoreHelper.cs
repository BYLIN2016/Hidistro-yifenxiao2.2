namespace Hidistro.Subsites.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Web;

    public static class SubsiteStoreHelper
    {
        public static void AddHotkeywords(int categoryId, string keywords)
        {
            SubsiteStoreProvider.Instance().AddHotkeywords(categoryId, keywords);
        }

        public static bool AddInpourBalance(InpourRequestInfo inpourRequest)
        {
            return SubsiteStoreProvider.Instance().AddInpourBlance(inpourRequest);
        }

        public static bool AddSiteRequest(SiteRequestInfo siteRequest)
        {
            return SubsiteStoreProvider.Instance().AddSiteRequest(siteRequest);
        }

        public static bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            return SubsiteStoreProvider.Instance().BalanceDrawRequest(balanceDrawRequest);
        }

        public static bool BindTaobaoSetting(bool isUserHomeSiteApp, string topkey, string topSecret)
        {
            if (HiContext.Current.User.UserRole != UserRole.Distributor)
            {
                return false;
            }
            return SubsiteStoreProvider.Instance().BindTaobaoSetting(isUserHomeSiteApp, topkey, topSecret);
        }

        public static CreateUserStatus CreateDistributor(Distributor distributor)
        {
            return Users.CreateUser(distributor, HiContext.Current.Config.RolesConfiguration.Distributor);
        }

        public static bool CreateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            return SubsiteStoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Create);
        }

        public static int CreateVote(VoteInfo vote)
        {
            int num = 0;
            long num2 = SubsiteStoreProvider.Instance().CreateVote(vote);
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
                    num += SubsiteStoreProvider.Instance().CreateVoteItem(info, null);
                }
            }
            return num;
        }

        public static void DeleteHotKeywords(int hid)
        {
            SubsiteStoreProvider.Instance().DeleteHotKeywords(hid);
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

        public static void DeleteSiteRequest()
        {
            SubsiteStoreProvider.Instance().DeleteSiteRequest();
        }

        public static int DeleteVote(long voteId)
        {
            return SubsiteStoreProvider.Instance().DeleteVote(voteId);
        }

        public static bool DistroHasDrawRequest()
        {
            return SubsiteStoreProvider.Instance().DistroHasDrawRequest();
        }

        public static int FriendlyLinkDelete(int linkId)
        {
            return SubsiteStoreProvider.Instance().FriendlyLinkDelete(linkId);
        }

        public static Distributor GetDistributor()
        {
            IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
            if ((user != null) && (user.UserRole == UserRole.Distributor))
            {
                return (user as Distributor);
            }
            return null;
        }

        public static DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
        {
            return SubsiteStoreProvider.Instance().GetDistributorGradeInfo(gradeId);
        }

        public static IList<PaymentModeInfo> GetDistributorPaymentModes()
        {
            return SubsiteStoreProvider.Instance().GetDistributorPaymentModes();
        }

        public static FriendlyLinksInfo GetFriendlyLink(int linkId)
        {
            return SubsiteStoreProvider.Instance().GetFriendlyLink(linkId);
        }

        public static IList<FriendlyLinksInfo> GetFriendlyLinks()
        {
            return SubsiteStoreProvider.Instance().GetFriendlyLinks();
        }

        public static DataTable GetHotKeywords()
        {
            return SubsiteStoreProvider.Instance().GetHotKeywords();
        }

        public static InpourRequestInfo GetInpouRequest(string inpourId)
        {
            return SubsiteStoreProvider.Instance().GetInpouRequest(inpourId);
        }

        public static AccountSummaryInfo GetMyAccountSummary()
        {
            return SubsiteStoreProvider.Instance().GetMyAccountSummary();
        }

        public static DbQueryResult GetMyBalanceDetails(BalanceDetailQuery query)
        {
            return SubsiteStoreProvider.Instance().GetMyBalanceDetails(query);
        }

        public static SiteRequestInfo GetMySiteRequest()
        {
            return SubsiteStoreProvider.Instance().GetMySiteRequest();
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return SubsiteStoreProvider.Instance().GetPaymentMode(modeId);
        }

        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return SubsiteStoreProvider.Instance().GetPaymentMode(gateway);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return SubsiteStoreProvider.Instance().GetPaymentModes();
        }

        public static VoteInfo GetVoteById(long voteId)
        {
            return SubsiteStoreProvider.Instance().GetVoteById(voteId);
        }

        public static int GetVoteCounts(long voteId)
        {
            return SubsiteStoreProvider.Instance().GetVoteCounts(voteId);
        }

        public static IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            return SubsiteStoreProvider.Instance().GetVoteItems(voteId);
        }

        public static DataSet GetVotes()
        {
            return SubsiteStoreProvider.Instance().GetVotes();
        }

        public static bool IsExitSiteUrl(string siteUrl)
        {
            return SubsiteStoreProvider.Instance().IsExitSiteUrl(siteUrl);
        }

        public static bool Recharge(BalanceDetailInfo balanceDetails)
        {
            bool flag = SubsiteStoreProvider.Instance().IsRecharge(balanceDetails.InpourId);
            if (!flag)
            {
                flag = SubsiteStoreProvider.Instance().AddBalanceDetail(balanceDetails);
                SubsiteStoreProvider.Instance().RemoveInpourRequest(balanceDetails.InpourId);
            }
            return flag;
        }

        public static void RemoveInpourRequest(string inpourId)
        {
            SubsiteStoreProvider.Instance().RemoveInpourRequest(inpourId);
        }

        public static int SetVoteIsBackup(long voteId)
        {
            return SubsiteStoreProvider.Instance().SetVoteIsBackup(voteId);
        }

        public static void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteStoreProvider.Instance().SwapFriendlyLinkSequence(linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteStoreProvider.Instance().SwapHotWordsSequence(hid, replaceHid, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateDistributor(Distributor distributor)
        {
            Globals.EntityCoding(distributor, true);
            return Users.UpdateUser(distributor);
        }

        public static bool UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            return SubsiteStoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Update);
        }

        public static void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
        {
            SubsiteStoreProvider.Instance().UpdateHotWords(hid, categoryId, hotKeyWords);
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
                    if (!SubsiteStoreProvider.Instance().UpdateVote(vote, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsiteStoreProvider.Instance().DeleteVoteItem(vote.VoteId, dbTran))
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
                            num += SubsiteStoreProvider.Instance().CreateVoteItem(info, dbTran);
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
            string str = HiContext.Current.GetStoragePath() + "/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static LoginUserStatus ValidLogin(Distributor distributor)
        {
            if (distributor == null)
            {
                return LoginUserStatus.InvalidCredentials;
            }
            return Users.ValidateUser(distributor);
        }
    }
}


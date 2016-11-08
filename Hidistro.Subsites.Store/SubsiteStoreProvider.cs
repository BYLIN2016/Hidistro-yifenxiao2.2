namespace Hidistro.Subsites.Store
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Hidistro.Core;

    public abstract class SubsiteStoreProvider
    {
        private static readonly SubsiteStoreProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.StoreData,Hidistro.Subsites.Data") as SubsiteStoreProvider);

        protected SubsiteStoreProvider()
        {
        }

        public abstract bool AddBalanceDetail(BalanceDetailInfo balanceDetails);
        public abstract void AddHotkeywords(int categoryId, string keywords);
        public abstract bool AddInpourBlance(InpourRequestInfo inpourRequest);
        public abstract bool AddSiteRequest(SiteRequestInfo siteRequest);
        public abstract bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest);
        public abstract bool BindTaobaoSetting(bool isUserHomeSiteApp, string topkey, string topSecret);
        public abstract bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action);
        public abstract long CreateVote(VoteInfo vote);
        public abstract int CreateVoteItem(VoteItemInfo voteItem, DbTransaction dbTran);
        public abstract void DeleteHotKeywords(int hId);
        public abstract void DeleteSiteRequest();
        public abstract int DeleteVote(long voteId);
        public abstract bool DeleteVoteItem(long voteItemId, DbTransaction dbTran);
        public abstract bool DistroHasDrawRequest();
        public abstract int FriendlyLinkDelete(int linkId);
        public abstract DistributorGradeInfo GetDistributorGradeInfo(int gradeId);
        public abstract IList<PaymentModeInfo> GetDistributorPaymentModes();
        public abstract FriendlyLinksInfo GetFriendlyLink(int linkId);
        public abstract IList<FriendlyLinksInfo> GetFriendlyLinks();
        public abstract DataTable GetHotKeywords();
        public abstract InpourRequestInfo GetInpouRequest(string inpourId);
        public abstract AccountSummaryInfo GetMyAccountSummary();
        public abstract DbQueryResult GetMyBalanceDetails(BalanceDetailQuery query);
        public abstract SiteRequestInfo GetMySiteRequest();
        public abstract PaymentModeInfo GetPaymentMode(int modeId);
        public abstract PaymentModeInfo GetPaymentMode(string gateway);
        public abstract IList<PaymentModeInfo> GetPaymentModes();
        public abstract VoteInfo GetVoteById(long voteId);
        public abstract int GetVoteCounts(long voteId);
        public abstract IList<VoteItemInfo> GetVoteItems(long voteId);
        public abstract DataSet GetVotes();
        public static SubsiteStoreProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool IsExitSiteUrl(string siteUrl);
        public abstract bool IsRecharge(string inpourId);
        public abstract void RemoveInpourRequest(string inpourId);
        public abstract int SetVoteIsBackup(long voteId);
        public abstract void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence);
        public abstract void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence);
        public abstract void UpdateHotWords(int hid, int categoryId, string hotKeyWords);
        public abstract bool UpdateVote(VoteInfo vote, DbTransaction dbTran);
    }
}


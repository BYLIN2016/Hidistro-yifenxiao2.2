namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Hidistro.Core;

    public abstract class StoreProvider
    {
        private static readonly StoreProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.StoreData,Hidistro.ControlPanel.Data") as StoreProvider);

        protected StoreProvider()
        {
        }

        public abstract void AddHotkeywords(int categoryId, string keywords);
        public abstract string BackupData(string path);
        public abstract void ClearRolePrivilege(Guid roleId);
        public abstract bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action);
        public abstract long CreateVote(VoteInfo vote);
        public abstract int CreateVoteItem(VoteItemInfo voteItem, DbTransaction dbTran);
        public abstract bool DeleteAllLogs();
        public abstract void DeleteHotKeywords(int hId);
        public abstract bool DeleteLog(long logId);
        public abstract int DeleteLogs(string strIds);
        public abstract bool DeleteManager(int userId);
        public abstract void DeleteQueuedCellphone(Guid cellphoneId);
        public abstract int DeleteVote(long voteId);
        public abstract bool DeleteVoteItem(long voteId, DbTransaction dbTran);
        public abstract DataTable DequeueCellphone();
        public abstract void EnqueuCellphone(string cellphoneNumber, string subject);
        public abstract int FriendlyLinkDelete(int linkId);
        public abstract FriendlyLinksInfo GetFriendlyLink(int linkId);
        public abstract IList<FriendlyLinksInfo> GetFriendlyLinks();
        public abstract string GetHotkeyword(int id);
        public abstract DataTable GetHotKeywords();
        public abstract DbQueryResult GetLogs(OperationLogQuery query);
        public abstract DbQueryResult GetManagers(ManagerQuery query);
        public abstract IList<string> GetOperationUserNames();
        public abstract VoteInfo GetVoteById(long voteId);
        public abstract int GetVoteCounts(long voteId);
        public abstract IList<VoteItemInfo> GetVoteItems(long voteId);
        public abstract DataSet GetVotes();
        public static StoreProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract void QueueSendingFailure(IList<Guid> cellphoneIds);
        public abstract void Restor();
        public abstract bool RestoreData(string bakFullName);
        public abstract int SetVoteIsBackup(long voteId);
        public abstract void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence);
        public abstract void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence);
        public abstract void UpdateHotWords(int hid, int categoryId, string hotKeyWords);
        public abstract bool UpdateVote(VoteInfo vote, DbTransaction dbTran);
        public abstract void WriteOperationLogEntry(OperationLogEntry entry);
    }
}


namespace Hidistro.SaleSystem.DistributionData
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class CommentData : CommentSubsiteProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override AfficheInfo GetAffiche(int afficheId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Affiche WHERE AfficheId = @AfficheId");
            this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
            AfficheInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateAffiche(reader);
                }
            }
            return info;
        }

        public override List<AfficheInfo> GetAfficheList()
        {
            List<AfficheInfo> list = new List<AfficheInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Affiche WHERE DistributorUserId=@DistributorUserId  ORDER BY AddedDate DESC");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    AfficheInfo item = DataMapper.PopulateAffiche(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override DataSet GetAllHotKeywords()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT CategoryId, Name AS CategoryName, RewriteName FROM distro_Categories WHERE DistributorUserId = {0} AND Depth = 1 ORDER BY DisplaySequence;", HiContext.Current.SiteSettings.UserId.Value) + string.Format(" SELECT * FROM distro_Hotkeywords WHERE DistributorUserId = {0} ORDER BY Frequency DESC", HiContext.Current.SiteSettings.UserId.Value));
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            set.Relations.Add("relation", set.Tables[0].Columns["CategoryId"], set.Tables[1].Columns["CategoryId"], false);
            return set;
        }

        public override ArticleInfo GetArticle(int articleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Articles WHERE ArticleId = @ArticleId AND DistributorUserId = @DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            ArticleInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateArticle(reader);
                }
            }
            return info;
        }

        public override ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            ArticleCategoryInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From distro_ArticleCategories WHERE CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateArticleCategory(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" IsRelease=1 ");
            if (!string.IsNullOrEmpty(articleQuery.Keywords))
            {
                builder.AppendFormat(" AND Title LIKE '%{0}%'", articleQuery.Keywords);
            }
            if (articleQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND CategoryId = {0}", articleQuery.CategoryId.Value);
            }
            if (articleQuery.StartArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >= '{0}'", articleQuery.StartArticleTime.Value);
            }
            if (articleQuery.EndArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <= '{0}'", articleQuery.EndArticleTime.Value);
            }
            builder.AppendFormat(" AND DistributorUserId = {0} ", HiContext.Current.SiteSettings.UserId.Value);
            return DataHelper.PagingByRownumber(articleQuery.PageIndex, articleQuery.PageSize, articleQuery.SortBy, articleQuery.SortOrder, articleQuery.IsCount, "vw_distro_Articles", "ArticleId", builder.ToString(), "*");
        }

        public override IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT TOP {0} * FROM vw_distro_Articles WHERE DistributorUserId = {1} and IsRelease=1", maxNum, HiContext.Current.SiteSettings.UserId.Value);
            if (categoryId != 0)
            {
                builder.AppendFormat(" AND CategoryId = {0}", categoryId);
            }
            builder.Append(" ORDER BY AddedDate DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            IList<ArticleInfo> list = new List<ArticleInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ArticleInfo item = DataMapper.PopulateArticle(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override IList<ArticleCategoryInfo> GetArticleMainCategories()
        {
            IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_ArticleCategories WHERE DistributorUserId=@DistributorUserId ORDER BY [DisplaySequence]");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ArticleCategoryInfo item = DataMapper.PopulateArticleCategory(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override DataTable GetArticlProductList(int articlId)
        {
            return null;
        }

        public override IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? number)
        {
            IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
            string query = string.Empty;
            if (number.HasValue)
            {
                query = string.Format("SELECT Top {0} * FROM distro_FriendlyLinks WHERE  Visible = 1 AND DistributorUserId=@DistributorUserId ORDER BY DisplaySequence DESC", number.Value);
            }
            else
            {
                query = string.Format("SELECT * FROM distro_FriendlyLinks WHERE  Visible = 1 and DistributorUserId=@DistributorUserId ORDER BY DisplaySequence DESC", new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateFriendlyLink(reader));
                }
            }
            return list;
        }

        public override AfficheInfo GetFrontOrNextAffiche(int afficheId, string type)
        {
            string query = string.Empty;
            if (type == "Next")
            {
                query = "SELECT TOP 1 * FROM distro_Affiche WHERE AfficheId< @AfficheId AND DistributorUserId=@DistributorUserId ORDER BY AfficheId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM distro_Affiche WHERE AfficheId> @AfficheId AND DistributorUserId=@DistributorUserId ORDER BY AfficheId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            AfficheInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateAffiche(reader);
                }
            }
            return info;
        }

        public override ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
        {
            string query = string.Empty;
            if (type == "Next")
            {
                query = "SELECT TOP 1 * FROM distro_Articles WHERE ArticleId< @ArticleId AND CategoryId=@CategoryId AND DistributorUserId=@DistributorUserId AND IsRelease=1 ORDER BY ArticleId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM distro_Articles WHERE ArticleId> @ArticleId AND CategoryId=@CategoryId AND DistributorUserId=@DistributorUserId AND IsRelease=1 ORDER BY ArticleId ASC";
            }
            ArticleInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateArticle(reader);
                }
            }
            return info;
        }

        public override HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type)
        {
            string query = string.Empty;
            if (type == "Next")
            {
                query = "SELECT TOP 1 * FROM distro_Helps WHERE HelpId< @HelpId AND CategoryId=@CategoryId AND DistributorUserId = @DistributorUserId ORDER BY HelpId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM distro_Helps WHERE HelpId> @HelpId AND CategoryId=@CategoryId AND DistributorUserId = @DistributorUserId ORDER BY HelpId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            HelpInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateHelp(reader);
                }
                reader.Close();
            }
            return info;
        }

        public override PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type)
        {
            string query = string.Empty;
            if (type == "Next")
            {
                query = "SELECT TOP 1 * FROM distro_Promotions WHERE activityId<@activityId AND PromoteType=@PromoteType AND DistributorUserId=@DistributorUserId ORDER BY activityId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM distro_Promotions WHERE activityId>@activityId AND PromoteType=@PromoteType AND DistributorUserId=@DistributorUserId ORDER BY activityId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "activityId", DbType.Int32, promote.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, Convert.ToInt32(promote.PromoteType));
            PromotionInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
                reader.Close();
            }
            return info;
        }

        public override HelpInfo GetHelp(int helpId)
        {
            HelpInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Helps WHERE HelpId=@HelpId AND DistributorUserId = @DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateHelp(reader);
                }
            }
            return info;
        }

        public override HelpCategoryInfo GetHelpCategory(int categoryId)
        {
            HelpCategoryInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_HelpCategories WHERE CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateHelpCategory(reader);
                }
            }
            return info;
        }

        public override IList<HelpCategoryInfo> GetHelpCategorys()
        {
            IList<HelpCategoryInfo> list = new List<HelpCategoryInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_HelpCategories WHERE DistributorUserId=@DistributorUserId ORDER BY DisplaySequence");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateHelpCategory(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetHelpList(HelpQuery helpQuery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Title LIKE '%{0}%'", helpQuery.Keywords);
            if (helpQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND CategoryId = {0}", helpQuery.CategoryId.Value);
            }
            if (helpQuery.StartArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >= '{0}'", helpQuery.StartArticleTime.Value);
            }
            if (helpQuery.EndArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <= '{0}'", helpQuery.EndArticleTime.Value);
            }
            builder.AppendFormat(" AND DistributorUserId = {0}", HiContext.Current.SiteSettings.UserId.Value);
            return DataHelper.PagingByTopnotin(helpQuery.PageIndex, helpQuery.PageSize, helpQuery.SortBy, helpQuery.SortOrder, helpQuery.IsCount, "vw_distro_Helps", "HelpId", builder.ToString(), "*");
        }

        public override DataSet GetHelps()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_HelpCategories WHERE IsShowFooter = 1 AND DistributorUserId=@DistributorUserId ORDER BY DisplaySequence SELECT * FROM distro_Helps WHERE IsShowFooter = 1  AND CategoryId IN (SELECT CategoryId FROM distro_HelpCategories WHERE IsShowFooter = 1 AND DistributorUserId=@DistributorUserId)");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["CateGoryId"];
            DataColumn childColumn = set.Tables[1].Columns["CateGoryId"];
            DataRelation relation = new DataRelation("CateGory", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public override DataSet GetHelpTitleList()
        {
            string query = "SELECT * FROM distro_HelpCategories where DistributorUserId=@DistributorUserId order by DisplaySequence";
            query = query + " SELECT HelpId,Title,CategoryId FROM distro_Helps where DistributorUserId=@DistributorUserId AND CategoryId IN (SELECT CategoryId FROM distro_HelpCategories WHERE DistributorUserId=@DistributorUserId)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["CateGoryId"];
            DataColumn childColumn = set.Tables[1].Columns["CateGoryId"];
            DataRelation relation = new DataRelation("CateGory", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public override DataTable GetHotKeywords(int categoryId, int hotKeywordsNum)
        {
            string query = string.Format("SELECT TOP {0} * FROM distro_Hotkeywords WHERE DistributorUserId={1}", hotKeywordsNum, HiContext.Current.SiteSettings.UserId.Value);
            if (categoryId != 0)
            {
                query = query + string.Format(" AND CategoryId = {0}", categoryId);
            }
            query = query + " ORDER BY Frequency DESC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT l.LeaveId FROM distro_LeaveComments l ");
            builder.AppendFormat(" WHERE IsReply = 1 AND DistributorUserId = {0}", HiContext.Current.SiteSettings.UserId.Value);
            builder.Append(" ORDER BY LastDate desc");
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_LeaveComments_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, builder.ToString());
            this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            DataSet set = this.database.ExecuteDataSet(storedProcCommand);
            set.Relations.Add("LeaveCommentReplays", set.Tables[0].Columns["LeaveId"], set.Tables[1].Columns["LeaveId"], false);
            result.Data = set;
            result.TotalRecords = (int) this.database.GetParameterValue(storedProcCommand, "Total");
            return result;
        }

        public override PromotionInfo GetPromote(int activityId)
        {
            PromotionInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Promotions WHERE DistributorUserId=@DistributorUserId AND ActivityId=@ActivityId");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
                reader.Close();
            }
            return info;
        }

        public override DataTable GetPromotes(Pagination pagination, int promotiontype, out int totalPromotes)
        {
            string query = string.Format("SELECT COUNT(*) FROM distro_Promotions WHERE DistributorUserId={0} ", HiContext.Current.SiteSettings.UserId.Value);
            if (promotiontype != 0)
            {
                query = query + string.Format(" AND PromoteType={0} ", promotiontype);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            totalPromotes = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string str2 = string.Empty;
            StringBuilder builder = new StringBuilder("case distro_Promotions.PromoteType");
            builder.AppendFormat(" when 1 then '商品直接打折'", new object[0]);
            builder.AppendFormat(" when 2 then '商品固定金额出售'", new object[0]);
            builder.AppendFormat(" when 3 then '商品减价优惠'", new object[0]);
            builder.AppendFormat(" when 4 then '批发打折'", new object[0]);
            builder.AppendFormat(" when 5 then '买商品赠送礼品'", new object[0]);
            builder.AppendFormat(" when 6 then '商品有买有送'", new object[0]);
            builder.AppendFormat(" when 11 then '订单满额打折'", new object[0]);
            builder.AppendFormat(" when 12 then '订单满额优惠金额'", new object[0]);
            builder.AppendFormat(" when 13 then '混合批发打折'", new object[0]);
            builder.AppendFormat(" when 14 then '混合批发优惠金额'", new object[0]);
            builder.AppendFormat(" when 15 then '订单满额送礼品'", new object[0]);
            builder.AppendFormat(" when 16 then '订单满额送倍数积分'", new object[0]);
            builder.AppendFormat(" when 17 then '订单满额免运费'", new object[0]);
            builder.Append(" end as PromoteTypeName");
            if (pagination.PageIndex == 1)
            {
                str2 = string.Format("SELECT TOP 10 *," + builder + " FROM distro_Promotions WHERE DistributorUserId={0} ", HiContext.Current.SiteSettings.UserId.Value);
            }
            else
            {
                str2 = string.Format("SELECT TOP {0} *," + builder + " FROM distro_Promotions WHERE  DistributorUserId={1}  AND ActivityId NOT IN (SELECT TOP {2} ActivityId FROM distro_Promotions WHERE AND DistributorUserId={3} )  ", new object[] { pagination.PageSize, HiContext.Current.SiteSettings.UserId.Value, pagination.PageSize * (pagination.PageIndex - 1), HiContext.Current.SiteSettings.UserId.Value });
            }
            if (promotiontype != 0)
            {
                str2 = str2 + string.Format(" AND PromoteType={0} ", promotiontype);
            }
            str2 = str2 + " ORDER BY ActivityId DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(str2);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override VoteInfo GetVoteById(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM distro_Votes WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            VoteInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateVote(reader);
                }
            }
            return info;
        }

        public override DataSet GetVoteByIsShow()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Votes WHERE IsBackup = 1 AND DistributorUserId=@DistributorUserId SELECT * FROM distro_VoteItems WHERE  voteId IN (SELECT voteId FROM distro_Votes WHERE IsBackup = 1 AND DistributorUserId=@DistributorUserId)");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["VoteId"];
            DataColumn childColumn = set.Tables[1].Columns["VoteId"];
            DataRelation relation = new DataRelation("Vote", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public override VoteItemInfo GetVoteItemById(long voteItemId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_VoteItems WHERE VoteItemId = @VoteItemId");
            this.database.AddInParameter(sqlStringCommand, "VoteItemId", DbType.Int64, voteItemId);
            VoteItemInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateVoteItem(reader);
                }
            }
            return info;
        }

        public override IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            IList<VoteItemInfo> list = new List<VoteItemInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                VoteItemInfo item = null;
                while (reader.Read())
                {
                    item = DataMapper.PopulateVoteItem(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override bool InsertLeaveComment(LeaveCommentInfo leave)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Insert into distro_LeaveComments(UserId,DistributorUserId,UserName,Title,PublishContent,PublishDate,LastDate) values(@UserId,@DistributorUserId,@UserName,@Title,@PublishContent,@PublishDate,@LastDate)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, leave.UserId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, leave.UserName);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, leave.Title);
            this.database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, leave.PublishContent);
            this.database.AddInParameter(sqlStringCommand, "PublishDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
            this.database.AddInParameter(sqlStringCommand, "LastDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int Vote(long voteItemId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_VoteItems SET ItemCount = ItemCount + 1 WHERE VoteItemId = @VoteItemId");
            this.database.AddInParameter(sqlStringCommand, "VoteItemId", DbType.Int32, voteItemId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}


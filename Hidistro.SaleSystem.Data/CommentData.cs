namespace Hidistro.SaleSystem.Data
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Comments;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class CommentData : CommentMasterProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override AfficheInfo GetAffiche(int afficheId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche WHERE AfficheId = @AfficheId");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche  ORDER BY AddedDate DESC");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId, Name AS CategoryName, RewriteName FROM Hishop_Categories WHERE Depth = 1 ORDER BY DisplaySequence; SELECT * FROM Hishop_Hotkeywords ORDER BY Frequency DESC");
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            set.Relations.Add("relation", set.Tables[0].Columns["CategoryId"], set.Tables[1].Columns["CategoryId"], false);
            return set;
        }

        public override ArticleInfo GetArticle(int articleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Articles WHERE ArticleId = @ArticleId");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From Hishop_ArticleCategories WHERE CategoryId=@CategoryId");
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
            builder.Append("IsRelease=1");
            builder.AppendFormat(" and Title LIKE '%{0}%'", articleQuery.Keywords);
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
            return DataHelper.PagingByRownumber(articleQuery.PageIndex, articleQuery.PageSize, articleQuery.SortBy, articleQuery.SortOrder, articleQuery.IsCount, "vw_Hishop_Articles", "ArticleId", builder.ToString(), "*");
        }

        public override IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT TOP {0} * FROM vw_Hishop_Articles WHERE IsRelease=1", maxNum);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ArticleCategories ORDER BY [DisplaySequence]");
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
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220 FROM Hishop_Products");
            builder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedArticsProducts WHERE ArticleId = {1})", 1, articlId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? number)
        {
            IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
            string query = string.Empty;
            if (number.HasValue)
            {
                query = string.Format("SELECT Top {0} * FROM Hishop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence DESC", number.Value);
            }
            else
            {
                query = string.Format("SELECT * FROM Hishop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence DESC", new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
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
                query = "SELECT TOP 1 * FROM Hishop_Affiche WHERE AfficheId < @AfficheId  ORDER BY AfficheId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM Hishop_Affiche WHERE AfficheId > @AfficheId ORDER BY AfficheId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
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

        public override ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
        {
            string query = string.Empty;
            if (type == "Next")
            {
                query = "SELECT TOP 1 * FROM Hishop_Articles WHERE ArticleId < @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM Hishop_Articles WHERE  ArticleId > @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId ASC";
            }
            ArticleInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
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
                query = "SELECT TOP 1 * FROM Hishop_Helps WHERE HelpId <@HelpId AND CategoryId=@CategoryId ORDER BY HelpId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM Hishop_Helps WHERE HelpId >@HelpId AND CategoryId=@CategoryId ORDER BY HelpId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
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
                query = "SELECT TOP 1 * FROM Hishop_Promotions WHERE activityId<@activityId AND PromoteType=@PromoteType  ORDER BY activityId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM Hishop_Promotions WHERE activityId>@activityId AND PromoteType=@PromoteType ORDER BY activityId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Helps WHERE HelpId=@HelpId");
            this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories WHERE CategoryId=@CategoryId");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories ORDER BY DisplaySequence");
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
            return DataHelper.PagingByTopnotin(helpQuery.PageIndex, helpQuery.PageSize, helpQuery.SortBy, helpQuery.SortOrder, helpQuery.IsCount, "vw_Hishop_Helps", "HelpId", builder.ToString(), "*");
        }

        public override DataSet GetHelps()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories WHERE IsShowFooter = 1 ORDER BY DisplaySequence SELECT * FROM Hishop_Helps WHERE IsShowFooter = 1  AND CategoryId IN (SELECT CategoryId FROM Hishop_HelpCategories WHERE IsShowFooter = 1)");
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["CateGoryId"];
            DataColumn childColumn = set.Tables[1].Columns["CateGoryId"];
            DataRelation relation = new DataRelation("CateGory", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public override DataSet GetHelpTitleList()
        {
            string format = "SELECT * FROM Hishop_HelpCategories order by DisplaySequence";
            format = format + " SELECT HelpId,Title,CategoryId FROM Hishop_Helps where CategoryId IN (SELECT CategoryId FROM Hishop_HelpCategories)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(format, new object[0]));
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["CateGoryId"];
            DataColumn childColumn = set.Tables[1].Columns["CateGoryId"];
            DataRelation relation = new DataRelation("CateGory", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public override DataTable GetHotKeywords(int categoryId, int hotKeywordsNum)
        {
            string query = string.Format("SELECT TOP {0} * FROM Hishop_Hotkeywords", hotKeywordsNum);
            if (categoryId != 0)
            {
                query = query + string.Format(" WHERE CategoryId = {0}", categoryId);
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
            builder.Append(" SELECT l.LeaveId FROM Hishop_LeaveComments l ");
            builder.Append(" WHERE IsReply = 1 ");
            builder.Append(" ORDER BY LastDate desc");
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_LeaveComments_Get");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE ActivityId=@ActivityId");
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

        public override DataTable GetPromotes(Pagination pagination, int promotionType, out int totalPromotes)
        {
            string query = string.Format("SELECT COUNT(*) FROM Hishop_Promotions WHERE 1=1 ", new object[0]);
            if (promotionType != 0)
            {
                query = query + string.Format(" AND PromoteType={0} ", promotionType);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            totalPromotes = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string str2 = string.Empty;
            StringBuilder builder = new StringBuilder("case Hishop_Promotions.PromoteType");
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
                str2 = "SELECT TOP 10 *," + builder + " FROM Hishop_Promotions WHERE 1=1 ";
            }
            else
            {
                str2 = string.Format("SELECT TOP {0} *," + builder + " FROM Hishop_Promotions WHERE ActivityId NOT IN (SELECT TOP {1} ActivityId FROM Hishop_Promotions) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
            }
            if (promotionType != 0)
            {
                str2 = str2 + string.Format(" AND PromoteType={0} ", promotionType);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM Hishop_Votes WHERE VoteId = @VoteId");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Votes WHERE IsBackup = 1 SELECT * FROM Hishop_VoteItems WHERE  voteId IN (SELECT voteId FROM Hishop_Votes WHERE IsBackup = 1)");
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["VoteId"];
            DataColumn childColumn = set.Tables[1].Columns["VoteId"];
            DataRelation relation = new DataRelation("Vote", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public override VoteItemInfo GetVoteItemById(long voteItemId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteItemId = @VoteItemId");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Insert into Hishop_LeaveComments(UserId,UserName,Title,PublishContent,PublishDate,LastDate)  values(@UserId,@UserName,@Title,@PublishContent,@PublishDate,@LastDate)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, leave.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, leave.UserName);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, leave.Title);
            this.database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, leave.PublishContent);
            this.database.AddInParameter(sqlStringCommand, "PublishDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
            this.database.AddInParameter(sqlStringCommand, "LastDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int Vote(long voteItemId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_VoteItems SET ItemCount = ItemCount + 1 WHERE VoteItemId = @VoteItemId");
            this.database.AddInParameter(sqlStringCommand, "VoteItemId", DbType.Int32, voteItemId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}


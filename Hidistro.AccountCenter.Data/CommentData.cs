namespace Hidistro.AccountCenter.Data
{
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public class CommentData : CommentMasterDataProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddProductToFavorite(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Favorite(ProductId, UserId, Tags, Remark)VALUES(@ProductId, @UserId, @Tags, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "Tags", DbType.String, string.Empty);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, string.Empty);
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string BuildConsultationAndReplyQuery(ProductConsultationAndReplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT ConsultationId FROM Hishop_ProductConsultations ");
            builder.Append(" WHERE 1 = 1");
            if (query.ProductId > 0)
            {
                builder.AppendFormat(" AND ProductId = {0} ", query.ProductId);
            }
            if (query.UserId > 0)
            {
                builder.AppendFormat(" AND UserId = {0} ", query.UserId);
            }
            if (query.Type == ConsultationReplyType.NoReply)
            {
                builder.Append(" AND ReplyText IS NULL");
            }
            else if (query.Type == ConsultationReplyType.Replyed)
            {
                builder.Append(" AND ReplyText IS NOT NULL");
            }
            builder.Append(" ORDER BY replydate DESC");
            return builder.ToString();
        }

        private static string BuildFavoriteQuery(int userId, string tags)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SELECT FavoriteId FROM Hishop_Favorite WHERE UserId = {0} ", userId);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1) ", new object[0]);
            if (!string.IsNullOrEmpty(tags))
            {
                builder.AppendFormat(" AND (ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1 AND ProductName LIKE '%{0}%') ", DataHelper.CleanSearchString(tags));
                builder.AppendFormat(" OR Tags LIKE '%{0}%')", DataHelper.CleanSearchString(tags));
            }
            builder.AppendFormat(" ORDER BY FavoriteId DESC", new object[0]);
            return builder.ToString();
        }

        private static string BuildUserReviewsAndReplysQuery(UserProductReviewAndReplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT ProductId FROM Hishop_ProductReviews ");
            builder.AppendFormat(" WHERE UserId = {0} ", HiContext.Current.User.UserId);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_Products)", new object[0]);
            builder.Append(" GROUP BY ProductId");
            return builder.ToString();
        }

        public override int DeleteFavorite(int favoriteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Favorite WHERE FavoriteId = @FavoriteId");
            this.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteFavorites(string ids)
        {
            string query = "DELETE from Hishop_Favorite WHERE FavoriteId IN (" + ids + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override int DeleteMemberMessages(IList<long> messageList)
        {
            string str = string.Empty;
            foreach (long num in messageList)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = str + num.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    str = str + "," + num.ToString(CultureInfo.InvariantCulture);
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("delete from Hishop_MemberMessageBox where MessageId in ({0}) ", str));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool ExistsProduct(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Favorite WHERE UserId=@UserId AND ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override DbQueryResult GetBatchBuyProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SaleStatus=1");
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND SKU LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            Member user = HiContext.Current.User as Member;
            int memberDiscount = this.GetMemberDiscount(user.GradeId);
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("SkuId, ProductId, SKU,ProductName, ThumbnailUrl40, DisplaySequence, Stock,");
            builder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
            builder2.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0})", user.GradeId);
            builder2.AppendFormat(" ELSE SalePrice * {0} /100 END) AS SalePrice", memberDiscount);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_ProductSkuList s", "SkuId", builder.ToString(), builder2.ToString());
        }

        public override DbQueryResult GetFavorites(int userId, string tags, Pagination page)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_Favorites_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, page.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, page.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, page.IsCount);
            Member user = HiContext.Current.User as Member;
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, user.GradeId);
            this.database.AddInParameter(storedProcCommand, "SqlPopulate", DbType.String, BuildFavoriteQuery(userId, tags));
            this.database.AddOutParameter(storedProcCommand, "TotalFavorites", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (page.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        private int GetMemberDiscount(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_MemberGrades WHERE GradeId=@GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override MessageBoxInfo GetMemberMessage(long messageId)
        {
            MessageBoxInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_MemberMessageBox WHERE MessageId=@MessageId;");
            this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateMessageBox(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query)
        {
            string filter = string.Format("Accepter='{0}'", query.Accepter);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_MemberMessageBox", "MessageId", filter, "*");
        }

        public override DbQueryResult GetMemberSendedMessages(MessageBoxQuery query)
        {
            string filter = string.Format("Sernder='{0}'", query.Sernder);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_MemberMessageBox", "MessageId", filter, "*");
        }

        public override DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_ConsultationsAndReplys_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildConsultationAndReplyQuery(query));
            this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            DataSet set = this.database.ExecuteDataSet(storedProcCommand);
            set.Relations.Add("ConsultationReplys", set.Tables[0].Columns["ConsultationId"], set.Tables[1].Columns["ConsultationId"], false);
            total = (int) this.database.GetParameterValue(storedProcCommand, "Total");
            return set;
        }

        public override ProductInfo GetProductDetails(int productId)
        {
            ProductInfo info = new ProductInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
            }
            return info;
        }

        public override DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_UserReviewsAndReplys_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserReviewsAndReplysQuery(query));
            this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            DataSet set = this.database.ExecuteDataSet(storedProcCommand);
            set.Relations.Add("PtReviews", set.Tables[0].Columns["ProductId"], set.Tables[1].Columns["ProductId"], false);
            total = (int) this.database.GetParameterValue(storedProcCommand, "Total");
            return set;
        }

        public override int GetUserProductReviewsCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ReviewId) AS Count FROM Hishop_ProductReviews WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            int num = 0;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return num;
                }
                if (DBNull.Value != reader["Count"])
                {
                    num = (int) reader["Count"];
                }
            }
            return num;
        }

        public override bool InsertMessage(MessageBoxInfo messageBoxInfo)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("BEGIN TRAN ");
            builder.Append("DECLARE @ContentId int ");
            builder.Append("DECLARE @errorSun INT ");
            builder.Append("SET @errorSun=0 ");
            builder.Append("INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) ");
            builder.Append("VALUES(@Title,@Content,@Date) ");
            builder.Append("SET @ContentId = @@IDENTITY  ");
            builder.Append("SET @errorSun=@errorSun+@@ERROR  ");
            builder.Append("INSERT INTO [Hishop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
            builder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
            builder.Append("SET @errorSun=@errorSun+@@ERROR  ");
            builder.Append("INSERT INTO [Hishop_ManagerMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
            builder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
            builder.Append("SET @errorSun=@errorSun+@@ERROR  ");
            builder.Append("IF @errorSun<>0 ");
            builder.Append("BEGIN ");
            builder.Append("ROLLBACK TRANSACTION  ");
            builder.Append("END ");
            builder.Append("ELSE ");
            builder.Append("BEGIN ");
            builder.Append("COMMIT TRANSACTION  ");
            builder.Append("END ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, messageBoxInfo.Title);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, messageBoxInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
            this.database.AddInParameter(sqlStringCommand, "Sernder", DbType.String, messageBoxInfo.Sernder);
            this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, messageBoxInfo.Accepter);
            this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Boolean, messageBoxInfo.IsRead);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool PostMemberMessageIsRead(long messageId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_MemberMessageBox set IsRead=1 where MessageId=@MessageId");
            this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Favorite SET Tags = @Tags, Remark = @Remark WHERE FavoriteId = @FavoriteId");
            this.database.AddInParameter(sqlStringCommand, "Tags", DbType.String, tags);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}


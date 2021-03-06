namespace Hidistro.ControlPanel.Data
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;

    public class CommentData : CommentsProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddAffiche(AfficheInfo affiche)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Affiche(Title, Content, AddedDate) VALUES (@Title, @Content, @AddedDate)");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, affiche.Title);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, affiche.Content);
            this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, affiche.AddedDate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddArticle(ArticleInfo article)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Articles(CategoryId, Title, Meta_Description, Meta_Keywords, IconUrl, Description, Content, AddedDate,IsRelease) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords,  @IconUrl, @Description, @Content, @AddedDate,@IsRelease)");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, article.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, article.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, article.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, article.IconUrl);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, article.Description);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, article.AddedDate);
            this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, article.IsRelease);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddHelp(HelpInfo help)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Helps(CategoryId, Title, Meta_Description, Meta_Keywords, Description, Content, AddedDate, IsShowFooter) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords, @Description, @Content, @AddedDate, @IsShowFooter)");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, help.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, help.Title);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, help.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, help.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, help.Description);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, help.Content);
            this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, help.AddedDate);
            this.database.AddInParameter(sqlStringCommand, "IsShowFooter", DbType.Boolean, help.IsShowFooter);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddMessage(MessageBoxInfo messageBoxInfo, UserRole toRole)
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
            builder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", (toRole == UserRole.Distributor) ? "Hishop_DistributorMessageBox" : "Hishop_MemberMessageBox");
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

        public override bool AddReleatesProdcutByArticId(int articId, int prodcutId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_RelatedArticsProducts(ArticleId, RelatedProductId) VALUES (@ArticleId, @RelatedProductId)");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, prodcutId);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
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
                builder.Append(" AND ReplyUserId IS NULL");
            }
            else if (query.Type == ConsultationReplyType.Replyed)
            {
                builder.Append(" AND ReplyUserId IS NOT NULL");
            }
            builder.Append(" ORDER BY replydate DESC");
            return builder.ToString();
        }

        private static string BuildLeaveCommentQuery(LeaveCommentQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT l.LeaveId FROM Hishop_LeaveComments l where 0=0");
            if (query.MessageStatus == MessageStatus.Replied)
            {
                builder.Append(" and (select Count(ReplyId) from Hishop_LeaveCommentReplys where LeaveId=l.LeaveId) >0 ");
            }
            if (query.MessageStatus == MessageStatus.NoReply)
            {
                builder.Append(" and (select Count(ReplyId) from Hishop_LeaveCommentReplys where LeaveId=l.LeaveId) <=0 ");
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            else
            {
                builder.Append(" ORDER BY LastDate desc");
            }
            return builder.ToString();
        }

        private static string BuildProductConsultationQuery(ProductConsultationAndReplyQuery consultationQuery)
        {
            HiContext current = HiContext.Current;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT c.ConsultationId FROM Hishop_Products p inner join Hishop_ProductConsultations c on p.productId=c.ProductId WHERE 0 = 0");
            if (consultationQuery.Type == ConsultationReplyType.NoReply)
            {
                builder.Append(" AND c.ReplyUserId IS NULL ");
            }
            else if (consultationQuery.Type == ConsultationReplyType.Replyed)
            {
                builder.Append(" AND c.ReplyUserId IS NOT NULL");
            }
            if (consultationQuery.ProductId > 0)
            {
                builder.AppendFormat(" AND p.ProductId = {0}", consultationQuery.ProductId);
                return builder.ToString();
            }
            if (!string.IsNullOrEmpty(consultationQuery.ProductCode))
            {
                builder.AppendFormat(" AND p.ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
            }
            if (!string.IsNullOrEmpty(consultationQuery.Keywords))
            {
                builder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
            }
            if (consultationQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (p.CategoryId = {0}", consultationQuery.CategoryId.Value);
                builder.AppendFormat(" OR p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", consultationQuery.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(consultationQuery.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(consultationQuery.SortBy), consultationQuery.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildReviewsQuery(ProductReviewQuery reviewQuery)
        {
            HiContext current = HiContext.Current;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT r.ReviewId FROM Hishop_Products p inner join Hishop_ProductReviews r on r.productId=p.ProductId WHERE 0 = 0");
            if (!string.IsNullOrEmpty(reviewQuery.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.ProductCode));
            }
            if (!string.IsNullOrEmpty(reviewQuery.Keywords))
            {
                builder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.Keywords));
            }
            if (reviewQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (p.CategoryId = {0}", reviewQuery.CategoryId.Value);
                builder.AppendFormat(" OR  p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", reviewQuery.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(reviewQuery.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(reviewQuery.SortBy), reviewQuery.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildUserReviewsAndReplysQuery(UserProductReviewAndReplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT ProductId FROM Hishop_ProductReviews ");
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_Products)", new object[0]);
            builder.Append(" GROUP BY ProductId");
            return builder.ToString();
        }

        public override bool CreateUpdateDeleteArticleCategory(ArticleCategoryInfo articleCategory, DataProviderAction action)
        {
            if (null == articleCategory)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ArticleCategory_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, articleCategory.CategoryId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "Name", DbType.String, articleCategory.Name);
                this.database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, articleCategory.IconUrl);
                this.database.AddInParameter(storedProcCommand, "Description", DbType.String, articleCategory.Description);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool CreateUpdateDeleteHelpCategory(HelpCategoryInfo helpCategory, DataProviderAction action)
        {
            if (null == helpCategory)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_HelpCategory_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, helpCategory.CategoryId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "Name", DbType.String, helpCategory.Name);
                this.database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, helpCategory.IconUrl);
                this.database.AddInParameter(storedProcCommand, "IndexChar", DbType.String, helpCategory.IndexChar);
                this.database.AddInParameter(storedProcCommand, "Description", DbType.String, helpCategory.Description);
                this.database.AddInParameter(storedProcCommand, "IsShowFooter", DbType.Boolean, helpCategory.IsShowFooter);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool DeleteAffiche(int afficheId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Affiche WHERE AfficheId = @AfficheId");
            this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteAffiches(List<int> afficheIds)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Affiche WHERE AfficheId=@AfficheId");
            this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32);
            foreach (int num2 in afficheIds)
            {
                this.database.SetParameterValue(sqlStringCommand, "AfficheId", num2);
                this.database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override bool DeleteArticle(int articleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Articles WHERE ArticleId = @ArticleId");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteArticles(IList<int> articles)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Articles WHERE ArticleId = @ArticleId");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32);
            foreach (int num2 in articles)
            {
                this.database.SetParameterValue(sqlStringCommand, "ArticleId", num2);
                this.database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override bool DeleteHelp(int helpId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Helps WHERE HelpId = @HelpId");
            this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteHelpCategorys(List<int> categoryIds)
        {
            if (null == categoryIds)
            {
                return 0;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_HelpCategories WHERE CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32);
            StringBuilder builder = new StringBuilder();
            int num = 0;
            foreach (int num2 in categoryIds)
            {
                this.database.SetParameterValue(sqlStringCommand, "CategoryId", num2);
                this.database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override int DeleteHelps(IList<int> helps)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Helps WHERE HelpId=@HelpId");
            this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32);
            foreach (int num2 in helps)
            {
                this.database.SetParameterValue(sqlStringCommand, "HelpId", num2);
                this.database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override bool DeleteLeaveComment(long leaveId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_LeaveCommentReplys WHERE LeaveId=@LeaveId;DELETE FROM Hishop_LeaveComments WHERE LeaveId=@LeaveId");
            this.database.AddInParameter(sqlStringCommand, "leaveId", DbType.Int64, leaveId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteLeaveCommentReply(long leaveReplyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_LeaveCommentReplys WHERE replyId=@replyId;");
            this.database.AddInParameter(sqlStringCommand, "replyId", DbType.Int64, leaveReplyId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int DeleteLeaveComments(IList<long> leaveIds)
        {
            string str = string.Empty;
            foreach (long num in leaveIds)
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_LeaveCommentReplys WHERE LeaveId in ({0});DELETE FROM Hishop_LeaveComments WHERE LeaveId in ({1}) ", str, str));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteManagerMessages(IList<long> messageList)
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("delete from Hishop_ManagerMessageBox where MessageId in ({0}) ", str));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteProductConsultation(int consultationId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductConsultations WHERE consultationId = @consultationId");
            this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int64, consultationId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteProductReview(long reviewId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductReviews WHERE ReviewId = @ReviewId");
            this.database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int64, reviewId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteReview(IList<int> reviews)
        {
            string str = string.Empty;
            foreach (long num in reviews)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = num.ToString();
                }
                else
                {
                    str = str + "," + num.ToString();
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_ProductReviews WHERE ReviewId in ({0})", str));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche ORDER BY AddedDate DESC");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ArticleCategories WHERE CategoryId = @CategoryId ORDER BY [DisplaySequence]");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    return DataMapper.PopulateArticleCategory(reader);
                }
                return null;
            }
        }

        public override DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(articleQuery.Keywords));
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

        public override DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductConsultation_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, consultationQuery.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, consultationQuery.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, consultationQuery.IsCount);
            if (consultationQuery.CategoryId.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, consultationQuery.CategoryId.Value);
            }
            this.database.AddInParameter(storedProcCommand, "SqlPopulate", DbType.String, BuildProductConsultationQuery(consultationQuery));
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (consultationQuery.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public override IList<Distributor> GetDistributorsByRank(int? gradeId)
        {
            DbCommand sqlStringCommand;
            IList<Distributor> list = new List<Distributor>();
            if (gradeId > 0)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors WHERE GradeId=@GradeId");
                this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors");
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    Distributor item = new Distributor();
                    item.UserId = (int) reader["UserId"];
                    item.Email = reader["Email"].ToString();
                    item.Username = reader["UserName"].ToString();
                    list.Add(item);
                }
            }
            return list;
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
            builder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(helpQuery.Keywords));
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

        public override LeaveCommentInfo GetLeaveComment(long leaveId)
        {
            LeaveCommentInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LeaveComments WHERE LeaveId=@LeaveId;");
            this.database.AddInParameter(sqlStringCommand, "LeaveId", DbType.Int64, leaveId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateLeaveComment(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_LeaveComments_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildLeaveCommentQuery(query));
            this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            DataSet set = this.database.ExecuteDataSet(storedProcCommand);
            set.Relations.Add("LeaveCommentReplays", set.Tables[0].Columns["LeaveId"], set.Tables[1].Columns["LeaveId"], false);
            result.Data = set;
            result.TotalRecords = (int) this.database.GetParameterValue(storedProcCommand, "Total");
            return result;
        }

        public override IList<ArticleCategoryInfo> GetMainArticleCategories()
        {
            IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From Hishop_ArticleCategories ORDER BY [DisplaySequence]");
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

        public override MessageBoxInfo GetManagerMessage(long messageId)
        {
            MessageBoxInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_ManagerMessageBox WHERE MessageId=@MessageId;");
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

        public override DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query, UserRole role)
        {
            string filter = string.Format("Accepter='{0}' AND Sernder IN (SELECT UserName FROM aspnet_Users WHERE UserRole = {1})", query.Accepter, (int) role);
            if (query.MessageStatus == MessageStatus.Replied)
            {
                filter = filter + " AND IsRead = 1";
            }
            if (query.MessageStatus == MessageStatus.NoReply)
            {
                filter = filter + " AND IsRead = 0";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_ManagerMessageBox", "MessageId", filter, "*");
        }

        public override DbQueryResult GetManagerSendedMessages(MessageBoxQuery query, UserRole role)
        {
            string filter = string.Format("Sernder='{0}' AND Accepter IN (SELECT UserName FROM aspnet_Users WHERE UserRole = {1})", query.Sernder, (int) role);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_ManagerMessageBox", "MessageId", filter, "*");
        }

        public override IList<Member> GetMembersByRank(int? gradeId)
        {
            DbCommand sqlStringCommand;
            IList<Member> list = new List<Member>();
            if (gradeId > 0)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members WHERE GradeId=@GradeId");
                this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members");
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    Member item = new Member(UserRole.Member);
                    item.UserId = (int) reader["UserId"];
                    item.Email = reader["Email"].ToString();
                    item.Username = reader["UserName"].ToString();
                    list.Add(item);
                }
            }
            return list;
        }

        public override int GetMemberUnReadMessageNum()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(COUNT(*),0) FROM Hishop_MemberMessageBox WHERE IsRead=0 and Accepter=@Accepter");
            this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, HiContext.Current.User.Username);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override ProductConsultationInfo GetProductConsultation(int consultationId)
        {
            ProductConsultationInfo info = null;
            string query = "SELECT * FROM Hishop_ProductConsultations WHERE ConsultationId=@ConsultationId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, consultationId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProductConsultation(reader);
                }
            }
            return info;
        }

        public override ProductReviewInfo GetProductReview(int reviewId)
        {
            ProductReviewInfo info = new ProductReviewInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductReviews WHERE ReviewId=@ReviewId");
            this.database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int32, reviewId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProductReview(reader);
                }
            }
            return info;
        }

        public override DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductReviews_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, reviewQuery.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, reviewQuery.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, reviewQuery.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildReviewsQuery(reviewQuery));
            this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            if (reviewQuery.CategoryId.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, reviewQuery.CategoryId.Value);
            }
            DataSet set = this.database.ExecuteDataSet(storedProcCommand);
            total = (int) this.database.GetParameterValue(storedProcCommand, "Total");
            return set;
        }

        public override DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SaleStatus = {0}", 1);
            builder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedArticsProducts WHERE ArticleId = {0})", articId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DataTable GetReplyLeaveComments(long leaveId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LeaveCommentReplys WHERE LeaveId=@LeaveId");
            this.database.AddInParameter(sqlStringCommand, "LeaveId", DbType.Int64, leaveId);
            DataTable table = new DataTable();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override bool InsertMessage(MessageBoxInfo messageBoxInfo, UserRole toRole)
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
            builder.Append("INSERT INTO [Hishop_ManagerMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
            builder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
            builder.Append("SET @errorSun=@errorSun+@@ERROR  ");
            builder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", (toRole == UserRole.Distributor) ? "Hishop_DistributorMessageBox" : "Hishop_MemberMessageBox");
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

        public override bool PostManagerMessageIsRead(long messageId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_ManagerMessageBox set IsRead=1 where MessageId=@MessageId");
            this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool RemoveReleatesProductByArticId(int articId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool RemoveReleatesProductByArticId(int articId, int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId AND RelatedProductId = @RelatedProductId");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, productId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_LeaveCommentReplys(LeaveId,UserId,ReplyContent,ReplyDate) VALUES(@LeaveId,@UserId,@ReplyContent,@ReplyDate);SELECT @@IDENTITY ");
            this.database.AddInParameter(sqlStringCommand, "leaveId", DbType.Int64, leaveReply.LeaveId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, leaveReply.UserId);
            this.database.AddInParameter(sqlStringCommand, "ReplyContent", DbType.String, leaveReply.ReplyContent);
            this.database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.String, DataHelper.GetSafeDateTimeFormat(leaveReply.ReplyDate));
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductConsultations SET ReplyText = @ReplyText, ReplyDate = @ReplyDate, ReplyUserId = @ReplyUserId WHERE ConsultationId = @ConsultationId");
            this.database.AddInParameter(sqlStringCommand, "ReplyText", DbType.String, productConsultation.ReplyText);
            this.database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.DateTime, productConsultation.ReplyDate);
            this.database.AddInParameter(sqlStringCommand, "ReplyUserId", DbType.Int32, productConsultation.ReplyUserId);
            this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, productConsultation.ConsultationId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_ArticleCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_HelpCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public override bool UpdateAffiche(AfficheInfo affiche)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Affiche SET Title = @Title, AddedDate = @AddedDate, Content = @Content WHERE AfficheId = @AfficheId");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, affiche.Title);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, affiche.Content);
            this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, affiche.AddedDate);
            this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, affiche.AfficheId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateArticle(ArticleInfo article)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Articles SET CategoryId = @CategoryId,AddedDate = @AddedDate,Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords, IconUrl=@IconUrl,Description = @Description,Content = @Content,IsRelease=@IsRelease WHERE ArticleId = @ArticleId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, article.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, article.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, article.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, article.IconUrl);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, article.Description);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, article.IsRelease);
            this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, article.AddedDate);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateHelp(HelpInfo help)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Helps SET CategoryId = @CategoryId, AddedDate = @AddedDate, Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords,  Description = @Description, Content = @Content, IsShowFooter = @IsShowFooter WHERE HelpId = @HelpId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, help.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, help.Title);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, help.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, help.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, help.Description);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, help.Content);
            this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, help.AddedDate);
            this.database.AddInParameter(sqlStringCommand, "IsShowFooter", DbType.Boolean, help.IsShowFooter);
            this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, help.HelpId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateRelease(int articId, bool release)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Articles set IsRelease=@IsRelease  where ArticleId = @ArticleId");
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, release);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }
    }
}


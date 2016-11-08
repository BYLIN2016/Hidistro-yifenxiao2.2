namespace Hidistro.Membership.Data
{
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class SettingsData : SiteSettingsProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override SiteSettings GetDistributorSettings(int userId)
        {
            SiteSettings settings = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Settings WHERE @UserId = UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    settings = SiteSettingsProvider.PopulateDistributorSettings(reader);
                }
                reader.Close();
            }
            return settings;
        }

        public override SiteSettings GetDistributorSettings(string siteUrl)
        {
            SiteSettings settings = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Settings WHERE LOWER(@SiteUrl) = LOWER(SiteUrl)");
            this.database.AddInParameter(sqlStringCommand, "SiteUrl", DbType.String, siteUrl);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    settings = SiteSettingsProvider.PopulateDistributorSettings(reader);
                }
                reader.Close();
            }
            return settings;
        }

        public override void SaveDistributorSettings(SiteSettings settings)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Settings SET SiteUrl=@SiteUrl, LogoUrl=@LogoUrl, SiteDescription=@SiteDescription, SiteName=@SiteName, Theme=@Theme, Footer=@Footer, SearchMetaKeywords=@SearchMetaKeywords, SearchMetaDescription=@SearchMetaDescription,DecimalLength=@DecimalLength, YourPriceName=@YourPriceName, Disabled=@Disabled, ReferralDeduct = @ReferralDeduct, DefaultProductImage=@DefaultProductImage, PointsRate=@PointsRate,OrderShowDays=@OrderShowDays, HtmlOnlineServiceCode=@HtmlOnlineServiceCode,EmailSender=@EmailSender, EmailSettings=@EmailSettings, SMSSender=@SMSSender, SMSSettings=@SMSSettings,RegisterAgreement=@RegisterAgreement,IsOpenEtao=@IsOpenEtao,EtaoID=@EtaoID, EtaoApplyTime=@EtaoApplyTime,EtaoStatus=@EtaoStatus WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, settings.UserId);
            this.database.AddInParameter(sqlStringCommand, "SiteUrl", DbType.String, settings.SiteUrl);
            this.database.AddInParameter(sqlStringCommand, "LogoUrl", DbType.String, settings.LogoUrl);
            this.database.AddInParameter(sqlStringCommand, "SiteDescription", DbType.String, settings.SiteDescription);
            this.database.AddInParameter(sqlStringCommand, "SiteName", DbType.String, settings.SiteName);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, settings.Theme);
            this.database.AddInParameter(sqlStringCommand, "Footer", DbType.String, settings.Footer);
            this.database.AddInParameter(sqlStringCommand, "SearchMetaKeywords", DbType.String, settings.SearchMetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "SearchMetaDescription", DbType.String, settings.SearchMetaDescription);
            this.database.AddInParameter(sqlStringCommand, "DecimalLength", DbType.Int32, settings.DecimalLength);
            this.database.AddInParameter(sqlStringCommand, "YourPriceName", DbType.String, settings.YourPriceName);
            this.database.AddInParameter(sqlStringCommand, "Disabled", DbType.Boolean, settings.Disabled);
            this.database.AddInParameter(sqlStringCommand, "ReferralDeduct", DbType.Int32, settings.ReferralDeduct);
            this.database.AddInParameter(sqlStringCommand, "DefaultProductImage", DbType.String, settings.DefaultProductImage);
            this.database.AddInParameter(sqlStringCommand, "PointsRate", DbType.Decimal, settings.PointsRate);
            this.database.AddInParameter(sqlStringCommand, "OrderShowDays", DbType.Int32, settings.OrderShowDays);
            this.database.AddInParameter(sqlStringCommand, "HtmlOnlineServiceCode", DbType.String, settings.HtmlOnlineServiceCode);
            this.database.AddInParameter(sqlStringCommand, "EmailSender", DbType.String, settings.EmailSender);
            this.database.AddInParameter(sqlStringCommand, "EmailSettings", DbType.String, settings.EmailSettings);
            this.database.AddInParameter(sqlStringCommand, "RegisterAgreement", DbType.String, settings.RegisterAgreement);
            this.database.AddInParameter(sqlStringCommand, "SMSSender", DbType.String, settings.SMSSender);
            this.database.AddInParameter(sqlStringCommand, "SMSSettings", DbType.String, settings.SMSSettings);
            this.database.AddInParameter(sqlStringCommand, "IsOpenEtao", DbType.Boolean, settings.IsOpenEtao);
            this.database.AddInParameter(sqlStringCommand, "EtaoID", DbType.String, settings.EtaoID);
            this.database.AddInParameter(sqlStringCommand, "EtaoApplyTime", DbType.DateTime, settings.EtaoApplyTime);
            this.database.AddInParameter(sqlStringCommand, "EtaoStatus", DbType.Int32, settings.EtaoStatus);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}


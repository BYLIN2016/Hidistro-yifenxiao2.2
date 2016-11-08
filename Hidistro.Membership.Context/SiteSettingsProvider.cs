namespace Hidistro.Membership.Context
{
    using System;
    using System.Data;
    using Hidistro.Core;

    public abstract class SiteSettingsProvider
    {
        private static readonly SiteSettingsProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.SettingsData,Hidistro.Membership.Data") as SiteSettingsProvider);

        protected SiteSettingsProvider()
        {
        }

        public abstract SiteSettings GetDistributorSettings(int userId);
        public abstract SiteSettings GetDistributorSettings(string siteUrl);
        public static SiteSettingsProvider Instance()
        {
            return _defaultInstance;
        }

        public static SiteSettings PopulateDistributorSettings(IDataReader reader)
        {
            SiteSettings settings = new SiteSettings(reader["SiteUrl"].ToString().ToLower(), new int?((int) reader["UserId"]));
            if (reader["LogoUrl"] != DBNull.Value)
            {
                settings.LogoUrl = reader["LogoUrl"].ToString();
            }
            if (reader["RequestDate"] != DBNull.Value)
            {
                settings.RequestDate = new DateTime?((DateTime) reader["RequestDate"]);
            }
            if (reader["CreateDate"] != DBNull.Value)
            {
                settings.CreateDate = new DateTime?((DateTime) reader["CreateDate"]);
            }
            if (reader["SiteDescription"] != DBNull.Value)
            {
                settings.SiteDescription = reader["SiteDescription"].ToString();
            }
            if (reader["SiteName"] != DBNull.Value)
            {
                settings.SiteName = reader["SiteName"].ToString();
            }
            if (reader["Theme"] != DBNull.Value)
            {
                settings.Theme = reader["Theme"].ToString();
            }
            if (reader["Footer"] != DBNull.Value)
            {
                settings.Footer = reader["Footer"].ToString();
            }
            if (reader["SearchMetaKeywords"] != DBNull.Value)
            {
                settings.SearchMetaKeywords = reader["SearchMetaKeywords"].ToString();
            }
            if (reader["SearchMetaDescription"] != DBNull.Value)
            {
                settings.SearchMetaDescription = reader["SearchMetaDescription"].ToString();
            }
            if (reader["DecimalLength"] != DBNull.Value)
            {
                settings.DecimalLength = (int) reader["DecimalLength"];
            }
            if (reader["YourPriceName"] != DBNull.Value)
            {
                settings.YourPriceName = reader["YourPriceName"].ToString();
            }
            if (reader["Disabled"] != DBNull.Value)
            {
                settings.Disabled = (bool) reader["Disabled"];
            }
            if (reader["ReferralDeduct"] != DBNull.Value)
            {
                settings.ReferralDeduct = (int) reader["ReferralDeduct"];
            }
            if (reader["DefaultProductImage"] != DBNull.Value)
            {
                settings.DefaultProductImage = reader["DefaultProductImage"].ToString();
            }
            if (reader["PointsRate"] != DBNull.Value)
            {
                settings.PointsRate = (decimal) reader["PointsRate"];
            }
            if (reader["OrderShowDays"] != DBNull.Value)
            {
                settings.OrderShowDays = (int) reader["OrderShowDays"];
            }
            if (reader["HtmlOnlineServiceCode"] != DBNull.Value)
            {
                settings.HtmlOnlineServiceCode = reader["HtmlOnlineServiceCode"].ToString();
            }
            if (reader["EmailSender"] != DBNull.Value)
            {
                settings.EmailSender = reader["EmailSender"].ToString();
            }
            if (reader["EmailSettings"] != DBNull.Value)
            {
                settings.EmailSettings = reader["EmailSettings"].ToString();
            }
            if (reader["SMSSender"] != DBNull.Value)
            {
                settings.SMSSender = reader["SMSSender"].ToString();
            }
            if (reader["SMSSettings"] != DBNull.Value)
            {
                settings.SMSSettings = reader["SMSSettings"].ToString();
            }
            if (reader["IsOpenEtao"] != DBNull.Value)
            {
                settings.IsOpenEtao = Convert.ToBoolean(reader["IsOpenEtao"]);
            }
            if (reader["EtaoID"] != DBNull.Value)
            {
                settings.EtaoID = Convert.ToString(reader["EtaoID"]);
            }
            if (reader["EtaoApplyTime"] != DBNull.Value)
            {
                settings.EtaoApplyTime = new DateTime?(Convert.ToDateTime(reader["EtaoApplyTime"]));
            }
            if (reader["EtaoStatus"] != DBNull.Value)
            {
                settings.EtaoStatus = Convert.ToInt32(reader["EtaoStatus"]);
            }
            if (reader["RegisterAgreement"] != DBNull.Value)
            {
                settings.RegisterAgreement = Convert.ToString(reader["RegisterAgreement"]);
            }
            return settings;
        }

        public abstract void SaveDistributorSettings(SiteSettings settings);
    }
}


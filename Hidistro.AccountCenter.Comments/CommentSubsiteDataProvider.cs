namespace Hidistro.AccountCenter.Comments
{
    using System;
    using Hidistro.Core;

    public abstract class CommentSubsiteDataProvider : CommentDataProvider
    {
        private static readonly CommentSubsiteDataProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.CommentData,Hidistro.AccountCenter.DistributionData") as CommentSubsiteDataProvider);

        protected CommentSubsiteDataProvider()
        {
        }

        public static CommentSubsiteDataProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}


namespace Hidistro.SaleSystem.Comments
{
    using System;
    using Hidistro.Core;

    public abstract class CommentSubsiteProvider : CommentProvider
    {
        private static readonly CommentSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.CommentData,Hidistro.SaleSystem.DistributionData") as CommentSubsiteProvider);

        protected CommentSubsiteProvider()
        {
        }

        public static CommentSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}


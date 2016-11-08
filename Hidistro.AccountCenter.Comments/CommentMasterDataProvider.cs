namespace Hidistro.AccountCenter.Comments
{
    using System;
    using Hidistro.Core;
    public abstract class CommentMasterDataProvider : CommentDataProvider
    {
        private static readonly CommentMasterDataProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.CommentData,Hidistro.AccountCenter.Data") as CommentMasterDataProvider);

        protected CommentMasterDataProvider()
        {
        }

        public static CommentMasterDataProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}


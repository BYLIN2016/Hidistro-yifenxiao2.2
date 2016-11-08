namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Hidistro.Core;

    public abstract class GalleryProvider
    {
        private static readonly GalleryProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.GalleryData,Hidistro.ControlPanel.Data") as GalleryProvider);

        protected GalleryProvider()
        {
        }

        public abstract bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize);
        public abstract bool AddPhotoCategory(string name);
        public abstract bool DeletePhoto(int photoId);
        public abstract bool DeletePhotoCategory(int categoryId);
        public abstract int GetDefaultPhotoCount();
        public abstract DataTable GetPhotoCategories();
        public abstract int GetPhotoCount();
        public abstract DbQueryResult GetPhotoList(string keyword, int? categoryId, Pagination page);
        public abstract string GetPhotoPath(int photoId);
        public static GalleryProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract int MovePhotoType(List<int> pList, int pTypeId);
        public abstract void RenamePhoto(int photoId, string newName);
        public abstract void ReplacePhoto(int photoId, int fileSize);
        public abstract void SwapSequence(int categoryId1, int categoryId2);
        public abstract int UpdatePhotoCategories(Dictionary<int, string> photoCategorys);
    }
}


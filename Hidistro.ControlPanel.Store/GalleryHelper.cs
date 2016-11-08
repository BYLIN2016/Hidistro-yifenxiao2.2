namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class GalleryHelper
    {
        public static bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize)
        {
            return GalleryProvider.Instance().AddPhote(categoryId, photoName, photoPath, fileSize);
        }

        public static bool AddPhotoCategory(string name)
        {
            return GalleryProvider.Instance().AddPhotoCategory(name);
        }

        public static bool DeletePhoto(int photoId)
        {
            return GalleryProvider.Instance().DeletePhoto(photoId);
        }

        public static bool DeletePhotoCategory(int categoryId)
        {
            return GalleryProvider.Instance().DeletePhotoCategory(categoryId);
        }

        public static int GetDefaultPhotoCount()
        {
            return GalleryProvider.Instance().GetDefaultPhotoCount();
        }

        public static DataTable GetPhotoCategories()
        {
            return GalleryProvider.Instance().GetPhotoCategories();
        }

        public static int GetPhotoCount()
        {
            return GalleryProvider.Instance().GetPhotoCount();
        }

        public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, PhotoListOrder order)
        {
            Pagination page = new Pagination();
            page.PageSize = 20;
            page.PageIndex = pageIndex;
            page.IsCount = true;
            switch (order)
            {
                case PhotoListOrder.UploadTimeDesc:
                    page.SortBy = "UploadTime";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.UploadTimeAsc:
                    page.SortBy = "UploadTime";
                    page.SortOrder = SortAction.Asc;
                    break;

                case PhotoListOrder.NameAsc:
                    page.SortBy = "PhotoName";
                    page.SortOrder = SortAction.Asc;
                    break;

                case PhotoListOrder.NameDesc:
                    page.SortBy = "PhotoName";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.UpdateTimeDesc:
                    page.SortBy = "LastUpdateTime";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.UpdateTimeAsc:
                    page.SortBy = "LastUpdateTime";
                    page.SortOrder = SortAction.Asc;
                    break;

                case PhotoListOrder.SizeDesc:
                    page.SortBy = "FileSize";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.SizeAsc:
                    page.SortBy = "FileSize";
                    page.SortOrder = SortAction.Asc;
                    break;
            }
            return GalleryProvider.Instance().GetPhotoList(keyword, categoryId, page);
        }

        public static string GetPhotoPath(int photoId)
        {
            return GalleryProvider.Instance().GetPhotoPath(photoId);
        }

        public static int MovePhotoType(List<int> pList, int pTypeId)
        {
            return GalleryProvider.Instance().MovePhotoType(pList, pTypeId);
        }

        public static void RenamePhoto(int photoId, string newName)
        {
            GalleryProvider.Instance().RenamePhoto(photoId, newName);
        }

        public static void ReplacePhoto(int photoId, int fileSize)
        {
            GalleryProvider.Instance().ReplacePhoto(photoId, fileSize);
        }

        public static void SwapSequence(int categoryId1, int categoryId2)
        {
            GalleryProvider.Instance().SwapSequence(categoryId1, categoryId2);
        }

        public static int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
        {
            return GalleryProvider.Instance().UpdatePhotoCategories(photoCategorys);
        }
    }
}


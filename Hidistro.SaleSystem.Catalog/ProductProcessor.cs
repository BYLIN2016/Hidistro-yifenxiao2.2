namespace Hidistro.SaleSystem.Catalog
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using System;

    public static class ProductProcessor
    {
        public static bool InsertProductConsultation(ProductConsultationInfo productConsultation)
        {
            Globals.EntityCoding(productConsultation, true);
            return ProductProvider.Instance().InsertProductConsultation(productConsultation);
        }

        public static bool InsertProductReview(ProductReviewInfo review)
        {
            Globals.EntityCoding(review, true);
            return ProductProvider.Instance().InsertProductReview(review);
        }

        public static bool ProductExists(int productId)
        {
            return ProductProvider.Instance().ProductExists(productId);
        }
    }
}


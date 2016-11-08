namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ImageTypeLabel : Label
    {
        protected override void Render(HtmlTextWriter writer)
        {
            string str = "<ul>";
            string str2 = string.Empty;
            DataTable photoCategories = GalleryHelper.GetPhotoCategories();
            int defaultPhotoCount = GalleryHelper.GetDefaultPhotoCount();
            string str3 = this.Page.Request.QueryString["ImageTypeId"];
            string str4 = string.Empty;
            if (!string.IsNullOrEmpty(str3))
            {
                str3 = this.Page.Request.QueryString["ImageTypeId"];
            }
            if (str3 == "0")
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<li><a href=\"", Globals.ApplicationPath, "/admin/store/ImageData.aspx?pageSize=20&ImageTypeId=0\" class='classLink'><s></s><strong>默认分类<span>(", defaultPhotoCount, ")</span></strong></a></li>" });
            }
            else
            {
                object obj3 = str;
                str = string.Concat(new object[] { obj3, "<li><a href=\"", Globals.ApplicationPath, "/admin/store/ImageData.aspx?pageSize=20&ImageTypeId=0\"><s></s><strong>默认分类<span>(", defaultPhotoCount, ")</span></strong></a></li>" });
            }
            foreach (DataRow row in photoCategories.Rows)
            {
                if (row["CategoryId"].ToString() == str3)
                {
                    str4 = "class='classLink'";
                }
                else
                {
                    str4 = "";
                }
                str2 = string.Format("<li><a href=\"" + Globals.ApplicationPath + "/admin/store/ImageData.aspx?pageSize=20&ImageTypeId={0}\" " + str4 + "><s></s>{1}<span>({2})</span></a></li>", row["CategoryId"], row["CategoryName"], row["PhotoCounts"].ToString());
                str = str + str2;
            }
            str = str + "</ul>";
            base.Text = str;
            base.Render(writer);
        }
    }
}


namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_PrimaryClass : AscxTemplatedWebControl
    {
        private Repeater rp_MainCategorys;

        protected override void AttachChildControls()
        {
            this.rp_MainCategorys = (Repeater) this.FindControl("rp_MainCategorys");
            this.rp_MainCategorys.ItemDataBound += new RepeaterItemEventHandler(this.rp_MainCategorys_ItemDataBound);
            this.rp_MainCategorys.ItemCreated += new RepeaterItemEventHandler(this.rp_MainCategorys_ItemCreated);
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", HiContext.Current.SiteSettings.Theme));
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + HiContext.Current.SiteSettings.UserId + "/{0}/config/HeaderMenu.xml", HiContext.Current.SiteSettings.Theme));
            }
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            int maxNum = int.Parse(document.SelectSingleNode("root").Attributes["CategoryNum"].Value);
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(0, maxNum);
            if ((maxSubCategories != null) && (maxSubCategories.Count > 0))
            {
                this.rp_MainCategorys.DataSource = maxSubCategories;
                this.rp_MainCategorys.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-Common_PrimaryClass.ascx";
            }
            base.OnInit(e);
        }

        private void rp_MainCategorys_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("rp_towCategorys");
            if (repeater != null)
            {
                repeater.ItemDataBound += new RepeaterItemEventHandler(this.rp_towCategorys_ItemDataBound);
            }
        }

        private void rp_MainCategorys_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("rp_towCategorys");
            if (repeater != null)
            {
                int categoryId = ((CategoryInfo) e.Item.DataItem).CategoryId;
                repeater.DataSource = CategoryBrowser.GetMaxSubCategories(categoryId, 0x3e8);
                repeater.DataBind();
            }
        }

        private void rp_towCategorys_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("rp_threeCategroys");
            if (repeater != null)
            {
                int categoryId = ((CategoryInfo) e.Item.DataItem).CategoryId;
                repeater.DataSource = CategoryBrowser.GetMaxSubCategories(categoryId, 0x3e8);
                repeater.DataBind();
            }
        }
    }
}


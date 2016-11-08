namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Member;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_Link_OpenId : WebControl
    {
        
        private string _ImageUrl;

        protected override void Render(HtmlTextWriter writer)
        {
            IList<OpenIdSettingsInfo> configedItems = MemberProcessor.GetConfigedItems();
            if (!string.IsNullOrEmpty(this.ImageUrl))
            {
                if (this.ImageUrl.StartsWith("~"))
                {
                    this.ImageUrl = base.ResolveUrl(this.ImageUrl);
                }
                else if (this.ImageUrl.StartsWith("/"))
                {
                    this.ImageUrl = HiContext.Current.GetSkinPath() + this.ImageUrl;
                }
                else
                {
                    this.ImageUrl = HiContext.Current.GetSkinPath() + "/" + this.ImageUrl;
                }
            }
            if ((configedItems != null) && (configedItems.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                foreach (OpenIdSettingsInfo info in configedItems)
                {
                    string imageUrl = this.ImageUrl;
                    imageUrl = Globals.ApplicationPath + "/plugins/openid/images/" + info.OpenIdType + ".gif";
                    if ((HiContext.Current.User.UserRole != UserRole.Member) && (HiContext.Current.User.UserRole != UserRole.Underling))
                    {
                        builder.AppendFormat("<a href=\"{0}/OpenId/RedirectLogin.aspx?ot={1}\">", Globals.ApplicationPath, info.OpenIdType);
                    }
                    else
                    {
                        builder.Append("<a href=\"#\">");
                    }
                    builder.AppendFormat("<img src=\"{0}\" alt=\"{1}\" /> ", imageUrl, info.Name);
                    builder.Append("</a>");
                }
                writer.Write(builder.ToString());
            }
        }

        public string ImageUrl
        {
            
            get
            {
                return _ImageUrl;
            }
            
            set
            {
                _ImageUrl = value;
            }
        }
    }
}


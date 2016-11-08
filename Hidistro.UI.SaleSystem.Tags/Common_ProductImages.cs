namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class Common_ProductImages : AscxTemplatedWebControl
    {
        
        private bool _Is410Image;
        
        private bool _Is60Image;
        private ProductInfo imageInfo;
        private HiImage imgBig;
        private HiImage imgSmall1;
        private HiImage imgSmall2;
        private HiImage imgSmall3;
        private HiImage imgSmall4;
        private HiImage imgSmall5;
        private HyperLink iptPicUrl1;
        private HyperLink iptPicUrl2;
        private HyperLink iptPicUrl3;
        private HyperLink iptPicUrl4;
        private HyperLink iptPicUrl5;
        public const string TagID = "common_ProductImages";
        private HyperLink zoom1;

        public Common_ProductImages()
        {
            base.ID = "common_ProductImages";
        }

        protected override void AttachChildControls()
        {
            this.imgBig = (HiImage) this.FindControl("imgBig");
            this.imgSmall1 = (HiImage) this.FindControl("imgSmall1");
            this.imgSmall2 = (HiImage) this.FindControl("imgSmall2");
            this.imgSmall3 = (HiImage) this.FindControl("imgSmall3");
            this.imgSmall4 = (HiImage) this.FindControl("imgSmall4");
            this.imgSmall5 = (HiImage) this.FindControl("imgSmall5");
            this.zoom1 = (HyperLink) this.FindControl("zoom1");
            this.iptPicUrl1 = (HyperLink) this.FindControl("iptPicUrl1");
            this.iptPicUrl2 = (HyperLink) this.FindControl("iptPicUrl2");
            this.iptPicUrl3 = (HyperLink) this.FindControl("iptPicUrl3");
            this.iptPicUrl4 = (HyperLink) this.FindControl("iptPicUrl4");
            this.iptPicUrl5 = (HyperLink) this.FindControl("iptPicUrl5");
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        private void BindData()
        {
            if (this.imageInfo != null)
            {
                string oldValue = "/Storage/master/product/images/";
                string newValue = Globals.ApplicationPath + "/Storage/master/product/thumbs310/310_";
                if (this.Is410Image)
                {
                    newValue = Globals.ApplicationPath + "/Storage/master/product/thumbs410/410_";
                }
                string str3 = Globals.ApplicationPath + "/Storage/master/product/thumbs40/40_";
                if (this.Is60Image)
                {
                    str3 = Globals.ApplicationPath + "/Storage/master/product/thumbs60/60_";
                }
                string str4 = "";
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl1))
                {
                    str4 = Globals.ApplicationPath + this.imageInfo.ImageUrl1;
                    if ((this.imageInfo.ImageUrl1.IndexOf("http://") >= 0) && !string.IsNullOrEmpty(Globals.ApplicationPath))
                    {
                        newValue = newValue.Replace(Globals.ApplicationPath, "");
                        str3 = str3.Replace(Globals.ApplicationPath, "");
                    }
                    if (this.imageInfo.ImageUrl1.IndexOf("http://") >= 0)
                    {
                        str4 = this.imageInfo.ImageUrl1;
                    }
                    this.imgBig.ImageUrl = this.imageInfo.ImageUrl1.Replace(oldValue, newValue);
                    this.imgSmall1.ImageUrl = this.imageInfo.ImageUrl1.Replace(oldValue, str3);
                    this.zoom1.NavigateUrl = this.iptPicUrl1.NavigateUrl = str4;
                    this.zoom1.Attributes["title"] = this.imageInfo.ProductName;
                    this.iptPicUrl1.Attributes["title"] = this.imageInfo.ProductName;
                    this.iptPicUrl1.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + this.imageInfo.ImageUrl1.Replace(oldValue, newValue) + "'";
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl2))
                {
                    str4 = Globals.ApplicationPath + this.imageInfo.ImageUrl2;
                    if ((this.imageInfo.ImageUrl2.IndexOf("http://") >= 0) && !string.IsNullOrEmpty(Globals.ApplicationPath))
                    {
                        newValue = newValue.Replace(Globals.ApplicationPath, "");
                        str3 = str3.Replace(Globals.ApplicationPath, "");
                        str4 = this.imageInfo.ImageUrl2;
                    }
                    if (this.imageInfo.ImageUrl2.IndexOf("http://") >= 0)
                    {
                        str4 = this.imageInfo.ImageUrl2;
                    }
                    this.iptPicUrl2.NavigateUrl = str4;
                    this.iptPicUrl2.Attributes["title"] = this.imageInfo.ProductName;
                    this.iptPicUrl2.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + this.imageInfo.ImageUrl2.Replace(oldValue, newValue) + "'";
                    this.imgSmall2.ImageUrl = this.imageInfo.ImageUrl2.Replace(oldValue, str3);
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl3))
                {
                    str4 = Globals.ApplicationPath + this.imageInfo.ImageUrl3;
                    if ((this.imageInfo.ImageUrl3.IndexOf("http://") >= 0) && !string.IsNullOrEmpty(Globals.ApplicationPath))
                    {
                        newValue = newValue.Replace(Globals.ApplicationPath, "");
                        str3 = str3.Replace(Globals.ApplicationPath, "");
                        str4 = this.imageInfo.ImageUrl3;
                    }
                    if (this.imageInfo.ImageUrl3.IndexOf("http://") >= 0)
                    {
                        str4 = this.imageInfo.ImageUrl3;
                    }
                    this.iptPicUrl3.NavigateUrl = str4;
                    this.iptPicUrl3.Attributes["title"] = this.imageInfo.ProductName;
                    this.iptPicUrl3.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + this.imageInfo.ImageUrl3.Replace(oldValue, newValue) + "'";
                    this.imgSmall3.ImageUrl = this.imageInfo.ImageUrl3.Replace(oldValue, str3);
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl4))
                {
                    str4 = Globals.ApplicationPath + this.imageInfo.ImageUrl4;
                    if ((this.imageInfo.ImageUrl4.IndexOf("http://") >= 0) && !string.IsNullOrEmpty(Globals.ApplicationPath))
                    {
                        newValue = newValue.Replace(Globals.ApplicationPath, "");
                        str3 = str3.Replace(Globals.ApplicationPath, "");
                        str4 = this.imageInfo.ImageUrl4;
                    }
                    if (this.imageInfo.ImageUrl4.IndexOf("http://") >= 0)
                    {
                        str4 = this.imageInfo.ImageUrl4;
                    }
                    this.iptPicUrl4.NavigateUrl = str4;
                    this.iptPicUrl4.Attributes["title"] = this.imageInfo.ProductName;
                    this.iptPicUrl4.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + this.imageInfo.ImageUrl4.Replace(oldValue, newValue) + "'";
                    this.imgSmall4.ImageUrl = this.imageInfo.ImageUrl4.Replace(oldValue, str3);
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl5))
                {
                    str4 = Globals.ApplicationPath + this.imageInfo.ImageUrl1 + this.imageInfo.ImageUrl5;
                    if ((this.imageInfo.ImageUrl5.IndexOf("http://") >= 0) && !string.IsNullOrEmpty(Globals.ApplicationPath))
                    {
                        newValue = newValue.Replace(Globals.ApplicationPath, "");
                        str3 = str3.Replace(Globals.ApplicationPath, "");
                        str4 = this.imageInfo.ImageUrl5;
                    }
                    if (this.imageInfo.ImageUrl5.IndexOf("http://") >= 0)
                    {
                        str4 = this.imageInfo.ImageUrl5;
                    }
                    this.iptPicUrl5.NavigateUrl = str4;
                    this.iptPicUrl5.Attributes["title"] = this.imageInfo.ProductName;
                    this.iptPicUrl5.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + this.imageInfo.ImageUrl5.Replace(oldValue, newValue) + "'";
                    this.imgSmall5.ImageUrl = this.imageInfo.ImageUrl5.Replace(oldValue, str3);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ViewProduct/Skin-Common_ProductImages.ascx";
            }
            base.OnInit(e);
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public ProductInfo ImageInfo
        {
            get
            {
                return this.imageInfo;
            }
            set
            {
                this.imageInfo = value;
            }
        }

        public bool Is410Image
        {
            
            get
            {
                return _Is410Image;
            }
            
            set
            {
                _Is410Image = value;
            }
        }

        public bool Is60Image
        {
            
            get
            {
                return _Is60Image;
            }
            
            set
            {
                _Is60Image = value;
            }
        }
    }
}


namespace Hidistro.UI.Web.Handler
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.SaleSystem.Tags;
    using Hishop.Components.Validation;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class DesigAdvert : AdminPage, IHttpHandler
    {
        private string elementId = "";
        private string message = "";
        private string modeId = "";
        private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";

        private bool CheckAdvertCustom(JavaScriptObject advertcustomobject)
        {
            if (string.IsNullOrEmpty(advertcustomobject["Html"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"自定义内容不允许为空！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(advertcustomobject["Id"].ToString()) && (advertcustomobject["Id"].ToString().Split(new char[] { '_' }).Length == 2))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
            return false;
        }

        private bool CheckAdvertImage(JavaScriptObject advertimageobject)
        {
            if (string.IsNullOrEmpty(advertimageobject["Image"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择广告图片！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(advertimageobject["Id"].ToString()) && (advertimageobject["Id"].ToString().Split(new char[] { '_' }).Length == 2))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
            return false;
        }

        private bool CheckAdvertSlide(JavaScriptObject avdvertobject)
        {
            if ((((string.IsNullOrEmpty(avdvertobject["Image1"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image2"].ToString())) && (string.IsNullOrEmpty(avdvertobject["Image3"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image4"].ToString()))) && ((string.IsNullOrEmpty(avdvertobject["Image5"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image6"].ToString())) && (string.IsNullOrEmpty(avdvertobject["Image7"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image8"].ToString())))) && (string.IsNullOrEmpty(avdvertobject["Image9"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image10"].ToString())))
            {
                this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
                return false;
            }
            int num = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (!string.IsNullOrEmpty(avdvertobject["Image" + i].ToString()))
                {
                    num++;
                }
            }
            if (num < 3)
            {
                this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(avdvertobject["Id"].ToString()) && (avdvertobject["Id"].ToString().Split(new char[] { '_' }).Length == 2))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
            return false;
        }

        private bool CheckLogo(JavaScriptObject logoobject)
        {
            if (string.IsNullOrEmpty(logoobject["LogoUrl"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请上传Logo图片！\"");
                return false;
            }
            return true;
        }

        public Dictionary<string, string> GetXmlNodeString(JavaScriptObject scriptobject)
        {
            return scriptobject.ToDictionary<KeyValuePair<string, object>, string, string>(delegate (KeyValuePair<string, object> s) {
                return s.Key;
            }, delegate (KeyValuePair<string, object> s) {
                return Globals.HtmlEncode(s.Value.ToString());
            });
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string str2;
                string str3;
                string str4;
                this.modeId = context.Request.Form["ModelId"];
                string modeId = this.modeId;
                if (modeId != null)
                {
                    if (!(modeId == "editeadvertslide"))
                    {
                        if (modeId == "editeadvertimage")
                        {
                            goto Label_00FD;
                        }
                        if (modeId == "editelogo")
                        {
                            goto Label_0196;
                        }
                        if (modeId == "editeadvertcustom")
                        {
                            goto Label_021D;
                        }
                    }
                    else
                    {
                        string str = context.Request.Form["Param"];
                        if (str != "")
                        {
                            JavaScriptObject avdvertobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str);
                            if (this.CheckAdvertSlide(avdvertobject) && this.UpdateAdvertSlide(avdvertobject))
                            {
                                Common_SlideAd ad = new Common_SlideAd();
                                ad.AdId = Convert.ToInt32(this.elementId);
                                var type = new {
                                    AdSlide = ad.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type));
                            }
                        }
                    }
                }
                goto Label_02CA;
            Label_00FD:
                str2 = context.Request.Form["Param"];
                if (str2 != "")
                {
                    JavaScriptObject advertimageobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str2);
                    if (this.CheckAdvertImage(advertimageobject) && this.UpdateAdvertImage(advertimageobject))
                    {
                        Common_ImageAd ad2 = new Common_ImageAd();
                        ad2.AdId = Convert.ToInt32(this.elementId);
                        var type2 = new {
                            AdImage = ad2.RendHtml()
                        };
                        this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type2));
                    }
                }
                goto Label_02CA;
            Label_0196:
                str3 = context.Request.Form["Param"];
                if (str3 != "")
                {
                    JavaScriptObject logoobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str3);
                    if (this.CheckLogo(logoobject) && this.UpdateLogo(logoobject))
                    {
                        Common_Logo logo = new Common_Logo();
                        var type3 = new {
                            LogoUrl = logo.RendHtml()
                        };
                        this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type3));
                    }
                }
                goto Label_02CA;
            Label_021D:
                str4 = context.Request.Form["Param"];
                if (str4 != "")
                {
                    JavaScriptObject advertcustomobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str4);
                    if (this.CheckAdvertCustom(advertcustomobject) && this.UpdateAdvertCustom(advertcustomobject))
                    {
                        Common_CustomAd ad3 = new Common_CustomAd();
                        ad3.AdId = Convert.ToInt32(this.elementId);
                        var type4 = new {
                            AdCustom = ad3.RendHtml()
                        };
                        this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type4));
                    }
                }
            }
            catch (Exception exception)
            {
                this.message = "{\"success\":false,\"Result\":\"未知错误:" + exception.Message + "\"}";
            }
        Label_02CA:
            context.Response.ContentType = "text/json";
            context.Response.Write(this.message);
        }

        private bool UpdateAdvertCustom(JavaScriptObject advertcustomobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"自定义编辑失败\"");
            this.elementId = advertcustomobject["Id"].ToString().Split(new char[] { '_' })[1];
            advertcustomobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertcustomobject);
            if (xmlNodeString.Keys.Contains<string>("Html"))
            {
                xmlNodeString["Html"] = Globals.HtmlDecode(xmlNodeString["Html"]);
            }
            return TagsHelper.UpdateAdNode(Convert.ToInt16(this.elementId), "custom", xmlNodeString);
        }

        private bool UpdateAdvertImage(JavaScriptObject advertimageobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改单张广告图片失败\"");
            this.elementId = advertimageobject["Id"].ToString().Split(new char[] { '_' })[1];
            advertimageobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertimageobject);
            return TagsHelper.UpdateAdNode(Convert.ToInt16(this.elementId), "image", xmlNodeString);
        }

        private bool UpdateAdvertSlide(JavaScriptObject avdvertobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改轮播广告失败\"");
            this.elementId = avdvertobject["Id"].ToString().Split(new char[] { '_' })[1];
            avdvertobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(avdvertobject);
            return TagsHelper.UpdateAdNode(Convert.ToInt16(this.elementId), "slide", xmlNodeString);
        }

        private bool UpdateLogo(JavaScriptObject advertimageobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改Logo图片失败\"");
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            siteSettings.LogoUrl = advertimageobject["LogoUrl"].ToString();
            SettingsManager.Save(siteSettings);
            return true;
        }

        private bool ValidationSettings(SiteSettings setting, ref string errors)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteSettings>(setting, new string[] { "ValMasterSettings" });
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    errors = errors + Formatter.FormatErrorMessage(result.Message);
                }
            }
            return results.IsValid;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}


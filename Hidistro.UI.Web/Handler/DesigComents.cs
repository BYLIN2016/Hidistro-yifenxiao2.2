namespace Hidistro.UI.Web.Handler
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.SaleSystem.Tags;
    using Hidistro.UI.Web;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using Hidistro.Entities.Commodities;

    public class DesigComents : IHttpHandler
    {
        private string elementId = "";
        private string message = "";
        private string modeId = "";
        private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";

        private bool CheckCommentArticle(JavaScriptObject articleobject)
        {
            if (string.IsNullOrEmpty(articleobject["Title"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请输入文字标题!\"");
                return false;
            }
            if ((!string.IsNullOrEmpty(articleobject["MaxNum"].ToString()) && (Convert.ToInt16(articleobject["MaxNum"].ToString()) > 0)) && (Convert.ToInt16(articleobject["MaxNum"].ToString()) <= 100))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
            return false;
        }

        private bool CheckCommentAttribute(JavaScriptObject attributeobject)
        {
            if (string.IsNullOrEmpty(attributeobject["CategoryId"].ToString()) || (Convert.ToInt16(attributeobject["CategoryId"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
                return false;
            }
            if (!string.IsNullOrEmpty(attributeobject["MaxNum"].ToString()) && (Convert.ToInt16(attributeobject["MaxNum"].ToString()) > 0))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
            return false;
        }

        private bool CheckCommentBrand(JavaScriptObject brandobject)
        {
            if (!string.IsNullOrEmpty(brandobject["CategoryId"].ToString()) && (Convert.ToInt16(brandobject["CategoryId"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
                return false;
            }
            if (string.IsNullOrEmpty(brandobject["IsShowLogo"].ToString()) || string.IsNullOrEmpty(brandobject["IsShowTitle"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"参数格式不正确!\"");
                return false;
            }
            if ((!string.IsNullOrEmpty(brandobject["MaxNum"].ToString()) && (Convert.ToInt16(brandobject["MaxNum"].ToString()) > 0)) && (Convert.ToInt16(brandobject["MaxNum"].ToString()) <= 100))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"显示数量必须为0~100的正整数！\"");
            return false;
        }

        private bool CheckCommentCategory(JavaScriptObject categoryobject)
        {
            if (string.IsNullOrEmpty(categoryobject["CategoryId"].ToString()) || (Convert.ToInt16(categoryobject["CategoryId"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
                return false;
            }
            if (!string.IsNullOrEmpty(categoryobject["MaxNum"].ToString()) && (Convert.ToInt16(categoryobject["MaxNum"].ToString()) > 0))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
            return false;
        }

        private bool CheckCommentKeyWord(JavaScriptObject keywordobject)
        {
            if (!string.IsNullOrEmpty(keywordobject["CategoryId"].ToString()) && (Convert.ToInt16(keywordobject["CategoryId"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
                return false;
            }
            if ((!string.IsNullOrEmpty(keywordobject["MaxNum"].ToString()) && (Convert.ToInt16(keywordobject["MaxNum"].ToString()) > 0)) && (Convert.ToInt16(keywordobject["MaxNum"].ToString()) <= 100))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"显示数量必须为1~100的正整数！\"");
            return false;
        }

        private bool CheckCommentMorelink(JavaScriptObject morelinkobject)
        {
            if (!string.IsNullOrEmpty(morelinkobject["CategoryId"].ToString()) && (Convert.ToInt16(morelinkobject["CategoryId"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
                return false;
            }
            if (string.IsNullOrEmpty(morelinkobject["Title"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请输入链接标题！\"");
                return false;
            }
            return true;
        }

        private bool CheckCommentTitle(JavaScriptObject titleobject)
        {
            if (!string.IsNullOrEmpty(titleobject["Title"].ToString()) && !string.IsNullOrEmpty(titleobject["ImageTitle"].ToString()))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"请输入标题或上传图片！\"");
            return false;
        }

        private DataTable ConvertListToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> list2 = new List<string>();
            if (propertyName != null)
            {
                list2.AddRange(propertyName);
            }
            DataTable table = new DataTable();
            if (list.Count > 0)
            {
                T local = list[0];
                PropertyInfo[] properties = local.GetType().GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    if (list2.Count == 0)
                    {
                        table.Columns.Add(info.Name, info.PropertyType);
                    }
                    else if (list2.Contains(info.Name))
                    {
                        table.Columns.Add(info.Name, info.PropertyType);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList list3 = new ArrayList();
                    foreach (PropertyInfo info2 in properties)
                    {
                        if (list2.Count == 0)
                        {
                            object obj2 = info2.GetValue(list[i], null);
                            list3.Add(obj2);
                        }
                        else if (list2.Contains(info2.Name))
                        {
                            object obj3 = info2.GetValue(list[i], null);
                            list3.Add(obj3);
                        }
                    }
                    object[] values = list3.ToArray();
                    table.LoadDataRow(values, true);
                }
            }
            return table;
        }

        private string GetCategorys()
        {
            DataTable table = null;
            string str = "";
            string[] propertyName = new string[] { "CategoryId", "Name", "Depth" };
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                table = this.ConvertListToDataTable<CategoryInfo>(SubsiteCatalogHelper.GetSequenceCategories(), propertyName);
            }
            else
            {
                table = this.ConvertListToDataTable<CategoryInfo>(CatalogHelper.GetSequenceCategories(), propertyName);
            }
            if (table != null)
            {
                str = JavaScriptConvert.SerializeObject(table, new JsonConverter[] { new ConvertTojson() });
            }
            return str;
        }

        private string GetMainArticleCategories()
        {
            return JavaScriptConvert.SerializeObject(CommentBrowser.GetArticleMainCategories());
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
                string mainArticleCategories = "";
                this.modeId = context.Request.Form["ModelId"];
                switch (this.modeId)
                {
                    case "commentarticleview":
                        mainArticleCategories = this.GetMainArticleCategories();
                        this.message = string.Format(this.resultformat, "true", mainArticleCategories);
                        goto Label_0554;

                    case "commentCategory":
                        mainArticleCategories = this.GetCategorys();
                        this.message = string.Format(this.resultformat, "true", mainArticleCategories);
                        goto Label_0554;

                    case "editecommentarticle":
                    {
                        string str2 = context.Request.Form["Param"];
                        if (!string.IsNullOrEmpty(str2))
                        {
                            JavaScriptObject articleobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str2);
                            if (this.CheckCommentArticle(articleobject) && this.UpdateCommentArticle(articleobject))
                            {
                                Common_SubjectArticle article = new Common_SubjectArticle();
                                article.CommentId = Convert.ToInt32(this.elementId);
                                var type = new {
                                    ComArticle = article.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type));
                            }
                        }
                        goto Label_0554;
                    }
                    case "editecommentcategory":
                    {
                        string str3 = context.Request.Form["Param"];
                        if (!string.IsNullOrEmpty(str3))
                        {
                            JavaScriptObject categoryobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str3);
                            if (this.CheckCommentCategory(categoryobject) && this.UpdateCommentCategory(categoryobject))
                            {
                                Common_SubjectCategory category = new Common_SubjectCategory();
                                category.CommentId = Convert.ToInt32(this.elementId);
                                var type2 = new {
                                    ComCategory = category.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type2));
                            }
                        }
                        goto Label_0554;
                    }
                    case "editecommentbrand":
                    {
                        string str4 = context.Request.Form["Param"];
                        if (!string.IsNullOrEmpty(str4))
                        {
                            JavaScriptObject brandobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str4);
                            if (this.CheckCommentBrand(brandobject) && this.UpdateCommentBrand(brandobject))
                            {
                                Common_SubjectBrand brand = new Common_SubjectBrand();
                                brand.CommentId = Convert.ToInt32(this.elementId);
                                var type3 = new {
                                    ComBrand = brand.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type3));
                            }
                        }
                        goto Label_0554;
                    }
                    case "editecommentkeyword":
                    {
                        string str5 = context.Request.Form["Param"];
                        if (!string.IsNullOrEmpty(str5))
                        {
                            JavaScriptObject keywordobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str5);
                            if (this.CheckCommentKeyWord(keywordobject) && this.UpdateCommentKeyWord(keywordobject))
                            {
                                Common_SubjectKeyword keyword = new Common_SubjectKeyword();
                                keyword.CommentId = Convert.ToInt32(this.elementId);
                                var type4 = new {
                                    ComCategory = keyword.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type4));
                            }
                        }
                        goto Label_0554;
                    }
                    case "editecommentattribute":
                    {
                        string str6 = context.Request.Form["Param"];
                        if (!string.IsNullOrEmpty(str6))
                        {
                            JavaScriptObject attributeobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str6);
                            if (this.CheckCommentAttribute(attributeobject) && this.UpdateCommentAttribute(attributeobject))
                            {
                                Common_SubjectAttribute attribute = new Common_SubjectAttribute();
                                attribute.CommentId = Convert.ToInt32(this.elementId);
                                var type5 = new {
                                    ComAttribute = attribute.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type5));
                            }
                        }
                        goto Label_0554;
                    }
                    case "editecommenttitle":
                    {
                        string str7 = context.Request.Form["Param"];
                        if (!string.IsNullOrEmpty(str7))
                        {
                            JavaScriptObject titleobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str7);
                            if (this.CheckCommentTitle(titleobject) && this.UpdateCommentTitle(titleobject))
                            {
                                Common_SubjectTitle title = new Common_SubjectTitle();
                                title.CommentId = Convert.ToInt32(this.elementId);
                                var type6 = new {
                                    ComTitle = title.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type6));
                            }
                        }
                        goto Label_0554;
                    }
                    case "editecommentmorelink":
                    {
                        string str8 = context.Request.Form["Param"];
                        if (!string.IsNullOrEmpty(str8))
                        {
                            JavaScriptObject morelinkobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str8);
                            if (this.CheckCommentMorelink(morelinkobject) && this.UpdateMorelink(morelinkobject))
                            {
                                Common_SubjectMoreLink link = new Common_SubjectMoreLink();
                                link.CommentId = Convert.ToInt32(this.elementId);
                                var type7 = new {
                                    ComMoreLink = link.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(type7));
                            }
                        }
                        goto Label_0554;
                    }
                }
            }
            catch (Exception exception)
            {
                this.message = "{\"success\":false,\"Result\":\"未知错误:" + exception.Message + "\"}";
            }
        Label_0554:
            context.Response.ContentType = "text/json";
            context.Response.Write(this.message);
        }

        private bool UpdateCommentArticle(JavaScriptObject articleobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改文章标签列表失败\"");
            this.elementId = articleobject["Id"].ToString().Split(new char[] { '_' })[1];
            articleobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(articleobject);
            return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "article", xmlNodeString);
        }

        private bool UpdateCommentAttribute(JavaScriptObject attributeobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改属性标签列表失败\"");
            this.elementId = attributeobject["Id"].ToString().Split(new char[] { '_' })[1];
            attributeobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
            return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "attribute", xmlNodeString);
        }

        private bool UpdateCommentBrand(JavaScriptObject attributeobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
            this.elementId = attributeobject["Id"].ToString().Split(new char[] { '_' })[1];
            attributeobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
            return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "brand", xmlNodeString);
        }

        private bool UpdateCommentCategory(JavaScriptObject categoryobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改分类标签列表失败\"");
            this.elementId = categoryobject["Id"].ToString().Split(new char[] { '_' })[1];
            categoryobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(categoryobject);
            return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "category", xmlNodeString);
        }

        private bool UpdateCommentKeyWord(JavaScriptObject keywordobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
            this.elementId = keywordobject["Id"].ToString().Split(new char[] { '_' })[1];
            keywordobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(keywordobject);
            return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "keyword", xmlNodeString);
        }

        private bool UpdateCommentTitle(JavaScriptObject titleobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
            this.elementId = titleobject["Id"].ToString().Split(new char[] { '_' })[1];
            titleobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(titleobject);
            return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "title", xmlNodeString);
        }

        private bool UpdateMorelink(JavaScriptObject morelinkobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
            this.elementId = morelinkobject["Id"].ToString().Split(new char[] { '_' })[1];
            morelinkobject["Id"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(morelinkobject);
            return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "morelink", xmlNodeString);
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


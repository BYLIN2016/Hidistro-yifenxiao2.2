namespace Hidistro.UI.Web.Handler
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
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

    public class DesigProduct : AdminPage, IHttpHandler
    {
        private string elementId = "";
        private string message = "";
        private string modeId = "";
        private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";

        private bool CheckProductFloor(JavaScriptObject floorobject)
        {
            if ((string.IsNullOrEmpty(floorobject["MaxNum"].ToString()) || (Convert.ToInt16(floorobject["MaxNum"].ToString()) <= 0)) || (Convert.ToInt16(floorobject["MaxNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(floorobject["SubCategoryNum"].ToString()) && ((Convert.ToInt16(floorobject["SubCategoryNum"].ToString()) < 0) || (Convert.ToInt16(floorobject["MaxNum"].ToString()) > 100)))
            {
                this.message = string.Format(this.resultformat, "false", "\"子类显示数量必须为0~100的正整数！\"");
                return false;
            }
            if (string.IsNullOrEmpty(floorobject["ImageSize"].ToString()) || (Convert.ToInt16(floorobject["ImageSize"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
                return false;
            }
            if (string.IsNullOrEmpty(floorobject["SubjectId"].ToString()) || (floorobject["SubjectId"].ToString().Split(new char[] { '_' }).Length != 2))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
                return false;
            }
            if (string.IsNullOrEmpty(floorobject["Title"].ToString()) && string.IsNullOrEmpty(floorobject["ImageTitle"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请上传标题图片或输入楼层标题\"");
                return false;
            }
            return true;
        }

        private bool CheckProductGroup(JavaScriptObject groupobject)
        {
            if (string.IsNullOrEmpty(groupobject["Title"].ToString()) && string.IsNullOrEmpty(groupobject["ImageTitle"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请上传标题图片或输入栏目标题\"");
                return false;
            }
            if ((string.IsNullOrEmpty(groupobject["MaxNum"].ToString()) || (Convert.ToInt16(groupobject["MaxNum"].ToString()) <= 0)) || (Convert.ToInt16(groupobject["MaxNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
                return false;
            }
            if ((string.IsNullOrEmpty(groupobject["SubCategoryNum"].ToString()) || (Convert.ToInt16(groupobject["SubCategoryNum"].ToString()) < 0)) || (Convert.ToInt16(groupobject["SubCategoryNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"子类商品数量必须为1~100！\"");
                return false;
            }
            if ((string.IsNullOrEmpty(groupobject["BrandNum"].ToString()) || (Convert.ToInt16(groupobject["BrandNum"].ToString()) < 0)) || (Convert.ToInt16(groupobject["BrandNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"品牌显示数量必须1~`100的正整数！\"");
                return false;
            }
            if ((string.IsNullOrEmpty(groupobject["HotKeywordNum"].ToString()) || (Convert.ToInt16(groupobject["HotKeywordNum"].ToString()) < 0)) || (Convert.ToInt16(groupobject["HotKeywordNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"热门关键字显示数量必须为1~100的正整数！\"");
                return false;
            }
            if (string.IsNullOrEmpty(groupobject["ImageSize"].ToString()) || (Convert.ToInt16(groupobject["ImageSize"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"商品图片规格不允许为空！\"");
                return false;
            }
            if ((string.IsNullOrEmpty(groupobject["SaleTopNum"].ToString()) || (Convert.ToInt16(groupobject["SaleTopNum"].ToString()) < 0)) || (Convert.ToInt16(groupobject["SaleTopNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"销售排行显示数量必须为1~100的正整数！\"");
                return false;
            }
            if ((string.IsNullOrEmpty(groupobject["ImageNum"].ToString()) || (Convert.ToInt16(groupobject["ImageNum"].ToString()) < 0)) || (Convert.ToInt16(groupobject["ImageNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"排行榜图片显示数量必须为1~100的正整数！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(groupobject["TopImageSize"].ToString()) && (Convert.ToInt16(groupobject["TopImageSize"].ToString()) > 0))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"排行榜图片图片规格不允许为空！\"");
            return false;
        }

        private bool CheckProductTab(JavaScriptObject tabobject)
        {
            if ((string.IsNullOrEmpty(tabobject["MaxNum"].ToString()) || (Convert.ToInt16(tabobject["MaxNum"].ToString()) <= 0)) || (Convert.ToInt16(tabobject["MaxNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
                return false;
            }
            if (string.IsNullOrEmpty(tabobject["ImageSize"].ToString()) || (Convert.ToInt16(tabobject["ImageSize"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(tabobject["SubjectId"].ToString()) && (tabobject["SubjectId"].ToString().Split(new char[] { '_' }).Length == 2))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
            return false;
        }

        private bool CheckProductTop(JavaScriptObject topobject)
        {
            if ((string.IsNullOrEmpty(topobject["MaxNum"].ToString()) || (Convert.ToInt16(topobject["MaxNum"].ToString()) <= 0)) || (Convert.ToInt16(topobject["MaxNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
                return false;
            }
            if ((string.IsNullOrEmpty(topobject["ImageNum"].ToString()) || (Convert.ToInt16(topobject["ImageNum"].ToString()) < 0)) || (Convert.ToInt16(topobject["ImageNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"图片显示数量必须为1~100的正整数！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(topobject["ImageSize"].ToString()) && (Convert.ToInt16(topobject["ImageSize"].ToString()) > 0))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"商品图片规格不允许为空！\"");
            return false;
        }

        private bool CheckSimpleProduct(JavaScriptObject simpleobject)
        {
            if ((string.IsNullOrEmpty(simpleobject["MaxNum"].ToString()) || (Convert.ToInt16(simpleobject["MaxNum"].ToString()) <= 0)) || (Convert.ToInt16(simpleobject["MaxNum"].ToString()) > 100))
            {
                this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
                return false;
            }
            if (string.IsNullOrEmpty(simpleobject["ImageSize"].ToString()) || (Convert.ToInt16(simpleobject["ImageSize"].ToString()) <= 0))
            {
                this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
                return false;
            }
            if (!string.IsNullOrEmpty(simpleobject["SubjectId"].ToString()) && (simpleobject["SubjectId"].ToString().Split(new char[] { '_' }).Length == 2))
            {
                return true;
            }
            this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
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

        private string GetProductBrand()
        {
            DataTable table = null;
            string str = "";
            int index = 2;
            table = ControlProvider.Instance().GetBrandCategories().Copy();
            if (table != null)
            {
                do
                {
                    table.Columns.RemoveAt(index);
                }
                while (table.Columns.Count > 2);
            }
            if (table != null)
            {
                str = JavaScriptConvert.SerializeObject(table, new JsonConverter[] { new ConvertTojson() });
            }
            return str;
        }

        private string GetProductTags()
        {
            DataTable tags = null;
            string str = "";
            tags = CatalogHelper.GetTags();
            if (tags != null)
            {
                str = JavaScriptConvert.SerializeObject(tags, new JsonConverter[] { new ConvertTojson() });
            }
            return str;
        }

        private IList<ProductTypeInfo> GetProductTypeList()
        {
            return ControlProvider.Instance().GetProductTypes();
        }

        private string GetSimpleProductView()
        {
            string productBrand = "";
            string categorys = "";
            string productTags = "";
            IList<ProductTypeInfo> productTypeList = null;
            string str4 = "";
            IList<AttributeInfo> attributes = null;
            string str5 = "";
            productBrand = this.GetProductBrand();
            categorys = this.GetCategorys();
            productTags = this.GetProductTags();
            productTypeList = this.GetProductTypeList();
            attributes = ProductTypeHelper.GetAttributes(AttributeUseageMode.MultiView);
            if (productTypeList != null)
            {
                str4 = JavaScriptConvert.SerializeObject(productTypeList);
            }
            if (attributes.Count > 0)
            {
                str5 = JavaScriptConvert.SerializeObject(attributes);
            }
            return ("{\"Categorys\":" + categorys + ",\"Brands\":" + productBrand + ",\"Tags\":" + productTags + ",\"ProductTypes\":" + str4 + ",\"Attributes\":" + str5 + "}");
        }

        public Dictionary<string, string> GetXmlNodeString(JavaScriptObject scriptobject)
        {
            return scriptobject.ToDictionary<KeyValuePair<string, object>, string, string>(delegate (KeyValuePair<string, object> s) {
                return s.Key;
            }, delegate (KeyValuePair<string, object> s) {
                return Globals.HtmlEncode(DataHelper.CleanSearchString(s.Value.ToString()));
            });
        }

        public string GetXmlPath(string xmlname)
        {
            if (xmlname != "")
            {
                return Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/config/" + xmlname + ".xml");
            }
            return null;
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                this.modeId = context.Request.Form["ModelId"];
                string categorys = "";
                string productBrand = "";
                string productTags = "";
                switch (this.modeId)
                {
                    case "simpleview":
                        this.message = string.Format(this.resultformat, "true", this.GetSimpleProductView());
                        goto Label_059E;

                    case "editesimple":
                    {
                        string str4 = context.Request.Form["Param"];
                        if (str4 != "")
                        {
                            JavaScriptObject simpleobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str4);
                            if (this.CheckSimpleProduct(simpleobject) && this.UpdateSimpleProduct(simpleobject))
                            {
                                Common_SubjectProduct_Simple simple = new Common_SubjectProduct_Simple();
                                simple.SubjectId = Convert.ToInt32(this.elementId);
                                var typea = new {
                                    Simple = simple.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(typea));
                            }
                        }
                        goto Label_059E;
                    }
                    case "producttabview":
                    {
                        categorys = this.GetCategorys();
                        productBrand = this.GetProductBrand();
                        productTags = this.GetProductTags();
                        string str5 = "{\"Categorys\":" + categorys + ",\"Brands\":" + productBrand + ",\"Tags\":" + productTags + "}";
                        this.message = string.Format(this.resultformat, "true", str5);
                        goto Label_059E;
                    }
                    case "editeproducttab":
                    {
                        string str6 = context.Request.Form["Param"];
                        if (str6 != "")
                        {
                            JavaScriptObject tabobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str6);
                            if (this.CheckProductTab(tabobject) && this.UpdateProductTab(tabobject))
                            {
                                Common_SubjectProduct_Tab tab = new Common_SubjectProduct_Tab();
                                tab.SubjectId = Convert.ToInt32(this.elementId);
                                var typeb = new {
                                    ProductTab = tab.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(typeb));
                            }
                        }
                        goto Label_059E;
                    }
                    case "productfloorview":
                    {
                        categorys = this.GetCategorys();
                        productBrand = this.GetProductBrand();
                        productTags = this.GetProductTags();
                        string str7 = "{\"Categorys\":" + categorys + ",\"Brands\":" + productBrand + ",\"Tags\":" + productTags + "}";
                        this.message = string.Format(this.resultformat, "true", str7);
                        goto Label_059E;
                    }
                    case "editeproductfloor":
                    {
                        string str8 = context.Request.Form["Param"];
                        if (str8 != "")
                        {
                            JavaScriptObject floorobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str8);
                            if (this.CheckProductFloor(floorobject) && this.UpdateProductFloor(floorobject))
                            {
                                Common_SubjectProduct_Floor floor = new Common_SubjectProduct_Floor();
                                floor.SubjectId = Convert.ToInt32(this.elementId);
                                var typec = new {
                                    ProductFloor = floor.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(typec));
                            }
                        }
                        goto Label_059E;
                    }
                    case "productgroupview":
                    {
                        categorys = this.GetCategorys();
                        string str9 = "{\"Categorys\":" + categorys + "}";
                        this.message = string.Format(this.resultformat, "true", str9);
                        goto Label_059E;
                    }
                    case "editeproductgroup":
                    {
                        string str10 = context.Request.Form["Param"];
                        if (str10 != "")
                        {
                            JavaScriptObject groupobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str10);
                            if (this.CheckProductGroup(groupobject) && this.UpdateProductGroup(groupobject))
                            {
                                Common_SubjectProduct_Group group = new Common_SubjectProduct_Group();
                                group.SubjectId = Convert.ToInt32(this.elementId);
                                var typed = new {
                                    ProductGroup = group.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(typed));
                            }
                        }
                        goto Label_059E;
                    }
                    case "producttopview":
                    {
                        categorys = this.GetCategorys();
                        string str11 = "{\"Categorys\":" + categorys + "}";
                        this.message = string.Format(this.resultformat, "true", str11);
                        goto Label_059E;
                    }
                    case "editeproducttop":
                    {
                        string str12 = context.Request.Form["Param"];
                        if (str12 != "")
                        {
                            JavaScriptObject topobject = (JavaScriptObject) JavaScriptConvert.DeserializeObject(str12);
                            if (this.CheckProductTop(topobject) && this.UpdateProductTop(topobject))
                            {
                                Common_SubjectProduct_Top top = new Common_SubjectProduct_Top();
                                top.SubjectId = Convert.ToInt32(this.elementId);
                                var typee = new {
                                    ProductTop = top.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(typee));
                            }
                        }
                        goto Label_059E;
                    }
                }
            }
            catch (Exception exception)
            {
                this.message = "{\"success\":false,\"Result\":\"未知错误:" + exception.Message + "\"}";
            }
        Label_059E:
            context.Response.ContentType = "text/json";
            context.Response.Write(this.message);
        }

        private bool UpdateProductFloor(JavaScriptObject floorobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改商品楼层失败\"");
            this.elementId = floorobject["SubjectId"].ToString().Split(new char[] { '_' })[1];
            floorobject["SubjectId"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(floorobject);
            return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "floor", xmlNodeString);
        }

        private bool UpdateProductGroup(JavaScriptObject groupobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改组合商品失败\"");
            this.elementId = groupobject["SubjectId"].ToString().Split(new char[] { '_' })[1];
            groupobject["SubjectId"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(groupobject);
            return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "group", xmlNodeString);
        }

        private bool UpdateProductTab(JavaScriptObject producttabobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改商品选项卡失败\"");
            this.elementId = producttabobject["SubjectId"].ToString().Split(new char[] { '_' })[1];
            producttabobject["SubjectId"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(producttabobject);
            return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "tab", xmlNodeString);
        }

        private bool UpdateProductTop(JavaScriptObject topobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改销售排行榜失败\"");
            this.elementId = topobject["SubjectId"].ToString().Split(new char[] { '_' })[1];
            topobject["SubjectId"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(topobject);
            return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "top", xmlNodeString);
        }

        private bool UpdateSimpleProduct(JavaScriptObject simpleobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改商品列表失败\"");
            this.elementId = simpleobject["SubjectId"].ToString().Split(new char[] { '_' })[1];
            simpleobject["SubjectId"] = this.elementId;
            Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(simpleobject);
            return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "simple", xmlNodeString);
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


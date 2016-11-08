namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Distribution;
    using Hidistro.Membership.Context;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Services;
    using System.Xml;

    [WebService(Namespace="http://tempuri.org/"), WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1)]
    public class DistributorHandler : IHttpHandler
    {
        public string ConvertTableToXml(DataTable table)
        {
            string str = "";
            string format = "<distributor><userid>{0}</userid><username>{1}</username><email>{2}</email><createdate>{3}</createdate><regionid>{4}</regionid><realname>{5}</realname><balance>{6}</balance><address>{7}</address><zipcode>{8}</zipcode><telphone>{9}</telphone><cellphone>{10}</cellphone><productcount>{11}</productcount><gradeid>{12}</gradeid></distributor>";
            foreach (DataRow row in table.Rows)
            {
                str = str + string.Format(format, new object[] { row["UserId"].ToString(), row["UserName"].ToString(), row["Email"].ToString(), row["CreateDate"].ToString(), row["RegionId"].ToString(), row["RealName"].ToString(), row["Balance"].ToString(), row["Address"].ToString(), row["Zipcode"].ToString(), row["TelPhone"].ToString(), row["CellPhone"].ToString(), row["ProductCount"], row["GradeId"].ToString() });
            }
            return str;
        }

        public SortedDictionary<string, string> GetDistriubots(DistributorQuery query)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            if (query.GradeId.HasValue)
            {
                dictionary.Add("GradeId", query.GradeId.Value.ToString());
            }
            if (query.LineId.HasValue)
            {
                dictionary.Add("LineId", query.LineId.Value.ToString());
            }
            dictionary.Add("SortBy", query.SortBy);
            if (query.SortOrder == SortAction.Desc)
            {
                dictionary.Add("SortOrder", "0");
            }
            else
            {
                dictionary.Add("SortOrder", "1");
            }
            dictionary.Add("RealName", query.RealName);
            dictionary.Add("Username", query.Username);
            dictionary.Add("PageIndex", query.PageIndex.ToString());
            dictionary.Add("PageSize", query.PageSize.ToString());
            return dictionary;
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str2 = "";
            string str3 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str4 = "";
            string s = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            new StringBuilder();
            str2 = context.Request.QueryString["action"].ToString();
            string sign = context.Request.Form["sign"];
            string str7 = context.Request.Form["format"];
            string checkCode = masterSettings.CheckCode;
            XmlDocument node = new XmlDocument();
            new Dictionary<string, string>();
            SortedDictionary<string, string> tmpParas = new SortedDictionary<string, string>();
            try
            {
                string str11;
                if (((str11 = str2) != null) && (str11 == "distribution_list"))
                {
                    string str9 = context.Request.Form["parma"].Trim();
                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "parma");
                    if (!string.IsNullOrEmpty(str9))
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
                        DistributorQuery query = new DistributorQuery();
                        query = (DistributorQuery) JavaScriptConvert.DeserializeObject(str9, typeof(DistributorQuery));
                        tmpParas = this.GetDistriubots(query);
                        tmpParas.Add("action", "distribution_list");
                        tmpParas.Add("format", str7);
                        if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                        {
                            DbQueryResult distributors = DistributorHelper.GetDistributors(query);
                            string format = str + "<response_distributors>{0}<totalcount>{1}</totalcount></response_distributors>";
                            if (distributors.Data != null)
                            {
                                s = string.Format(format, this.ConvertTableToXml((DataTable) distributors.Data), distributors.TotalRecords.ToString());
                            }
                            else
                            {
                                s = string.Format(format, "", "0");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, exception.Message);
            }
            if (s == "")
            {
                s = s + str3 + str4;
            }
            context.Response.ContentType = "text/xml";
            if (str7 == "json")
            {
                s = s.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>", "");
                node.Load(new MemoryStream(Encoding.GetEncoding("UTF-8").GetBytes(s)));
                s = JavaScriptConvert.SerializeXmlNode(node);
                context.Response.ContentType = "text/json";
            }
            context.Response.Write(s);
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


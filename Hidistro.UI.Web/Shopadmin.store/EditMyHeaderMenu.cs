namespace Hidistro.UI.Web.Shopadmin.store
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class EditMyHeaderMenu : DistributorPage
    {
        protected Button btnSave;
        private string category;
        protected SystemPageDropDownList dropSystemPageDropDownList;
        private int id;
        protected BrandCategoriesList listBrandCategories;
        protected DistributorProductCategoriesListBox listProductCategories;
        protected ListBox radProductTags;
        private string themName;
        private string title;
        protected TextBox txtCustomLink;
        protected TextBox txtKeyword;
        protected TextBox txtMaxPrice;
        protected HtmlGenericControl txtMenuNameTip;
        protected TrimTextBox txtMenuType;
        protected TextBox txtMinPrice;
        protected TextBox txtTitle;
        private string url;
        private string where;

        private void btnSave_Click(object sender, EventArgs e)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlNode node in document.SelectSingleNode("root").ChildNodes)
            {
                if (node.Attributes["Id"].Value == this.id.ToString())
                {
                    node.Attributes["Title"].Value = this.txtTitle.Text;
                    node.Attributes["Url"].Value = this.GetUrl();
                    node.Attributes["Where"].Value = this.GetWhere();
                    break;
                }
            }
            document.Save(filename);
            base.Response.Redirect("SetMyHeaderMenu.aspx");
        }

        private string GetUrl()
        {
            if (this.category == "1")
            {
                return this.dropSystemPageDropDownList.SelectedValue;
            }
            if (this.category == "3")
            {
                return this.txtCustomLink.Text;
            }
            return string.Empty;
        }

        private string GetWhere()
        {
            string str = string.Empty;
            if (!(this.category == "2"))
            {
                return str;
            }
            decimal result = 0M;
            decimal num2 = 0M;
            string str2 = str;
            str = str2 + this.listProductCategories.SelectedCategoryId.ToString() + "," + this.listBrandCategories.SelectedValue + "," + this.radProductTags.SelectedValue + ",";
            if (decimal.TryParse(this.txtMinPrice.Text, out result))
            {
                str = str + this.txtMinPrice.Text;
            }
            str = str + ",";
            if (decimal.TryParse(this.txtMaxPrice.Text, out num2))
            {
                str = str + this.txtMaxPrice.Text;
            }
            return (str + "," + this.txtKeyword.Text);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["ThemName"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.themName = base.Request.QueryString["ThemName"];
                int.TryParse(base.Request.QueryString["Id"], out this.id);
                string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                foreach (XmlNode node in document.SelectSingleNode("root").ChildNodes)
                {
                    if (node.Attributes["Id"].Value == this.id.ToString())
                    {
                        this.title = node.Attributes["Title"].Value;
                        this.category = node.Attributes["Category"].Value;
                        this.url = node.Attributes["Url"].Value;
                        this.where = node.Attributes["Where"].Value;
                        break;
                    }
                }
                this.btnSave.Click += new EventHandler(this.btnSave_Click);
                if (!base.IsPostBack)
                {
                    this.txtMenuType.Text = this.category;
                    this.dropSystemPageDropDownList.DataBind();
                    this.listProductCategories.DataBind();
                    this.listBrandCategories.DataBind();
                    this.radProductTags.DataSource = CatalogHelper.GetTags();
                    this.radProductTags.DataTextField = "TagName";
                    this.radProductTags.DataValueField = "TagID";
                    this.radProductTags.DataBind();
                    this.radProductTags.Items.Insert(0, new ListItem("--任意--", "0"));
                    this.txtTitle.Text = this.title;
                    if (this.category == "1")
                    {
                        this.dropSystemPageDropDownList.SelectedValue = this.url;
                    }
                    else if (this.category == "3")
                    {
                        this.txtCustomLink.Text = this.url;
                    }
                    else
                    {
                        string[] strArray = this.where.Split(new char[] { ',' });
                        int result = 0;
                        if (int.TryParse(strArray[0], out result))
                        {
                            this.listProductCategories.SelectedCategoryId = result;
                        }
                        this.listBrandCategories.SelectedValue = strArray[1];
                        this.radProductTags.SelectedValue = strArray[2];
                        this.txtMinPrice.Text = strArray[3];
                        this.txtMaxPrice.Text = strArray[4];
                        this.txtKeyword.Text = strArray[5];
                    }
                }
            }
        }
    }
}


namespace Hidistro.UI.Web.Shopadmin.store
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.Themes)]
    public class AddMyHeaderMenu : DistributorPage
    {
        protected Button btnAdd;
        protected SystemPageDropDownList dropSystemPageDropDownList;
        protected BrandCategoriesList listBrandCategories;
        protected DistributorProductCategoriesListBox listProductCategories;
        protected HeaderMenuTypeRadioButtonList radHeaderMenu;
        protected ListBox radProductTags;
        private string themName;
        protected TextBox txtCustomLink;
        protected TextBox txtKeyword;
        protected TextBox txtMaxPrice;
        protected HtmlGenericControl txtMenuNameTip;
        protected TrimTextBox txtMenuType;
        protected TextBox txtMinPrice;
        protected TextBox txtTitle;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode rootNode = document.SelectSingleNode("root");
            XmlElement newChild = document.CreateElement("Menu");
            newChild.SetAttribute("Id", this.GetId(rootNode));
            newChild.SetAttribute("Title", this.txtTitle.Text);
            newChild.SetAttribute("DisplaySequence", "1");
            newChild.SetAttribute("Category", this.txtMenuType.Text);
            newChild.SetAttribute("Url", this.GetUrl(this.txtMenuType.Text));
            newChild.SetAttribute("Where", this.GetWhere(this.txtMenuType.Text));
            newChild.SetAttribute("Visible", "true");
            rootNode.AppendChild(newChild);
            document.Save(filename);
            base.Response.Redirect("SetMyHeaderMenu.aspx");
        }

        private string GetId(XmlNode rootNode)
        {
            int num = 1;
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (int.Parse(node.Attributes["Id"].Value) > num)
                {
                    num = int.Parse(node.Attributes["Id"].Value);
                }
            }
            int num2 = num + 1;
            return num2.ToString();
        }

        private string GetUrl(string category)
        {
            if (category == "1")
            {
                return this.dropSystemPageDropDownList.SelectedValue;
            }
            if (category == "3")
            {
                return this.txtCustomLink.Text;
            }
            return string.Empty;
        }

        private string GetWhere(string category)
        {
            string str = string.Empty;
            if (!(category == "2"))
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
                this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
                if (!base.IsPostBack)
                {
                    this.dropSystemPageDropDownList.DataBind();
                    this.listProductCategories.DataBind();
                    this.listBrandCategories.DataBind();
                    this.radProductTags.DataSource = CatalogHelper.GetTags();
                    this.radProductTags.DataTextField = "TagName";
                    this.radProductTags.DataValueField = "TagID";
                    this.radProductTags.DataBind();
                    this.radProductTags.Items.Insert(0, new ListItem("--任意--", "0"));
                }
            }
        }
    }
}


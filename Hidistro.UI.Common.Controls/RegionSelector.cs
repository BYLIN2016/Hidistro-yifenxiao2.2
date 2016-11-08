namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class RegionSelector : WebControl
    {
        
        private string _CityTitle;
        
        private string _CountyTitle;
        
        private string _NullToDisplay;
        
        private string _ProvinceTitle;
        
        private string _Separator;
        private int? cityId;
        private int? countyId;
        private int? currentRegionId;
        private bool dataLoaded;
        private WebControl ddlCitys;
        private WebControl ddlCountys;
        private WebControl ddlProvinces;
        private int? provinceId;

        public RegionSelector()
        {
            this.ProvinceTitle = "省：";
            this.CityTitle = "市：";
            this.CountyTitle = "区/县：";
            this.NullToDisplay = "-请选择-";
            this.Separator = "，";
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.dataLoaded)
            {
                if (!string.IsNullOrEmpty(this.Context.Request.Form["regionSelectorValue"]))
                {
                    this.currentRegionId = new int?(int.Parse(this.Context.Request.Form["regionSelectorValue"]));
                }
                this.dataLoaded = true;
            }
            if (this.currentRegionId.HasValue)
            {
                XmlNode region = RegionHelper.GetRegion(this.currentRegionId.Value);
                if (region != null)
                {
                    if (region.Name == "county")
                    {
                        this.countyId = new int?(this.currentRegionId.Value);
                        this.cityId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
                        this.provinceId = new int?(int.Parse(region.ParentNode.ParentNode.Attributes["id"].Value));
                    }
                    else if (region.Name == "city")
                    {
                        this.cityId = new int?(this.currentRegionId.Value);
                        this.provinceId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
                    }
                    else if (region.Name == "province")
                    {
                        this.provinceId = new int?(this.currentRegionId.Value);
                    }
                }
            }
            this.Controls.Add(CreateTitleControl(this.ProvinceTitle));
            this.ddlProvinces = this.CreateDropDownList("ddlRegions1");
            FillDropDownList(this.ddlProvinces, RegionHelper.GetAllProvinces(), this.provinceId);
            this.Controls.Add(CreateTag("<span>"));
            this.Controls.Add(this.ddlProvinces);
            this.Controls.Add(CreateTag("</span>"));
            this.Controls.Add(CreateTitleControl(this.CityTitle));
            this.ddlCitys = this.CreateDropDownList("ddlRegions2");
            if (this.provinceId.HasValue)
            {
                FillDropDownList(this.ddlCitys, RegionHelper.GetCitys(this.provinceId.Value), this.cityId);
            }
            this.Controls.Add(CreateTag("<span>"));
            this.Controls.Add(this.ddlCitys);
            this.Controls.Add(CreateTag("</span>"));
            this.Controls.Add(CreateTitleControl(this.CountyTitle));
            this.ddlCountys = this.CreateDropDownList("ddlRegions3");
            if (this.cityId.HasValue)
            {
                FillDropDownList(this.ddlCountys, RegionHelper.GetCountys(this.cityId.Value), this.countyId);
            }
            this.Controls.Add(CreateTag("<span>"));
            this.Controls.Add(this.ddlCountys);
            this.Controls.Add(CreateTag("</span>"));
        }

        private WebControl CreateDropDownList(string controlId)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.Select);
            control.Attributes.Add("id", controlId);
            control.Attributes.Add("name", controlId);
            control.Attributes.Add("selectset", "regions");
            WebControl child = new WebControl(HtmlTextWriterTag.Option);
            child.Controls.Add(new LiteralControl(this.NullToDisplay));
            child.Attributes.Add("value", "");
            control.Controls.Add(child);
            return control;
        }

        private static WebControl CreateOption(string val, string text)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.Option);
            control.Attributes.Add("value", val);
            control.Controls.Add(new LiteralControl(text.Trim()));
            return control;
        }

        private static Literal CreateTag(string tagName)
        {
            Literal literal2 = new Literal();
            literal2.Text = tagName;
            return literal2;
        }

        private static Label CreateTitleControl(string title)
        {
            Label label2 = new Label();
            label2.Text = title;
            Label label = label2;
            label.Attributes.Add("style", "margin-left:5px");
            return label;
        }

        private static void FillDropDownList(WebControl ddlRegions, Dictionary<int, string> regions, int? selectedId)
        {
            foreach (int num in regions.Keys)
            {
                WebControl child = CreateOption(num.ToString(CultureInfo.InvariantCulture), regions[num]);
                if (selectedId.HasValue && (num == selectedId.Value))
                {
                    child.Attributes.Add("selected", "true");
                }
                ddlRegions.Controls.Add(child);
            }
        }

        public int? GetSelectedRegionId()
        {
            if (!string.IsNullOrEmpty(this.Context.Request.Form["regionSelectorValue"]))
            {
                return new int?(int.Parse(this.Context.Request.Form["regionSelectorValue"]));
            }
            return null;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.AddAttribute("id", "regionSelectorValue");
            writer.AddAttribute("name", "regionSelectorValue");
            writer.AddAttribute("value", this.currentRegionId.HasValue ? this.currentRegionId.Value.ToString(CultureInfo.InvariantCulture) : "");
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.AddAttribute("id", "regionSelectorNull");
            writer.AddAttribute("name", "regionSelectorNull");
            writer.AddAttribute("value", this.NullToDisplay);
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "RegionSelectScript"))
            {
                string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.region.helper.js"));
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "RegionSelectScript", script, false);
            }
        }

        public void SetSelectedRegionId(int? selectedRegionId)
        {
            this.currentRegionId = selectedRegionId;
            this.dataLoaded = true;
        }

        public string CityTitle
        {
            
            get
            {
                return _CityTitle;
            }
            
            set
            {
                _CityTitle = value;
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                base.EnsureChildControls();
                return base.Controls;
            }
        }

        public string CountyTitle
        {
            
            get
            {
                return _CountyTitle;
            }
            
            set
            {
                _CountyTitle = value;
            }
        }

        public string NullToDisplay
        {
            
            get
            {
                return _NullToDisplay;
            }
            
            set
            {
                _NullToDisplay = value;
            }
        }

        public string ProvinceTitle
        {
            
            get
            {
                return _ProvinceTitle;
            }
            
            set
            {
                _ProvinceTitle = value;
            }
        }

        public string SelectedRegions
        {
            get
            {
                int? selectedRegionId = this.GetSelectedRegionId();
                if (!selectedRegionId.HasValue)
                {
                    return "";
                }
                return RegionHelper.GetFullRegion(selectedRegionId.Value, this.Separator);
            }
            set
            {
                string[] strArray = value.Split(new char[] { ',' });
                if (strArray.Length >= 3)
                {
                    int? selectedRegionId = new int?(RegionHelper.GetRegionId(strArray[2], strArray[1], strArray[0]));
                    this.SetSelectedRegionId(selectedRegionId);
                }
            }
        }

        public string Separator
        {
            
            get
            {
                return _Separator;
            }
            
            set
            {
                _Separator = value;
            }
        }
    }
}


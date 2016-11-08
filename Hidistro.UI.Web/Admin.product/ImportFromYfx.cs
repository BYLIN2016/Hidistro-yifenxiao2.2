namespace Hidistro.UI.Web.Admin.product
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.TransferManager;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.ProductBatchUpload)]
    public class ImportFromYfx : AdminPage
    {
        private string _dataPath;
        protected Button btnImport;
        protected Button btnUpload;
        protected CheckBox chkFlag;
        protected CheckBox chkIncludeCostPrice;
        protected CheckBox chkIncludeImages;
        protected CheckBox chkIncludeStock;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductCategoriesDropDownList dropCategories;
        protected DropDownList dropFiles;
        protected DropDownList dropImportVersions;
        protected ProductLineDropDownList dropProductLines;
        protected FileUpload fileUploader;
        protected Literal lblQuantity;
        protected Literal lblVersion;
        protected RadioButton radInStock;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;
        protected TextBox txtMappedTypes;
        protected TextBox txtProductTypeXml;
        protected TextBox txtPTXml;

        private void BindFiles()
        {
            this.dropFiles.Items.Clear();
            this.dropFiles.Items.Add(new ListItem("-请选择-", ""));
            DirectoryInfo info = new DirectoryInfo(this._dataPath);
            foreach (FileInfo info2 in info.GetFiles("*.zip", SearchOption.TopDirectoryOnly))
            {
                string name = info2.Name;
                this.dropFiles.Items.Add(new ListItem(name, name));
            }
        }

        private void BindImporters()
        {
            this.dropImportVersions.Items.Clear();
            this.dropImportVersions.Items.Add(new ListItem("-请选择-", ""));
            Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "分销商城");
            foreach (string str in importAdapters.Keys)
            {
                this.dropImportVersions.Items.Add(new ListItem(importAdapters[str], str));
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.CheckItems())
            {
                string selectedValue = this.dropFiles.SelectedValue;
                string path = Path.Combine(this._dataPath, Path.GetFileNameWithoutExtension(selectedValue));
                ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue, new object[0]);
                DataSet mappingSet = null;
                if (this.txtMappedTypes.Text.Length > 0)
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(this.txtMappedTypes.Text);
                    mappingSet = importer.CreateMapping(new object[] { document, path })[0] as DataSet;
                    ProductHelper.EnsureMapping(mappingSet);
                }
                bool includeCostPrice = this.chkIncludeCostPrice.Checked;
                bool includeStock = this.chkIncludeStock.Checked;
                bool includeImages = this.chkIncludeImages.Checked;
                int categoryId = this.dropCategories.SelectedValue.Value;
                int lineId = this.dropProductLines.SelectedValue.Value;
                int? bandId = this.dropBrandList.SelectedValue;
                ProductSaleStatus delete = ProductSaleStatus.Delete;
                if (this.radInStock.Checked)
                {
                    delete = ProductSaleStatus.OnStock;
                }
                if (this.radUnSales.Checked)
                {
                    delete = ProductSaleStatus.UnSale;
                }
                if (this.radOnSales.Checked)
                {
                    delete = ProductSaleStatus.OnSale;
                }
                ProductHelper.ImportProducts((DataSet) importer.ParseProductData(new object[] { mappingSet, path, includeCostPrice, includeStock, includeImages })[0], categoryId, lineId, bandId, delete, includeCostPrice, includeStock, includeImages);
                File.Delete(Path.Combine(this._dataPath, selectedValue));
                Directory.Delete(path, true);
                this.chkFlag.Checked = false;
                this.txtMappedTypes.Text = string.Empty;
                this.txtProductTypeXml.Text = string.Empty;
                this.txtPTXml.Text = string.Empty;
                this.OutputProductTypes();
                this.BindFiles();
                this.ShowMsg("此次商品批量导入操作已成功！", true);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.dropImportVersions.SelectedValue.Length == 0)
            {
                this.ShowMsg("请先选择一个导入插件", false);
            }
            else if (!this.fileUploader.HasFile)
            {
                this.ShowMsg("请先选择一个数据包文件", false);
            }
            else if ((this.fileUploader.PostedFile.ContentLength == 0) || (((this.fileUploader.PostedFile.ContentType != "application/x-zip-compressed") && (this.fileUploader.PostedFile.ContentType != "application/zip")) && (this.fileUploader.PostedFile.ContentType != "application/octet-stream")))
            {
                this.ShowMsg("请上传正确的数据包文件", false);
            }
            else
            {
                string fileName = Path.GetFileName(this.fileUploader.PostedFile.FileName);
                this.fileUploader.PostedFile.SaveAs(Path.Combine(this._dataPath, fileName));
                this.BindFiles();
                this.dropFiles.SelectedValue = fileName;
                this.PrepareZipFile(fileName);
            }
        }

        private bool CheckItems()
        {
            string str = "";
            if (this.dropImportVersions.SelectedValue.Length == 0)
            {
                str = str + Formatter.FormatErrorMessage("请选择一个导入插件！");
            }
            if (this.dropFiles.SelectedValue.Length == 0)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的数据包文件！");
            }
            if (!this.dropCategories.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的商品分类！");
            }
            if (!this.dropProductLines.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的产品线！");
            }
            if (string.IsNullOrEmpty(str) && (str.Length <= 0))
            {
                return true;
            }
            this.ShowMsg(str, false);
            return false;
        }

        private void DoCallback()
        {
            base.Response.Clear();
            base.Response.ContentType = "text/xml";
            switch (base.Request.QueryString["action"])
            {
                case "getAttributes":
                {
                    IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(int.Parse(base.Request.QueryString["typeId"]));
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<xml><attributes>");
                    foreach (AttributeInfo info in attributes)
                    {
                        builder.Append("<item attributeId=\"").Append(info.AttributeId.ToString(CultureInfo.InvariantCulture)).Append("\" attributeName=\"").Append(info.AttributeName).Append("\" typeId=\"").Append(info.TypeId.ToString(CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    builder.Append("</attributes></xml>");
                    base.Response.Write(builder.ToString());
                    break;
                }
                case "getValues":
                {
                    AttributeInfo attribute = ProductTypeHelper.GetAttribute(int.Parse(base.Request.QueryString["attributeId"]));
                    StringBuilder builder2 = new StringBuilder();
                    builder2.Append("<xml><values>");
                    if ((attribute != null) && (attribute.AttributeValues.Count > 0))
                    {
                        foreach (AttributeValueInfo info3 in attribute.AttributeValues)
                        {
                            builder2.Append("<item valueId=\"").Append(info3.ValueId.ToString(CultureInfo.InvariantCulture)).Append("\" valueStr=\"").Append(info3.ValueStr).Append("\" attributeId=\"").Append(info3.AttributeId.ToString(CultureInfo.InvariantCulture)).Append("\" />");
                        }
                    }
                    builder2.Append("</values></xml>");
                    base.Response.Write(builder2.ToString());
                    break;
                }
            }
            base.Response.End();
        }

        private void dropFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.dropFiles.SelectedValue.Length > 0) && (this.dropImportVersions.SelectedValue.Length == 0))
            {
                this.ShowMsg("请先选择一个导入插件", false);
                this.dropFiles.SelectedValue = "";
            }
            else
            {
                this.PrepareZipFile(this.dropFiles.SelectedValue);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            if (base.Request.QueryString["isCallback"] == "true")
            {
                this.DoCallback();
            }
            else
            {
                this._dataPath = this.Page.Request.MapPath("~/storage/data/yfx");
                this.btnImport.Click += new EventHandler(this.btnImport_Click);
                this.btnUpload.Click += new EventHandler(this.btnUpload_Click);
                this.dropFiles.SelectedIndexChanged += new EventHandler(this.dropFiles_SelectedIndexChanged);
                if (!this.Page.IsPostBack)
                {
                    this.dropCategories.DataBind();
                    this.dropProductLines.DataBind();
                    this.dropBrandList.DataBind();
                    this.BindImporters();
                    this.BindFiles();
                    this.OutputProductTypes();
                }
            }
        }

        private void OutputProductTypes()
        {
            IList<ProductTypeInfo> productTypes = ControlProvider.Instance().GetProductTypes();
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml><types>");
            foreach (ProductTypeInfo info in productTypes)
            {
                builder.Append("<item typeId=\"").Append(info.TypeId.ToString(CultureInfo.InvariantCulture)).Append("\" typeName=\"").Append(info.TypeName).Append("\" />");
            }
            builder.Append("</types></xml>");
            this.txtProductTypeXml.Text = builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void PrepareZipFile(string filename)
        {
            if (string.IsNullOrEmpty(filename) || (filename.Length == 0))
            {
                this.chkFlag.Checked = false;
                this.txtPTXml.Text = string.Empty;
            }
            else
            {
                filename = Path.Combine(this._dataPath, filename);
                if (!File.Exists(filename))
                {
                    this.chkFlag.Checked = false;
                    this.txtPTXml.Text = string.Empty;
                }
                else
                {
                    ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue, new object[0]);
                    string str = importer.PrepareDataFiles(new object[] { filename });
                    object[] objArray = importer.ParseIndexes(new object[] { str });
                    this.lblVersion.Text = (string) objArray[0];
                    this.lblQuantity.Text = objArray[1].ToString();
                    this.chkIncludeCostPrice.Checked = (bool) objArray[2];
                    this.chkIncludeStock.Checked = (bool) objArray[3];
                    this.chkIncludeImages.Checked = (bool) objArray[4];
                    this.txtPTXml.Text = (string) objArray[5];
                    this.chkFlag.Checked = true;
                }
            }
        }
    }
}


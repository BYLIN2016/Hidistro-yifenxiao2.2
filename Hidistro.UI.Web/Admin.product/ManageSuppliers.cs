namespace Hidistro.UI.Web.Admin.product
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class ManageSuppliers : AdminPage
    {
        protected ImageLinkButton btnDelete;
        protected ImageLinkButton btnDelete1;
        protected Button btnOk;
        protected CheckBox chkAddFlag;
        protected Grid grdSuppliers;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected TrimTextBox txtOldSupplierName;
        protected TrimTextBox txtSupplierDescription;
        protected TrimTextBox txtSupplierName;

        private void BindSuppliers()
        {
            Pagination pagination2 = new Pagination();
            pagination2.PageIndex = this.pager.PageIndex;
            pagination2.PageSize = this.hrefPageSize.SelectedSize;
            Pagination page = pagination2;
            DbQueryResult suppliers = ProductLineHelper.GetSuppliers(page);
            this.grdSuppliers.DataSource = suppliers.Data;
            this.grdSuppliers.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = suppliers.TotalRecords;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.DeleteSelected();
            this.BindSuppliers();
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            this.DeleteSelected();
            this.BindSuppliers();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.CheckValues())
            {
                bool flag;
                if (this.chkAddFlag.Checked)
                {
                    flag = ProductLineHelper.AddSupplier(Globals.HtmlEncode(this.txtSupplierName.Text.Replace(',', ' ')), Globals.HtmlEncode(this.txtSupplierDescription.Text));
                }
                else
                {
                    flag = ProductLineHelper.UpdateSupplier(Globals.HtmlEncode(this.txtOldSupplierName.Text), Globals.HtmlEncode(this.txtSupplierName.Text.Replace(',', ' ')), Globals.HtmlEncode(this.txtSupplierDescription.Text));
                }
                if (!flag)
                {
                    this.ShowMsg("操作失败，供货商名称不能重复！", false);
                }
                else
                {
                    this.BindSuppliers();
                }
            }
        }

        private bool CheckValues()
        {
            if (this.txtSupplierName.Text.Length == 0)
            {
                this.ShowMsg("请填写供货商名称！", false);
                return false;
            }
            if (this.txtSupplierName.Text.Length > 50)
            {
                this.ShowMsg("供货商名称的长度不能超过50个字符！", false);
                return false;
            }
            if (this.txtSupplierDescription.Text.Length > 500)
            {
                this.ShowMsg("供货商描述的长度不能超过500个字符！", false);
                return false;
            }
            return true;
        }

        private void DeleteSelected()
        {
            if (string.IsNullOrEmpty(base.Request.Form["CheckBoxGroup"]))
            {
                this.ShowMsg("请先勾选要删除的供货商！", false);
            }
            else
            {
                foreach (string str in base.Request.Form["CheckBoxGroup"].Split(new char[] { ',' }))
                {
                    ProductLineHelper.DeleteSupplier(Globals.HtmlEncode(str));
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnDelete1.Click += new EventHandler(this.btnDelete1_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindSuppliers();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
    }
}


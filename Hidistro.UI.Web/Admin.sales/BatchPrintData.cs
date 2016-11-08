namespace Hidistro.UI.Web.Admin.sales
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class BatchPrintData : AdminPage
    {
        protected Button btnPrint;
        protected Button btnUpdateAddrdss;
        protected ShippersDropDownList ddlShoperTag;
        protected DropDownList ddlTemplates;
        protected RegionSelector dropRegions;
        protected Literal litNumber;
        protected static string orderIds = string.Empty;
        protected Panel pnlEmptySender;
        protected Panel pnlEmptyTemplates;
        protected Panel pnlShipper;
        protected Panel pnlTask;
        protected Panel pnlTaskEmpty;
        protected Panel pnlTemplates;
        protected TextBox txtAddress;
        protected TextBox txtCellphone;
        protected TextBox txtShipTo;
        protected TextBox txtStartCode;
        protected TextBox txtTelphone;
        protected TextBox txtZipcode;

        private void btnUpdateAddrdss_Click(object sender, EventArgs e)
        {
            if (!this.dropRegions.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择发货地区！", false);
            }
            else if (this.UpdateAddress())
            {
                this.ShowMsg("修改成功", true);
            }
            else
            {
                this.ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
            }
        }

        private void ddlShoperTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadShipper();
        }

        private void LoadShipper()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                this.txtAddress.Text = shipper.Address;
                this.txtCellphone.Text = shipper.CellPhone;
                this.txtShipTo.Text = shipper.ShipperName;
                this.txtTelphone.Text = shipper.TelPhone;
                this.txtZipcode.Text = shipper.Zipcode;
                this.dropRegions.SetSelectedRegionId(new int?(shipper.RegionId));
                this.pnlEmptySender.Visible = false;
                this.pnlShipper.Visible = true;
            }
            else
            {
                this.pnlShipper.Visible = false;
                this.pnlEmptySender.Visible = true;
            }
        }

        private void LoadTemplates()
        {
            DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
            if ((isUserExpressTemplates != null) && (isUserExpressTemplates.Rows.Count > 0))
            {
                this.ddlTemplates.Items.Add(new ListItem("-请选择-", ""));
                foreach (DataRow row in isUserExpressTemplates.Rows)
                {
                    this.ddlTemplates.Items.Add(new ListItem(row["ExpressName"].ToString(), row["XmlFile"].ToString()));
                }
                this.pnlEmptyTemplates.Visible = false;
                this.pnlTemplates.Visible = true;
            }
            else
            {
                this.pnlEmptyTemplates.Visible = true;
                this.pnlTemplates.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["OrderIds"]))
            {
                orderIds = base.Request["OrderIds"];
                this.litNumber.Text = orderIds.Trim(new char[] { ',' }).Split(new char[] { ',' }).Length.ToString();
            }
            this.ddlShoperTag.SelectedIndexChanged += new EventHandler(this.ddlShoperTag_SelectedIndexChanged);
            this.btnUpdateAddrdss.Click += new EventHandler(this.btnUpdateAddrdss_Click);
            if (!this.Page.IsPostBack)
            {
                this.ddlShoperTag.DataBind();
                foreach (ShippersInfo info in SalesHelper.GetShippers(false))
                {
                    if (info.IsDefault)
                    {
                        this.ddlShoperTag.SelectedValue = info.ShipperId;
                    }
                }
                this.LoadShipper();
                this.LoadTemplates();
            }
        }

        private bool UpdateAddress()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                shipper.Address = this.txtAddress.Text;
                shipper.CellPhone = this.txtCellphone.Text;
                shipper.RegionId = this.dropRegions.GetSelectedRegionId().Value;
                shipper.ShipperName = this.txtShipTo.Text;
                shipper.TelPhone = this.txtTelphone.Text;
                shipper.Zipcode = this.txtZipcode.Text;
                return SalesHelper.UpdateShipper(shipper);
            }
            return false;
        }
    }
}


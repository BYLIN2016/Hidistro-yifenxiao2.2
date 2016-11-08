namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class MyGifts : DistributorPage
    {
        protected Button btnSearchButton;
        private string giftName;
        protected Grid grdGift;
        protected PageSize hrefPageSize;
        protected LinkButton lkbtnDownloadCheck1;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtSearchText;

        private void BindData()
        {
            GiftQuery query = new GiftQuery();
            query.Name = Globals.HtmlEncode(this.giftName);
            query.Page.PageSize = this.pager.PageSize;
            query.Page.PageIndex = this.pager.PageIndex;
            query.Page.SortBy = this.grdGift.SortOrderBy;
            if (this.grdGift.SortOrder.ToLower() == "desc")
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            DbQueryResult gifts = SubsiteGiftHelper.GetGifts(query);
            this.grdGift.DataSource = gifts.Data;
            this.grdGift.DataBind();
            this.pager.TotalRecords = gifts.TotalRecords;
            this.pager1.TotalRecords = gifts.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadGiftsList(true);
        }

        private void DownLoad()
        {
            if ((!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && (base.Request.QueryString["oper"].Trim() == "download")) && (!string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && (int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0)))
            {
                GiftInfo giftDetails = GiftHelper.GetGiftDetails(int.Parse(base.Request.QueryString["GiftId"].Trim()));
                if (giftDetails.IsDownLoad && (giftDetails.PurchasePrice > 0M))
                {
                    if (SubsiteGiftHelper.DownLoadGift(giftDetails))
                    {
                        this.ReloadGiftsList(true);
                        this.ShowMsg("下载礼品" + giftDetails.Name + "成功！", true);
                    }
                    else
                    {
                        this.ShowMsg("下载礼品" + giftDetails.Name + "失败！", false);
                    }
                }
            }
        }

        private void grdGift_ReBindData(object sender)
        {
            this.ReloadGiftsList(false);
        }

        private void lkbtnDownloadCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in this.grdGift.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    num++;
                }
            }
            if (num == 0)
            {
                this.ShowMsg("请先选择要下载的礼品", false);
            }
            else
            {
                foreach (GridViewRow row2 in this.grdGift.Rows)
                {
                    CheckBox box2 = (CheckBox) row2.FindControl("checkboxCol");
                    if (box2.Checked)
                    {
                        int giftId = (int) this.grdGift.DataKeys[row2.RowIndex].Value;
                        SubsiteGiftHelper.DownLoadGift(GiftHelper.GetGiftDetails(giftId));
                    }
                }
                this.ShowMsg("下载的礼品成功", true);
                this.ReloadGiftsList(true);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GiftName"]))
                {
                    this.giftName = this.Page.Request.QueryString["GiftName"];
                }
                this.txtSearchText.Text = Globals.HtmlDecode(this.giftName);
            }
            else
            {
                this.giftName = Globals.HtmlEncode(this.txtSearchText.Text.Trim());
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDownloadCheck1.Click += new EventHandler(this.lkbtnDownloadCheck_Click);
            this.grdGift.ReBindData += new Grid.ReBindDataEventHandler(this.grdGift_ReBindData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
                this.DownLoad();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadGiftsList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("GiftName", Globals.HtmlEncode(this.txtSearchText.Text.Trim()));
            queryStrings.Add("pageSize", this.hrefPageSize.SelectedSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}


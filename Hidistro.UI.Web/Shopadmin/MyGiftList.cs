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
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MyGiftList : DistributorPage
    {
        protected Button btnSearchButton;
        protected HtmlInputCheckBox chkPromotion;
        private string giftName;
        protected Grid grdGift;
        protected PageSize hrefPageSize;
        private bool isPromotion;
        protected LinkButton lkbtnDelete;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtSearchText;

        private void BindData()
        {
            GiftQuery query = new GiftQuery();
            query.Name = Globals.HtmlEncode(this.giftName);
            query.IsPromotion = this.isPromotion;
            query.Page.PageSize = this.pager.PageSize;
            query.Page.PageIndex = this.pager.PageIndex;
            query.Page.SortBy = this.grdGift.SortOrderBy;
            if (this.grdGift.SortOrder.ToLower() == "desc")
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            DbQueryResult abstroGiftsById = SubsiteGiftHelper.GetAbstroGiftsById(query);
            this.grdGift.DataSource = abstroGiftsById.Data;
            this.grdGift.DataBind();
            this.pager.TotalRecords = abstroGiftsById.TotalRecords;
            this.pager1.TotalRecords = abstroGiftsById.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadGiftsList(true);
        }

        private void DeleteGriftById()
        {
            if ((!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && (base.Request.QueryString["oper"].Trim() == "delete")) && (!string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && (int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0)))
            {
                int giftId = int.Parse(base.Request.QueryString["GiftId"].Trim());
                GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
                if (SubsiteGiftHelper.DeleteGiftById(giftId))
                {
                    this.ReloadGiftsList(true);
                    this.ShowMsg("删除礼品" + giftDetails.Name + "成功！", true);
                }
                else
                {
                    this.ShowMsg("删除礼品" + giftDetails.Name + "失败！", false);
                }
            }
        }

        private void grdGift_ReBindData(object sender)
        {
            this.ReloadGiftsList(false);
        }

        private void lkbtnDelete_Click(object sender, EventArgs e)
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
                this.ShowMsg("请先选择要删除的礼品", false);
            }
            else
            {
                bool success = true;
                string msg = "删除礼品成功！";
                foreach (GridViewRow row2 in this.grdGift.Rows)
                {
                    CheckBox box2 = (CheckBox) row2.FindControl("checkboxCol");
                    if (box2.Checked)
                    {
                        int giftId = (int) this.grdGift.DataKeys[row2.RowIndex].Value;
                        if (!SubsiteGiftHelper.DeleteGiftById(giftId))
                        {
                            success = false;
                            msg = "删除礼品失败！";
                            break;
                        }
                    }
                }
                this.ShowMsg(msg, success);
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isPromotion"]))
                {
                    bool.TryParse(this.Page.Request.QueryString["isPromotion"], out this.isPromotion);
                }
                this.chkPromotion.Checked = this.isPromotion;
                this.txtSearchText.Text = Globals.HtmlDecode(this.giftName);
            }
            else
            {
                this.giftName = Globals.HtmlEncode(this.txtSearchText.Text.Trim());
                this.isPromotion = this.chkPromotion.Checked;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDelete.Click += new EventHandler(this.lkbtnDelete_Click);
            this.grdGift.ReBindData += new Grid.ReBindDataEventHandler(this.grdGift_ReBindData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
                this.DeleteGriftById();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadGiftsList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("GiftName", Globals.HtmlEncode(this.txtSearchText.Text.Trim()));
            queryStrings.Add("isPromotion", this.chkPromotion.Checked.ToString());
            queryStrings.Add("pageSize", this.hrefPageSize.SelectedSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}


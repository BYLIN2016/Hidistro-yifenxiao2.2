namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Gifts)]
    public class Gifts : AdminPage
    {
        protected Button btnSearchButton;
        protected HtmlInputCheckBox chkPromotion;
        private string giftName;
        protected Grid grdGift;
        protected PageSize hrefPageSize;
        private bool isPromotion;
        protected ImageLinkButton lkbDelectCheck;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtSearchText;

        private void BindData()
        {
            GiftQuery query = new GiftQuery();
            query.Name = this.giftName;
            query.IsPromotion = this.isPromotion;
            query.Page.PageSize = this.pager.PageSize;
            query.Page.PageIndex = this.pager.PageIndex;
            query.Page.SortBy = this.grdGift.SortOrderBy;
            if (this.grdGift.SortOrder.ToLower() == "desc")
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            DbQueryResult gifts = GiftHelper.GetGifts(query);
            this.grdGift.DataSource = gifts.Data;
            this.grdGift.DataBind();
            this.pager.TotalRecords = gifts.TotalRecords;
            this.pager1.TotalRecords = gifts.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadGiftsList(true);
        }

        private void grdGift_ReBindData(object sender)
        {
            this.BindData();
        }

        private void grdGift_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int giftId = (int) this.grdGift.DataKeys[e.RowIndex].Value;
            if (GiftHelper.DeleteGift(giftId))
            {
                this.ShowMsg("成功的删除了一件礼品信息", true);
                this.BindData();
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        private void grdGift_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int num = (int) this.grdGift.DataKeys[e.NewEditIndex].Value;
            HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/promotion/EditGift.aspx?giftId={0}", num)));
        }

        private void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in this.grdGift.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked && GiftHelper.DeleteGift(Convert.ToInt32(this.grdGift.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture)))
                {
                    num++;
                }
            }
            if (num > 0)
            {
                this.ShowMsg(string.Format("成功的删除了{0}件礼品", num), true);
                this.BindData();
            }
            else
            {
                this.ShowMsg("请选择您要删除的礼品", false);
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
                this.txtSearchText.Text = Globals.HtmlDecode(this.giftName);
                this.chkPromotion.Checked = this.isPromotion;
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
            this.lkbDelectCheck.Click += new EventHandler(this.lkbDelectCheck_Click);
            this.grdGift.RowEditing += new GridViewEditEventHandler(this.grdGift_RowEditing);
            this.grdGift.RowDeleting += new GridViewDeleteEventHandler(this.grdGift_RowDeleting);
            this.grdGift.ReBindData += new Grid.ReBindDataEventHandler(this.grdGift_ReBindData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
                this.UpdateIsDownLoad();
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

        private void UpdateIsDownLoad()
        {
            if (((!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && (base.Request.QueryString["oper"].Trim() == "update")) && (!string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && !string.IsNullOrEmpty(base.Request.QueryString["Status"]))) && ((int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0) && (int.Parse(base.Request.QueryString["Status"].Trim()) >= 0)))
            {
                int giftId = int.Parse(base.Request.QueryString["GiftId"]);
                bool isdownload = false;
                string str = "取消";
                if (int.Parse(base.Request.QueryString["Status"].Trim()) == 1)
                {
                    isdownload = true;
                    str = "允许";
                }
                if (GiftHelper.UpdateIsDownLoad(giftId, isdownload))
                {
                    this.BindData();
                    this.ShowMsg(str + "当前礼品下载成功！", true);
                }
                else
                {
                    this.ShowMsg(str + "当前礼品下载失败！", false);
                }
            }
        }
    }
}


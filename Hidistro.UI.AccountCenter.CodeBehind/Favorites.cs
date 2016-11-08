namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class Favorites : MemberTemplatedWebControl
    {
        private LinkButton btnDeleteSelect;
        private IButton btnSearch;
        private Common_Favorite_ProductList favorites;
        private Pager pager;
        private TextBox txtKeyWord;

        protected override void AttachChildControls()
        {
            this.favorites = (Common_Favorite_ProductList) this.FindControl("list_Common_Favorite_ProList");
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.txtKeyWord = (TextBox) this.FindControl("txtKeyWord");
            this.pager = (Pager) this.FindControl("pager");
            this.btnDeleteSelect = (LinkButton) this.FindControl("btnDeleteSelect");
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.favorites._ItemCommand += new Common_Favorite_ProductList.CommandEventHandler(this.favorites_ItemCommand);
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            PageTitle.AddSiteNameTitle("商品收藏夹", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductId"]))
                {
                    int result = 0;
                    int.TryParse(this.Page.Request.QueryString["ProductId"], out result);
                    if (!CommentsHelper.ExistsProduct(result) && !CommentsHelper.AddProductToFavorite(result))
                    {
                        this.ShowMessage("添加商品到收藏夹失败", false);
                    }
                }
                this.BindList();
            }
        }

        private void BindList()
        {
            Pagination page = new Pagination();
            page.PageIndex = this.pager.PageIndex;
            page.PageSize = this.pager.PageSize;
            string tags = string.Empty;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
            {
                tags = this.Page.Request.QueryString["keyword"];
            }
            DbQueryResult favorites = CommentsHelper.GetFavorites(tags, page);
            this.favorites.DataSource = favorites.Data;
            this.favorites.DataBind();
            this.txtKeyWord.Text = tags;
            this.pager.TotalRecords = favorites.TotalRecords;
        }

        protected void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            string ids = this.Page.Request["CheckboxGroup"];
            if (!CommentsHelper.DeleteFavorites(ids))
            {
                this.ShowMessage("删除失败", false);
            }
            else
            {
                this.BindList();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadFavorites();
        }

        protected void favorites_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int favoriteId = (int) this.favorites.DataKeys[e.Item.ItemIndex];
            if (e.CommandName == "Edit")
            {
                this.favorites.EditItemIndex = e.Item.ItemIndex;
                this.BindList();
            }
            if (e.CommandName == "Cancel")
            {
                this.favorites.EditItemIndex = -1;
                this.BindList();
            }
            if (e.CommandName == "Update")
            {
                TextBox box = (TextBox) e.Item.FindControl("txtTags");
                TextBox box2 = (TextBox) e.Item.FindControl("txtRemark");
                if (box.Text.Length > 100)
                {
                    this.ShowMessage("修改商品收藏信息失败，标签信息的长度限制在100个字符以內", false);
                    return;
                }
                if (box2.Text.Length > 500)
                {
                    this.ShowMessage("修改商品收藏信息失败，备注信息的长度限制在500個字符以內", false);
                    return;
                }
                if (CommentsHelper.UpdateFavorite(favoriteId, Globals.HtmlEncode(box.Text.Trim()), Globals.HtmlEncode(box2.Text.Trim())) > 0)
                {
                    this.favorites.EditItemIndex = -1;
                    this.BindList();
                    this.ShowMessage("成功的修改了收藏夹的信息", true);
                }
                else
                {
                    this.ShowMessage("没有修改你要修改的內容", false);
                }
            }
            if (e.CommandName == "Deleted")
            {
                if (CommentsHelper.DeleteFavorite(favoriteId) > 0)
                {
                    this.BindList();
                    this.ShowMessage("成功删除了选择的收藏商品", true);
                }
                else
                {
                    this.ShowMessage("删除失败", false);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-Favorites.html";
            }
            base.OnInit(e);
        }

        private void ReloadFavorites()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("keyword", this.txtKeyWord.Text.Trim());
            base.ReloadPage(queryStrings);
        }
    }
}


namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_CutdownSearch : AscxTemplatedWebControl
    {
        private IButton btnSearch;
        private ProductTagsCheckBoxList ckbListproductSearchType;
        private ReSearchEventHandler ReSearch;
        public const string TagID = "search_Common_CutdownSearch";
        private TextBox txtEndPrice;
        private TextBox txtKeywords;
        private TextBox txtStartPrice;

        public event ReSearchEventHandler _ReSearch
        {
            add
            {
                ReSearchEventHandler handler2;
                ReSearchEventHandler reSearch = this.ReSearch;
                do
                {
                    handler2 = reSearch;
                    ReSearchEventHandler handler3 = (ReSearchEventHandler) Delegate.Combine(handler2, value);
                    reSearch = Interlocked.CompareExchange<ReSearchEventHandler>(ref this.ReSearch, handler3, handler2);
                }
                while (reSearch != handler2);
            }
            remove
            {
                ReSearchEventHandler handler2;
                ReSearchEventHandler reSearch = this.ReSearch;
                do
                {
                    handler2 = reSearch;
                    ReSearchEventHandler handler3 = (ReSearchEventHandler) Delegate.Remove(handler2, value);
                    reSearch = Interlocked.CompareExchange<ReSearchEventHandler>(ref this.ReSearch, handler3, handler2);
                }
                while (reSearch != handler2);
            }
        }

        public Common_CutdownSearch()
        {
            base.ID = "search_Common_CutdownSearch";
        }

        protected override void AttachChildControls()
        {
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.txtKeywords = (TextBox) this.FindControl("txtKeywords");
            this.txtStartPrice = (TextBox) this.FindControl("txtStartPrice");
            this.txtEndPrice = (TextBox) this.FindControl("txtEndPrice");
            this.ckbListproductSearchType = (ProductTagsCheckBoxList) this.FindControl("ckbListproductSearchType");
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
                {
                    this.txtKeywords.Text = DataHelper.CleanSearchString(Globals.UrlDecode(this.Page.Request.QueryString["keywords"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
                {
                    this.txtStartPrice.Text = this.Page.Request.QueryString["minSalePrice"].ToString();
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
                {
                    this.txtEndPrice.Text = this.Page.Request.QueryString["maxSalePrice"].ToString();
                }
            }
            this.ckbListproductSearchType.DataBind();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
            {
                IList<int> list = new List<int>();
                foreach (string str in this.Page.Request.QueryString["TagIds"].Split(new char[] { '_' }))
                {
                    list.Add(Convert.ToInt32(str));
                }
                this.ckbListproductSearchType.SelectedValue = list;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.OnReSearch(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_Search/Skin-Common_CutdownSearch.ascx";
            }
            base.OnInit(e);
        }

        public void OnReSearch(object sender, EventArgs e)
        {
            if (this.ReSearch != null)
            {
                this.ReSearch(sender, e);
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public ProductBrowseQuery Item
        {
            get
            {
                ProductBrowseQuery entity = new ProductBrowseQuery();
                if (this.txtKeywords != null)
                {
                    entity.Keywords = this.txtKeywords.Text.Trim();
                }
                if (this.txtStartPrice != null)
                {
                    decimal result = 0M;
                    if (!string.IsNullOrEmpty(this.txtStartPrice.Text.Trim()) && decimal.TryParse(this.txtStartPrice.Text.Trim(), out result))
                    {
                        entity.MinSalePrice = new decimal?(result);
                    }
                    else
                    {
                        entity.MinSalePrice = null;
                    }
                }
                if (this.txtEndPrice != null)
                {
                    decimal num2 = 0M;
                    if (!string.IsNullOrEmpty(this.txtEndPrice.Text.Trim()) && decimal.TryParse(this.txtEndPrice.Text.Trim(), out num2))
                    {
                        entity.MaxSalePrice = new decimal?(num2);
                    }
                    else
                    {
                        entity.MaxSalePrice = null;
                    }
                }
                entity.ProductCode = "";
                string str = string.Empty;
                IList<int> selectedValue = this.ckbListproductSearchType.SelectedValue;
                if ((selectedValue != null) && (selectedValue.Count > 0))
                {
                    foreach (int num3 in selectedValue)
                    {
                        str = str + num3.ToString() + "_";
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                entity.TagIds = str;
                Globals.EntityCoding(entity, true);
                return entity;
            }
        }

        public delegate void ReSearchEventHandler(object sender, EventArgs e);
    }
}


namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.HotKeywords)]
    public class ManageHotKeywords : AdminPage
    {
        protected Button btnEditHotkeyword;
        protected Button btnSubmitHotkeyword;
        protected ProductCategoriesDropDownList dropCategory;
        protected ProductCategoriesDropDownList dropEditCategory;
        protected Grid grdHotKeywords;
        protected HtmlInputHidden hicategory;
        protected HtmlInputHidden hiHotKeyword;
        protected TextBox txtEditHotKeyword;
        protected HtmlInputHidden txtHid;
        protected TextBox txtHotKeywords;

        private void BindData()
        {
            this.grdHotKeywords.DataSource = StoreHelper.GetHotKeywords();
            this.grdHotKeywords.DataBind();
        }

        private void btnEditHotkeyword_Click(object sender, EventArgs e)
        {
            int hid = Convert.ToInt32(this.txtHid.Value);
            if (string.IsNullOrEmpty(this.txtEditHotKeyword.Text.Trim()) || (this.txtEditHotKeyword.Text.Trim().Length > 60))
            {
                this.ShowMsg("热门关键字不能为空,长度限制在60个字符以内", false);
            }
            else if (!this.dropEditCategory.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择商品主分类", false);
            }
            else
            {
                Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (!regex.IsMatch(this.txtEditHotKeyword.Text.Trim()))
                {
                    this.ShowMsg("热门关键字只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                }
                else if ((string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value) != 0) && this.IsSame(this.txtEditHotKeyword.Text.Trim(), Convert.ToInt32(this.dropEditCategory.SelectedValue.Value)))
                {
                    this.ShowMsg("存在相同的的关键字，编辑失败", false);
                }
                else if ((((string.Compare(this.dropEditCategory.SelectedValue.Value.ToString(), this.hicategory.Value) == 0) & (string.Compare(this.txtEditHotKeyword.Text, this.hiHotKeyword.Value) != 0)) && this.IsSame(this.txtEditHotKeyword.Text.Trim(), Convert.ToInt32(this.dropEditCategory.SelectedValue.Value))) || (((string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value) == 0) && (string.Compare(this.dropEditCategory.SelectedValue.Value.ToString(), this.hicategory.Value) != 0)) && this.IsSame(this.txtEditHotKeyword.Text.Trim(), Convert.ToInt32(this.dropEditCategory.SelectedValue.Value))))
                {
                    this.ShowMsg("同一分类型不允许存在相同的关键字,编辑失败", false);
                }
                else
                {
                    StoreHelper.UpdateHotWords(hid, this.dropEditCategory.SelectedValue.Value, this.txtEditHotKeyword.Text.Trim());
                    this.ShowMsg("编辑热门关键字成功！", true);
                    this.hicategory.Value = "";
                    this.hiHotKeyword.Value = "";
                    this.BindData();
                }
            }
        }

        private void btnSubmitHotkeyword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHotKeywords.Text.Trim()))
            {
                this.ShowMsg("热门关键字不能为空", false);
            }
            else if (!this.dropCategory.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择商品主分类", false);
            }
            else
            {
                string[] strArray = this.txtHotKeywords.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                int num = 0;
                foreach (string str2 in strArray)
                {
                    Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                    if (regex.IsMatch(str2) && !this.IsSame(str2, Convert.ToInt32(this.dropCategory.SelectedValue.Value)))
                    {
                        StoreHelper.AddHotkeywords(this.dropCategory.SelectedValue.Value, str2);
                        num++;
                    }
                }
                if (num > 0)
                {
                    this.ShowMsg(string.Format("成功添加了{0}个热门关键字", num), true);
                    this.txtHotKeywords.Text = "";
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("添加失败，请检查是否存在同类型的同名关键字", false);
                }
            }
        }

        private void grdHotKeywords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Fall") || (e.CommandName == "Rise"))
            {
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int hid = (int) this.grdHotKeywords.DataKeys[rowIndex].Value;
                int displaySequence = int.Parse((this.grdHotKeywords.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceHid = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if ((rowIndex + 1) != this.grdHotKeywords.Rows.Count)
                    {
                        replaceHid = (int) this.grdHotKeywords.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = int.Parse((this.grdHotKeywords.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex != 0))
                {
                    replaceHid = (int) this.grdHotKeywords.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = int.Parse((this.grdHotKeywords.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if (replaceHid != 0)
                {
                    StoreHelper.SwapHotWordsSequence(hid, replaceHid, displaySequence, replaceDisplaySequence);
                    this.BindData();
                }
            }
        }

        private void grdHotKeywords_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int hid = (int) this.grdHotKeywords.DataKeys[e.RowIndex].Value;
            StoreHelper.DeleteHotKeywords(hid);
            this.BindData();
        }

        private bool IsSame(string word, int categoryId)
        {
            foreach (DataRow row in StoreHelper.GetHotKeywords().Rows)
            {
                string str = row["Keywords"].ToString();
                if ((word == str) && (categoryId == Convert.ToInt32(row["CategoryId"].ToString())))
                {
                    return true;
                }
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmitHotkeyword.Click += new EventHandler(this.btnSubmitHotkeyword_Click);
            this.grdHotKeywords.RowDeleting += new GridViewDeleteEventHandler(this.grdHotKeywords_RowDeleting);
            this.grdHotKeywords.RowCommand += new GridViewCommandEventHandler(this.grdHotKeywords_RowCommand);
            this.btnEditHotkeyword.Click += new EventHandler(this.btnEditHotkeyword_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategory.DataBind();
                this.dropEditCategory.DataBind();
                this.BindData();
            }
        }
    }
}


namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Desig_Templete : HtmlTemplatedWebControl
    {
        protected string skintemp = "";
        private const string templetestr = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:Hidistro_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:Hidistro_designer.moveUp()\" class=\"up updisable\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:Hidistro_designer.moveDown()\" class=\"down downdisable\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"Hidistro_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"Hidistro_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><script>Hidistro_designer.Design_Page_Init();</script>";
        protected string tempurl = "";
        protected string viewname = "";

        protected override void AttachChildControls()
        {
            Literal literal = (Literal) this.FindControl("litPageName");
            Literal literal2 = (Literal) this.FindControl("litTempete");
            Literal literal3 = (Literal) this.FindControl("litaccount");
            Literal literal4 = (Literal) this.FindControl("litview");
            Literal literal5 = (Literal) this.FindControl("litDefault");
            if (!this.Page.IsPostBack)
            {
                if (literal != null)
                {
                    literal.Text = "<script>Hidistro_designer.CurrentPageName='" + this.skintemp + "'</script>";
                }
                if (literal2 != null)
                {
                    literal2.Text = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:Hidistro_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:Hidistro_designer.moveUp()\" class=\"up updisable\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:Hidistro_designer.moveDown()\" class=\"down downdisable\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"Hidistro_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"Hidistro_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><script>Hidistro_designer.Design_Page_Init();</script>";
                }
                if (literal3 != null)
                {
                    IUser contexUser = Users.GetContexUser();
                    if (contexUser != null)
                    {
                        literal3.Text = "<a>我的账号：" + contexUser.Username + "</a>";
                    }
                }
                if (literal5 != null)
                {
                    literal5.Text = "<a href=\"" + Globals.ApplicationPath + "/\">查看店铺</a>";
                }
                if (literal4 != null)
                {
                    string str = Globals.ApplicationPath + "/";
                    if (this.viewname != "")
                    {
                        str = Globals.GetSiteUrls().UrlData.FormatUrl(this.viewname);
                    }
                    literal4.Text = "<a href=\"" + str + "\" target=\"_blank\" class=\"button\">预览</a>";
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            IUser contexUser = Users.GetContexUser();
            if (!HiContext.Current.SiteSettings.IsDistributorSettings && (contexUser.UserRole != UserRole.SiteManager))
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("login.aspx"), true);
            }
            if (HiContext.Current.SiteSettings.IsDistributorSettings && (contexUser.UserRole != UserRole.Distributor))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "Shopadmin/DistributorLogin.aspx", true);
            }
            this.SetDesignSkinName();
            if ((this.SkinName == null) || (this.tempurl == ""))
            {
                base.GotoResourceNotFound();
            }
            base.OnInit(e);
        }

        protected void SetDesignSkinName()
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["skintemp"]))
            {
                base.GotoResourceNotFound();
            }
            this.skintemp = this.Page.Request.QueryString["skintemp"];
            switch (this.skintemp)
            {
                case "default":
                    this.SkinName = "Skin-Desig_Templete.html";
                    this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Default.html");
                    return;

                case "login":
                    this.SkinName = "Skin-Desig_login.html";
                    this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Login.html");
                    return;

                case "brand":
                    this.SkinName = "Skin-Desig_Brand.html";
                    this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Brand.html");
                    return;
            }
            this.SkinName = null;
        }
    }
}


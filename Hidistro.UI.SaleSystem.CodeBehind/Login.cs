namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Member;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    public class Login : HtmlTemplatedWebControl
    {
        private IButton btnLogin;
        private DropDownList ddlPlugins;
        private static string ReturnURL = string.Empty;
        private TextBox txtPassword;
        private TextBox txtUserName;

        protected override void AttachChildControls()
        {
            if (this.Context.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
                IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
                if (userCookie != null)
                {
                    userCookie.DeleteCookie(authCookie);
                }
                RoleHelper.SignOut(HiContext.Current.User.Username);
            }
            if (!string.IsNullOrEmpty(this.Page.Request["action"]) && (this.Page.Request["action"] == "Common_UserLogin"))
            {
                string str = this.UserLogin(this.Page.Request["username"], this.Page.Request["password"]);
                string str2 = string.IsNullOrEmpty(str) ? "Succes" : "Fail";
                this.Page.Response.Clear();
                this.Page.Response.ContentType = "application/json";
                this.Page.Response.Write("{\"Status\":\"" + str2 + "\",\"Msg\":\"" + str + "\"}");
                this.Page.Response.End();
            }
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtPassword = (TextBox) this.FindControl("txtPassword");
            this.btnLogin = ButtonManager.Create(this.FindControl("btnLogin"));
            this.ddlPlugins = (DropDownList) this.FindControl("ddlPlugins");
            if (this.ddlPlugins != null)
            {
                this.ddlPlugins.Items.Add(new ListItem("请选择登录方式", ""));
                IList<OpenIdSettingsInfo> configedItems = MemberProcessor.GetConfigedItems();
                if ((configedItems != null) && (configedItems.Count > 0))
                {
                    foreach (OpenIdSettingsInfo info in configedItems)
                    {
                        this.ddlPlugins.Items.Add(new ListItem(info.Name, info.OpenIdType));
                    }
                }
                this.ddlPlugins.SelectedIndexChanged += new EventHandler(this.ddlPlugins_SelectedIndexChanged);
            }
            if ((this.Page.Request.UrlReferrer != null) && !string.IsNullOrEmpty(this.Page.Request.UrlReferrer.OriginalString))
            {
                ReturnURL = this.Page.Request.UrlReferrer.OriginalString;
            }
            this.txtUserName.Focus();
            PageTitle.AddSiteNameTitle("用户登录", HiContext.Current.Context);
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                string pattern = @"[\u4e00-\u9fa5a-zA-Z0-9]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
                Regex regex = new Regex(pattern);
                if ((!regex.IsMatch(this.txtUserName.Text.Trim()) || (this.txtUserName.Text.Trim().Length < 2)) || (this.txtUserName.Text.Trim().Length > 20))
                {
                    this.ShowMessage("用户名不能为空，必须以汉字或是字母开头,且在2-20个字符之间", false);
                }
                else if (this.txtUserName.Text.Contains(","))
                {
                    this.ShowMessage("用户名不能包含逗号", false);
                }
                else
                {
                    string str2 = this.UserLogin(this.txtUserName.Text.Trim(), this.txtPassword.Text);
                    if (!string.IsNullOrEmpty(str2))
                    {
                        this.ShowMessage(str2, false);
                    }
                    else
                    {
                        string returnURL = this.Page.Request.QueryString["ReturnUrl"];
                        if (string.IsNullOrEmpty(returnURL))
                        {
                            returnURL = Globals.ApplicationPath + "/User/UserDefault.aspx";
                        }
                        else if (string.IsNullOrEmpty(ReturnURL))
                        {
                            returnURL = ReturnURL;
                        }
                        this.Page.Response.Redirect(returnURL);
                    }
                }
            }
        }

        private void ddlPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlPlugins.SelectedValue.Length > 0)
            {
                this.Page.Response.Redirect("OpenId/RedirectLogin.aspx?ot=" + this.ddlPlugins.SelectedValue);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Login.html";
            }
            base.OnInit(e);
        }

        private string UserLogin(string userName, string password)
        {
            string str = string.Empty;
            Member member = Users.GetUser(0, userName, false, true) as Member;
            if ((member == null) || member.IsAnonymous)
            {
                return "用户名或密码错误";
            }
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                if (member.ParentUserId.HasValue)
                {
                    if (member.ParentUserId.Value == HiContext.Current.SiteSettings.UserId)
                    {
                        goto Label_00B2;
                    }
                }
                return "您不是本站会员，请您进行注册";
            }
            if (member.ParentUserId.HasValue && (member.ParentUserId.Value != 0))
            {
                return "您不是本站会员，请您进行注册";
            }
        Label_00B2:
            member.Password = password;
            switch (MemberProcessor.ValidLogin(member))
            {
                case LoginUserStatus.AccountPending:
                    return "用户账号还没有通过审核";

                case LoginUserStatus.InvalidCredentials:
                    return "用户名或密码错误";

                case LoginUserStatus.Success:
                {
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
                    member.GetUserCookie().WriteCookie(authCookie, 30, false);
                    ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                    CookieShoppingProvider.Instance().ClearShoppingCart();
                    HiContext.Current.User = member;
                    if (shoppingCart != null)
                    {
                        ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                    }
                    member.OnLogin();
                    return str;
                }
            }
            return "未知错误";
        }
    }
}


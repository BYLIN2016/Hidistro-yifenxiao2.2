namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Member;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ProductReviews : HtmlTemplatedWebControl
    {
        private IButton btnRefer;
        private ProductDetailsLink productdetailLink;
        private int productId;
        private HtmlControl spReviewPsw;
        private HtmlControl spReviewReg;
        private HtmlControl spReviewUserName;
        private TextBox txtContent;
        private TextBox txtEmail;
        private HtmlInputText txtReviewCode;
        private HtmlInputText txtReviewPsw;
        private HtmlInputText txtReviewUserName;
        private TextBox txtUserName;
        private string verifyCodeKey = "VerifyCode";

        public ProductReviews()
        {
            if ((HiContext.Current.User.UserRole != UserRole.Member) && (HiContext.Current.User.UserRole != UserRole.Underling))
            {
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("login", new object[] { this.Page.Request.RawUrl }), true);
            }
        }

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.txtEmail = (TextBox) this.FindControl("txtEmail");
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtContent = (TextBox) this.FindControl("txtContent");
            this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
            this.spReviewUserName = (HtmlControl) this.FindControl("spReviewUserName");
            this.spReviewPsw = (HtmlControl) this.FindControl("spReviewPsw");
            this.spReviewReg = (HtmlControl) this.FindControl("spReviewReg");
            this.txtReviewUserName = (HtmlInputText) this.FindControl("txtReviewUserName");
            this.txtReviewPsw = (HtmlInputText) this.FindControl("txtReviewPsw");
            this.txtReviewCode = (HtmlInputText) this.FindControl("txtReviewCode");
            this.productdetailLink = (ProductDetailsLink) this.FindControl("ProductDetailsLink1");
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            if (!this.Page.IsPostBack)
            {
                PageTitle.AddSiteNameTitle("商品评论", HiContext.Current.Context);
                if ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling))
                {
                    this.txtUserName.Text = HiContext.Current.User.Username;
                    this.txtEmail.Text = HiContext.Current.User.Email;
                    this.txtReviewUserName.Value = string.Empty;
                    this.txtReviewPsw.Value = string.Empty;
                    this.spReviewUserName.Visible = false;
                    this.spReviewPsw.Visible = false;
                    this.spReviewReg.Visible = false;
                    this.btnRefer.Text = "评论";
                }
                else
                {
                    this.spReviewUserName.Visible = true;
                    this.spReviewPsw.Visible = true;
                    this.spReviewReg.Visible = true;
                    this.btnRefer.Text = "登录并评论";
                }
                this.txtReviewCode.Value = string.Empty;
                ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
                if (productSimpleInfo != null)
                {
                    this.productdetailLink.ProductId = this.productId;
                    this.productdetailLink.ProductName = productSimpleInfo.ProductName;
                }
            }
        }

        public void btnRefer_Click(object sender, EventArgs e)
        {
            if (this.ValidateConvert())
            {
                ProductReviewInfo target = new ProductReviewInfo();
                target.ReviewDate = DateTime.Now;
                target.ProductId = this.productId;
                target.UserId = HiContext.Current.User.UserId;
                target.UserName = this.txtUserName.Text;
                target.UserEmail = this.txtEmail.Text;
                target.ReviewText = this.txtContent.Text;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductReviewInfo>(target, new string[] { "Refer" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    this.ShowMessage(msg, false);
                }
                else if (((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling)) || this.userRegion(this.txtReviewUserName.Value, this.txtReviewPsw.Value))
                {
                    if (string.IsNullOrEmpty(this.txtReviewCode.Value))
                    {
                        this.ShowMessage("请输入验证码", false);
                    }
                    else if (!HiContext.Current.CheckVerifyCode(this.txtReviewCode.Value.Trim()))
                    {
                        this.ShowMessage("验证码不正确", false);
                    }
                    else
                    {
                        int buyNum = 0;
                        int reviewNum = 0;
                        ProductBrowser.LoadProductReview(this.productId, out buyNum, out reviewNum);
                        if (buyNum == 0)
                        {
                            this.ShowMessage("您没有购买此商品，因此不能进行评论", false);
                        }
                        else if (reviewNum >= buyNum)
                        {
                            this.ShowMessage("您已经对此商品进行了评论，请再次购买后方能再进行评论", false);
                        }
                        else if (ProductProcessor.InsertProductReview(target))
                        {
                            this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "评论成功", Globals.GetSiteUrls().UrlData.FormatUrl("productReviews", new object[] { this.productId })));
                        }
                        else
                        {
                            this.ShowMessage("评论失败，请重试", false);
                        }
                    }
                }
            }
        }

        private bool CheckVerifyCode(string verifyCode)
        {
            if (HttpContext.Current.Request.Cookies[this.verifyCodeKey] == null)
            {
                return false;
            }
            return (string.Compare(HiCryptographer.Decrypt(HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ProductReviews.html";
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["isCallback"]) && (HttpContext.Current.Request["isCallback"] == "true"))
            {
                string verifyCode = HttpContext.Current.Request["code"];
                string str2 = "";
                if (!this.CheckVerifyCode(verifyCode))
                {
                    str2 = "0";
                }
                else
                {
                    str2 = "1";
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write("{ ");
                HttpContext.Current.Response.Write(string.Format("\"flag\":\"{0}\"", str2));
                HttpContext.Current.Response.Write("}");
                HttpContext.Current.Response.End();
            }
            base.OnInit(e);
        }

        private bool userRegion(string username, string password)
        {
            HiContext current = HiContext.Current;
            Member member = Users.GetUser(0, username, false, true) as Member;
            if ((member == null) || member.IsAnonymous)
            {
                this.ShowMessage("用户名或密码错误", false);
                return false;
            }
            member.Password = password;
            switch (MemberProcessor.ValidLogin(member))
            {
                case LoginUserStatus.Success:
                {
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
                    member.GetUserCookie().WriteCookie(authCookie, 30, false);
                    current.User = member;
                    return true;
                }
                case LoginUserStatus.AccountPending:
                    this.ShowMessage("用户账号还没有通过审核", false);
                    return false;

                case LoginUserStatus.InvalidCredentials:
                    this.ShowMessage("用户名或密码错误", false);
                    return false;
            }
            this.ShowMessage("未知错误", false);
            return false;
        }

        private bool ValidateConvert()
        {
            string str = string.Empty;
            if (((HiContext.Current.User.UserRole != UserRole.Member) && (HiContext.Current.User.UserRole != UserRole.Underling)) && (string.IsNullOrEmpty(this.txtReviewUserName.Value) || string.IsNullOrEmpty(this.txtReviewPsw.Value)))
            {
                str = str + Formatter.FormatErrorMessage("请填写用户名和密码");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMessage(str, false);
                return false;
            }
            return true;
        }
    }
}


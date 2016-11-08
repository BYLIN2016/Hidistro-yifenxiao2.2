namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Messages;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class DistributorsRegister : HtmlTemplatedWebControl
    {
        private Button btnOK;
        private RegionSelector dropRegion;
        private TextBox txtAddress;
        private TextBox txtCellPhone;
        private TextBox txtCompanyName;
        private TextBox txtEmail;
        private TextBox txtMSN;
        private TextBox txtPassword;
        private TextBox txtPasswordAnswer;
        private TextBox txtPasswordCompare;
        private TextBox txtPasswordQuestion;
        private TextBox txtQQ;
        private TextBox txtRealName;
        private TextBox txtTelPhone;
        private TextBox txtTransactionPassword;
        private TextBox txtTransactionPasswordCompare;
        private TextBox txtUserName;
        private TextBox txtWangwang;
        private TextBox txtZipcode;

        protected override void AttachChildControls()
        {
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtEmail = (TextBox) this.FindControl("txtEmail");
            this.txtPassword = (TextBox) this.FindControl("txtPassword");
            this.txtPasswordCompare = (TextBox) this.FindControl("txtPasswordCompare");
            this.txtTransactionPassword = (TextBox) this.FindControl("txtTransactionPassword");
            this.txtTransactionPasswordCompare = (TextBox) this.FindControl("txtTransactionPasswordCompare");
            this.txtRealName = (TextBox) this.FindControl("txtRealName");
            this.txtCompanyName = (TextBox) this.FindControl("txtCompanyName");
            this.dropRegion = (RegionSelector) this.FindControl("dropRegion");
            this.txtAddress = (TextBox) this.FindControl("txtAddress");
            this.txtZipcode = (TextBox) this.FindControl("txtZipcode");
            this.txtQQ = (TextBox) this.FindControl("txtQQ");
            this.txtWangwang = (TextBox) this.FindControl("txtWangwang");
            this.txtMSN = (TextBox) this.FindControl("txtMSN");
            this.txtTelPhone = (TextBox) this.FindControl("txtTelPhone");
            this.txtCellPhone = (TextBox) this.FindControl("txtCellPhone");
            this.txtPasswordQuestion = (TextBox) this.FindControl("txtPasswordQuestion");
            this.txtPasswordAnswer = (TextBox) this.FindControl("txtPasswordAnswer");
            this.btnOK = (Button) this.FindControl("btnOK");
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.ValidationInput())
            {
                int? selectedRegionId = this.dropRegion.GetSelectedRegionId();
                HiMembershipUser membershipUser = new HiMembershipUser(false, UserRole.Distributor);
                Distributor distributor = new Distributor(membershipUser);
                distributor.IsApproved = false;
                distributor.Username = this.txtUserName.Text;
                distributor.Email = this.txtEmail.Text;
                distributor.Password = this.txtPasswordCompare.Text;
                if (!string.IsNullOrEmpty(this.txtTransactionPasswordCompare.Text))
                {
                    distributor.TradePassword = this.txtTransactionPasswordCompare.Text;
                }
                else
                {
                    distributor.TradePassword = distributor.Password;
                }
                distributor.RealName = this.txtRealName.Text;
                distributor.CompanyName = this.txtCompanyName.Text;
                if (selectedRegionId.HasValue)
                {
                    distributor.RegionId = selectedRegionId.Value;
                    distributor.TopRegionId = RegionHelper.GetTopRegionId(distributor.RegionId);
                }
                distributor.Address = this.txtAddress.Text;
                distributor.Zipcode = this.txtZipcode.Text;
                distributor.QQ = this.txtQQ.Text;
                distributor.Wangwang = this.txtWangwang.Text;
                distributor.MSN = this.txtMSN.Text;
                distributor.TelPhone = this.txtTelPhone.Text;
                distributor.CellPhone = this.txtCellPhone.Text;
                distributor.Remark = string.Empty;
                if (this.ValidationDistributorRequest(distributor))
                {
                    switch (SubsiteStoreHelper.CreateDistributor(distributor))
                    {
                        case CreateUserStatus.UnknownFailure:
                            this.ShowMessage("未知错误", false);
                            return;

                        case CreateUserStatus.Created:
                            distributor.ChangePasswordQuestionAndAnswer(null, this.txtPasswordQuestion.Text, this.txtPasswordAnswer.Text);
                            Messenger.UserRegister(distributor, this.txtPasswordCompare.Text);
                            distributor.OnRegister(new UserEventArgs(distributor.Username, this.txtPasswordCompare.Text, null));
                            this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/DistributorsRegisterComplete.aspx");
                            return;

                        case CreateUserStatus.DuplicateUsername:
                            this.ShowMessage("您输入的用户名已经被注册使用", false);
                            return;

                        case CreateUserStatus.DuplicateEmailAddress:
                            this.ShowMessage("您输入的电子邮件地址已经被注册使用", false);
                            return;

                        case CreateUserStatus.InvalidFirstCharacter:
                        case CreateUserStatus.Updated:
                        case CreateUserStatus.Deleted:
                        case CreateUserStatus.InvalidQuestionAnswer:
                            return;

                        case CreateUserStatus.DisallowedUsername:
                            this.ShowMessage("用户名被禁止注册", false);
                            return;

                        case CreateUserStatus.InvalidPassword:
                            this.ShowMessage("无效的密码", false);
                            return;

                        case CreateUserStatus.InvalidEmail:
                            this.ShowMessage("无效的电子邮件地址", false);
                            return;
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorsRegister.html";
            }
            base.OnInit(e);
        }

        private bool ValidationDistributorRequest(Distributor distributor)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Distributor>(distributor, new string[] { "ValDistributor" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMessage(msg, false);
            }
            return results.IsValid;
        }

        private bool ValidationInput()
        {
            string str = string.Empty;
            if (this.txtUserName.Text.Trim().Length <= 1)
            {
                str = str + "请输入至少两位长度的字符";
            }
            if (string.Compare(this.txtPassword.Text, this.txtPasswordCompare.Text) != 0)
            {
                str = str + "请确定两次输入的登录密码相同";
            }
            if (string.IsNullOrEmpty(this.txtTransactionPassword.Text.Trim()))
            {
                str = str + "<br/>交易密码不允许为空！";
            }
            if (string.IsNullOrEmpty(this.txtTransactionPasswordCompare.Text.Trim()))
            {
                str = str + "<br/>重复交易密码不允许为空！";
            }
            if (!string.IsNullOrEmpty(this.txtTransactionPassword.Text) && (string.Compare(this.txtTransactionPassword.Text, this.txtTransactionPasswordCompare.Text) != 0))
            {
                str = str + "<br/>请确定两次输入的交易密码相同";
            }
            if ((string.IsNullOrEmpty(this.txtQQ.Text) && string.IsNullOrEmpty(this.txtWangwang.Text)) && string.IsNullOrEmpty(this.txtMSN.Text))
            {
                str = str + "<br/>QQ,旺旺,MSN,三者必填其一";
            }
            if (string.IsNullOrEmpty(this.txtTelPhone.Text) && string.IsNullOrEmpty(this.txtCellPhone.Text))
            {
                str = str + "<br/>固定电话和手机,二者必填其一";
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


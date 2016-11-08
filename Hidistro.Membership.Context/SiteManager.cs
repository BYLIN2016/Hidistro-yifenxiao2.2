namespace Hidistro.Membership.Context
{
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Web.Security;

    [HasSelfValidation]
    public class SiteManager : IUser
    {
        
        private HiMembershipUser _MembershipUser;

        public SiteManager()
        {
            this.MembershipUser = new HiMembershipUser(false, Hidistro.Membership.Core.Enums.UserRole.SiteManager);
        }

        public SiteManager(HiMembershipUser membershipUser)
        {
            this.MembershipUser = membershipUser;
        }

        public bool ChangePassword(string newPassword)
        {
            if (HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.SiteManager)
            {
                SiteManager user = HiContext.Current.User as SiteManager;
                if (((user != null) && (user.UserId != this.UserId)) && user.IsAdministrator)
                {
                    string oldPassword = this.MembershipUser.Membership.ResetPassword();
                    return this.MembershipUser.Membership.ChangePassword(oldPassword, newPassword);
                }
            }
            return false;
        }

        public bool ChangePassword(string password, string newPassword)
        {
            return this.MembershipUser.Membership.ChangePassword(password, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            return this.MembershipUser.ChangePasswordQuestionAndAnswer(newQuestion, newAnswer);
        }

        public bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            return this.MembershipUser.ChangePasswordQuestionAndAnswer(oldAnswer, newQuestion, newAnswer);
        }

        public bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            return this.MembershipUser.ChangePasswordWithAnswer(answer, newPassword);
        }

        public bool ChangeTradePassword(string newPassword)
        {
            return true;
        }

        public bool ChangeTradePassword(string oldPassword, string newPassword)
        {
            return true;
        }

        [SelfValidation(Ruleset="ValManagerEmail")]
        public void CheckManagerEmail(ValidationResults results)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            if (string.IsNullOrEmpty(this.Email) || (this.Email.Length > 0x100))
            {
                results.AddResult(new ValidationResult("电子邮件的长度必须小于256个字符", this, "", "", null));
            }
            else if (!Regex.IsMatch(this.Email, config.EmailRegex))
            {
                results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
            }
        }

        [SelfValidation(Ruleset="ValManagerName")]
        public void CheckManagerName(ValidationResults results)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            if ((string.IsNullOrEmpty(this.Username) || (this.Username.Length > config.UsernameMaxLength)) || (this.Username.Length < config.UsernameMinLength))
            {
                results.AddResult(new ValidationResult(string.Format("用户名不能为空，长度限制在{0}-{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength), this, "", "", null));
            }
            else if (string.Compare(this.Username, "anonymous", true) == 0)
            {
                results.AddResult(new ValidationResult("不能使用anonymous作为用户名", this, "", "", null));
            }
            else if (!Regex.IsMatch(this.Username, config.UsernameRegex))
            {
                results.AddResult(new ValidationResult("用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头", this, "", "", null));
            }
        }

        [SelfValidation(Ruleset="ValManagerPassword")]
        public void CheckManagerPassword(ValidationResults results)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            if ((string.IsNullOrEmpty(this.Password) || (this.Password.Length < Membership.Provider.MinRequiredPasswordLength)) || (this.Password.Length > config.PasswordMaxLength))
            {
                results.AddResult(new ValidationResult(string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength), this, "", "", null));
            }
        }

        public IUserCookie GetUserCookie()
        {
            return new UserCookie(this);
        }

        public bool HasPrivilege(int privilegeCode)
        {
            IList<int> userPrivileges = RoleHelper.GetUserPrivileges(this.Username);
            if ((userPrivileges == null) || (userPrivileges.Count == 0))
            {
                return false;
            }
            return userPrivileges.Contains(privilegeCode);
        }

        public bool IsInRole(string roleName)
        {
            return Roles.IsUserInRole(this.Username, roleName);
        }

        public string ResetPassword(string answer)
        {
            return this.MembershipUser.ResetPassword(answer);
        }

        public bool ValidatePasswordAnswer(string answer)
        {
            return this.MembershipUser.ValidatePasswordAnswer(answer);
        }

        public DateTime? BirthDate
        {
            get
            {
                return this.MembershipUser.BirthDate;
            }
            set
            {
                this.MembershipUser.BirthDate = value;
            }
        }

        public string Comment
        {
            get
            {
                return this.MembershipUser.Comment;
            }
            set
            {
                this.MembershipUser.Comment = value;
            }
        }

        public DateTime CreateDate
        {
            get
            {
                return this.MembershipUser.CreateDate;
            }
        }

        public string Email
        {
            get
            {
                return this.MembershipUser.Email;
            }
            set
            {
                this.MembershipUser.Email = value;
            }
        }

        public Hidistro.Membership.Core.Enums.Gender Gender
        {
            get
            {
                return this.MembershipUser.Gender;
            }
            set
            {
                this.MembershipUser.Gender = value;
            }
        }

        public bool IsAdministrator
        {
            get
            {
                bool flag = false;
                try
                {
                    if (this.IsAnonymous)
                    {
                        return false;
                    }
                    RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;
                    flag = this.IsInRole(rolesConfiguration.SystemAdministrator);
                }
                catch
                {
                }
                return flag;
            }
        }

        public bool IsAnonymous
        {
            get
            {
                return this.MembershipUser.IsAnonymous;
            }
        }

        public bool IsApproved
        {
            get
            {
                return this.MembershipUser.IsApproved;
            }
            set
            {
                this.MembershipUser.IsApproved = value;
            }
        }

        public bool IsLockedOut
        {
            get
            {
                return this.MembershipUser.IsLockedOut;
            }
        }

        public DateTime LastActivityDate
        {
            get
            {
                return this.MembershipUser.LastActivityDate;
            }
            set
            {
                this.MembershipUser.LastActivityDate = value;
            }
        }

        public DateTime LastLockoutDate
        {
            get
            {
                return this.MembershipUser.LastLockoutDate;
            }
        }

        public DateTime LastLoginDate
        {
            get
            {
                return this.MembershipUser.LastLoginDate;
            }
        }

        public DateTime LastPasswordChangedDate
        {
            get
            {
                return this.MembershipUser.LastPasswordChangedDate;
            }
        }

        public HiMembershipUser MembershipUser
        {
            
            get
            {
                return _MembershipUser;
            }
            
            private set
            {
                _MembershipUser = value;
            }
        }

        public string MobilePIN
        {
            get
            {
                return this.MembershipUser.MobilePIN;
            }
            set
            {
                this.MembershipUser.MobilePIN = value;
            }
        }

        public string Password
        {
            get
            {
                return this.MembershipUser.Password;
            }
            set
            {
                this.MembershipUser.Password = value;
            }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return this.MembershipUser.PasswordFormat;
            }
            set
            {
                this.MembershipUser.PasswordFormat = value;
            }
        }

        public string PasswordQuestion
        {
            get
            {
                return this.MembershipUser.PasswordQuestion;
            }
        }

        public string TradePassword
        {
            get
            {
                return this.MembershipUser.TradePassword;
            }
            set
            {
                this.MembershipUser.TradePassword = value;
            }
        }

        public MembershipPasswordFormat TradePasswordFormat
        {
            get
            {
                return this.MembershipUser.TradePasswordFormat;
            }
            set
            {
                this.MembershipUser.TradePasswordFormat = value;
            }
        }

        public int UserId
        {
            get
            {
                return this.MembershipUser.UserId;
            }
            set
            {
                this.MembershipUser.UserId = value;
            }
        }

        public string Username
        {
            get
            {
                return this.MembershipUser.Username;
            }
            set
            {
                this.MembershipUser.Username = value;
            }
        }

        public Hidistro.Membership.Core.Enums.UserRole UserRole
        {
            get
            {
                return this.MembershipUser.UserRole;
            }
        }
    }
}


namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.Security;

    [HasSelfValidation]
    public class Distributor : IUser
    {
        
        private string _Address;
        
        private decimal _Balance;
        
        private string _CellPhone;
        
        private string _CompanyName;
        
        private decimal _Expenditure;
        
        private int _GradeId;
        
        private bool _IsCreate;
        
        private int _MemberCount;
        
        private HiMembershipUser _MembershipUser;
        
        private string _MSN;
        
        private int _PurchaseOrder;
        
        private string _QQ;
        
        private string _RealName;
        
        private int _RegionId;
        
        private string _Remark;
        
        private decimal _RequestBalance;
        
        private string _TelPhone;
        
        private int _TopRegionId;
        
        private string _Wangwang;
        
        private string _Zipcode;
        private static EventHandler<UserEventArgs> DealPasswordChanged;
        private static EventHandler<UserEventArgs> FindPassword;
        private static EventHandler<EventArgs> Login;
        private static EventHandler<UserEventArgs> Logout;
        private static EventHandler<UserEventArgs> PasswordChanged;
        private static EventHandler<UserEventArgs> Register;

        public static event EventHandler<UserEventArgs> _DealPasswordChanged
        {
            add
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> dealPasswordChanged = DealPasswordChanged;
                do
                {
                    handler2 = dealPasswordChanged;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Combine(handler2, value);
                    dealPasswordChanged = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref DealPasswordChanged, handler3, handler2);
                }
                while (dealPasswordChanged != handler2);
            }
            remove
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> dealPasswordChanged = DealPasswordChanged;
                do
                {
                    handler2 = dealPasswordChanged;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Remove(handler2, value);
                    dealPasswordChanged = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref DealPasswordChanged, handler3, handler2);
                }
                while (dealPasswordChanged != handler2);
            }
        }

        public static event EventHandler<UserEventArgs> _FindPassword
        {
            add
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> findPassword = FindPassword;
                do
                {
                    handler2 = findPassword;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Combine(handler2, value);
                    findPassword = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref FindPassword, handler3, handler2);
                }
                while (findPassword != handler2);
            }
            remove
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> findPassword = FindPassword;
                do
                {
                    handler2 = findPassword;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Remove(handler2, value);
                    findPassword = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref FindPassword, handler3, handler2);
                }
                while (findPassword != handler2);
            }
        }

        public static event EventHandler<EventArgs> _Login
        {
            add
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> login = Login;
                do
                {
                    handler2 = login;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Combine(handler2, value);
                    login = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Login, handler3, handler2);
                }
                while (login != handler2);
            }
            remove
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> login = Login;
                do
                {
                    handler2 = login;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Remove(handler2, value);
                    login = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Login, handler3, handler2);
                }
                while (login != handler2);
            }
        }

        public static event EventHandler<UserEventArgs> _Logout
        {
            add
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> logout = Logout;
                do
                {
                    handler2 = logout;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Combine(handler2, value);
                    logout = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref Logout, handler3, handler2);
                }
                while (logout != handler2);
            }
            remove
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> logout = Logout;
                do
                {
                    handler2 = logout;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Remove(handler2, value);
                    logout = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref Logout, handler3, handler2);
                }
                while (logout != handler2);
            }
        }

        public static event EventHandler<UserEventArgs> _PasswordChanged
        {
            add
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> passwordChanged = PasswordChanged;
                do
                {
                    handler2 = passwordChanged;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Combine(handler2, value);
                    passwordChanged = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref PasswordChanged, handler3, handler2);
                }
                while (passwordChanged != handler2);
            }
            remove
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> passwordChanged = PasswordChanged;
                do
                {
                    handler2 = passwordChanged;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Remove(handler2, value);
                    passwordChanged = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref PasswordChanged, handler3, handler2);
                }
                while (passwordChanged != handler2);
            }
        }

        public static event EventHandler<UserEventArgs> _Register
        {
            add
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> register = Register;
                do
                {
                    handler2 = register;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Combine(handler2, value);
                    register = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref Register, handler3, handler2);
                }
                while (register != handler2);
            }
            remove
            {
                EventHandler<UserEventArgs> handler2;
                EventHandler<UserEventArgs> register = Register;
                do
                {
                    handler2 = register;
                    EventHandler<UserEventArgs> handler3 = (EventHandler<UserEventArgs>) Delegate.Remove(handler2, value);
                    register = Interlocked.CompareExchange<EventHandler<UserEventArgs>>(ref Register, handler3, handler2);
                }
                while (register != handler2);
            }
        }

        public Distributor()
        {
            this.MembershipUser = new HiMembershipUser(false, Hidistro.Membership.Core.Enums.UserRole.Distributor);
        }

        public Distributor(HiMembershipUser membershipUser)
        {
            this.MembershipUser = membershipUser;
        }

        public bool ChangePassword(string newPassword)
        {
            if (HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.SiteManager)
            {
                string password = this.MembershipUser.Membership.ResetPassword();
                if (this.MembershipUser.ChangePassword(password, newPassword))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return this.MembershipUser.ChangePassword(oldPassword, newPassword);
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

        public bool ChangePasswordWithoutAnswer(string newPassword)
        {
            string password = this.MembershipUser.Membership.ResetPassword();
            return this.MembershipUser.ChangePassword(password, newPassword);
        }

        public bool ChangeTradePassword(string newPassword)
        {
            return DistributorFactory.Instance().ChangeTradePassword(this.Username, newPassword);
        }

        public bool ChangeTradePassword(string oldPassword, string newPassword)
        {
            return DistributorFactory.Instance().ChangeTradePassword(this.Username, oldPassword, newPassword);
        }

        [SelfValidation(Ruleset="ValDistributor")]
        public void CheckDistributor(ValidationResults results)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            if ((string.IsNullOrEmpty(this.Username) || (this.Username.Length > config.UsernameMaxLength)) || (this.Username.Length < config.UsernameMinLength))
            {
                results.AddResult(new ValidationResult(string.Format("用户名不能为空，长度必须在{0}-{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength), this, "", "", null));
            }
            else if (!Regex.IsMatch(this.Username, config.UsernameRegex))
            {
                results.AddResult(new ValidationResult("用户名的格式错误", this, "", "", null));
            }
            if (string.IsNullOrEmpty(this.Email) || (this.Email.Length > 0x100))
            {
                results.AddResult(new ValidationResult("电子邮件不能为空，长度必须小于256个字符", this, "", "", null));
            }
            else if (!Regex.IsMatch(this.Email, config.EmailRegex))
            {
                results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
            }
            if (this.IsCreate)
            {
                if ((string.IsNullOrEmpty(this.Password) || (this.Password.Length > config.PasswordMaxLength)) || (this.Password.Length < 6))
                {
                    results.AddResult(new ValidationResult(string.Format("密码不能为空，长度必须在{0}-{1}个字符之间", 6, config.PasswordMaxLength), this, "", "", null));
                }
                if ((string.IsNullOrEmpty(this.TradePassword) || (this.TradePassword.Length > config.PasswordMaxLength)) || (this.TradePassword.Length < 6))
                {
                    results.AddResult(new ValidationResult(string.Format("交易密码不能为空，长度必须在{0}-{1}个字符之间", 6, config.PasswordMaxLength), this, "", "", null));
                }
            }
            if (!(string.IsNullOrEmpty(this.QQ) || (((this.QQ.Length <= 20) && (this.QQ.Length >= 3)) && Regex.IsMatch(this.QQ, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("QQ号长度限制在3-20个字符之间，只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.Zipcode) || (((this.Zipcode.Length <= 10) && (this.Zipcode.Length >= 3)) && Regex.IsMatch(this.Zipcode, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("邮编长度限制在3-10个字符之间，只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.Wangwang) || ((this.Wangwang.Length <= 20) && (this.Wangwang.Length >= 3))))
            {
                results.AddResult(new ValidationResult("旺旺长度限制在3-20个字符之间", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.MSN) || (((this.MSN.Length <= 0x100) && (this.MSN.Length >= 1)) && Regex.IsMatch(this.MSN, config.EmailRegex))))
            {
                results.AddResult(new ValidationResult("请输入正确MSN帐号，长度在1-256个字符以内", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.CellPhone) || (((this.CellPhone.Length <= 20) && (this.CellPhone.Length >= 3)) && Regex.IsMatch(this.CellPhone, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("手机号码长度限制在3-20个字符之间,只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.TelPhone) || (((this.TelPhone.Length <= 20) && (this.TelPhone.Length >= 3)) && Regex.IsMatch(this.TelPhone, "^[0-9-]*$"))))
            {
                results.AddResult(new ValidationResult("电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”", this, "", "", null));
            }
        }

        public IUserCookie GetUserCookie()
        {
            return new UserCookie(this);
        }

        public bool IsInRole(string roleName)
        {
            return roleName.Equals(HiContext.Current.Config.RolesConfiguration.Distributor);
        }

        public void OnDealPasswordChanged(UserEventArgs args)
        {
            if (DealPasswordChanged != null)
            {
                DealPasswordChanged(this, args);
            }
        }

        public static void OnDealPasswordChanged(Member member, UserEventArgs args)
        {
            if (DealPasswordChanged != null)
            {
                DealPasswordChanged(member, args);
            }
        }

        public void OnFindPassword(UserEventArgs args)
        {
            if (FindPassword != null)
            {
                FindPassword(this, args);
            }
        }

        public static void OnFindPassword(Member member, UserEventArgs args)
        {
            if (FindPassword != null)
            {
                FindPassword(member, args);
            }
        }

        public void OnLogin()
        {
            if (Login != null)
            {
                Login(this, new EventArgs());
            }
        }

        public static void OnLogin(Member member)
        {
            if (Login != null)
            {
                Login(member, new EventArgs());
            }
        }

        public static void OnLogout(UserEventArgs args)
        {
            if (Logout != null)
            {
                Logout(null, args);
            }
        }

        public void OnPasswordChanged(UserEventArgs args)
        {
            if (PasswordChanged != null)
            {
                PasswordChanged(this, args);
            }
        }

        public static void OnPasswordChanged(Member member, UserEventArgs args)
        {
            if (PasswordChanged != null)
            {
                PasswordChanged(member, args);
            }
        }

        public void OnRegister(UserEventArgs args)
        {
            if (Register != null)
            {
                Register(this, args);
            }
        }

        public static void OnRegister(Member member, UserEventArgs args)
        {
            if (Register != null)
            {
                Register(member, args);
            }
        }

        public string ResetPassword(string answer)
        {
            return this.MembershipUser.ResetPassword(answer);
        }

        public bool ValidatePasswordAnswer(string answer)
        {
            return this.MembershipUser.ValidatePasswordAnswer(answer);
        }

        [StringLengthValidator(0, 100, Ruleset="ValDistributor", MessageTemplate="详细地址必须控制在100个字符以内"), HtmlCoding]
        public string Address
        {
            
            get
            {
                return _Address;
            }
            
            set
            {
                _Address = value;
            }
        }

        public decimal Balance
        {
            
            get
            {
                return _Balance;
            }
            
            set
            {
                _Balance = value;
            }
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

        public string CellPhone
        {
            
            get
            {
                return _CellPhone;
            }
            
            set
            {
                _CellPhone = value;
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

        [StringLengthValidator(0, 60, Ruleset="ValDistributor", MessageTemplate="公司名称必须控制在60个字符以内"), HtmlCoding]
        public string CompanyName
        {
            
            get
            {
                return _CompanyName;
            }
            
            set
            {
                _CompanyName = value;
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

        public decimal Expenditure
        {
            
            get
            {
                return _Expenditure;
            }
            
            set
            {
                _Expenditure = value;
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

        public int GradeId
        {
            
            get
            {
                return _GradeId;
            }
            
            set
            {
                _GradeId = value;
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

        public bool IsCreate
        {
            
            get
            {
                return _IsCreate;
            }
            
            set
            {
                _IsCreate = value;
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

        public int MemberCount
        {
            
            get
            {
                return _MemberCount;
            }
            
            set
            {
                _MemberCount = value;
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

        public string MSN
        {
            
            get
            {
                return _MSN;
            }
            
            set
            {
                _MSN = value;
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

        public int PurchaseOrder
        {
            
            get
            {
                return _PurchaseOrder;
            }
            
            set
            {
                _PurchaseOrder = value;
            }
        }

        public string QQ
        {
            
            get
            {
                return _QQ;
            }
            
            set
            {
                _QQ = value;
            }
        }

        [StringLengthValidator(0, 20, Ruleset="ValDistributor", MessageTemplate="真实姓名必须控制在20个字符以内")]
        public string RealName
        {
            
            get
            {
                return _RealName;
            }
            
            set
            {
                _RealName = value;
            }
        }

        public int RegionId
        {
            
            get
            {
                return _RegionId;
            }
            
            set
            {
                _RegionId = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValDistributor", MessageTemplate="合作备忘录必须控制在300个字符以内")]
        public string Remark
        {
            
            get
            {
                return _Remark;
            }
            
            set
            {
                _Remark = value;
            }
        }

        public decimal RequestBalance
        {
            
            get
            {
                return _RequestBalance;
            }
            
            set
            {
                _RequestBalance = value;
            }
        }

        public string TelPhone
        {
            
            get
            {
                return _TelPhone;
            }
            
            set
            {
                _TelPhone = value;
            }
        }

        public int TopRegionId
        {
            
            get
            {
                return _TopRegionId;
            }
            
            set
            {
                _TopRegionId = value;
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

        public string Wangwang
        {
            
            get
            {
                return _Wangwang;
            }
            
            set
            {
                _Wangwang = value;
            }
        }

        public string Zipcode
        {
            
            get
            {
                return _Zipcode;
            }
            
            set
            {
                _Zipcode = value;
            }
        }
    }
}


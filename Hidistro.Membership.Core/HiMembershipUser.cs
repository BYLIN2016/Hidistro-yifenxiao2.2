namespace Hidistro.Membership.Core
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.Security;

    public class HiMembershipUser
    {
        
        private DateTime? _BirthDate;
        
        private DateTime _CreateDate;
        
        private Hidistro.Membership.Core.Enums.Gender _Gender;
        
        private bool _IsAnonymous;
        
        private bool _IsLockedOut;
        
        private bool _IsOpenBalance;
        
        private DateTime _LastLockoutDate;
        
        private DateTime _LastPasswordChangedDate;
        
        private MembershipUser _Membership;
        
        private string _MobilePIN;
        
        private string _Password;
        
        private MembershipPasswordFormat _PasswordFormat;
        
        private string _PasswordQuestion;
        
        private string _TradePassword;
        
        private MembershipPasswordFormat _TradePasswordFormat;
        
        private int _UserId;
        
        private string _Username;
        
        private Hidistro.Membership.Core.Enums.UserRole _UserRole;
        private string comment;
        private string email;
        private bool isApproved;
        private DateTime lastActivityDate;
        private DateTime lastLoginDate;

        public HiMembershipUser(bool isAnonymous, Hidistro.Membership.Core.Enums.UserRole userRole)
        {
            if (isAnonymous && (userRole != Hidistro.Membership.Core.Enums.UserRole.Anonymous))
            {
                throw new Exception(string.Format("Current user is Anonymous, But the user role is '{0}'", userRole.ToString()));
            }
            this.UserRole = userRole;
            this.IsAnonymous = userRole == Hidistro.Membership.Core.Enums.UserRole.Anonymous;
        }

        public HiMembershipUser(bool isAnonymous, Hidistro.Membership.Core.Enums.UserRole userRole, MembershipUser mu) : this(isAnonymous, userRole)
        {
            this.RefreshMembershipUser(mu);
        }

        public virtual bool ChangePassword(string password, string newPassword)
        {
            return this.Membership.ChangePassword(password, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
            {
                return false;
            }
            if ((newQuestion.Length > 0x100) || (newAnswer.Length > 0x80))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(this.PasswordQuestion))
            {
                return false;
            }
            return MemberUserProvider.Instance().ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer);
        }

        public virtual bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
            {
                return false;
            }
            if ((newQuestion.Length > 0x100) || (newAnswer.Length > 0x80))
            {
                return false;
            }
            return (this.ValidatePasswordAnswer(oldAnswer) && MemberUserProvider.Instance().ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer));
        }

        public virtual bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            try
            {
                string str = this.ResetPassword(answer);
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
                return this.ChangePassword(str, newPassword);
            }
            catch
            {
                return false;
            }
        }

        public void RefreshMembershipUser(MembershipUser mu)
        {
            if (mu == null)
            {
                throw new Exception("A null MembershipUser is not valid to instantiate a new User");
            }
            this.Membership = mu;
            this.Username = mu.UserName;
            this.UserId = (int) mu.ProviderUserKey;
            this.Comment = mu.Comment;
            this.LastLockoutDate = mu.LastLockoutDate;
            this.LastPasswordChangedDate = mu.LastPasswordChangedDate;
            this.LastLoginDate = mu.LastLoginDate;
            this.CreateDate = mu.CreationDate;
            this.IsLockedOut = mu.IsLockedOut;
            this.IsApproved = mu.IsApproved;
            this.PasswordQuestion = mu.PasswordQuestion;
            this.Email = mu.Email;
            this.LastActivityDate = mu.LastActivityDate;
        }

        public virtual string ResetPassword(string answer)
        {
            try
            {
                if (this.ValidatePasswordAnswer(answer))
                {
                    return this.Membership.ResetPassword();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public virtual bool ValidatePasswordAnswer(string answer)
        {
            return MemberUserProvider.Instance().ValidatePasswordAnswer(this.Username, answer);
        }

        public DateTime? BirthDate
        {
            
            get
            {
                return _BirthDate;
            }
            
            set
            {
                _BirthDate = value;
            }
        }

        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
                if (this.Membership != null)
                {
                    this.Membership.Comment = value;
                }
            }
        }

        public DateTime CreateDate
        {
            
            get
            {
                return _CreateDate;
            }
            
            private set
            {
                _CreateDate = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
                if (this.Membership != null)
                {
                    this.Membership.Email = value;
                }
            }
        }

        public Hidistro.Membership.Core.Enums.Gender Gender
        {
            
            get
            {
                return _Gender;
            }
            
            set
            {
                _Gender = value;
            }
        }

        public bool IsAnonymous
        {
            
            get
            {
                return _IsAnonymous;
            }
            
            private set
            {
                _IsAnonymous = value;
            }
        }

        public bool IsApproved
        {
            get
            {
                return this.isApproved;
            }
            set
            {
                this.isApproved = value;
                if (this.Membership != null)
                {
                    this.Membership.IsApproved = value;
                }
            }
        }

        public bool IsLockedOut
        {
            
            get
            {
                return _IsLockedOut;
            }
            
            private set
            {
                _IsLockedOut = value;
            }
        }

        public bool IsOpenBalance
        {
            
            get
            {
                return _IsOpenBalance;
            }
            
            set
            {
                _IsOpenBalance = value;
            }
        }

        public DateTime LastActivityDate
        {
            get
            {
                return this.lastActivityDate;
            }
            set
            {
                this.lastActivityDate = value;
                if (this.Membership != null)
                {
                    this.Membership.LastActivityDate = value;
                }
            }
        }

        public DateTime LastLockoutDate
        {
            
            get
            {
                return _LastLockoutDate;
            }
            
            private set
            {
                _LastLockoutDate = value;
            }
        }

        public DateTime LastLoginDate
        {
            get
            {
                return this.lastLoginDate;
            }
            set
            {
                this.lastLoginDate = value;
                if (this.Membership != null)
                {
                    this.Membership.LastLoginDate = value;
                }
            }
        }

        public DateTime LastPasswordChangedDate
        {
            
            get
            {
                return _LastPasswordChangedDate;
            }
            
            private set
            {
                _LastPasswordChangedDate = value;
            }
        }

        public MembershipUser Membership
        {
            
            get
            {
                return _Membership;
            }
            
            private set
            {
                _Membership = value;
            }
        }

        public string MobilePIN
        {
            
            get
            {
                return _MobilePIN;
            }
            
            set
            {
                _MobilePIN = value;
            }
        }

        public string Password
        {
            
            get
            {
                return _Password;
            }
            
            set
            {
                _Password = value;
            }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            
            get
            {
                return _PasswordFormat;
            }
            
            set
            {
                _PasswordFormat = value;
            }
        }

        public string PasswordQuestion
        {
            
            get
            {
                return _PasswordQuestion;
            }
            
            private set
            {
                _PasswordQuestion = value;
            }
        }

        public string TradePassword
        {
            
            get
            {
                return _TradePassword;
            }
            
            set
            {
                _TradePassword = value;
            }
        }

        public MembershipPasswordFormat TradePasswordFormat
        {
            
            get
            {
                return _TradePasswordFormat;
            }
            
            set
            {
                _TradePasswordFormat = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return _UserId;
            }
            
            set
            {
                _UserId = value;
            }
        }

        public string Username
        {
            
            get
            {
                return _Username;
            }
            
            set
            {
                _Username = value;
            }
        }

        public Hidistro.Membership.Core.Enums.UserRole UserRole
        {
            
            get
            {
                return _UserRole;
            }
            
            private set
            {
                _UserRole = value;
            }
        }
    }
}


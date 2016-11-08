namespace Hidistro.Membership.Core
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Runtime.Serialization;

    public class CreateUserException : Exception
    {
        private Hidistro.Membership.Core.Enums.CreateUserStatus status;

        public CreateUserException()
        {
        }

        public CreateUserException(Hidistro.Membership.Core.Enums.CreateUserStatus status)
        {
            this.status = status;
        }

        public CreateUserException(string message) : base(message)
        {
        }

        public CreateUserException(Hidistro.Membership.Core.Enums.CreateUserStatus status, string message) : base(message)
        {
            this.status = status;
        }

        protected CreateUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CreateUserException(string message, Exception inner) : base(message, inner)
        {
        }

        public CreateUserException(Hidistro.Membership.Core.Enums.CreateUserStatus status, string message, Exception inner) : base(message, inner)
        {
            this.status = status;
        }

        public Hidistro.Membership.Core.Enums.CreateUserStatus CreateUserStatus
        {
            get
            {
                return this.status;
            }
        }
    }
}


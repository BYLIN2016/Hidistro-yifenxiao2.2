namespace Hidistro.Messages
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hishop.Plugins;
    using System;
    using System.Net.Mail;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Messenger
    {
        public static void AcceptRequest(IUser user)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("AcceptDistributorRequest");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericUserMessages(siteSettings, user.Username, user.Email, null, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        internal static EmailSender CreateEmailSender(SiteSettings settings)
        {
            string str;
            return CreateEmailSender(settings, out str);
        }

        internal static EmailSender CreateEmailSender(SiteSettings settings, out string msg)
        {
            try
            {
                msg = "";
                if (!settings.EmailEnabled)
                {
                    return null;
                }
                return EmailSender.CreateInstance(settings.EmailSender, HiCryptographer.Decrypt(settings.EmailSettings));
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return null;
            }
        }

        internal static SMSSender CreateSMSSender(SiteSettings settings)
        {
            string str;
            return CreateSMSSender(settings, out str);
        }

        internal static SMSSender CreateSMSSender(SiteSettings settings, out string msg)
        {
            try
            {
                msg = "";
                if (!settings.SMSEnabled)
                {
                    return null;
                }
                return SMSSender.CreateInstance(settings.SMSSender, HiCryptographer.Decrypt(settings.SMSSettings));
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return null;
            }
        }

        private static MailMessage GenericOrderEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
        {
            MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
            if (emailTemplate == null)
            {
                return null;
            }
            emailTemplate.Subject = GenericOrderMessageFormatter(settings, username, emailTemplate.Subject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
            emailTemplate.Body = GenericOrderMessageFormatter(settings, username, emailTemplate.Body, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
            return emailTemplate;
        }

        private static string GenericOrderMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
        {
            stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
            stringToFormat = stringToFormat.Replace("$Username$", username);
            stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
            stringToFormat = stringToFormat.Replace("$Total$", total.ToString("F"));
            stringToFormat = stringToFormat.Replace("$Memo$", memo);
            stringToFormat = stringToFormat.Replace("$Shipping_Type$", shippingType);
            stringToFormat = stringToFormat.Replace("$Shipping_Name$", shippingName);
            stringToFormat = stringToFormat.Replace("$Shipping_Addr$", shippingAddress);
            stringToFormat = stringToFormat.Replace("$Shipping_Zip$", shippingZip);
            stringToFormat = stringToFormat.Replace("$Shipping_Phone$", shippingPhone);
            stringToFormat = stringToFormat.Replace("$Shipping_Cell$", shippingCell);
            stringToFormat = stringToFormat.Replace("$Shipping_Email$", shippingEmail);
            stringToFormat = stringToFormat.Replace("$Shipping_Billno$", shippingBillno);
            stringToFormat = stringToFormat.Replace("$RefundMoney$", refundMoney.ToString("F"));
            stringToFormat = stringToFormat.Replace("$CloseReason$", closeReason);
            return stringToFormat;
        }

        private static void GenericOrderMessages(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
        {
            email = null;
            smsMessage = null;
            innerSubject = (string) (innerMessage = null);
            if ((template != null) && (settings != null))
            {
                if (template.SendEmail && settings.EmailEnabled)
                {
                    email = GenericOrderEmail(template, settings, username, userEmail, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                }
                if (template.SendSMS && settings.SMSEnabled)
                {
                    smsMessage = GenericOrderMessageFormatter(settings, username, template.SMSBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                }
                if (template.SendInnerMessage)
                {
                    innerSubject = GenericOrderMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                    innerMessage = GenericOrderMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                }
            }
        }

        private static MailMessage GenericUserEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string password, string dealPassword)
        {
            MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
            if (emailTemplate == null)
            {
                return null;
            }
            emailTemplate.Subject = GenericUserMessageFormatter(settings, emailTemplate.Subject, username, userEmail, password, dealPassword);
            emailTemplate.Body = GenericUserMessageFormatter(settings, emailTemplate.Body, username, userEmail, password, dealPassword);
            return emailTemplate;
        }

        private static string GenericUserMessageFormatter(SiteSettings settings, string stringToFormat, string username, string userEmail, string password, string dealPassword)
        {
            stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
            stringToFormat = stringToFormat.Replace("$Username$", username.Trim());
            stringToFormat = stringToFormat.Replace("$Email$", userEmail.Trim());
            stringToFormat = stringToFormat.Replace("$Password$", password);
            stringToFormat = stringToFormat.Replace("$DealPassword$", dealPassword);
            return stringToFormat;
        }

        private static void GenericUserMessages(SiteSettings settings, string username, string userEmail, string password, string dealPassword, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
        {
            email = null;
            smsMessage = null;
            innerSubject = (string) (innerMessage = null);
            if ((template != null) && (settings != null))
            {
                if (template.SendEmail && settings.EmailEnabled)
                {
                    email = GenericUserEmail(template, settings, username, userEmail, password, dealPassword);
                }
                if (template.SendSMS && settings.SMSEnabled)
                {
                    smsMessage = GenericUserMessageFormatter(settings, template.SMSBody, username, userEmail, password, dealPassword);
                }
                if (template.SendInnerMessage)
                {
                    innerSubject = GenericUserMessageFormatter(settings, template.InnerMessageSubject, username, userEmail, password, dealPassword);
                    innerMessage = GenericUserMessageFormatter(settings, template.InnerMessageBody, username, userEmail, password, dealPassword);
                }
            }
        }

        private static string GetUserCellPhone(IUser user)
        {
            if (user != null)
            {
                if (user is Member)
                {
                    return ((Member) user).CellPhone;
                }
                if (user is Distributor)
                {
                    return ((Distributor) user).CellPhone;
                }
            }
            return null;
        }

        public static void OrderClosed(IUser user, string orderId, string reason)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderClosed");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericOrderMessages(siteSettings, user.Username, user.Email, orderId, 0M, null, null, null, null, null, null, null, null, null, 0M, reason, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        public static void OrderCreated(OrderInfo order, IUser user)
        {
            if ((order != null) && (user != null))
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderCreated");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, order.GetTotal(), order.Remark, order.ModeName, order.ShipTo, order.Address, order.ZipCode, order.TelPhone, order.CellPhone, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        public static void OrderPayment(IUser user, string orderId, decimal amount)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderPayment");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericOrderMessages(siteSettings, user.Username, user.Email, orderId, amount, null, null, null, null, null, null, null, null, null, 0M, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        public static void OrderRefund(IUser user, string orderId, decimal amount)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderRefund");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericOrderMessages(siteSettings, user.Username, user.Email, orderId, 0M, null, null, null, null, null, null, null, null, null, amount, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        public static void OrderShipping(OrderInfo order, IUser user)
        {
            if ((order != null) && (user != null))
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderShipping");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, order.GetTotal(), order.Remark, order.RealModeName, order.ShipTo, order.Address, order.ZipCode, order.TelPhone, order.CellPhone, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        private static void Send(MessageTemplate template, SiteSettings settings, IUser user, bool sendFirst, MailMessage email, string innerSubject, string innerMessage, string smsMessage)
        {
            if (template.SendEmail && (email != null))
            {
                if (sendFirst)
                {
                    EmailSender sender = CreateEmailSender(settings);
                    if (!((sender != null) && SendMail(email, sender)))
                    {
                        Emails.EnqueuEmail(email, settings);
                    }
                }
                else
                {
                    Emails.EnqueuEmail(email, settings);
                }
            }
            if (template.SendSMS)
            {
                string userCellPhone = GetUserCellPhone(user);
                if (!string.IsNullOrEmpty(userCellPhone))
                {
                    string str2;
                    SendSMS(userCellPhone, smsMessage, settings, out str2);
                }
            }
            if (template.SendInnerMessage)
            {
                SendInnerMessage(settings, innerSubject, innerMessage, user.Username);
            }
        }

        public static SendStatus SendInnerMessage(SiteSettings settings, string subject, string message, string sendto)
        {
            if (((string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message)) || (subject.Trim().Length == 0)) || (message.Trim().Length == 0))
            {
                return SendStatus.RequireMsg;
            }
            if (settings == null)
            {
                return SendStatus.NoProvider;
            }
            if (settings.IsDistributorSettings)
            {
                IUser user = Users.GetUser(settings.UserId.Value);
                return (InnerMessageProvider.Instance().SendDistributorMessage(subject, message, user.Username, sendto) ? SendStatus.Success : SendStatus.Fail);
            }
            return (InnerMessageProvider.Instance().SendMessage(subject, message, sendto) ? SendStatus.Success : SendStatus.Fail);
        }

        internal static bool SendMail(MailMessage email, EmailSender sender)
        {
            string str;
            return SendMail(email, sender, out str);
        }

        internal static bool SendMail(MailMessage email, EmailSender sender, out string msg)
        {
            try
            {
                msg = "";
                return sender.Send(email, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding));
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return false;
            }
        }

        public static SendStatus SendMail(string subject, string body, string emailTo, SiteSettings settings, out string msg)
        {
            msg = "";
            if ((((string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body)) || (string.IsNullOrEmpty(emailTo) || (subject.Trim().Length == 0))) || (body.Trim().Length == 0)) || (emailTo.Trim().Length == 0))
            {
                return SendStatus.RequireMsg;
            }
            if (!((settings != null) && settings.EmailEnabled))
            {
                return SendStatus.NoProvider;
            }
            EmailSender sender = CreateEmailSender(settings, out msg);
            if (sender == null)
            {
                return SendStatus.ConfigError;
            }
            MailMessage message2 = new MailMessage();
            message2.IsBodyHtml = true;
            message2.Priority = MailPriority.High;
            message2.Body = body.Trim();
            message2.Subject = subject.Trim();
            MailMessage email = message2;
            email.To.Add(emailTo);
            return (SendMail(email, sender, out msg) ? SendStatus.Success : SendStatus.Fail);
        }

        public static SendStatus SendMail(string subject, string body, string[] cc, string[] bcc, SiteSettings settings, out string msg)
        {
            msg = "";
            if (((string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body)) || ((subject.Trim().Length == 0) || (body.Trim().Length == 0))) || (((cc == null) || (cc.Length == 0)) && ((bcc == null) || (bcc.Length == 0))))
            {
                return SendStatus.RequireMsg;
            }
            if (!((settings != null) && settings.EmailEnabled))
            {
                return SendStatus.NoProvider;
            }
            EmailSender sender = CreateEmailSender(settings, out msg);
            if (sender == null)
            {
                return SendStatus.ConfigError;
            }
            MailMessage message2 = new MailMessage();
            message2.IsBodyHtml = true;
            message2.Priority = MailPriority.High;
            message2.Body = body.Trim();
            message2.Subject = subject.Trim();
            MailMessage email = message2;
            if ((cc != null) && (cc.Length > 0))
            {
                foreach (string str in cc)
                {
                    email.CC.Add(str);
                }
            }
            if ((bcc != null) && (bcc.Length > 0))
            {
                foreach (string str in bcc)
                {
                    email.Bcc.Add(str);
                }
            }
            return (SendMail(email, sender, out msg) ? SendStatus.Success : SendStatus.Fail);
        }

        public static SendStatus SendSMS(string[] phoneNumbers, string message, SiteSettings settings, out string msg)
        {
            msg = "";
            if ((((phoneNumbers == null) || string.IsNullOrEmpty(message)) || (phoneNumbers.Length == 0)) || (message.Trim().Length == 0))
            {
                return SendStatus.RequireMsg;
            }
            if (!((settings != null) && settings.SMSEnabled))
            {
                return SendStatus.NoProvider;
            }
            SMSSender sender = CreateSMSSender(settings, out msg);
            if (sender == null)
            {
                return SendStatus.ConfigError;
            }
            return (sender.Send(phoneNumbers, message, out msg) ? SendStatus.Success : SendStatus.Fail);
        }

        public static SendStatus SendSMS(string phoneNumber, string message, SiteSettings settings, out string msg)
        {
            msg = "";
            if (((string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message)) || (phoneNumber.Trim().Length == 0)) || (message.Trim().Length == 0))
            {
                return SendStatus.RequireMsg;
            }
            if (!((settings != null) && settings.SMSEnabled))
            {
                return SendStatus.NoProvider;
            }
            SMSSender sender = CreateSMSSender(settings, out msg);
            if (sender == null)
            {
                return SendStatus.ConfigError;
            }
            return (sender.Send(phoneNumber, message, out msg) ? SendStatus.Success : SendStatus.Fail);
        }

        public static void UserDealPasswordChanged(IUser user, string changedDealPassword)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedDealPassword");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericUserMessages(siteSettings, user.Username, user.Email, null, changedDealPassword, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        public static void UserPasswordChanged(IUser user, string changedPassword)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedPassword");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericUserMessages(siteSettings, user.Username, user.Email, changedPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        public static void UserPasswordForgotten(IUser user, string resetPassword)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("ForgottenPassword");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericUserMessages(siteSettings, user.Username, user.Email, resetPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }

        public static void UserRegister(IUser user, string createPassword)
        {
            if (user != null)
            {
                MessageTemplate template = MessageTemplateHelper.GetTemplate("NewUserAccountCreated");
                if (template != null)
                {
                    MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    GenericUserMessages(siteSettings, user.Username, user.Email, createPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage);
                }
            }
        }
    }
}


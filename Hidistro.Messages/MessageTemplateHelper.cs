namespace Hidistro.Messages
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Mail;

    public static class MessageTemplateHelper
    {
        private const string CacheKey = "Message-{0}";
        private const string DistributorCacheKey = "Message-{0}-{1}";

        public static MessageTemplate GetDistributorMessageTemplate(string messageType, int distributorUserId)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                return null;
            }
            return MessageTemplateProvider.Instance().GetDistributorMessageTemplate(messageType, distributorUserId);
        }

        public static IList<MessageTemplate> GetDistributorMessageTemplates()
        {
            return MessageTemplateProvider.Instance().GetDistributorMessageTemplates();
        }

        internal static MailMessage GetEmailTemplate(MessageTemplate template, string emailTo)
        {
            if (((template == null) || !template.SendEmail) || string.IsNullOrEmpty(emailTo))
            {
                return null;
            }
            MailMessage message2 = new MailMessage();
            message2.IsBodyHtml = true;
            message2.Priority = MailPriority.High;
            message2.Body = template.EmailBody.Trim();
            message2.Subject = template.EmailSubject.Trim();
            MailMessage message = message2;
            message.To.Add(emailTo);
            return message;
        }

        public static MessageTemplate GetMessageTemplate(string messageType)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                return null;
            }
            return MessageTemplateProvider.Instance().GetMessageTemplate(messageType);
        }

        public static IList<MessageTemplate> GetMessageTemplates()
        {
            return MessageTemplateProvider.Instance().GetMessageTemplates();
        }

        internal static MessageTemplate GetTemplate(string messageType)
        {
            string str;
            messageType = messageType.ToLower();
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (siteSettings.IsDistributorSettings)
            {
                str = string.Format("Message-{0}-{1}", siteSettings.UserId.Value.ToString(CultureInfo.InvariantCulture), messageType);
            }
            else
            {
                str = string.Format("Message-{0}", messageType);
            }
            MessageTemplate template = HiCache.Get(str) as MessageTemplate;
            if (template == null)
            {
                template = siteSettings.IsDistributorSettings ? GetDistributorMessageTemplate(messageType, siteSettings.UserId.Value) : GetMessageTemplate(messageType);
                if (template != null)
                {
                    HiCache.Max(str, template);
                }
            }
            return template;
        }

        public static void UpdateDistributorSettings(IList<MessageTemplate> templates)
        {
            if ((templates != null) && (templates.Count != 0))
            {
                MessageTemplateProvider.Instance().UpdateDistributorSettings(templates);
                string str = HiContext.Current.User.UserId.ToString(CultureInfo.InvariantCulture);
                foreach (MessageTemplate template in templates)
                {
                    HiCache.Remove(string.Format("Message-{0}-{1}", str, template.MessageType.ToLower()));
                }
            }
        }

        public static void UpdateDistributorTemplate(MessageTemplate template)
        {
            if (template != null)
            {
                MessageTemplateProvider.Instance().UpdateDistributorTemplate(template);
                HiCache.Remove(string.Format("Message-{0}-{1}", HiContext.Current.User.UserId.ToString(CultureInfo.InvariantCulture), template.MessageType.ToLower()));
            }
        }

        public static void UpdateSettings(IList<MessageTemplate> templates)
        {
            if ((templates != null) && (templates.Count != 0))
            {
                MessageTemplateProvider.Instance().UpdateSettings(templates);
                foreach (MessageTemplate template in templates)
                {
                    HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
                }
            }
        }

        public static void UpdateTemplate(MessageTemplate template)
        {
            if (template != null)
            {
                MessageTemplateProvider.Instance().UpdateTemplate(template);
                HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
            }
        }
    }
}


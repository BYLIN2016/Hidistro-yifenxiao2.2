namespace Hidistro.Messages
{
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Context;
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Threading;

    public static class Emails
    {
        internal static void EnqueuEmail(MailMessage email, SiteSettings settings)
        {
            if (((email != null) && (email.To != null)) && (email.To.Count > 0))
            {
                if (settings.IsDistributorSettings)
                {
                    EmailQueueProvider.Instance().QueueDistributorEmail(email, settings.UserId.Value);
                }
                else
                {
                    EmailQueueProvider.Instance().QueueEmail(email);
                }
            }
        }

        public static void SendQueuedEmails(int failureInterval, int maxNumberOfTries, SiteSettings settings)
        {
            if (settings != null)
            {
                HiConfiguration config = HiConfiguration.GetConfig();
                Dictionary<Guid, MailMessage> dictionary = EmailQueueProvider.Instance().DequeueEmail();
                IList<Guid> list = new List<Guid>();
                EmailSender sender = Messenger.CreateEmailSender(settings);
                if (sender != null)
                {
                    int num = 0;
                    short smtpServerConnectionLimit = config.SmtpServerConnectionLimit;
                    foreach (Guid guid in dictionary.Keys)
                    {
                        if (Messenger.SendMail(dictionary[guid], sender))
                        {
                            EmailQueueProvider.Instance().DeleteQueuedEmail(guid);
                            if ((smtpServerConnectionLimit != -1) && (++num >= smtpServerConnectionLimit))
                            {
                                Thread.Sleep(new TimeSpan(0, 0, 0, 15, 0));
                                num = 0;
                            }
                        }
                        else
                        {
                            list.Add(guid);
                        }
                    }
                    if (list.Count > 0)
                    {
                        EmailQueueProvider.Instance().QueueSendingFailure(list, failureInterval, maxNumberOfTries);
                    }
                }
            }
        }

        public static void SendSubsiteEmails(int failureInterval, int maxNumberOfTries)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            Dictionary<Guid, SubsiteMailMessage> dictionary = EmailQueueProvider.Instance().DequeueDistributorEmail();
            Dictionary<int, EmailSender> dictionary2 = new Dictionary<int, EmailSender>();
            IList<Guid> list = new List<Guid>();
            IList<int> list2 = new List<int>();
            int num = 0;
            short smtpServerConnectionLimit = config.SmtpServerConnectionLimit;
            foreach (Guid guid in dictionary.Keys)
            {
                int distributorUserId = dictionary[guid].DistributorUserId;
                if (!list2.Contains(distributorUserId))
                {
                    EmailSender sender = null;
                    if (!dictionary2.ContainsKey(distributorUserId))
                    {
                        SiteSettings siteSettings = SettingsManager.GetSiteSettings(distributorUserId);
                        if (siteSettings == null)
                        {
                            list2.Add(distributorUserId);
                            continue;
                        }
                        sender = Messenger.CreateEmailSender(siteSettings);
                        if (sender == null)
                        {
                            list2.Add(distributorUserId);
                            continue;
                        }
                        dictionary2.Add(distributorUserId, sender);
                    }
                    else
                    {
                        sender = dictionary2[distributorUserId];
                    }
                    if (Messenger.SendMail(dictionary[guid].Mail, sender))
                    {
                        EmailQueueProvider.Instance().DeleteDistributorQueuedEmail(guid);
                        if ((smtpServerConnectionLimit != -1) && (++num >= smtpServerConnectionLimit))
                        {
                            Thread.Sleep(new TimeSpan(0, 0, 0, 15, 0));
                            num = 0;
                        }
                    }
                    else
                    {
                        list.Add(guid);
                    }
                }
            }
            if (list.Count > 0)
            {
                EmailQueueProvider.Instance().QueueDistributorSendingFailure(list, failureInterval, maxNumberOfTries);
            }
        }
    }
}


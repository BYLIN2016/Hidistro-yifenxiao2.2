namespace Hidistro.Jobs
{
    using Hidistro.Core.Jobs;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using System;
    using System.Globalization;
    using System.Xml;

    public class EmailJob : IJob
    {
        private int failureInterval = 15;
        private int numberOfTries = 5;

        public void Execute(XmlNode node)
        {
            if (null != node)
            {
                XmlAttribute attribute = node.Attributes["failureInterval"];
                XmlAttribute attribute2 = node.Attributes["numberOfTries"];
                if (attribute != null)
                {
                    try
                    {
                        this.failureInterval = int.Parse(attribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.failureInterval = 15;
                    }
                }
                if (attribute2 != null)
                {
                    try
                    {
                        this.numberOfTries = int.Parse(attribute2.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.numberOfTries = 5;
                    }
                }
                this.SendQueuedEmailJob();
            }
        }

        public void SendQueuedEmailJob()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings != null)
            {
                Emails.SendQueuedEmails(this.failureInterval, this.numberOfTries, masterSettings);
            }
        }
    }
}


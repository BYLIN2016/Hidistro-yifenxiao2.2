namespace Hidistro.Jobs
{
    using Hidistro.Core.Jobs;
    using Hidistro.Messages;
    using System;
    using System.Globalization;
    using System.Xml;

    public class SubsiteEmailJob : IJob
    {
        private int failureInterval = 15;
        private int numberOfTries = 3;

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
                        this.numberOfTries = 3;
                    }
                }
                Emails.SendSubsiteEmails(this.failureInterval, this.numberOfTries);
            }
        }
    }
}


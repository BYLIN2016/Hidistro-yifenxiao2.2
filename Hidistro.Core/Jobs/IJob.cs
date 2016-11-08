namespace Hidistro.Core.Jobs
{
    using System;
    using System.Xml;

    public interface IJob
    {
        void Execute(XmlNode node);
    }
}


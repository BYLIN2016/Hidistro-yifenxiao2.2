namespace Hishop.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class ConfigData
    {
        
        private IList<string> _ErrorMsgs;
        
        private bool _IsValid;
        
        private bool _NeedProtect;
        private readonly XmlDocument doc;
        private readonly XmlNode root;

        public ConfigData()
        {
            this.IsValid = true;
            this.ErrorMsgs = new List<string>();
            this.doc = new XmlDocument();
            this.root = this.doc.CreateElement("xml");
            this.doc.AppendChild(this.root);
        }

        public ConfigData(string xml)
        {
            this.IsValid = true;
            this.ErrorMsgs = new List<string>();
            this.doc = new XmlDocument();
            this.doc.LoadXml(xml);
            this.root = this.doc.FirstChild;
        }

        internal void Add(string attributeName, string val)
        {
            if ((!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(val)) && ((attributeName.Trim().Length > 0) && (val.Length > 0)))
            {
                XmlNode newChild = this.doc.CreateElement(attributeName);
                newChild.InnerText = val;
                this.root.AppendChild(newChild);
            }
        }

        internal XmlNodeList AttributeNodes
        {
            get
            {
                return this.root.ChildNodes;
            }
        }

        public IList<string> ErrorMsgs
        {
            
            get
            {
                return _ErrorMsgs;
            }
            
            private set
            {
                _ErrorMsgs = value;
            }
        }

        public bool IsValid
        {
            
            get
            {
                return _IsValid;
            }
            
            internal set
            {
                _IsValid = value;
            }
        }

        public bool NeedProtect
        {
            
            get
            {
                return _NeedProtect;
            }
            
            internal set
            {
                _NeedProtect = value;
            }
        }

        public string SettingsXml
        {
            get
            {
                return this.root.OuterXml;
            }
        }
    }
}


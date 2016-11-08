using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;
namespace HtmlAgilityPack
{
	[DebuggerDisplay("Name: {OriginalName}}")]
	public class HtmlNode : IXPathNavigable
	{
		internal HtmlAttributeCollection _attributes;
		internal HtmlNodeCollection _childnodes;
		internal HtmlNode _endnode;
		internal bool _innerchanged;
		internal string _innerhtml;
		internal int _innerlength;
		internal int _innerstartindex;
		internal int _line;
		internal int _lineposition;
		private string _name;
		internal int _namelength;
		internal int _namestartindex;
		internal HtmlNode _nextnode;
		internal HtmlNodeType _nodetype;
		internal bool _outerchanged;
		internal string _outerhtml;
		internal int _outerlength;
		internal int _outerstartindex;
		private string _optimizedName;
		internal HtmlDocument _ownerdocument;
		internal HtmlNode _parentnode;
		internal HtmlNode _prevnode;
		internal HtmlNode _prevwithsamename;
		internal bool _starttag;
		internal int _streamposition;
		public static readonly string HtmlNodeTypeNameComment;
		public static readonly string HtmlNodeTypeNameDocument;
		public static readonly string HtmlNodeTypeNameText;
		public static Dictionary<string, HtmlElementFlag> ElementsFlags;
		public HtmlAttributeCollection Attributes
		{
			get
			{
				if (!this.HasAttributes)
				{
					this._attributes = new HtmlAttributeCollection(this);
				}
				return this._attributes;
			}
			internal set
			{
				this._attributes = value;
			}
		}
		public HtmlNodeCollection ChildNodes
		{
			get
			{
				HtmlNodeCollection arg_19_0;
				if ((arg_19_0 = this._childnodes) == null)
				{
					arg_19_0 = (this._childnodes = new HtmlNodeCollection(this));
				}
				return arg_19_0;
			}
			internal set
			{
				this._childnodes = value;
			}
		}
		public bool Closed
		{
			get
			{
				return this._endnode != null;
			}
		}
		public HtmlAttributeCollection ClosingAttributes
		{
			get
			{
				if (this.HasClosingAttributes)
				{
					return this._endnode.Attributes;
				}
				return new HtmlAttributeCollection(this);
			}
		}
		internal HtmlNode EndNode
		{
			get
			{
				return this._endnode;
			}
		}
		public HtmlNode FirstChild
		{
			get
			{
				if (this.HasChildNodes)
				{
					return this._childnodes[0];
				}
				return null;
			}
		}
		public bool HasAttributes
		{
			get
			{
				return this._attributes != null && this._attributes.Count > 0;
			}
		}
		public bool HasChildNodes
		{
			get
			{
				return this._childnodes != null && this._childnodes.Count > 0;
			}
		}
		public bool HasClosingAttributes
		{
			get
			{
				return this._endnode != null && this._endnode != this && this._endnode._attributes != null && this._endnode._attributes.Count > 0;
			}
		}
		public string Id
		{
			get
			{
				if (this._ownerdocument.Nodesid == null)
				{
					throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
				}
				return this.GetId();
			}
			set
			{
				if (this._ownerdocument.Nodesid == null)
				{
					throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.SetId(value);
			}
		}
		public virtual string InnerHtml
		{
			get
			{
				if (this._innerchanged)
				{
					this._innerhtml = this.WriteContentTo();
					this._innerchanged = false;
					return this._innerhtml;
				}
				if (this._innerhtml != null)
				{
					return this._innerhtml;
				}
				if (this._innerstartindex < 0)
				{
					return string.Empty;
				}
				return this._ownerdocument.Text.Substring(this._innerstartindex, this._innerlength);
			}
			set
			{
				HtmlDocument htmlDocument = new HtmlDocument();
				htmlDocument.LoadHtml(value);
				this.RemoveAllChildren();
				this.AppendChildren(htmlDocument.DocumentNode.ChildNodes);
			}
		}
		public virtual string InnerText
		{
			get
			{
				if (this._nodetype == HtmlNodeType.Text)
				{
					return ((HtmlTextNode)this).Text;
				}
				if (this._nodetype == HtmlNodeType.Comment)
				{
					return ((HtmlCommentNode)this).Comment;
				}
				if (!this.HasChildNodes)
				{
					return string.Empty;
				}
				string text = null;
				foreach (HtmlNode current in (IEnumerable<HtmlNode>)this.ChildNodes)
				{
					text += current.InnerText;
				}
				return text;
			}
		}
		public HtmlNode LastChild
		{
			get
			{
				if (this.HasChildNodes)
				{
					return this._childnodes[this._childnodes.Count - 1];
				}
				return null;
			}
		}
		public int Line
		{
			get
			{
				return this._line;
			}
			internal set
			{
				this._line = value;
			}
		}
		public int LinePosition
		{
			get
			{
				return this._lineposition;
			}
			internal set
			{
				this._lineposition = value;
			}
		}
		public string Name
		{
			get
			{
				if (this._optimizedName == null)
				{
					if (this._name == null)
					{
						this.Name = this._ownerdocument.Text.Substring(this._namestartindex, this._namelength);
					}
					if (this._name == null)
					{
						this._optimizedName = string.Empty;
					}
					else
					{
						this._optimizedName = this._name.ToLower();
					}
				}
				return this._optimizedName;
			}
			set
			{
				this._name = value;
				this._optimizedName = null;
			}
		}
		public HtmlNode NextSibling
		{
			get
			{
				return this._nextnode;
			}
			internal set
			{
				this._nextnode = value;
			}
		}
		public HtmlNodeType NodeType
		{
			get
			{
				return this._nodetype;
			}
			internal set
			{
				this._nodetype = value;
			}
		}
		public string OriginalName
		{
			get
			{
				return this._name;
			}
		}
		public virtual string OuterHtml
		{
			get
			{
				if (this._outerchanged)
				{
					this._outerhtml = this.WriteTo();
					this._outerchanged = false;
					return this._outerhtml;
				}
				if (this._outerhtml != null)
				{
					return this._outerhtml;
				}
				if (this._outerstartindex < 0)
				{
					return string.Empty;
				}
				return this._ownerdocument.Text.Substring(this._outerstartindex, this._outerlength);
			}
		}
		public HtmlDocument OwnerDocument
		{
			get
			{
				return this._ownerdocument;
			}
			internal set
			{
				this._ownerdocument = value;
			}
		}
		public HtmlNode ParentNode
		{
			get
			{
				return this._parentnode;
			}
			internal set
			{
				this._parentnode = value;
			}
		}
		public HtmlNode PreviousSibling
		{
			get
			{
				return this._prevnode;
			}
			internal set
			{
				this._prevnode = value;
			}
		}
		public int StreamPosition
		{
			get
			{
				return this._streamposition;
			}
		}
		public string XPath
		{
			get
			{
				string str = (this.ParentNode == null || this.ParentNode.NodeType == HtmlNodeType.Document) ? "/" : (this.ParentNode.XPath + "/");
				return str + this.GetRelativeXpath();
			}
		}
		public XPathNavigator CreateNavigator()
		{
			return new HtmlNodeNavigator(this.OwnerDocument, this);
		}
		public XPathNavigator CreateRootNavigator()
		{
			return new HtmlNodeNavigator(this.OwnerDocument, this.OwnerDocument.DocumentNode);
		}
		public HtmlNodeCollection SelectNodes(string xpath)
		{
			HtmlNodeCollection htmlNodeCollection = new HtmlNodeCollection(null);
			HtmlNodeNavigator htmlNodeNavigator = new HtmlNodeNavigator(this.OwnerDocument, this);
			XPathNodeIterator xPathNodeIterator = htmlNodeNavigator.Select(xpath);
			while (xPathNodeIterator.MoveNext())
			{
				HtmlNodeNavigator htmlNodeNavigator2 = (HtmlNodeNavigator)xPathNodeIterator.Current;
				htmlNodeCollection.Add(htmlNodeNavigator2.CurrentNode);
			}
			if (htmlNodeCollection.Count == 0)
			{
				return null;
			}
			return htmlNodeCollection;
		}
		public HtmlNode SelectSingleNode(string xpath)
		{
			if (xpath == null)
			{
				throw new ArgumentNullException("xpath");
			}
			HtmlNodeNavigator htmlNodeNavigator = new HtmlNodeNavigator(this.OwnerDocument, this);
			XPathNodeIterator xPathNodeIterator = htmlNodeNavigator.Select(xpath);
			if (!xPathNodeIterator.MoveNext())
			{
				return null;
			}
			HtmlNodeNavigator htmlNodeNavigator2 = (HtmlNodeNavigator)xPathNodeIterator.Current;
			return htmlNodeNavigator2.CurrentNode;
		}
		static HtmlNode()
		{
			HtmlNode.HtmlNodeTypeNameComment = "#comment";
			HtmlNode.HtmlNodeTypeNameDocument = "#document";
			HtmlNode.HtmlNodeTypeNameText = "#text";
			HtmlNode.ElementsFlags = new Dictionary<string, HtmlElementFlag>();
			HtmlNode.ElementsFlags.Add("script", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("style", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("noxhtml", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("base", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("link", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("meta", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("isindex", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("hr", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("col", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("img", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("param", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("embed", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("frame", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("wbr", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("bgsound", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("spacer", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("keygen", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("area", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("input", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("basefont", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("form", HtmlElementFlag.Empty | HtmlElementFlag.CanOverlap);
			HtmlNode.ElementsFlags.Add("option", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("br", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
			HtmlNode.ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
		}
		public HtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index)
		{
			this._nodetype = type;
			this._ownerdocument = ownerdocument;
			this._outerstartindex = index;
			switch (type)
			{
			case HtmlNodeType.Document:
				this.Name = HtmlNode.HtmlNodeTypeNameDocument;
				this._endnode = this;
				break;
			case HtmlNodeType.Comment:
				this.Name = HtmlNode.HtmlNodeTypeNameComment;
				this._endnode = this;
				break;
			case HtmlNodeType.Text:
				this.Name = HtmlNode.HtmlNodeTypeNameText;
				this._endnode = this;
				break;
			}
			if (this._ownerdocument.Openednodes != null && !this.Closed && -1 != index)
			{
				this._ownerdocument.Openednodes.Add(index, this);
			}
			if (-1 != index || type == HtmlNodeType.Comment || type == HtmlNodeType.Text)
			{
				return;
			}
			this._outerchanged = true;
			this._innerchanged = true;
		}
		public static bool CanOverlapElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!HtmlNode.ElementsFlags.ContainsKey(name.ToLower()))
			{
				return false;
			}
			HtmlElementFlag htmlElementFlag = HtmlNode.ElementsFlags[name.ToLower()];
			return (htmlElementFlag & HtmlElementFlag.CanOverlap) != (HtmlElementFlag)0;
		}
		public static HtmlNode CreateNode(string html)
		{
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);
			return htmlDocument.DocumentNode.FirstChild;
		}
		public static bool IsCDataElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!HtmlNode.ElementsFlags.ContainsKey(name.ToLower()))
			{
				return false;
			}
			HtmlElementFlag htmlElementFlag = HtmlNode.ElementsFlags[name.ToLower()];
			return (htmlElementFlag & HtmlElementFlag.CData) != (HtmlElementFlag)0;
		}
		public static bool IsClosedElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!HtmlNode.ElementsFlags.ContainsKey(name.ToLower()))
			{
				return false;
			}
			HtmlElementFlag htmlElementFlag = HtmlNode.ElementsFlags[name.ToLower()];
			return (htmlElementFlag & HtmlElementFlag.Closed) != (HtmlElementFlag)0;
		}
		public static bool IsEmptyElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				return true;
			}
			if ('!' == name[0])
			{
				return true;
			}
			if ('?' == name[0])
			{
				return true;
			}
			if (!HtmlNode.ElementsFlags.ContainsKey(name.ToLower()))
			{
				return false;
			}
			HtmlElementFlag htmlElementFlag = HtmlNode.ElementsFlags[name.ToLower()];
			return (htmlElementFlag & HtmlElementFlag.Empty) != (HtmlElementFlag)0;
		}
		public static bool IsOverlappedClosingElement(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (text.Length <= 4)
			{
				return false;
			}
			if (text[0] != '<' || text[text.Length - 1] != '>' || text[1] != '/')
			{
				return false;
			}
			string name = text.Substring(2, text.Length - 3);
			return HtmlNode.CanOverlapElement(name);
		}
		public IEnumerable<HtmlNode> Ancestors()
		{
			HtmlNode parentNode = this.ParentNode;
			while (parentNode.ParentNode != null)
			{
				yield return parentNode.ParentNode;
				parentNode = parentNode.ParentNode;
			}
			yield break;
		}
		public IEnumerable<HtmlNode> Ancestors(string name)
		{
			for (HtmlNode parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
			{
				if (parentNode.Name == name)
				{
					yield return parentNode;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> AncestorsAndSelf()
		{
			for (HtmlNode htmlNode = this; htmlNode != null; htmlNode = htmlNode.ParentNode)
			{
				yield return htmlNode;
			}
			yield break;
		}
		public IEnumerable<HtmlNode> AncestorsAndSelf(string name)
		{
			for (HtmlNode htmlNode = this; htmlNode != null; htmlNode = htmlNode.ParentNode)
			{
				if (htmlNode.Name == name)
				{
					yield return htmlNode;
				}
			}
			yield break;
		}
		public HtmlNode AppendChild(HtmlNode newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			this.ChildNodes.Append(newChild);
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this._outerchanged = true;
			this._innerchanged = true;
			return newChild;
		}
		public void AppendChildren(HtmlNodeCollection newChildren)
		{
			if (newChildren == null)
			{
				throw new ArgumentNullException("newChildren");
			}
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)newChildren)
			{
				this.AppendChild(current);
			}
		}
		public IEnumerable<HtmlAttribute> ChildAttributes(string name)
		{
			return this.Attributes.AttributesWithName(name);
		}
		public HtmlNode Clone()
		{
			return this.CloneNode(true);
		}
		public HtmlNode CloneNode(string newName)
		{
			return this.CloneNode(newName, true);
		}
		public HtmlNode CloneNode(string newName, bool deep)
		{
			if (newName == null)
			{
				throw new ArgumentNullException("newName");
			}
			HtmlNode htmlNode = this.CloneNode(deep);
			htmlNode.Name = newName;
			return htmlNode;
		}
		public HtmlNode CloneNode(bool deep)
		{
			HtmlNode htmlNode = this._ownerdocument.CreateNode(this._nodetype);
			htmlNode.Name = this.Name;
			switch (this._nodetype)
			{
			case HtmlNodeType.Comment:
				((HtmlCommentNode)htmlNode).Comment = ((HtmlCommentNode)this).Comment;
				return htmlNode;
			case HtmlNodeType.Text:
				((HtmlTextNode)htmlNode).Text = ((HtmlTextNode)this).Text;
				return htmlNode;
			default:
				if (this.HasAttributes)
				{
					foreach (HtmlAttribute current in (IEnumerable<HtmlAttribute>)this._attributes)
					{
						HtmlAttribute newAttribute = current.Clone();
						htmlNode.Attributes.Append(newAttribute);
					}
				}
				if (this.HasClosingAttributes)
				{
					htmlNode._endnode = this._endnode.CloneNode(false);
					foreach (HtmlAttribute current2 in (IEnumerable<HtmlAttribute>)this._endnode._attributes)
					{
						HtmlAttribute newAttribute2 = current2.Clone();
						htmlNode._endnode._attributes.Append(newAttribute2);
					}
				}
				if (!deep)
				{
					return htmlNode;
				}
				if (!this.HasChildNodes)
				{
					return htmlNode;
				}
				foreach (HtmlNode current3 in (IEnumerable<HtmlNode>)this._childnodes)
				{
					HtmlNode newChild = current3.Clone();
					htmlNode.AppendChild(newChild);
				}
				return htmlNode;
			}
		}
		public void CopyFrom(HtmlNode node)
		{
			this.CopyFrom(node, true);
		}
		public void CopyFrom(HtmlNode node, bool deep)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.Attributes.RemoveAll();
			if (node.HasAttributes)
			{
				foreach (HtmlAttribute current in (IEnumerable<HtmlAttribute>)node.Attributes)
				{
					this.SetAttributeValue(current.Name, current.Value);
				}
			}
			if (!deep)
			{
				this.RemoveAllChildren();
				if (node.HasChildNodes)
				{
					foreach (HtmlNode current2 in (IEnumerable<HtmlNode>)node.ChildNodes)
					{
						this.AppendChild(current2.CloneNode(true));
					}
				}
			}
		}
		[Obsolete("Use Descendants() instead, the results of this function will change in a future version")]
		public IEnumerable<HtmlNode> DescendantNodes()
		{
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)this.ChildNodes)
			{
				yield return current;
				foreach (HtmlNode current2 in current.DescendantNodes())
				{
					yield return current2;
				}
			}
			yield break;
		}
		[Obsolete("Use DescendantsAndSelf() instead, the results of this function will change in a future version")]
		public IEnumerable<HtmlNode> DescendantNodesAndSelf()
		{
			return this.DescendantsAndSelf();
		}
		public IEnumerable<HtmlNode> Descendants()
		{
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)this.ChildNodes)
			{
				yield return current;
				foreach (HtmlNode current2 in current.Descendants())
				{
					yield return current2;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> Descendants(string name)
		{
			name = name.ToLowerInvariant();
			foreach (HtmlNode current in this.Descendants())
			{
				if (current.Name.Equals(name))
				{
					yield return current;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> DescendantsAndSelf()
		{
			yield return this;
			foreach (HtmlNode current in this.Descendants())
			{
				HtmlNode htmlNode = current;
				if (htmlNode != null)
				{
					yield return htmlNode;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> DescendantsAndSelf(string name)
		{
			yield return this;
			foreach (HtmlNode current in this.Descendants())
			{
				if (current.Name == name)
				{
					yield return current;
				}
			}
			yield break;
		}
		public HtmlNode Element(string name)
		{
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)this.ChildNodes)
			{
				if (current.Name == name)
				{
					return current;
				}
			}
			return null;
		}
		public IEnumerable<HtmlNode> Elements(string name)
		{
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)this.ChildNodes)
			{
				if (current.Name == name)
				{
					yield return current;
				}
			}
			yield break;
		}
		public string GetAttributeValue(string name, string def)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!this.HasAttributes)
			{
				return def;
			}
			HtmlAttribute htmlAttribute = this.Attributes[name];
			if (htmlAttribute == null)
			{
				return def;
			}
			return htmlAttribute.Value;
		}
		public int GetAttributeValue(string name, int def)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!this.HasAttributes)
			{
				return def;
			}
			HtmlAttribute htmlAttribute = this.Attributes[name];
			if (htmlAttribute == null)
			{
				return def;
			}
			int result;
			try
			{
				result = Convert.ToInt32(htmlAttribute.Value);
			}
			catch
			{
				result = def;
			}
			return result;
		}
		public bool GetAttributeValue(string name, bool def)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!this.HasAttributes)
			{
				return def;
			}
			HtmlAttribute htmlAttribute = this.Attributes[name];
			if (htmlAttribute == null)
			{
				return def;
			}
			bool result;
			try
			{
				result = Convert.ToBoolean(htmlAttribute.Value);
			}
			catch
			{
				result = def;
			}
			return result;
		}
		public HtmlNode InsertAfter(HtmlNode newChild, HtmlNode refChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			if (refChild == null)
			{
				return this.PrependChild(newChild);
			}
			if (newChild == refChild)
			{
				return newChild;
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[refChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Insert(num + 1, newChild);
			}
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this._outerchanged = true;
			this._innerchanged = true;
			return newChild;
		}
		public HtmlNode InsertBefore(HtmlNode newChild, HtmlNode refChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			if (refChild == null)
			{
				return this.AppendChild(newChild);
			}
			if (newChild == refChild)
			{
				return newChild;
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[refChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Insert(num, newChild);
			}
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this._outerchanged = true;
			this._innerchanged = true;
			return newChild;
		}
		public HtmlNode PrependChild(HtmlNode newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			this.ChildNodes.Prepend(newChild);
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this._outerchanged = true;
			this._innerchanged = true;
			return newChild;
		}
		public void PrependChildren(HtmlNodeCollection newChildren)
		{
			if (newChildren == null)
			{
				throw new ArgumentNullException("newChildren");
			}
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)newChildren)
			{
				this.PrependChild(current);
			}
		}
		public void Remove()
		{
			if (this.ParentNode != null)
			{
				this.ParentNode.ChildNodes.Remove(this);
			}
		}
		public void RemoveAll()
		{
			this.RemoveAllChildren();
			if (this.HasAttributes)
			{
				this._attributes.Clear();
			}
			if (this._endnode != null && this._endnode != this && this._endnode._attributes != null)
			{
				this._endnode._attributes.Clear();
			}
			this._outerchanged = true;
			this._innerchanged = true;
		}
		public void RemoveAllChildren()
		{
			if (!this.HasChildNodes)
			{
				return;
			}
			if (this._ownerdocument.OptionUseIdAttribute)
			{
				foreach (HtmlNode current in (IEnumerable<HtmlNode>)this._childnodes)
				{
					this._ownerdocument.SetIdForNode(null, current.GetId());
				}
			}
			this._childnodes.Clear();
			this._outerchanged = true;
			this._innerchanged = true;
		}
		public HtmlNode RemoveChild(HtmlNode oldChild)
		{
			if (oldChild == null)
			{
				throw new ArgumentNullException("oldChild");
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[oldChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Remove(num);
			}
			this._ownerdocument.SetIdForNode(null, oldChild.GetId());
			this._outerchanged = true;
			this._innerchanged = true;
			return oldChild;
		}
		public HtmlNode RemoveChild(HtmlNode oldChild, bool keepGrandChildren)
		{
			if (oldChild == null)
			{
				throw new ArgumentNullException("oldChild");
			}
			if (oldChild._childnodes != null && keepGrandChildren)
			{
				HtmlNode previousSibling = oldChild.PreviousSibling;
				foreach (HtmlNode current in (IEnumerable<HtmlNode>)oldChild._childnodes)
				{
					this.InsertAfter(current, previousSibling);
				}
			}
			this.RemoveChild(oldChild);
			this._outerchanged = true;
			this._innerchanged = true;
			return oldChild;
		}
		public HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild)
		{
			if (newChild == null)
			{
				return this.RemoveChild(oldChild);
			}
			if (oldChild == null)
			{
				return this.AppendChild(newChild);
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[oldChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Replace(num, newChild);
			}
			this._ownerdocument.SetIdForNode(null, oldChild.GetId());
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this._outerchanged = true;
			this._innerchanged = true;
			return newChild;
		}
		public HtmlAttribute SetAttributeValue(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlAttribute htmlAttribute = this.Attributes[name];
			if (htmlAttribute == null)
			{
				return this.Attributes.Append(this._ownerdocument.CreateAttribute(name, value));
			}
			htmlAttribute.Value = value;
			return htmlAttribute;
		}
		public void WriteContentTo(TextWriter outText)
		{
			if (this._childnodes == null)
			{
				return;
			}
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)this._childnodes)
			{
				current.WriteTo(outText);
			}
		}
		public string WriteContentTo()
		{
			StringWriter stringWriter = new StringWriter();
			this.WriteContentTo(stringWriter);
			stringWriter.Flush();
			return stringWriter.ToString();
		}
		public void WriteTo(TextWriter outText)
		{
			switch (this._nodetype)
			{
			case HtmlNodeType.Document:
				if (this._ownerdocument.OptionOutputAsXml)
				{
					outText.Write("<?xml version=\"1.0\" encoding=\"" + this._ownerdocument.GetOutEncoding().BodyName + "\"?>");
					if (this._ownerdocument.DocumentNode.HasChildNodes)
					{
						int num = this._ownerdocument.DocumentNode._childnodes.Count;
						if (num > 0)
						{
							HtmlNode xmlDeclaration = this._ownerdocument.GetXmlDeclaration();
							if (xmlDeclaration != null)
							{
								num--;
							}
							if (num > 1)
							{
								if (this._ownerdocument.OptionOutputUpperCase)
								{
									outText.Write("<SPAN>");
									this.WriteContentTo(outText);
									outText.Write("</SPAN>");
									return;
								}
								outText.Write("<span>");
								this.WriteContentTo(outText);
								outText.Write("</span>");
								return;
							}
						}
					}
				}
				this.WriteContentTo(outText);
				return;
			case HtmlNodeType.Element:
			{
				string text = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpper() : this.Name;
				if (this._ownerdocument.OptionOutputOriginalCase)
				{
					text = this.OriginalName;
				}
				if (this._ownerdocument.OptionOutputAsXml)
				{
					if (text.Length <= 0)
					{
						return;
					}
					if (text[0] == '?')
					{
						return;
					}
					if (text.Trim().Length == 0)
					{
						return;
					}
					text = HtmlDocument.GetXmlName(text);
				}
				outText.Write("<" + text);
				this.WriteAttributes(outText, false);
				if (this.HasChildNodes)
				{
					outText.Write(">");
					bool flag = false;
					if (this._ownerdocument.OptionOutputAsXml && HtmlNode.IsCDataElement(this.Name))
					{
						flag = true;
						outText.Write("\r\n//<![CDATA[\r\n");
					}
					if (flag)
					{
						if (this.HasChildNodes)
						{
							this.ChildNodes[0].WriteTo(outText);
						}
						outText.Write("\r\n//]]>//\r\n");
					}
					else
					{
						this.WriteContentTo(outText);
					}
					outText.Write("</" + text);
					if (!this._ownerdocument.OptionOutputAsXml)
					{
						this.WriteAttributes(outText, true);
					}
					outText.Write(">");
					return;
				}
				if (HtmlNode.IsEmptyElement(this.Name))
				{
					if (this._ownerdocument.OptionWriteEmptyNodes || this._ownerdocument.OptionOutputAsXml)
					{
						outText.Write(" />");
						return;
					}
					if (this.Name.Length > 0 && this.Name[0] == '?')
					{
						outText.Write("?");
					}
					outText.Write(">");
					return;
				}
				else
				{
					outText.Write("></" + text + ">");
				}
				return;
			}
			case HtmlNodeType.Comment:
			{
				string text2 = ((HtmlCommentNode)this).Comment;
				if (this._ownerdocument.OptionOutputAsXml)
				{
					outText.Write("<!--" + HtmlNode.GetXmlComment((HtmlCommentNode)this) + " -->");
					return;
				}
				outText.Write(text2);
				return;
			}
			case HtmlNodeType.Text:
			{
				string text2 = ((HtmlTextNode)this).Text;
				outText.Write(this._ownerdocument.OptionOutputAsXml ? HtmlDocument.HtmlEncode(text2) : text2);
				return;
			}
			default:
				return;
			}
		}
		public void WriteTo(XmlWriter writer)
		{
			switch (this._nodetype)
			{
			case HtmlNodeType.Document:
				writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"" + this._ownerdocument.GetOutEncoding().BodyName + "\"");
				if (!this.HasChildNodes)
				{
					return;
				}
				using (IEnumerator<HtmlNode> enumerator = ((IEnumerable<HtmlNode>)this.ChildNodes).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						HtmlNode current = enumerator.Current;
						current.WriteTo(writer);
					}
					return;
				}
				break;
			case HtmlNodeType.Element:
			{
				string localName = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpper() : this.Name;
				if (this._ownerdocument.OptionOutputOriginalCase)
				{
					localName = this.OriginalName;
				}
				writer.WriteStartElement(localName);
				HtmlNode.WriteAttributes(writer, this);
				if (this.HasChildNodes)
				{
					foreach (HtmlNode current2 in (IEnumerable<HtmlNode>)this.ChildNodes)
					{
						current2.WriteTo(writer);
					}
				}
				writer.WriteEndElement();
				return;
			}
			case HtmlNodeType.Comment:
				writer.WriteComment(HtmlNode.GetXmlComment((HtmlCommentNode)this));
				return;
			case HtmlNodeType.Text:
				break;
			default:
				return;
			}
			string text = ((HtmlTextNode)this).Text;
			writer.WriteString(text);
		}
		public string WriteTo()
		{
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				this.WriteTo(stringWriter);
				stringWriter.Flush();
				result = stringWriter.ToString();
			}
			return result;
		}
		internal static string GetXmlComment(HtmlCommentNode comment)
		{
			string comment2 = comment.Comment;
			return comment2.Substring(4, comment2.Length - 7).Replace("--", " - -");
		}
		internal static void WriteAttributes(XmlWriter writer, HtmlNode node)
		{
			if (!node.HasAttributes)
			{
				return;
			}
			foreach (HtmlAttribute current in node.Attributes.Hashitems.Values)
			{
				writer.WriteAttributeString(current.XmlName, current.Value);
			}
		}
		internal void CloseNode(HtmlNode endnode)
		{
			if (!this._ownerdocument.OptionAutoCloseOnEnd && this._childnodes != null)
			{
				foreach (HtmlNode current in (IEnumerable<HtmlNode>)this._childnodes)
				{
					if (!current.Closed)
					{
						HtmlNode htmlNode = new HtmlNode(this.NodeType, this._ownerdocument, -1);
						htmlNode._endnode = htmlNode;
						current.CloseNode(htmlNode);
					}
				}
			}
			if (!this.Closed)
			{
				this._endnode = endnode;
				if (this._ownerdocument.Openednodes != null)
				{
					this._ownerdocument.Openednodes.Remove(this._outerstartindex);
				}
				HtmlNode dictionaryValueOrNull = Utilities.GetDictionaryValueOrNull<string, HtmlNode>(this._ownerdocument.Lastnodes, this.Name);
				if (dictionaryValueOrNull == this)
				{
					this._ownerdocument.Lastnodes.Remove(this.Name);
					this._ownerdocument.UpdateLastParentNode();
				}
				if (endnode == this)
				{
					return;
				}
				this._innerstartindex = this._outerstartindex + this._outerlength;
				this._innerlength = endnode._outerstartindex - this._innerstartindex;
				this._outerlength = endnode._outerstartindex + endnode._outerlength - this._outerstartindex;
			}
		}
		internal string GetId()
		{
			HtmlAttribute htmlAttribute = this.Attributes["id"];
			if (htmlAttribute != null)
			{
				return htmlAttribute.Value;
			}
			return string.Empty;
		}
		internal void SetId(string id)
		{
			HtmlAttribute htmlAttribute = this.Attributes["id"] ?? this._ownerdocument.CreateAttribute("id");
			htmlAttribute.Value = id;
			this._ownerdocument.SetIdForNode(this, htmlAttribute.Value);
			this._outerchanged = true;
		}
		internal void WriteAttribute(TextWriter outText, HtmlAttribute att)
		{
			string text = (att.QuoteType == AttributeValueQuote.DoubleQuote) ? "\"" : "'";
			string text2;
			if (this._ownerdocument.OptionOutputAsXml)
			{
				text2 = (this._ownerdocument.OptionOutputUpperCase ? att.XmlName.ToUpper() : att.XmlName);
				if (this._ownerdocument.OptionOutputOriginalCase)
				{
					text2 = att.OriginalName;
				}
				outText.Write(string.Concat(new string[]
				{
					" ",
					text2,
					"=",
					text,
					HtmlDocument.HtmlEncode(att.XmlValue),
					text
				}));
				return;
			}
			text2 = (this._ownerdocument.OptionOutputUpperCase ? att.Name.ToUpper() : att.Name);
			if (att.Name.Length >= 4 && att.Name[0] == '<' && att.Name[1] == '%' && att.Name[att.Name.Length - 1] == '>' && att.Name[att.Name.Length - 2] == '%')
			{
				outText.Write(" " + text2);
				return;
			}
			if (!this._ownerdocument.OptionOutputOptimizeAttributeValues)
			{
				outText.Write(string.Concat(new string[]
				{
					" ",
					text2,
					"=",
					text,
					att.Value,
					text
				}));
				return;
			}
			if (att.Value.IndexOfAny(new char[]
			{
				'\n',
				'\r',
				'\t',
				' '
			}) < 0)
			{
				outText.Write(" " + text2 + "=" + att.Value);
				return;
			}
			outText.Write(string.Concat(new string[]
			{
				" ",
				text2,
				"=",
				text,
				att.Value,
				text
			}));
		}
		internal void WriteAttributes(TextWriter outText, bool closing)
		{
			if (!this._ownerdocument.OptionOutputAsXml)
			{
				if (!closing)
				{
					if (this._attributes != null)
					{
						foreach (HtmlAttribute current in (IEnumerable<HtmlAttribute>)this._attributes)
						{
							this.WriteAttribute(outText, current);
						}
					}
					if (!this._ownerdocument.OptionAddDebuggingAttributes)
					{
						return;
					}
					this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
					this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));
					int num = 0;
					using (IEnumerator<HtmlNode> enumerator2 = ((IEnumerable<HtmlNode>)this.ChildNodes).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							HtmlNode current2 = enumerator2.Current;
							this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_child_" + num, current2.Name));
							num++;
						}
						return;
					}
				}
				if (this._endnode == null || this._endnode._attributes == null || this._endnode == this)
				{
					return;
				}
				foreach (HtmlAttribute current3 in (IEnumerable<HtmlAttribute>)this._endnode._attributes)
				{
					this.WriteAttribute(outText, current3);
				}
				if (!this._ownerdocument.OptionAddDebuggingAttributes)
				{
					return;
				}
				this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
				this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));
				return;
			}
			if (this._attributes == null)
			{
				return;
			}
			foreach (HtmlAttribute current4 in this._attributes.Hashitems.Values)
			{
				this.WriteAttribute(outText, current4);
			}
		}
		private string GetRelativeXpath()
		{
			if (this.ParentNode == null)
			{
				return this.Name;
			}
			if (this.NodeType == HtmlNodeType.Document)
			{
				return string.Empty;
			}
			int num = 1;
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)this.ParentNode.ChildNodes)
			{
				if (!(current.Name != this.Name))
				{
					if (current == this)
					{
						break;
					}
					num++;
				}
			}
			return string.Concat(new object[]
			{
				this.Name,
				"[",
				num,
				"]"
			});
		}
	}
}

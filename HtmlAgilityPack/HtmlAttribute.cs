using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace HtmlAgilityPack
{
	[DebuggerDisplay("Name: {OriginalName}, Value: {Value}")]
	public class HtmlAttribute : IComparable
	{
		private int _line;
		internal int _lineposition;
		internal string _name;
		internal int _namelength;
		internal int _namestartindex;
		internal HtmlDocument _ownerdocument;
		internal HtmlNode _ownernode;
		private AttributeValueQuote _quoteType = AttributeValueQuote.DoubleQuote;
		internal int _streamposition;
		internal string _value;
		internal int _valuelength;
		internal int _valuestartindex;
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
		}
		public string Name
		{
			get
			{
				if (this._name == null)
				{
					this._name = this._ownerdocument.Text.Substring(this._namestartindex, this._namelength);
				}
				return this._name.ToLower();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._name = value;
				if (this._ownernode != null)
				{
					this._ownernode._innerchanged = true;
					this._ownernode._outerchanged = true;
				}
			}
		}
		public string OriginalName
		{
			get
			{
				return this._name;
			}
		}
		public HtmlDocument OwnerDocument
		{
			get
			{
				return this._ownerdocument;
			}
		}
		public HtmlNode OwnerNode
		{
			get
			{
				return this._ownernode;
			}
		}
		public AttributeValueQuote QuoteType
		{
			get
			{
				return this._quoteType;
			}
			set
			{
				this._quoteType = value;
			}
		}
		public int StreamPosition
		{
			get
			{
				return this._streamposition;
			}
		}
		public string Value
		{
			get
			{
				if (this._value == null)
				{
					this._value = this._ownerdocument.Text.Substring(this._valuestartindex, this._valuelength);
				}
				return this._value;
			}
			set
			{
				this._value = value;
				if (this._ownernode != null)
				{
					this._ownernode._innerchanged = true;
					this._ownernode._outerchanged = true;
				}
			}
		}
		internal string XmlName
		{
			get
			{
				return HtmlDocument.GetXmlName(this.Name);
			}
		}
		internal string XmlValue
		{
			get
			{
				return this.Value;
			}
		}
		public string XPath
		{
			get
			{
				string str = (this.OwnerNode == null) ? "/" : (this.OwnerNode.XPath + "/");
				return str + this.GetRelativeXpath();
			}
		}
		internal HtmlAttribute(HtmlDocument ownerdocument)
		{
			this._ownerdocument = ownerdocument;
		}
		public int CompareTo(object obj)
		{
			HtmlAttribute htmlAttribute = obj as HtmlAttribute;
			if (htmlAttribute == null)
			{
				throw new ArgumentException("obj");
			}
			return this.Name.CompareTo(htmlAttribute.Name);
		}
		public HtmlAttribute Clone()
		{
			return new HtmlAttribute(this._ownerdocument)
			{
				Name = this.Name,
				Value = this.Value
			};
		}
		public void Remove()
		{
			this._ownernode.Attributes.Remove(this);
		}
		private string GetRelativeXpath()
		{
			if (this.OwnerNode == null)
			{
				return this.Name;
			}
			int num = 1;
			foreach (HtmlAttribute current in (IEnumerable<HtmlAttribute>)this.OwnerNode.Attributes)
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
				"@",
				this.Name,
				"[",
				num,
				"]"
			});
		}
	}
}

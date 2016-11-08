using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
namespace HtmlAgilityPack
{
	public class HtmlDocument : IXPathNavigable
	{
		private enum ParseState
		{
			Text,
			WhichTag,
			Tag,
			BetweenAttributes,
			EmptyTag,
			AttributeName,
			AttributeBeforeEquals,
			AttributeAfterEquals,
			AttributeValue,
			Comment,
			QuotedAttributeValue,
			ServerSideCode,
			PcData
		}
		private int _c;
		private Crc32 _crc32;
		private HtmlAttribute _currentattribute;
		private HtmlNode _currentnode;
		private Encoding _declaredencoding;
		private HtmlNode _documentnode;
		private bool _fullcomment;
		private int _index;
		internal Dictionary<string, HtmlNode> Lastnodes = new Dictionary<string, HtmlNode>();
		private HtmlNode _lastparentnode;
		private int _line;
		private int _lineposition;
		private int _maxlineposition;
		internal Dictionary<string, HtmlNode> Nodesid;
		private HtmlDocument.ParseState _oldstate;
		private bool _onlyDetectEncoding;
		internal Dictionary<int, HtmlNode> Openednodes;
		private List<HtmlParseError> _parseerrors = new List<HtmlParseError>();
		private string _remainder;
		private int _remainderOffset;
		private HtmlDocument.ParseState _state;
		private Encoding _streamencoding;
		internal string Text;
		public bool OptionAddDebuggingAttributes;
		public bool OptionAutoCloseOnEnd;
		public bool OptionCheckSyntax = true;
		public bool OptionComputeChecksum;
		public Encoding OptionDefaultStreamEncoding;
		public bool OptionExtractErrorSourceText;
		public int OptionExtractErrorSourceTextMaxLength = 100;
		public bool OptionFixNestedTags;
		public bool OptionOutputAsXml;
		public bool OptionOutputOptimizeAttributeValues;
		public bool OptionOutputOriginalCase;
		public bool OptionOutputUpperCase;
		public bool OptionReadEncoding = true;
		public string OptionStopperNodeName;
		public bool OptionUseIdAttribute = true;
		public bool OptionWriteEmptyNodes;
		internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";
		internal static readonly string HtmlExceptionUseIdAttributeFalse = "You need to set UseIdAttribute property to true to enable this feature";
		public int CheckSum
		{
			get
			{
				if (this._crc32 != null)
				{
					return (int)this._crc32.CheckSum;
				}
				return 0;
			}
		}
		public Encoding DeclaredEncoding
		{
			get
			{
				return this._declaredencoding;
			}
		}
		public HtmlNode DocumentNode
		{
			get
			{
				return this._documentnode;
			}
		}
		public Encoding Encoding
		{
			get
			{
				return this.GetOutEncoding();
			}
		}
		public IEnumerable<HtmlParseError> ParseErrors
		{
			get
			{
				return this._parseerrors;
			}
		}
		public string Remainder
		{
			get
			{
				return this._remainder;
			}
		}
		public int RemainderOffset
		{
			get
			{
				return this._remainderOffset;
			}
		}
		public Encoding StreamEncoding
		{
			get
			{
				return this._streamencoding;
			}
		}
		public void DetectEncodingAndLoad(string path)
		{
			this.DetectEncodingAndLoad(path, true);
		}
		public void DetectEncodingAndLoad(string path, bool detectEncoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			Encoding encoding;
			if (detectEncoding)
			{
				encoding = this.DetectEncoding(path);
			}
			else
			{
				encoding = null;
			}
			if (encoding == null)
			{
				this.Load(path);
				return;
			}
			this.Load(path, encoding);
		}
		public Encoding DetectEncoding(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			Encoding result;
			using (StreamReader streamReader = new StreamReader(path, this.OptionDefaultStreamEncoding))
			{
				Encoding encoding = this.DetectEncoding(streamReader);
				result = encoding;
			}
			return result;
		}
		public void Load(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			using (StreamReader streamReader = new StreamReader(path, this.OptionDefaultStreamEncoding))
			{
				this.Load(streamReader);
			}
		}
		public void Load(string path, bool detectEncodingFromByteOrderMarks)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			using (StreamReader streamReader = new StreamReader(path, detectEncodingFromByteOrderMarks))
			{
				this.Load(streamReader);
			}
		}
		public void Load(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				this.Load(streamReader);
			}
		}
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks))
			{
				this.Load(streamReader);
			}
		}
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize))
			{
				this.Load(streamReader);
			}
		}
		public void Save(string filename)
		{
			using (StreamWriter streamWriter = new StreamWriter(filename, false, this.GetOutEncoding()))
			{
				this.Save(streamWriter);
			}
		}
		public void Save(string filename, Encoding encoding)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamWriter streamWriter = new StreamWriter(filename, false, encoding))
			{
				this.Save(streamWriter);
			}
		}
		public XPathNavigator CreateNavigator()
		{
			return new HtmlNodeNavigator(this, this._documentnode);
		}
		public HtmlDocument()
		{
			this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
			this.OptionDefaultStreamEncoding = Encoding.Default;
		}
		public static string GetXmlName(string name)
		{
			string text = string.Empty;
			bool flag = true;
			for (int i = 0; i < name.Length; i++)
			{
				if ((name[i] >= 'a' && name[i] <= 'z') || (name[i] >= '0' && name[i] <= '9') || name[i] == '_' || name[i] == '-' || name[i] == '.')
				{
					text += name[i];
				}
				else
				{
					flag = false;
					byte[] bytes = Encoding.UTF8.GetBytes(new char[]
					{
						name[i]
					});
					for (int j = 0; j < bytes.Length; j++)
					{
						text += bytes[j].ToString("x2");
					}
					text += "_";
				}
			}
			if (flag)
			{
				return text;
			}
			return "_" + text;
		}
		public static string HtmlEncode(string html)
		{
			if (html == null)
			{
				throw new ArgumentNullException("html");
			}
			Regex regex = new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;))", RegexOptions.IgnoreCase);
			return regex.Replace(html, "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
		}
		public static bool IsWhiteSpace(int c)
		{
			return c == 10 || c == 13 || c == 32 || c == 9;
		}
		public HtmlAttribute CreateAttribute(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlAttribute htmlAttribute = this.CreateAttribute();
			htmlAttribute.Name = name;
			return htmlAttribute;
		}
		public HtmlAttribute CreateAttribute(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlAttribute htmlAttribute = this.CreateAttribute(name);
			htmlAttribute.Value = value;
			return htmlAttribute;
		}
		public HtmlCommentNode CreateComment()
		{
			return (HtmlCommentNode)this.CreateNode(HtmlNodeType.Comment);
		}
		public HtmlCommentNode CreateComment(string comment)
		{
			if (comment == null)
			{
				throw new ArgumentNullException("comment");
			}
			HtmlCommentNode htmlCommentNode = this.CreateComment();
			htmlCommentNode.Comment = comment;
			return htmlCommentNode;
		}
		public HtmlNode CreateElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlNode htmlNode = this.CreateNode(HtmlNodeType.Element);
			htmlNode.Name = name;
			return htmlNode;
		}
		public HtmlTextNode CreateTextNode()
		{
			return (HtmlTextNode)this.CreateNode(HtmlNodeType.Text);
		}
		public HtmlTextNode CreateTextNode(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			HtmlTextNode htmlTextNode = this.CreateTextNode();
			htmlTextNode.Text = text;
			return htmlTextNode;
		}
		public Encoding DetectEncoding(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			return this.DetectEncoding(new StreamReader(stream));
		}
		public Encoding DetectEncoding(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._onlyDetectEncoding = true;
			if (this.OptionCheckSyntax)
			{
				this.Openednodes = new Dictionary<int, HtmlNode>();
			}
			else
			{
				this.Openednodes = null;
			}
			if (this.OptionUseIdAttribute)
			{
				this.Nodesid = new Dictionary<string, HtmlNode>();
			}
			else
			{
				this.Nodesid = null;
			}
			StreamReader streamReader = reader as StreamReader;
			if (streamReader != null)
			{
				this._streamencoding = streamReader.CurrentEncoding;
			}
			else
			{
				this._streamencoding = null;
			}
			this._declaredencoding = null;
			this.Text = reader.ReadToEnd();
			this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
			try
			{
				this.Parse();
			}
			catch (EncodingFoundException ex)
			{
				return ex.Encoding;
			}
			return null;
		}
		public Encoding DetectEncodingHtml(string html)
		{
			if (html == null)
			{
				throw new ArgumentNullException("html");
			}
			Encoding result;
			using (StringReader stringReader = new StringReader(html))
			{
				Encoding encoding = this.DetectEncoding(stringReader);
				result = encoding;
			}
			return result;
		}
		public HtmlNode GetElementbyId(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (this.Nodesid == null)
			{
				throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
			}
			if (!this.Nodesid.ContainsKey(id.ToLower()))
			{
				return null;
			}
			return this.Nodesid[id.ToLower()];
		}
		public void Load(Stream stream)
		{
			this.Load(new StreamReader(stream, this.OptionDefaultStreamEncoding));
		}
		public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
		}
		public void Load(Stream stream, Encoding encoding)
		{
			this.Load(new StreamReader(stream, encoding));
		}
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
		}
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
		}
		public void Load(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._onlyDetectEncoding = false;
			if (this.OptionCheckSyntax)
			{
				this.Openednodes = new Dictionary<int, HtmlNode>();
			}
			else
			{
				this.Openednodes = null;
			}
			if (this.OptionUseIdAttribute)
			{
				this.Nodesid = new Dictionary<string, HtmlNode>();
			}
			else
			{
				this.Nodesid = null;
			}
			StreamReader streamReader = reader as StreamReader;
			if (streamReader != null)
			{
				try
				{
					streamReader.Peek();
				}
				catch (Exception)
				{
				}
				this._streamencoding = streamReader.CurrentEncoding;
			}
			else
			{
				this._streamencoding = null;
			}
			this._declaredencoding = null;
			this.Text = reader.ReadToEnd();
			this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
			this.Parse();
			if (!this.OptionCheckSyntax || this.Openednodes == null)
			{
				return;
			}
			foreach (HtmlNode current in this.Openednodes.Values)
			{
				if (current._starttag)
				{
					string text;
					if (this.OptionExtractErrorSourceText)
					{
						text = current.OuterHtml;
						if (text.Length > this.OptionExtractErrorSourceTextMaxLength)
						{
							text = text.Substring(0, this.OptionExtractErrorSourceTextMaxLength);
						}
					}
					else
					{
						text = string.Empty;
					}
					this.AddError(HtmlParseErrorCode.TagNotClosed, current._line, current._lineposition, current._streamposition, text, "End tag </" + current.Name + "> was not found");
				}
			}
			this.Openednodes.Clear();
		}
		public void LoadHtml(string html)
		{
			if (html == null)
			{
				throw new ArgumentNullException("html");
			}
			using (StringReader stringReader = new StringReader(html))
			{
				this.Load(stringReader);
			}
		}
		public void Save(Stream outStream)
		{
			StreamWriter writer = new StreamWriter(outStream, this.GetOutEncoding());
			this.Save(writer);
		}
		public void Save(Stream outStream, Encoding encoding)
		{
			if (outStream == null)
			{
				throw new ArgumentNullException("outStream");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			StreamWriter writer = new StreamWriter(outStream, encoding);
			this.Save(writer);
		}
		public void Save(StreamWriter writer)
		{
			this.Save(writer);
		}
		public void Save(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.DocumentNode.WriteTo(writer);
			writer.Flush();
		}
		public void Save(XmlWriter writer)
		{
			this.DocumentNode.WriteTo(writer);
			writer.Flush();
		}
		internal HtmlAttribute CreateAttribute()
		{
			return new HtmlAttribute(this);
		}
		internal HtmlNode CreateNode(HtmlNodeType type)
		{
			return this.CreateNode(type, -1);
		}
		internal HtmlNode CreateNode(HtmlNodeType type, int index)
		{
			switch (type)
			{
			case HtmlNodeType.Comment:
				return new HtmlCommentNode(this, index);
			case HtmlNodeType.Text:
				return new HtmlTextNode(this, index);
			default:
				return new HtmlNode(type, this, index);
			}
		}
		internal Encoding GetOutEncoding()
		{
			Encoding arg_1A_0;
			if ((arg_1A_0 = this._declaredencoding) == null)
			{
				arg_1A_0 = (this._streamencoding ?? this.OptionDefaultStreamEncoding);
			}
			return arg_1A_0;
		}
		internal HtmlNode GetXmlDeclaration()
		{
			if (!this._documentnode.HasChildNodes)
			{
				return null;
			}
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)this._documentnode._childnodes)
			{
				if (current.Name == "?xml")
				{
					return current;
				}
			}
			return null;
		}
		internal void SetIdForNode(HtmlNode node, string id)
		{
			if (!this.OptionUseIdAttribute)
			{
				return;
			}
			if (this.Nodesid == null || id == null)
			{
				return;
			}
			if (node == null)
			{
				this.Nodesid.Remove(id.ToLower());
				return;
			}
			this.Nodesid[id.ToLower()] = node;
		}
		internal void UpdateLastParentNode()
		{
			do
			{
				if (this._lastparentnode.Closed)
				{
					this._lastparentnode = this._lastparentnode.ParentNode;
				}
			}
			while (this._lastparentnode != null && this._lastparentnode.Closed);
			if (this._lastparentnode == null)
			{
				this._lastparentnode = this._documentnode;
			}
		}
		private void AddError(HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText, string reason)
		{
			HtmlParseError item = new HtmlParseError(code, line, linePosition, streamPosition, sourceText, reason);
			this._parseerrors.Add(item);
		}
		private void CloseCurrentNode()
		{
			if (this._currentnode.Closed)
			{
				return;
			}
			bool flag = false;
			HtmlNode dictionaryValueOrNull = Utilities.GetDictionaryValueOrNull<string, HtmlNode>(this.Lastnodes, this._currentnode.Name);
			if (dictionaryValueOrNull == null)
			{
				if (HtmlNode.IsClosedElement(this._currentnode.Name))
				{
					this._currentnode.CloseNode(this._currentnode);
					if (this._lastparentnode != null)
					{
						HtmlNode htmlNode = null;
						Stack<HtmlNode> stack = new Stack<HtmlNode>();
						for (HtmlNode htmlNode2 = this._lastparentnode.LastChild; htmlNode2 != null; htmlNode2 = htmlNode2.PreviousSibling)
						{
							if (htmlNode2.Name == this._currentnode.Name && !htmlNode2.HasChildNodes)
							{
								htmlNode = htmlNode2;
								break;
							}
							stack.Push(htmlNode2);
						}
						if (htmlNode != null)
						{
							while (stack.Count != 0)
							{
								HtmlNode htmlNode3 = stack.Pop();
								this._lastparentnode.RemoveChild(htmlNode3);
								htmlNode.AppendChild(htmlNode3);
							}
						}
						else
						{
							this._lastparentnode.AppendChild(this._currentnode);
						}
					}
				}
				else
				{
					if (HtmlNode.CanOverlapElement(this._currentnode.Name))
					{
						HtmlNode htmlNode4 = this.CreateNode(HtmlNodeType.Text, this._currentnode._outerstartindex);
						htmlNode4._outerlength = this._currentnode._outerlength;
						((HtmlTextNode)htmlNode4).Text = ((HtmlTextNode)htmlNode4).Text.ToLower();
						if (this._lastparentnode != null)
						{
							this._lastparentnode.AppendChild(htmlNode4);
						}
					}
					else
					{
						if (HtmlNode.IsEmptyElement(this._currentnode.Name))
						{
							this.AddError(HtmlParseErrorCode.EndTagNotRequired, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "End tag </" + this._currentnode.Name + "> is not required");
						}
						else
						{
							this.AddError(HtmlParseErrorCode.TagNotOpened, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "Start tag <" + this._currentnode.Name + "> was not found");
							flag = true;
						}
					}
				}
			}
			else
			{
				if (this.OptionFixNestedTags && this.FindResetterNodes(dictionaryValueOrNull, this.GetResetters(this._currentnode.Name)))
				{
					this.AddError(HtmlParseErrorCode.EndTagInvalidHere, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "End tag </" + this._currentnode.Name + "> invalid here");
					flag = true;
				}
				if (!flag)
				{
					this.Lastnodes[this._currentnode.Name] = dictionaryValueOrNull._prevwithsamename;
					dictionaryValueOrNull.CloseNode(this._currentnode);
				}
			}
			if (!flag && this._lastparentnode != null && (!HtmlNode.IsClosedElement(this._currentnode.Name) || this._currentnode._starttag))
			{
				this.UpdateLastParentNode();
			}
		}
		private string CurrentNodeName()
		{
			return this.Text.Substring(this._currentnode._namestartindex, this._currentnode._namelength);
		}
		private void DecrementPosition()
		{
			this._index--;
			if (this._lineposition == 1)
			{
				this._lineposition = this._maxlineposition;
				this._line--;
				return;
			}
			this._lineposition--;
		}
		private HtmlNode FindResetterNode(HtmlNode node, string name)
		{
			HtmlNode dictionaryValueOrNull = Utilities.GetDictionaryValueOrNull<string, HtmlNode>(this.Lastnodes, name);
			if (dictionaryValueOrNull == null)
			{
				return null;
			}
			if (dictionaryValueOrNull.Closed)
			{
				return null;
			}
			if (dictionaryValueOrNull._streamposition < node._streamposition)
			{
				return null;
			}
			return dictionaryValueOrNull;
		}
		private bool FindResetterNodes(HtmlNode node, string[] names)
		{
			if (names == null)
			{
				return false;
			}
			for (int i = 0; i < names.Length; i++)
			{
				if (this.FindResetterNode(node, names[i]) != null)
				{
					return true;
				}
			}
			return false;
		}
		private void FixNestedTag(string name, string[] resetters)
		{
			if (resetters == null)
			{
				return;
			}
			HtmlNode dictionaryValueOrNull = Utilities.GetDictionaryValueOrNull<string, HtmlNode>(this.Lastnodes, this._currentnode.Name);
			if (dictionaryValueOrNull == null || this.Lastnodes[name].Closed)
			{
				return;
			}
			if (this.FindResetterNodes(dictionaryValueOrNull, resetters))
			{
				return;
			}
			HtmlNode htmlNode = new HtmlNode(dictionaryValueOrNull.NodeType, this, -1);
			htmlNode._endnode = htmlNode;
			dictionaryValueOrNull.CloseNode(htmlNode);
		}
		private void FixNestedTags()
		{
			if (!this._currentnode._starttag)
			{
				return;
			}
			string name = this.CurrentNodeName();
			this.FixNestedTag(name, this.GetResetters(name));
		}
		private string[] GetResetters(string name)
		{
			if (name != null)
			{
				if (name == "li")
				{
					return new string[]
					{
						"ul"
					};
				}
				if (name == "tr")
				{
					return new string[]
					{
						"table"
					};
				}
				if (name == "th" || name == "td")
				{
					return new string[]
					{
						"tr",
						"table"
					};
				}
			}
			return null;
		}
		private void IncrementPosition()
		{
			if (this._crc32 != null)
			{
				this._crc32.AddToCRC32(this._c);
			}
			this._index++;
			this._maxlineposition = this._lineposition;
			if (this._c == 10)
			{
				this._lineposition = 1;
				this._line++;
				return;
			}
			this._lineposition++;
		}
		private bool NewCheck()
		{
			if (this._c != 60)
			{
				return false;
			}
			if (this._index < this.Text.Length && this.Text[this._index] == '%')
			{
				HtmlDocument.ParseState state = this._state;
				switch (state)
				{
				case HtmlDocument.ParseState.WhichTag:
					this.PushNodeNameStart(true, this._index - 1);
					this._state = HtmlDocument.ParseState.Tag;
					break;
				case HtmlDocument.ParseState.Tag:
					break;
				case HtmlDocument.ParseState.BetweenAttributes:
					this.PushAttributeNameStart(this._index - 1);
					break;
				default:
					if (state == HtmlDocument.ParseState.AttributeAfterEquals)
					{
						this.PushAttributeValueStart(this._index - 1);
					}
					break;
				}
				this._oldstate = this._state;
				this._state = HtmlDocument.ParseState.ServerSideCode;
				return true;
			}
			if (!this.PushNodeEnd(this._index - 1, true))
			{
				this._index = this.Text.Length;
				return true;
			}
			this._state = HtmlDocument.ParseState.WhichTag;
			if (this._index - 1 <= this.Text.Length - 2 && this.Text[this._index] == '!')
			{
				this.PushNodeStart(HtmlNodeType.Comment, this._index - 1);
				this.PushNodeNameStart(true, this._index);
				this.PushNodeNameEnd(this._index + 1);
				this._state = HtmlDocument.ParseState.Comment;
				if (this._index < this.Text.Length - 2)
				{
					if (this.Text[this._index + 1] == '-' && this.Text[this._index + 2] == '-')
					{
						this._fullcomment = true;
					}
					else
					{
						this._fullcomment = false;
					}
				}
				return true;
			}
			this.PushNodeStart(HtmlNodeType.Element, this._index - 1);
			return true;
		}
		private void Parse()
		{
			int num = 0;
			if (this.OptionComputeChecksum)
			{
				this._crc32 = new Crc32();
			}
			this.Lastnodes = new Dictionary<string, HtmlNode>();
			this._c = 0;
			this._fullcomment = false;
			this._parseerrors = new List<HtmlParseError>();
			this._line = 1;
			this._lineposition = 1;
			this._maxlineposition = 1;
			this._state = HtmlDocument.ParseState.Text;
			this._oldstate = this._state;
			this._documentnode._innerlength = this.Text.Length;
			this._documentnode._outerlength = this.Text.Length;
			this._remainderOffset = this.Text.Length;
			this._lastparentnode = this._documentnode;
			this._currentnode = this.CreateNode(HtmlNodeType.Text, 0);
			this._currentattribute = null;
			this._index = 0;
			this.PushNodeStart(HtmlNodeType.Text, 0);
			while (this._index < this.Text.Length)
			{
				this._c = (int)this.Text[this._index];
				this.IncrementPosition();
				switch (this._state)
				{
				case HtmlDocument.ParseState.Text:
					if (this.NewCheck())
					{
					}
					break;
				case HtmlDocument.ParseState.WhichTag:
					if (!this.NewCheck())
					{
						if (this._c == 47)
						{
							this.PushNodeNameStart(false, this._index);
						}
						else
						{
							this.PushNodeNameStart(true, this._index - 1);
							this.DecrementPosition();
						}
						this._state = HtmlDocument.ParseState.Tag;
					}
					break;
				case HtmlDocument.ParseState.Tag:
					if (!this.NewCheck())
					{
						if (HtmlDocument.IsWhiteSpace(this._c))
						{
							this.PushNodeNameEnd(this._index - 1);
							if (this._state == HtmlDocument.ParseState.Tag)
							{
								this._state = HtmlDocument.ParseState.BetweenAttributes;
							}
						}
						else
						{
							if (this._c == 47)
							{
								this.PushNodeNameEnd(this._index - 1);
								if (this._state == HtmlDocument.ParseState.Tag)
								{
									this._state = HtmlDocument.ParseState.EmptyTag;
								}
							}
							else
							{
								if (this._c == 62)
								{
									this.PushNodeNameEnd(this._index - 1);
									if (this._state == HtmlDocument.ParseState.Tag)
									{
										if (!this.PushNodeEnd(this._index, false))
										{
											this._index = this.Text.Length;
										}
										else
										{
											if (this._state == HtmlDocument.ParseState.Tag)
											{
												this._state = HtmlDocument.ParseState.Text;
												this.PushNodeStart(HtmlNodeType.Text, this._index);
											}
										}
									}
								}
							}
						}
					}
					break;
				case HtmlDocument.ParseState.BetweenAttributes:
					if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
					{
						if (this._c == 47 || this._c == 63)
						{
							this._state = HtmlDocument.ParseState.EmptyTag;
						}
						else
						{
							if (this._c == 62)
							{
								if (!this.PushNodeEnd(this._index, false))
								{
									this._index = this.Text.Length;
								}
								else
								{
									if (this._state == HtmlDocument.ParseState.BetweenAttributes)
									{
										this._state = HtmlDocument.ParseState.Text;
										this.PushNodeStart(HtmlNodeType.Text, this._index);
									}
								}
							}
							else
							{
								this.PushAttributeNameStart(this._index - 1);
								this._state = HtmlDocument.ParseState.AttributeName;
							}
						}
					}
					break;
				case HtmlDocument.ParseState.EmptyTag:
					if (!this.NewCheck())
					{
						if (this._c == 62)
						{
							if (!this.PushNodeEnd(this._index, true))
							{
								this._index = this.Text.Length;
							}
							else
							{
								if (this._state == HtmlDocument.ParseState.EmptyTag)
								{
									this._state = HtmlDocument.ParseState.Text;
									this.PushNodeStart(HtmlNodeType.Text, this._index);
								}
							}
						}
						else
						{
							this._state = HtmlDocument.ParseState.BetweenAttributes;
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeName:
					if (!this.NewCheck())
					{
						if (HtmlDocument.IsWhiteSpace(this._c))
						{
							this.PushAttributeNameEnd(this._index - 1);
							this._state = HtmlDocument.ParseState.AttributeBeforeEquals;
						}
						else
						{
							if (this._c == 61)
							{
								this.PushAttributeNameEnd(this._index - 1);
								this._state = HtmlDocument.ParseState.AttributeAfterEquals;
							}
							else
							{
								if (this._c == 62)
								{
									this.PushAttributeNameEnd(this._index - 1);
									if (!this.PushNodeEnd(this._index, false))
									{
										this._index = this.Text.Length;
									}
									else
									{
										if (this._state == HtmlDocument.ParseState.AttributeName)
										{
											this._state = HtmlDocument.ParseState.Text;
											this.PushNodeStart(HtmlNodeType.Text, this._index);
										}
									}
								}
							}
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeBeforeEquals:
					if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
					{
						if (this._c == 62)
						{
							if (!this.PushNodeEnd(this._index, false))
							{
								this._index = this.Text.Length;
							}
							else
							{
								if (this._state == HtmlDocument.ParseState.AttributeBeforeEquals)
								{
									this._state = HtmlDocument.ParseState.Text;
									this.PushNodeStart(HtmlNodeType.Text, this._index);
								}
							}
						}
						else
						{
							if (this._c == 61)
							{
								this._state = HtmlDocument.ParseState.AttributeAfterEquals;
							}
							else
							{
								this._state = HtmlDocument.ParseState.BetweenAttributes;
								this.DecrementPosition();
							}
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeAfterEquals:
					if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
					{
						if (this._c == 39 || this._c == 34)
						{
							this._state = HtmlDocument.ParseState.QuotedAttributeValue;
							this.PushAttributeValueStart(this._index, this._c);
							num = this._c;
						}
						else
						{
							if (this._c == 62)
							{
								if (!this.PushNodeEnd(this._index, false))
								{
									this._index = this.Text.Length;
								}
								else
								{
									if (this._state == HtmlDocument.ParseState.AttributeAfterEquals)
									{
										this._state = HtmlDocument.ParseState.Text;
										this.PushNodeStart(HtmlNodeType.Text, this._index);
									}
								}
							}
							else
							{
								this.PushAttributeValueStart(this._index - 1);
								this._state = HtmlDocument.ParseState.AttributeValue;
							}
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeValue:
					if (!this.NewCheck())
					{
						if (HtmlDocument.IsWhiteSpace(this._c))
						{
							this.PushAttributeValueEnd(this._index - 1);
							this._state = HtmlDocument.ParseState.BetweenAttributes;
						}
						else
						{
							if (this._c == 62)
							{
								this.PushAttributeValueEnd(this._index - 1);
								if (!this.PushNodeEnd(this._index, false))
								{
									this._index = this.Text.Length;
								}
								else
								{
									if (this._state == HtmlDocument.ParseState.AttributeValue)
									{
										this._state = HtmlDocument.ParseState.Text;
										this.PushNodeStart(HtmlNodeType.Text, this._index);
									}
								}
							}
						}
					}
					break;
				case HtmlDocument.ParseState.Comment:
					if (this._c == 62 && (!this._fullcomment || (this.Text[this._index - 2] == '-' && this.Text[this._index - 3] == '-')))
					{
						if (!this.PushNodeEnd(this._index, false))
						{
							this._index = this.Text.Length;
						}
						else
						{
							this._state = HtmlDocument.ParseState.Text;
							this.PushNodeStart(HtmlNodeType.Text, this._index);
						}
					}
					break;
				case HtmlDocument.ParseState.QuotedAttributeValue:
					if (this._c == num)
					{
						this.PushAttributeValueEnd(this._index - 1);
						this._state = HtmlDocument.ParseState.BetweenAttributes;
					}
					else
					{
						if (this._c == 60 && this._index < this.Text.Length && this.Text[this._index] == '%')
						{
							this._oldstate = this._state;
							this._state = HtmlDocument.ParseState.ServerSideCode;
						}
					}
					break;
				case HtmlDocument.ParseState.ServerSideCode:
					if (this._c == 37 && this._index < this.Text.Length && this.Text[this._index] == '>')
					{
						HtmlDocument.ParseState oldstate = this._oldstate;
						if (oldstate != HtmlDocument.ParseState.BetweenAttributes)
						{
							if (oldstate == HtmlDocument.ParseState.AttributeAfterEquals)
							{
								this._state = HtmlDocument.ParseState.AttributeValue;
							}
							else
							{
								this._state = this._oldstate;
							}
						}
						else
						{
							this.PushAttributeNameEnd(this._index + 1);
							this._state = HtmlDocument.ParseState.BetweenAttributes;
						}
						this.IncrementPosition();
					}
					break;
				case HtmlDocument.ParseState.PcData:
					if (this._currentnode._namelength + 3 <= this.Text.Length - (this._index - 1) && string.Compare(this.Text.Substring(this._index - 1, this._currentnode._namelength + 2), "</" + this._currentnode.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						int num2 = (int)this.Text[this._index - 1 + 2 + this._currentnode.Name.Length];
						if (num2 == 62 || HtmlDocument.IsWhiteSpace(num2))
						{
							HtmlNode htmlNode = this.CreateNode(HtmlNodeType.Text, this._currentnode._outerstartindex + this._currentnode._outerlength);
							htmlNode._outerlength = this._index - 1 - htmlNode._outerstartindex;
							this._currentnode.AppendChild(htmlNode);
							this.PushNodeStart(HtmlNodeType.Element, this._index - 1);
							this.PushNodeNameStart(false, this._index - 1 + 2);
							this._state = HtmlDocument.ParseState.Tag;
							this.IncrementPosition();
						}
					}
					break;
				}
			}
			if (this._currentnode._namestartindex > 0)
			{
				this.PushNodeNameEnd(this._index);
			}
			this.PushNodeEnd(this._index, false);
			this.Lastnodes.Clear();
		}
		private void PushAttributeNameEnd(int index)
		{
			this._currentattribute._namelength = index - this._currentattribute._namestartindex;
			this._currentnode.Attributes.Append(this._currentattribute);
		}
		private void PushAttributeNameStart(int index)
		{
			this._currentattribute = this.CreateAttribute();
			this._currentattribute._namestartindex = index;
			this._currentattribute.Line = this._line;
			this._currentattribute._lineposition = this._lineposition;
			this._currentattribute._streamposition = index;
		}
		private void PushAttributeValueEnd(int index)
		{
			this._currentattribute._valuelength = index - this._currentattribute._valuestartindex;
		}
		private void PushAttributeValueStart(int index)
		{
			this.PushAttributeValueStart(index, 0);
		}
		private void PushAttributeValueStart(int index, int quote)
		{
			this._currentattribute._valuestartindex = index;
			if (quote == 39)
			{
				this._currentattribute.QuoteType = AttributeValueQuote.SingleQuote;
			}
		}
		private bool PushNodeEnd(int index, bool close)
		{
			this._currentnode._outerlength = index - this._currentnode._outerstartindex;
			if (this._currentnode._nodetype == HtmlNodeType.Text || this._currentnode._nodetype == HtmlNodeType.Comment)
			{
				if (this._currentnode._outerlength > 0)
				{
					this._currentnode._innerlength = this._currentnode._outerlength;
					this._currentnode._innerstartindex = this._currentnode._outerstartindex;
					if (this._lastparentnode != null)
					{
						this._lastparentnode.AppendChild(this._currentnode);
					}
				}
			}
			else
			{
				if (this._currentnode._starttag && this._lastparentnode != this._currentnode)
				{
					if (this._lastparentnode != null)
					{
						this._lastparentnode.AppendChild(this._currentnode);
					}
					this.ReadDocumentEncoding(this._currentnode);
					HtmlNode dictionaryValueOrNull = Utilities.GetDictionaryValueOrNull<string, HtmlNode>(this.Lastnodes, this._currentnode.Name);
					this._currentnode._prevwithsamename = dictionaryValueOrNull;
					this.Lastnodes[this._currentnode.Name] = this._currentnode;
					if (this._currentnode.NodeType == HtmlNodeType.Document || this._currentnode.NodeType == HtmlNodeType.Element)
					{
						this._lastparentnode = this._currentnode;
					}
					if (HtmlNode.IsCDataElement(this.CurrentNodeName()))
					{
						this._state = HtmlDocument.ParseState.PcData;
						return true;
					}
					if (HtmlNode.IsClosedElement(this._currentnode.Name) || HtmlNode.IsEmptyElement(this._currentnode.Name))
					{
						close = true;
					}
				}
			}
			if (close || !this._currentnode._starttag)
			{
				if (this.OptionStopperNodeName != null && this._remainder == null && string.Compare(this._currentnode.Name, this.OptionStopperNodeName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this._remainderOffset = index;
					this._remainder = this.Text.Substring(this._remainderOffset);
					this.CloseCurrentNode();
					return false;
				}
				this.CloseCurrentNode();
			}
			return true;
		}
		private void PushNodeNameEnd(int index)
		{
			this._currentnode._namelength = index - this._currentnode._namestartindex;
			if (this.OptionFixNestedTags)
			{
				this.FixNestedTags();
			}
		}
		private void PushNodeNameStart(bool starttag, int index)
		{
			this._currentnode._starttag = starttag;
			this._currentnode._namestartindex = index;
		}
		private void PushNodeStart(HtmlNodeType type, int index)
		{
			this._currentnode = this.CreateNode(type, index);
			this._currentnode._line = this._line;
			this._currentnode._lineposition = this._lineposition;
			if (type == HtmlNodeType.Element)
			{
				this._currentnode._lineposition--;
			}
			this._currentnode._streamposition = index;
		}
		private void ReadDocumentEncoding(HtmlNode node)
		{
			if (!this.OptionReadEncoding)
			{
				return;
			}
			if (node._namelength != 4)
			{
				return;
			}
			if (node.Name != "meta")
			{
				return;
			}
			HtmlAttribute htmlAttribute = node.Attributes["http-equiv"];
			if (htmlAttribute == null)
			{
				return;
			}
			if (string.Compare(htmlAttribute.Value, "content-type", StringComparison.OrdinalIgnoreCase) != 0)
			{
				return;
			}
			HtmlAttribute htmlAttribute2 = node.Attributes["content"];
			if (htmlAttribute2 != null)
			{
				string text = NameValuePairList.GetNameValuePairsValue(htmlAttribute2.Value, "charset");
				if (!string.IsNullOrEmpty(text))
				{
					if (string.Equals(text, "utf8", StringComparison.OrdinalIgnoreCase))
					{
						text = "utf-8";
					}
					try
					{
						this._declaredencoding = Encoding.GetEncoding(text);
					}
					catch (ArgumentException)
					{
						this._declaredencoding = null;
					}
					if (this._onlyDetectEncoding)
					{
						throw new EncodingFoundException(this._declaredencoding);
					}
					if (this._streamencoding != null && this._declaredencoding != null && this._declaredencoding.WindowsCodePage != this._streamencoding.WindowsCodePage)
					{
						this.AddError(HtmlParseErrorCode.CharsetMismatch, this._line, this._lineposition, this._index, node.OuterHtml, "Encoding mismatch between StreamEncoding: " + this._streamencoding.WebName + " and DeclaredEncoding: " + this._declaredencoding.WebName);
					}
				}
			}
		}
	}
}

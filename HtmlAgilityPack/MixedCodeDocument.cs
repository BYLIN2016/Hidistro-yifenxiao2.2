using System;
using System.IO;
using System.Text;
namespace HtmlAgilityPack
{
	public class MixedCodeDocument
	{
		private enum ParseState
		{
			Text,
			Code
		}
		private int _c;
		internal MixedCodeDocumentFragmentList _codefragments;
		private MixedCodeDocumentFragment _currentfragment;
		internal MixedCodeDocumentFragmentList _fragments;
		private int _index;
		private int _line;
		private int _lineposition;
		private MixedCodeDocument.ParseState _state;
		private Encoding _streamencoding;
		internal string _text;
		internal MixedCodeDocumentFragmentList _textfragments;
		public string TokenCodeEnd = "%>";
		public string TokenCodeStart = "<%";
		public string TokenDirective = "@";
		public string TokenResponseWrite = "Response.Write ";
		private string TokenTextBlock = "TextBlock({0})";
		public string Code
		{
			get
			{
				string text = "";
				int num = 0;
				foreach (MixedCodeDocumentFragment current in this._fragments)
				{
					switch (current._type)
					{
					case MixedCodeDocumentFragmentType.Code:
						text = text + ((MixedCodeDocumentCodeFragment)current).Code + "\n";
						break;
					case MixedCodeDocumentFragmentType.Text:
						text = text + this.TokenResponseWrite + string.Format(this.TokenTextBlock, num) + "\n";
						num++;
						break;
					}
				}
				return text;
			}
		}
		public MixedCodeDocumentFragmentList CodeFragments
		{
			get
			{
				return this._codefragments;
			}
		}
		public MixedCodeDocumentFragmentList Fragments
		{
			get
			{
				return this._fragments;
			}
		}
		public Encoding StreamEncoding
		{
			get
			{
				return this._streamencoding;
			}
		}
		public MixedCodeDocumentFragmentList TextFragments
		{
			get
			{
				return this._textfragments;
			}
		}
		public MixedCodeDocument()
		{
			this._codefragments = new MixedCodeDocumentFragmentList(this);
			this._textfragments = new MixedCodeDocumentFragmentList(this);
			this._fragments = new MixedCodeDocumentFragmentList(this);
		}
		public MixedCodeDocumentCodeFragment CreateCodeFragment()
		{
			return (MixedCodeDocumentCodeFragment)this.CreateFragment(MixedCodeDocumentFragmentType.Code);
		}
		public MixedCodeDocumentTextFragment CreateTextFragment()
		{
			return (MixedCodeDocumentTextFragment)this.CreateFragment(MixedCodeDocumentFragmentType.Text);
		}
		public void Load(Stream stream)
		{
			this.Load(new StreamReader(stream));
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
		public void Load(string path)
		{
			this.Load(new StreamReader(path));
		}
		public void Load(string path, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(path, detectEncodingFromByteOrderMarks));
		}
		public void Load(string path, Encoding encoding)
		{
			this.Load(new StreamReader(path, encoding));
		}
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks));
		}
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			this.Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize));
		}
		public void Load(TextReader reader)
		{
			this._codefragments.Clear();
			this._textfragments.Clear();
			StreamReader streamReader = reader as StreamReader;
			if (streamReader != null)
			{
				this._streamencoding = streamReader.CurrentEncoding;
			}
			this._text = reader.ReadToEnd();
			reader.Close();
			this.Parse();
		}
		public void LoadHtml(string html)
		{
			this.Load(new StringReader(html));
		}
		public void Save(Stream outStream)
		{
			StreamWriter writer = new StreamWriter(outStream, this.GetOutEncoding());
			this.Save(writer);
		}
		public void Save(Stream outStream, Encoding encoding)
		{
			StreamWriter writer = new StreamWriter(outStream, encoding);
			this.Save(writer);
		}
		public void Save(string filename)
		{
			StreamWriter writer = new StreamWriter(filename, false, this.GetOutEncoding());
			this.Save(writer);
		}
		public void Save(string filename, Encoding encoding)
		{
			StreamWriter writer = new StreamWriter(filename, false, encoding);
			this.Save(writer);
		}
		public void Save(StreamWriter writer)
		{
			this.Save(writer);
		}
		public void Save(TextWriter writer)
		{
			writer.Flush();
		}
		internal MixedCodeDocumentFragment CreateFragment(MixedCodeDocumentFragmentType type)
		{
			switch (type)
			{
			case MixedCodeDocumentFragmentType.Code:
				return new MixedCodeDocumentCodeFragment(this);
			case MixedCodeDocumentFragmentType.Text:
				return new MixedCodeDocumentTextFragment(this);
			default:
				throw new NotSupportedException();
			}
		}
		internal Encoding GetOutEncoding()
		{
			if (this._streamencoding != null)
			{
				return this._streamencoding;
			}
			return Encoding.UTF8;
		}
		private void IncrementPosition()
		{
			this._index++;
			if (this._c == 10)
			{
				this._lineposition = 1;
				this._line++;
				return;
			}
			this._lineposition++;
		}
		private void Parse()
		{
			this._state = MixedCodeDocument.ParseState.Text;
			this._index = 0;
			this._currentfragment = this.CreateFragment(MixedCodeDocumentFragmentType.Text);
			while (this._index < this._text.Length)
			{
				this._c = (int)this._text[this._index];
				this.IncrementPosition();
				switch (this._state)
				{
				case MixedCodeDocument.ParseState.Text:
					if (this._index + this.TokenCodeStart.Length < this._text.Length && this._text.Substring(this._index - 1, this.TokenCodeStart.Length) == this.TokenCodeStart)
					{
						this._state = MixedCodeDocument.ParseState.Code;
						this._currentfragment.Length = this._index - 1 - this._currentfragment.Index;
						this._currentfragment = this.CreateFragment(MixedCodeDocumentFragmentType.Code);
						this.SetPosition();
					}
					break;
				case MixedCodeDocument.ParseState.Code:
					if (this._index + this.TokenCodeEnd.Length < this._text.Length && this._text.Substring(this._index - 1, this.TokenCodeEnd.Length) == this.TokenCodeEnd)
					{
						this._state = MixedCodeDocument.ParseState.Text;
						this._currentfragment.Length = this._index + this.TokenCodeEnd.Length - this._currentfragment.Index;
						this._index += this.TokenCodeEnd.Length;
						this._lineposition += this.TokenCodeEnd.Length;
						this._currentfragment = this.CreateFragment(MixedCodeDocumentFragmentType.Text);
						this.SetPosition();
					}
					break;
				}
			}
			this._currentfragment.Length = this._index - this._currentfragment.Index;
		}
		private void SetPosition()
		{
			this._currentfragment.Line = this._line;
			this._currentfragment._lineposition = this._lineposition;
			this._currentfragment.Index = this._index - 1;
			this._currentfragment.Length = 0;
		}
	}
}

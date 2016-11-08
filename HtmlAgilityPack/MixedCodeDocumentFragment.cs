using System;
namespace HtmlAgilityPack
{
	public abstract class MixedCodeDocumentFragment
	{
		internal MixedCodeDocument Doc;
		private string _fragmentText;
		internal int Index;
		internal int Length;
		private int _line;
		internal int _lineposition;
		internal MixedCodeDocumentFragmentType _type;
		public string FragmentText
		{
			get
			{
				if (this._fragmentText == null)
				{
					this._fragmentText = this.Doc._text.Substring(this.Index, this.Length);
				}
				return this.FragmentText;
			}
			internal set
			{
				this._fragmentText = value;
			}
		}
		public MixedCodeDocumentFragmentType FragmentType
		{
			get
			{
				return this._type;
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
		}
		public int StreamPosition
		{
			get
			{
				return this.Index;
			}
		}
		internal MixedCodeDocumentFragment(MixedCodeDocument doc, MixedCodeDocumentFragmentType type)
		{
			this.Doc = doc;
			this._type = type;
			switch (type)
			{
			case MixedCodeDocumentFragmentType.Code:
				this.Doc._codefragments.Append(this);
				break;
			case MixedCodeDocumentFragmentType.Text:
				this.Doc._textfragments.Append(this);
				break;
			}
			this.Doc._fragments.Append(this);
		}
	}
}

using System;
namespace HtmlAgilityPack
{
	public class MixedCodeDocumentCodeFragment : MixedCodeDocumentFragment
	{
		private string _code;
		public string Code
		{
			get
			{
				if (this._code == null)
				{
					this._code = base.FragmentText.Substring(this.Doc.TokenCodeStart.Length, base.FragmentText.Length - this.Doc.TokenCodeEnd.Length - this.Doc.TokenCodeStart.Length - 1).Trim();
					if (this._code.StartsWith("="))
					{
						this._code = this.Doc.TokenResponseWrite + this._code.Substring(1, this._code.Length - 1);
					}
				}
				return this._code;
			}
			set
			{
				this._code = value;
			}
		}
		internal MixedCodeDocumentCodeFragment(MixedCodeDocument doc) : base(doc, MixedCodeDocumentFragmentType.Code)
		{
		}
	}
}

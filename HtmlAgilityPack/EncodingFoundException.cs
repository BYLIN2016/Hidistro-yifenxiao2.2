using System;
using System.Text;
namespace HtmlAgilityPack
{
	internal class EncodingFoundException : Exception
	{
		private Encoding _encoding;
		internal Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
		}
		internal EncodingFoundException(Encoding encoding)
		{
			this._encoding = encoding;
		}
	}
}

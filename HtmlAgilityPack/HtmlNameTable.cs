using System;
using System.Xml;
namespace HtmlAgilityPack
{
	internal class HtmlNameTable : XmlNameTable
	{
		private NameTable _nametable = new NameTable();
		public override string Add(string array)
		{
			return this._nametable.Add(array);
		}
		public override string Add(char[] array, int offset, int length)
		{
			return this._nametable.Add(array, offset, length);
		}
		public override string Get(string array)
		{
			return this._nametable.Get(array);
		}
		public override string Get(char[] array, int offset, int length)
		{
			return this._nametable.Get(array, offset, length);
		}
		internal string GetOrAdd(string array)
		{
			string text = this.Get(array);
			if (text == null)
			{
				return this.Add(array);
			}
			return text;
		}
	}
}

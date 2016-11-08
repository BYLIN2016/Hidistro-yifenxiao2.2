using System;
using System.Collections.Generic;
namespace HtmlAgilityPack
{
	internal class NameValuePairList
	{
		internal readonly string Text;
		private List<KeyValuePair<string, string>> _allPairs;
		private Dictionary<string, List<KeyValuePair<string, string>>> _pairsWithName;
		internal NameValuePairList() : this(null)
		{
		}
		internal NameValuePairList(string text)
		{
			this.Text = text;
			this._allPairs = new List<KeyValuePair<string, string>>();
			this._pairsWithName = new Dictionary<string, List<KeyValuePair<string, string>>>();
			this.Parse(text);
		}
		internal static string GetNameValuePairsValue(string text, string name)
		{
			NameValuePairList nameValuePairList = new NameValuePairList(text);
			return nameValuePairList.GetNameValuePairValue(name);
		}
		internal List<KeyValuePair<string, string>> GetNameValuePairs(string name)
		{
			if (name == null)
			{
				return this._allPairs;
			}
			if (!this._pairsWithName.ContainsKey(name))
			{
				return new List<KeyValuePair<string, string>>();
			}
			return this._pairsWithName[name];
		}
		internal string GetNameValuePairValue(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			List<KeyValuePair<string, string>> nameValuePairs = this.GetNameValuePairs(name);
			if (nameValuePairs.Count == 0)
			{
				return string.Empty;
			}
			return nameValuePairs[0].Value.Trim();
		}
		private void Parse(string text)
		{
			this._allPairs.Clear();
			this._pairsWithName.Clear();
			if (text == null)
			{
				return;
			}
			string[] array = text.Split(new char[]
			{
				';'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				if (text2.Length != 0)
				{
					string[] array3 = text2.Split(new char[]
					{
						'='
					}, 2);
					if (array3.Length != 0)
					{
						KeyValuePair<string, string> item = new KeyValuePair<string, string>(array3[0].Trim().ToLower(), (array3.Length < 2) ? "" : array3[1]);
						this._allPairs.Add(item);
						List<KeyValuePair<string, string>> list;
						if (!this._pairsWithName.ContainsKey(item.Key))
						{
							list = new List<KeyValuePair<string, string>>();
							this._pairsWithName[item.Key] = list;
						}
						else
						{
							list = this._pairsWithName[item.Key];
						}
						list.Add(item);
					}
				}
			}
		}
	}
}

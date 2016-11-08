using System;
using System.Collections;
using System.Collections.Generic;
namespace HtmlAgilityPack
{
	public class HtmlAttributeCollection : IList<HtmlAttribute>, ICollection<HtmlAttribute>, IEnumerable<HtmlAttribute>, IEnumerable
	{
		internal Dictionary<string, HtmlAttribute> Hashitems = new Dictionary<string, HtmlAttribute>();
		private HtmlNode _ownernode;
		private List<HtmlAttribute> items = new List<HtmlAttribute>();
		public HtmlAttribute this[string name]
		{
			get
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				HtmlAttribute result;
				if (!this.Hashitems.TryGetValue(name.ToLower(), out result))
				{
					return null;
				}
				return result;
			}
			set
			{
				this.Append(value);
			}
		}
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		public HtmlAttribute this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				this.items[index] = value;
			}
		}
		internal HtmlAttributeCollection(HtmlNode ownernode)
		{
			this._ownernode = ownernode;
		}
		public void Add(HtmlAttribute item)
		{
			this.Append(item);
		}
		void ICollection<HtmlAttribute>.Clear()
		{
			this.items.Clear();
		}
		public bool Contains(HtmlAttribute item)
		{
			return this.items.Contains(item);
		}
		public void CopyTo(HtmlAttribute[] array, int arrayIndex)
		{
			this.items.CopyTo(array, arrayIndex);
		}
		IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		public int IndexOf(HtmlAttribute item)
		{
			return this.items.IndexOf(item);
		}
		public void Insert(int index, HtmlAttribute item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.Hashitems[item.Name] = item;
			item._ownernode = this._ownernode;
			this.items.Insert(index, item);
			this._ownernode._innerchanged = true;
			this._ownernode._outerchanged = true;
		}
		bool ICollection<HtmlAttribute>.Remove(HtmlAttribute item)
		{
			return this.items.Remove(item);
		}
		public void RemoveAt(int index)
		{
			HtmlAttribute htmlAttribute = this.items[index];
			this.Hashitems.Remove(htmlAttribute.Name);
			this.items.RemoveAt(index);
			this._ownernode._innerchanged = true;
			this._ownernode._outerchanged = true;
		}
		public void Add(string name, string value)
		{
			this.Append(name, value);
		}
		public HtmlAttribute Append(HtmlAttribute newAttribute)
		{
			if (newAttribute == null)
			{
				throw new ArgumentNullException("newAttribute");
			}
			this.Hashitems[newAttribute.Name] = newAttribute;
			newAttribute._ownernode = this._ownernode;
			this.items.Add(newAttribute);
			this._ownernode._innerchanged = true;
			this._ownernode._outerchanged = true;
			return newAttribute;
		}
		public HtmlAttribute Append(string name)
		{
			HtmlAttribute newAttribute = this._ownernode._ownerdocument.CreateAttribute(name);
			return this.Append(newAttribute);
		}
		public HtmlAttribute Append(string name, string value)
		{
			HtmlAttribute newAttribute = this._ownernode._ownerdocument.CreateAttribute(name, value);
			return this.Append(newAttribute);
		}
		public bool Contains(string name)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].Name.Equals(name.ToLower()))
				{
					return true;
				}
			}
			return false;
		}
		public HtmlAttribute Prepend(HtmlAttribute newAttribute)
		{
			this.Insert(0, newAttribute);
			return newAttribute;
		}
		public void Remove(HtmlAttribute attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			int attributeIndex = this.GetAttributeIndex(attribute);
			if (attributeIndex == -1)
			{
				throw new IndexOutOfRangeException();
			}
			this.RemoveAt(attributeIndex);
		}
		public void Remove(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			string b = name.ToLower();
			for (int i = 0; i < this.items.Count; i++)
			{
				HtmlAttribute htmlAttribute = this.items[i];
				if (htmlAttribute.Name == b)
				{
					this.RemoveAt(i);
				}
			}
		}
		public void RemoveAll()
		{
			this.Hashitems.Clear();
			this.items.Clear();
			this._ownernode._innerchanged = true;
			this._ownernode._outerchanged = true;
		}
		public IEnumerable<HtmlAttribute> AttributesWithName(string attributeName)
		{
			attributeName = attributeName.ToLower();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].Name.Equals(attributeName))
				{
					yield return this.items[i];
				}
			}
			yield break;
		}
		public void Remove()
		{
			foreach (HtmlAttribute current in this.items)
			{
				current.Remove();
			}
		}
		internal void Clear()
		{
			this.Hashitems.Clear();
			this.items.Clear();
		}
		internal int GetAttributeIndex(HtmlAttribute attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == attribute)
				{
					return i;
				}
			}
			return -1;
		}
		internal int GetAttributeIndex(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			string b = name.ToLower();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].Name == b)
				{
					return i;
				}
			}
			return -1;
		}
	}
}

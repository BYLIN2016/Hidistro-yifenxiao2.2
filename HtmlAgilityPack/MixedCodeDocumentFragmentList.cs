using System;
using System.Collections;
using System.Collections.Generic;
namespace HtmlAgilityPack
{
	public class MixedCodeDocumentFragmentList : IEnumerable
	{
		public class MixedCodeDocumentFragmentEnumerator : IEnumerator
		{
			private int _index;
			private IList<MixedCodeDocumentFragment> _items;
			public MixedCodeDocumentFragment Current
			{
				get
				{
					return this._items[this._index];
				}
			}
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}
			internal MixedCodeDocumentFragmentEnumerator(IList<MixedCodeDocumentFragment> items)
			{
				this._items = items;
				this._index = -1;
			}
			public bool MoveNext()
			{
				this._index++;
				return this._index < this._items.Count;
			}
			public void Reset()
			{
				this._index = -1;
			}
		}
		private MixedCodeDocument _doc;
		private IList<MixedCodeDocumentFragment> _items = new List<MixedCodeDocumentFragment>();
		public MixedCodeDocument Doc
		{
			get
			{
				return this._doc;
			}
		}
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}
		public MixedCodeDocumentFragment this[int index]
		{
			get
			{
				return this._items[index];
			}
		}
		internal MixedCodeDocumentFragmentList(MixedCodeDocument doc)
		{
			this._doc = doc;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		public void Append(MixedCodeDocumentFragment newFragment)
		{
			if (newFragment == null)
			{
				throw new ArgumentNullException("newFragment");
			}
			this._items.Add(newFragment);
		}
		public MixedCodeDocumentFragmentList.MixedCodeDocumentFragmentEnumerator GetEnumerator()
		{
			return new MixedCodeDocumentFragmentList.MixedCodeDocumentFragmentEnumerator(this._items);
		}
		public void Prepend(MixedCodeDocumentFragment newFragment)
		{
			if (newFragment == null)
			{
				throw new ArgumentNullException("newFragment");
			}
			this._items.Insert(0, newFragment);
		}
		public void Remove(MixedCodeDocumentFragment fragment)
		{
			if (fragment == null)
			{
				throw new ArgumentNullException("fragment");
			}
			int fragmentIndex = this.GetFragmentIndex(fragment);
			if (fragmentIndex == -1)
			{
				throw new IndexOutOfRangeException();
			}
			this.RemoveAt(fragmentIndex);
		}
		public void RemoveAll()
		{
			this._items.Clear();
		}
		public void RemoveAt(int index)
		{
			this._items.RemoveAt(index);
		}
		internal void Clear()
		{
			this._items.Clear();
		}
		internal int GetFragmentIndex(MixedCodeDocumentFragment fragment)
		{
			if (fragment == null)
			{
				throw new ArgumentNullException("fragment");
			}
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i] == fragment)
				{
					return i;
				}
			}
			return -1;
		}
	}
}

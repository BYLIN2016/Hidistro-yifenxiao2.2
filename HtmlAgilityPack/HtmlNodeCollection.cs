using System;
using System.Collections;
using System.Collections.Generic;
namespace HtmlAgilityPack
{
	public class HtmlNodeCollection : IList<HtmlNode>, ICollection<HtmlNode>, IEnumerable<HtmlNode>, IEnumerable
	{
		private readonly HtmlNode _parentnode;
		private readonly List<HtmlNode> _items = new List<HtmlNode>();
		public int this[HtmlNode node]
		{
			get
			{
				int nodeIndex = this.GetNodeIndex(node);
				if (nodeIndex == -1)
				{
					throw new ArgumentOutOfRangeException("node", "Node \"" + node.CloneNode(false).OuterHtml + "\" was not found in the collection");
				}
				return nodeIndex;
			}
		}
		public HtmlNode this[string nodeName]
		{
			get
			{
				nodeName = nodeName.ToLower();
				for (int i = 0; i < this._items.Count; i++)
				{
					if (this._items[i].Name.Equals(nodeName))
					{
						return this._items[i];
					}
				}
				return null;
			}
		}
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		public HtmlNode this[int index]
		{
			get
			{
				return this._items[index];
			}
			set
			{
				this._items[index] = value;
			}
		}
		public HtmlNodeCollection(HtmlNode parentnode)
		{
			this._parentnode = parentnode;
		}
		public void Add(HtmlNode node)
		{
			this._items.Add(node);
		}
		public void Clear()
		{
			foreach (HtmlNode current in this._items)
			{
				current.ParentNode = null;
				current.NextSibling = null;
				current.PreviousSibling = null;
			}
			this._items.Clear();
		}
		public bool Contains(HtmlNode item)
		{
			return this._items.Contains(item);
		}
		public void CopyTo(HtmlNode[] array, int arrayIndex)
		{
			this._items.CopyTo(array, arrayIndex);
		}
		IEnumerator<HtmlNode> IEnumerable<HtmlNode>.GetEnumerator()
		{
			return this._items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._items.GetEnumerator();
		}
		public int IndexOf(HtmlNode item)
		{
			return this._items.IndexOf(item);
		}
		public void Insert(int index, HtmlNode node)
		{
			HtmlNode htmlNode = null;
			HtmlNode htmlNode2 = null;
			if (index > 0)
			{
				htmlNode2 = this._items[index - 1];
			}
			if (index < this._items.Count)
			{
				htmlNode = this._items[index];
			}
			this._items.Insert(index, node);
			if (htmlNode2 != null)
			{
				if (node == htmlNode2)
				{
					throw new InvalidProgramException("Unexpected error.");
				}
				htmlNode2._nextnode = node;
			}
			if (htmlNode != null)
			{
				htmlNode._prevnode = node;
			}
			node._prevnode = htmlNode2;
			if (htmlNode == node)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			node._nextnode = htmlNode;
			node._parentnode = this._parentnode;
		}
		public bool Remove(HtmlNode item)
		{
			int index = this._items.IndexOf(item);
			this.RemoveAt(index);
			return true;
		}
		public void RemoveAt(int index)
		{
			HtmlNode htmlNode = null;
			HtmlNode htmlNode2 = null;
			HtmlNode htmlNode3 = this._items[index];
			if (index > 0)
			{
				htmlNode2 = this._items[index - 1];
			}
			if (index < this._items.Count - 1)
			{
				htmlNode = this._items[index + 1];
			}
			this._items.RemoveAt(index);
			if (htmlNode2 != null)
			{
				if (htmlNode == htmlNode2)
				{
					throw new InvalidProgramException("Unexpected error.");
				}
				htmlNode2._nextnode = htmlNode;
			}
			if (htmlNode != null)
			{
				htmlNode._prevnode = htmlNode2;
			}
			htmlNode3._prevnode = null;
			htmlNode3._nextnode = null;
			htmlNode3._parentnode = null;
		}
		public static HtmlNode FindFirst(HtmlNodeCollection items, string name)
		{
			foreach (HtmlNode current in (IEnumerable<HtmlNode>)items)
			{
				if (current.Name.ToLower().Contains(name))
				{
					HtmlNode result = current;
					return result;
				}
				if (current.HasChildNodes)
				{
					HtmlNode htmlNode = HtmlNodeCollection.FindFirst(current.ChildNodes, name);
					if (htmlNode != null)
					{
						HtmlNode result = htmlNode;
						return result;
					}
				}
			}
			return null;
		}
		public void Append(HtmlNode node)
		{
			HtmlNode htmlNode = null;
			if (this._items.Count > 0)
			{
				htmlNode = this._items[this._items.Count - 1];
			}
			this._items.Add(node);
			node._prevnode = htmlNode;
			node._nextnode = null;
			node._parentnode = this._parentnode;
			if (htmlNode == null)
			{
				return;
			}
			if (htmlNode == node)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			htmlNode._nextnode = node;
		}
		public HtmlNode FindFirst(string name)
		{
			return HtmlNodeCollection.FindFirst(this, name);
		}
		public int GetNodeIndex(HtmlNode node)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (node == this._items[i])
				{
					return i;
				}
			}
			return -1;
		}
		public void Prepend(HtmlNode node)
		{
			HtmlNode htmlNode = null;
			if (this._items.Count > 0)
			{
				htmlNode = this._items[0];
			}
			this._items.Insert(0, node);
			if (node == htmlNode)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			node._nextnode = htmlNode;
			node._prevnode = null;
			node._parentnode = this._parentnode;
			if (htmlNode != null)
			{
				htmlNode._prevnode = node;
			}
		}
		public bool Remove(int index)
		{
			this.RemoveAt(index);
			return true;
		}
		public void Replace(int index, HtmlNode node)
		{
			HtmlNode htmlNode = null;
			HtmlNode htmlNode2 = null;
			HtmlNode htmlNode3 = this._items[index];
			if (index > 0)
			{
				htmlNode2 = this._items[index - 1];
			}
			if (index < this._items.Count - 1)
			{
				htmlNode = this._items[index + 1];
			}
			this._items[index] = node;
			if (htmlNode2 != null)
			{
				if (node == htmlNode2)
				{
					throw new InvalidProgramException("Unexpected error.");
				}
				htmlNode2._nextnode = node;
			}
			if (htmlNode != null)
			{
				htmlNode._prevnode = node;
			}
			node._prevnode = htmlNode2;
			if (htmlNode == node)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			node._nextnode = htmlNode;
			node._parentnode = this._parentnode;
			htmlNode3._prevnode = null;
			htmlNode3._nextnode = null;
			htmlNode3._parentnode = null;
		}
		public IEnumerable<HtmlNode> Descendants()
		{
			foreach (HtmlNode current in this._items)
			{
				foreach (HtmlNode current2 in current.Descendants())
				{
					yield return current2;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> Descendants(string name)
		{
			foreach (HtmlNode current in this._items)
			{
				foreach (HtmlNode current2 in current.Descendants(name))
				{
					yield return current2;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> Elements()
		{
			foreach (HtmlNode current in this._items)
			{
				foreach (HtmlNode current2 in (IEnumerable<HtmlNode>)current.ChildNodes)
				{
					yield return current2;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> Elements(string name)
		{
			foreach (HtmlNode current in this._items)
			{
				foreach (HtmlNode current2 in current.Elements(name))
				{
					yield return current2;
				}
			}
			yield break;
		}
		public IEnumerable<HtmlNode> Nodes()
		{
			foreach (HtmlNode current in this._items)
			{
				foreach (HtmlNode current2 in (IEnumerable<HtmlNode>)current.ChildNodes)
				{
					yield return current2;
				}
			}
			yield break;
		}
	}
}

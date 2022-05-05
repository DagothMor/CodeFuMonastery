using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures
{

	public class Node<T>
	{
		public T value;
		public Node<T> next, prev;

		public Node(T _value)
		{
			value = _value;
			next = null;
			prev = null;
		}
	}

	public class OrderedList<T>
	{
		public Node<T> head, tail;
		private bool _ascending;

		public OrderedList(bool asc)
		{
			head = null;
			tail = null;
			_ascending = asc;
		}

		public int Compare(T v1, T v2)
		{
			int result = 0;
			if (typeof(T) == typeof(String))
			{
				result = FirstValueIsBiggerByLexicographicOrder(v1.ToString().Trim(), v2.ToString().Trim());
			}
			else
			{
				var intFirstVar = Int32.Parse(v1.ToString());
				var intSecondVar = Int32.Parse(v2.ToString());
				if (intFirstVar < intSecondVar) result = -1;
				if (intFirstVar > intSecondVar) result = 1;
			}
			return result;
		}

		public void Add(T value)
		{
			var newNode = new Node<T>(value);
			var node = head;
			int isAscending = _ascending ? 1 : -1;
			if (node == null)
			{
				head = newNode;
				head.next = null;
				head.prev = null;
				tail = newNode;
				return;
			}
				if (Compare(head.value, newNode.value) == isAscending)
				{
					newNode.next = node;
					node.prev = newNode;
					head = newNode;
					return;
				}
				while (node.next != null)
				{
					if (Compare(node.value, newNode.value) == isAscending)
					{
						newNode.prev = node.prev;
						newNode.next = node;
						node.prev.next = newNode;
						node.prev = newNode;
						return;
					}
					node = node.next;
				}
				if (Compare(node.value, newNode.value) == isAscending)
				{
					newNode.prev = node.prev;
					newNode.next = node;
					node.prev.next = newNode;
					node.prev = newNode;
					return;
				}
				tail.next = newNode;
				newNode.prev = tail;
				tail = newNode;
				return;
		}

		public Node<T> Find(T val)
		{
			var bufferNode = new Node<T>(val);
			if (this._ascending == true && Compare(head.value, val) == 1) return null;
			if (this._ascending == true && Compare(tail.value, val) == -1) return null;
			if (this._ascending == false && Compare(head.value, val) == -1) return null;
			if (this._ascending == false && Compare(tail.value, val) == 1) return null;
			var node = BinarySearch(GetAll(), val);
			return node;
		}

		public void Delete(T val)
		{
			var node = head;
			while (node != null)
			{
				if (Compare(node.value, val) == 0)
				{
					if (node.prev == null)
					{
						head = node.next;
						node = head;
						if (node == null) tail = null;
						else node.prev = null;
						break;
					}
					if (node.next != null)
					{
						node.prev.next = node.next;
						node.next.prev = node.prev;
					}
					else
					{
						node.prev.next = null;
						tail = node.prev;
					}
					break;
				}
				node = node.next;
			}
		}

		public void Clear(bool asc)
		{
			_ascending = asc;
			head = null;
			tail = null;
		}

		public int Count()
		{
			Node<T> node = head;
			int count = 0;
			while (node != null)
			{
				count++;
				node = node.next;
			}
			return count;
		}

		List<Node<T>> GetAll()
		{
			List<Node<T>> r = new List<Node<T>>();
			Node<T> node = head;
			while (node != null)
			{
				r.Add(node);
				node = node.next;
			}
			return r;
		}
		private int FirstValueIsBiggerByLexicographicOrder(string firstVal, string secondVal)
		{
			var isAscending = this._ascending?1:-1;
			if (this._ascending)
			{
				if (firstVal == secondVal) return 0;
				if (firstVal.Length > secondVal.Length)
				{
					for (int i = 0; i < secondVal.Length; i++)
					{
						if (secondVal[i] == firstVal[i]) continue;
						if (secondVal[i] < firstVal[i]) return 1;
						if (secondVal[i] > firstVal[i]) return -1;
					}
					return 1;
				}
				for (int i = 0; i < firstVal.Length; i++)
				{
					if (secondVal[i] == firstVal[i]) continue;
					if (secondVal[i] < firstVal[i]) return 1;
					if (secondVal[i] > firstVal[i]) return -1;
				}
				return -1;
			}
			else 
			{
				if (firstVal == secondVal) return 0;
				if (firstVal.Length > secondVal.Length)
				{
					for (int i = 0; i < secondVal.Length; i++)
					{
						if (secondVal[i] == firstVal[i]) continue;
						if (secondVal[i] < firstVal[i]) return 1;
						if (secondVal[i] > firstVal[i]) return -1;
					}
					return 1;
				}
				for (int i = 0; i < firstVal.Length; i++)
				{
					if (secondVal[i] == firstVal[i]) continue;
					if (secondVal[i] < firstVal[i]) return 1;
					if (secondVal[i] > firstVal[i]) return -1;
				}
				return -1;
			}
		}

		private Node<T> BinarySearch(List<Node<T>> list, T val)
		{
			if (list.Count == 0) return null;
			var first = 0;
			var last = list.Count() - 1;
			int middlepoint;
			int isAscending = this._ascending ? 1 : -1;

			int compareState;

			while (first <= last)
			{
				middlepoint = (int)((first + last) / 2);
				compareState = Compare(list[middlepoint].value, val);
				if (compareState == 0)
				{
					return list[middlepoint];
				}
				if (compareState == isAscending) last = middlepoint - 1;
				if (compareState == -isAscending) first = middlepoint + 1;
			}
			return null;
		}
	}

}
using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

	public class Node
	{
		public int value;
		public Node next, prev;

		public Node(int _value)
		{
			value = _value;
			next = null;
			prev = null;
		}
	}

	public class LinkedList2
	{
		public Node head;
		public Node tail;

		public LinkedList2()
		{
			head = null;
			tail = null;
		}

		public void AddInTail(Node _item)
		{
			if (head == null)
			{
				head = _item;
				head.next = null;
				head.prev = null;
			}
			else
			{
				tail.next = _item;
				_item.prev = tail;
			}
			tail = _item;
		}

		public Node Find(int _value)
		{
			var node = head;
			while (node != null)
			{
				if (node.value == _value) return node;
				node = node.next;
			}
			return null;
		}

		public List<Node> FindAll(int _value)
		{
			List<Node> nodes = new List<Node>();
			var node = head;
			while (node != null)
			{
				if (node.value == _value) nodes.Add(node);
				node = node.next;
			}
			return nodes;
		}

		public bool Remove(int _value)
		{
			Node node = head;
			while (node != null)
			{
				if (node.value == _value)
				{
					if (node.prev == null)
					{
						head = node.next;
						//head.prev = null;
						node = head;
						if (node == null) tail = null;
						else node.prev = null;
						return true;
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
					return true;
				}
				node = node.next;
			}
			return false;
		}

		public void RemoveAll(int _value)
		{
			Node node = head;
			while (node != null)
			{
				if (node.value == _value)
				{
					if (node.prev == null)
					{
						head = node.next;
						//head.prev = null;
						node = head;
						if (node == null) tail = null;
						else node.prev = null;
						continue;
					}
					if (node.next != null)
					{
						node.prev.next = node.next;
						node.next.prev = node.prev;
						node = node.next;
						continue;
					}
					else
					{
						node.prev.next = null;
						tail = node.prev;
						break;
					}
				}
				node = node.next;
			}
		}

		public void Clear()
		{
			head = null;
			tail = null;
		}

		public int Count()
		{
			Node node = head;
			int count = 0;
			while (node != null)
			{
				count++;
				node = node.next;
			}
			return count;
		}

		public void InsertAfter(Node _nodeAfter, Node _nodeToInsert)
		{

			if (_nodeToInsert == null) return;

			Node node = head;

			if (_nodeAfter == null)
			{
				if (node == null)
				{
					head = _nodeToInsert;
					tail = _nodeToInsert;
					return;
				}
				_nodeToInsert.next = node;
				head = _nodeToInsert;
				node.prev = head;
				return;
			}

			while (node != null)
			{
				if (node.Equals(_nodeAfter))
				{
					if (node.next != null)
					{
						_nodeToInsert.next = node.next;
						_nodeToInsert.prev = node;
						node.next.prev = _nodeToInsert;
						node.next = _nodeToInsert;
					}
					else
					{
						node.next = _nodeToInsert;
						tail = _nodeToInsert;
						tail.prev = node;
					}
					break;
				}
				node = node.next;
			}

		}

	}
}
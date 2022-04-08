using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

	public class Node
	{
		public int value;
		public Node next;
		public Node(int _value) { value = _value; }
	}

	public class LinkedList
	{
		public Node head;
		public Node tail;

		public LinkedList()
		{
			head = null;
			tail = null;
		}

		public void AddInTail(Node _item)
		{
			if (head == null) head = _item;
			else tail.next = _item;
			tail = _item;
		}

		public Node Find(int _value)
		{
			Node node = head;
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
			Node node = head;
			while (node != null)
			{
				if (node.value == _value)
				{
					nodes.Add(node);
				}
				
				node = node.next;
			}
			return nodes;
		}

		public bool Remove(int _value)
		{
			Node node = head;
			Node _nodePrevious = null;
			while (node != null)
			{
				if (node.value == _value)
				{
					if (_nodePrevious == null)
					{
						head = node.next;
						node = head;
						if(node == null) tail = null;
						return true;
					}
					if (node.next != null)
					{

						_nodePrevious.next = node.next;
					}
					else
					{
						_nodePrevious.next = null;
						tail = _nodePrevious;
					}
					return true; 
				}
				_nodePrevious = node;
				node = node.next;
			}
			return false; 
		}

		public void RemoveAll(int _value)
		{
			Node node = head;
			Node _nodePrevious = null;
			while (node != null)
			{
				if (node.value == _value)
				{
					if(_nodePrevious == null)
					{
						head = node.next;
						node = head;
						if (node == null) tail = null;
						continue;
					}
					if (node.next != null)
					{
						_nodePrevious.next = node.next;
						node = node.next;
						continue;
					}
					else
					{
						_nodePrevious.next = null;
						tail = _nodePrevious;
						break;
					}
				}
				_nodePrevious = node;
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
				return;
			}

			while (node != null)
			{
				if (node.Equals(_nodeAfter))
				{
					if (node.next != null)
					{
						_nodeToInsert.next = node.next;
						node.next = _nodeToInsert;
						
					}
					else
					{
						node.next = _nodeToInsert;
						tail = _nodeToInsert;
					}
					break;
				}
				node = node.next;
			}
		}

	}
}
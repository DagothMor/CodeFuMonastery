using System.Collections.Generic;

namespace AlgorithmsDataStructures
{
	public class DNode : Node
	{
		public DNode()
		{
			value = null;
		}
	}
	public class DLinkedList
	{
		private DNode dummyHead = new DNode();
		private DNode dummyTail = new DNode();

		public DNode head;
		public DNode tail;

		public DLinkedList()
		{
			head = dummyHead;
			tail = dummyTail;
			head.next = tail;
			head.prev = tail;
			tail.prev = head;
			tail.next = head;
		}

		public void AddInTail(Node _item)
		{
			tail.prev.next = _item;
			_item.prev = tail.prev;
			tail.prev = _item;
			_item.next = tail;
		}

		public Node Find(int _value)
		{
			var node = head.next;
			while (node != tail)
			{
				if (node.value == _value) return node;
				node = node.next;
			}
			return null;
		}

		public List<Node> FindAll(int _value)
		{
			List<Node> nodes = new List<Node>();
			var node = head.next;
			while (node != tail)
			{
				if (node.value == _value) nodes.Add(node);
				node = node.next;
			}
			return nodes;
		}

		public bool Remove(int _value)
		{
			Node node = head.next;
			while (node != tail)
			{
				if (node.value == _value)
				{
					node.prev.next = node.next;
					node.next.prev = node.prev;
					return true;
				}
				node = node.next;
			}
			return false;
		}

		public void RemoveAll(int _value)
		{
			Node node = head.next;
			while (node != tail)
			{
				if (node.value == _value)
				{
					node.prev.next = node.next;
					node.next.prev = node.prev;
					node = node.next;
					continue;
				}
				node = node.next;
			}
		}

		public void Clear()
		{
			if (head.next == tail) return;
			head.next = tail;
			head.prev = tail;
			tail.prev = head;
			tail.next = head;
		}

		public int Count()
		{
			Node node = head.next;
			int count = 0;
			while (node != tail)
			{
				count++;
				node = node.next;
			}
			return count;
		}

		public void InsertAfter(Node _nodeAfter, Node _nodeToInsert)
		{

			if (_nodeToInsert == null) return;

			Node node = head.next;

			if (_nodeAfter == null)
			{

				_nodeToInsert.next = head.next;
				head.next.prev = _nodeToInsert;
				head.next = _nodeToInsert;
				_nodeToInsert.prev = head;
				return;
			}

			while (node != tail)
			{
				if (node.Equals(_nodeAfter))
				{
					_nodeToInsert.next = node.next;
					node.next.prev = _nodeToInsert;
					node.next = _nodeToInsert;
					_nodeToInsert.prev = node;
					break;
				}
				node = node.next;
			}

		}

	}
}
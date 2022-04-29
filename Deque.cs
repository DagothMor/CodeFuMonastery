using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{
	public class DummyDequeNode<T> : DequeNode<T>
	{
		public DummyDequeNode(T _value) : base(_value)
		{
			value = _value;
		}
		public DummyDequeNode() : base()
		{
			value = default;
		}

	}
	public class DequeNode<T>
	{
		public T value;
		public DequeNode<T> next, prev;

		public DequeNode(T _value)
		{
			value = _value;
			next = null;
			prev = null;
		}
		public DequeNode()
		{
			value = default;
			next = null;
			prev = null;
		}
	}
	public class Deque<T>
	{
		private DummyDequeNode<T> dummyHead = new DummyDequeNode<T>();
		private DummyDequeNode<T> dummyTail = new DummyDequeNode<T>();

		public DummyDequeNode<T> head;
		public DummyDequeNode<T> tail;
		public Deque()
		{
			head = dummyHead;
			tail = dummyTail;
			head.next = tail;
			head.prev = tail;
			tail.prev = head;
			tail.next = head;
		}

		public void AddFront(T item)
		{
			var node = new DequeNode<T>(item);
			head.next.prev = node;
			node.next = head.next;
			node.prev = head;
			head.next = node;
			
		}

		public void AddTail(T item)
		{
			var node = new DequeNode<T>(item);
			tail.prev.next = node;
			node.prev = tail.prev;
			node.next = tail;
			tail.prev = node;
			
		}

		public T RemoveFront()
		{
			var node = head.next;
			if (node is DummyDequeNode<T>) return default(T);
			node.next.prev = head;
			head.next = node.next;
			return node.value;
		}

		public T RemoveTail()
		{
			var node = tail.prev;
			if (node is DummyDequeNode<T>) return default(T);
			node.prev.next = tail;
			tail.prev = node.prev;
			return node.value;
		}

		public int Size()
		{
			var node = head.next;
			int count = 0;
			while (!(node is DummyDequeNode<T>))
			{
				count++;
				node = node.next;
			}
			return count;
		}
	}

}
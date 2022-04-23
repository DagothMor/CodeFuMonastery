using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

	public class Stack<T>
	{
		readonly LinkedList<T> stack = new LinkedList<T>();
		public Stack()
		{
			stack = new LinkedList<T>();
		}

		public int Size()
		{
			return stack.Count;
		}

		public T Pop()
		{
			if (stack.Count == 0) return default(T);
			var value = stack.Last.Value;
			stack.RemoveLast();
			return value;
		}

		public void Push(T val)
		{
			stack.AddLast(val);
		}

		public T Peek()
		{
			if (stack.Count == 0) return default(T);
			return stack.Last.Value;
		}
	}

}
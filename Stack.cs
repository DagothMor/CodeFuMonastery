using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

    public class Stack<T>
    {
		readonly List<T> stack = new List<T>();
        public Stack()
        {
            stack = new List<T>();
        }

        public int Size()
        {
            return stack.Count;
        }

        public T Pop()
        {
            if(stack.Count == 0) return default(T);
            var kek = stack[stack.Count-1];
            stack.RemoveAt(stack.Count-1);
            return kek;
        }

        public void Push(T val)
        {
            stack.Insert(0,val);
        }

        public T Peek()
        {
            if (stack.Count == 0) return default(T);
            return stack[stack.Count-1];
        }
    }

}
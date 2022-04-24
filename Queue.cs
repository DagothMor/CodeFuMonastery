using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

    public class Queue<T>
    {
        readonly LinkedList<T> queue = new LinkedList<T>();
        public Queue()
        {
            queue = new LinkedList<T>();
        }

        public void Enqueue(T item)
        {
            queue.AddFirst(item);
        }

        public T Dequeue()
        {
            if (queue.Count == 0) return default(T);
            var value = queue.Last.Value;
            queue.RemoveLast();
            return value;
        }  

        public int Size()
        {
            return queue.Count;
        }

    }
}
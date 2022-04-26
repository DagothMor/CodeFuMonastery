﻿using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

    public class TwoStacksQueue<T>
    {
        readonly Stack<T> firstStack = new Stack<T>();
        readonly Stack<T> secondStack = new Stack<T>();
        public TwoStacksQueue()
        {
            firstStack = new Stack<T>();
            secondStack = new Stack<T>();
        }

        public void Enqueue(T item)
        {
            firstStack.Push(item);
        }

        public T Dequeue()
        {
            if (firstStack.Size() == 0) return default(T);

            while (firstStack.Size() > 1)
            {
                secondStack.Push(firstStack.Pop());
            }

            var value = firstStack.Pop();

            while (secondStack.Size() > 0)
            {
                firstStack.Push(secondStack.Pop());
            }
            return value;
        }

        public int Size()
        {
            return firstStack.Size();
        }

    }
}
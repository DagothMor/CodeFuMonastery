using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

    public class NativeDictionary<T>
    {
        public int size;
        public string[] slots;
        public T[] values;

        public NativeDictionary(int sz)
        {
            size = sz;
            slots = new string[size];
            values = new T[size];
        }

        public int HashFun(string key)
        {
            int index = 0;
            foreach (var item in key)
            {
                index += ((byte)item);
            }
            return index % size;
        }

        public bool IsKey(string key)
        {
            var index = HashFun(key);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (bufferValue == key) return true;
            do
            {
                index += 1;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (bufferValue == default) return false;
                if (bufferValue == key) return true;
            }
            while (bufferValue != firstValue);

            return false;

        }

        public void Put(string key, T value)
        {
            var index = HashFun(key);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (bufferValue == key && bufferValue == default)
            {
                values[index] = value;
                return;
            }
            do
            {
                index += 1;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (bufferValue == key && bufferValue == default)
                {
                    values[index] = value;
                    return;
                }
            }
            while (bufferValue != firstValue);
        }

        public T Get(string key)
        {
            var index = HashFun(key);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (bufferValue == key) return values[index];
            do
            {
                index += 1;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (bufferValue == default) return default;
                if (bufferValue == key) return values[index];
            }
            while (bufferValue != firstValue);

            return default;
        }
    }
}
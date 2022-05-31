using System.Collections.Generic;
using System;
using System.IO;
using System.Collections;

namespace AlgorithmsDataStructures
{
    public class NativeCache<T>
    {
        public int size;
        public String[] slots;
        public T[] values;
        public int[] hits;
        public Dictionary<String, T> cache;

        public NativeCache(int Size)
        {
            size = Size;
            slots = new string[size];
            values = new T[size];
            hits = new int[size];

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

        public int FindLessHits()
        {
            int index = 0;
            int buffer = hits[0];
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] < buffer)
                {
                    buffer = hits[i];
                    index = i;
                }
            }
            return index;
        }

        public void Put(string key, T value)
        {
            var index = HashFun(key);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (bufferValue == key)
            {
                values[index] = value;
                hits[index]++;
                return;
            }
            if (bufferValue == default)
            {
                values[index] = value;
                slots[index] = key;
                hits[index] = 0;
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
                if (bufferValue == key)
                {
                    values[index] = value;
                    hits[index]++;
                    return;
                }
                if (bufferValue == default)
                {
                    values[index] = value;
                    slots[index] = key;
                    hits[index] = 0;
                    return;
                }
                hits[index]++;
            }
            while (bufferValue != firstValue);
            var deleteIndex = FindLessHits();
            slots[deleteIndex] = key;
            values[deleteIndex] = value;
            hits[deleteIndex] = 0;
        }

        public T Get(string key)
        {
            var index = HashFun(key);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (bufferValue == key)
            {
                hits[index]++;
                return values[index];

            }
            if (bufferValue == default) return default;
            do
            {
                index += 1;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (bufferValue == default) return default;
                if (bufferValue == key)
                {
                    hits[index]++;
                    return values[index];
                }
                hits[index]++;
            }
            while (bufferValue != firstValue);
            return default;
        }
    }
}

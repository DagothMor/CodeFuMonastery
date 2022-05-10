using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

    public class HashTable
    {
        public int size;
        public int step;
        public string[] slots;

        public HashTable(int sz, int stp)
        {
            size = sz;
            step = stp;
            slots = new string[size];
            for (int i = 0; i < size; i++) slots[i] = null;
        }

        public int HashFun(string value)
        {
            int index = 0;
            foreach (var item in value)
            {
                index += ((byte)item);
            }
            return index % size;
        }

        public int SeekSlot(string value)
        {
            int index = HashFun(value);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (slots[index] == default) return index;
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (bufferValue == default) return index;
            }
            while (bufferValue != firstValue);
            return -1;
        }

        public int Put(string value)
        {
            var index = SeekSlot(value);
            if (index != -1) slots[index] = value;
            return index;
        }

        public int Find(string value)
        {
            int index = HashFun(value);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (bufferValue == value) return index;
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (bufferValue == default) return -1;
                if (bufferValue == value) return index;
            }
            while (bufferValue != firstValue);
            
            return -1;
        }
    }

}
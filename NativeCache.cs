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
            int slotIndex = 0;
            foreach (var letterInKey in key)
                // Буква в ключе.
            {
                slotIndex += ((byte)letterInKey);
            }
            return slotIndex % size;
        }

        public bool IsKey(string key)
        {
            var slotIndex = HashFun(key);
            var firstValue = slots[slotIndex];
            var slotByIndex = firstValue;
            if (slotByIndex == key) return true;
            do
            {
                slotIndex += 1;
                while (slotIndex >= slots.Length)
                {
                    slotIndex -= slots.Length;
                }
                slotByIndex = slots[slotIndex];
                if (slotByIndex == default) return false;
                if (slotByIndex == key) return true;
            }
            while (slotByIndex != firstValue);

            return false;

        }

        public int FindLessHits()
        {
            int slotIndex = 0;
            // 7.5 было Buffer, стало hitsLessValue.
            int buffer = hits[0];
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] < buffer)
                {
                    buffer = hits[i];
                    slotIndex = i;
                }
            }
            return slotIndex;
        }

        public void Put(string key, T inputValue)
            // Входящее значение.
        {
            var slotIndex = HashFun(key);
            // Индекс слота.
            var firstValue = slots[slotIndex];
            var bufferValue = firstValue;
            if (bufferValue == key)
            {
                values[slotIndex] = inputValue; // не поймешь какое значение, теперь яснее что ячейке мы присваиваем имено входящее.
                hits[slotIndex]++;
                return;
            }
            if (bufferValue == default)
            {
                values[slotIndex] = inputValue;
                slots[slotIndex] = key;
                hits[slotIndex] = 0;
                return;
            }
            do
            {
                slotIndex += 1;
                while (slotIndex >= slots.Length)
                {
                    slotIndex -= slots.Length;
                }
                bufferValue = slots[slotIndex];
                if (bufferValue == key)
                {
                    values[slotIndex] = inputValue;
                    hits[slotIndex]++;
                    return;
                }
                if (bufferValue == default)
                {
                    values[slotIndex] = inputValue;
                    slots[slotIndex] = key;
                    hits[slotIndex] = 0;
                    return;
                }
                hits[slotIndex]++;
            }
            while (bufferValue != firstValue);
            var deleteIndex = FindLessHits();
            slots[deleteIndex] = key;
            values[deleteIndex] = inputValue;
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

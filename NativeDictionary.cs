using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

    public class NativeDictionary<T>
    {
        public int size;
        public string[] slots;
        public T[] values;

        public NativeDictionary(int size)
        // размер.
        {
            this.size = size;
            slots = new string[size];
            values = new T[size];
        }

        public int HashFun(string key)
        {
            int slotIndex = 0;
            foreach (var item in key)
            {
                slotIndex += ((byte)item);
            }
            return slotIndex % size;
        }

        public bool IsKey(string key)
        {
            var slotIndex = HashFun(key);
            var firstValue = slots[slotIndex];
            var bufferValue = firstValue;
            if (bufferValue == key) return true;
            do
            {
                slotIndex++;
                while (slotIndex >= slots.Length)
                {
                    slotIndex -= slots.Length;
                }
                bufferValue = slots[slotIndex];
                if (bufferValue == default) return false;
                if (bufferValue == key) return true;
            }
            while (bufferValue != firstValue);

            return false;

        }

        public void Put(string key, T inputValue)
        {
            var slotIndex = HashFun(key);
            var firstValueOfSlots = slots[slotIndex]; // первое значение чего? поправил.
            var bufferValue = firstValueOfSlots;
            if (bufferValue == key)
            {
                values[slotIndex] = inputValue;
                return;
            }
            if (bufferValue == default) {
                values[slotIndex] = inputValue;
                slots[slotIndex] = key;
                return;
            } 
            do
            {
                slotIndex++;
                while (slotIndex >= slots.Length)
                {
                    slotIndex -= slots.Length;
                }
                bufferValue = slots[slotIndex];
                if (bufferValue == key)
                {
                    values[slotIndex] = inputValue;
                    return;
                }
                if (bufferValue == default)
                {
                    values[slotIndex] = inputValue;
                    slots[slotIndex] = key;
                    return;
                }
            }
            while (bufferValue != firstValueOfSlots);
        }

        public T Get(string key)
        {
            var slotIndex = HashFun(key);
            var firstValueOfSlots = slots[slotIndex];
            // Первое значение слота.
            var bufferValue = firstValueOfSlots;
            if (bufferValue == key) return values[slotIndex];
            if (bufferValue == default) return default;
            do
            {
                slotIndex++;
                while (slotIndex >= slots.Length)
                {
                    slotIndex -= slots.Length;
                }
                bufferValue = slots[slotIndex];
                if (bufferValue == default) return default;
                if (bufferValue == key) return values[slotIndex];
            }
            while (bufferValue != firstValueOfSlots);

            return default;
        }
    }
}
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
            var slotByIndex = firstValue;
            if (slotByIndex == key) return true;
            do
            {
                slotIndex++;
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

        public void Put(string key, T inputValue)
        {
            var slotIndex = HashFun(key);
            var firstValueOfSlots = slots[slotIndex]; // первое значение чего? поправил.
            var slotByIndex = firstValueOfSlots;
            if (slotByIndex == key)
            {
                values[slotIndex] = inputValue;
                return;
            }
            if (slotByIndex == default) {
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
                slotByIndex = slots[slotIndex];
                if (slotByIndex == key)
                {
                    values[slotIndex] = inputValue;
                    return;
                }
                if (slotByIndex == default)
                {
                    values[slotIndex] = inputValue;
                    slots[slotIndex] = key;
                    return;
                }
            }
            while (slotByIndex != firstValueOfSlots);
        }

        public T Get(string key)
        {
            var slotIndex = HashFun(key);
            var firstValueOfSlots = slots[slotIndex];
            // Первое значение слота.
            var slotByIndex = firstValueOfSlots;
            if (slotByIndex == key) return values[slotIndex];
            if (slotByIndex == default) return default;
            do
            {
                slotIndex++;
                while (slotIndex >= slots.Length)
                {
                    slotIndex -= slots.Length;
                }
                slotByIndex = slots[slotIndex];
                if (slotByIndex == default) return default;
                if (slotByIndex == key) return values[slotIndex];
            }
            while (slotByIndex != firstValueOfSlots);

            return default;
        }
    }
}
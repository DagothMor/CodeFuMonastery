using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{
    public class PowerSet<T>
    {
        public int size;
        public int step;
        public T[] slots;

        public PowerSet()
        {
            size = 20000;
            step = 1;
            slots = new T[size];
            for (int i = 0; i < size; i++) slots[i] = default(T);
        }
        public int HashFun(T value)
        {
            var hash = value.GetHashCode() & 0x7FFFFFFF;
            return hash % size;
        }
        public int SeekSlot(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T))) return -1;
            int index = HashFun(value);
            // 7.5 было firstValue стало ValueByFirstSlotIndex
            var ValueByFirstSlotIndex = slots[index];
            // 7.5 было bufferValue стало slotByIndex.
            var slotByIndex = ValueByFirstSlotIndex;
            if (EqualityComparer<T>.Default.Equals(slots[index], default(T))) return index;
            if (EqualityComparer<T>.Default.Equals(slotByIndex, value)) return -1;
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                slotByIndex = slots[index];
                if (EqualityComparer<T>.Default.Equals(slotByIndex, default(T))) return index;
                if (EqualityComparer<T>.Default.Equals(slotByIndex, value)) return -1;
            }
            while (!EqualityComparer<T>.Default.Equals(slotByIndex, ValueByFirstSlotIndex));
            return -1;
        }
        public void Put(T value)
        {
            var index = SeekSlot(value);
            if (index != -1) slots[index] = value;
        }
        public int Size()
        {
            int count = 0;
            foreach (var slot in slots)
            {
                if (EqualityComparer<T>.Default.Equals(slot, default(T)))
                    continue;
                count++;
            }
            return count;
        }
        public bool Get(T value)
        {
            int index = HashFun(value);
            var ValueByFirstSlotIndex = slots[index];
            var slotByIndex = ValueByFirstSlotIndex;
            if (EqualityComparer<T>.Default.Equals(slotByIndex, value)) return true;
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                slotByIndex = slots[index];
                if (EqualityComparer<T>.Default.Equals(slotByIndex, value)) return true;
                if (EqualityComparer<T>.Default.Equals(slotByIndex, default(T))) return false;

            }
            while (!EqualityComparer<T>.Default.Equals(slotByIndex, ValueByFirstSlotIndex));

            return false;
        }
        public bool Remove(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T))) return false;
            int index = HashFun(value);
            var ValueByFirstSlotIndex = slots[index];
            var slotByIndex = ValueByFirstSlotIndex;
            if (EqualityComparer<T>.Default.Equals(slotByIndex, value)) { slots[index] = default(T); return true; }
            if (EqualityComparer<T>.Default.Equals(slotByIndex, default(T)))
            {
                var startPos = index;
                do
                {
                    index += step;
                    while (index >= slots.Length)
                    {
                        index -= slots.Length;
                    }
                    slotByIndex = slots[index];
                    if (EqualityComparer<T>.Default.Equals(slotByIndex, value)) { slots[index] = default(T); return true; }

                }
                while (!index.Equals(startPos));
                return false;
            }
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                slotByIndex = slots[index];
                if (EqualityComparer<T>.Default.Equals(slotByIndex, value)) { slots[index] = default(T); return true; }
            }
            while (!EqualityComparer<T>.Default.Equals(slotByIndex, ValueByFirstSlotIndex));

            return false;
        }
        public PowerSet<T> Intersection(PowerSet<T> set2)
        {
            var powerSet = new PowerSet<T>();
            if (set2.Size() == 0) return powerSet;

            if (set2.Size() >= this.Size())
            {
                foreach (var item in set2.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    if (this.Get(item)) powerSet.Put(item);
                }
                return powerSet;
            }
            else
            {
                foreach (var item in this.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    if (set2.Get(item)) powerSet.Put(item);
                }
                return powerSet;
            }
        }
        public PowerSet<T> Union(PowerSet<T> set2)
        {
            var powerSet = new PowerSet<T>();
            if (set2.Size() == 0 && this.Size() == 0) return powerSet;
            if (set2.Size() == 0)
            {
                foreach (var item in this.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    powerSet.Put(item);
                }
                return powerSet;
            }
            if (this.Size() == 0)
            {
                foreach (var item in set2.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    powerSet.Put(item);
                }
                return powerSet;
            }
            foreach (var item in set2.slots)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                powerSet.Put(item);
            }
            foreach (var item in this.slots)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                powerSet.Put(item);
            }
            return powerSet;
        }
        public PowerSet<T> Difference(PowerSet<T> set2)
        {
            var powerSet = new PowerSet<T>();
            if (set2.Size() == 0 && this.Size() == 0) return powerSet;

            if (set2.Size() == 0)
            {
                foreach (var item in this.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    powerSet.Put(item);
                }
                return powerSet;
            }
            if (this.Size() == 0)
            {
                foreach (var item in set2.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    powerSet.Put(item);
                }
                return powerSet;
            }
            foreach (var item in this.slots)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                if (!set2.Get(item)) powerSet.Put(item);
            }
            return powerSet;
        }
        public bool IsSubset(PowerSet<T> set2)
        {
            if (set2.Size() == 0) return true;
            if (this.Size() == 0) return true;
            foreach (var item in set2.slots)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                if (!this.Get(item)) return false;
            }
            return true;
        }
    }
}
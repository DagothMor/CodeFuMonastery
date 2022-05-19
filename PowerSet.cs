﻿using System;
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
            int index = HashFun(value);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (EqualityComparer<T>.Default.Equals(slots[index], default(T))) return index;
            if (EqualityComparer<T>.Default.Equals(bufferValue, value)) return -1;
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (EqualityComparer<T>.Default.Equals(bufferValue, default(T))) return index;
                if (EqualityComparer<T>.Default.Equals(bufferValue, value)) return -1;
            }
            while (!EqualityComparer<T>.Default.Equals(bufferValue, firstValue));
            return -1;
        }
        public void Put(T value)
        {
            var index = SeekSlot(value);
            if (index != -1) slots[index] = value;
        }
        public int Find(T value)
        {
            int index = HashFun(value);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (EqualityComparer<T>.Default.Equals(bufferValue, value)) return index;
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (EqualityComparer<T>.Default.Equals(bufferValue, default(T))) return -1;
                if (EqualityComparer<T>.Default.Equals(bufferValue, value)) return index;
            }
            while (!EqualityComparer<T>.Default.Equals(bufferValue, firstValue));

            return -1;
        }
        public int Size()
        {
            int count = 0;
            foreach (var slot in slots)
            {
                if (EqualityComparer<T>.Default.Equals(slot, default(T)))
                //if(default(T).Equals(slot))
                    continue;
                count++;
            }
            return count;
        }
        public bool Get(T value)
        {

            int index = HashFun(value);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (EqualityComparer<T>.Default.Equals(bufferValue, value)) return true;
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (EqualityComparer<T>.Default.Equals(bufferValue, default(T))) return false;
                if (EqualityComparer<T>.Default.Equals(bufferValue, value)) return true;
            }
            while (!EqualityComparer<T>.Default.Equals(bufferValue, firstValue));

            return false;
        }
        public bool Remove(T value)
        {
            int index = HashFun(value);
            var firstValue = slots[index];
            var bufferValue = firstValue;
            if (EqualityComparer<T>.Default.Equals(bufferValue, value)) { slots[index] = default(T); return true; }
            do
            {
                index += step;
                while (index >= slots.Length)
                {
                    index -= slots.Length;
                }
                bufferValue = slots[index];
                if (EqualityComparer<T>.Default.Equals(bufferValue, default(T))) return false;
                if (EqualityComparer<T>.Default.Equals(bufferValue, value)) { slots[index] = default(T); return true; }
            }
            while (!EqualityComparer<T>.Default.Equals(bufferValue, firstValue));

            return false;
        }

        public PowerSet<T> Intersection(PowerSet<T> set2)
        {
            if (set2.size == 0) return null;
            var powerSet = new PowerSet<T>();
            if (set2.size > this.size)
            {
                foreach (var item in set2.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    if (this.Get(item)) powerSet.Put(item);
                }
                return powerSet.Size() == 0 ? null: powerSet;
            }
            else
            {
                foreach (var item in this.slots)
                {
                    if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                    if (set2.Get(item)) powerSet.Put(item);
                }
                return powerSet.Size() == 0 ? null : powerSet;
            }
        }
        public PowerSet<T> Union(PowerSet<T> set2)
        {
            if (set2.size == 0) return this;
            foreach (var item in set2.slots)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                this.Put(item);
            }
            return this;
        }
        public PowerSet<T> Difference(PowerSet<T> set2)
        {
            if (set2.size == 0) return this;
            foreach (var item in set2.slots)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                if (this.Get(item)) this.Remove(item);
            }
            return this;
        }

        //Done
        public bool IsSubset(PowerSet<T> set2)
        {
            if (set2.size == 0) return false;
            foreach (var item in set2.slots)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T))) continue;
                if (!this.Get(item)) return false;
            }
            return true;
        }
    }
}
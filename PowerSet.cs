using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{
    public class PowerSet<T>
    {
        public int count;
        public List<T> slots;

        public PowerSet()
        {
            slots = new List<T>();
            count = 0;
        }

        public int Size()
        {
            return count;
        }

        public void Put(T value)
        {
            if (!Get(value))
            {
                slots.Add(value);
                count++;
            }
        }

        public bool Get(T value)
        {
            return slots.Contains(value);
        }

        public bool Remove(T value)
        {
            if (Get(value))
            {
                slots.Remove(value);
                count--;
                return true;
            }
            return false;
        }

        public PowerSet<T> Intersection(PowerSet<T> set2)
        {
            var powerSet = new PowerSet<T>();
            if (count > 0 && set2.count > 0)
            {
                foreach (T item in slots)
                {
                    if (set2.Get(item))
                    {
                        powerSet.Put(item);
                    }
                }
            }
            return powerSet;
        }

        public PowerSet<T> Union(PowerSet<T> set2)
        {
            var powerSet = new PowerSet<T>();
            if (count > 0)
            {
                foreach (var item in slots)
                {
                    powerSet.Put(item);
                }
            }
            if (set2.count > 0)
            {
                foreach (var item in set2.slots)
                {
                    powerSet.Put(item);
                }
            }
            return powerSet;
        }

        public PowerSet<T> Difference(PowerSet<T> set2)
        {
            var powerSet = new PowerSet<T>();
            if (count > 0)
            {
                foreach (T item in slots)
                {
                    powerSet.Put(item);
                }
                foreach (T item in set2.slots)
                {
                    powerSet.Remove(item);
                }
            }
            return powerSet;
        }

        public bool IsSubset(PowerSet<T> set2)
        {
            int count = 0;
            if (this.count >= set2.count)
            {
                foreach (T item in slots)
                {
                    if (set2.Get(item))
                    {
                        count++;
                    }
                }
                if (count == set2.count)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
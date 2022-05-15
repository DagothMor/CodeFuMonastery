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
            int index = HashFun(key);
            var value = slots[index];
            if (value == key) return true;
            return false;
        }

        public void Put(string key, T value)
        {
            int index = HashFun(key);
            values[index] = value;
        }

        public T Get(string key)
        {
            var index = HashFun(key);
            try
            {
                var value = values[index];
                return value;
            }
            catch (Exception)
            {

                return default(T);
            }
        }
    }
}
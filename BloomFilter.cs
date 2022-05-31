using System.Collections.Generic;
using System;
using System.IO;
using System.Collections;

namespace AlgorithmsDataStructures
{
    public class ArrayOfBits
    {
        public int lenght = 32;
        public int value = 0;
        public ArrayOfBits() { }
        public void SetValue(int bit) 
        {
            int bitToSet = 1 << bit;
            value = value | bitToSet;   
        }
        public bool getValue(int bit) 
        {
            return ((value >> bit) & 1) != 0;
        }
    }
    public class BloomFilter
    {
        public int filter_len;
        public ArrayOfBits arrayOfBits;

        public BloomFilter(int f_len)
        {
            filter_len = f_len;
            arrayOfBits = new ArrayOfBits();
        }

        public int Hash1(string str1) // abcdefg
        {
            if (str1.Length < 1) return 0;

            int result = 0;

            for (int i = 1; i < str1.Length; i++)
            {
                int code = (int)str1[i];
                result = (result * 17 + code) % arrayOfBits.lenght;
            }
            return result;
        }
        public int Hash2(string str1)
        {
            if (str1.Length < 1) return 0;

            int result = 0;

            for (int i = 1; i < str1.Length; i++)
            {
                int code = (int)str1[i];
                result = (result * 223 + code) % arrayOfBits.lenght;
            }
            return result;
        }

        public void Add(string str1)
        {
            arrayOfBits.SetValue(Hash1(str1));
            arrayOfBits.SetValue(Hash2(str1));
        }

        public bool IsValue(string str1)
        {
            return arrayOfBits.getValue(Hash1(str1)) && arrayOfBits.getValue(Hash2(str1));
        }
    }
}
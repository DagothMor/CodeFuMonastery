using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class Heap
    {

        public int[] HeapArray;
        public int size;

        private int GetLeftChildIndex(int parentindex) { return 2 * parentindex + 1; }
        private int GetRightChildIndex(int parentindex) { return 2 * parentindex + 2; }
        private int GetParentIndex(int childIndex) { return (childIndex - 1) / 2; }

        private bool HasLeftChild(int index) { return GetLeftChildIndex(index) < HeapArray.Length - 1; }
        private bool HasRightChild(int index) { return GetRightChildIndex(index) < HeapArray.Length - 1; }
        private bool HasParent(int index) { return GetParentIndex(index) >= 0; }

        private int LeftChild(int index) { return HeapArray[GetLeftChildIndex(index)]; }
        private int RightChild(int index) { return HeapArray[GetRightChildIndex(index)]; }
        private int Parent(int index) { return HeapArray[GetParentIndex(index)]; }

        private int GetSize(int depth) => depth == 0 ? 1 : (int)Math.Pow(2, depth) + GetSize(depth - 1);

        public Heap() { HeapArray = null; size = 0; }

        public void MakeHeap(int[] a, int depth)
        {
            HeapArray = new int[GetSize(depth)];
            for (int i = 0; i < a.Length; i++)
            {
                Add(a[i]);
            }
        }


        public int GetMax()
        {
            if (HeapArray == null)
                return -1;

            int item = HeapArray[0];
            HeapArray[0] = HeapArray[size - 1];
            HeapArray[size - 1] = 0;
            size--;
            HeapDown();
            return item;
        }

        public bool Add(int key)
        {
            if (size == HeapArray.Length)
                return false;
            HeapArray[size] = key;
            size++;
            HeapUp();
            return true;
        }

        public void HeapUp()
        {
            int index = size - 1;

            while (HasParent(index) && Parent(index) < HeapArray[index])
            {
                swap(GetParentIndex(index), index);
                index = GetParentIndex(index);
            }
        }
        public void HeapDown()
        {
            int index = 0;
            while (HasLeftChild(index))
            {
                int smallerChildIndex = GetLeftChildIndex(index);
                if (HasRightChild(index) && RightChild(index) > LeftChild(index))
                {
                    smallerChildIndex = GetRightChildIndex(index);
                }
                if (HeapArray[index] > HeapArray[smallerChildIndex]) { break; }
                else { swap(index, smallerChildIndex); }
                index = smallerChildIndex;
            }
        }
        private void swap(int firstIndex, int secondIndex)
        {
            int temp = HeapArray[firstIndex];
            HeapArray[firstIndex] = HeapArray[secondIndex];
            HeapArray[secondIndex] = temp;
        }

    }
}
using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class aBST
    {
        public int?[] Tree;

        public aBST(int depth)
        {
            depth = depth < 0 ? 0 : depth;
            int tree_size = GetSize(depth);
            Tree = new int?[tree_size];
            for (int i = 0; i < tree_size; i++) Tree[i] = null;
        }
        public int GetSize(int depth) => depth == 0 ? 1 : (int)Math.Pow(2, depth) + GetSize(depth - 1);

        public int? FindKeyIndex(int key)
        {
            if (this.Tree.Length == 0) return null;
            return FindKeyIndexStart(key, 0);
        }

        public int? FindKeyIndexStart(int key, int currentIndex)
        {
            var currentNode = this.Tree[currentIndex];
            if (key == currentNode) return currentIndex;
            if (this.Tree[currentIndex] == null) return -currentIndex;
            var newIndex = key > currentNode
                ? GetChildren(currentIndex, false)
                : GetChildren(currentIndex, true);
            if (newIndex == -1) return null;
            return FindKeyIndexStart(key, newIndex);
        }

        public int GetChildren(int currentNodeIndex, bool isLeft)
        {
            var newIndex = 2 * currentNodeIndex + (isLeft ? 1 : 2);
            if (newIndex >= this.Tree.Length || newIndex < 0)
                return -1;
            else return newIndex;
        }


        public int AddKey(int key)
        {
            return AddKeyStart(key, 0);
        }

        public int AddKeyStart(int key, int currentIndex)
        {
            if (currentIndex == -1) return -1;
            if (this.Tree[currentIndex] == null)
            {
                this.Tree[currentIndex] = key;
                return currentIndex;
            }
            if (this.Tree[currentIndex] == key)
                return currentIndex;
            return AddKeyStart(key, GetChildren(currentIndex, key < this.Tree[currentIndex]));
        }


    }
}
using System;
using System.Collections.Generic;


namespace AlgorithmsDataStructures2
{
    public static class BalancedBST
    {
        public static int[] GenerateBBSTArray(int[] a)
        {
            Array.Sort(a);
            var balancedList = new List<int>();
            var tree = new BST<int>(GenerateBalancedTree(a));
            var arrayOfNodes = tree.WideAllNodes();
            foreach (var node in arrayOfNodes)
            {
                balancedList.Add(node.NodeKey);
            }
            return balancedList.ToArray();
        }

        public static BSTNode<int> GenerateBalancedTree(int[] arr)
        {
            return sortedArrayToBST(arr, 0, arr.Length - 1, null);
        }

        public static BSTNode<int> sortedArrayToBST(int[] arr, int start, int end, BSTNode<int> currentNode)
        {
            if (start > end) return null;
            int mid = (start + end) / 2;
            BSTNode<int> newNode = new BSTNode<int>(arr[mid], 0, currentNode);
            newNode.LeftChild = sortedArrayToBST(arr, start, mid - 1, newNode);
            newNode.RightChild = sortedArrayToBST(arr, mid + 1, end, newNode);
            return newNode;
        }

    }
}
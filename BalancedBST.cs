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
            var tree = new BTree(GenerateBalancedTree(a));
            var arrayOfNodes = tree.WideAllNodes();
            foreach (var node in arrayOfNodes)
            {
                balancedList.Add(node.Key);
            }
            return balancedList.ToArray();
        }

        public static Node GenerateBalancedTree(int[] arr)
        {
            return sortedArrayToBST(arr, 0, arr.Length - 1, null);
        }

        public static Node sortedArrayToBST(int[] arr, int start, int end, Node currentNode)
        {
            if (start > end) return null;
            int mid = (start + end) / 2;
            var newNode = new Node(arr[mid],currentNode);
            newNode.LeftChild = sortedArrayToBST(arr, start, mid - 1, newNode);
            newNode.RightChild = sortedArrayToBST(arr, mid + 1, end, newNode);
            return newNode;
        }

    }
    public class Node
    {
        public int Key;
        public Node LeftChild;
        public Node RightChild;
        public Node Parent;
        public Node(int key, Node parent)
        {
            Key = key;
            Parent = parent;
        }
    }
    public class BTree
    {
        public Node root;
        public BTree(Node node)
        {
            root = node;
        }
        public List<Node> WideAllNodes()
        {
            List<Node> listOfNodes = new List<Node>();

            List<Node> bufferList = new List<Node>();

            bufferList.Add(root);

            while (bufferList.Count > 0)
            {
                var bufferNode = bufferList[0];
                var bstNode = new Node(bufferNode.Key,null);
                listOfNodes.Add(bstNode);
                bufferList.RemoveAt(0);
                if (bufferNode.LeftChild != null)
                {
                    bufferList.Add(bufferNode.LeftChild);
                }
                if (bufferNode.RightChild != null)
                {
                    bufferList.Add(bufferNode.RightChild);
                }
            }
            return listOfNodes;
        }
    }
}
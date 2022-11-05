using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2
{
    public class BSTNode
    {
        public int NodeKey;
        public BSTNode Parent;
        public BSTNode LeftChild;
        public BSTNode RightChild;
        public int Level;

        public BSTNode(int key, BSTNode parent)
        {
            NodeKey = key;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        }
    }


    public class BalancedBST
    {
        public BSTNode Root; 

        public BalancedBST()
        {
            Root = null;
        }

        public void GenerateTree(int[] a)
        {
            Array.Sort(a);
            this.Root = StartGenerate(a, 0, a.Length - 1, 0, null);
        }
        public BSTNode StartGenerate(int[] a, int start, int end, int level, BSTNode currentNode)
        {
            if (start > end) return null;
            int mid = (start + end) / 2;
            var newNode = new BSTNode(a[mid], currentNode);
            newNode.Level = level;
            newNode.LeftChild = StartGenerate(a, start, mid - 1, level + 1, newNode);
            newNode.RightChild = StartGenerate(a, mid + 1, end, level + 1, newNode);
            return newNode;
        }

        public bool IsBalanced(BSTNode root_node)
        {
            if (root_node == null) return false;
            var maxLevelLeftChild = DeepAllNodesStart(root_node.LeftChild);
            var maxLevelRightChild = DeepAllNodesStart(root_node.RightChild);
            var maxLeft = 0;
            var maxRight = 0;
            foreach (var item in maxLevelLeftChild)
            {
                if (maxLeft<item)
                {
                    maxLeft = item;
                }
            }
            foreach (var item in maxLevelRightChild)
            {
                if (maxRight < item)
                {
                    maxRight = item;
                }
            }
            return Math.Abs(maxLeft - maxRight)<=1;
        }
        public List<int> DeepAllNodesStart(BSTNode node)
        {
            List<int> listOflevels = new List<int>();
            if (node == null) return listOflevels;
            listOflevels.AddRange(DeepAllNodesStart(node.LeftChild));
            listOflevels.AddRange(DeepAllNodesStart(node.RightChild));
            listOflevels.Add(node.Level);
            return listOflevels;
        }
    }
}
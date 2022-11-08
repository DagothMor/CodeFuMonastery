using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class BSTNode<T>
    {
        public int NodeKey;
        public T NodeValue;
        public BSTNode<T> Parent;
        public BSTNode<T> LeftChild;
        public BSTNode<T> RightChild;

        public BSTNode(int key, T val, BSTNode<T> parent)
        {
            NodeKey = key;
            NodeValue = val;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        }
    }

    public class BSTFind<T>
    {
        public BSTNode<T> Node;

        public bool NodeHasKey;

        public bool ToLeft;

        public BSTFind() { Node = null; }
    }

    public class BST<T>
    {
        public BSTNode<T> Root;

        public BST(BSTNode<T> node)
        {
            Root = node;
        }

        public BST()
        {
            Root = null;
        }

        public BSTFind<T> FindNodeByKey(int key)
        {
            var rootNode = new BSTFind<T>();
            if (this.Root == null)
            {
                rootNode.NodeHasKey = false;
                rootNode.ToLeft = false;
                rootNode.Node = null;
                return rootNode;
            }
            rootNode.Node = this.Root;
            return FindNodeByKeyStart(key, rootNode);
        }

        private BSTFind<T> FindNodeByKeyStart(int key, BSTFind<T> bufferNode)
        {
            if (bufferNode.Node.NodeKey.Equals(key))
            {
                bufferNode.NodeHasKey = true;
                return bufferNode;
            }
            if (bufferNode.Node.LeftChild == null && bufferNode.Node.RightChild == null)
            {
                bufferNode.ToLeft = bufferNode.Node.NodeKey > key;
                return bufferNode;
            }
            if (bufferNode.Node.NodeKey < key)
            {
                if (bufferNode.Node.RightChild == null)
                {
                    bufferNode.ToLeft = false;
                    bufferNode.NodeHasKey = false;
                    return bufferNode;
                }
                bufferNode.Node = bufferNode.Node.RightChild;
                return FindNodeByKeyStart(key, bufferNode);
            }
            else
            {
                if (bufferNode.Node.LeftChild == null)
                {
                    bufferNode.ToLeft = true;
                    bufferNode.NodeHasKey = false;
                    return bufferNode;
                }
                bufferNode.Node = bufferNode.Node.LeftChild;
                return FindNodeByKeyStart(key, bufferNode);
            }
        }

        public bool AddKeyValue(int key, T val)
        {
            if (this.Root == null)
            {
                this.Root = new BSTNode<T>(key, val, null);
                return true;

            }
            var foundedNode = FindNodeByKey(key);

            if (foundedNode.NodeHasKey)
            {
                return false;
            }
            if (foundedNode.ToLeft) foundedNode.Node.LeftChild = new BSTNode<T>(key, val, foundedNode.Node);
            else foundedNode.Node.RightChild = new BSTNode<T>(key, val, foundedNode.Node);
            return true;
        }

        public BSTNode<T> FinMinMax(BSTNode<T> FromNode, bool FindMax)
        {
            if (FromNode.LeftChild == null && !FindMax) return FromNode;
            if (FromNode.RightChild == null && FindMax) return FromNode;
            return FindMax ? FinMinMax(FromNode.RightChild, FindMax) : FinMinMax(FromNode.LeftChild, FindMax);
        }

        public bool DeleteNodeByKey(int key)
        {
            var foundedNode = FindNodeByKey(key);

            if (!foundedNode.NodeHasKey)
            {
                return false;
            }

            var deletingNode = foundedNode.Node;

            // if foundedNode is root.
            if (deletingNode.Parent == null && deletingNode.LeftChild == null && deletingNode.RightChild == null)
            {
                this.Root = null;
                return true;
            }
            if (deletingNode.Parent == null && deletingNode.RightChild == null)
            {
                this.Root = deletingNode.LeftChild;
                deletingNode.LeftChild.Parent = null;
                return true;
            }
            if (deletingNode.Parent == null && deletingNode.LeftChild == null)
            {
                this.Root = deletingNode.RightChild;
                deletingNode.RightChild.Parent = null;
                return true;
            }

            if (deletingNode.Parent == null)
            {
                var minNode = FinMinMax(deletingNode.RightChild, false);
                if (minNode.LeftChild == null && minNode.RightChild == null)
                {
                    minNode.Parent.LeftChild = minNode.Parent.LeftChild.Equals(minNode) ? null : minNode.Parent.LeftChild;
                    minNode.Parent.RightChild = minNode.Parent.RightChild.Equals(minNode) ? null : minNode.Parent.RightChild;

                    minNode.LeftChild = deletingNode.LeftChild.NodeKey < minNode.NodeKey ? deletingNode.LeftChild : deletingNode.RightChild;
                    minNode.RightChild = deletingNode.RightChild.NodeKey > minNode.NodeKey ? deletingNode.RightChild : deletingNode.LeftChild;


                    minNode.Parent = deletingNode.Parent;
                    deletingNode.RightChild.Parent = minNode;
                    deletingNode.LeftChild.Parent = minNode;
                    this.Root = minNode;
                    return true;
                }
                else
                {
                    deletingNode.Parent.LeftChild = deletingNode.Parent.LeftChild.Equals(deletingNode) ? minNode : deletingNode.Parent.LeftChild;
                    deletingNode.Parent.RightChild = deletingNode.Parent.RightChild.Equals(deletingNode) ? minNode : deletingNode.Parent.RightChild;

                    minNode.LeftChild = deletingNode.LeftChild;
                    deletingNode.LeftChild.Parent = minNode;
                    minNode.Parent = deletingNode.Parent;
                    this.Root = minNode;
                    return true;
                }
            }

            return DeleteNodeByKeyStart(foundedNode);
        }
        private bool DeleteNodeByKeyStart(BSTFind<T> foundedNode)
        {

            var deletingNode = foundedNode.Node;
            if (deletingNode.LeftChild == null && deletingNode.RightChild == null)
            {

                if (deletingNode.Parent.LeftChild != null)
                    deletingNode.Parent.LeftChild = deletingNode.Parent.LeftChild.Equals(deletingNode) ? null : deletingNode.Parent.LeftChild;
                if (deletingNode.Parent.RightChild != null)
                    deletingNode.Parent.RightChild = deletingNode.Parent.RightChild.Equals(deletingNode) ? null : deletingNode.Parent.RightChild;
                return true;
            }
            if (deletingNode.LeftChild == null)
            {
                deletingNode.RightChild.Parent = deletingNode.Parent;
                deletingNode.Parent.LeftChild = deletingNode.Parent.LeftChild.Equals(deletingNode) ? deletingNode.LeftChild : deletingNode.Parent.LeftChild;
                deletingNode.Parent.RightChild = deletingNode.Parent.RightChild.Equals(deletingNode) ? deletingNode.RightChild : deletingNode.Parent.RightChild;


                return true;
            }
            if (deletingNode.RightChild == null)
            {
                deletingNode.LeftChild.Parent = deletingNode.Parent;
                deletingNode.Parent.LeftChild = deletingNode.Parent.LeftChild.Equals(deletingNode) ? deletingNode.LeftChild : deletingNode.Parent.LeftChild;
                deletingNode.Parent.RightChild = deletingNode.Parent.RightChild.Equals(deletingNode) ? deletingNode.LeftChild : deletingNode.Parent.RightChild;


                return true;
            }
            else
            {
                var minNode = foundedNode.Node.RightChild != null ? FinMinMax(foundedNode.Node.RightChild, false) : foundedNode.Node.Parent;

                if (minNode.NodeKey < deletingNode.NodeKey) return false;
                if (minNode.NodeKey == deletingNode.NodeKey)
                {
                    this.Root = null;
                    return true;
                }

                if (minNode.LeftChild == null && minNode.RightChild == null)
                {
                    minNode.Parent.LeftChild = minNode.Parent.LeftChild.Equals(minNode) ? null : minNode.Parent.LeftChild;
                    minNode.Parent.RightChild = minNode.Parent.RightChild.Equals(minNode) ? null : minNode.Parent.RightChild;


                    deletingNode.Parent.LeftChild = deletingNode.Parent.LeftChild.Equals(deletingNode) ? minNode : deletingNode.Parent.LeftChild;
                    deletingNode.Parent.RightChild = deletingNode.Parent.RightChild.Equals(deletingNode) ? minNode : deletingNode.Parent.RightChild;



                    minNode.LeftChild = deletingNode.LeftChild.NodeKey < minNode.NodeKey ? deletingNode.LeftChild : deletingNode.RightChild;
                    minNode.RightChild = deletingNode.RightChild.NodeKey > minNode.NodeKey ? deletingNode.RightChild : deletingNode.LeftChild;


                    minNode.Parent = deletingNode.Parent;
                    deletingNode.RightChild.Parent = minNode;
                    deletingNode.LeftChild.Parent = minNode;
                }
                else
                {
                    deletingNode.Parent.LeftChild = deletingNode.Parent.LeftChild.Equals(deletingNode) ? minNode : deletingNode.Parent.LeftChild;
                    deletingNode.Parent.RightChild = deletingNode.Parent.RightChild.Equals(deletingNode) ? minNode : deletingNode.Parent.RightChild;

                    minNode.LeftChild = deletingNode.LeftChild;
                    deletingNode.LeftChild.Parent = minNode;
                    minNode.Parent = deletingNode.Parent;

                }

                return true;
            }

        }

        public int Count()
        {
            if (this.Root == null)
                return 0;
            return 1 + CountStart(this.Root);
        }

        public int CountStart(BSTNode<T> node)
        {
            int count = 0;
            if (node.RightChild != null)
            {
                count++;
                count += CountStart(node.RightChild);
            }
            if (node.LeftChild != null)
            {
                count++;
                count += CountStart(node.LeftChild);
            }
            return count;
        }
        public List<LiteBSTNode> WideAllNodes()
        {
            List<LiteBSTNode> listOfNodes = new List<LiteBSTNode>();

            List<BSTNode<T>> bufferList = new List<BSTNode<T>>();

            bufferList.Add(Root);

            while (bufferList.Count > 0)
            {
                var bufferNode = bufferList[0];
                var bstNode = new LiteBSTNode(bufferNode.NodeKey);
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

        public List<LiteBSTNode> DeepAllNodes(int order)
        {
            return DeepAllNodesStart(Root, order);
        }

        public List<LiteBSTNode> DeepAllNodesStart(BSTNode<T> node, int order)
        {

            List<LiteBSTNode> listOfNodes = new List<LiteBSTNode>();

            if (node == null) return listOfNodes;

            // in order
            if (order == 0)
            {
                listOfNodes.AddRange(DeepAllNodesStart(node.LeftChild, order));
                listOfNodes.Add(new LiteBSTNode(node.NodeKey));
                listOfNodes.AddRange(DeepAllNodesStart(node.RightChild, order));
            }
            //post order
            else if (order == 1)
            {
                listOfNodes.AddRange(DeepAllNodesStart(node.LeftChild, order));
                listOfNodes.AddRange(DeepAllNodesStart(node.RightChild, order));
                listOfNodes.Add(new LiteBSTNode(node.NodeKey));
            }
            // pre order
            if (order == 2)
            {
                listOfNodes.Add(new LiteBSTNode(node.NodeKey));
                listOfNodes.AddRange(DeepAllNodesStart(node.LeftChild, order));
                listOfNodes.AddRange(DeepAllNodesStart(node.RightChild, order));
            }
            return listOfNodes;
        }
    }

    public class LiteBSTNode
    {
        public int NodeKey;

        public LiteBSTNode(int key)
        {
            NodeKey = key;
        }
    }
}
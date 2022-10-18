using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class SimpleTreeNode<T>
    {
        public T NodeValue; // значение в узле
        public SimpleTreeNode<T> Parent; // родитель или null для корня
        public List<SimpleTreeNode<T>> Children; // список дочерних узлов или null
        public int Level;

        public SimpleTreeNode(T val, SimpleTreeNode<T> parent)
        {
            NodeValue = val;
            Parent = parent;
            Children = null;
            Level = 0;
        }

        public SimpleTreeNode(T val)
        {
            NodeValue = val;
            Parent = null;
            Children = null;
            Level = 0;
        }
    }

    public class SimpleTree<T>
    {
        public SimpleTreeNode<T> Root;

        public SimpleTree(SimpleTreeNode<T> root)
        {
            Root = root;
            Root.Level = 0;
        }
        public SimpleTree()
        {
            Root = null;
        }

        public void AddChild(SimpleTreeNode<T> ParentNode, SimpleTreeNode<T> NewChild)
        {
            if (ParentNode == null || NewChild == null) return;
            if (ParentNode.Children == null)
                ParentNode.Children = new List<SimpleTreeNode<T>>(); 
            ParentNode.Children.Add(NewChild);
            if (NewChild.Children == null || NewChild.Children.Count == 0) 
            {
                NewChild.Parent = ParentNode;
                NewChild.Level = ParentNode.Level + 1;
            }
            else 
            {
                NewChild.Parent = ParentNode;
                this.AddLevelToAllNodes();
            }
        }

        public void DeleteNode(SimpleTreeNode<T> NodeToDelete)
        {
            if (NodeToDelete == null) return;
            var parentNode = NodeToDelete.Parent;
            if (parentNode == null) return;

            parentNode.Children.Remove(NodeToDelete);
            NodeToDelete.Parent = null;
        }

        public List<SimpleTreeNode<T>> GetAllNodes()
        {
            var listOfNodes = new List<SimpleTreeNode<T>>();
            if (this.Root == null) return listOfNodes;
            listOfNodes.Add(this.Root);
            listOfNodes.AddRange(BFS(this.Root));
            return listOfNodes;
        }

        
        private List<SimpleTreeNode<T>> BFS(SimpleTreeNode<T> currentNode)
        {
            if (currentNode.Children == null || currentNode.Children.Count == 0)
            {
                return new List<SimpleTreeNode<T>>();
            }
            var listOfNodes = new List<SimpleTreeNode<T>>();
            foreach (var node in currentNode.Children)
            {
                listOfNodes.Add(node);
            }
            foreach (var node in currentNode.Children)
            {
                listOfNodes.AddRange(BFS(node));
            }
            return listOfNodes;
        }

        public List<SimpleTreeNode<T>> FindNodesByValue(T val)
        {
            var listOfNodes = new List<SimpleTreeNode<T>>();
            if (this.Root == null) return listOfNodes;
            if (this.Root.NodeValue.Equals(val)) listOfNodes.Add(this.Root);
            listOfNodes.AddRange(BFSNodesByValue(this.Root, val));

            return listOfNodes;
        }

        private List<SimpleTreeNode<T>> BFSNodesByValue(SimpleTreeNode<T> currentNode,T val)
        {
            var listOfNodes = new List<SimpleTreeNode<T>>();
            if (currentNode.Children == null) return listOfNodes;
            foreach (var node in currentNode.Children)
            {
                if(node.NodeValue.Equals(val))
                listOfNodes.Add(node);
            }
            foreach (var node in currentNode.Children)
            {
                listOfNodes.AddRange(BFSNodesByValue(node, val));
            }
            return listOfNodes;
        }

        public void MoveNode(SimpleTreeNode<T> OriginalNode, SimpleTreeNode<T> NewParent)
        {
            if (OriginalNode == null || NewParent == null) return;
            var parentOfOriginalNode = OriginalNode.Parent;
            parentOfOriginalNode.Children.Remove(OriginalNode);
            OriginalNode.Parent = NewParent;

            if (NewParent.Children == null)
                NewParent.Children = new List<SimpleTreeNode<T>>();

            NewParent.Children.Add(OriginalNode);
            this.AddLevelToAllNodes();
        }
        public int Count()
        {if (this.Root == null) return 0;
            return 1 + BFSCount(this.Root);
        }

        private int BFSCount(SimpleTreeNode<T> currentNode)
        {
            if (currentNode.Children == null || currentNode.Children.Count == 0)
            {
                return 0;
            }
            var count = 0;
            foreach (var node in currentNode.Children)
            {
                count++;
            }
            foreach (var node in currentNode.Children)
            {
                count += BFSCount(node);
            }
            return count;
        }
        public int LeafCount()
        {
            return DFSLeafCount(this.Root);
        }
        private int DFSLeafCount(SimpleTreeNode<T> currentNode)
        {
            if (currentNode.Children == null || currentNode.Children.Count == 0)
            {
                return 1;
            }
            var count = 0;
            foreach (var node in currentNode.Children)
            {
                count += DFSLeafCount(node);
            }
            return count;
        }
        public void AddLevelToAllNodes() 
        {
            BFSAddLevelToAllNodes(this.Root,1);
        }
        private void BFSAddLevelToAllNodes(SimpleTreeNode<T> currentNode,int level)
        {
            if (currentNode.Children == null) return;
            foreach (var node in currentNode.Children)
            {
                node.Level = level;
            }
            foreach (var node in currentNode.Children)
            {
                BFSAddLevelToAllNodes(node, level+1);
            }
        }

        public bool FindNode(SimpleTreeNode<T> nodeToFind)
        {
            if (this.Root == null || nodeToFind == null) return false;
            return BFSNode(this.Root, nodeToFind);
        }

        private bool BFSNode(SimpleTreeNode<T> currentNode,SimpleTreeNode<T> nodeToFind)
        {
            if (currentNode.Children == null) return currentNode.Equals(nodeToFind);
            foreach (var node in currentNode.Children)
            {
                if (node.Equals(nodeToFind))
                    return true;
            }
            foreach (var node in currentNode.Children)
            {
                if (BFSNode(node, nodeToFind))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
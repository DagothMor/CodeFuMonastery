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
    }

    public class SimpleTree<T>
    {
        public SimpleTreeNode<T> Root; // корень, может быть null

        public SimpleTree(SimpleTreeNode<T> root)
        {
            Root = root;
            Root.Level = 0;
        }

        // Done.
        public void AddChild(SimpleTreeNode<T> ParentNode, SimpleTreeNode<T> NewChild)
        {
            if (ParentNode == null || NewChild == null) return;
            ParentNode.Children.Add(NewChild);
            NewChild.Parent = ParentNode;
            NewChild.Level = ParentNode.Level + 1;
        }

        // Done.
        public void DeleteNode(SimpleTreeNode<T> NodeToDelete)
        {
            if (NodeToDelete == null) return;
            var parentNode = NodeToDelete.Parent;
            if (parentNode == null) return;

            parentNode.Children.Remove(NodeToDelete);
            NodeToDelete.Parent = null;
        }

        // Done.
        public List<SimpleTreeNode<T>> GetAllNodes()
        {
            return BFS(this.Root);
        }

        
        public List<SimpleTreeNode<T>> BFS(SimpleTreeNode<T> currentNode)
        {
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

        // Done.
        public List<SimpleTreeNode<T>> FindNodesByValue(T val)
        {
            return BFSNodesByValue(this.Root, val);
        }

        public List<SimpleTreeNode<T>> BFSNodesByValue(SimpleTreeNode<T> currentNode,T val)
        {
            var listOfNodes = new List<SimpleTreeNode<T>>();
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

        //Done.
        public void MoveNode(SimpleTreeNode<T> OriginalNode, SimpleTreeNode<T> NewParent)
        {
            // ваш код перемещения узла вместе с его поддеревом -- 
            // в качестве дочернего для узла NewParent
            var parentOfOriginalNode = OriginalNode.Parent;
            parentOfOriginalNode.Children.Remove(OriginalNode);
            OriginalNode.Parent = NewParent;
            NewParent.Children.Add(OriginalNode);
        }

        // Done.
        public int Count()
        {
            return BFSCount(this.Root);
        }

        public int BFSCount(SimpleTreeNode<T> currentNode)
        {
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

        //Done
        public int LeafCount()
        {
            return DFSLeafCount(this.Root);
        }
        public int DFSLeafCount(SimpleTreeNode<T> currentNode)
        {
            if (currentNode.Children.Count == 0)
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

        // Done.
        public void AddLevelToAllNodes() 
        {
            BFSAddLevelToAllNodes(this.Root,1);
        }
        public void BFSAddLevelToAllNodes(SimpleTreeNode<T> currentNode,int level)
        {
            foreach (var node in currentNode.Children)
            {
                node.Level = level;
            }
            foreach (var node in currentNode.Children)
            {
                BFSAddLevelToAllNodes(node, level+1);
            }
        }
    }

}
using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class BSTNode<T>
    {
        public int NodeKey; // ключ узла
        public T NodeValue; // значение в узле
        public BSTNode<T> Parent; // родитель или null для корня
        public BSTNode<T> LeftChild; // левый потомок
        public BSTNode<T> RightChild; // правый потомок	

        public BSTNode(int key, T val, BSTNode<T> parent)
        {
            NodeKey = key;
            NodeValue = val;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        }
    }

    // промежуточный результат поиска
    public class BSTFind<T>
    {
        // null если в дереве вообще нету узлов
        public BSTNode<T> Node;

        // true если узел найден
        public bool NodeHasKey;

        // true, если родительскому узлу надо добавить новый левым
        public bool ToLeft;

        public BSTFind() { Node = null; }
    }

    public class BST<T>
    {
        BSTNode<T> Root; // корень дерева, или null

        public BST(BSTNode<T> node)
        {
            Root = node;
        }

        public BSTFind<T> FindNodeByKey(int key)
        {
            // ищем в дереве узел и сопутствующую информацию по ключу
            var rootNode = new BSTFind<T>();
            rootNode.Node = this.Root;
            return FindNodeByKeyStart(key, rootNode);
        }

        public BSTFind<T> FindNodeByKeyStart(int key, BSTFind<T> bufferNode)
        {
            if (bufferNode.Node.NodeKey.Equals(key))
            {
                bufferNode.NodeHasKey = true;
                return bufferNode;
            }
            // если лист
            if (bufferNode.Node.LeftChild == null && bufferNode.Node.RightChild == null)
            {
                bufferNode.ToLeft = bufferNode.Node.NodeKey < key;
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
            // ищем в дереве узел и сопутствующую информацию по ключу
        }

        public bool AddKeyValue(int key, T val)
        {
            var foundedNode = FindNodeByKey(key);

            if (foundedNode.NodeHasKey) 
            {
                return false; // если ключ уже есть
            }
            if (foundedNode.ToLeft)  foundedNode.Node.LeftChild = new BSTNode<T>(key, val, foundedNode.Node); 
            else foundedNode.Node.RightChild = new BSTNode<T>(key, val,foundedNode.Node);
            return true;
        }

        public BSTNode<T> FinMinMax(BSTNode<T> FromNode, bool FindMax)
        {
            if (FromNode.LeftChild == null && !FindMax) return FromNode;
            if (FromNode.RightChild == null && FindMax) return FromNode;
            return FindMax? FinMinMax(FromNode.RightChild, FindMax) : FinMinMax(FromNode.LeftChild, FindMax);
        }



        public bool DeleteNodeByKey(int key)
        {
            // удаляем узел по ключу
            return false; // если узел не найден
        }

        public int Count()
        {
            return 0; // количество узлов в дереве
        }

    }
}
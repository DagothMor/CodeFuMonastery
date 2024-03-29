﻿using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class Vertex<T>
    {
        public bool Hit;
        public T Value;
        public Vertex(T val)
        {
            Value = val;
            Hit = false;
        }
    }

    public class Vertex
    {
        public int Value;
        public Vertex(int val)
        {
            Value = val;
        }
    }

    public class SimpleGraph<T>
    {
        public Vertex<T>[] vertex;
        public int[,] m_adjacency;
        public int max_vertex;

        public SimpleGraph(int size)
        {
            max_vertex = size;
            m_adjacency = new int[size, size];
            vertex = new Vertex<T>[size];
        }

        public void AddVertex(T value)
        {
            for (int i = 0; i < max_vertex; i++)
            {
                if (vertex[i] == null)
                {
                    var newVertex = new Vertex<T>(value);
                    vertex[i] = newVertex;
                    return;
                }
            }
        }

        public void RemoveVertex(int v)
        {
            vertex[v] = null;
            for (int i = 0; i < max_vertex; i++)
            {
                RemoveEdge(v, i);
            }
        }

        public bool IsEdge(int v1, int v2)
        {
            if (m_adjacency[v1, v2] == 1 || m_adjacency[v2, v1] == 1) return true;
            return false;
        }

        public void AddEdge(int v1, int v2)
        {
            m_adjacency[v1, v2] = 1;
            m_adjacency[v2, v1] = 1;
        }

        public void RemoveEdge(int v1, int v2)
        {
            m_adjacency[v1, v2] = 0;
            m_adjacency[v2, v1] = 0;
        }
        public List<Vertex<T>> DepthFirstSearch(int VFrom, int VTo)
        {
            if (VFrom < 0 ||
                VTo < 0 ||
                VFrom >= this.max_vertex ||
                VTo >= this.max_vertex ||
                this.vertex[VFrom] == null ||
                this.vertex[VTo] == null) return new List<Vertex<T>>();

            var stack = new Stack<int>();
            stack.Clear();
            for (int i = 0; i < max_vertex; i++)
            {
                if (this.vertex[i] != null) this.vertex[i].Hit = false;
            }

            var currentVertex = this.vertex[VFrom];

            stack.Push(VFrom);

            if (this.IsEdge(VFrom, VTo))
            {
                stack.Push(VTo);
                return GetVertexes(stack);
            }

            bool hasHit = false;

            while (stack.Count != 0)
            {
                VFrom = stack.Peek();

                this.vertex[VFrom].Hit = true;

                hasHit = false;

                if (this.IsEdge(VFrom, VTo))
                {
                    stack.Push(VTo);
                    return GetVertexes(stack);
                }

                for (int i = 0; i < max_vertex; i++)
                {
                    if (this.IsEdge(VFrom, i) && !this.vertex[i].Hit)
                    {
                        VFrom = i;
                        hasHit = true;
                        break;
                    }
                }
                if (hasHit) { stack.Push(VFrom); continue; }
                else { stack.Pop(); }
            }
            return new List<Vertex<T>>();
        }

        public List<Vertex<T>> BreadthFirstSearch(int VFrom, int VTo)
        {
            if (VFrom < 0 ||
                VTo < 0 ||
                VFrom >= this.max_vertex ||
                VTo >= this.max_vertex ||
                this.vertex[VFrom] == null ||
                this.vertex[VTo] == null) return new List<Vertex<T>>();

            var queue = new Queue<int>();
            var parents = new int[this.max_vertex];
            parents[VFrom] = -1;

            for (int i = 0; i < max_vertex; i++)
            {
                if (this.vertex[i] != null) this.vertex[i].Hit = false;
            }

            queue.Enqueue(VFrom);

            while (queue.Count != 0)
            {
                VFrom = queue.Dequeue();

                this.vertex[VFrom].Hit = true;

                for (int i = 0; i < max_vertex; i++)
                {
                    if (this.IsEdge(VFrom, i) && !this.vertex[i].Hit && !queue.Contains(i))
                    {
                        queue.Enqueue(i);
                        parents[i] = VFrom;
                    }
                    if (this.IsEdge(VFrom, i) && !this.vertex[i].Hit && this.vertex[i].Equals(this.vertex[VTo]))
                    {
                        return GetVertexes(i, parents);
                    }
                }
            }
            return new List<Vertex<T>>();
        }


        public List<Vertex<T>> WeakVertices()
        {
            if (this.max_vertex == 0 || this.vertex[0] == null) return new List<Vertex<T>>();

            var VFrom = 0;

            var queue = new Queue<int>();

            var weakVertices = new List<Vertex<T>>();

            for (int i = 0; i < max_vertex; i++)
            {
                if (this.vertex[i] != null) this.vertex[i].Hit = false;
            }

            queue.Enqueue(VFrom);

            while (queue.Count != 0)
            {
                VFrom = queue.Dequeue();

                this.vertex[VFrom].Hit = true;

                var neighbours = new List<int>();

                var isTriangle = false;

                for (int i = 0; i < max_vertex; i++)
                {
                    if (HasEdge(VFrom, i))
                    {
                        neighbours.Add(i);
                    }
                }
                for (int i = 0; i < max_vertex; i++)
                {
                    if (neighbours.Contains(i) && NotVisited(i) && NotContainsInQueue(queue, i))
                    {
                        queue.Enqueue(i);
                    }
                }
                foreach (var neighbour in neighbours)
                {
                    for (int i = 0; i < max_vertex; i++)
                    {
                        if (i == VFrom || neighbour == i) continue;
                        if (HasEdge(neighbour, i) && HasEdge(VFrom, i))
                        {
                            isTriangle = true;
                            break;
                        }
                    }
                    if (isTriangle) { break; }
                }
                if (!isTriangle) { weakVertices.Add(this.vertex[VFrom]); }
            }
            return weakVertices;
        }

        private static bool NotContainsInQueue(Queue<int> queue, int i)
        {
            return !queue.Contains(i);
        }

        private bool NotVisited(int i)
        {
            return !this.vertex[i].Hit;
        }

        private bool HasEdge(int VFrom, int i)
        {
            return this.IsEdge(VFrom, i);
        }

        private List<Vertex<T>> GetVertexes(int VTo, int[] parents)
        {
            var list = new List<Vertex<T>>();
            for (int v = VTo; v != -1; v = parents[v])
            {
                list.Add(this.vertex[v]);
            }
            list.Reverse();
            return list;
        }
        private List<Vertex<T>> GetVertexes(Stack<int> stack)
        {
            var list = new List<Vertex<T>>();
            var reverseStack = new Stack<int>();

            while (stack.Count != 0)
            {
                reverseStack.Push(stack.Pop());
            }
            while (reverseStack.Count != 0)
            {
                list.Add(this.vertex[reverseStack.Pop()]);
            }
            return list;
        }
    }


}
using System;
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

        private List<Vertex<T>> DFSStart(int VFrom, int VTo,Stack<int> stack) 
        {
            return null;
        }
        public List<Vertex<T>> DepthFirstSearch(int VFrom, int VTo)
        {
            if (VFrom < 0 || VTo<0 || VFrom >= this.max_vertex || VTo >= this.max_vertex) return null;
           
            var stack = new Stack<int>();
            stack.Clear();
            for (int i = 0; i < max_vertex; i++)
            {
                this.vertex[i].Hit = false;
            }

            //1 Выбираем текущую вершину X. Для начала работы это будет исходная вершина А.
            var currentVertex = this.vertex[VFrom];
            //2 Фиксируем вершину X как посещённую.
            //currentVertex.Hit = true;
            //3 Помещаем вершину X в стек.
            stack.Push(VFrom);
            //4 Ищем среди смежных вершин вершины X целевую вершину Б. Если она найдена,
            //записываем её в стек и возвращаем сам стек как результат работы (путь из А в Б).
            // Если целевой вершины среди смежных нету, то выбираем среди смежных такую вершину, которая ещё не была посещена.
            // Если такая вершина найдена, делаем её текущей X и переходим к п. 2.

            if (this.IsEdge(VFrom, VTo))
            {
                stack.Push(VTo);
                return GetVertexes(stack);
            }

            //5 Если непосещённых смежных вершин более нету, удаляем из стека верхний элемент.

            // Если стек пуст, то прекращаем работу и информируем, что путь не найден.
            // В противном случае делаем текущей вершиной X верхний элемент стека,
            // помечаем его как посещённый, и после чего переходим к п. 4.
            bool hasHit = false;

            while (stack.Count !=0)
            {
                //currentVertex = stack.Pop();
                this.vertex[VFrom].Hit = true;
                hasHit = false;
                //4 Ищем среди смежных вершин вершины X целевую вершину Б. Если она найдена,
                //записываем её в стек и возвращаем сам стек как результат работы (путь из А в Б).
                if (this.IsEdge(VFrom, VTo))
                {
                    stack.Push(VTo);
                    return GetVertexes(stack);
                }

                // Если целевой вершины среди смежных нету, то выбираем среди смежных такую вершину, которая ещё не была посещена.
                for (int i = 0; i < max_vertex; i++)
                {
                    if (this.IsEdge(VFrom, i) && !this.vertex[i].Hit)
                    {
                        // Если такая вершина найдена, делаем её текущей X и переходим к п. 2.
                        VFrom = i;
                        hasHit = true;
                        break;
                    }
                }
                if (hasHit) { stack.Push(VFrom); continue; }
                else { stack.Pop(); VFrom = stack.Peek(); }
            }
            return null;
        }
        private List<Vertex<T>> GetVertexes(Stack<int> stack) 
        {
            var list = new List<Vertex<T>>();
            var reverseStack = new Stack<int>();

            while (stack.Count !=0)
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
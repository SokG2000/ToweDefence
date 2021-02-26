using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Field
{
    public partial class VertexDiconnectivityComponents
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;
        private Graph m_Graph;
        private List<int> m_Component;
        private List<int> m_Up;
        private List<int> m_Depth;
        private List<bool> m_Visited;

        public VertexDiconnectivityComponents(Grid mGrid, Vector2Int mTarget, Vector2Int mStart)
        {
            m_Grid = mGrid;
            m_Target = mTarget;
            m_Start = mStart;
            m_Graph = new Graph(mGrid);
        }

        private int FindComponentsDfs(ref Stack<int> edgeStack, ref List<bool> visitedEdge, int prevVertex, int prevEdge, int currentComponent, int depth, int u)
        {
            m_Depth[u] = depth;
            m_Visited[u] = true;
            int up = depth - 1;
            foreach (Graph.Connection connection in m_Graph.GetNeighbours(u))
            {
                int w = connection.vertex;
                int e = connection.edgeId;
                if (!visitedEdge[e])
                {
                    visitedEdge[e] = true;
                    edgeStack.Push(e);
                }

                if (m_Visited[w])
                {
                    up = Math.Min(up, m_Depth[w]);
                }
                else
                {
                    currentComponent = FindComponentsDfs(ref edgeStack, ref visitedEdge, u, e, currentComponent, depth + 1,w);
                    up = Math.Min(up, m_Up[w]);
                }
            }
            m_Up[u] = up;
            if (m_Up[u] == depth - 1 && depth > 0)
            {
                while (edgeStack.Count > 0 && edgeStack.Peek() != prevEdge)
                {
                    m_Component[edgeStack.Pop()] = currentComponent;
                }

                if (edgeStack.Count > 0)
                {
                    m_Component[edgeStack.Pop()] = currentComponent;
                }
                ++currentComponent;
            }
            return currentComponent;
        }

        /// <summary>
        /// Разбивает рёбра графа на компоненты вершинной двусвязности
        /// </summary>
        private void FindComponents()
        {
            int vertexNum = m_Graph.MVertexNum;
            int edgeNum = m_Graph.MEdgeNum;
            m_Component = new List<int>(edgeNum);
            for (int i = 0; i < edgeNum; ++i)
            {
                m_Component.Add(-1);
            }

            // Не нашёл как нормально создавать заполненный начальными значениями List данного размера
            m_Up = new List<int>(vertexNum);
            m_Depth = new List<int>(vertexNum);
            m_Visited = new List<bool>(vertexNum);
            for (int i = 0; i < vertexNum; ++i)
            {
                m_Up.Add(-1);
                m_Depth.Add(-1);
                m_Visited.Add(false);
            }
            Stack<int> edgeStack = new Stack<int>();
            List<bool> visitedEdge = new List<bool>(edgeNum);
            for (int i = 0; i < edgeNum; ++i)
            {
                visitedEdge.Add(false);
            }
            FindComponentsDfs(ref edgeStack, ref visitedEdge, -1, -1, 0, 0, 0);
        }

        private void GetWayDFS(ref List<Graph.Connection> prev, ref List<bool> used, int u)
        {
            used[u] = true;
            foreach (Graph.Connection connection in m_Graph.GetNeighbours(u))
            {
                int v = connection.vertex;
                if (!used[v])
                {
                    prev[v] = new Graph.Connection(u, connection.edgeId);
                    GetWayDFS(ref prev, ref used, v);
                }
            }
        }
        
        private List<Graph.Connection> GetWay(int start, int finish, int vertexNum)
        {
            List<Graph.Connection> prev = new List<Graph.Connection>(vertexNum);
            List<bool> used = new List<bool>(vertexNum);
            for (int i = 0; i < vertexNum; i++)
            {
                prev.Add(new Graph.Connection());
                used.Add(false);
            }
            GetWayDFS(ref prev, ref used, finish);
            List<Graph.Connection> res = new List<Graph.Connection>();
            if (!used[start])
            {
                return res;
            }
            int v = start;
            while (v != finish)
            {
                res.Add(prev[v]);
                v = prev[v].vertex;
            }
            return res;
        } 

        /// <summary>
        /// Выставляет корректные значения OccupationAvailability для текущего положения поля.
        /// Для уже занятых клеток OccupationAvailability = true.
        /// </summary>
        public void SetOccupationAvailability()
        {
            // Нельзя занимать start, target и такие вершины на пути из start в target,
            // что инцидентные им рёбра с этого пути лежат в разных компонентах вершинной двусвязности
            m_Graph.ReadCurrentPosition();
            int vertexNum = m_Graph.MVertexNum;
            int edgeNum = m_Graph.MEdgeNum;
            FindComponents();
            m_Grid.SetAllAvailable();
            m_Grid.GetNode(m_Start).OccupationAvailability = false;
            m_Grid.GetNode(m_Target).OccupationAvailability = false;
            int start = m_Graph.GetVertexIndex(m_Start);
            int finish = m_Graph.GetVertexIndex(m_Target);
            List<Graph.Connection> way = GetWay(start, finish, vertexNum);
            for (int i = 0; i < way.Count - 1; ++i)
            {
                if (m_Component[way[i].edgeId] != m_Component[way[i + 1].edgeId])
                {
                    m_Grid.GetNode(m_Graph.GetVertexCoordinate(way[i].vertex)).OccupationAvailability = false;
                }
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Graph
    {
        public struct Connection
        {
            public int vertex, edgeId;

            public Connection(int vertex, int edgeId)
            {
                this.vertex = vertex;
                this.edgeId = edgeId;
            }
        }
        private Grid m_Grid;
        private int[,] m_VertexIndex; // index of vertex if it is empty and -1 if occupied
        private int m_Width;
        private int m_Height;
        private List<Vector2Int> m_VertexCoordinates; // coordinates by index
        private List<List<Connection>> m_Graph; // list of edges for every vertex
        private int m_VertexNum;
        private int m_EdgeNum;

        public Graph(Grid mGrid)
        {
            m_Grid = mGrid;
            m_Width = mGrid.Width;
            m_Height = mGrid.Height;
            m_VertexIndex = new int[m_Width, m_Height];
            //m_VertexCoordinates = new List<Vector2Int>();
            //m_Graph = new List<List<Connection>>();
            ReadCurrentPosition();
        }

        public void ReadCurrentPosition()
        {
            m_VertexCoordinates = new List<Vector2Int>();
            m_Graph = new List<List<Connection>>();
            int currentIndex = 0;
            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; ++j)
                {
                    if (!m_Grid.GetNode(i, j).isOccupied)
                    {
                        m_VertexCoordinates.Add(new Vector2Int(i, j));
                        m_Graph.Add(new List<Connection>());
                        m_VertexIndex[i, j] = currentIndex;
                        ++currentIndex;
                    }
                    else
                    {
                        m_VertexIndex[i, j] = -1;
                    }
                }
            }
            m_VertexNum = currentIndex;
            
            currentIndex = 0;
            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    int u = m_VertexIndex[i, j];
                    if (u != -1)
                    {
                        if (i < m_Width - 1 && m_VertexIndex[i + 1, j] != -1)
                        {
                            int w = m_VertexIndex[i + 1, j];
                            m_Graph[u].Add(new Connection(w, currentIndex));
                            m_Graph[w].Add(new Connection(u, currentIndex));
                            ++currentIndex;
                        }

                        if (j < m_Height - 1 && m_VertexIndex[i, j + 1] != -1)
                        {
                            int w = m_VertexIndex[i, j + 1];
                            m_Graph[u].Add(new Connection(w, currentIndex));
                            m_Graph[w].Add(new Connection(u, currentIndex));
                            ++currentIndex;
                        }
                    }
                }
            }
            m_EdgeNum = currentIndex;
        }
        
        public IEnumerable<Connection> GetNeighbours(int vertex)
        {
            foreach (Connection connection in m_Graph[vertex])
            {
                yield return connection;
            }
        }

        public Vector2Int GetVertexCoordinate(int v)
        {
            if (v < 0 || v > m_VertexNum)
            {
                return new Vector2Int(-1, -1);
            }
            return m_VertexCoordinates[v];
        }

        public int GetVertexIndex(int x, int y)
        {
            if (x < 0 || x > m_Width || y < 0 || y > m_Height)
            {
                return -1;
            }
            return m_VertexIndex[x, y];
        }

        public int GetVertexIndex(Vector2Int position)
        {
            return GetVertexIndex(position.x, position.y);
        }

        public int MWidth => m_Width;

        public int MHeight => m_Height;

        public int MVertexNum => m_VertexNum;

        public int MEdgeNum => m_EdgeNum;
    }
}
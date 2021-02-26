using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;
        private int m_Width;
        private int m_Height;


        public int Width => m_Width;

        public int Height => m_Height;

        private FlowFieldPathfinding m_Pathfinding;
        private VertexDiconnectivityComponents m_AvailabilityChecking;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int target, Vector2Int start)
        {
            m_Height = height;
            m_Width = width;
            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + 0.5f, 0f, j + 0.5f) * nodeSize);
                }
            }

            m_Pathfinding = new FlowFieldPathfinding(this, target);
            m_Pathfinding.UpdateField();
            m_AvailabilityChecking = new VertexDiconnectivityComponents(this, target, start);
            UpdateOccupationAvailability();
        }

        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }

        public bool CanOccupyNode(Vector2Int coordinate)
        {
            if (coordinate.x < 0 || coordinate.x >= m_Width)
            {
                return false;
            }

            if (coordinate.y < 0 || coordinate.y >= m_Height)
            {
                return false;
            }
            // TODO If some enemy is going on this cell return false
            return m_Nodes[coordinate.x, coordinate.y].OccupationAvailability;
        }

        public bool TryOccupyNode(Vector2Int coordinate)
        {
            if (!CanOccupyNode(coordinate))
            {
                return false;
            }
            m_Nodes[coordinate.x, coordinate.y].isOccupied = true;
            UpdatePathfinding();
            UpdateOccupationAvailability();
            // TODO Enemies that are not in this node but are going to this node change target
            return true;
        }

        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }
            return m_Nodes[i, j];
        }

        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; ++j)
                {
                    yield return m_Nodes[i, j];
                }
            }
        }

        public void UpdatePathfinding()
        {
            m_Pathfinding.UpdateField();
        }

        public void UpdateOccupationAvailability()
        {
            // TODO write this method
            m_AvailabilityChecking.SetOccupationAvailability();
        }

        public void SetAllAvailable()
        {
            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    m_Nodes[i, j].OccupationAvailability = true;
                }
            }
        }
    }
}
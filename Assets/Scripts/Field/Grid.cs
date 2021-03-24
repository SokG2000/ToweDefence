using System;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;
        private int m_Width;
        private int m_Height;
        
        private float m_NodeSize;
        private Vector3 m_Offset;

        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;

        public int Width => m_Width;

        public int Height => m_Height;

        private FlowFieldPathfinding m_Pathfinding;
        private VertexDiconnectivityComponents m_AvailabilityChecking;

        private Node m_SelectedNode = null;
        
        public Vector3 GetCellCenter(Vector2Int coordinateOnGrid)
        {
            float xShift = (coordinateOnGrid.x + 0.5f) * m_NodeSize;
            float zShift = (coordinateOnGrid.y + 0.5f) * m_NodeSize;
            Vector3 shift = new Vector3(xShift, 0f, zShift);
            return m_Offset + shift;
        }

        public Vector2Int GetCellCoordinateOnGrid(Vector3 coordinateOnPlane)
        {
            Vector3 difference = coordinateOnPlane - m_Offset;
            int x = (int) (difference.x / m_NodeSize);
            int y = (int) (difference.z / m_NodeSize);
            return new Vector2Int(x, y);
        }

        public Node GetNodeAtPoint(Vector3 coordinateOnPlane)
        {
            return GetNode(GetCellCoordinateOnGrid(coordinateOnPlane));
        }

        private float OneDimensionalDistanceFromPointToSegment(float min_end, float max_end, float point)
        {
            if (min_end <= point && point <= max_end)
            {
                return 0f;
            }

            if (point < min_end)
            {
                return min_end - point;
            }
            return point - max_end;
        }
        private float SqrDistanceFromPointToNode(Node node, Vector3 point)
        {
            Vector3 nodePosition = node.Position;
            float x_dist = OneDimensionalDistanceFromPointToSegment(nodePosition.x - m_NodeSize / 2,
                nodePosition.x + m_NodeSize / 2, point.x);
            float z_dist = OneDimensionalDistanceFromPointToSegment(nodePosition.z - m_NodeSize / 2,
                nodePosition.z + m_NodeSize / 2, point.z);
            return x_dist * x_dist + z_dist * z_dist;
        }

        private float DistanceFromPointToNode(Node node, Vector3 point)
        {
            return Mathf.Sqrt(SqrDistanceFromPointToNode(node, point));
        }
        
        private class NodeDistanceComparer: IComparer<Node>
        {
            private Vector3 point;
            private Grid m_Grid;

            public NodeDistanceComparer(Vector3 point, Grid grid)
            {
                this.point = point;
                m_Grid = grid;
            }

            public int Compare(Node x, Node y)
            {
                return m_Grid.DistanceFromPointToNode(x, point).CompareTo(m_Grid.DistanceFromPointToNode(y, point));
            }
        }

        public List<Node> GetNodesInCircle(Vector3 point, float radius, bool needSort=true)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; ++j)
                {
                    if (DistanceFromPointToNode(m_Nodes[i, j], point) < radius)
                    {
                        nodes.Add(m_Nodes[i, j]);
                    }
                }
            }
            if (needSort)
            {
                nodes.Sort(new NodeDistanceComparer(point, this));
            }
            return nodes;
        }

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int target, Vector2Int start)
        {
            m_Height = height;
            m_Width = width;
            m_NodeSize = nodeSize;
            m_Offset = offset;

            m_StartCoordinate = start;
            m_TargetCoordinate = target;
            
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

        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }

        public void SelectCoordinate(Vector2Int coodinate)
        {
            m_SelectedNode = GetNode(coodinate);
        }

        public void UnselectNode()
        {
            m_SelectedNode = null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }

        public bool HaveSelectedNode()
        {
            return m_SelectedNode != null;
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

        public bool CanOccupyNode(Node node)
        {
            return node.OccupationAvailability;
        }

        public bool TryOccupyNode(Node node)
        {
            if (!CanOccupyNode(node))
            {
                return false;
            }
            node.isOccupied = true;
            UpdatePathfinding();
            UpdateOccupationAvailability();
            // TODO Enemies that are not in this node but are going to this node change target
            return true;
        }

        public bool TryOccupyNode(Vector2Int coordinate)
        {
            return CanOccupyNode(m_Nodes[coordinate.x, coordinate.y]);
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
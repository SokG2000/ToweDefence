using System;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class FlowFieldPathfinding
    {
        private static float m_Sqrt = (float) Math.Sqrt(2f);
        private struct Connection
        {
            public Vector2Int position;
            public float distance;

            public Connection(Vector2Int position, float distance)
            {
                this.position = position;
                this.distance = distance;
            }
        }
        
        private class ConnectionComparer : IComparer<Connection>
        {
            public int Compare(Connection x, Connection y)
            {
                if (x.distance.CompareTo(y.distance) != 0)
                {
                    return x.distance.CompareTo(y.distance);
                }
                if (x.position.x.CompareTo(y.position.x) != 0)
                {
                    return x.position.x.CompareTo(y.position.x);
                }
                if (x.position.y.CompareTo(y.position.y) != 0)
                {
                    return x.position.y.CompareTo(y.position.y);
                }
                return 0;
            }
        }
        
        private Grid m_Grid;
        private Vector2Int Target;

        public FlowFieldPathfinding(Grid mGrid, Vector2Int target)
        {
            m_Grid = mGrid;
            Target = target;
        }

        public void UpdateField()
        {
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
            }
            m_Grid.GetNode(Target).PathWeight = 0f;

            SortedSet<Connection> unstablePositions = new SortedSet<Connection>(new ConnectionComparer());
            unstablePositions.Add(new Connection(Target, 0f));
            while (unstablePositions.Count != 0)
            {
                Connection current = unstablePositions.Min;
                unstablePositions.Remove(current);
                //Vector2Int currentPosition = current.position;
                Node currentNode = m_Grid.GetNode(current.position);
                foreach (Connection neigbour in GetNeighbours(current.position))
                {
                    float newWeight = current.distance + neigbour.distance;
                    Node neigbourNode = m_Grid.GetNode(neigbour.position);
                    if (newWeight < neigbourNode.PathWeight)
                    {
                        unstablePositions.Remove(new Connection(neigbour.position, neigbourNode.PathWeight));
                        neigbourNode.PathWeight = current.distance + neigbour.distance;
                        neigbourNode.NextNode = currentNode;
                        unstablePositions.Add(new Connection(neigbour.position, newWeight));
                    }
                }
            }

            /*Queue<Vector2Int> queue = new Queue<Vector2Int>();
            
            queue.Enqueue(Target);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);
                float weightToTarget = currentNode.PathWeight + 1f;

                foreach (Vector2Int neighbour in GetNeighbours(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour);
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour);
                    }
                }
            }
            */


        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;
            Vector2Int rightDownCoordinate = rightCoordinate + Vector2Int.down;
            Vector2Int rightUpCoordinate = rightCoordinate + Vector2Int.up;
            Vector2Int leftDownCoordinate = leftCoordinate + Vector2Int.down;
            Vector2Int leftUpCoordinate = leftCoordinate + Vector2Int.up;
            /*Debug.Log(coordinate);
            Debug.Log(rightCoordinate);
            Debug.Log(leftCoordinate);
            Debug.Log(upCoordinate);
            Debug.Log(downCoordinate);*/

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).isOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).isOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).isOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).isOccupied;
            bool hasRightDownNode = hasRightNode && hasDownNode && !m_Grid.GetNode(rightDownCoordinate).isOccupied;
            bool hasRightUpNode = hasRightNode && hasUpNode && !m_Grid.GetNode(rightUpCoordinate).isOccupied;
            bool hasLeftDownNode = hasLeftNode && hasDownNode && !m_Grid.GetNode(leftDownCoordinate).isOccupied;
            bool hasLeftUpNode = hasLeftNode && hasUpNode && !m_Grid.GetNode(leftUpCoordinate).isOccupied;
            Debug.Log(rightCoordinate + hasRightNode.ToString());
            Debug.Log(leftCoordinate + hasLeftNode.ToString());
            Debug.Log(upCoordinate + hasUpNode.ToString());
            Debug.Log(downCoordinate + hasDownNode.ToString());

            if (hasRightNode)
            {
                yield return new Connection(rightCoordinate, 1f);
            }
            if (hasLeftNode)
            {
                yield return new Connection(leftCoordinate, 1f);
            }
            if (hasUpNode)
            {
                yield return new Connection(upCoordinate, 1f);
            }
            if (hasDownNode)
            {
                yield return new Connection(downCoordinate, 1f);
            }

            if (hasRightDownNode)
            {
                yield return new Connection(rightDownCoordinate, m_Sqrt);
            }
            if (hasRightUpNode)
            {
                yield return new Connection(rightUpCoordinate, m_Sqrt);
            }
            if (hasLeftDownNode)
            {
                yield return new Connection(leftDownCoordinate, m_Sqrt);
            }
            if (hasLeftUpNode)
            {
                yield return new Connection(leftUpCoordinate, m_Sqrt);
            }
        }
    }

}
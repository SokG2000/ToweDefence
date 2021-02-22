using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class FlowFieldPathfinding
    {
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
            
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            
            queue.Enqueue(Target);
            m_Grid.GetNode(Target).PathWeight = 0f;

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
        }

        private IEnumerable<Vector2Int> GetNeighbours(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).isOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).isOccupied;
            bool hasUpNode = downCoordinate.y < m_Grid.Height && !m_Grid.GetNode(downCoordinate).isOccupied;
            bool hasDownNode = upCoordinate.y >= 0 && !m_Grid.GetNode(upCoordinate).isOccupied;

            if (hasRightNode)
            {
                yield return rightCoordinate;
            }
            if (hasLeftNode)
            {
                yield return leftCoordinate;
            }
            if (hasUpNode)
            {
                yield return upCoordinate;
            }
            if (hasDownNode)
            {
                yield return downCoordinate;
            }
        }
    }

}
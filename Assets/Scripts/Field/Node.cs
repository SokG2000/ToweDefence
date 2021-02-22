using UnityEngine;

namespace Field
{
    public class Node
    {
        public Vector3 m_Position;
        
        public Node NextNode;
        public bool isOccupied;

        public float PathWeight;

        public Node(Vector3 position)
        {
            m_Position = position;
        }
        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}
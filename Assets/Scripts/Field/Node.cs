﻿using UnityEngine;

namespace Field
{
    public class Node
    {
        public Vector3 Position;
        public bool OccupationAvailability;
        
        public Node NextNode;
        public bool isOccupied;

        public float PathWeight;

        public Node(Vector3 position)
        {
            Position = position;
            OccupationAvailability = true;
        }
        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}
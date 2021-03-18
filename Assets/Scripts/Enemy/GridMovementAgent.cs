using System.Collections;
using System.Collections.Generic;
using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{

    public class GridMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;

        public GridMovementAgent(float mSpeed, Transform mTransform, Grid grid)
        {
            m_Speed = mSpeed;
            m_Transform = mTransform;

            SetTargetNode(grid.GetStartNode());
        }

        private Node m_TargetNode;

        private const float TOLERANCE = 0.1f;

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            float distance = (target - m_Transform.position).magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            Vector3 dir = target - m_Transform.position;
            dir.y = 0f;
            Vector3 delta = dir.normalized * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}
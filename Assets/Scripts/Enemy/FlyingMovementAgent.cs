﻿using System.Collections;
using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        private EnemyData m_Data;

        public FlyingMovementAgent(float speed, Transform transform, Grid grid, EnemyData enemyData)
        {
            m_Speed = speed;
            m_Transform = transform;
            m_Data = enemyData;
            SetTargetNode(grid.GetTargetNode());
            Node node = Game.Player.Grid.GetNodeAtPoint(transform.position);
            node.EnemyDatas.Add(enemyData);
        }
        
        private Node m_TargetNode;

        private const float TOLERANCE = 0.1f;

        public void TickMovement()
        {
            Node oldNode = Game.Player.Grid.GetNodeAtPoint(m_Transform.position);
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            target.y = m_Transform.position.y;
            float distance = (target - m_Transform.position).magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }
        
            Vector3 dir = target - m_Transform.position;
            dir.y = 0f;
            Vector3 delta = dir.normalized * (m_Speed * Time.deltaTime);
            //Debug.Log(delta);
            m_Transform.Translate(delta);
            Node newNode = Game.Player.Grid.GetNodeAtPoint(m_Transform.position);
            if (!Equals(newNode, oldNode))
            {
                oldNode.EnemyDatas.Remove(m_Data);
                newNode.EnemyDatas.Add(m_Data);
            }
        }
        public void Die()
        {
            Node node = Game.Player.Grid.GetNodeAtPoint(m_Transform.position);
            node.EnemyDatas.Remove(m_Data);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }

    }
}
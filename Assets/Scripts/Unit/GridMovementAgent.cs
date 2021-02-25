using System.Collections;
using System.Collections.Generic;
using Field;
using UnityEngine;

public class GridMovementAgent : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;

    private Node m_TargetNode;

    private const float TOLERANCE = 0.1f;

    void Update()
    {
        if (m_TargetNode == null)
        {
            return;
        }

        Vector3 target = m_TargetNode.Position;
        float distance = (target - transform.position).magnitude;
        if (distance < TOLERANCE)
        {
            m_TargetNode = m_TargetNode.NextNode;
            return;
        }
        
        Vector3 dir = (target - transform.position).normalized;
        Vector3 delta = dir * (m_Speed * Time.deltaTime);
        transform.Translate(delta);
    }

    public void SetStartNode(Node node)
    {
        m_TargetNode = node;
    }

}

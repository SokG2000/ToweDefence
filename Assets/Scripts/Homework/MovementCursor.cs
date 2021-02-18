using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCursor : MonoBehaviour
{
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private float m_CursorSize;

        [SerializeField]
        private float m_NodeSize;

        [SerializeField]
        private MovementAgent m_MovementAgent;

        [SerializeField]
        private GameObject m_Cursor;
        
        private Camera m_Camera;

        private Vector3 m_Offset;

        /// <summary>
        /// Makes cursor size independent of field size
        /// </summary>
        private void SetCursorScale()
        {
            int node_num = Math.Max(m_GridWidth, m_GridHeight);
            float scale = 0.05f * m_NodeSize * node_num * m_CursorSize;
            // 0.05 is constant that makes cursor size good in my opinion when m_CursorSize = 1.
            m_Cursor.transform.localScale = new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// Makes all field visible
        /// </summary>
        private void PlaceCamera()
        {
            int node_num = Math.Max(m_GridWidth, m_GridHeight);
            float y = node_num * m_NodeSize * 2f;
            m_Camera.transform.position = new Vector3(0, y, 0);
            m_Camera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        private void Awake()
        {
            m_Camera = Camera.main;
            
            // Default plane size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            m_Offset = transform.position - new Vector3(width * 0.5f, 0, height * 0.5f);
            
            PlaceCamera();
            SetCursorScale();
        }

        private Vector3 GetCellCenter(Vector2Int coordinateOnGrid)
        {
            float xShift = (coordinateOnGrid.x + 0.5f) * m_NodeSize;
            float zShift = (coordinateOnGrid.y + 0.5f) * m_NodeSize;
            Vector3 shift = new Vector3(xShift, 0f, zShift);
            return m_Offset + shift;
        }

        private Vector2Int GetCell(Vector3 coordinateOnPlane)
        {
            Vector3 difference = coordinateOnPlane - m_Offset;
            int x = (int) (difference.x / m_NodeSize);
            int y = (int) (difference.z / m_NodeSize);
            return new Vector2Int(x, y);

        }

        private void Update()
        {
            if (m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            bool is_hitted = (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.transform == transform);

            if (is_hitted)
            {
                m_Cursor.SetActive(true);
                m_Cursor.transform.position = hit.point;
            }
            else
            {
                m_Cursor.SetActive(false);
            }

            if (is_hitted)
            {
                //if (hit.transform != transform)
                //{
                //    return;
                //}

                Vector3 hitPosition = hit.point;
                //Vector3 difference = hitPosition - m_Offset;

                Vector2Int coordinateOnGrid = GetCell(hitPosition);

                //int x = (int) (difference.x / m_NodeSize);
                //int y = (int) (difference.z / m_NodeSize);
                
                //Debug.Log("hit");
                //Debug.Log(x.ToString() + " " + y.ToString());

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Pressed primary button.");
                    Vector3 newTarget = GetCellCenter(coordinateOnGrid);
                    Debug.Log("New target is " + newTarget.ToString());
                    m_MovementAgent.SetTarget(newTarget);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 0.3f);
            float min_x = m_Offset.x;
            float max_x = min_x + m_GridWidth * m_NodeSize;
            float min_z = m_Offset.z;
            float max_z = min_z + m_GridHeight * m_NodeSize;
            for (int i = 0; i <= m_GridWidth; ++i)
            {
                Gizmos.DrawLine(new Vector3(min_x + i * m_NodeSize, 0, min_z),
                    new Vector3(min_x + i * m_NodeSize, 0, max_z));
            }

            for (int j = 0; j < m_GridHeight; ++j)
            {
                Gizmos.DrawLine(new Vector3(min_x, 0, min_z + j * m_NodeSize),
                    new Vector3(max_x, 0, min_z + j * m_NodeSize));
            }
        }

        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            m_Offset = transform.position - new Vector3(width * 0.5f, 0, height * 0.5f);
        }
}

using System;
using UnityEngine;

namespace Field
{
    public class GridHolder : MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;
        [SerializeField] private Vector2Int m_Target;
        [SerializeField] private Vector2Int m_StartCoordinate;

        [SerializeField]
        private float m_NodeSize;

        private Grid m_Grid;
        
        public Vector2Int StartCoordinate => m_StartCoordinate;

        public Grid Grid => m_Grid;


        private Camera m_Camera;

        private Vector3 m_Offset;

        private void PlaceCamera()
        {
            int node_num = Math.Max(m_GridWidth, m_GridHeight);
            float y = node_num * m_NodeSize;
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
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_Target, m_StartCoordinate);
            PlaceCamera();
        }

        private void Update()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            
            bool is_hitted = (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.transform == transform);
            if (!is_hitted) return;
            
            Vector3 hitPosition = hit.point;
            Vector2Int coordinateOnGrid = GetCell(hitPosition);

            
            Debug.Log("hit");

            if (Input.GetMouseButtonDown(0))
            {
                Node node = m_Grid.GetNode(coordinateOnGrid);
                if (node.isOccupied)
                {
                    node.isOccupied = false;
                    m_Grid.UpdatePathfinding();
                    m_Grid.UpdateOccupationAvailability();
                }
                else
                {
                    m_Grid.TryOccupyNode(coordinateOnGrid);
                }
            }
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 0.3f);
            if (m_Grid == null) return;
            
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }

                if (node.isOccupied)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(node.Position, 0.5f);
                    Gizmos.color = Color.red;
                    continue;
                }

                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;

                Vector3 dir = end - start;
                start -= dir * 0.25f;
                end -= dir * 0.75f;
                
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.1f);
            }
        }
    }
}
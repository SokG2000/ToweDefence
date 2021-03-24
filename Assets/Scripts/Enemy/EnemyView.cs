using Assets;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        private EnemyData m_Data;
        private IMovementAgent m_MovementAgent;

        public EnemyData Data => m_Data;

        public IMovementAgent MMovementAgent => m_MovementAgent;

        public void AttachData(EnemyData data)
        {
            m_Data = data;
        }

        public void CreateMovementAgent(Grid grid)
        {
            EnemyAsset enemyAsset = m_Data.EnemyAsset;
            if (enemyAsset.IsFlyingEnemy)
            {
                m_MovementAgent = new FlyingMovementAgent(enemyAsset.Speed, transform, grid, m_Data);
            }
            else
            {
                m_MovementAgent = new GridMovementAgent(enemyAsset.Speed, transform, grid, m_Data);
            }
        }
    }
}
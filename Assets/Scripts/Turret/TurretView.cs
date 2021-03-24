using UnityEngine;

namespace Turret
{
    public class TurretView: MonoBehaviour
    {

        private TurretData m_Data;

        private Transform m_ProjectileOrigin;

        public Transform ProjectileOrigin => m_ProjectileOrigin;

        public TurretData Data => m_Data;
        public void AttachData(TurretData turretData)
        {
            m_Data = turretData;
            transform.position = m_Data.MNode.Position;
        }
    }
}
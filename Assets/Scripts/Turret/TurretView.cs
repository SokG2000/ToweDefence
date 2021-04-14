using JetBrains.Annotations;
using UnityEngine;

namespace Turret
{
    public class TurretView: MonoBehaviour
    {

        private TurretData m_Data;

        [SerializeField]
        private Transform m_ProjectileOrigin;

        [CanBeNull]
        [SerializeField] private Animator m_Animator;

        [SerializeField] private Transform m_Tower;
        private static readonly int ShotAnimationIndex = Animator.StringToHash("Shot");

        public Transform ProjectileOrigin => m_ProjectileOrigin;

        public TurretData Data => m_Data;
        public void AttachData(TurretData turretData)
        {
            m_Data = turretData;
            transform.position = m_Data.MNode.Position;
        }

        public void TowerLookAt(Vector3 point)
        {
            point.y = m_Tower.position.y;
            m_Tower.LookAt(point);
        }

        public void AnimateShot()
        {
            //Debug.Log(m_Animator == null);
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.SetTrigger(ShotAnimationIndex);
        }
    }
}
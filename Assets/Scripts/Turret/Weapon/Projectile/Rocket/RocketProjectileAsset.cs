using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    [CreateAssetMenu(menuName = "Assets/RocketProjectileAsset", fileName = "Rocket")]
    public class RocketProjectileAsset: ProjectileAssetBase
    {
        [SerializeField]
        private RocketProjectile m_RocketPrefab;

        [SerializeField] private float m_Damage;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_Radius;

        public float Damage => m_Damage;

        public float Speed => m_Speed;

        public float Radius => m_Radius;

        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            RocketProjectile rocketProjectile = Instantiate(m_RocketPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            rocketProjectile.SetTarget(enemyData);
            rocketProjectile.SetAsset(this);
            return rocketProjectile;
        }
    }
}
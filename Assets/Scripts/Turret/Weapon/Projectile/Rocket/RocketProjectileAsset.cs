using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    [CreateAssetMenu(menuName = "Assets/RocketProjectileAsset", fileName = "Rocket")]
    public class RocketProjectileAsset: ProjectileAssetBase
    {
        [SerializeField]
        private RocketProjectile m_RocketPrefab;
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            RocketProjectile rocketProjectile = Instantiate(m_RocketPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            rocketProjectile.SetTarget(enemyData);
            return rocketProjectile;
        }
    }
}
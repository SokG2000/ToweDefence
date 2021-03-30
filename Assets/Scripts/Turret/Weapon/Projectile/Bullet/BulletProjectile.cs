using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Bullet
{
    public class BulletProjectile: MonoBehaviour, IProjectile
    {
        private float m_Speed = 10f;
        private bool m_DigHit;
        private int m_Damage = 5;
        private EnemyData m_HitEnemy = null;
        public void TickApproaching()
        {
            transform.Translate(transform.forward * (m_Speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            m_DigHit = true;
            if (other.CompareTag("Enemy"))
            {
                EnemyView enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    m_HitEnemy = enemyView.Data;
                }
            }
        }

        public bool DidHit()
        {
            return m_DigHit;
        }

        public void DestroyProjectile()
        {
            if (m_HitEnemy != null)
            {
                Debug.Log("Hit");
                m_HitEnemy.GetDamage(m_Damage);
            }
            Destroy(gameObject);
        }
    }
}
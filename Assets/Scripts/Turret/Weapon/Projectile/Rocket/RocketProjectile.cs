using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile: MonoBehaviour, IProjectile
    {
        private float m_Speed = 5f;
        private bool m_DigHit;
        private int m_Damage = 5;
        private float m_Radius = 3;
        private EnemyData m_Target;

        public void SetTarget(EnemyData enemyData)
        {
            m_Target = enemyData;
        }
        public void TickApproaching()
        {
            transform.LookAt(m_Target.MView.transform.position);
            transform.Translate(transform.forward * (m_Speed * Time.deltaTime), Space.World);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            m_DigHit = true;
        }

        public bool DidHit()
        {
            return m_DigHit;
        }

        public void DestroyProjectile()
        {
            Vector3 position = transform.position;
            List<Node> accesibleNodes = Game.Player.Grid.GetNodesInCircle(position, m_Radius, false);
            foreach (EnemyData enemyData in EnemySearch.GetAllEnemies(accesibleNodes, position, m_Radius))
            {
                enemyData.GetDamage(m_Damage);
            }
            Destroy(gameObject);
        }
    }
}
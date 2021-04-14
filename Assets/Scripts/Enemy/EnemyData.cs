using System.Collections;
using Assets;
using Runtime;
using UnityEngine;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        private float m_Health;

        public EnemyView MView => m_View;

        public readonly EnemyAsset EnemyAsset;

        public EnemyData(EnemyAsset asset)
        {
            EnemyAsset = asset;
            m_Health = asset.StartHealth;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }

        public void GetDamage(float damage)
        {
            m_Health -= damage;
            //Debug.Log("Get " + damage.ToString() + " damage.");
            if (m_Health < 0)
            {
                Die();
            }
        }

        private void Die()
        {
            //Debug.Log("Die");
            m_View.AnimateDie();
            Game.Player.EnemyDied(this);
            m_View.MMovementAgent.Die();
        }
    }
}
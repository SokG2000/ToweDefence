using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Field
{
    public class TurretFieldWeapon: ITurretWeapon
    {
        private TurretFieldWeaponAsset m_Asset;
        private TurretView m_View;
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        private float m_DamagePerSecond;
        private float m_MaxDistance;
        
        private List<Node> AccessibleNodes;

        private GameObject m_Field;
        
        public TurretFieldWeapon(TurretFieldWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_DamagePerSecond = asset.DamagePerSecond;
            m_MaxDistance = asset.MaxDistance;
            m_Field = Object.Instantiate(asset.FieldPrefab, m_View.transform.position, Quaternion.identity);
            Renderer renderer = m_Field.GetComponent<Renderer>();
            Color color = new Color(255, 255, 255, 0.5f);
            renderer.material.color = color;
            AccessibleNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
        }

        public void TickShoot()
        {
            List<EnemyData> enemyDatas =
                EnemySearch.GetAllEnemies(AccessibleNodes, m_View.ProjectileOrigin.position, m_MaxDistance);
            foreach (EnemyData enemyData in enemyDatas)
            {
                enemyData.GetDamage(m_DamagePerSecond * Time.deltaTime);
            }
        }
    }
}
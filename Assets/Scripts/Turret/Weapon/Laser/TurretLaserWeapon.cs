using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Laser
{
    public class TurretLaserWeapon: ITurretWeapon
    {
        private TurretLaserWeaponAsset m_Asset;
        private TurretView m_View;
        private LineRenderer m_LineRenderer;        
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        private float m_DamagePerSecond;
        private float m_MaxDistance;
        
        private List<Node> AccessibleNodes;

        public TurretLaserWeapon(TurretLaserWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_DamagePerSecond = asset.DamagePerSecond;
            m_MaxDistance = asset.MaxDistance;
            AccessibleNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
            //m_LineRenderer = Object.Instantiate(asset.LineRendererPrefab, m_View);
        }

        public void TickShoot()
        {
            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(AccessibleNodes, m_View.transform.position, m_MaxDistance);

            if (m_ClosestEnemyData == null)
            {
                return;
            }
            
            TickTower();
            
            m_ClosestEnemyData.GetDamage(m_DamagePerSecond * Time.deltaTime);
        }
        
        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                m_View.TowerLookAt(m_ClosestEnemyData.MView.transform.position);
            }
        }

    }
}
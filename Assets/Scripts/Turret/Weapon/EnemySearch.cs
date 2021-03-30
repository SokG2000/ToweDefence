using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon
{
    public static class EnemySearch
    {
        [CanBeNull]
        public static EnemyData GetClosestEnemy(List<Node> accessibleNodes, Vector3 center, float maxDistance)
        {
            float maxSqrtDistance = maxDistance * maxDistance;

            float minSqrDistance = float.MaxValue;
            EnemyData closestEnemy = null;

            foreach (Node node in accessibleNodes)
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    float sqrDistance = (enemyData.MView.transform.position - center).sqrMagnitude;
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        closestEnemy = enemyData;
                    }
                }
                if (minSqrDistance <= maxDistance)
                {
                    return closestEnemy;
                }
            }
            return null;
        }

        public static List<EnemyData> GetAllEnemies(List<Node> accessibleNodes, Vector3 center, float maxDistance)
        {
            float maxSqrtDistance = maxDistance * maxDistance;
            List<EnemyData> enemyDatas = new List<EnemyData>();
            foreach (Node node in accessibleNodes)
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    float sqrDistance = (enemyData.MView.transform.position - center).sqrMagnitude;
                    if (sqrDistance <= maxSqrtDistance)
                    {
                        enemyDatas.Add(enemyData);
                    }
                }
            }
            return enemyDatas;
        }
    }
}
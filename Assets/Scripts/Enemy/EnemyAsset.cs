using Enemy;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Assets/EnemyAsset", fileName = "EnemyAsset")]
    public class EnemyAsset : ScriptableObject
    {
        public float StartHealth;
        public bool IsFlyingEnemy;
        public float Speed;
        
        public EnemyView ViewPrefab;
    }
}
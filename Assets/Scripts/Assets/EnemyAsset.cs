using Enemy;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/EnemyAsset", fileName = "EnemyAsset")]
    public class EnemyAsset : ScriptableObject
    {
        public int StartHealth;
        public bool IsFlyingEnemy;
        public float Speed;
        
        public EnemyView ViewPrefab;
    }
}
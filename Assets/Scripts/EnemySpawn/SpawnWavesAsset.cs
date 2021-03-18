using EnemySpawn;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "Spawn Wave", menuName = "Assets/Spawn Wave")]
    public class SpawnWavesAsset : ScriptableObject
    {
        public SpawnWave[] SpawnWaves;
    }
}
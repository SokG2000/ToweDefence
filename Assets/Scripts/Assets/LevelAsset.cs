using TurretSpawn;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/LevelAsset", fileName = "LevelAsset")]
    public class LevelAsset : ScriptableObject
    {
        public SceneAsset SceneAsset;
        public SpawnWavesAsset spawnWavesAsset;
        public TurretMarketAsset TurretMarketAsset;
    }
}
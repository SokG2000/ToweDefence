using Turret;
using UnityEngine;

namespace TurretSpawn
{
    [CreateAssetMenu(menuName = "Assets/TurretMarketAsset", fileName = "Turret Market Asset")]

    public class TurretMarketAsset: ScriptableObject
    {
        public TurretAsset[] TurretAssets;
    }
}
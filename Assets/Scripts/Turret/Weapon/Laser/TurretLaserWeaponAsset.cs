using UnityEngine;

namespace Turret.Weapon.Laser
{
    [CreateAssetMenu(menuName = "Assets/Turret Laser Weapon Asset", fileName = "TurretLaserWeapon")]
    public class TurretLaserWeaponAsset: TurretWeaponAssetBase
    {
        public float DamagePerSecond;
        public float MaxDistance;
        
        public LineRenderer LineRendererPrefab;
        
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretLaserWeapon(this, view);
        }
    }
}
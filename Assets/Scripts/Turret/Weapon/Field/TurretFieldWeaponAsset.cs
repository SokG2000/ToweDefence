using UnityEngine;

namespace Turret.Weapon.Field
{
    [CreateAssetMenu(menuName = "Assets/Turret Field Weapon Asset", fileName = "FieldWeapon")]
    public class TurretFieldWeaponAsset: TurretWeaponAssetBase
    {
        public float DamagePerSecond;
        public float MaxDistance;

        public GameObject FieldPrefab;
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretFieldWeapon(this, view);
        }
    }
}
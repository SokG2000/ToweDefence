using System;
using UnityEngine;

namespace Turret.Weapon.Projectile
{
    [CreateAssetMenu(menuName = "Assets/Turret Projectile Weapon Asses", fileName = "TurretProjectileWeapon")]
    public class TurretProjectileWeaponAsset: TurretWeaponAssetBase
    {
        public float RateOfFire;
        public float MaxDistance;

        public ProjectileAssetBase ProjectileAsset;
        
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretProjectileWeapon(this, view);
        }
    }
}
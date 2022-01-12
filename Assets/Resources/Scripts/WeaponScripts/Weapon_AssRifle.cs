using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AssRifle : Weapon_Base
{
    [Header("Assault Weapon Settings")]
    [SerializeField] protected float initInaccuracy;

    public override void init(WeaponStatHolderBase stats)
    {
        weaponSoundName = "AssaultRifleSound";
        base.init(stats);
    }

    public override void FireWeapon(bool isFiredByPlayer, float damageScale = 1.0f, float fireRateScale = 1.0f)
    {
        base.FireWeapon(isFiredByPlayer);
        if(currentShotTimer <= 0.0f)
        {
            GameObject hitObject = null;
            FireHitscanShot(isFiredByPlayer, ref hitObject);
            currentShotTimer = 1 / fireRate;
        }
    }
}

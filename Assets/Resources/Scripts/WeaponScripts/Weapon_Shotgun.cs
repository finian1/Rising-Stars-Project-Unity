using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shotgun : Weapon_Base
{
    [Header("Shotgun Weapon Settings")]
    [SerializeField] private int shotCount;

    public override void init(WeaponStatHolderBase stats)
    {
        base.init(stats);
        shotCount = stats.shotCount;
    }

    public override void FireWeapon(bool isFiredByPlayer)
    {
        if (currentShotTimer >= 1 / fireRate)
        {
            for (int i = 0; i < shotCount; i++)
            {
                GameObject hitObject = null;
                FireHitscanShot(isFiredByPlayer, ref hitObject);
            }
            currentShotTimer = 0;
        }
    }
}

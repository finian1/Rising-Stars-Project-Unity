using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AssRifle : Weapon_Base
{
    public override void init(WeaponStatHolderBase stats)
    {
        base.init(stats);
    }

    public override void FireWeapon(bool isFiredByPlayer)
    {
        if(currentShotTimer >= 1/fireRate)
        {
            GameObject hitObject = null;
            FireHitscanShot(isFiredByPlayer, ref hitObject);
            currentShotTimer = 0;
        }
    }
}

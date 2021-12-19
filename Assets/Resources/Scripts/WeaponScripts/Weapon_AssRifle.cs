using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AssRifle : Weapon_Base
{
    public Weapon_AssRifle(float wepDamage, float wepRange, float wepInnacuracy, float wepFireRate, float wepShotLifetime, float wepShotWidth, 
        string wepNickname = "Nameless")
        : base(wepDamage, wepRange, wepInnacuracy, wepFireRate, wepShotLifetime, wepShotWidth, wepNickname)
    {
    }

    public override void FireWeapon(bool isFiredByPlayer)
    {
        if(currentShotTimer >= 1/fireRate)
        {
            GameObject hitObject = new GameObject();
            FireHitscanShot(isFiredByPlayer, ref hitObject);
            currentShotTimer = 0;
        }
    }
}

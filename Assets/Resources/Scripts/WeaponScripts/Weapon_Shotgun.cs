using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shotgun : Weapon_Base
{
    [Header("Shotgun Weapon Settings")]
    [SerializeField] private int shotCount;

    public Weapon_Shotgun(float wepDamage, float wepRange, float wepInnacuracy, float wepFireRate, float wepShotLifetime, float wepShotWidth, 
        int wepShotCount, string wepNickname = "Nameless")
        : base(wepDamage, wepRange, wepInnacuracy, wepFireRate, wepShotLifetime, wepShotWidth, wepNickname)
    {
        shotCount = wepShotCount;
    }

    public override void FireWeapon(bool isFiredByPlayer)
    {
        if (currentShotTimer >= 1 / fireRate)
        {
            for (int i = 0; i < shotCount; i++)
            {
                GameObject hitObject = new GameObject();
                FireHitscanShot(isFiredByPlayer, ref hitObject);
            }
            currentShotTimer = 0;
        }
    }
}

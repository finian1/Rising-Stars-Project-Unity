using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopContainer
{
    //WeaponStatHolderBase(System.Type wepType,
    //float wepDamage, float wepRange, float wepInnacuracy, float wepFireRate, float wepShotLifetime,
    //float wepShotWidth, int weaponCost = 0, string wepNickname = "Nameless",
    //    /*minigun vars*/float mini_wepStartFireRate = 0.0f, float mini_wepTimeToRevUp = 0.0f, float mini_wepCoolDownSpeed = 0.0f,
    //    /*shotgun vars*/int shot_wepShotCount = 0,
    //    /*assault vars*/float ass_wepInitInaccuracy = 0.0f
    //    )
    public static List<WeaponStatHolderBase> weaponsForSale = new List<WeaponStatHolderBase> {
        new WeaponStatHolderBase(typeof(Weapon_AssRifle), 
            wepDamage : 5, 
            wepRange : 30, 
            wepInnacuracy : 8, 
            wepFireRate : 10, 
            wepShotLifetime : 0.1f, 
            wepShotWidth : 0.1f, 
            weaponCost : 500,
            crystalMultiplier : 1.0f,
            wepNickname : "Speedy Rifle"),
        new WeaponStatHolderBase(typeof(Weapon_Shotgun), 
            wepDamage : 5, 
            wepRange : 5, 
            wepInnacuracy : 70, 
            wepFireRate : 0.5f, 
            wepShotLifetime : 0.1f, 
            wepShotWidth : 0.1f, 
            weaponCost : 500,
            crystalMultiplier : 2.0f,
            wepNickname : "Short n' Shotty", 
            shot_wepShotCount : 15),
        new WeaponStatHolderBase(typeof(Weapon_MiniGun), 
            wepDamage : 5, 
            wepRange : 25, 
            wepInnacuracy : 20, 
            wepFireRate : 20, 
            wepShotLifetime : 0.1f, 
            wepShotWidth : 0.1f, 
            weaponCost: 500,
            crystalMultiplier : 0.5f,
            wepNickname : "Mini Boy", 
            mini_wepStartFireRate : 1, 
            mini_wepTimeToRevUp : 10, 
            mini_wepCoolDownSpeed : 3)
        };
}

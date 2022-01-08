using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopContainer
{
    public static List<WeaponStatHolderBase> weaponsForSale = new List<WeaponStatHolderBase> {
        new WeaponStatHolderBase(typeof(Weapon_AssRifle), 5, 20, 5, 5, 0.1f, 0.1f, 100, "TestAssaultRifle"),
        new WeaponStatHolderBase(typeof(Weapon_Shotgun), 5, 20, 5, 5, 0.1f, 0.1f, 200, "TestShotgun", shot_wepShotCount : 5)
        };
}

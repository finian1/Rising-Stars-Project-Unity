using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static float health;
    public static int points;
    public static int currency;
    public static WeaponStatHolderBase[] weaponsOwned =
    {
        new WeaponStatHolderBase(typeof(Weapon_AssRifle), 10, 50, 10, 5, 0.1f, 0.1f, "TestyAssault"),
        new WeaponStatHolderBase(typeof(Weapon_MiniGun), 6, 50, 15, 25, 0.1f, 0.1f, "TestyMini", wepStartFireRate: 1, wepTimeToRevUp: 10.0f, wepCoolDownSpeed: 1.0f),
        new WeaponStatHolderBase(typeof(Weapon_Shotgun), 3, 20, 45, 1, 0.1f, 0.1f, "TestyShoty", wepShotCount: 10)
    };
}

public class WeaponStatHolderBase
{
    public WeaponStatHolderBase(System.Type wepType, float wepDamage, float wepRange, float wepInnacuracy, float wepFireRate, float wepShotLifetime, float wepShotWidth, string wepNickname = "Nameless",
        /*minigun vars*/float wepStartFireRate = 0.0f, float wepTimeToRevUp = 0.0f, float wepCoolDownSpeed = 0.0f,
        /*shotgun vars*/int wepShotCount = 0
        )
    {
        weaponType = wepType;
        weaponNickname = wepNickname;
        shotDamage = wepDamage;
        range = wepRange;
        innacuracy = wepInnacuracy;
        fireRate = wepFireRate;
        shotLifetime = wepShotLifetime;
        shotWidth = wepShotWidth;

        startFireRate = wepStartFireRate;
        timeToRevUp = wepTimeToRevUp;
        coolDownSpeed = wepCoolDownSpeed;

        shotCount = wepShotCount;
    }

    public System.Type weaponType;
    public string weaponNickname;
    public float shotDamage;
    public float range;
    public float innacuracy;
    public float fireRate;
    public float shotLifetime;
    public float shotWidth;
    //Minigun vars
    public float startFireRate;
    public float timeToRevUp;
    public float coolDownSpeed;
    //Shotgun vars
    public int shotCount;
}





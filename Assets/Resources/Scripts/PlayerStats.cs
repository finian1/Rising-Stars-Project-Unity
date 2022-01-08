using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static float initialHealth = 100.0f;
    public static float health = initialHealth;
    public static int points;
    public static int currency = 1000;
    public static float difficulty = 1.0f;
    public static List<WeaponStatHolderBase> weaponsOwned = new List<WeaponStatHolderBase>
    {
        new WeaponStatHolderBase(typeof(Weapon_AssRifle), 10, 50, 10, 5, 0.1f, 0.1f, 0, "TestyAssault"),
        new WeaponStatHolderBase(typeof(Weapon_MiniGun), 6, 50, 15, 25, 0.1f, 0.1f, 0, "TestyMini", mini_wepStartFireRate: 1, mini_wepTimeToRevUp: 10.0f, mini_wepCoolDownSpeed: 1.0f),
        new WeaponStatHolderBase(typeof(Weapon_Shotgun), 3, 20, 45, 1, 0.1f, 0.1f, 0, "TestyShoty", shot_wepShotCount: 10)
    };
    public static WeaponStatHolderBase primaryWeapon;
    public static WeaponStatHolderBase secondaryWeapon;

}

public class WeaponStatHolderBase
{
    public WeaponStatHolderBase(System.Type wepType, float wepDamage, float wepRange, float wepInnacuracy, float wepFireRate, float wepShotLifetime, float wepShotWidth, int weaponCost = 0, string wepNickname = "Nameless",
        /*minigun vars*/float mini_wepStartFireRate = 0.0f, float mini_wepTimeToRevUp = 0.0f, float mini_wepCoolDownSpeed = 0.0f,
        /*shotgun vars*/int shot_wepShotCount = 0,
        /*assault vars*/float ass_wepInitInaccuracy = 0.0f
        )
    {
        weaponType = wepType;
        weaponValue = weaponCost;
        weaponNickname = wepNickname;
        shotDamage = wepDamage;
        range = wepRange;
        innacuracy = wepInnacuracy;
        fireRate = wepFireRate;
        shotLifetime = wepShotLifetime;
        shotWidth = wepShotWidth;

        startFireRate = mini_wepStartFireRate;
        timeToRevUp = mini_wepTimeToRevUp;
        coolDownSpeed = mini_wepCoolDownSpeed;

        shotCount = shot_wepShotCount;

        initInaccuracy = ass_wepInitInaccuracy;
    }
    public WeaponStatHolderBase()
    {
        weaponType = null;
        weaponNickname = "";
        shotDamage = 0.0f;
        range = 0.0f;
        innacuracy =0.0f;
        fireRate = 0.0f;
        shotLifetime = 0.0f;
        shotWidth = 0.0f;

        startFireRate = 0.0f;
        timeToRevUp = 0.0f;
        coolDownSpeed = 0.0f;

        shotCount = 0;

        initInaccuracy = 0.0f;
    }

    public System.Type weaponType;
    public int weaponValue;
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
    //Assault vars
    public float initInaccuracy;
}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static float initialHealth = 100.0f;
    public static float health = initialHealth;
    public static float starterHealth = initialHealth;
    public static int points;
    public static int currency = 10000;
    public static float difficulty = 1.0f;
    public static float initDifficulty = 1.0f;
    public static List<WeaponStatHolderBase> weaponsOwned = new List<WeaponStatHolderBase>
    {
        new WeaponStatHolderBase(typeof(Weapon_AssRifle), 
            wepDamage : 2, 
            wepRange : 50, 
            wepInnacuracy : 2, 
            wepFireRate : 5, 
            wepShotLifetime : 0.1f, 
            wepShotWidth : 0.1f, 
            wepColour : Color.green, 
            crystalMultiplier : 1.0f, 
            weaponCost : 0, 
            wepNickname : "Starter Assault Rifle")
    };
    public static WeaponStatHolderBase primaryWeapon;
    public static WeaponStatHolderBase secondaryWeapon;
    public static bool isInTrap;

    //Stat Levels
    public static int maxBuffLevel = 10;
    public static int startBuffPrice = 1000;
    public static int priceIncreasePerLevel = 100;

    public static int healthLevel = 0;
    public static float buffPerLevel_Health = 10;

    public static int jumpLevel = 0;
    public static float buffPerLevel_Jump = 1;

    public static int speedLevel = 0;
    public static float buffPerLevel_Speed = 1;

    public static int crystalDropLevel = 0;
    public static float buffPerLevel_CrystalDrop = 1;

    public static int crystalWorthLevel = 0;
    public static float buffPerLevel_CrystalWorth = 2;

    public static int GetBuffPrice(int level)
    {
        return startBuffPrice + (level * priceIncreasePerLevel);
    }

}

public class WeaponStatHolderBase
{
    public WeaponStatHolderBase(
        System.Type wepType,
        float wepDamage,
        float wepRange,
        float wepInnacuracy,
        float wepFireRate,
        float wepShotLifetime,
        float wepShotWidth,
        Color wepColour,
        float crystalMultiplier = 1.0f,
        int weaponCost = 0,
        string wepNickname = "Nameless",
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
        weaponColour = wepColour;
        crysMultiplier = crystalMultiplier;

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
        crysMultiplier = 1.0f;
        weaponColour = Color.white;

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
    public float crysMultiplier;
    public Color weaponColour;
    //Minigun vars
    public float startFireRate;
    public float timeToRevUp;
    public float coolDownSpeed;
    //Shotgun vars
    public int shotCount;
    //Assault vars
    public float initInaccuracy;
}





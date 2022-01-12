using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_MiniGun : Weapon_Base
{
    [Header("Minigun Weapon Settings")]
    [SerializeField] private float startFireRate;
    [SerializeField] private float timeToRevUp;
    [SerializeField] private float coolDownSpeed;

    private float currentRevPercent;
    private float currentFireRate;

    public override void init(WeaponStatHolderBase stats)
    {
        weaponSoundName = "MiniGunSound";
        base.init(stats);
        startFireRate = stats.startFireRate;
        timeToRevUp = stats.timeToRevUp;
        coolDownSpeed = stats.coolDownSpeed;
    }

    protected override void Update()
    {
        base.Update();
        if (!isFiring)
        {
            if (currentRevPercent > 0)
            {
                currentRevPercent -= coolDownSpeed * Time.deltaTime;
                if (currentRevPercent < 0)
                {
                    currentRevPercent = 0;
                }
            }
        }
    }

    public override void StopFiring()
    {
        isFiring = false;
    }

    public override void FireWeapon(bool isFiredByPlayer, float damageScale = 1.0f, float fireRateScale = 1.0f)
    {
        base.FireWeapon(isFiredByPlayer);
        isFiring = true;
        if(currentRevPercent < 1)
        {
            float stepVal = Time.deltaTime/timeToRevUp;
            currentRevPercent += stepVal;
            if(currentRevPercent > 1)
            {
                currentRevPercent = 1;
            }
        }
        
        currentFireRate = startFireRate + ((fireRate - startFireRate) * currentRevPercent);
        Debug.Log(currentRevPercent);
        if (currentShotTimer <= 0.0f)
        {
            GameObject hitObject = null;
            FireHitscanShot(isFiredByPlayer, ref hitObject);
            currentShotTimer = 1 / fireRate;
        }

    }
}

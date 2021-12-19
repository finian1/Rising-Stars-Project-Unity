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

    public Weapon_MiniGun(float wepDamage, float wepRange, float wepInnacuracy, float wepFireRate, float wepShotLifetime, float wepShotWidth,
        float wepStartFireRate, float wepTimeToRevUp, float wepCoolDownSpeed, string wepNickname = "Nameless")
        : base(wepDamage, wepRange, wepInnacuracy, wepFireRate, wepShotLifetime, wepShotWidth, wepNickname)
    {
        startFireRate = wepStartFireRate;
        timeToRevUp = wepTimeToRevUp;
        coolDownSpeed = wepCoolDownSpeed;
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

    public override void FireWeapon(bool isFiredByPlayer)
    {
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
        if (currentShotTimer >= 1 / currentFireRate)
        {
            GameObject hitObject = new GameObject();
            FireHitscanShot(isFiredByPlayer, ref hitObject);
            currentShotTimer = 0;
        }

    }
}

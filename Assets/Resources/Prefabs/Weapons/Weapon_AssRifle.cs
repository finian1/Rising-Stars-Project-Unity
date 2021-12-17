using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AssRifle : Weapon_Base
{
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

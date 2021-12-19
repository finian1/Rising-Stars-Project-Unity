using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static float health;
    public static int points;
    public static int currency;
    public static Weapon_Base[] weaponsOwned =
    {
        new Weapon_AssRifle(10, 50, 10, 5, 0.1f, 0.1f, "TestyAssault"),
        new Weapon_MiniGun(6, 50, 15, 25, 0.1f, 0.1f, 2, 5, 1, "TestyMini"),
        

    };
}



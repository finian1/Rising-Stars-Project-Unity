using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Health : Status_Base
{
    public override void StartBuff(GameObject player)
    {
        base.StartBuff(player);
        PlayerStats.health += strengthOfEffect;
        
    }

    public override void Tick()
    {
        base.Tick();
    }

    protected override void EndBuff()
    {
        base.EndBuff();
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Jump : Status_Base
{
    public override void StartBuff(GameObject player)
    {
        base.StartBuff(player);

        player.GetComponent<PlayerController>().jumpForce += strengthOfEffect;
    }

    public override void Tick()
    {
        base.Tick();
    }

    protected override void EndBuff()
    {
        base.EndBuff();
        playerController.ResetJumpHeight();
    }

}

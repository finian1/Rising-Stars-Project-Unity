using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Base : MonoBehaviour
{
    public string buffName;
    protected float durationOfEffect;
    protected float strengthOfEffect;

    private float buffTimer;
    private bool isActive = false;
    protected GameObject playerObject;
    protected PlayerController playerController;

    private void Update()
    {
        if (isActive)
        {
            Tick();
        }
    }

    public virtual void StartBuff(GameObject player)
    {
        playerObject = player;
        playerController = playerObject.GetComponent<PlayerController>();
        isActive = true;
    }

    public virtual void Tick()
    {
        buffTimer += Time.deltaTime;
    }

    protected virtual void EndBuff()
    {
        isActive = false;
    }
}

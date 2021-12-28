using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript_Agressive : EnemyScript_Base
{


    // Update is called once per frame
    protected override void Update()
    {
        if (player != null)
        { 
            FireAtPlayer();
        }

        if (currentNodeTarget != null)
        {
            MoveTowards(currentNodeTarget);
            Debug.DrawLine(transform.position, currentNodeTarget.transform.position, Color.green);
            if (transform.position == currentNodeTarget.transform.position)
            {
                if (player != null)
                {
                    currentNodeTarget = PickAgressiveNode();
                    if (currentNodeTarget != null)
                    {
                        nodeMemory.Add(currentNodeTarget);
                    }
                }
            }
        }
        else
        {
            currentNodeTarget = FindClosestNode();
        }
    }
}

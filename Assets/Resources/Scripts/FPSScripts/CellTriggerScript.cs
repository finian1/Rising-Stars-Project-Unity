using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTriggerScript : MonoBehaviour
{
    public Cell thisCell;
    private bool hasPlayer = false;
    Collider playerCollider;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            thisCell.TriggerEntered(other);
            playerCollider = other;
            hasPlayer = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasPlayer = true;
        }
    }

    public void TriggerIfPlayer()
    {
        if (hasPlayer)
        {
            thisCell.TriggerEntered(playerCollider);
        }
    }

}

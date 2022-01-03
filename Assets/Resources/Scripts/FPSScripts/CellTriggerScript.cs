using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTriggerScript : MonoBehaviour
{
    public Cell thisCell;

    public void OnTriggerEnter(Collider other)
    {
         thisCell.TriggerEntered(other);
    }

}

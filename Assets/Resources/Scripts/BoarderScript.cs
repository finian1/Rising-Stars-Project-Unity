using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarderScript : MonoBehaviour
{
    public Game gameController;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.EndGame(false);
        }
    }
}

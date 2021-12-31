using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatformScript : MonoBehaviour
{
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           Destroy(gameObject);
        }
    }
}

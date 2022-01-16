using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatformScript : MonoBehaviour
{
    private Vector3 decentSpeed = new Vector3(0,-1,0);
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position += decentSpeed * Time.deltaTime;
    }
}

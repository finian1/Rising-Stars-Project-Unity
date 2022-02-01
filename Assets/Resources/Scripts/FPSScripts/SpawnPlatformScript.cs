using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatformScript : MonoBehaviour
{
    private Vector3 decentSpeed = new Vector3(0,-1,0);

    private void Start()
    {
        CleanupScript.objectCache.Add(this.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CleanupScript.objectCache.Remove(this.gameObject);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!PlayerStats.pausedGame && ! PlayerStats.lookingAtMap)
        {
            transform.position += decentSpeed * Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CleanupScript
{
    public static List<GameObject> objectCache = new List<GameObject>();

    public static void Cleanup()
    {
        foreach(GameObject obj in objectCache)
        {
            Object.Destroy(obj);
        }
    }

}

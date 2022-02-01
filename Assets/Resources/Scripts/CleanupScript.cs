using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to store game objects that need to be deleted when the game ends.
public static class CleanupScript
{

    public static List<GameObject> objectCache = new List<GameObject>();
    ///<summary>
    ///Destroys all objects stored in the cache array.
    ///</summary>
    public static void Cleanup()
    {
        foreach(GameObject obj in objectCache)
        {
            Object.Destroy(obj);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionScript : MonoBehaviour
{
    public void Destruct(float destructionTime)
    {
        Destroy(gameObject, destructionTime);
    }
}

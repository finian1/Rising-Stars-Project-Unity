using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyScript : MonoBehaviour
{
    public Vector3 rotationSpeeds;

    private void FixedUpdate()
    {
        if (!PlayerStats.pausedGame)
        {
            gameObject.transform.Rotate(rotationSpeeds * Time.deltaTime);
        }
    }
}

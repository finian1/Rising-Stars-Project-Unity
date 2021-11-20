using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{

    private float revealSpeed = 50.0f;
    public float spawnHeightStart;
    public float spawnHeightEnd;
    public float obstacleSize = 2.5f;
    private float depthSoftening = 0.75f;

    [SerializeField]protected bool isRevealing = false;

    
    void Start()
    {
        //isRevealing = false;
        //gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isRevealing)
        {
            MoveForReveal();
        }
    }

    public void BeginReveal(GameObject playerObject)
    {
        //Set the object to active and alter the height of the object's start position based on how far away the object is from the player
        //to create a wave effect
        gameObject.SetActive(true);
        float distToPlayer = Vector3.Magnitude(playerObject.transform.position - gameObject.transform.position);
        transform.localPosition = new Vector3(transform.localPosition.x, spawnHeightStart - (distToPlayer/depthSoftening), transform.localPosition.z);
        isRevealing = true;
    }

    public void SetFinalHeight(float val)
    {
        spawnHeightEnd = val;
    }

    private void MoveForReveal()
    {
        //Move the object up until it reaches the required height
        if(transform.localPosition.y < spawnHeightEnd)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (revealSpeed * Time.deltaTime), transform.localPosition.z);
            if (transform.localPosition.y >= spawnHeightEnd)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, spawnHeightEnd, transform.localPosition.z);
                isRevealing = false;
            }
        }
    }

}
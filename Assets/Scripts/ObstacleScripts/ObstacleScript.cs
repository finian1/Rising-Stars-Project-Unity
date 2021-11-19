using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{

    public float revealSpeed;
    public float spawnHeightStart;
    public float spawnHeightEnd;

    protected bool isRevealing = false;

    // Start is called before the first frame update
    void Start()
    {
        isRevealing = false;
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        
    }

    public void BeginReveal(GameObject playerObject)
    {
        //Set the object to active and alter the height of the object's start position based on how far away the object is from the player
        //to create a wave effect
        gameObject.SetActive(true);
        float distToPlayer = Vector3.Magnitude(playerObject.transform.position - gameObject.transform.position);
        transform.position = new Vector3(transform.position.x, spawnHeightStart - distToPlayer, transform.position.z);
        isRevealing = true;
    }

    private void MoveForReveal()
    {
        //Move the object up until it reaches the required height
        while(transform.position.y < spawnHeightEnd)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (revealSpeed * Time.deltaTime), transform.position.z);
            if (transform.position.y >= spawnHeightEnd)
            {
                transform.position = new Vector3(transform.position.x, spawnHeightEnd, transform.position.z);
                isRevealing = false;
            }
        }
    }

}

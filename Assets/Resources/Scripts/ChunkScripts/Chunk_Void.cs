using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_Void : ChunkClass
{
    private bool warningFade;
    private bool falling;
    private float fallingDistance = 50.0f;
    private float fallingSpeed = 200.0f;

    private float warningTime = 2;
    private float lerpStep;
    private float currentLerp = 0;
    private Color fullWarningColor = new Color(1.0f, 0.0f, 0.0f);
    private Color startColor;

    public override void SpawnObjects()
    {
        startColor = this.GetComponent<Renderer>().material.color;
        lerpStep = 1 / warningTime;
        warningFade = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (warningFade)
        {
            WarnFade();
        }else if (falling)
        {
            Fall();
        }
    }

    private void WarnFade()
    {
        this.GetComponent<Renderer>().material.color = Color.Lerp(startColor, fullWarningColor, currentLerp);
        currentLerp += lerpStep*Time.deltaTime;
        if(currentLerp >= 1.0f)
        {
            this.GetComponent<Renderer>().material.color = fullWarningColor;
            warningFade = false;
            falling = true;
        }
    }

    private void Fall()
    {
        if(transform.localPosition.y > -fallingDistance)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - fallingSpeed * Time.deltaTime, transform.localPosition.z);
        }
        if(transform.localPosition.y <= -fallingDistance)
        {
            gameObject.SetActive(false);
        }
    }


}

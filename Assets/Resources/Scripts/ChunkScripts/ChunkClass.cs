using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkClass : MonoBehaviour
{
    enum ChunkType
    {
        CUBE,
        CUBE_MOVING,
        PLAIN,
        ROOM
    }

    public GameObject[] obstacleArray;
    public GameObject playerObject;
    private int cellID;
    private int chunkPosX;
    private int chunkPosY;
    private ChunkType chunkType;
    protected float chunkSizeX = 10.0f;
    protected float chunkSizeY = 10.0f;

    
    public void SetObstacleColours(Color newColour)
    {
        foreach(Transform child in this.transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.color = newColour;
            }
            if(child.childCount > 0)
            {
                SetObstacleColours(child, newColour);
            }
        }
    }
    public void SetObstacleColours(Transform objectToColor, Color newColour)
    {
        foreach (Transform child in objectToColor)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.color = newColour;
            }
            if (child.childCount > 0)
            {
                SetObstacleColours(child, newColour);
            }
        }
    }

    public virtual void SpawnObjects() {

        

    }
    public void RevealObstacles()
    {
        if (obstacleArray != null)
        {
            for (int i = 0; i < obstacleArray.Length; i++)
            {
                if (obstacleArray[i] != null)
                {

                    obstacleArray[i].GetComponent<ObstacleScript>().BeginReveal(playerObject);
                }
            }
        }
    }


}

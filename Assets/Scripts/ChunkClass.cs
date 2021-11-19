using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkClass : MonoBehaviour
{
    public GameObject[] obstacleArray;
    private int chunkPosX;
    private int chunkPosY;
    private float chunkSizeX = 10.0f;
    private float chunkSizeY = 10.0f;

    public void RevealObstacles()
    {
        for(int i = 0; i < obstacleArray.Length; i++)
        {
            obstacleArray[i].GetComponent<ObstacleScript>().BeginReveal();
        }
    }


}

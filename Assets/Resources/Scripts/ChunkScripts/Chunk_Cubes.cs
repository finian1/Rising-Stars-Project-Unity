using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_Cubes : ChunkClass
{
    private GameObject cubeObstacle;
    private ObstacleScript obstacleScript;
    private float cubeSize;
    private int numOfCubesX;
    private int numOfCubesY;
    // Start is called before the first frame update
    void Start()
    {
        cubeObstacle = Resources.Load("Prefabs/ObstaclePrefabs/CubeObstacle_Static", typeof(GameObject)) as GameObject;
        obstacleScript = cubeObstacle.GetComponent<ObstacleScript>();
        cubeSize = cubeObstacle.GetComponent<ObstacleScript>().obstacleSize;
        numOfCubesX = (int)(chunkSizeX / cubeSize);
        numOfCubesY = (int)(chunkSizeY / cubeSize);
    }

    public override void SpawnObjects()
    {
        //This isn't working just yet
        obstacleArray = new GameObject[(int)(chunkSizeX / cubeSize * chunkSizeY / cubeSize)];
        Vector3 startPoint = new Vector3(-chunkSizeX / 2, 0, -chunkSizeY / 2);
        
        for(int i = 0; i < obstacleArray.Length; i++)
        {
            int currIndex = i;
            int column = i % numOfCubesX;
            int row = 0;

            while (currIndex >= numOfCubesX)
            {
                currIndex -= numOfCubesX;
                row++;
            }

            obstacleArray[i] = Instantiate(cubeObstacle, this.transform);
            obstacleArray[i].transform.localPosition = new Vector3(startPoint.x + (column * cubeSize), obstacleScript.spawnHeightStart, startPoint.z + (row * cubeSize));
            
            obstacleScript.SetFinalHeight(Random.Range(0.0f, 5.0f));
        }


    }
}

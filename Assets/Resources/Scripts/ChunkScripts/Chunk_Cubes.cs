using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_Cubes : ChunkClass
{
    private GameObject cubeObstacle;
    private ObstacleScript obstacleScript;
    private float cubeSize = 2.5f;
    private int numOfCubesX;
    private int numOfCubesY;
    private float maxSpawnHeight = 1.0f;
    private float minSpawnHeight = -3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void SpawnObjects()
    {
        //This isn't working just yet
        cubeObstacle = Resources.Load("Prefabs/ObstaclePrefabs/CubeObstacle_Static", typeof(GameObject)) as GameObject;
        obstacleScript = cubeObstacle.GetComponent<ObstacleScript>();
        numOfCubesX = (int)(chunkSizeX / cubeSize);
        numOfCubesY = (int)(chunkSizeY / cubeSize);

        obstacleArray = new GameObject[(int)(chunkSizeX / cubeSize * chunkSizeY / cubeSize)];
        Vector3 startPoint = new Vector3(-chunkSizeX / 2, 0, -chunkSizeY / 2);
        /*GameObject debugObject = new GameObject();
        debugObject.transform.parent = this.transform;
        debugObject.transform.localPosition = startPoint;*/

        for (int i = 0; i < obstacleArray.Length; i++)
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
            obstacleArray[i].transform.localPosition = new Vector3(startPoint.x + column * cubeSize +cubeSize/2, obstacleScript.spawnHeightStart, startPoint.z + row * cubeSize + cubeSize/2);

            obstacleArray[i].GetComponent<ObstacleScript>().SetFinalHeight(Random.Range(minSpawnHeight, maxSpawnHeight));
        }


    }
}

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
    private float maxSpawnHeight = 3f;
    private float minSpawnHeight = -7f;
    private int smoothingPasses = 2;
    private float[] yArray;

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
        yArray = new float[obstacleArray.Length];
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
            for (int j = 0; j < obstacleArray.Length; j++)
            {
                yArray[j] = Random.Range(minSpawnHeight, maxSpawnHeight);
            }

            
        }

        for(int i = 0; i < smoothingPasses; i++)
        {
            Smooth(ref yArray);
        }
        for (int i = 0; i < obstacleArray.Length; i++)
        {
            obstacleArray[i].GetComponent<ObstacleScript>().SetFinalHeight(yArray[i]);
        }

    }

    private void Smooth(ref float[] array)
    {
        float[] prevYArray = new float[array.Length];
        float[] currentYArray = new float[array.Length];
        for(int i = 0; i < array.Length; i++)
        {
            prevYArray[i] = array[i];
            currentYArray[i] = 0;
        }
        Vector2Int[] _neighbours;
        _neighbours = new Vector2Int[8]
        {
            new Vector2Int(-numOfCubesX - 1, -1),
            new Vector2Int(-numOfCubesX, -1),
            new Vector2Int(-numOfCubesX + 1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(numOfCubesX - 1, 1),
            new Vector2Int(numOfCubesX, 1 ),
            new Vector2Int(numOfCubesX + 1, 1)
        };

        for (int index = 0; index < array.Length; index++)
        {
            int boxRow = index / numOfCubesX;
            int numOfSurroundingBlocks = 0;
            float currentSum = 0;
            for (int count = 0; count < _neighbours.Length; ++count)
            {
                int neighbourIndex = index + _neighbours[count].x;
                int expectedRow = boxRow + _neighbours[count].y;
                int neighbourRow = neighbourIndex / numOfCubesX;
                if (expectedRow == neighbourRow && neighbourIndex >= 0 && neighbourIndex < obstacleArray.Length)
                {
                    currentSum += array[neighbourIndex];
                    numOfSurroundingBlocks++;
                }
            }
            currentSum /= numOfSurroundingBlocks;
            array[index] = currentSum;
        }

    }
}

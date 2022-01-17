using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_Room : ChunkClass
{
    // Start is called before the first frame update
    private GameObject roomObject;
    private string[] prefabNames =
    {
        "Prefabs/ObstaclePrefabs/RoomPrefab",
        "Prefabs/ObstaclePrefabs/TreePrefab1",
        "Prefabs/ObstaclePrefabs/TreePrefab2"

    };
    ObstacleScript obstacleScript;
    float[] rotations =
    {
        0.0f, 90.0f, 180.0f, 270.0f
    };
    public override void SpawnObjects()
    {
        obstacleArray = new GameObject[1];
        roomObject = Resources.Load(prefabNames[Random.Range(0, prefabNames.Length)], typeof(GameObject)) as GameObject;
        obstacleScript = roomObject.GetComponent<ObstacleScript>();

        obstacleArray[0] = Instantiate(roomObject, this.transform);
        obstacleArray[0].transform.localPosition = new Vector3(0, obstacleScript.spawnHeightStart, 0);
        obstacleArray[0].transform.eulerAngles = new Vector3(0, rotations[Random.Range(0, rotations.Length)], 0);
    }
}

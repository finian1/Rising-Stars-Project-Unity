using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_Cubes : ChunkClass
{
    public GameObject cubeObstacle;
    private float cubeSize;
    // Start is called before the first frame update
    void Start()
    {
        cubeSize = cubeObstacle.GetComponent<ObstacleScript>().obstacleSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
